﻿using System;
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
using Edex.GeneralObjects.GeneralClasses;
using Edex.ModelSystem;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using Edex.Model.Language;
using DevExpress.XtraGrid;
using Edex.Model;
using Edex.DAL.Stc_itemDAL;
using Edex.StockObjects.StoresClasses;
using Edex.DAL.Accounting;
using DevExpress.XtraReports.UI;
using Edex.StockObjects.Codes;
using Edex.AccountsObjects.Codes;
using System.IO;
using Edex.DAL;
using System.Data.SqlClient;
using System.Data.OleDb;
using Edex.AccountsObjects.Transactions;
using System.Globalization;
using Edex.DAL.SalseSystem.Stc_itemDAL;
namespace Edex.StockObjects.Transactions
{
    public partial class frmMatirialInOnBail : BaseForm
    {     
        #region Declare
        //list detail
  
        DataTable dtDeclaration;
        public const int DocumentType = 17;
        int rowIndex;
        string Barcode = "";
        string columnName;
        string FocusedControl = "";
        private string strSQL;
        public CultureInfo culture = new CultureInfo("en-US");
        private string PrimaryName;
        private string ItemName;
        private string SizeName;
        private string GroupName;
        private string CaptionBarCode;
        private string CaptionGroupID;
        private string CaptionGroupName;
        private string CaptionItemID;
        private string CaptionItemName;
        private string CaptionSizeID;
        private string CaptionSizeName;
        private string CaptionPackingQty;
        private string CaptionExpiryDate;
        private string CaptionQTY;
        private string CaptionTotal;
        private string CaptionCostPrice;
        private string CaptionBones;
        private string CaptionSalePrice;
        private string CaptionDescription;

        private bool IsNewRecord;
        private Stc_MatirialInonBailDAL cClass;
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
        BindingList<Stc_MatirialInonBail_Details> AllRecords = new BindingList<Stc_MatirialInonBail_Details>();

        //list detail
        BindingList<Stc_MatirialInonBail_Details> lstDetail = new BindingList<Stc_MatirialInonBail_Details>();

        //Detail
        Stc_MatirialInonBail_Details BoDetail = new Stc_MatirialInonBail_Details();

        #endregion
        public frmMatirialInOnBail()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                InitializeComponent();
                ItemName = "ArbItemName";
                SizeName = "ArbSizeName";
                GroupName = "ArbGroupName";
                PrimaryName = "ArbName";
                CaptionBarCode = "الباركود";
                CaptionGroupID = "رقم المجموعة";
                CaptionGroupName = "إسم المجموعة";
                CaptionItemID = "رقم الصنف";
                CaptionItemName = "اسم الصنف";
                CaptionSizeID = "رقم الوحدة";
                CaptionSizeName = "الوحدة ";
                CaptionPackingQty = "التعبية";
                CaptionExpiryDate = "تاريخ الصلاحية";
                CaptionQTY = "الوزن";
                CaptionTotal = "الإجمالي";
                CaptionCostPrice = "سعر التكلفة";
                 CaptionBones = "الأجور";
                CaptionSalePrice = "سعر البيع";
                CaptionDescription = "البيان";
                strSQL = "ArbName";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, "Arb");
                if (UserInfo.Language == iLanguage.English)
                {
                    ItemName = "EngItemName";
                    SizeName = "EngSizeName";
                    GroupName = "EngGroupName";
                    PrimaryName = "EngName";
                    CaptionBarCode = "Bar Code";
                    CaptionGroupID = "Group ID";
                    CaptionGroupName = "Group Name";
                    CaptionBones = "Bones";
                    CaptionItemID = "Item ID";
                    CaptionItemName = "ItemName";
                    CaptionSizeID = "Size ID ";
                    CaptionSizeName = "Size Name";
                    CaptionPackingQty = "Packing Quantity";
                    CaptionExpiryDate = "Expiry Date";
                    CaptionQTY = "Quantity";
                    CaptionTotal = "Total";
                    CaptionCostPrice = "Cost Price";
                    CaptionSalePrice = "Cost Price";
                    CaptionDescription = "Description";
                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(PrimaryName, "Eng");

                }
                InitGrid();
                /*********************** Fill Data ComboBox  ****************************/
                FillCombo.FillComboBox(cmbCurency, "Acc_Currency", "ID", strSQL, "", " BranchID=" +  MySession.GlobalBranchID, (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                /***********************Component ReadOnly  ****************************/
                TextEdit[] txtEdit = new TextEdit[3];
                txtEdit[0] = lblStoreName;
                txtEdit[1] = lblStoreName;
                txtEdit[2] = lblCostCenterName;
                foreach (TextEdit item in txtEdit)
                {
                    item.ReadOnly = true;
                    item.Enabled = false;
                    item.Properties.AppearanceDisabled.ForeColor = Color.Black;
                    item.Properties.AppearanceDisabled.BackColor = Color.WhiteSmoke;
                }
                /*********************** Date Format dd/MM/yyyy ****************************/
                InitializeFormatDate(txtInvoiceDate);

                /************************  Form Printing ***************************************/
           
                /*********************** Roles From ****************************/



               
                txtInvoiceDate.ReadOnly = !MySession.GlobalAllowChangefrmMatirialInOnBailInvoiceDate;
                txtStoreID.ReadOnly = !MySession.GlobalAllowChangefrmMatirialInOnBailStoreID;
                txtCostCenterID.ReadOnly = !MySession.GlobalAllowChangefrmMatirialInOnBailCostCenterID;
                cmbCurency.ReadOnly = !MySession.GlobalAllowChangefrmMatirialInOnBailCurrncyID;
                txtSupplierID.ReadOnly = !MySession.GlobalAllowChangefrmMatirialInOnBailSupplier;
                
                /************TextEdit Account ID ***************/
             
                /************ Button Search Account ID ***************/
                RolesButtonSearchAccountID();
                /********************* Event For Account Component ****************************/

                this.btnDebitSearch.Click += new System.EventHandler(this.btnDebitSearch_Click);
                this.btnCrditSearch.Click += new System.EventHandler(this.btnCrditSearch_Click);

                /********************* Event For TextEdit Component **************************/
                if (MySession.GlobalAllowWhenEnterOpenPopup)
                {
                    this.txtInvoiceDate.Enter += new System.EventHandler(this.PublicTextEdit_Enter);
                    this.cmbCurency.Enter += new System.EventHandler(this.PublicCombox_Enter);
                }
                if (MySession.GlobalAllowWhenClickOpenPopup)
                {
                    this.txtInvoiceDate.Click += new System.EventHandler(this.PublicTextEdit_Click);
                    this.cmbCurency.Click += new System.EventHandler(this.PublicCombox_Click);
                }
                this.txtInvoiceID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtStoreID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtCostCenterID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtSupplierID.Validating += txtSupplierID_Validating;
                this.txtInvoiceID.Validating += new System.ComponentModel.CancelEventHandler(this.txtInvoiceID_Validating);
                this.txtStoreID.Validating += new System.ComponentModel.CancelEventHandler(this.txtStoreID_Validating);
                this.txtCostCenterID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCostCenterID_Validating);


                /***************************** Event For GridView *****************************/
                this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmGoodsOpeningInvoice_KeyDown);
                this.gridControl.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.gridControl_ProcessGridKey);
                this.gridView1.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gridView1_InitNewRow);
                this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
                this.gridView1.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.gridView1_FocusedColumnChanged);
                this.gridView1.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView1_CellValueChanging);
                this.gridView1.ShownEditor += new System.EventHandler(this.gridView1_ShownEditor);
                this.gridView1.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView1_ValidatingEditor);
                this.gridView1.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.gridView1_InvalidRowException);
                this.gridView1.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.gridView1_ValidateRow);
                this.gridView1.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gridView1_CustomUnboundColumnData);
                this.gridView1.PopupMenuShowing += gridView1_PopupMenuShowing;
                /******************************************/
                FillCombo.FillComboBox(cmbCurency, "Acc_Currency", "ID", strSQL, "", " BranchID=" +  MySession.GlobalBranchID, (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                 FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
                cmbBranchesID.EditValue =MySession.GlobalBranchID;
                cmbBranchesID.ReadOnly = !MySession.GlobalAllowBranchModificationAllScreens;
                cmbCurency.EditValue = Comon.cInt(MySession.GlobalDefaultSaleCurencyID);
                FillCombo.FillComboBoxLookUpEdit(cmbStatus, "Manu_TypeStatus", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select StatUs" : "حدد الحالة "));
                cmbStatus.EditValue = MySession.GlobalDefaultProcessPostedStatus;    

                DoNew();
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
            lstDetail = new BindingList<Stc_MatirialInonBail_Details>();
            lstDetail.AllowNew = true;
            lstDetail.AllowEdit = true;
            lstDetail.AllowRemove = true;
            gridControl.DataSource = lstDetail;


            gridView1.Columns["ID"].Visible = false;
            gridView1.Columns["ArbGroupName"].Visible = false;
            gridView1.Columns["EngGroupName"].Visible = false;
            gridView1.Columns["ArbItemName"].Visible = gridView1.Columns["ArbItemName"].Name == "col" + ItemName ? true : false;
            gridView1.Columns["EngItemName"].Visible = gridView1.Columns["EngItemName"].Name == "col" + ItemName ? true : false;
            gridView1.Columns["ArbSizeName"].Visible = gridView1.Columns["ArbSizeName"].Name == "col" + SizeName ? true : false;
            gridView1.Columns["EngSizeName"].Visible = gridView1.Columns["EngSizeName"].Name == "col" + SizeName ? true : false;
            gridView1.Columns["BarCode"].Visible = true;
            gridView1.Columns["ExpiryDate"].Visible = false;
            gridView1.Columns["ItemID"].Visible = false;

        
             
            gridView1.Columns["SalePrice"].Visible = true;
            gridView1.Columns["ExpiryDateStr"].Visible = false;
     
            gridView1.Columns["TheCount"].Visible = false;
            gridView1.Columns["Serials"].Visible = false;
            gridView1.Columns["BranchID"].Visible = false;
            gridView1.Columns["FacilityID"].Visible = false;
            gridView1.Columns["InvoiceID"].Visible = false;
           
            gridView1.Columns["Cancel"].Visible = false;
            gridView1.Columns["PageNo"].Visible = false;
            gridView1.Columns["ItemImage"].Visible = false;
            gridView1.Columns["BaseID"].Visible = false;
            gridView1.Columns["TypeID"].Visible = false;
            gridView1.Columns["IsService"].Visible = false;
            gridView1.Columns["MatirialInOnBailMaster"].Visible = false;
            //gridView1.Columns["Description"].Visible = false;
            gridView1.Columns["PackingQty"].Visible = false;
            gridView1.Columns["SizeID"].Visible = false;
            gridView1.Columns["GroupID"].Visible = false;

            gridView1.Columns["SalePrice"].Visible = false;

            gridView1.Columns["TotalCost"].Caption = CaptionTotal;
            gridView1.Columns["TotalCost"].OptionsColumn.AllowFocus = false;
            gridView1.Columns["TotalCost"].OptionsColumn.AllowEdit = true;
            /******************* Columns Visible=true *******************/

            
            gridView1.Columns["BarCode"].Caption = CaptionBarCode;
            gridView1.Columns["BarCode"].Width = 100;
            gridView1.Columns["GroupID"].Caption = CaptionGroupID;
            gridView1.Columns["GroupID"].Width = 10;
            gridView1.Columns[GroupName].Caption = CaptionGroupName;
            gridView1.Columns["ItemID"].Caption = CaptionItemID;
            gridView1.Columns[ItemName].Caption = CaptionItemName;
            gridView1.Columns[ItemName].Width = 200;
            gridView1.Columns["SizeID"].Caption = CaptionSizeID;
            gridView1.Columns[SizeName].Caption = CaptionSizeName;
            gridView1.Columns["PackingQty"].Caption = CaptionPackingQty;
            gridView1.Columns["ExpiryDate"].Caption = CaptionExpiryDate;
            gridView1.Columns["QTY"].Caption = CaptionQTY;
        
            gridView1.Columns["CostPrice"].Caption = CaptionCostPrice;
            gridView1.Columns["Bones"].Caption = CaptionBones;
            gridView1.Columns["SalePrice"].Caption = CaptionSalePrice;
            gridView1.Columns["Description"].Caption = CaptionDescription;
            gridView1.Columns["Bones"].Visible = false;
         

            gridView1.Focus();


            /*************************Columns Properties ****************************/
            gridView1.Columns[ItemName].OptionsColumn.ReadOnly = false;
            gridView1.Columns[ItemName].OptionsColumn.AllowFocus = true;
            gridView1.Columns[SizeName].OptionsColumn.ReadOnly = false;
            gridView1.Columns[SizeName].OptionsColumn.AllowFocus = true;
         

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
            /************************ Look Up Edit **************************/
            RepositoryItemLookUpEdit rGroupItem = Common.LookUpEditGroupItemID();
            gridView1.Columns[GroupName].ColumnEdit = rGroupItem;
            gridControl.RepositoryItems.Add(rGroupItem);

            RepositoryItemLookUpEdit rSize = Common.LookUpEditSize();
            gridView1.Columns[SizeName].ColumnEdit = rSize;
            gridControl.RepositoryItems.Add(rSize);

            DataTable dt = Lip.SelectRecord("SELECT ArbName FROM Stc_ItemsGroups WHERE Cancel=0 and AccountTypeID= " + 1 + " and BranchID=" + MySession.GlobalBranchID);
            string[] companiesGroupitems = new string[dt.Rows.Count];
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
                companiesGroupitems[i] = dt.Rows[i]["ArbName"].ToString();

            RepositoryItemComboBox riComboBoxGroupitems = new RepositoryItemComboBox();
            riComboBoxGroupitems.Items.AddRange(companiesGroupitems);
            gridControl.RepositoryItems.Add(riComboBoxGroupitems);
            gridView1.Columns[GroupName].ColumnEdit = riComboBoxGroupitems;
            gridView1.Columns[GroupName].Width = 120;

            gridView1.Columns[GroupName].VisibleIndex = 1;
            gridView1.Columns[GroupName].Visible = true;

          



            /************************ Auto Number **************************/
            DevExpress.XtraGrid.Columns.GridColumn col = gridView1.Columns.AddVisible("#");
            col.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            col.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            col.OptionsColumn.AllowEdit = false;
            col.OptionsColumn.ReadOnly = true;
            col.OptionsColumn.AllowFocus = false;
            col.Width = 20;
            gridView1.BestFitColumns();
            /******************************** Menu ***************************************/
            menu = new GridViewMenu(gridView1);
            menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("أسعار الصنف", new EventHandler(Price_Click)));
            menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("بيانات الصنف", new EventHandler(item_Click)));
            menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("كرت الصنف"));
            menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("باركود الصنف"));

        }
        private void Price_Click(object sender, EventArgs e)
        {


        }
        private void item_Click(object sender, EventArgs e)
        {


        }

        private void gridView1_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            if (e.HitInfo != null && e.HitInfo.Column.Name == "colCostPrice")
                if (e.HitInfo.HitTest == DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.RowCell)
                    e.Menu = menu;
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
                    if (col.FieldName == "QTY" || col.FieldName == "SizeID" )
                    {

                        var val = gridView1.GetRowCellValue(e.RowHandle, col);
                        double num;
                        if (val == null || string.IsNullOrWhiteSpace(val.ToString()))
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            gridView1.SetColumnError(col, Messages.msgInputIsRequired);
                        }
                        else if (!(double.TryParse(val.ToString(), out num)))
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            gridView1.SetColumnError(col, Messages.msgInputShouldBeNumber);
                        }
                        else if (Comon.ConvertToDecimalPrice(val.ToString()) <= 0)
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            gridView1.SetColumnError(col, Messages.msgInputIsGreaterThanZero);
                        }
                        else
                        {
                            e.Valid = true;
                            gridView1.SetColumnError(col, "");
                        }
                    }
                    else if (col.FieldName == SizeName)
                    {
                        var val = gridView1.GetRowCellValue(e.RowHandle, col);
                        if (val == null || string.IsNullOrWhiteSpace(val.ToString()))
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            gridView1.SetColumnError(col, Messages.msgInputIsRequired);
                        }
                    }
                    else if (col.FieldName == ItemName)
                    {
                        var val = gridView1.GetRowCellValue(e.RowHandle, col);
                        if (val == null || string.IsNullOrWhiteSpace(val.ToString()))
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            gridView1.SetColumnError(gridView1.Columns[col.FieldName], Messages.msgInputIsRequired);
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
            if (this.gridView1.ActiveEditor is CheckEdit)
            {
                if (e.Value != null)
                {
                   
                    CalculateRow(gridView1.FocusedRowHandle);
                }
            }
            else if (this.gridView1.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                HasColumnErrors = false;
                string ColName = view.FocusedColumn.FieldName;
                if (ColName == "SizeID" || ColName == "QTY" || ColName == "GroupID"  )
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
                    else
                    {
                        e.Valid = true;
                        view.SetColumnError(gridView1.Columns[ColName], "");

                    }
                    /****************************************/
                    if(ColName=="QTY")
                    {
                        decimal QTYTotal = Comon.cDec(val.ToString());
                        decimal CostPrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CostPrice").ToString());
                        decimal Total = Comon.ConvertToDecimalPrice(QTYTotal * CostPrice);
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "TotalCost", Total);
                    }
                    if (ColName == "ItemID")
                    {
                        DataTable dtItem = Stc_itemsDAL.GetTopItemDataByItemID(Comon.cInt(val.ToString()), UserInfo.FacilityID);
                        if (dtItem.Rows.Count == 0)
                        {
                            //e.Valid = false;
                            //HasColumnErrors = true;
                            //e.ErrorText = Messages.msgNoFoundThisBarCode;
                            //view.SetColumnError(gridView1.Columns[ColName], Messages.msgNoFoundThisBarCode);
                        }
                        else
                        {
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName], dtItem.Rows[0]["ArbName"].ToString());
                            if (UserInfo.Language == iLanguage.English)
                            {
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName], dtItem.Rows[0]["ItemName"].ToString());
                            }
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["StoreID"], txtStoreID.Text);
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["FacilityID"], UserInfo.FacilityID);
                        }

                    }
                    
                    else if (ColName == "SizeID")
                    {

                        int ItemID = Comon.cInt(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"]).ToString());
                        DataTable dt = Stc_itemsDAL.GetItemDataByItemID_SizeID(ItemID, Comon.cInt(val.ToString()), UserInfo.FacilityID);

                        if (dt.Rows.Count == 0)
                        {
                            DataTable dtSize = Lip.SelectRecord("SELECT SizeID, " + PrimaryName + " AS " + SizeName + " FROM Stc_SizingUnits Where Cancel=0 and BranchID=" +  MySession.GlobalBranchID+" And SizeID=" + Comon.cInt(val.ToString()) + " And FacilityID=" + UserInfo.FacilityID);
                            if (dtSize.Rows.Count == 0)
                            {
                                e.Valid = false;
                                HasColumnErrors = true;
                                e.ErrorText = Messages.msgNoFoundSizeForItem;
                                view.SetColumnError(gridView1.Columns[ColName], Messages.msgNoFoundSizeForItem);
                            }
                            else
                            {
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeID"], dtSize.Rows[0]["SizeID"].ToString());
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], dtSize.Rows[0][SizeName].ToString());
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["StoreID"], txtStoreID.Text);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["FacilityID"], UserInfo.FacilityID);

                            }
                        }
                        else
                        {
                            //RepositoryItemLookUpEdit rSize = Common.LookUpEditSize(Comon.cInt(dt.Rows[0]["ItemID"].ToString()));
                            //gridView1.Columns[SizeName].ColumnEdit = rSize;
                            //gridControl.RepositoryItems.Add(rSize);
                            FileItemData(dt);
                            e.Valid = true;
                            view.SetColumnError(gridView1.Columns[ColName], "");
                        }

                    }
                    else if (ColName == "GroupID")
                    {
                        DataTable dtItemGroup = Lip.SelectRecord("SELECT GroupID, " + PrimaryName + " AS " + GroupName + " FROM Stc_ItemsGroups Where Cancel=0 and BranchID=" +  MySession.GlobalBranchID+" And GroupID=" + Comon.cInt(val.ToString()) + " And FacilityID=" + UserInfo.FacilityID);
                        if (dtItemGroup.Rows.Count == 0)
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundSizeForItem;
                            view.SetColumnError(gridView1.Columns[ColName], Messages.msgNoFoundSizeForItem);
                        }
                        else
                        {
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["GroupID"], dtItemGroup.Rows[0]["GroupID"].ToString());
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[GroupName], dtItemGroup.Rows[0][GroupName].ToString());
                       
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["StoreID"], txtStoreID.Text);
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["FacilityID"], UserInfo.FacilityID);

                        }
                    }
                }
                if (ColName == "BarCode")
                {
                    DataTable dt = Stc_itemsDAL.GetItemData(val.ToString(), UserInfo.FacilityID);
                    if (dt.Rows.Count == 0)
                    {
                        //e.Valid = false;
                        //HasColumnErrors = true;
                        //e.ErrorText = Messages.msgNoFoundThisBarCode;
                        FileItemData(dt);
                    }
                    else
                    {
                        //RepositoryItemLookUpEdit rSize = Common.LookUpEditSize(Comon.cInt(dt.Rows[0]["ItemID"].ToString()));
                        //gridView1.Columns[SizeName].ColumnEdit = rSize;
                        //gridControl.RepositoryItems.Add(rSize);
                        FileItemData(dt);
                        e.Valid = true;
                        view.SetColumnError(gridView1.Columns[ColName], "");
                    }
                }

                else if (ColName == ItemName)
                {

                    DataTable dtItemID = Lip.SelectRecord("Select ItemID from Stc_Items Where Cancel=0 and BranchID=" +  MySession.GlobalBranchID+" And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                    if (dtItemID.Rows.Count > 0)
                    {
                        DataTable dtItem = Stc_itemsDAL.GetTopItemDataByItemID(Comon.cInt(dtItemID.Rows[0]["ItemID"].ToString()), UserInfo.FacilityID);
                        if (dtItem.Rows.Count == 0)
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisBarCode;
                            view.SetColumnError(gridView1.Columns[ColName], Messages.msgNoFoundThisItem);
                        }

                        else
                        {
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"], dtItem.Rows[0]["ItemID"].ToString());
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName], dtItem.Rows[0]["ArbName"].ToString());
                            if (UserInfo.Language == iLanguage.English)
                            {
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName], dtItem.Rows[0]["ItemName"].ToString());
                            }
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["StoreID"], txtStoreID.Text);
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["FacilityID"], UserInfo.FacilityID);

                        }
                    }
                    else
                    {
                        //e.Valid = false;
                        //HasColumnErrors = true;
                        //e.ErrorText = Messages.msgNoFoundThisItem;
                        //view.SetColumnError(gridView1.Columns[ColName], Messages.msgNoFoundThisItem);
                        //if (Find())
                        //{
                        //    return;
                        //}
                    }


                }

                else if (ColName == SizeName)
                {

                    DataTable dtSize = Lip.SelectRecord("Select SizeID, " + PrimaryName + " AS " + SizeName + " from Stc_SizingUnits Where Cancel=0 and BranchID=" +  MySession.GlobalBranchID+" And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                    if (dtSize.Rows.Count > 0)
                    {
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeID"], dtSize.Rows[0]["SizeID"].ToString());
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], dtSize.Rows[0][SizeName].ToString());
                        if (MySession.GlobalLanguageName == iLanguage.English)
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], dtSize.Rows[0]["SizeName"].ToString());
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["StoreID"], txtStoreID.Text);
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["FacilityID"], UserInfo.FacilityID);

                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundSizeForItem;
                        view.SetColumnError(gridView1.Columns[ColName], Messages.msgNoFoundSizeForItem);
                        if (Find())
                        {
                            return;
                        }
                    }


                }
                else if (ColName == GroupName)
                {
                    DataTable dtItemGroup = Lip.SelectRecord("Select GroupID, " + PrimaryName + " AS " + GroupName + " from Stc_ItemsGroups Where Cancel=0 and BranchID=" +  MySession.GlobalBranchID+" And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                    if (dtItemGroup.Rows.Count == 0)
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundSizeForItem;
                        view.SetColumnError(gridView1.Columns[ColName], Messages.msgNoFoundSizeForItem);
                    }
                    else
                    {

                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["GroupID"], dtItemGroup.Rows[0]["GroupID"].ToString());
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[GroupName], dtItemGroup.Rows[0][GroupName].ToString());
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName], dtItemGroup.Rows[0][GroupName].ToString());
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["StoreID"], txtStoreID.Text);
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["FacilityID"], UserInfo.FacilityID);
                    }
                }
                else  if (ColName == "CostPrice")
                {
                    decimal QTYTotal = Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "QTY").ToString());
                    decimal CostPrice = Comon.ConvertToDecimalPrice(val.ToString());
                    decimal Total = Comon.ConvertToDecimalPrice(QTYTotal * CostPrice);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "TotalCost", Total);
                }
                else if (ColName == "Discount")
                {
                    decimal QTY = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "QTY").ToString());
                    decimal CostPrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CostPrice").ToString());
                    decimal Total = QTY * CostPrice;
                    
                    if (!(double.TryParse(val.ToString(), out num)))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputShouldBeNumber;
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
                if (e.KeyValue == 107)
                {
                    if (this.gridView1.ActiveEditor is CheckEdit)
                    {
                        view.SetFocusedValue(!Comon.cbool(view.GetFocusedValue()));
                        CalculateRow(gridView1.FocusedRowHandle);
                    }
                }
                else if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Enter)
                {
                    if (view.ActiveEditor is TextEdit)
                    {

                        double num;
                        HasColumnErrors = false;
                        var cellValue = view.GetRowCellValue(view.FocusedRowHandle, view.FocusedColumn.FieldName);
                        string ColName = view.FocusedColumn.FieldName;
                        if (ColName == "QTY" || ColName == "SizeID"  )
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
        private void gridView1_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (this.gridView1.ActiveEditor is CheckEdit)
            {
                gridView1.Columns["HavVat"].OptionsColumn.AllowEdit = true;
                CalculateRow(gridView1.FocusedRowHandle);
            }

        }
        private void gridView1_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleInfo, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {

            try
            {


            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleInfo, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            rowIndex = e.RowHandle;
        }

        private void FileItemData(DataTable dt)
        {
           
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Stc_itemsDAL.CheckIfStopItemUnit(dt.Rows[0]["BarCode"].ToString(), MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                {
                    Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                    gridView1.DeleteRow(gridView1.FocusedRowHandle);
                    return;
                }
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"], dt.Rows[0]["BarCode"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"], dt.Rows[0]["ItemID"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName], dt.Rows[0]["ArbName"].ToString());
                if(UserInfo.Language==iLanguage.English)
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName], dt.Rows[0]["ItemName"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["GroupID"], dt.Rows[0]["GroupID"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[GroupName], dt.Rows[0][GroupName].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ExpiryDate"], Comon.ConvertSerialToDate(dt.Rows[0]["ExpiryDate"].ToString()));
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SalePrice"], dt.Rows[0]["SalePrice"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CostPrice"], dt.Rows[0]["CostPrice"].ToString());
                //Get  AverageCostPrice
                decimal AverageCost = frmItems.GetItemAverageCostPrice(Comon.cLong(dt.Rows[0]["ItemID"]), Comon.cInt(dt.Rows[0]["SizeID"]), Comon.cInt(txtStoreID.Text), 0, 0, 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CostPrice"], AverageCost);
                ///////////////////
                ///
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeID"], dt.Rows[0]["SizeID"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["StoreID"], txtStoreID.Text);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["TypeID"], dt.Rows[0]["TypeID"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BaseID"], dt.Rows[0]["BaseID"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["IsService"], dt.Rows[0]["IsService"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["PackingQty"], "1");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Bones"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Cancel"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Serials"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Description"], "");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["PageNo"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Caliber"], dt.Rows[0]["SizeName"].ToString());

                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["DIAMOND_W"], dt.Rows[0]["DIAMOND_W"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["STONE_W"], dt.Rows[0]["STONE_W"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BAGET_W"], dt.Rows[0]["BAGET_W"].ToString());


                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Equivalen"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["TheCount"], 1);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Height"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Width"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Total"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QTY"], 0);


            }
            else
            {
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["PackingQty"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["GroupID"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[GroupName], " ");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"], "0");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName], " ");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemName"], " ");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], " ");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeName"], " ");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ExpiryDate"], DateTime.Now.ToShortDateString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SalePrice"], "0");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CostPrice"], "0");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeID"], "0");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"], "0");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QTY"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["StoreID"], txtStoreID.Text);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Bones"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Description"], "");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["PageNo"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Caliber"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Equivalen"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["TheCount"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Bones"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Cancel"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Serials"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Height"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Width"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Total"], 0);

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
            if (IsNewRecord)
            {
                gridView1.Columns["PackingQty"].OptionsColumn.AllowEdit = Value;
                gridView1.Columns["PackingQty"].OptionsColumn.AllowFocus = Value;
                gridView1.Columns["PackingQty"].OptionsColumn.ReadOnly = !Value;
            }
            else
            {
                gridView1.Columns["PackingQty"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["PackingQty"].OptionsColumn.AllowFocus = false;
                gridView1.Columns["PackingQty"].OptionsColumn.ReadOnly = true;
            }
            foreach (GridColumn col in gridView1.Columns)
            {
                if (col.FieldName == "BarCode" || col.FieldName == "GroupID" || col.FieldName == GroupName || col.FieldName == "SalePrice" || col.FieldName == SizeName || col.FieldName == ItemName || col.FieldName == "BarCode" || col.FieldName == "Description" || col.FieldName == "ExpiryDate" || col.FieldName == "SizeID" || col.FieldName == "ItemID" )
                {
                    gridView1.Columns[col.FieldName].OptionsColumn.AllowEdit = Value;
                    gridView1.Columns[col.FieldName].OptionsColumn.AllowFocus = Value;
                    gridView1.Columns[col.FieldName].OptionsColumn.ReadOnly = !Value;
                }

            }
            if (Value)
                RolesButtonSearchAccountID();

           
        }
        private void txtSupplierID_Validating(object sender, CancelEventArgs e)
        {
           
            try
            {
                strSQL = "SELECT ArbName AS AccountName FROM Acc_Accounts WHERE   (Cancel = 0) AND (AccountID = " + txtSupplierID.Text + ") and BranchID=" + MySession.GlobalBranchID;
                CSearch.ControlValidating(txtSupplierID, lblSupplierName, strSQL);
            }
            catch (Exception ex)
            {
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
                    if (col.FieldName == "SizeID" /*|| col.FieldName == "PackingQty"*/  )
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
                        else if (Comon.cDbl(cellValue.ToString()) <= 0)
                        {
                            gridView1.SetColumnError(col, Messages.msgInputIsGreaterThanZero);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                    }
                    else if (col.FieldName == ItemName)
                    {
                        var cellValue = gridView1.GetRowCellValue(i, col);
                        if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                        {
                            gridView1.SetColumnError(gridView1.Columns[col.FieldName], Messages.msgInputIsRequired);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        #region Calculate
        private void CalculateRow(int Row = -1)
        {
            try
            {
                SumTotalBalanceAndDiscount(Row);


        }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
        }
    }
        public void SumTotalBalanceAndDiscount(int row = -1)
        {
            try
            {
                decimal Total = 0;

                decimal QTYRowEquivalen = 0;
                decimal QTYRow = 0;
            
                decimal CostPriceRow = 0;
                decimal TotalRow = 0;


                decimal QTYTotal = 0;
                decimal QTYTotalEquivalen = 0;
                decimal TotalBones = 0;
                
                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {

                    QTYRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "QTY").ToString()); 
                    int Caliber = Comon.cInt(gridView1.GetRowCellValue(i, SizeName).ToString());

                    QTYRowEquivalen = Comon.ConvertTo21Caliber(QTYRow, Caliber);
                    TotalBones += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Bones").ToString()); 
                    CostPriceRow  = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CostPrice").ToString());
                   

                    decimal Net = Comon.ConvertToDecimalPrice(CostPriceRow );     

                    TotalRow = Comon.ConvertToDecimalPrice(Net);
                    Total += (QTYRow * CostPriceRow);
                    QTYTotal += QTYRow;
                    QTYTotalEquivalen += QTYRowEquivalen;
                }
                if (rowIndex < 0)
                {
                    var ResultQTY = gridView1.GetRowCellValue(rowIndex, "QTY");
 
                    var ResultCostPrice = gridView1.GetRowCellValue(rowIndex, "CostPrice");
                    var TotalBonesrowin="";
                    if (  gridView1.GetRowCellValue(rowIndex, "Bones")!=null)
                      TotalBonesrowin= gridView1.GetRowCellValue(rowIndex, "Bones").ToString(); 
                    QTYRow = ResultQTY != null ? Comon.ConvertToDecimalPrice(ResultQTY.ToString()) : 0;

                    int Caliber = Comon.cInt(gridView1.GetRowCellValue(rowIndex, SizeName));
                    QTYRowEquivalen = Comon.ConvertTo21Caliber(QTYRow, Caliber);
                    CostPriceRow  = ResultCostPrice != null ? Comon.ConvertToDecimalPrice(ResultCostPrice.ToString()) : 0;
                    QTYTotal += QTYRow;
                    QTYTotalEquivalen += QTYRowEquivalen;
                    Total += (QTYRow * CostPriceRow);
                    TotalBones +=Comon.cDec( TotalBonesrowin);
                }             
                lblInvoiceTotalWeght.Text = Comon.ConvertToDecimalPrice(QTYTotal).ToString();
                lblNetBalance.Text = Comon.ConvertToDecimalPrice(Total) + "";
                lblTotalBones.Text = TotalBones + "";
                int isLocalCurrncy = Comon.cInt(Lip.GetValue("select TypeCurrency from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0 and BranchID=" + MySession.GlobalBranchID));
                if (isLocalCurrncy > 1)
                {
                    decimal CurrncyPrice = Comon.cDec(Lip.GetValue("select ExchangeRate from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0 and BranchID=" + MySession.GlobalBranchID));
                    lblCurrencyEqv.Text = Comon.cDec(Comon.cDec(Comon.cDec(lblNetBalance.Text) + Comon.cDec(lblTotalBones.Text)) * Comon.cDec(txtCurrncyPrice.Text)) + "";

                }
                else
                {
                    txtCurrncyPrice.Text = "1";
                    lblCurrencyEqv.Visible = false;
                    lblCurrncyPric.Visible = false;
                    lblcurrncyEquvilant.Visible = false;
                    txtCurrncyPrice.Visible = false;
                }
        }
                
            

            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
        }
    }

        #endregion
        #endregion
        #region Function
        #region Other Function
        private void AddRow()
        {
            try
            {
                if ((gridView1.IsNewItemRow(gridView1.FocusedRowHandle)))
                    gridView1.AddNewRow();
            }
            catch (Exception ex)
            {

            }

        }
        public bool Find()
        {

            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = " Where 1=1 ";

            FocusedControl = GetIndexFocusedControl();

            if (FocusedControl == null) return false;

            if (FocusedControl.Trim() == txtSupplierID.Name)
            {
                if (!MySession.GlobalAllowChangefrmMatirialInOnBailSupplier) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return false; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtSupplierID, lblSupplierName, "SublierID", "رقم المـــورد", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtSupplierID, lblSupplierName, "SublierID", "SublierID ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtStoreID.Name)
            {
                if (!MySession.GlobalAllowChangefrmMatirialInOnBailStoreID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return false; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "Account ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtInvoiceID.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return false; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtInvoiceID, null, "MatirialInOnBail", "رقـم الـفـاتـورة", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtInvoiceID, null, "MatirialInOnBail", "Invoice ID", MySession.GlobalBranchID);
            }

            else if (FocusedControl.Trim() == txtCostCenterID.Name)
            {
                if (!MySession.GlobalAllowChangefrmMatirialInOnBailCostCenterID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return false; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "رقم مركز التكلفة", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "Cost Center ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == gridControl.Name)
            {
                if (gridView1.FocusedColumn == null) return false;
                
                if (gridView1.FocusedColumn.Name == "colBarCode" || gridView1.FocusedColumn.Name == "colItemName" || gridView1.FocusedColumn.Name == "colItemID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "BarCode", MySession.GlobalBranchID);
                }
                if (gridView1.FocusedColumn.Name == "colSizeName" || gridView1.FocusedColumn.Name == "colSizeID")
                {
                    //var itemID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"]);
                    //var Barcode = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"]);
                    //if (itemID != null && Barcode != null)
                    //{

                    //   // Condition += " And ItemID=" + Comon.cInt(itemID);
                    //    if (UserInfo.Language == iLanguage.Arabic)
                    //        PrepareSearchQuery.Find(ref cls, null, null, "ItemBySize", "رقـم الـوحـــده", MySession.GlobalBranchID, Condition);
                    //    else
                    //        PrepareSearchQuery.Find(ref cls, null, null, "ItemBySize", "Size ID", MySession.GlobalBranchID, Condition);
                    //}
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "رقـم الـوحـــده", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "Size ID", MySession.GlobalBranchID, Condition);

                }
                else if (gridView1.FocusedColumn.Name == ("col" + GroupName) || gridView1.FocusedColumn.Name == "colGroupID")
                {

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "GroupID", "رقـم المجـمـوعة", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "GroupID", "Group ID", MySession.GlobalBranchID);
                }
                else if (gridView1.FocusedColumn.Name == "colQTY")
                {
                    frmRemindQtyItem frm = new frmRemindQtyItem();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);

                        frm.Show();
                        if (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ItemID") != null)
                            frm.SetValueToControl(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ItemID").ToString(), txtStoreID.Text.ToString());
                          else
                        {
                            Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "ارجاء اختيار صنف ومن  ثم اعادة عرض الكمية المتبقية" : "Please select an item and re-display the remaining quantity");
                            frm.Close();
                             
                        }
                    }
                    else
                        frm.Dispose();
                }
            }
            return GetSelectedSearchValue(cls);
        }
        public bool GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {

                if (FocusedControl == txtSupplierID.Name)
                {
                    txtSupplierID.Text = cls.PrimaryKeyValue.ToString();
                    txtSupplierID_Validating(null, null);
                }

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

                else if (FocusedControl == txtInvoiceID.Name)
                {
                    txtInvoiceID.Text = cls.PrimaryKeyValue.ToString();
                    txtInvoiceID_Validating(null, null);
                }

                else if (FocusedControl == gridControl.Name)
                {
                    if (gridView1.FocusedColumn.Name == "colBarCode")
                    {
                        Barcode = cls.PrimaryKeyValue.ToString();
                        gridView1.AddNewRow();
                        gridView1.SetRowCellValue(rowIndex, gridView1.Columns["BarCode"], Barcode);
                        FileItemData(Stc_itemsDAL.GetItemData(Barcode, UserInfo.FacilityID));
                        //double ItemID = Comon.cDbl(gridView1.GetRowCellValue(rowIndex, gridView1.Columns["ItemID"]));
                        //RepositoryItemLookUpEdit rSize = Common.LookUpEditSize(ItemID);
                        //gridView1.Columns[SizeName].ColumnEdit = rSize;
                        //gridControl.RepositoryItems.Add(rSize);
                        CalculateRow();
                        Find();
                    }
                    else if (gridView1.FocusedColumn.Name == ("col" + ItemName) || gridView1.FocusedColumn.Name == "colItemID")
                    {

                        AddRow();
                        Barcode = cls.PrimaryKeyValue.ToString();
                        DataTable dtItem = Stc_itemsDAL.GetItemData(Barcode, UserInfo.FacilityID);
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"], dtItem.Rows[0]["ItemID"].ToString());

                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName], dtItem.Rows[0]["ArbName"].ToString());
                        if (UserInfo.Language == iLanguage.English)
                        {
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName], dtItem.Rows[0]["ItemName"].ToString());
                        }
                      
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["FacilityID"], UserInfo.FacilityID);
                        CalculateRow();

                    }
                    else if (gridView1.FocusedColumn.Name == "colSizeName" || gridView1.FocusedColumn.Name == "colSizeID")
                    {
                        //int SizeID = Comon.cInt(cls.PrimaryKeyValue.ToString());
                        //var itemID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"]);
                        //var Barcode = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"]);
                        //if (itemID != null && Barcode != null)
                        //{

                        //    if (Stc_itemsDAL.CheckIfStopItemUnit(Comon.cInt(itemID), SizeID, MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                        //    {
                        //        Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                        //        return;
                        //    }
                        //    FileItemData(Stc_itemsDAL.GetItemDataByItemID_SizeID(Comon.cInt(itemID), SizeID, UserInfo.FacilityID));
                        //    CalculateRow();
                        //}
                        AddRow();
                        int SizeID = Comon.cInt(cls.PrimaryKeyValue.ToString());
                        DataTable dtSize = Lip.SelectRecord("SELECT SizeID, " + PrimaryName + " AS " + SizeName + " FROM Stc_SizingUnits Where Cancel=0 and BranchID=" +  MySession.GlobalBranchID+" And SizeID=" + SizeID);
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeID"], dtSize.Rows[0]["SizeID"].ToString());
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], dtSize.Rows[0][SizeName].ToString());
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["StoreID"], txtStoreID.Text);
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["FacilityID"], UserInfo.FacilityID);
                        CalculateRow();
                    }
                    else if (gridView1.FocusedColumn.Name == ("col" + GroupName) || gridView1.FocusedColumn.Name == "colGroupID")
                    {
                        AddRow();
                        int GroupID = Comon.cInt(cls.PrimaryKeyValue.ToString());
                        DataTable dtItemGroup = Lip.SelectRecord("SELECT GroupID, " + PrimaryName + " AS " + GroupName + " FROM Stc_ItemsGroups Where Cancel=0 and BranchID=" +  MySession.GlobalBranchID+" And GroupID=" + GroupID);
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["GroupID"], dtItemGroup.Rows[0]["GroupID"].ToString());
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[GroupName], dtItemGroup.Rows[0][GroupName].ToString());
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["StoreID"], txtStoreID.Text);
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["FacilityID"], UserInfo.FacilityID);
                    }

                }
                return true;
            }
            return false;

        }
        public void ReadRecord(long InvoiceID, bool flag = false)
        {
            try
            {

                ClearFields();
                {

                    dt = Stc_MatirialInonBailDAL.frmGetDataDetailByID(InvoiceID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        IsNewRecord = false;
                        //Validate
                        txtStoreID.Text = dt.Rows[0]["StoreIDMaster"].ToString();
                        txtStoreID_Validating(null, null);

                        txtCostCenterID.Text = dt.Rows[0]["CostCenterID"].ToString();
                        txtCostCenterID_Validating(null, null);

                       
                        txtSupplierID.Text = dt.Rows[0]["CreditAccount"].ToString();
                        txtSupplierID_Validating(null, null);
                     
 

                       

                        //Masterdata
                        txtInvoiceID.Text = dt.Rows[0]["InvoiceID"].ToString();
                        txtNotes.Text = dt.Rows[0]["Notes"].ToString();
                        txtDocumentID.Text = dt.Rows[0]["DocumentID"].ToString();

                        txtCurrncyPrice.Text = dt.Rows[0]["CurrencyPrice"].ToString();
                        lblCurrencyEqv.Text = dt.Rows[0]["CurrencyEquivalent"].ToString();
                        cmbCurency.EditValue = Comon.cInt(dt.Rows[0]["CurrencyID"].ToString());
                        cmbCurency_EditValueChanged(null, null);
                        //Date

                        if (Comon.ConvertSerialDateTo(dt.Rows[0]["InvoiceDate"].ToString()) == "")
                            txtInvoiceDate.Text = "";

                        else
                            // txtInvoiceDate.DateTime = Convert.ToDateTime(Comon.ConvertSerialDateTo(dt.Rows[0]["InvoiceDate"].ToString()));

                            txtInvoiceDate.DateTime = DateTime.ParseExact(Comon.ConvertSerialDateTo(dt.Rows[0]["InvoiceDate"].ToString()), "dd/MM/yyyy", culture);//CultureInfo.InvariantCulture);

                        //  txtInvoiceDate.Text = Comon.ConvertSerialDateTo(dt.Rows[0]["InvoiceDate"].ToString());
                        //GridVeiw
                        gridControl.DataSource = dt;
                        //  frmPrintItemSticker.dtBarcode = dt;
                        lstDetail.AllowNew = true;
                        lstDetail.AllowEdit = true;
                        lstDetail.AllowRemove = true;
                        CalculateRow();

                        Validations.DoReadRipon(this, ribbonControl1);
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
                try
                {
                    txtCostCenterID.Text = MySession.GlobalDefaultMatirialInOnBailCostCenterID.ToString();
                    txtCostCenterID_Validating(null, null);
                    txtStoreID.Text = MySession.GlobalDefaultMatirialInOnBailStoreAccountID.ToString();
                    txtStoreID_Validating(null, null);
                    txtDocumentID.Text = "";
                    txtNotes.Text = "";
                    txtInvoiceDate.EditValue = DateTime.Now;
                    txtSupplierID.Text = MySession.GlobalDefaultMatirialInOnBailSupplierID;
                    txtSupplierID_Validating(null, null);
                    cmbCurency.EditValue = Comon.cInt(MySession.GlobalDefaultMatirialInOnBailCurencyID);
                }
                catch { }

                lstDetail = new BindingList<Stc_MatirialInonBail_Details>();

                lstDetail.AllowNew = true;
                lstDetail.AllowEdit = true;
                lstDetail.AllowRemove = true;
                gridControl.DataSource = lstDetail;

                dt = new DataTable();
                Barcode = "";


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
                    strSQL = "SELECT TOP 1 * FROM " + Stc_MatirialInonBailDAL.TableName + " Where Cancel =0 and BranchID=" + MySession.GlobalBranchID;
                    switch (Direction)
                    {
                        case xMoveFirst:
                            {
                                strSQL = strSQL + " ORDER BY " + Stc_MatirialInonBailDAL.PremaryKey + " ASC";
                                break;
                            }

                        case xMoveNext:
                            {
                                strSQL = strSQL + " And " + Stc_MatirialInonBailDAL.PremaryKey + ">" + PremaryKeyValue + " ORDER BY " + Stc_MatirialInonBailDAL.PremaryKey + " asc";
                                break;
                            }

                        case xMovePrev:
                            {
                                strSQL = strSQL + " And " + Stc_MatirialInonBailDAL.PremaryKey + "<" + PremaryKeyValue + " ORDER BY " + Stc_MatirialInonBailDAL.PremaryKey + " desc";
                                break;
                            }

                        case xMoveLast:
                            {
                                strSQL = strSQL + " ORDER BY " + Stc_MatirialInonBailDAL.PremaryKey + " DESC";
                                break;
                            }
                    }
                    cClass = new Stc_MatirialInonBailDAL();

                    long InvoicIDTemp = Comon.cLong(txtInvoiceID.Text);
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
                txtInvoiceID.Text = Stc_MatirialInonBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID).ToString();
                ClearFields();
                EnabledControl(true);
                txtStoreID.Focus();

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
                MoveRec(Comon.cInt(txtInvoiceID.Text), xMoveNext);


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
                MoveRec(Comon.cInt(txtInvoiceID.Text), xMovePrev);
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
                txtInvoiceID.Enabled = true;
                txtInvoiceID.Focus();
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
            dtItem.Columns.Add("FacilityID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("ItemID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("SizeID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("Description", System.Type.GetType("System.String"));
            
            dtItem.Columns.Add("Cancel", System.Type.GetType("System.String"));
            dtItem.Columns.Add("BarCode", System.Type.GetType("System.String"));
            dtItem.Columns.Add(ItemName, System.Type.GetType("System.String"));
            dtItem.Columns.Add(SizeName, System.Type.GetType("System.String"));
            dtItem.Columns.Add("QTY", System.Type.GetType("System.Decimal"));
           // dtItem.Columns.Add("PackingQty", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("CostPrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("ExpiryDate", System.Type.GetType("System.String"));
            dtItem.Columns.Add("SalePrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("GroupID", System.Type.GetType("System.String"));
            dtItem.Columns.Add(GroupName, System.Type.GetType("System.String"));
            dtItem.Columns.Add("Bones", System.Type.GetType("System.String"));
            dtItem.Columns.Add("TypeID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("BaseID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("IsService", System.Type.GetType("System.String"));
            dtItem.Columns.Add("TotalCost", System.Type.GetType("System.Decimal"));
            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                dtItem.Rows.Add();
                dtItem.Rows[i]["ID"] = i;
                dtItem.Rows[i]["FacilityID"] = UserInfo.FacilityID;
                dtItem.Rows[i]["BarCode"] = gridView1.GetRowCellValue(i, "BarCode").ToString();
                dtItem.Rows[i]["GroupID"] = Comon.cInt(gridView1.GetRowCellValue(i, "GroupID").ToString());
                dtItem.Rows[i][GroupName] = gridView1.GetRowCellValue(i, GroupName).ToString();
                dtItem.Rows[i]["TypeID"] = Comon.cInt(gridView1.GetRowCellValue(i, "TypeID").ToString());
                dtItem.Rows[i]["IsService"] = Comon.cInt(gridView1.GetRowCellValue(i, "IsService").ToString());
                dtItem.Rows[i]["BaseID"] = Comon.cInt(gridView1.GetRowCellValue(i, "BaseID").ToString());
                dtItem.Rows[i]["ItemID"] = Comon.cInt(gridView1.GetRowCellValue(i, "ItemID").ToString());
                dtItem.Rows[i]["SizeID"] = Comon.cInt(gridView1.GetRowCellValue(i, "SizeID").ToString());
                dtItem.Rows[i][ItemName] = gridView1.GetRowCellValue(i, ItemName).ToString();
                dtItem.Rows[i][SizeName] = gridView1.GetRowCellValue(i, SizeName).ToString();
                dtItem.Rows[i]["QTY"] = Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(i, "QTY").ToString());
              //  dtItem.Rows[i]["PackingQty"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "PackingQty").ToString());
                dtItem.Rows[i]["SalePrice"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice").ToString());
                dtItem.Rows[i]["Description"] = gridView1.GetRowCellValue(i, "Description").ToString();
                dtItem.Rows[i]["ExpiryDate"] = gridView1.GetRowCellValue(i, "ExpiryDate");
                dtItem.Rows[i]["CostPrice"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CostPrice").ToString());
                dtItem.Rows[i]["TotalCost"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "TotalCost").ToString());            
                dtItem.Rows[i]["Bones"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Bones").ToString());
                dtItem.Rows[i]["Cancel"] = 0;

            }
            gridControl.DataSource = dtItem;
            EnabledControl(true);
            Validations.DoEditRipon(this, ribbonControl1);
        }
    

        private bool AddItems(int Rowindex, string BarCode)
        {
            try
            {
                string[] ArrValues = new string[10000];
                DataTable dtTest = new DataTable();
                Application.DoEvents();
                //'إضافة المواد
                cItems Item = new cItems();
                Application.DoEvents();
                Lip.NewFields();
                Lip.Table = "Stc_Items";
                Boolean IsNewItem = false;
                long ItemID = Comon.cInt(Lip.GetValue(" Select ItemID from Stc_ItemUnits  where BarCode='" + BarCode.Trim() + "' and BranchID=" + MySession.GlobalBranchID));

                double GroupID = Comon.cDbl(gridView1.GetRowCellValue(Rowindex, "GroupID").ToString());

                double ItemG = 0;

                if (ItemID == 0)
                {
                    ItemID = Item.GetNewID();
                    ItemG = Lip.GetNewID(GroupID);
                    IsNewItem = true;
                }
                else
                    ItemG = Comon.cDbl(Lip.GetValue(" Select ItemGroupID from Stc_Items Where ItemID=" + ItemID + " and BranchID=" + MySession.GlobalBranchID).ToString());

                Lip.AddNumericField("ItemID", ItemID.ToString());
                gridView1.SetRowCellValue(Rowindex, gridView1.Columns["ItemID"], ItemID.ToString());
                Lip.AddStringField("ArbName", gridView1.GetRowCellValue(Rowindex, ItemName).ToString());
                Lip.AddStringField("EngName", gridView1.GetRowCellValue(Rowindex, ItemName).ToString());
                Lip.AddNumericField("GroupID", GroupID.ToString());
                Lip.AddNumericField("ItemGroupID", ItemG.ToString());

                Lip.AddStringField("Notes", "");
                Lip.AddNumericField("TypeID", gridView1.GetRowCellValue(Rowindex, "TypeID").ToString());
                Lip.AddNumericField("UserID", UserInfo.ID);
                Lip.AddNumericField("RegDate", Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate())).ToString());
                Lip.AddNumericField("RegTime", Comon.cDbl(Lip.GetServerTimeSerial()).ToString());
                Lip.AddNumericField("EditUserID", UserInfo.ID);
                Lip.AddNumericField("EditDate", Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate())).ToString());
                Lip.AddNumericField("EditTime", Comon.cDbl(Lip.GetServerTimeSerial()).ToString());
                Lip.AddStringField("ComputerInfo", UserInfo.ComputerInfo);
                Lip.AddStringField("EditComputerInfo", UserInfo.ComputerInfo);
                Lip.AddNumericField("Cancel", 0);
                Lip.AddStringField("IsVat", "1");
                Lip.AddNumericField("ColorID", 0);
                Lip.AddNumericField("BrandID", 0);
                Lip.AddNumericField("BaseID", gridView1.GetRowCellValue(Rowindex, "BaseID").ToString());
                Lip.AddNumericField("BranchID", 0);

                Lip.AddNumericField("STONE_W", 0);
                Lip.AddNumericField("DIAMOND_W", 0);
                Lip.AddNumericField("BAGET_W", 0);
                Lip.AddNumericField("FacilityID", UserInfo.FacilityID.ToString());
                Lip.sCondition = " ItemID = " + ItemID;

                if (IsNewItem)
                    Lip.ExecuteInsert();
                else
                    Lip.ExecuteUpdate();

                //'إضافة وحدات المواد
                cItemsUnits ItemUnit = new cItemsUnits();

                strSQL = "delete from Stc_ItemUnits where BarCode='" + BarCode.Trim() + "'";
                Lip.ExecututeSQL(strSQL);

                Application.DoEvents();
                Lip.NewFields();
                Lip.Table = "Stc_ItemUnits";
                long SizeID = Comon.cLong(Lip.GetValue("Select Top 1 SizeID From Stc_SizingUnits Where ArbName='" + gridView1.GetRowCellValue(Rowindex, "ArbSizeName").ToString() + "' and BranchID=" + MySession.GlobalBranchID));
                Lip.AddNumericField("ItemID", ItemID.ToString());

                strSQL = "Select Notes From Stc_ItemsGroups where GroupID=" + GroupID;
                DataTable dtGroup = Lip.SelectRecord(strSQL);
                string GroupName = dtGroup.Rows[0]["Notes"].ToString();

                if (BarCode == string.Empty)
                    BarCode = GroupName + ItemG.ToString().PadLeft(4, '0');

                 
                Lip.AddNumericField("SizeID", SizeID.ToString());
                Lip.AddStringField("BarCode", BarCode);
                Lip.AddNumericField("PackingQty", 1);

                gridView1.SetRowCellValue(Rowindex, gridView1.Columns["BarCode"], BarCode.ToString());
                gridView1.SetRowCellValue(Rowindex, gridView1.Columns["SizeID"], SizeID.ToString());
                gridView1.SetRowCellValue(Rowindex, gridView1.Columns["ItemID"], ItemID.ToString());


                decimal CostPrice = Comon.cDec(gridView1.GetRowCellValue(Rowindex, "CostPrice").ToString());
           
                int Caliber = Comon.cInt(gridView1.GetRowCellValue(rowIndex, SizeName));

                Lip.AddNumericField("SalePrice", gridView1.GetRowCellValue(Rowindex, "SalePrice").ToString());
                Lip.AddNumericField("STONE_W", 0);
                Lip.AddNumericField("DIAMOND_W",0);
                Lip.AddNumericField("BAGET_W", 0);
                Lip.AddNumericField("MinLimitQty", gridView1.GetRowCellValue(Rowindex, "QTY").ToString());

                Lip.AddNumericField("MaxLimitQty", 0);
                Lip.AddNumericField("LastCostPrice", 0);
                Lip.AddNumericField("LastSalePrice", 0);
                Lip.AddNumericField("SpecialSalePrice", 0);
                Lip.AddNumericField("SpecialCostPrice", 0);
                Lip.AddNumericField("ItemProfit", 20);
                Lip.AddNumericField("AllowedPercentDiscount", 50);
                Lip.AddNumericField("UnitCancel", 0);
                Lip.AddNumericField("AverageCostPrice", 0);
                Lip.AddNumericField("BranchID", 0);
                Lip.AddNumericField("FacilityID", UserInfo.FacilityID.ToString());
                Lip.ExecuteInsert();


                strSQL = "delete from Sales_PurchaseInvoiceDetails where  InvoiceID= -1 And  BarCode='" + BarCode.Trim() + "' and BranchID=" + MySession.GlobalBranchID;
                Lip.ExecututeSQL(strSQL);


                {
                    Application.DoEvents();
                    Lip.NewFields();
                    Lip.Table = "Sales_PurchaseInvoiceDetails";
                    Lip.AddNumericField("InvoiceID", -1);
                    Lip.AddNumericField("BranchID", 0);
                    Lip.AddNumericField("FacilityID", UserInfo.FacilityID.ToString());
                    Lip.AddNumericField("ItemID", ItemID.ToString());
                    Lip.AddNumericField("SizeID", SizeID.ToString());
                    Lip.AddNumericField("QTY", gridView1.GetRowCellValue(Rowindex, "QTY").ToString());
                    Lip.AddNumericField("CostPrice", gridView1.GetRowCellValue(Rowindex, "CostPrice").ToString());
                    Lip.AddNumericField("Bones", gridView1.GetRowCellValue(Rowindex, "Bones").ToString());
                    Lip.AddNumericField("StoreID", 0);
                    Lip.AddNumericField("Discount", 0);
                    Lip.AddNumericField("ExpiryDate", 20201101);
                    Lip.AddNumericField("SalePrice", gridView1.GetRowCellValue(Rowindex, "SalePrice").ToString());
                    Lip.AddStringField("BarCode", BarCode);
                    Lip.AddStringField("Serials", "");
                    Lip.AddNumericField("Cancel", 0);
                    Lip.AddNumericField("ItemStatus", -1);
                    Lip.AddNumericField("AdditionalValue", 0);
                    Lip.AddNumericField("Caliber", Caliber);
                    Lip.AddNumericField("STONE_W",0);
                    Lip.AddNumericField("DIAMOND_W", 0);
                    Lip.AddNumericField("BAGET_W", 0);
                    Lip.AddStringField("Description", gridView1.GetRowCellValue(Rowindex, "Description").ToString());

                    Lip.AddStringField("CLARITY", "");
                    Lip.AddStringField("Color","");
                    Lip.ExecuteInsert();
                }
                return true;
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return false;
            }
        }
        protected override void DoSave()
        {
            try
            {

               

                if (!Validations.IsValidForm(this))
                    return;
                if (!Validations.IsValidFormCmb(cmbCurency))
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
                if (!Lip.CheckTheProcessesIsPosted("Stc_MatirialInonBail_Master", Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(cmbStatus.EditValue), Comon.cLong(txtInvoiceID.Text)))
                {
                    Messages.MsgWarning(Messages.TitleError, Messages.msgTheProcessIsNotUpdateBecuseIsPosted);
                    return;
                }
                //for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                //{
                //    String BarCode = gridView1.GetRowCellValue(i, "BarCode").ToString();

                //    if (AddItems(i, BarCode) == false)
                //    {
                //        long ItemID = Comon.cInt(Lip.GetValue(" Select ItemID from Stc_ItemUnits  where BarCode='" + BarCode.Trim() + "'"));
                //        Lip.ExecututeSQL("Delete from Stc_ItemUnits Where ItemID=" + ItemID);
                //        Lip.ExecututeSQL("Delete from Stc_Items Where ItemID=" + ItemID);
                //        Lip.ExecututeSQL("Delete from Sales_PurchaseInvoiceDetails Where ItemID=" + ItemID);
                //        Messages.MsgInfo("يرجى التاكد من بيانات الصنف ", BarCode);
                //        return;
                //    }
                //}
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
        private int SaveStockMoveing(int DocumentID)
        {
            Stc_ItemsMoviing objRecord = new Stc_ItemsMoviing();
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.DocumentTypeID = DocumentType;
            objRecord.MoveType = 1;
            objRecord.MoveID = 0;
            objRecord.TranseID = DocumentID;
            objRecord.Posted = Comon.cInt(cmbStatus.EditValue);
            Stc_ItemsMoviing returned;
            List<Stc_ItemsMoviing> listreturned = new List<Stc_ItemsMoviing>();
            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                returned = new Stc_ItemsMoviing();
                returned.ID = i + 1;
                returned.MoveDate = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();
                returned.MoveID = 0;
                returned.TranseID = DocumentID;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.DocumentTypeID = DocumentType;
                returned.MoveType = 1;
                returned.StoreID = Comon.cDbl(txtStoreID.Text.ToString());
                returned.AccountID = Comon.cDbl(txtSupplierID.Text);
                returned.BarCode = gridView1.GetRowCellValue(i, "BarCode").ToString();
                returned.ItemID = Comon.cInt(gridView1.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(gridView1.GetRowCellValue(i, "SizeID").ToString());
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID + " and BranchID=" + MySession.GlobalBranchID));
                returned.QTY = Comon.cDbl(gridView1.GetRowCellValue(i, "QTY").ToString());
                returned.InPrice = Comon.cDbl(Comon.cDbl(gridView1.GetRowCellValue(i, "CostPrice").ToString()) );
                //returned.Bones = Comon.cDbl(gridView1.GetRowCellValue(i, "Bones").ToString());
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
            gridView1.MoveLastVisible();
            CalculateRow();
            txtInvoiceDate_EditValueChanged(null, null);
            int InvoiceID = Comon.cInt(txtInvoiceID.Text);
            Stc_MatirialInonBail_Master objRecord = new Stc_MatirialInonBail_Master();

            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = UserInfo.FacilityID;

            objRecord.InvoiceID = InvoiceID;
            objRecord.InvoiceDate = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();

            objRecord.CurencyID = Comon.cInt(cmbCurency.EditValue);
            objRecord.CurrencyName = cmbCurency.Text.ToString();
            objRecord.CurrencyPrice = Comon.cDec(txtCurrncyPrice.Text);
            objRecord.CurrencyEquivalent = Comon.cDec(lblcurrncyEquvilant.Text);

            objRecord.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            objRecord.StoreID = Comon.cDbl(txtStoreID.Text);
            objRecord.OperationTypeName = (UserInfo.Language == iLanguage.English ? "In Invoice Gold" : "توريد مخزني -مواد خام ");
            txtNotes.Text = (txtNotes.Text.Trim() != "" ? txtNotes.Text.Trim() : (UserInfo.Language == iLanguage.English ? "In Invoice Gold" : "توريد مخزني- مواد خام"));
            objRecord.Notes = txtNotes.Text.Trim();
            objRecord.DocumentID = Comon.cInt(txtDocumentID.Text);
            objRecord.CreditAccount = Comon.cDbl(txtSupplierID.Text);
            //Account

            objRecord.Posted = Comon.cInt(cmbStatus.EditValue);
           
            objRecord.ItemImage = DefaultImage();
            objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);

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

            Stc_MatirialInonBail_Details returned;
            List<Stc_MatirialInonBail_Details> listreturned = new List<Stc_MatirialInonBail_Details>();

           
            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                returned = new Stc_MatirialInonBail_Details();
                returned.ID = i;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.BarCode = gridView1.GetRowCellValue(i, "BarCode").ToString();
                returned.GroupID = Comon.cDbl(gridView1.GetRowCellValue(i, "GroupID").ToString());
                returned.ArbItemName = gridView1.GetRowCellValue(i, ItemName).ToString();
                returned.EngItemName = gridView1.GetRowCellValue(i, ItemName).ToString();
                if (MySession.GlobalLanguageName == iLanguage.Arabic)
                    returned.EngItemName = Common.getWordEng(gridView1.GetRowCellValue(i, ItemName).ToString());
                else
                    returned.ArbItemName = Common.getWordArb(gridView1.GetRowCellValue(i, ItemName).ToString());

            
                returned.ItemID = Comon.cLong(gridView1.GetRowCellValue(i, "ItemID").ToString());
                returned.TypeID = Comon.cInt(gridView1.GetRowCellValue(i, "TypeID").ToString());
                returned.BaseID = Comon.cInt(gridView1.GetRowCellValue(i, "BaseID").ToString());
                returned.IsService = Comon.cInt(gridView1.GetRowCellValue(i, "IsService").ToString());
                returned.SizeID = Comon.cInt(gridView1.GetRowCellValue(i, "SizeID").ToString());
                // returned.PackingQty = Comon.cInt(gridView1.GetRowCellValue(i, "PackingQty").ToString());
                returned.QTY = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "QTY").ToString());
                returned.SalePrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice").ToString()); 
                returned.Description = gridView1.GetRowCellValue(i, "Description").ToString();
                returned.ExpiryDateStr = Comon.ConvertDateToSerial(gridView1.GetRowCellValue(i, "ExpiryDate").ToString());
                returned.TypeID = Comon.cInt(gridView1.GetRowCellValue(i, "TypeID").ToString());
                returned.IsService = Comon.cInt(gridView1.GetRowCellValue(i, "IsService").ToString());
                returned.Bones = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Bones").ToString());
                returned.CostPrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CostPrice").ToString());
                returned.TotalCost = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "TotalCost").ToString());
             
                returned.Cancel = 0;
                returned.Serials = "";
                if (  returned.SizeID <= 0)
                    continue;
               
                listreturned.Add(returned);              
            }
           
            if (listreturned.Count > 0)
            {
                objRecord.MatirialInOnBailDatails = listreturned;
                int Result = Stc_MatirialInonBailDAL.InsertUsingXML(objRecord, IsNewRecord);
                if (Comon.cInt(cmbStatus.EditValue) > 1)
                {
                    // حفظ الحركة المخزنية 
                    if (Comon.cInt(Result) > 0)
                    {
                        int MoveID = SaveStockMoveing(Comon.cInt(Result));
                        if (MoveID == 0)
                            Messages.MsgError(Messages.TitleInfo, "خطا في حفظ الحركة المخزنية");
                    }
                      //حفظ القيد الالي

                    if (Comon.cInt(Result) > 0)
                    {
                        //حفظ القيد الالي
                        long VoucherID = SaveVariousVoucherMachin(Comon.cInt(Result));
                        if (VoucherID == 0)
                            Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                        else
                            Lip.ExecututeSQL("Update " + Stc_MatirialInonBailDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Stc_MatirialInonBailDAL.PremaryKey + " = " + txtInvoiceID.Text + " and BranchID=" + MySession.GlobalBranchID);

                    }
                }
                SplashScreenManager.CloseForm(false);


                if (IsNewRecord == true)
                {
                    if (Result >0)
                    {
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);

                        DoNew();
                    }
                    else if (Result == 2627)
                    {
                        Messages.MsgWarning(Messages.TitleWorning, Messages.msgWorningSaveDuplicateBarcode);
                    }
                    else
                    {
                        Messages.MsgError(Messages.TitleError, Messages.msgErrorSave + " " + Result);
                    }

                }
                else
                {

                    if (Result > 0)
                    {
                        txtInvoiceID_Validating(null, null);
                        EnabledControl(false);
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);

                    }
                    else if (Result == 2627)
                    {
                        Messages.MsgWarning(Messages.TitleWorning, Messages.msgWorningSaveDuplicateBarcode);
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

        long SaveVariousVoucherMachin(int DocumentID)
        {
            int VoucherID = 0;
            long Result = 0;
            Acc_VariousVoucherMachinMaster objRecord = new Acc_VariousVoucherMachinMaster();
            objRecord.DocumentType = DocumentType;
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " And DocumentType=" + objRecord.DocumentType + " And BranchID=" + Comon.cInt(MySession.GlobalBranchID)));

            objRecord.VoucherID = VoucherID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = MySession.GlobalFacilityID;
            //Date
            objRecord.VoucherDate = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();
            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
            objRecord.CurrencyName = cmbCurency.Text.ToString();
            objRecord.CurrencyPrice = Comon.cDec(txtCurrncyPrice.Text);
            objRecord.CurrencyEquivalent = Comon.cDec(lblcurrncyEquvilant.Text);
            objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);
            // objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
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
            decimal QTYDiamond=0;
            decimal QTYMatirial=0;
            decimal PriceDiamondBons=0;
            decimal PriceDiamond=0;
            decimal PriceMatirialBons=0;
             decimal PriceMatirial=0;
            //Debit Matirial
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {

                if (Comon.cInt(gridView1.GetRowCellValue(i, "BaseID").ToString()) > 1 && Comon.cInt(gridView1.GetRowCellValue(i, "BaseID").ToString())<=3)
                {
                    if (Comon.cInt(gridView1.GetRowCellValue(i, "IsService")) == 1)
                    {
                        PriceDiamondBons += Comon.cDec(gridView1.GetRowCellValue(i, "Bones"));
                        QTYDiamond += Comon.cDec(gridView1.GetRowCellValue(i, "QTY"));
                    }
                    else
                      PriceDiamond += Comon.cDec(Comon.cDec(gridView1.GetRowCellValue(i, "CostPrice")) * Comon.cDec(gridView1.GetRowCellValue(i, "QTY")));
                     
                }
                else
                {
                    if (Comon.cInt(gridView1.GetRowCellValue(i, "IsService")) == 1)
                    {
                        PriceMatirialBons += Comon.cDec(gridView1.GetRowCellValue(i, "Bones"));
                        QTYMatirial += Comon.cDec(gridView1.GetRowCellValue(i, "QTY"));
                    }
                    else
                     PriceMatirial += Comon.cDec(Comon.cDec(gridView1.GetRowCellValue(i, "CostPrice")) * Comon.cDec(gridView1.GetRowCellValue(i, "QTY")));
                     
                }
            }
            if (QTYDiamond > 0)
            {
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 2;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(txtStoreID.Text);
                returned.VoucherID = VoucherID;
                returned.Debit = Comon.cDbl(PriceDiamondBons);
                returned.DebitDiamond = Comon.cDbl(QTYDiamond);
                returned.Declaration = txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
                returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
                returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Debit) * Comon.cDbl(returned.CurrencyPrice)); 
                listreturned.Add(returned);

                //Credit Diamond    
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 1;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cLong(txtSupplierID.Text);
                returned.VoucherID = VoucherID;
                returned.CreditDiamond = Comon.cDbl(QTYDiamond);
                returned.Credit = Comon.cDbl(PriceDiamondBons);
                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
                returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
                returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Credit) * Comon.cDbl(returned.CurrencyPrice)); 
                listreturned.Add(returned);
            }
            
            if (QTYMatirial > 0)
            {
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 2;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(txtStoreID.Text);
                returned.VoucherID = VoucherID;
                returned.Debit = Comon.cDbl(PriceMatirialBons);
                returned.DebitMatirial = Comon.cDbl(QTYMatirial);
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
                returned.AccountID = Comon.cLong(txtSupplierID.Text);
                returned.VoucherID = VoucherID;
                returned.CreditMatirial = Comon.cDbl(QTYMatirial);
                returned.Credit = Comon.cDbl(PriceMatirialBons);
                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
                returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
                returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Credit) * Comon.cDbl(returned.CurrencyPrice)); 
                listreturned.Add(returned);
            }
            if (PriceMatirial > 0 || PriceDiamond>0)
            {
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 2;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(txtStoreID.Text);
                returned.VoucherID = VoucherID;
                returned.Debit = Comon.cDbl(PriceMatirial + PriceDiamond);

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
                returned.AccountID = Comon.cLong(txtSupplierID.Text);
                returned.VoucherID = VoucherID;
                returned.Credit = Comon.cDbl(PriceMatirial + PriceDiamond);
                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
                returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
                returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Credit) * Comon.cDbl(returned.CurrencyPrice)); 
                listreturned.Add(returned);
            }

            //=
            if (listreturned.Count > 0)
            {
                objRecord.VariousVoucherDetails = listreturned;
                Result = VariousVoucherMachinDAL.InsertUsingXML(objRecord, IsNewRecord);
            }
            return Result;
        }
        int DeleteStockMoving(int DocumentID)
        {
            int Result = 0;
            Stc_ItemsMoviing objRecord = new Stc_ItemsMoviing();
            objRecord.DocumentTypeID = DocumentType;
            objRecord.TranseID = DocumentID;
            objRecord.BranchID = UserInfo.BRANCHID;
            objRecord.FacilityID = UserInfo.FacilityID;
            Result = Stc_ItemsMoviingDAL.Delete(objRecord);
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
                int TempID = Comon.cInt(txtInvoiceID.Text);
                //Check if The BarCode is have Opration 
                bool FlageChecKDelete = false;

                //for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                //{

                //    FlageChecKDelete = Lip.CheckTheItemIsHaveTransactionByBarCode(gridView1.GetRowCellValue(i, "BarCode").ToString(), "Stc_ItemsOutonBail_Details");
                //    if (FlageChecKDelete)
                //    {
                //        SplashScreenManager.CloseForm();
                //        Messages.MsgError("Error Delete ", "لا يمكن حذف الصنف بسبب وجود عمليات محاسبية علية");
                //        return;
                //    }
                //}

         
                Stc_MatirialInonBail_Master model = new Stc_MatirialInonBail_Master();
                model.InvoiceID = Comon.cInt(txtInvoiceID.Text);
                model.EditUserID = UserInfo.ID;
                model.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                model.FacilityID = UserInfo.FacilityID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());

                int Result = Stc_MatirialInonBailDAL.Delete(model);
                //حذف الحركة المخزنية 
                if (Comon.cInt(Result) > 0)
                {
                    int MoveID = DeleteStockMoving(Comon.cInt(Result));
                    if (MoveID == 0)
                        Messages.MsgError(Messages.TitleInfo, "خطا في حذف الحركة  المخزنية");
                }
                if (Comon.cInt(Result) > 0)
                {
                    //حذف القيد الالي

                    int VoucherID = DeleteVariousVoucherMachin(Comon.cInt(Result));
                    if (VoucherID == 0)
                        Messages.MsgError(Messages.TitleInfo, "خطا في حذف قيد العملية");

                }
                SplashScreenManager.CloseForm(false);
                if (Comon.cInt(Result) >= 0)
                {
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);
                    MoveRec(model.InvoiceID, xMovePrev);
                }
                else
                {
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgErrorSave);
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

        int DeleteVariousVoucherMachin(int DocumentID)
        {
            int VoucherID = 0;
            int Result = 0;
            Acc_VariousVoucherMachinMaster objRecord = new Acc_VariousVoucherMachinMaster();
            objRecord.DocumentType = DocumentType;
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " And DocumentType=" + objRecord.DocumentType + " And BranchID=" + Comon.cInt(MySession.GlobalBranchID)));

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

                bool IncludeHeader = true;
                string rptFromName = "rptGoodsOpeningInvoice";
                rptFromName += (UserInfo.Language == iLanguage.English ? "Eng" : "Arb");
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFromName + ".repx", true);

                /********************** Master *****************************/
                rptForm.RequestParameters = false;
                rptForm.Parameters["ReportName"].Value = "توريد مخزني -مواد خام ";
                rptForm.Parameters["SupplierName"].Value = lblSupplierName.Text;
                rptForm.Parameters["InvoiceID"].Value = txtInvoiceID.Text.Trim().ToString();
                rptForm.Parameters["StoreName"].Value = lblStoreName.Text.Trim().ToString();
                rptForm.Parameters["CostCenterName"].Value = lblCostCenterName.Text.Trim().ToString();
                rptForm.Parameters["InvoiceDate"].Value = txtInvoiceDate.Text.Trim().ToString();
                rptForm.Parameters["Notes"].Value = txtNotes.Text.Trim().ToString();
                /********Total*********/
                rptForm.Parameters["InvoiceTotal"].Value =lblInvoiceTotalWeght.Text;


                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                var dataTable = new dsReports.rptGoodsOpeningInvoiceDataTable();

                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();

                    row["#"] = i + 1;
                    row["BarCode"] = gridView1.GetRowCellValue(i, "BarCode").ToString();
                    row["ItemName"] = gridView1.GetRowCellValue(i, ItemName).ToString();
                    row["SizeName"] = gridView1.GetRowCellValue(i, SizeName).ToString();
                    row["QTY"] = gridView1.GetRowCellValue(i, "QTY").ToString();
                   
                    row["CostPrice"] = gridView1.GetRowCellValue(i, "CostPrice").ToString();
                    row["SalePrice"] = gridView1.GetRowCellValue(i, "SalePrice").ToString();
                    row["Description"] = gridView1.GetRowCellValue(i, "Description").ToString();
                    row["Total"] = Comon.cDec(Comon.cDec(row["QTY"]) * Comon.cDec(row["CostPrice"])).ToString();
                    row["ExpiryDate"] = Comon.ConvertSerialToDate(Comon.ConvertDateToSerial(gridView1.GetRowCellValue(i, "ExpiryDate").ToString()).ToString());
                    dataTable.Rows.Add(row);
                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = "rptGoodsOpeningInvoice";

                /******************** Report Binding ************************/

                XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                subreport.Visible = IncludeHeader;
                subreport.ReportSource = ReportComponent.CompanyHeader();

                rptForm.CreateDocument();

                SplashScreenManager.CloseForm(false);

                frmReportViewer frmRptViewer = new frmReportViewer();
                frmRptViewer.documentViewer1.DocumentSource = rptForm;

                frmRptViewer.ShowDialog();
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        #endregion
        #endregion
        #region Event
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

            else if (FocusedControl.Trim() == gridControl.Name)
            {

                if (gridView1.FocusedColumn.Name == "colGroupID" || gridView1.FocusedColumn.Name == "col" + GroupName)
                {
                    frmItemsGroups frm = new frmItemsGroups();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);
                        frm.Show();
                    }
                    else
                        frm.Dispose();
                }
                else if (gridView1.FocusedColumn.Name == "colSizeName" || gridView1.FocusedColumn.Name == "colSizeID")
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
        #region Validating
        private void txtRegistrationNo_Validated(object sender, EventArgs e)
        {
            if (FormView == true)
                ReadRecord(Comon.cLong(txtRegistrationNo.Text), true);

            else
            {
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                return;
            }
        }
        private void txtInvoiceID_Validating(object sender, CancelEventArgs e)
        {
            if (FormView == true)
                ReadRecord(Comon.cLong(txtInvoiceID.Text));
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
                strSQL = "SELECT ArbName AS AccountName FROM Acc_Accounts WHERE   (Cancel = 0) AND (AccountID = " + txtStoreID.Text + ") and BranchID=" + MySession.GlobalBranchID;
                CSearch.ControlValidating(txtStoreID, lblStoreName, strSQL);
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
                strSQL = "SELECT ArbName as CostCenterName FROM Acc_CostCenters WHERE CostCenterID =" + Comon.cInt(txtCostCenterID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtCostCenterID, lblCostCenterName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
       
      

      
        #endregion
        #region Search
        /***************************Event Search ***************************/
        private void btnDebitSearch_Click(object sender, EventArgs e)
        {
            try
            {
                PrepareSearchQuery.SearchForAccounts(txtStoreID, lblStoreName);
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void btnCrditSearch_Click(object sender, EventArgs e)
        {
            try
            {
                PrepareSearchQuery.SearchForAccounts(txtSupplierID, lblSupplierName);
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }



        #endregion
        /************************Event From **************************/
        private void frmGoodsOpeningInvoice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                Find();
            if (e.KeyCode == Keys.F2)
                ShortcutOpen();
        }
        /*******************Event CheckBoc***************************/
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

        #endregion
        #region Event Combox

        private void PublicCombox_Enter(object sender, EventArgs e)
        {
            (sender as LookUpEdit).ShowPopup();
        }
        private void PublicCombox_Click(object sender, EventArgs e)
        {
            (sender as LookUpEdit).ShowPopup();
        }



        #endregion
        #endregion
        #region InitializeComponent
        private void RolesButtonSearchAccountID()
        {
            btnDebitSearch.Enabled = MySession.GlobalAllowChangefrmMatirialInOnBailStoreID;
            btnCrditSearch.Enabled = MySession.GlobalAllowChangefrmMatirialInOnBailSupplier;
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




        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }

        public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }
        private byte[] DefaultImage()
        {
            string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            Path = Path + @"\Images\Default.png";
            System.Drawing.Image img = System.Drawing.Image.FromFile(Path);
            MemoryStream ms = new System.IO.MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();

        }


        private void SaveImage(byte[] data)
        {
            try
            {

                SqlConnection Con = new GlobalConnection().Conn;
                if (Con.State == ConnectionState.Closed)
                    Con.Open();

                SqlCommand sc;
                sc = new SqlCommand("Update  Sales_PurchaseInvoiceMaster Set InvoiceImage=@p Where InvoiceID=" + txtInvoiceID.Text + " And BranchID=" + Comon.cInt(MySession.GlobalBranchID), Con);
                sc.Parameters.AddWithValue("@p", data);
                sc.ExecuteNonQuery();

            }
            catch
            {

            }
        }


        private void picInvoiceImage_DoubleClick(object sender, EventArgs e)
        {

        }



        protected string getImageID()
        {
            Double days = 0;
            DateTime StartDate = new DateTime((DateTime.Now.Year), 01, 01);
            TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - StartDate.Ticks);
            System.Random RandNum = new System.Random();
            int MyRandomNumber = RandNum.Next(0, 99);
            days = ts.Days + 1;
            int intSecondOfDay = 0;
            string strReturn = "";
            strReturn = days.ToString().PadLeft(3, '0');
            strReturn = strReturn + MyRandomNumber.ToString().PadLeft(2, '0');
            intSecondOfDay = (DateTime.Now.Hour * 3600) + (DateTime.Now.Minute * 60) + DateTime.Now.Second;
            return strReturn + intSecondOfDay.ToString().PadLeft(5, '0');
        }



        #endregion
        private void frmGoodsOpening_Load(object sender, EventArgs e)
        {
        }
        private void btnPrintBarCode_Click(object sender, EventArgs e)
        {
            frmPrintItemSticker frm = new frmPrintItemSticker();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
                BindingSource bs = new BindingSource();
                bs.DataSource = gridControl.DataSource;
                //DataTable dd = new DataTable();
                //dd = dt;
                frm.Show();
                //frm.fillMAsterData(dd);

                frm.gridControl.DataSource = bs;
               // this.Close();
            }
            else
                frm.Dispose();
        }
        private void txtInvoiceDate_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtInvoiceDate.Text.Trim()))
                txtInvoiceDate.EditValue = DateTime.Now;
            if (Lip.CheckDateISAvilable(txtInvoiceDate.Text))
            {
                Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheDateGreaterCurrntDate);
                txtInvoiceDate.Text = Lip.GetServerDate();
                return;
            }
            //if (Comon.ConvertDateToSerial(txtInvoiceDate.Text) > Comon.cLong((Lip.GetServerDateSerial())))
            //    txtInvoiceDate.Text = Lip.GetServerDate();
        }
        public void DoSaveFromFinance()
        {
            DoSave();
        }
         
        private void simpleButton18_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog1 = new OpenFileDialog();
                OpenFileDialog1.Filter = "All Files|*.*";
                OpenFileDialog1.FileName = "";
                OpenFileDialog1.ShowDialog();
                if ((OpenFileDialog1.FileName != ""))
                {
                    txtExcelPath.Text = OpenFileDialog1.FileName;

                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
        }
        private void EmportItems()
        {
            DoNew();
            OleDbConnection oledbConn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + txtExcelPath.Text + ";Extended Properties=Excel 12.0");
            cItemsStores Store = new cItemsStores();
            bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, "تأكيد الاسنيراد  ؟");
            if (!Yes)
                return;

            Application.DoEvents();
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
            oledbConn.Open();

            OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet$]", oledbConn);

            OleDbDataAdapter oleda = new OleDbDataAdapter();
            oleda.SelectCommand = cmd;
            DataTable dt = new DataTable();
            oleda.Fill(dt);
            oledbConn.Close();

            if (dt.Rows.Count < 1)
                return;
            lstDetail = new BindingList<Stc_MatirialInonBail_Details>();
            lstDetail.AllowNew = true;
            lstDetail.AllowEdit = true;
            lstDetail.AllowRemove = true;
            gridControl.DataSource = lstDetail;

            Stc_MatirialInonBail_Details obj = new Stc_MatirialInonBail_Details();


            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                obj = new Stc_MatirialInonBail_Details();
                obj.ArbItemName = dt.Rows[i]["ITEM_NAME"].ToString();
                obj.EngItemName = dt.Rows[i]["ITEM_NAME"].ToString();
                obj.GroupID = Comon.cDbl(dt.Rows[i]["GroupID"].ToString()); 
                obj.CostPrice = Comon.ConvertToDecimalPrice(dt.Rows[i]["price"].ToString());
                obj.QTY = Comon.ConvertToDecimalPrice(dt.Rows[i]["GOLD_GRAM_W"].ToString());
               
                obj.Serials = dt.Rows[i]["ITEM_NO"].ToString();
                obj.BarCode = dt.Rows[i]["BarCode"].ToString();


                string Barcode = Lip.GetValue("select itemid from Sales_PurchaseInvoiceDetails Where Barcode='" + obj.BarCode + "' and BranchID=" + MySession.GlobalBranchID);
                if (Comon.cInt(Barcode) > 0)
                {
                    Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, "الصنف موجود مسبقا" + " هل تريد الاستمرار " + obj.BarCode);
                    if (Yes)
                        continue;
                    else
                    {
                        SplashScreenManager.CloseForm(false);
                        return;
                    }
                }


               
                decimal CostPrice = Comon.ConvertToDecimalPrice(obj.CostPrice.ToString());
                decimal Bones = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(obj.CostPrice.ToString()));

                 obj.ArbGroupName = Lip.GetValue("Select  ArbName from Stc_ItemsGroups Where GroupID=" + obj.GroupID + " and BranchID=" + MySession.GlobalBranchID);
                obj.EngGroupName = obj.ArbGroupName;

 


                lstDetail.Add(obj);

            }
            SumTotalBalanceAndDiscount();

            gridControl.DataSource = lstDetail;
            SplashScreenManager.CloseForm(false);
        }
        private void btnemport_Click(object sender, EventArgs e)
        {
        label1: if (txtExcelPath.Text == string.Empty)
            {
                Messages.MsgError(Messages.TitleConfirm, "يجب تحديد مسار ملف الأكسل");
                txtExcelPath.Focus();
                simpleButton18_Click(null, null);
                goto label1;
            }

            EmportItems();
            txtExcelPath.Text = "";
            Messages.MsgInfo(Messages.TitleConfirm, "تم الاستيراد بنجاح - يجب حفظ عملية الاستيراد");

        }

        private void btnMachinResraction_Click(object sender, EventArgs e)
        {
            if (IsNewRecord == true) return;
            int ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(MySession.GlobalBranchID) + " And DocumentID=" + txtInvoiceID.Text + " And DocumentType=" + DocumentType).ToString());
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


        public void Transaction()
        {


            strSQL = "Select * from " + Stc_MatirialInonBailDAL.TableName + " where Cancel=0 and BranchID=" + MySession.GlobalBranchID;
            DataTable dtSend = new DataTable();
            dtSend = Lip.SelectRecord(strSQL);
            if (dtSend.Rows.Count > 0)
            {
                for (int i = 0; i <= dtSend.Rows.Count - 1; i++)
                {
                    txtInvoiceID.Text = dtSend.Rows[i]["InvoiceID"].ToString();
                    cmbBranchesID.EditValue = Comon.cInt(dtSend.Rows[i]["BranchID"].ToString());
                    txtCostCenterID.Text = dtSend.Rows[i]["CostCenterID"].ToString();
                    txtStoreID.Text = dtSend.Rows[i]["StoreID"].ToString();
                    txtInvoiceID_Validating(null, null);
                    IsNewRecord = true;
                    if (Comon.cInt(txtInvoiceID.Text) > 0)
                    {
                        //حفظ القيد الالي
                        long VoucherID = SaveVariousVoucherMachin(Comon.cInt(txtInvoiceID.Text));
                        if (VoucherID == 0)
                            Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                        else
                            Lip.ExecututeSQL("Update " + Stc_MatirialInonBailDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Stc_MatirialInonBailDAL.PremaryKey + " = " + txtInvoiceID.Text + " AND BranchID=" + Comon.cInt(MySession.GlobalBranchID));

                    }



                }

                this.Close();
            }
        }

        private void cmbCurency_EditValueChanged(object sender, EventArgs e)
        {
            int isLocalCurrncy = Comon.cInt(Lip.GetValue("select TypeCurrency from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0  and BranchID=" + MySession.GlobalBranchID));
            if (isLocalCurrncy > 1)
            {
                decimal CurrncyPrice = Comon.cDec(Lip.GetValue("select ExchangeRate from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0 and BranchID=" + MySession.GlobalBranchID));
                txtCurrncyPrice.Text = CurrncyPrice + "";
                lblCurrencyEqv.Visible = true;
                lblCurrncyPric.Visible = true;
                lblcurrncyEquvilant.Visible = true;
                txtCurrncyPrice.Visible = true;
       
            }
            else
            {
                txtCurrncyPrice.Text = "1";
                lblCurrencyEqv.Visible = false;
                lblCurrncyPric.Visible = false;
                lblcurrncyEquvilant.Visible = false;
                txtCurrncyPrice.Visible = false;
                
            }
        }
    }
}
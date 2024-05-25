using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using Edex.DAL.SalseSystem;
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
using System.Text;
using System.Windows.Forms;

namespace Edex.StockObjects.Codes
{
    public partial class frmPrintItemSticker : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        #region Declare
        DataTable dtDeclaration;
        int rowIndex;
        static public DataTable dtBarcode;
        string FocusedControl = "";
        private string strSQL;
        private string PrimaryName;
        private string ItemName;
        private string SizeName;
        private string CaptionBarCode;
        private string CaptionItemID;
        private string CaptionItemName;
        private string CaptionSizeID;
        private string CaptionSizeName;
        private string CaptionExpiryDate;
        private string CaptionQTY;
        private string CaptionSalePrice;
        public short CopyNO = 1;
        private bool IsNewRecord;
        private Sales_SaleInvoicesDAL cClass;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        public bool HasColumnErrors = false;
        Boolean StopSomeCode = false;
        DataTable dt = new DataTable();
        //all record master and detail
        BindingList<Sales_SalesInvoiceDetails> AllRecords = new BindingList<Sales_SalesInvoiceDetails>();

        //list detail
        BindingList<Sales_SalesInvoiceDetails> lstDetail = new BindingList<Sales_SalesInvoiceDetails>();

        //Detail
        Sales_SalesInvoiceDetails BoDetail = new Sales_SalesInvoiceDetails();


        #endregion
        public frmPrintItemSticker()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

                InitializeComponent();
                ItemName = "ArbItemName";
                SizeName = "ArbSizeName";
                PrimaryName = "ArbName";
                CaptionBarCode = "الباركود";
                CaptionItemID = "رقم الصنف";
                CaptionItemName = "اسم الصنف";
                CaptionSizeID = "رقم الوحدة";
                CaptionSizeName = "العيار";
                CaptionExpiryDate = "تاريخ الصلاحية";
                CaptionQTY = "الوزن";


                CaptionSalePrice = "السعر";
                strSQL = "ArbName";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, "Arb");
                if (UserInfo.Language == iLanguage.English)
                {
                    ItemName = "EngItemName";
                    SizeName = "EngSizeName";
                    PrimaryName = "EngName";
                    CaptionBarCode = "Bar Code";
                    CaptionItemID = "Item ID";
                    CaptionItemName = "ItemName";
                    CaptionSizeID = "Size ID ";
                    CaptionSizeName = "Size Name";
                    CaptionExpiryDate = "Expiry Date";
                    CaptionQTY = "Quantity";
                    CaptionSalePrice = "Sale Price";


                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(PrimaryName, "Eng");

                }
                InitGrid();
                /***************************** Event For GridView *****************************/
                this.KeyPreview = true;
                this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPrintItemSticker_KeyDown);
                this.gridControl.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.gridControl_ProcessGridKey);
                this.gridView1.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gridView1_InitNewRow);
                this.gridView1.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView1_ValidatingEditor);
                this.gridView1.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.gridView1_InvalidRowException);
                this.gridView1.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gridView1_CustomUnboundColumnData);

                /******************************************/

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
            lstDetail = new BindingList<Sales_SalesInvoiceDetails>();
            lstDetail.AllowNew = true;
            lstDetail.AllowEdit = true;
            lstDetail.AllowRemove = true;
            gridControl.DataSource = lstDetail;

            /******************* Columns Visible=false ********************/


            gridView1.Columns["ItemImage"].Visible = false;
            gridView1.Columns["RemainQty"].Visible = false;
            gridView1.Columns["CaratPrice"].Visible = false;
            gridView1.Columns["PackingQty"].Visible = false;
            gridView1.Columns["GroupID"].Visible = true;
            gridView1.Columns["SizeID"].Visible = false;

            gridView1.Columns["ArbGroupName"].Visible = false;
            gridView1.Columns["EngGroupName"].Visible = false;
            gridView1.Columns["DateFirst"].Visible = false;
            gridView1.Columns["SpendPrice"].Visible = false;
            gridView1.Columns["ExpiryDateStr"].Visible = false;
            gridView1.Columns["DateFirstStr"].Visible = false;

            gridView1.Columns["BranchID"].Visible = false;
            gridView1.Columns["BAGET_W"].Visible = true;
            gridView1.Columns["STONE_W"].Visible = true;
            gridView1.Columns["DIAMOND_W"].Visible = true;
            gridView1.Columns["Equivalen"].Visible = false;
            gridView1.Columns["Caliber"].Visible = false;
            gridView1.Columns["CostPrice"].Visible = false;
            gridView1.Columns["ExpiryDateStr"].Visible = false;
            gridView1.Columns["Bones"].Visible = false;
            gridView1.Columns["Height"].Visible = false;
            gridView1.Columns["Width"].Visible = false;
            gridView1.Columns["TheCount"].Visible = false;
            gridView1.Columns["Serials"].Visible = true;
            gridView1.Columns["InvoiceID"].Visible = false;
            gridView1.Columns["ID"].Visible = false;
            gridView1.Columns["FacilityID"].Visible = false;
            gridView1.Columns["StoreID"].Visible = false;
            gridView1.Columns["Cancel"].Visible = false;
            gridView1.Columns["SaleMaster"].Visible = false;
            gridView1.Columns["ArbItemName"].Visible = gridView1.Columns["ArbItemName"].Name == "col" + ItemName ? true : false;
            gridView1.Columns["EngItemName"].Visible = gridView1.Columns["EngItemName"].Name == "col" + ItemName ? true : false;
            gridView1.Columns["ArbSizeName"].Visible = gridView1.Columns["ArbSizeName"].Name == "col" + SizeName ? true : false;
            gridView1.Columns["EngSizeName"].Visible = gridView1.Columns["EngSizeName"].Name == "col" + SizeName ? true : false;

            gridView1.Columns["ArbGroupName"].Visible = gridView1.Columns["ArbSizeName"].Name == "col" + SizeName ? true : false;
            gridView1.Columns["EngGroupName"].Visible = gridView1.Columns["EngSizeName"].Name == "col" + SizeName ? true : false;



            gridView1.Columns["Description"].Visible = false;
            gridView1.Columns["HavVat"].Visible = false;
            gridView1.Columns["Discount"].Visible = false;
            gridView1.Columns["AdditionalValue"].Visible = false;
            gridView1.Columns["Net"].Visible = false;
            gridView1.Columns["BarCode"].Visible = true;
            gridView1.Columns["ExpiryDate"].Visible = MySession.GlobalAllowUsingDateItems;
            gridView1.Columns["Total"].Visible = false;
            gridView1.Columns["GroupID"].Visible = false;
            /******************* Columns Visible=true *******************/

            gridView1.Columns[ItemName].Visible = true;
            gridView1.Columns[SizeName].Visible = true;

            gridView1.Columns["BarCode"].Caption = CaptionBarCode;
            gridView1.Columns["ItemID"].Caption = CaptionItemID;
            gridView1.Columns[ItemName].Caption = CaptionItemName;
            gridView1.Columns[ItemName].Width = 200;
            gridView1.Columns["SizeID"].Caption = CaptionSizeID;
            gridView1.Columns[SizeName].Caption = CaptionSizeName;
            gridView1.Columns["ExpiryDate"].Caption = CaptionExpiryDate;
            gridView1.Columns["QTY"].Caption = CaptionQTY;
            gridView1.Columns["SalePrice"].Caption = CaptionSalePrice;
            gridView1.Columns["ArbGroupName"].Caption = "اسم المجموعة";
            gridView1.Columns["BAGET_W"].Caption = "الباجيت";
            gridView1.Columns["STONE_W"].Caption = "الأحجار";
            gridView1.Columns["DIAMOND_W"].Caption = "الألماس";

            gridView1.Columns["GroupID"].Caption = "رقم المجموعة";

            gridView1.Columns["CLARITY"].Caption = "النقاء";
            gridView1.Columns["Color"].Caption = "اللون";
            gridView1.Columns["Serials"].Caption = "المرجع";



            /*************************Columns Properties ****************************/
            gridView1.Columns[ItemName].OptionsColumn.ReadOnly = true;
            gridView1.Columns[ItemName].OptionsColumn.AllowFocus = false;
            gridView1.Columns[SizeName].OptionsColumn.ReadOnly = true;
            gridView1.Columns[SizeName].OptionsColumn.AllowFocus = false;
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
            gridView1.Columns["ExpiryDate"].OptionsColumn.ReadOnly = true;
            /************************ Look Up Edit **************************/

            RepositoryItemLookUpEdit rItem = Common.LookUpEditItemName();
            gridView1.Columns[ItemName].ColumnEdit = rItem;
            gridControl.RepositoryItems.Add(rItem);

            RepositoryItemLookUpEdit rBarCode = Common.LookUpEditBarCode();
            gridView1.Columns["BarCode"].ColumnEdit = rBarCode;
            gridControl.RepositoryItems.Add(rBarCode);

            /************************ Auto Number **************************/
            DevExpress.XtraGrid.Columns.GridColumn col = gridView1.Columns.AddVisible("#");
            col.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            col.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            col.OptionsColumn.AllowEdit = false;
            col.OptionsColumn.ReadOnly = true;
            col.OptionsColumn.AllowFocus = false;
            col.Width = 20;
            gridView1.BestFitColumns();
            gridView1.Columns["ArbGroupName"].Width = 110;

           gridView1.Columns["ArbGroupName"].VisibleIndex = 4;
            txtCompanyName.Text = MySession.GlobalFacilityName;

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
                if (ColName == "BarCode" || ColName == "SizeID" || ColName == "ItemID" || ColName == "QTY" || ColName == "SalePrice")
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
                    if (ColName == "BarCode")
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

                else if (e.KeyData == Keys.Delete)
                {

                    //if (!IsNewRecord)
                    //{
                    //    if (!FormDelete)
                    //    {
                    //        Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToDeleteRecord);
                    //        return;
                    //    }
                    //    else
                    //    {
                    //        bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, Messages.msgConfirmDelete);
                    //        if (!Yes)
                    //            return;
                    //    }
                    //}

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
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {

                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"], dt.Rows[0]["BarCode"].ToString());
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"], dt.Rows[0]["ItemID"].ToString());
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName], dt.Rows[0]["ArbName"].ToString());
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeID"], dt.Rows[0]["SizeID"].ToString());
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], dt.Rows[0]["ArbSizeName"].ToString());
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["EngSizeName"], dt.Rows[0]["SizeName"].ToString());
                    if (UserInfo.Language == iLanguage.English)
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());
                    if (UserInfo.Language == iLanguage.English)
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ExpiryDate"], Comon.ConvertSerialToDate(dt.Rows[0]["ExpiryDate"].ToString()));
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SalePrice"], dt.Rows[0]["SalePrice"].ToString());
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CostPrice"], dt.Rows[0]["CostPrice"].ToString());
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["StoreID"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Bones"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Discount"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Bones"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AdditionalValue"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Cancel"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Serials"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Description"], "");
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["PageNo"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemStatus"], 1);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Caliber"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Equivalen"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Net"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["TheCount"], 1);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AdditionaAmmount"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Height"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Width"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Total"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QTY"], 1);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["HavVat"], true);

                }
                else
                {
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"], "0");
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName], " ");
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], " ");
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ExpiryDate"], DateTime.Now.ToShortDateString());
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SalePrice"], "0");
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CostPrice"], "0");
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeID"], "0");
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"], "0");
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["StoreID"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Bones"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Description"], "");
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["PageNo"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemStatus"], 1);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Caliber"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Equivalen"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Net"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["TheCount"], 1);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AdditionaAmmount"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Bones"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AdditionalValue"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Cancel"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Serials"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Height"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Width"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Total"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["HavVat"], true);

                }
            }
            catch { }
        }

        #endregion


        #region Function
        protected override void DoPrint()
        {
           
            try
            {
                gridView1.PostEditor();
                gridView1.MoveLast();
                if (chkSaleVat.Checked == true)
                {
                    DoPrint2();
                    return;
                }

                Application.DoEvents();
                ReportName = "rptPrintItemSticker";
                /******************** Report Body *************************/
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Arb1" : ReportName + "Arb1");
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);
                rptForm.PrintingSystem.StartPrint += new DevExpress.XtraPrinting.PrintDocumentEventHandler(PrintingSystem_StartPrint);
                /********************** Master *****************************/
                rptForm.RequestParameters = false;
                if (checkEdit4.Checked == true)
                    rptForm.Parameters["CompanyName"].Value = txtCompanyName.Text.Trim().ToString();
                     
                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                var dataTable = new dsReports.rptPrintItemStickerDataTable();
                if (Comon.cInt(txtfrom.Text) == 0 && Comon.cInt(txtTo.Text) == 0)

                {
                    txtfrom.Text = "1";
                    txtTo.Text = gridView1.DataRowCount.ToString();
                }

                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                   

                        if (i >= Comon.cInt(txtfrom.Text) - 1 && i < Comon.cInt(txtTo.Text))
                    {
                        dataTable.Rows.Clear();
                        var row = dataTable.NewRow();
                        row["BarCode"] = gridView1.GetRowCellValue(i, "BarCode").ToString();
                        row["ItemName"] = gridView1.GetRowCellValue(i, ItemName).ToString();
                       
                        row["SizeName"] =Comon.ConvertToDecimalQty( gridView1.GetRowCellValue(i, "STONE_W"));
                        row["QTY"] = Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(i, "QTY").ToString());
                        row["SalePrice"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice").ToString());
                        row["ExpiryDate"] = Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(i, "BAGET_W").ToString());
                        row["Total"] = Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(i, "DIAMOND_W"));

                     
                        dataTable.Rows.Add(row);
                        rptForm.DataSource = dataTable;
                        rptForm.DataMember = ReportName;
                        /******************** Report Binding ************************/
                        rptForm.ShowPrintStatusDialog = false;
                        rptForm.ShowPrintMarginsWarning = false;
                        rptForm.CreateDocument();
                        SplashScreenManager.CloseForm(false);
                        //ShowReportInReportViewer = true;
                        //if (ShowReportInReportViewer)
                        //{
                        //    frmReportViewer frmRptViewer = new frmReportViewer();
                        //    frmRptViewer.documentViewer1.DocumentSource = rptForm;
                        //    frmRptViewer.ShowDialog();
                        //}
                        //else
                        //{
                            bool IsSelectedPrinter = false;
                            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                            DataTable dt = ReportComponent.SelectRecord("SELECT *  from Printers where ReportName='" + ReportName + "'");
                            if (dt.Rows.Count > 0) for (int p = 1; p < 6; p++)
                                {
                                    string PrinterName = dt.Rows[0]["PrinterName" + p.ToString()].ToString().ToUpper();
                                    if (!string.IsNullOrEmpty(PrinterName))
                                    {
                                        rptForm.PrinterName = PrinterName;
                                        // rptForm.print
                                        rptForm.Print(PrinterName);

                                        IsSelectedPrinter = true;
                                    }
                                }
                            SplashScreenManager.CloseForm(false);
                            if (!IsSelectedPrinter)
                                Messages.MsgWarning(Messages.TitleWorning, Messages.msgThereIsNotPrinterSelected);
                        //}
                    }
                    
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

           
        }
        public  void DoPrint2()
        {

            try
            {
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                /******************** Report Body *************************/
               // string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                string rptFormName = "rptPrintItemStickerForSales";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);
                rptForm.PrintingSystem.StartPrint += new DevExpress.XtraPrinting.PrintDocumentEventHandler(PrintingSystem_StartPrint);
                /********************** Master *****************************/
                rptForm.RequestParameters = false;
                if (checkEdit4.Checked == true)
                    rptForm.Parameters["CompanyName"].Value = txtCompanyName.Text.Trim().ToString();

                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                var dataTable = new dsReports.rptPrintItemStickerDataTable();
                //if (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "BarCode") != null)
                //{
                //    var row = dataTable.NewRow();
                //    row["BarCode"] = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "BarCode").ToString();
                //    if (chkPrintItemName.Checked==true)
                //    row["ItemName"] = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, ItemName).ToString();


                //    row["SizeName"] = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, SizeName).ToString();
                //    row["QTY"] = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "QTY").ToString();
                //    if (chkPrintSalePrice.Checked == true)
                //    row["SalePrice"] = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SalePrice").ToString();
                //    if (chkPrintExpiryDate.Checked == true)
                //    row["ExpiryDate"] = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ExpiryDate").ToString().Substring(0,10);
                //    dataTable.Rows.Add(row);

                //}
                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {


                   
                    dataTable.Rows.Clear();

                    var row = dataTable.NewRow();
                    row["BarCode"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice").ToString())+Comon.ConvertToDecimalPrice( Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice").ToString()) / 100 * MySession.GlobalPercentVat);
                    row["ItemName"] = gridView1.GetRowCellValue(i, ItemName).ToString();
                    row["SizeName"] = gridView1.GetRowCellValue(i, SizeName).ToString();
                    row["QTY"] = gridView1.GetRowCellValue(i, "QTY").ToString();
                    row["ExpiryDate"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i,"SalePrice").ToString());
                    CopyNO = Comon.cShort(row["QTY"]);

                    if (i >= Comon.cInt(txtfrom) && i <= Comon.cInt(txtTo))
                        dataTable.Rows.Add(row);

                    rptForm.DataSource = dataTable;
                    rptForm.DataMember = ReportName;

                    /******************** Report Binding ************************/
                    rptForm.ShowPrintStatusDialog = false;
                    rptForm.ShowPrintMarginsWarning = false;
                    rptForm.CreateDocument();

                    SplashScreenManager.CloseForm(false);
                    ShowReportInReportViewer = false;
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
                        if (dt.Rows.Count > 0) for (int p = 1; p < 6; p++)
                            {
                                string PrinterName = dt.Rows[0]["PrinterName" + p.ToString()].ToString().ToUpper();
                                if (!string.IsNullOrEmpty(PrinterName))
                                {
                                    rptForm.PrinterName = PrinterName;
                                    // rptForm.print
                                    rptForm.Print(PrinterName);

                                    IsSelectedPrinter = true;
                                }
                            }
                        SplashScreenManager.CloseForm(false);
                        if (!IsSelectedPrinter)
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgThereIsNotPrinterSelected);
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        private void PrintingSystem_StartPrint(object sender, DevExpress.XtraPrinting.PrintDocumentEventArgs e)
        {
            e.PrintDocument.PrinterSettings.Copies = CopyNO;
        }
        //protected override void DoPrint()
        //{

        //    try
        //    {

        //        Application.DoEvents();
        //        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

        //        /******************** Report Body *************************/
        //        string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
        //        XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

        //        /********************** Master *****************************/
        //        rptForm.RequestParameters = false;

        //        rptForm.Parameters["CompanyName"].Value = txtCompanyName.Text.Trim().ToString();

        //        for (int i = 0; i < rptForm.Parameters.Count; i++)
        //            rptForm.Parameters[i].Visible = false;
        //        /********************** Details ****************************/
        //        var dataTable = new dsReports.rptPrintItemStickerDataTable();
        //        if (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "BarCode") != null)
        //        {
        //            var row = dataTable.NewRow();
        //            row["BarCode"] = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "BarCode").ToString();

        //            row["ItemName"] = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, ItemName).ToString();
        //            row["SizeName"] = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, SizeName).ToString();
        //            row["QTY"] = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "QTY").ToString();
        //            row["SalePrice"] = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SalePrice").ToString();
        //            row["ExpiryDate"] = Comon.ConvertSerialToDate(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ExpiryDate").ToString()).ToString("dd/MM/yyyy");
        //            dataTable.Rows.Add(row);

        //        }
        //        for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
        //        {
        //            var row = dataTable.NewRow();
        //            row["BarCode"] = gridView1.GetRowCellValue(i, "BarCode").ToString();
        //            row["ItemName"] = gridView1.GetRowCellValue(i, ItemName).ToString();
        //            row["SizeName"] = gridView1.GetRowCellValue(i, SizeName).ToString();
        //            row["QTY"] = gridView1.GetRowCellValue(i, "QTY").ToString();
        //            row["SalePrice"] = gridView1.GetRowCellValue(i, "SalePrice").ToString();

        //            row["ExpiryDate"] = Comon.ConvertSerialToDate(gridView1.GetRowCellValue(i, "ExpiryDate").ToString()).ToString("dd/MM/yyyy"); dataTable.Rows.Add(row);

        //        }
        //        rptForm.DataSource = dataTable;
        //        rptForm.DataMember = ReportName;

        //        /******************** Report Binding ************************/
        //        rptForm.ShowPrintStatusDialog = false;
        //        rptForm.ShowPrintMarginsWarning = false;
        //        rptForm.CreateDocument();

        //        SplashScreenManager.CloseForm(false);

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
        //            if (dt.Rows.Count > 0) for (int i = 1; i < 6; i++)
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
        protected override void DoSearch()
        {
            try
            {
                gridControl.Focus();
                Find();
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public void Find()
        {
            try{
            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = "Where 1=1";

            FocusedControl = GetIndexFocusedControl();

            if (FocusedControl == null) return;

            if (FocusedControl.Trim() == gridControl.Name)
            {
                if (gridView1.FocusedColumn == null) return;

                if (gridView1.FocusedColumn.Name == "colBarCode" || gridView1.FocusedColumn.Name == "colItemName" || gridView1.FocusedColumn.Name == "colItemID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "BarCode", MySession.GlobalBranchID);
                }
                if (gridView1.FocusedColumn.Name == "colSizeName" || gridView1.FocusedColumn.Name == "colSizeID")
                {
                    var itemID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"]);
                    var Barcode = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"]);
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
            catch { }
        }
        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {
                if (FocusedControl == gridControl.Name)
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
                        FileItemData(Stc_itemsDAL.GetItemData(Barcode, UserInfo.FacilityID));
                        Find();
                    }
                    else if (gridView1.FocusedColumn.Name == "colSizeName" || gridView1.FocusedColumn.Name == "colSizeID")
                    {

                        int SizeID = Comon.cInt(cls.PrimaryKeyValue.ToString());
                        var itemID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"]);
                        var Barcode = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"]);
                        if (itemID != null && Barcode != null)
                        {
                            if (Stc_itemsDAL.CheckIfStopItemUnit(Comon.cInt(itemID), SizeID, MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                            {
                                Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                                return;
                            }
                            FileItemData(Stc_itemsDAL.GetItemDataByItemID_SizeID(Comon.cInt(itemID), SizeID, UserInfo.FacilityID));
                        }

                    }
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
        /************************Event From **************************/
        private void frmPrintItemSticker_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                Find();
        }

        #endregion


        private void FileItemRow(DataRow dt)
        {
            try
            {
                if (dt != null)
                {

                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"], dt["BarCode"].ToString());
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"], dt["ItemID"].ToString());
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName], dt[ItemName].ToString());
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeID"], dt["SizeID"].ToString());
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], dt[SizeName].ToString());
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["GroupID"], dt["GroupID"].ToString());
                    //  gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[GroupName], dt[GroupName].ToString());

                    if (UserInfo.Language == iLanguage.English)
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], dt["SizeName"].ToString());
                    if (UserInfo.Language == iLanguage.English)
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], dt["SizeName"].ToString());
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ExpiryDate"], Comon.ConvertSerialToDate(dt["ExpiryDate"].ToString()));
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SalePrice"], dt["SalePrice"].ToString());
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CostPrice"], dt["CostPrice"].ToString());
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["StoreID"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Bones"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Discount"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Bones"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AdditionalValue"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Cancel"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Serials"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Description"], "");
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["PageNo"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemStatus"], 1);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Caliber"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Equivalen"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Net"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["TheCount"], 1);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AdditionaAmmount"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Height"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Width"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Total"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QTY"], dt["QTY"].ToString());
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["HavVat"], true);

                }
                else
                {
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"], "0");
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName], " ");
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], " ");
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ExpiryDate"], DateTime.Now.ToShortDateString());
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SalePrice"], "0");
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CostPrice"], "0");
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeID"], "0");
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"], "0");
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["StoreID"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Bones"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Description"], "");
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["PageNo"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemStatus"], 1);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Caliber"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Equivalen"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Net"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["TheCount"], 1);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AdditionaAmmount"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Bones"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AdditionalValue"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Cancel"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Serials"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Height"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Width"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Total"], 0);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["HavVat"], true);

                }
            
             }
            catch { }
        }
        private void frmPrintItemSticker_Load(object sender, EventArgs e)
        {
            try
            {
                ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Visible = false;
               

                string strSQL = "Select Top 1 * From StickersSettings Where BranchID=" + MySession.GlobalBranchID;
                DataTable dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                {
                    chkFontBold.Checked = Comon.cbool(dt.Rows[0]["FontBold"]);
                    chkPrintExpiryDate.Checked = Comon.cbool(dt.Rows[0]["PrintExpiryDate"]);
                    chkPrintItemName.Checked = Comon.cbool(dt.Rows[0]["PrintItemName"]);
                    chkPrintSalePrice.Checked = Comon.cbool(dt.Rows[0]["PrintSalePrice"]);
                    txtCompanyName.Text = MySession.GlobalFacilityName;
                    txtBarCodeHeight.EditValue = Comon.cInt(dt.Rows[0]["BarCodeHeight"]);
                    txtStartFrom.EditValue = Comon.cInt(dt.Rows[0]["StartFrom"]);
                    cmbFontSize.EditValue = Comon.cInt(dt.Rows[0]["FontSize"]);
                }

                if (dtBarcode != null && dtBarcode.Rows.Count > 0)
                {
                    foreach (DataRow item in dtBarcode.Rows)
                    {
                        gridView1.AddNewRow();
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"], item["Barcode"].ToString());
                        FileItemRow(item);
                    }

                }
                chkPrintExpiryDate.Checked = false;
                checkEdit4.Checked = true;
            }
            catch { }
        }
    }
}

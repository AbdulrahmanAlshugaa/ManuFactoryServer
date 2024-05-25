using DevExpress.XtraGrid.Menu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Edex.Model;
using Edex.DAL.Stc_itemDAL;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralForms;
using DevExpress.XtraSplashScreen;
using ITIN.ModelSystem;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using Edex.GeneralObjects.GeneralClasses;
using System.IO;
using System.Data.SqlClient;
using Edex.DAL;
using System.Globalization;
using System.Data.OleDb;
using Edex.StockObjects.StoresClasses;
using DevExpress.XtraReports.UI;
using Edex.SalesAndSaleObjects.Transactions;
using Edex.ModelSystem;
namespace Edex.StockObjects.Codes
{
    public partial class frmItems : Edex.GeneralObjects.GeneralForms.BaseForm
    {
      /**************This Is Variable Declare  **********************/
        #region Declare 
        int rowIndex;
         
        string FocusedControl = "";
        private string strSQL;
        private string PrimaryName;
        public CultureInfo culture = new CultureInfo("en-US");
        private string SizeName;
        private string CaptionBarCode;
        private string CaptionSizeID;
        private string CaptionSizeName;
        private string CaptionPackingQty;
        private string CaptionAllowedPercentDiscount;
        private string CaptionLastSalePrice;
        private string CaptionLastCostPrice;
        private string CaptionSpecialSalePrice;
        private string CaptionSpecialCostPrice;
        private string CaptionMaxLimitQty;
       
        private string CaptionUnitCancel;
        private string CaptionCostPrice;
        private string CaptionSalePrice;
        private string CaptionCLARITY;
        private string CaptionColor;
        private string CaptionItemProfit;
        private bool IsNewRecord;

        private Stc_itemsDAL cClass;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;

        public bool HasColumnErrors = false;
     
        OpenFileDialog OpenFileDialog1 = null;
        DataTable dt = new DataTable();
         
        //all record master and detail
        BindingList<Stc_ItemUnits> AllRecords = new BindingList<Stc_ItemUnits>();

        //list detail
        BindingList<Stc_ItemUnits> lstDetail = new BindingList<Stc_ItemUnits>();

        //Detail
        Stc_ItemUnits BoDetail = new Stc_ItemUnits();
        BindingList<Stc_ItemBarcode> lstDetailBarcode = new BindingList<Stc_ItemBarcode>();
        #endregion       
        public frmItems()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);            
                InitializeComponent();
                SizeName = "ArbSizeName";
                PrimaryName = "ArbName";
                CaptionBarCode = "باركود ";
                CaptionPackingQty = "التعبئة";
                CaptionAllowedPercentDiscount = "نسبة الخصم المسموح";
                CaptionSizeID = "رقم الوحدة";
                CaptionSizeName = "الوحدة ";
                CaptionLastSalePrice = "اخر سعر البيع";
                CaptionLastCostPrice = "اخر سعر التكلفة";
                CaptionSpecialSalePrice = "سعر بيع خاص";
                CaptionSpecialCostPrice = "سعر تكلفة خاص";
                CaptionMaxLimitQty = "الحد الأعلى للكمية"; 
                CaptionSalePrice = "سعر البيع";
                CaptionCostPrice = "سعر التكلفة";
                CaptionItemProfit = "الربح";
                CaptionUnitCancel = "ايقاف";
                CaptionColor = "اللون";
                CaptionCLARITY = "النقاء";

                strSQL = "ArbName";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, "Arb");
                if (UserInfo.Language == iLanguage.English)
                {
                    SizeName = "EngSizeName";
                    PrimaryName = "EngName";
                    CaptionBarCode = "BarCode";
                    CaptionPackingQty = "PackingQty";
                    CaptionAllowedPercentDiscount = "AllowedPercentDiscount";
                    CaptionSizeID = "SizeID";
                    CaptionSizeName = "SizeName";
                    CaptionLastSalePrice = "LastSalePrice";
                    CaptionLastCostPrice = "LastCostPrice";
                    CaptionSpecialSalePrice = "SpecialSalePrice";
                    CaptionSpecialCostPrice = "SpecialCostPrice";
                    CaptionMaxLimitQty = "MaxLimitQty";                    
                    CaptionUnitCancel = "UnitCancel";
                    CaptionSalePrice = "SalePrice";
                    CaptionCostPrice = "CostPrice";
                    CaptionItemProfit = "ItemProfit";
                    CaptionColor = "Color";
                    CaptionCLARITY = "CLARITY";
                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(PrimaryName, "Eng");


                    dvgColDate.Caption = "Date";
                    dvgColType.Caption = "Type ";
                    dvgColUnit.Caption = "Unit Size";
                    dvgColStorID.Caption = "Store ID";
                    dvgColQTY.Caption = "QTY";
                    dvgColCost.Caption = "Cost";
                    dvgColTotal.Caption = "Total";
                    dvgColOutQTY.Caption = "Out QTY";
                    dvgColOutPrice.Caption = "Out Price";
                    dvgColTotalOut.Caption = "Total Out"; 
                    dvgColBalance.Caption = "Balanc";
                    dvgColCostPrice.Caption = "Cost Price";
                    dvgColTotalBalance.Caption = "Total Balance";

                }
                this.txtItemID.Validating += new System.ComponentModel.CancelEventHandler(this.txtItemID_Validating);
                this.txtGroupID.Validating += new System.ComponentModel.CancelEventHandler(this.txtGroupID_Validating);
                this.txtTypeID.Validating += new System.ComponentModel.CancelEventHandler(this.txtTypeID_Validating);
                this.txtBaseID.Validating += new System.ComponentModel.CancelEventHandler(this.txtBaseID_Validating);
                this.txtArbName.EditValueChanged += new System.EventHandler(this.txtArbName_EditValueChanged);
                this.txtArbName.Validating += new System.ComponentModel.CancelEventHandler(this.txtArbName_Validating);
                this.txtEngName.Validating += new System.ComponentModel.CancelEventHandler(this.txtEngName_Validating);


                this.lnkAddImage.Click += new System.EventHandler(this.lnkAddImage_Click);
                ///***************************** Event For GridView *****************************/
                this.KeyPreview = true;
                this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmItems_KeyDown);
               
                 this.gridControl.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.gridControl_ProcessGridKey);
                 
                this.gridView1.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gridView1_InitNewRow);                 
                this.gridView1.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView1_CellValueChanging);
               
                this.gridView1.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView1_ValidatingEditor);
                 
                this.gridView1.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.gridView1_InvalidRowException);
                 
                this.gridView1.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.gridView1_ValidateRow);
                this.gridView1.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gridView1_CustomUnboundColumnData);

                FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
                cmbBranchesID.EditValue = UserInfo.BRANCHID;
                /******************************************/             
                InitGrid();
                InitGridBarcode();
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

        #region Events
       
        /// <summary>
        ///  Handle the InvalidRowException event for gridView1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            // Set the ExceptionMode property to NoAction
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            // This will prevent the exception from being thrown and will not perform any action.
        }
        /// <summary>
        ///  This method is called when a new row is initialized in gridView1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            // Store the index of the newly initialized row in the rowIndex variable
            rowIndex = e.RowHandle;
        }
        
        /// <summary>
        /// This Event To txtGroupID Validating and set Group Name To txtGroupName 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>    
        private void txtGroupID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string groupid = "0" + Comon.cDbl(txtGroupID.Text.ToString());
                strSQL = "SELECT " + PrimaryName + " as GroupName FROM Stc_ItemsGroups WHERE GroupID ='" + groupid + "' And Cancel = 0 and BranchID="+MySession.GlobalBranchID+" and AccountTypeID="+1;
                CSearch.ControlValidating(txtGroupID, txtGroupName, strSQL);
                long dtMaxBarcode = Comon.cLong(Lip.GetValue("SELECT Max(ItemID)+1 FROM Stc_Items WHERE GroupID=" + groupid + " And Cancel = 0 and BranchID=" + MySession.GlobalBranchID));

                    if (dtMaxBarcode == 0)
                        txtItemID.Text = groupid + (Comon.cLong("1").ToString()).PadLeft(3, '0');
                    else
                        txtItemID.Text = "0" + (Comon.cLong(dtMaxBarcode).ToString());
               
            }
            catch (Exception ex)
            {

                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }


        /// <summary>
        /// This Event To txtTypeID Validating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTypeID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as TypeName FROM Stc_ItemTypes WHERE TypeID =" + Comon.cInt(txtTypeID.Text) + " And Cancel =0  and BranchID=" + MySession.GlobalBranchID;
                CSearch.ControlValidating(txtTypeID, txtTypeName, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void txtBaseID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as BaseName FROM Stc_ItemsBases WHERE BaseID =" + Comon.cInt(txtBaseID.Text) + " And Cancel =0  "  ;
                CSearch.ControlValidating(txtBaseID,lblBaseName, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        private void txtArbName_EditValueChanged(object sender, EventArgs e)
        {
            // Set the border color of the control to black
            ((TextEdit)sender).Properties.Appearance.BorderColor = Color.Black;

        }

        /// <summary>
        /// This Event To Genrate The txtEngName when The TxtArbName is  Validating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtArbName_Validating(object sender, CancelEventArgs e)
        {
            // Check if it is a new record before continuing
            if (IsNewRecord == false) return;
            TextEdit obj = (TextEdit)sender;

            // Check the user's language and convert the Arabic name to the other language
            if (UserInfo.Language == iLanguage.Arabic)
                txtEngName.Text = Translator.ConvertNameToOtherLanguage(obj.Text.Trim().ToString(), UserInfo.Language);
        }

        /// <summary>
        /// This Event To Genrate TxtArbName The  when The  txtEngName is  Validating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtEngName_Validating(object sender, CancelEventArgs e)
        {
            // Check if it is a new record before continuing
            if (IsNewRecord == false) return;
            TextEdit obj = (TextEdit)sender;

            // Check the user's language and convert the English name to the other language
            if (UserInfo.Language == iLanguage.English)
                txtArbName.Text = Translator.ConvertNameToOtherLanguage(obj.Text.Trim().ToString(), UserInfo.Language);
        }  

        /// <summary>
        /// Event Show folder To Selcect Image Item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkAddImage_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog1 = new OpenFileDialog();
                OpenFileDialog1.Filter = "All Files|*.*|Bitmaps|*.bmp|GIFs|*.gif|JPEGs|*.jpg";
                OpenFileDialog1.FileName = "";
                OpenFileDialog1.ShowDialog();
                if ((OpenFileDialog1.FileName != ""))
                {

                    picItemImage.Image = Image.FromFile(OpenFileDialog1.FileName);
                    picItemImage.Visible = true;
                    byte[] Imagebyte = imageToByteArray(picItemImage.Image);
                    SaveImage(Imagebyte);

                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
        }

        /// <summary>
        /// This  Event Execute when Leave the txtNotes 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNotes_Leave(object sender, EventArgs e)
        {
            gridView1.SelectCell(0, gridView1.Columns["BarCode"]);
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];
            gridView1.VisibleColumns[1].Width = 120;
            // gridView1.SetFocusedRowCellValue()
        }

        /// <summary>
        /// This Event is Executed when the Click button import
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            AddItems();
        }
        
        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            try
            {
                if (!gridView1.IsLastVisibleRow)
                    gridView1.MoveLast();

                foreach (GridColumn col in gridView1.Columns)
                {

                    if (col.FieldName == "PackingQty" || col.FieldName == "SizeID"  )
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
                        else if (!HasColumnErrors)
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
                if (ColName == "PackingQty"  || ColName == "SizeID"  )
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
                }
                    /****************************************/
                    if (ColName == "BarCode")
                    {
                        strSQL = "SELECT InvoiceID FROM Sales_PurchaseInvoiceDetails WHERE BarCode ='" + val.ToString() + "' And InvoiceID  = -1 and BranchID=" + MySession.GlobalBranchID;
                        DataTable dt1 = Lip.SelectRecord(strSQL);
                        if (dt1.Rows.Count > 0)
                        {
                            e.Valid = false;
                            HasColumnErrors  = true;
                            e.ErrorText = " الباركود موجود مسبقا";
                        }
                        else
                        {
                            DataTable dt = Stc_itemsDAL.GetItemData(val.ToString(), UserInfo.FacilityID);
                            if (dt.Rows.Count == 0)
                            {

                            }
                            else
                                FileItemData(dt);

                        }
                    }


                //else if (ColName == "PackingQty")
                //{

                //    if (gridView1.DataRowCount > 0)
                //    {
                //        double MinPackingQty = double.MaxValue;
                //        double PackingQty = 0;
                //        int position = -1;
                //        string TypeOperation = "/";

                //        for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                //        {
                //            PackingQty = Comon.cDbl(gridView1.GetRowCellValue(i, "PackingQty").ToString());
                //            if (MinPackingQty > PackingQty)
                //            {
                //                MinPackingQty = PackingQty;
                //                position = i;

                //                if (Comon.cDbl(e.Value) < PackingQty)
                //                {
                //                    TypeOperation = "/";
                //                }

                //            }
                //            if (Comon.cDbl(e.Value) == PackingQty)
                //            {
                //                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CostPrice"], 0);
                //                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SalePrice"], 0);
                //                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["LastCostPrice"], 0);
                //                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["LastSalePrice"], 0);
                //                return;
                //            }
                //        }
                //        if (position > -1)
                //        {
                //            if (position == gridView1.FocusedRowHandle)
                //                return;

                //            var CostPrise = gridView1.GetRowCellValue(position, "CostPrice");
                //            if (CostPrise != null)
                //            {
                //                PackingQty = Comon.cDbl(val.ToString());
                //                decimal totalcost = Comon.ConvertToDecimalPrice(Comon.cDec(CostPrise) * Comon.cDec(PackingQty));
                //                decimal totalcost1 = Comon.ConvertToDecimalPrice(Comon.cDec(CostPrise) / Comon.cDec(PackingQty));

                //                if (TypeOperation == "*")
                //                {
                //                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CostPrice"], totalcost);
                //                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["LastCostPrice"], totalcost);
                //                }
                //                else
                //                {
                //                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CostPrice"], totalcost1);
                //                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["LastCostPrice"], totalcost1);
                //                }

                //            }
                //            var SalePrice = gridView1.GetRowCellValue(position, "SalePrice");
                //            if (SalePrice != null)
                //            {
                //                decimal totalsale = Comon.ConvertToDecimalPrice(Comon.cDec(SalePrice) * Comon.cDec(PackingQty));
                //                decimal totalsale1 = Comon.ConvertToDecimalPrice(Comon.cDec(SalePrice) / Comon.cDec(PackingQty));
                //                if (TypeOperation == "*")
                //                {
                //                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SalePrice"], totalsale);
                //                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["LastSalePrice"], totalsale);
                //                }
                //                else
                //                {
                //                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SalePrice"], totalsale1);
                //                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["LastSalePrice"], totalsale1);
                //                }
                //            }
                //        }
                //    }
                //}



                else if (ColName == "PackingQty")
                {
                    if (gridView1.DataRowCount > 0)
                    {
                        double MinPackingQty = double.MaxValue;
                        double PackingQty = 0;
                        int position = -1;
                        string TypeOperation = "/";

                        //for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                        //{
                        //    PackingQty = Comon.cDbl(gridView1.GetRowCellValue(i, "PackingQty").ToString());
                        //    if (MinPackingQty > PackingQty)
                        //    {
                        //        MinPackingQty = PackingQty;
                        //        position = i;

                        //        if (Comon.cDbl(e.Value) < PackingQty)
                        //        {
                        //            TypeOperation = "/";
                        //        }
                        //    }
                        //    if (Comon.cDbl(e.Value) == PackingQty)
                        //    {
                        //        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CostPrice"], 0);
                        //        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SalePrice"], 0);
                        //        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["LastCostPrice"], 0);
                        //        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["LastSalePrice"], 0);
                        //        return;
                        //    }
                        //}
                        if (gridView1.FocusedRowHandle < 0)
                            position = gridView1.DataRowCount - 1 ;
                        else
                            position = gridView1.FocusedRowHandle - 1;
                        if (position > -1)
                        {
                            if (position == gridView1.FocusedRowHandle)
                                return;

                            var CostPrise = gridView1.GetRowCellValue(position, "CostPrice");
                            if (CostPrise != null)
                            {
                                PackingQty = Comon.cDbl(e.Value);
                                decimal totalcost = (TypeOperation == "*") ? Comon.ConvertToDecimalPrice(Comon.cDec(CostPrise) * Comon.cDec(PackingQty)) : Comon.ConvertToDecimalPrice(Comon.cDec(CostPrise) / Comon.cDec(PackingQty));
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CostPrice"], totalcost);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["LastCostPrice"], totalcost);
                            }

                            var SalePrice = gridView1.GetRowCellValue(position, "SalePrice");
                            if (SalePrice != null)
                            {
                                decimal totalsale = (TypeOperation == "*") ? Comon.ConvertToDecimalPrice(Comon.cDec(SalePrice) * Comon.cDec(PackingQty)) : Comon.ConvertToDecimalPrice(Comon.cDec(SalePrice) / Comon.cDec(PackingQty));
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SalePrice"], totalsale);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["LastSalePrice"], totalsale);
                            }
                        }
                    }
                }




                else if (ColName == "SizeID")
                    {
                     
                        
                        DataTable dtSize = Lip.SelectRecord("SELECT SizeID, " + PrimaryName + " AS " + SizeName + " FROM Stc_SizingUnits Where Cancel=0 And SizeID=" + Comon.cInt(val.ToString()) + " and BranchID=" +MySession.GlobalBranchID+" And FacilityID=" + UserInfo.FacilityID);
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
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["FacilityID"], UserInfo.FacilityID);
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BranchID"], UserInfo.BRANCHID);

                        }
                    }

                    else if (ColName == SizeName)
                    {
                        DataTable dtSize = Lip.SelectRecord("SELECT SizeID  FROM Stc_SizingUnits Where Cancel=0 And "+PrimaryName+"='" + val.ToString() + "' and BranchID=" +MySession.GlobalBranchID +" And FacilityID=" + UserInfo.FacilityID);

                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeID"], dtSize.Rows[0]["SizeID"].ToString());
                        var Y = val.ToString();
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["FacilityID"], UserInfo.FacilityID);
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BranchID"], UserInfo.BRANCHID);
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
                        if (ColName == "PackingQty" || ColName == "SizeID"  )
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
                            if (ColName == "SalePrice")
                        {
                       //   gridView1.AddNewRow();
                            //gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                            //gridView1.FocusedColumn = gridView2.VisibleColumns[1];
                        
                        
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

        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            e.Value = (e.ListSourceRowIndex + 1);
        }
        private void gridView1_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
        }

        private void txtItemID_Validating(object sender, CancelEventArgs e)
        {
            // Check if this is a new record and there is more than one row in the grid view
            if (IsNewRecord && gridView1.RowCount > 1) return;

            // If FormView is true, read the record with the specified ItemID
            if (FormView == true)
                ReadRecord(Comon.cLong(txtItemID.Text));
            else
            {
                // If FormView is false, display an info message and return
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                return;
            }
        }

        private void frmItems_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the F3 key is pressed and call the Find() function if it is
            if (e.KeyCode == Keys.F3)
                Find();
            if (e.KeyCode == Keys.F2)
                ShortcutScreens();
            // Check if the F9 key is pressed and call the DoSave() function if it is
            if (e.KeyCode == Keys.F9)
                DoSave();

            // Check if the F12 key is pressed and call the btnAddBarcode_Click() function if it is
            if (e.KeyCode == Keys.F12)
                btnAddBarcode_Click(null, null);
        }
        /// <summary>
        /// This method is called when the txtNotes field is being validated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNotes_Validating(object sender, CancelEventArgs e)
        {
            // Show the btnImport button only if the txtNotes text is "ITMU"
            if (txtNotes.Text.Trim() == "ITMU")
                btnImport.Visible = true;
            else
                btnImport.Visible = false;

            // Show the btnRest button only if the txtNotes text is "RestIT"
            if (txtNotes.Text.Trim() == "RestIT")
                btnRest.Visible = true;
            else
                btnRest.Visible = false;

            /* 
            This method checks the text entered in the txtNotes field 
            and shows/hides the btnImport and btnRest buttons accordingly.
            */
        }


        /// <summary>
        /// This is the click event handler for the btnRest button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRest_Click(object sender, EventArgs e) 
        {
            try
            {
                bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, "سيتم حذف جميع بيانات المواد هل أنت متأكد من المتابعة ؟"); // Display a confirmation message asking the user to confirm the deletion of all data.
                if (!Yes)
                    return; // If the user didn't confirm, return and don't do anything.

                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true); // Show a Splash screen form during the backup process.
                frmBackupDataBase frm1 = new frmBackupDataBase(); // Create a new instance of the frmBackupDataBase form.
                frm1.DoClick(); // Call the DoClick method of the frmBackupDataBase form to start the backup process.
                Lip.ExecuteProcedure("Dell_ALLTransRest_SP");
                SplashScreenManager.CloseForm(false);
                this.Close();
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void btnAddBarcode_Click(object sender, EventArgs e)
        {

        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            // Cast the sender as a GridView
            GridView view = sender as GridView;

            try
            {
                // Check if IsNewRecord is true and return if it is
                if (IsNewRecord == true) return;

                // Check if the focused row handle is greater than or equal to 0
                if (e.FocusedRowHandle >= 0)
                {
                    // If it is, call the InitGridBarcode() function
                    InitGridBarcode();
                }
            }
            catch { }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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

                Save2Print();


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

        /// <summary>
        /// This Event is Execute to show frmEditSalePrice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmEditSalePrice frm = new frmEditSalePrice();
            frm.Show();
        }
        private void txtBrandID_Validated(object sender, EventArgs e)
        {
            try
            {   //This Stetment To Select Name when the validat txtBrandID
                strSQL = "SELECT " + PrimaryName + " as GroupName FROM Stc_ItemsBrands WHERE BrandID =" + Comon.cInt(txtBrandID.Text) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID;
                CSearch.ControlValidating(txtBrandID, lblBrandID, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        /// <summary>
        /// This Event is Execut to Show frmItemsBrands
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click_1(object sender, EventArgs e)
        {
            frmItemsBrands frm = new frmItemsBrands();
            frm.Show();
            //Set Variable
            frm.FormAdd = true;
            frm.FormView = true;
            frm.FormUpdate = true;
            frm.FormDelete = true;

        }
        #endregion
        
        #region Function
        #region Other Function
        /// <summary>
        /// This method adds a new row to the grid view.
        /// </summary>
        private void AddRow()
        {
            // Display an error message.
            Messages.MsgError(Messages.msgErrorSave, gridView1.IsNewItemRow(gridView1.FocusedRowHandle) + "");

            try
            {
                // If the focused row is a new item row, add a new row to the grid view.
                if ((gridView1.IsNewItemRow(gridView1.FocusedRowHandle)))
                    gridView1.AddNewRow();
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur.
            }
        }

        void ShortcutScreens()
        {
            FocusedControl = GetIndexFocusedControl();

            if (FocusedControl.Trim() == txtGroupID.Name)
            {
                frmItemsGroups frm = new frmItemsGroups();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                }
                else
                    frm.Dispose();
               
                 
            }
            else  if (FocusedControl.Trim() == txtTypeID.Name)
            {
                frmItemType frm = new frmItemType();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                }
                else
                    frm.Dispose();
               
            }
            else  if (gridView1.FocusedColumn.Name == "col" + SizeName || gridView1.FocusedColumn.Name == "colSizeID")
            {
                frmSizingUnits frm = new frmSizingUnits();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                }
                else
                    frm.Dispose();
               
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

            if (FocusedControl.Trim() == txtItemID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtItemID, null, "Items", "رقـم الـمــادة", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtItemID, null, "Items", "Item ID", MySession.GlobalBranchID);
            }
            
            else if (FocusedControl.Trim() == txtTypeID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtTypeID, null, "TypeID", "الــــنـــــــــــوع", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtTypeID, null, "TypeID", "Type ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtBaseID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtBaseID, null, "BaseID", "التصـــنيف", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtBaseID, null, "BaseID", "Base ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtGroupID.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return false; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtGroupID, null, "GroupID", "رقـم المجـمـوعة", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtGroupID, null, "GroupID", "Group ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtBrandID.Name)
            {
                

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtBrandID, null, "BrandID", "رقـم المجـمـوعة", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtBrandID, null, "BrandID", "Group ID", MySession.GlobalBranchID);
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
                if (gridView1.FocusedColumn.Name == "col"+SizeName || gridView1.FocusedColumn.Name == "colSizeID")
                {

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "رقـم الـوحـــده", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "Size ID", MySession.GlobalBranchID, Condition);

                }

            }

            

            return GetSelectedSearchValue(cls);
        }
        public bool GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {
                if (FocusedControl == txtTypeID.Name)
                {
                    txtTypeID.Text = cls.PrimaryKeyValue.ToString();
                    txtTypeID_Validating(null, null);
                }
                if (FocusedControl == txtBaseID.Name)
                {
                    txtBaseID.Text = cls.PrimaryKeyValue.ToString();
                    txtBaseID_Validating(null, null);
                }
                else if (FocusedControl == txtItemID.Name)
                {
                    txtItemID.Text = cls.PrimaryKeyValue.ToString();
                    txtItemID_Validating(null, null);
                }
                 
                else if (FocusedControl == txtGroupID.Name)
                {
                    txtGroupID.Text = cls.PrimaryKeyValue.ToString();
                    txtGroupID_Validating(null, null);
                }
                else if (FocusedControl == txtBrandID.Name)
                {
                    txtBrandID.Text = cls.PrimaryKeyValue.ToString();
                    txtBrandID_Validated(null, null);
                }
                else if (FocusedControl == gridControl.Name)
                {
                    if (gridView1.FocusedColumn.Name == "colBarCode")
                    {
                        string Barcode = cls.PrimaryKeyValue.ToString();
                        gridView1.AddNewRow();
                        gridView1.SetRowCellValue(rowIndex, gridView1.Columns["BarCode"], Barcode);
                        FileItemData(Stc_itemsDAL.GetItemData(Barcode, UserInfo.FacilityID));
                    }

                    else if (gridView1.FocusedColumn.Name == "col"+SizeName || gridView1.FocusedColumn.Name == "colSizeID")
                    {
                        AddRow();
                        int SizeID = Comon.cInt(cls.PrimaryKeyValue.ToString());
                        DataTable dtSize = Lip.SelectRecord("SELECT SizeID, " + PrimaryName + " AS " + SizeName + " FROM Stc_SizingUnits Where Cancel=0 and BranchID=" +MySession.GlobalBranchID+" And SizeID=" + SizeID);
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeID"], dtSize.Rows[0]["SizeID"].ToString());
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], dtSize.Rows[0][SizeName].ToString());
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["FacilityID"], UserInfo.FacilityID);
                    }


                } 

                
                return true;
            }
            return false;
        }
        public void ReadRecord(long ItemID)
        {
            try
            {
               
                ClearFields();
                {
                    dt = Stc_itemsDAL.frmGetDataDetailByID(ItemID, UserInfo.BRANCHID, UserInfo.FacilityID);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        IsNewRecord = false;
                        //Validate
                       
                        txtGroupID.Text = dt.Rows[0]["GroupID"].ToString();
                        txtGroupID_Validating(null, null);

                        txtTypeID.Text = dt.Rows[0]["TypeID"].ToString();
                        txtTypeID_Validating(null, null);

                        txtBaseID.Text = dt.Rows[0]["BaseID"].ToString();
                        txtBaseID_Validating(null, null);
                     
                        txtBrandID.Text = dt.Rows[0]["BrandID"].ToString();
                        txtBrandID_Validated(null, null);

                        try
                        {
                            byte[] imgByte = null;
                            if (DBNull.Value != dt.Rows[0]["ItemImage"])
                            {
                                imgByte = (byte[])dt.Rows[0]["ItemImage"];

                                picItemImage.Image = byteArrayToImage(imgByte);
                            }
                            else
                                picItemImage.Image = null;
                        }
                        catch { }
                        //Masterdata
                        txtItemID.Text = dt.Rows[0]["ItemID"].ToString();
                        txtNotes.Text = dt.Rows[0]["Notes"].ToString();
                        txtArbName.Text = dt.Rows[0]["ArbName"].ToString();
                        txtEngName.Text = dt.Rows[0]["EngName"].ToString();
                        chkIsVAT.Checked = Comon.cInt(dt.Rows[0]["IsVat"].ToString()) == 1 ? true : false;
                        chkIsService.Checked = Comon.cInt(dt.Rows[0]["IsService"].ToString()) == 1 ? true : false;
                        chkStopeItem.Checked = Comon.cInt(dt.Rows[0]["StopeItem"].ToString()) == 1 ? true : false;
                        chkIsUnbreakable.Checked = Comon.cInt(dt.Rows[0]["IsUnbreakable"].ToString()) == 1 ? true : false;

                        chkShowInOrderDetils.Checked = Comon.cInt(dt.Rows[0]["ShowInOrderDetils"].ToString()) == 1 ? true : false;
                        //GridVeiw
                         
                        gridControl.DataSource = dt;
                        lstDetail.AllowNew = true;
                        lstDetail.AllowEdit = true;
                        lstDetail.AllowRemove = true;
                       
                        EnabledControl(true);
                        Validations.DoReadRipon(this,ribbonControl1);
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        /// <summary>
        /// This function returns the name of the currently focused control.
        /// </summary>
        /// <returns></returns>
        string GetIndexFocusedControl()
        {
            // Get the currently active control.
            Control c = this.ActiveControl;

            // If the active control is a DevExpress LayoutControl, get the focused child control.
            if (c is DevExpress.XtraLayout.LayoutControl)
            {
                if (!(((DevExpress.XtraLayout.LayoutControl)ActiveControl).ActiveControl == null))
                {
                    c = ((DevExpress.XtraLayout.LayoutControl)ActiveControl).ActiveControl;
                }
            }
            // If the active control is a DevExpress TextBoxMaskBox,
            // set the control to its parent control.
            if (c is DevExpress.XtraEditors.TextBoxMaskBox)
            {
                c = c.Parent;
            }

            // If the parent of the active control is a DevExpress GridControl,
            // return its name as the focused control.
            if (c.Parent is DevExpress.XtraGrid.GridControl)
            {
                return c.Parent.Name;
            }

            // Otherwise, return the name of the active control.
            return c.Name;
        }

        public void ClearFields()
        {         
            try
            {
                //txtItemID.Text = Stc_itemsDAL.GetNewID().ToString();
                txtArbName.Text = "";
                txtBrandID.Text = "";
                lblBrandID.Text = "";
                txtEngName.Text = "";
                txtNotes.Text = "";
                picItemImage.Image = null;
                cmbBranchesID.ReadOnly = !MySession.GlobalAllowBranchModificationAllScreens;
                txtGroupID.Text = "";
                txtGroupID_Validating(null, null);
                txtTypeID.Text = "1";
                txtBaseID.Text = "";
                txtBaseID_Validating(null, null);
                txtTypeID_Validating(null, null);
                lstDetail = new BindingList<Stc_ItemUnits>();
                lstDetail.AllowNew = true;
                lstDetail.AllowEdit = true;
                lstDetail.AllowRemove = true;
                gridControl.DataSource = lstDetail;
                chkStopeItem.Checked = false;
                chkIsUnbreakable.Checked = false;
                chkIsService.Checked = false;
                lstDetailBarcode = new BindingList<Stc_ItemBarcode>();             
                dt = new DataTable();

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        /**********************MoveRec*****************************************/
        public void MoveRec(long PremaryKeyValue, int Direction)
        {
            try
            {
                #region If
                if (FormView == true)
                {
                    strSQL = "SELECT TOP 1 * FROM " + Stc_itemsDAL.TableName + " Where Cancel =0 and BranchID=" + MySession.GlobalBranchID;
                    switch (Direction)
                    {
                        case xMoveFirst:
                            {
                                strSQL = strSQL + " ORDER BY " + Stc_itemsDAL.PremaryKey + " ASC";
                                break;
                            }
                        case xMoveNext:
                            {
                                strSQL = strSQL + " And " + Stc_itemsDAL.PremaryKey + ">" + PremaryKeyValue + " ORDER BY " + Stc_itemsDAL.PremaryKey + " asc";
                                break;
                            }
                        case xMovePrev:
                            {
                                strSQL = strSQL + " And " + Stc_itemsDAL.PremaryKey + "<" + PremaryKeyValue + " ORDER BY " + Stc_itemsDAL.PremaryKey + " desc";
                                break;
                            }
                        case xMoveLast:
                            {
                                strSQL = strSQL + " ORDER BY " + Stc_itemsDAL.PremaryKey + " DESC";
                                break;
                            }
                    }
                    cClass = new Stc_itemsDAL();
                    long InvoicIDTemp = Comon.cLong(txtItemID.Text);
                    InvoicIDTemp = cClass.GetRecordSetBySQL(strSQL);
                    if (cClass.FoundResult == true)
                       ReadRecord(InvoicIDTemp);                        
                    SendKeys.Send("{Escape}");
                }
                #endregion
                else
                {
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
        /********************this Function To Save Image Item**************/
        /// <summary>
        ///  Define a function called SaveImage() that takes a byte array as a parameter and saves it to a database record
        /// </summary>
        /// <param name="data"></param>
        private void SaveImage(byte[] data)
        {
            try
            {
                // Get a connection to the database from a global connection object
                SqlConnection Con = new GlobalConnection().Conn;

                // Open the connection if it's not already open
                if (Con.State == ConnectionState.Closed)
                    Con.Open();
                // Create a SQL command to update a record in the Stc_Items table with the input byte array as the ItemImage column
                SqlCommand sc;
                sc = new SqlCommand("Update Stc_Items Set ItemImage=@p Where ItemID=" + txtItemID.Text + " and BranchID=" + MySession.GlobalBranchID, Con);

                // Add the input byte array as a parameter to the SQL command
                sc.Parameters.AddWithValue("@p", data);

                // Execute the SQL command
                sc.ExecuteNonQuery();
            }
            catch
            {
                // Handle any exceptions that occur during database operations
            }
        }

        /******************This Function To Convert The Image To Byte Array****************/
        /// <summary>
        /// Define a function called imageToByteArray() that takes a System.Drawing.Image object as a parameter and returns a byte array
        /// </summary>
        /// <param name="imageIn"></param>
        /// <returns></returns>
        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            // Create a new memory stream
            MemoryStream ms = new MemoryStream();

            // Save the image to the memory stream as a PNG
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            // Return the memory stream as a byte array
            return ms.ToArray();
        }

        /// <summary>
        ///  Define a function called byteArrayToImage() that takes a byte array as a parameter and returns a System.Drawing.Image object
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            // Create a new memory stream using the input byte array
            MemoryStream ms = new MemoryStream(byteArrayIn);

            // Load the image into a System.Drawing.Image object
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);

            // Return the image object
            return returnImage;
        }

        /// <summary>
        /// Define a function called DefaultImage() that returns a byte array
        /// </summary>
        /// <returns></returns>
        private byte[] DefaultImage()
        {
            // Get the directory path of the executable
            string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);

            // Append the relative path to the image file
            Path = Path + @"\Images\Default.png";

            // Load the image into a System.Drawing.Image object
            System.Drawing.Image img = System.Drawing.Image.FromFile(Path);

            // Create a new memory stream
            MemoryStream ms = new System.IO.MemoryStream();

            // Save the image to the memory stream as a PNG
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            // Return the memory stream as a byte array
            return ms.ToArray();
        }
       /// <summary>
       /// This Function is Used To Fill item data
       /// </summary>
       /// <param name="dt"></param>
        private void FileItemData(DataTable dt)
        {

            if (dt != null && dt.Rows.Count > 0)
            {
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"], dt.Rows[0]["BarCode"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"], dt.Rows[0]["ItemID"].ToString());

                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], dt.Rows[0][SizeName].ToString());

                if (UserInfo.Language == iLanguage.English)
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], dt.Rows[0][SizeName].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ExpiryDate"], Comon.ConvertSerialToDate(dt.Rows[0]["ExpiryDate"].ToString()));
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SalePrice"], dt.Rows[0]["SalePrice"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CostPrice"], dt.Rows[0]["CostPrice"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeID"], dt.Rows[0]["SizeID"].ToString());

                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CLARITY"], dt.Rows[0]["CLARITY"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Color"], dt.Rows[0]["Color"].ToString());



                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["PackingQty"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["LastCostPrice"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["LastSalePrice"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SpecialSalePrice"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SpecialCostPrice"], 0);

                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AverageCostPrice"], "");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AllowedPercentDiscount"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemProfit"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["MinLimitQty"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["MaxLimitQty"], 0);

                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Caliber"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Equivalen"], 0);

                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["UnitCancel"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Serials"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Height"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Width"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Caliber"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BAGET_W"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["STONE_W"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["DIAMOND_W"], 0);

            }
            else
            {
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"], "0");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"], "0");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeID"], "0");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], " ");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ExpiryDate"], DateTime.Now.ToShortDateString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["PackingQty"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SalePrice"], "0");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CostPrice"], "0");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["LastCostPrice"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["LastSalePrice"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SpecialSalePrice"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SpecialCostPrice"], 0);

                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AverageCostPrice"], "");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AllowedPercentDiscount"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemProfit"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["MinLimitQty"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["MaxLimitQty"], 0);

                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Caliber"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Equivalen"], 0);

                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["UnitCancel"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Serials"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Height"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Width"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Caliber"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BAGET_W"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["STONE_W"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["DIAMOND_W"], 0);
            }

        }

        /// <summary>
        /// This Function is used to Enabled Control  in this form 
        /// </summary>
        /// <param name="Value"></param>
        private void EnabledControl(bool Value)
        {
            // Loop through all controls in the form
            foreach (Control item in this.Controls)
            {
                // For TextEdit controls that don't have "AccountID" or "AccountName" in their name,
                // and don't have "lbl" and "Name" in their name
                if (item is TextEdit && ((!(item.Name.Contains("AccountID"))) && (!(item.Name.Contains("AccountName")))))
                {
                    if (!(item.Name.Contains("lbl") && item.Name.Contains("Name")))
                    {
                        // Set their Enabled property and specified AppearanceDisabled foreground and background color based on the Value parameter
                        item.Enabled = Value;
                        ((TextEdit)item).Properties.AppearanceDisabled.ForeColor = Color.Black;
                        ((TextEdit)item).Properties.AppearanceDisabled.BackColor = Color.White;
                        if (Value == true)
                            ((TextEdit)item).Properties.AppearanceDisabled.BackColor = Color.White;
                    }
                }
                // For TextEdit controls that have "AccountID" or "AccountName" in their name
                else if (item is TextEdit && (((item.Name.Contains("AccountID"))) || ((item.Name.Contains("AccountName")))))
                {
                    // Set their Enabled property and specified AppearanceDisabled foreground and background color based on the Value parameter
                    item.Enabled = Value;
                    ((TextEdit)item).Properties.AppearanceDisabled.ForeColor = Color.Black;
                    ((TextEdit)item).Properties.AppearanceDisabled.BackColor = Color.White;
                    if (Value)
                        ((TextEdit)item).Properties.AppearanceDisabled.BackColor = Color.White;
                }
                // For SimpleButton controls that have "btn" and "Search" in their name
                else if (item is SimpleButton && (((item.Name.Contains("btn"))) && ((item.Name.Contains("Search")))))
                {
                    // Set their Enabled property based on the Value parameter
                    ((SimpleButton)item).Enabled = Value;
                }
            }

            // Loop through each column in gridView1
            foreach (GridColumn col in gridView1.Columns)
            {
                // Allow or disallow editing, focusing, and read-only based on the Value parameter
                // for the specified column names
                if (col.FieldName == "PackingQty" || col.FieldName == "SizeID" || col.FieldName == "ItemID" || col.FieldName == "CostPrice")
                {
                    gridView1.Columns[col.FieldName].OptionsColumn.AllowEdit = Value;
                    gridView1.Columns[col.FieldName].OptionsColumn.AllowFocus = Value;
                    gridView1.Columns[col.FieldName].OptionsColumn.ReadOnly = !Value;
                    

                }
                //Set readonly To Wigth BAGET
                if (col.FieldName == "BAGET_W" || col.FieldName == "MinLimitQty" || col.FieldName == "DIAMOND_W"|| col.FieldName == "STONE_W")
                    gridView1.Columns[col.FieldName].OptionsColumn.ReadOnly = false;
                


            }

        }

        /// <summary>
        /// In this function, the entries of the gridview are checked if they are correct and not incomplete or empty, 
        /// or if the field entries are numbers and the entry is in text form,
        /// an error message is displayed in front of the field in which the error exists..
        /// </summary>
        /// 
        /// <returns>
        /// The function returns a boolean value. If all the inputs are true,
        /// the value of True is returned, and if one of the fields has incorrect inputs, the value is returned False.
        /// </returns>
        bool IsValidGrid()
        {
            double num;

            // Check if there are any error columns
            if (HasColumnErrors)
            {
                Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                return !HasColumnErrors;
            }

            // Move to the last row of the grid and check if there are any records
            gridView1.MoveLast();
            int length = gridView1.RowCount - 1;
            if (length <= 0)
            {
                Messages.MsgError(Messages.TitleError, Messages.msgThereIsNoRecordInput);
                return false;
            }

            // Loop through each row and column to check for errors in the input values
            for (int i = 0; i < length; i++)
            {
                foreach (GridColumn col in gridView1.Columns)
                {
                    if (col.FieldName == "PackingQty" || col.FieldName == "SizeID")
                    {
                        // Get the cell value and check if it is null or empty
                        var cellValue = gridView1.GetRowCellValue(i, col);
                        if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                        {
                            gridView1.SetColumnError(col, Messages.msgInputIsRequired);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }

                        // Check if the cell value is a valid number
                        else if (!(double.TryParse(cellValue.ToString(), out num)))
                        {
                            gridView1.SetColumnError(col, Messages.msgInputShouldBeNumber);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }

                        // Check if the cell value is greater than zero
                        else if (Comon.cDbl(cellValue.ToString()) <= 0)
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
        /// <summary>
        /// This Function is used to import the item by button import
        /// </summary>
        private void AddItems()
        {
            try
            {

                string[] ArrValues = new string[10000];

                long inc;
                bool Found;
                frmItemsGroups frmGroup;
                frmSizingUnits frmUnits;
                frmItems frmItems;
                DataTable dtTest = new DataTable();
                string ItemName = "";
                string SizeName = "";
                OleDbConnection oledbConn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\\AddItems.xlsx;Extended Properties=Excel 12.0");
                int Result;

                cItemsStores Store = new cItemsStores();
                bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, "سيتم حذف جميع بيانات المواد هل أنت متأكد من المتابعة ؟");
                if (!Yes)
                    return;

                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                oledbConn.Open();

                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [StockMaster$]", oledbConn);
                OleDbDataAdapter oleda = new OleDbDataAdapter();
                oleda.SelectCommand = cmd;
                DataTable dt = new DataTable();
                oleda.Fill(dt);
                oledbConn.Close();

                if (dt.Rows.Count < 1)
                    return;

                //OleDbConnection oledbConn1 = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\\55r55.xlsx;Extended Properties=Excel 12.0");
                //oledbConn.Open();
                //OleDbCommand cmd1 = new OleDbCommand("SELECT * FROM [StockMaster$]", oledbConn1);
                //OleDbDataAdapter oleda1 = new OleDbDataAdapter();
                //oleda1.SelectCommand = cmd1;
                //DataTable dtFilter = new DataTable();
                //oleda1.Fill(dtFilter);
                //oledbConn1.Close();

                Lip.ExecuteProcedure("Dell_ALLTransItems_SP");
                //Lip.ExecututeSQL("delete from dbo.Sales_PurchaseInvoiceDetails");
                //Lip.ExecututeSQL("delete from dbo.Stc_Items");
                //Lip.ExecututeSQL("delete from dbo.Stc_ItemUnits");
                //Lip.ExecututeSQL("delete from dbo.Stc_ItemsGroups");
                //Lip.ExecututeSQL("delete from dbo.Stc_SizingUnits");
                //Lip.ExecututeSQL("delete from dbo.Sales_PurchaseInvoiceMaster");
                //Lip.ExecututeSQL("delete from dbo.Stc_Stores");
                //Lip.ExecututeSQL("Delete From Acc_VariousVoucherDetails");
                //Lip.ExecututeSQL("Delete From Acc_VariousVoucherMaster");
                //Lip.ExecututeSQL("Delete From Sales_SalesInvoiceMaster");
                //Lip.ExecututeSQL("Delete From Sales_SalesInvoiceDetails");
                //   Lip.ExecututeSQL("Delete From Stc_ItemBarcode");



                //'إضافة المستودع الأول
                Lip.NewFields();
                Lip.Table = Store.TableName;
                //  Lip.AddNumericField("StoreID", Store.GetNewID().ToString());
                Lip.AddStringField("ArbName", "المستودع الرئيسي");
                Lip.AddStringField("EngName", "Main Store");
                Lip.AddNumericField("BranchID", MySession.GlobalBranchID.ToString());
                Lip.AddNumericField("FacilityID", UserInfo.FacilityID.ToString());
                Lip.AddStringField("Tel", "");
                Lip.AddStringField("Mobile", "");
                Lip.AddStringField("Fax", "");
                Lip.AddStringField("Address", "");
                Lip.AddStringField("StoreManger", "");
                Lip.AddStringField("Notes", "");
                Lip.AddNumericField("UserID", UserInfo.ID);
                Lip.AddNumericField("RegDate", Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate())).ToString());
                Lip.AddNumericField("RegTime", Comon.cDbl(Lip.GetServerTimeSerial()).ToString());
                Lip.AddNumericField("EditUserID", UserInfo.ID);
                Lip.AddNumericField("EditDate", Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate())).ToString());
                Lip.AddNumericField("EditTime", Comon.cDbl(Lip.GetServerTimeSerial()).ToString());
                Lip.AddStringField("ComputerInfo", MySession.GlobalComputerInfo);
                Lip.AddStringField("EditComputerInfo", MySession.GlobalComputerInfo);
                Lip.AddNumericField("Cancel", 0);
                Lip.ExecuteInsert();

                //'إضافة المجموعات
                frmGroup = new frmItemsGroups();
                frmGroup.Show();
                inc = 0;
                for (int i = 0; i <= dt.Rows.Count - 1; ++i)
                {
                    Found = false;
                    for (int j = 1; j <= ArrValues.Length - 1; ++j)
                    {
                        if ((dt.Rows[i]["GruopID"]).ToString() == ArrValues[j])
                        {
                            Found = true;
                            break;
                        }
                    }
                    if (Found == false)
                    {
                        inc = inc + 1;
                        ArrValues[inc] = dt.Rows[i]["GruopID"].ToString();
                        frmGroup.txtArbName.Text = dt.Rows[i]["GruopID"].ToString();
                        frmGroup.txtEngName.Text = dt.Rows[i]["GruopID"].ToString();
                        frmGroup.IsFromanotherForms = true;
                        frmGroup.save();
                    }
                }
                frmGroup.Dispose();
                //'إضافة الوحدات الأساسية
                for (int i = 1; i <= ArrValues.Length - 1; ++i)
                {

                    ArrValues[i] = "";
                }
                frmUnits = new frmSizingUnits();
                frmUnits.Show();
                inc = 0;
                for (int i = 0; i <= dt.Rows.Count - 1; ++i)
                {
                    Found = false;
                    for (int j = 1; j <= ArrValues.Length - 1; ++j)
                    {

                        if (dt.Rows[i]["UnitName"].ToString() == ArrValues[j])
                        {
                            Found = true;
                            break;
                        }
                    }
                    if (Found == false)
                    {
                        inc = inc + 1;
                        ArrValues[inc] = dt.Rows[i]["UnitName"].ToString();
                        frmUnits.txtArbName.Text = dt.Rows[i]["UnitName"].ToString();
                        frmUnits.txtEngName.Text = dt.Rows[i]["UnitName"].ToString();
                        frmUnits.IsFromanotherForms = true;
                        frmUnits.save();

                    }
                }

                frmUnits.Dispose();
                bool flag = true;
                //'إضافة المواد
                cItems Item = new cItems();
                for (int i = 0; i <= dt.Rows.Count - 1; ++i)
                {
                    if (string.IsNullOrEmpty(dt.Rows[i]["Barcode"].ToString()))
                        continue;
                    Application.DoEvents();
                    ItemName = dt.Rows[i]["ItemName"].ToString();
                    Lip.NewFields();
                    Lip.Table = "Stc_Items";
                    Lip.AddNumericField("ItemID", Comon.cInt(new cItems().GetNewID()));
                    Lip.AddStringField("ArbName", dt.Rows[i]["ItemName"].ToString());
                    Lip.AddStringField("EngName", Translator.ConvertNameToOtherLanguage(dt.Rows[i]["ItemName"].ToString().Trim(), UserInfo.Language));
                    long GroupID = Comon.cLong(Lip.GetValue("Select Top 1 GroupID From Stc_ItemsGroups Where ArbName='" + dt.Rows[i]["GruopID"] + "' and BranchID=" + MySession.GlobalBranchID));
                    Lip.AddNumericField("GroupID", GroupID.ToString());
                    Lip.AddStringField("Notes", "");
                    Lip.AddNumericField("TypeID", 1);
                    Lip.AddNumericField("UserID", UserInfo.ID);
                    Lip.AddNumericField("RegDate", Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate())).ToString());
                    Lip.AddNumericField("RegTime", Comon.cDbl(Lip.GetServerTimeSerial()).ToString());
                    Lip.AddNumericField("EditUserID", UserInfo.ID);
                    Lip.AddNumericField("EditDate", Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate())).ToString());
                    Lip.AddNumericField("EditTime", Comon.cDbl(Lip.GetServerTimeSerial()).ToString());
                    Lip.AddStringField("ComputerInfo", MySession.GlobalComputerInfo);
                    Lip.AddStringField("EditComputerInfo", MySession.GlobalComputerInfo);
                    Lip.AddNumericField("Cancel", 0);
                    Lip.AddStringField("IsVat", "1");
                    Lip.AddStringField("IsService", "0");
                    Lip.AddStringField("StopeItem", "1");
                    Lip.AddStringField("IsUnbreakable", "0");
                    Lip.AddNumericField("ColorID", 0);
                    Lip.AddNumericField("BrandID", 0);
                    Lip.AddNumericField("BaseID", txtBaseID.Text);
                    Lip.AddNumericField("BranchID", 0);
                    Lip.AddNumericField("FacilityID", UserInfo.FacilityID.ToString());
                    Lip.ExecuteInsert();
                }

                //'إضافة وحدات المواد
                cItemsUnits ItemUnit = new cItemsUnits();
                string BarCode = "";
                for (int i = 0; i <= dt.Rows.Count - 1; ++i)
                {
                    Application.DoEvents();
                    if (string.IsNullOrEmpty(dt.Rows[i]["Barcode"].ToString()))
                        continue;
                    BarCode = dt.Rows[i]["Barcode"].ToString();
                    Lip.NewFields();
                    Lip.Table = "Stc_ItemUnits";
                    long itemID = Comon.cLong(Lip.GetValue("Select Top 1 ItemID From Stc_Items Where ArbName='" + dt.Rows[i]["ItemName"] + "' and BranchID=" + MySession.GlobalBranchID));
                    long SizeID = Comon.cLong(Lip.GetValue("Select Top 1 SizeID From Stc_SizingUnits Where ArbName='" + dt.Rows[i]["UnitName"] + "' and BranchID=" + MySession.GlobalBranchID));
                    Lip.AddNumericField("ItemID", itemID.ToString());
                    BarCode = dt.Rows[i]["BarCode"].ToString();
                    Lip.AddNumericField("SizeID", SizeID.ToString());
                    Lip.AddStringField("BarCode", BarCode);
                    Lip.AddNumericField("PackingQty", Comon.ConvertToDecimalPrice(dt.Rows[i]["PackingQty"].ToString()).ToString());
                    Lip.AddStringField("CostPrice", Comon.ConvertToDecimalPrice(dt.Rows[i]["CostPrice"].ToString()).ToString());
                    Lip.AddNumericField("SalePrice", Comon.ConvertToDecimalPrice(dt.Rows[i]["SalePrice"].ToString()).ToString());
                    Lip.AddNumericField("MinLimitQty", 10);
                    Lip.AddNumericField("MaxLimitQty", 400);
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

                }

                //'إضافة وحدات المواد في فواتير الشراء
                DataTable dtItemUnits = new DataTable();
                var strSQL1 = "Select * From Stc_ItemUnits where BranchID=" + MySession.GlobalBranchID;
                dtItemUnits = Lip.SelectRecord(strSQL1);
                for (int i = 0; i <= dtItemUnits.Rows.Count - 1; ++i)
                {
                    Application.DoEvents();
                    Lip.NewFields();
                    Lip.Table = "Sales_PurchaseInvoiceDetails";
                    Lip.AddNumericField("InvoiceID", -1);
                    Lip.AddNumericField("BranchID", Comon.cInt(cmbBranchesID.EditValue));
                    Lip.AddNumericField("FacilityID", UserInfo.FacilityID.ToString());
                    Lip.AddNumericField("ItemID", dtItemUnits.Rows[i]["ItemID"].ToString());
                    Lip.AddNumericField("SizeID", dtItemUnits.Rows[i]["SizeID"].ToString());
                    Lip.AddNumericField("QTY", 0);
                    Lip.AddNumericField("CostPrice", Comon.ConvertToDecimalPrice(dtItemUnits.Rows[i]["CostPrice"]).ToString());
                    Lip.AddNumericField("Bones", 0);
                    Lip.AddNumericField("StoreID", 0);
                    Lip.AddNumericField("Discount", 0);
                    Lip.AddNumericField("ExpiryDate", 20201101);
                    Lip.AddNumericField("SalePrice", Comon.ConvertToDecimalPrice(dtItemUnits.Rows[i]["SalePrice"]).ToString());
                    Lip.AddStringField("BarCode", dtItemUnits.Rows[i]["BarCode"].ToString());
                    Lip.AddStringField("Serials", "");
                    Lip.AddNumericField("Cancel", 0);
                    Lip.AddNumericField("AdditionalValue", 0);
                    Lip.ExecuteInsert();
                }


                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("تمت العملية بنجاح", "");
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }

        }

        /// <summary>
        /// This  Initialize GridView
        /// </summary>
        void InitGrid()
        {
            lstDetail = new BindingList<Stc_ItemUnits>();
            lstDetail.AllowNew = true;
            lstDetail.AllowEdit = true;
            lstDetail.AllowRemove = true;
            gridControl.DataSource = lstDetail;


            /******************* Columns Visible=true *******************/
            gridView1.Columns["BarCode"].Caption = CaptionBarCode;
            gridView1.Columns["SizeID"].Caption = CaptionSizeID;
            gridView1.Columns[SizeName].Caption = CaptionSizeName;

            gridView1.Columns["PackingQty"].Caption = CaptionPackingQty;

            gridView1.Columns["MaxLimitQty"].Caption = CaptionMaxLimitQty;

            gridView1.Columns["SalePrice"].Caption = CaptionSalePrice;
            gridView1.Columns["CostPrice"].Caption = CaptionCostPrice;

            gridView1.Columns["LastCostPrice"].Caption = CaptionLastCostPrice;
            gridView1.Columns["LastSalePrice"].Caption = CaptionLastSalePrice;

            gridView1.Columns["SpecialSalePrice"].Caption = CaptionSpecialSalePrice;
            gridView1.Columns["SpecialCostPrice"].Caption = CaptionSpecialCostPrice;

            gridView1.Columns["ItemProfit"].Caption = CaptionItemProfit;
            gridView1.Columns["AllowedPercentDiscount"].Caption = CaptionAllowedPercentDiscount;
            gridView1.Columns["UnitCancel"].Caption = CaptionUnitCancel;

            /********************Set Caption for Columns***********************/
            gridView1.Columns["MinLimitQty"].Caption = "الوزن ";
          
            gridView1.Columns["DIAMOND_W"].Caption = "وزن الالماس";
            gridView1.Columns["Equivalen"].Caption = " المعادل";
            gridView1.Columns["STONE_W"].Caption = "وزن الأحجار";
             
            gridView1.Columns["BAGET_W"].Caption = "الباجيت";
            gridView1.Columns["ZIRCON_W"].Caption = "وزن الزركون";
            gridView1.Columns["CLARITY"].Caption =CaptionCLARITY ;
            gridView1.Columns["Color"].Caption = CaptionColor;


            /******************* Columns Visible=false ********************/
            gridView1.Columns["PackingQty"].Visible = true;
           
            gridView1.Columns["MaxLimitQty"].Visible = false;

            gridView1.Columns["SpecialCostPrice"].Visible = false;
            gridView1.Columns["ItemProfit"].Visible = false;
            gridView1.Columns["AllowedPercentDiscount"].Visible = false;
            gridView1.Columns[SizeName].Visible = true;
            gridView1.Columns["Caliber"].Visible = false;
            gridView1.Columns["ID"].Visible = false;
            gridView1.Columns["ItemID"].Visible = false;
            gridView1.Columns["AverageCostPrice"].Visible = false;
        
            gridView1.Columns["DIAMOND_W"].Visible = true;
            gridView1.Columns["Equivalen"].Visible = false;
            gridView1.Columns["STONE_W"].Visible = true;

            gridView1.Columns["BAGET_W"].Visible = true;
            gridView1.Columns["Height"].Visible = false;
            gridView1.Columns["Width"].Visible = false;
            gridView1.Columns["Serials"].Visible = false;
            gridView1.Columns["Stc_Items"].Visible = false;
            gridView1.Columns["BranchID"].Visible = false;
            gridView1.Columns["FacilityID"].Visible = false;
            gridView1.Columns["UnitCancel"].Visible = false;
            gridView1.Columns["SizeID"].Visible = false;

            gridView1.Columns["SpecialSalePrice"].Visible = true;
            gridView1.Columns["SpecialCostPrice"].Visible = false;
            gridView1.Columns["LastCostPrice"].Visible = false;
            gridView1.Columns["LastSalePrice"].Visible = false;
            gridView1.Columns["ItemProfit"].Visible = false;
            gridView1.Columns["LastSalePrice"].Visible = false;

            gridView1.Columns["ArbSizeName"].Visible = gridView1.Columns["ArbSizeName"].Name == "col" + SizeName ? true : false;
            gridView1.Columns["EngSizeName"].Visible = gridView1.Columns["EngSizeName"].Name == "col" + SizeName ? true : false;

            gridView1.Columns["STONE_W"].Visible = false;
            gridView1.Columns["ZIRCON_W"].Visible = false;
            gridView1.Columns["BAGET_W"].Visible = false;
            gridView1.Columns["DIAMOND_W"].Visible = false;
            gridView1.Columns["ArbSizeName"].VisibleIndex = 1;
            gridView1.Columns["MinLimitQty"].VisibleIndex = 5;
            gridView1.Columns["MinLimitQty"].Visible = false;
            gridView1.Columns["CostPrice"].VisibleIndex = gridView1.Columns["PackingQty"].VisibleIndex + 1;
            gridView1.Focus();
            RepositoryItemLookUpEdit rSize = Common.LookUpEditSize();
            gridView1.Columns[SizeName].ColumnEdit = rSize;
            gridControl.RepositoryItems.Add(rSize);

            //RepositoryItemLookUpEdit rItemID = Common.LookUpEditItemID();
            //gridView1.Columns["ItemID"].ColumnEdit = rItemID;
            //gridControl.RepositoryItems.Add(rItemID);

            //RepositoryItemLookUpEdit rItem = Common.LookUpEditItemName();
            //gridView1.Columns[ItemName].ColumnEdit = rItem;
            //gridControl.RepositoryItems.Add(rItem);

            //RepositoryItemLookUpEdit rBarCode = Common.LookUpEditBarCode();
            //gridView1.Columns["BarCode"].ColumnEdit = rBarCode;
            //gridControl.RepositoryItems.Add(rBarCode);

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
        /// <summary>
        /// Initializes the grid barcode by creating a new BindingList object and setting the AllowNew, AllowEdit, and AllowRemove properties to true
        /// </summary>
        void InitGridBarcode()
        {
            // Creates a new BindingList of Stc_ItemBarcode objects
            lstDetailBarcode = new BindingList<Stc_ItemBarcode>();
            // Sets the AllowNew property of the BindingList to true
            lstDetailBarcode.AllowNew = true;
            // Sets the AllowEdit property of the BindingList to true
            lstDetailBarcode.AllowEdit = true;
            // Sets the AllowRemove property of the BindingList to true
            lstDetailBarcode.AllowRemove = true;
        }
        #endregion
        #region Do Function
        protected override void DoNew()
        {
            try
            {
                IsNewRecord = true;
                //txtItemID.Text = Stc_itemsDAL.GetNewID().ToString();
                ClearFields();
                EnabledControl(true);

                txtArbName.Focus();
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
                MoveRec(Comon.cInt(txtItemID.Text), xMoveNext);


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
                MoveRec(Comon.cInt(txtItemID.Text), xMovePrev);
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
                txtItemID.Enabled = true;
                txtItemID.Focus();
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
            bool isTrans = Lip.CheckTheItemIsHaveTransactionByItemID(Comon.cInt(txtItemID.Text));
            if (isTrans)
            {
                Messages.MsgExclamationk(Messages.TitleInfo, "لابمكن تعديل  مادة  لها حركات  مخزنية ");
                return;
            }
            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("ID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("FacilityID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("BranchID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("ItemID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("SizeID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("ItemProfit", System.Type.GetType("System.String"));
            dtItem.Columns.Add("PackingQty", System.Type.GetType("System.String"));
            dtItem.Columns.Add("BarCode", System.Type.GetType("System.String"));
            dtItem.Columns.Add(SizeName, System.Type.GetType("System.String"));
            dtItem.Columns.Add("LastCostPrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("LastSalePrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("MaxLimitQty", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("SalePrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("CostPrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("SpecialCostPrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("SpecialSalePrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("UnitCancel", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("AllowedPercentDiscount", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("AverageCostPrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("MinLimitQty", System.Type.GetType("System.Decimal"));

            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {

                dtItem.Rows.Add();
                dtItem.Rows[i]["ID"] = i;
                dtItem.Rows[i]["FacilityID"] = UserInfo.FacilityID;
                dtItem.Rows[i]["BranchID"] = MySession.GlobalBranchID;
                dtItem.Rows[i]["BarCode"] = gridView1.GetRowCellValue(i, "BarCode") == null ? string.Empty : gridView1.GetRowCellValue(i, "BarCode");
                dtItem.Rows[i][SizeName] = gridView1.GetRowCellValue(i, SizeName).ToString();
                dtItem.Rows[i]["ItemID"] = Comon.cInt(gridView1.GetRowCellValue(i, "ItemID").ToString());
                dtItem.Rows[i]["ItemProfit"] = Comon.cDbl(gridView1.GetRowCellValue(i, "ItemProfit").ToString());
                dtItem.Rows[i]["PackingQty"] = Comon.cDbl(gridView1.GetRowCellValue(i, "PackingQty").ToString());

                dtItem.Rows[i]["SizeID"] = Comon.cInt(gridView1.GetRowCellValue(i, "SizeID").ToString());
                dtItem.Rows[i]["SalePrice"] = Comon.cDbl(gridView1.GetRowCellValue(i, "SalePrice").ToString());
                dtItem.Rows[i]["CostPrice"] = Comon.cDbl(gridView1.GetRowCellValue(i, "CostPrice").ToString());
                dtItem.Rows[i]["LastCostPrice"] = Comon.cDbl(gridView1.GetRowCellValue(i, "LastCostPrice").ToString());
                dtItem.Rows[i]["LastSalePrice"] = Comon.cDbl(gridView1.GetRowCellValue(i, "LastSalePrice").ToString());
                dtItem.Rows[i]["MaxLimitQty"] = Comon.cDbl(gridView1.GetRowCellValue(i, "MaxLimitQty").ToString());
                dtItem.Rows[i]["MinLimitQty"] = Comon.cDbl(gridView1.GetRowCellValue(i, "MinLimitQty").ToString());
                dtItem.Rows[i]["SpecialCostPrice"] = Comon.cDbl(gridView1.GetRowCellValue(i, "SpecialCostPrice").ToString());
                dtItem.Rows[i]["SpecialSalePrice"] = Comon.cDbl(gridView1.GetRowCellValue(i, "SpecialSalePrice").ToString());
                dtItem.Rows[i]["UnitCancel"] = Comon.cInt(gridView1.GetRowCellValue(i, "UnitCancel").ToString());
                dtItem.Rows[i]["AllowedPercentDiscount"] = Comon.cDbl(gridView1.GetRowCellValue(i, "AllowedPercentDiscount").ToString());
                dtItem.Rows[i]["AverageCostPrice"] = Comon.cDbl(gridView1.GetRowCellValue(i, "AverageCostPrice").ToString());

            }
            gridControl.DataSource = dtItem;
            EnabledControl(true);
            Validations.DoEditRipon(this, ribbonControl1);
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
        private bool AddBarCode(int Rowindex, string BarCode,int sizeID)
        {
            try
            {
               //if(IsNewRecord==false)
               // strSQL = "Select GroupID From Stc_ItemsGroups where GroupID='"+"0" + txtGroupID.Text+"'";
               //else
               //    strSQL = "Select GroupID From Stc_ItemsGroups where GroupID='"  +txtGroupID.Text + "'";
               // DataTable dtGroup = Lip.SelectRecord(strSQL);
               // string GroupName = dtGroup.Rows[0]["GroupID"].ToString();

                //if (BarCode == string.Empty)
                    BarCode =  txtItemID.Text+ sizeID.ToString();
                gridView1.SetRowCellValue(Rowindex, gridView1.Columns["BarCode"], BarCode.ToString());
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void Save()
        {
            gridView1.MoveLastVisible();
           
            long ItemID = Comon.cLong(txtItemID.Text);
            Stc_Items objRecord = new Stc_Items();

            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = UserInfo.FacilityID;

            objRecord.ItemID = ItemID;
            objRecord.ArbName = txtArbName.Text;
            objRecord.EngName = txtEngName.Text;

            objRecord.GroupID = Comon.cDbl(txtGroupID.Text);
            objRecord.TypeID = Comon.cInt(txtTypeID.Text);
            objRecord.BrandID = Comon.cInt(txtBrandID.Text);
            objRecord.BaseID = Comon.cInt(txtBaseID.Text);

            txtNotes.Text = (txtNotes.Text.Trim() != "" ? txtNotes.Text.Trim() : (UserInfo.Language == iLanguage.English ? "Stock Items " : " شاشة الاصناف "));
            objRecord.Notes = txtNotes.Text.Trim();
            objRecord.IsVAT = chkIsVAT.Checked == true ? 1 : 0;

            objRecord.IsService = chkIsService.Checked == true ? 1 : 0;
            objRecord.ShowInOrderDetils = chkShowInOrderDetils.Checked == true ? 1 : 0;
            objRecord.StopeItem = chkStopeItem.Checked == true ? 1 : 0;
            objRecord.IsUnbreakable = chkIsUnbreakable.Checked == true ? 1 : 0;
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

                objRecord.EditUserID = UserInfo.ID;
                objRecord.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                objRecord.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
                objRecord.EditComputerInfo = UserInfo.ComputerInfo;
                if (picItemImage.Image!=null){
                byte[] Imagebyte = imageToByteArray(picItemImage.Image);
                objRecord.picItemImage = Imagebyte;
                }
            }

            if (OpenFileDialog1 != null && (OpenFileDialog1.FileName != ""))
            {
                picItemImage.Image = Image.FromFile(OpenFileDialog1.FileName);
                picItemImage.Visible = true;
                byte[] Imagebyte = imageToByteArray(picItemImage.Image);
                objRecord.picItemImage = Imagebyte;
            }
            else
                objRecord.picItemImage = DefaultImage();
            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {

                String BarCode = gridView1.GetRowCellValue(i, "BarCode") == null ? string.Empty : gridView1.GetRowCellValue(i, "BarCode").ToString();

                if (AddBarCode(i, BarCode, Comon.cInt(gridView1.GetRowCellValue(i, "SizeID"))) == false)
                {
                    SplashScreenManager.CloseForm(false);
                    Messages.MsgInfo("يرجى التاكد من بيانات الصنف ", BarCode);
                  
                    return;

                }
            }

            Stc_ItemUnits returned;
            List<Stc_ItemUnits> listreturned = new List<Stc_ItemUnits>();
            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                returned = new Stc_ItemUnits();
                returned.ID = i;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = MySession.GlobalBranchID;
                returned.SizeID = Comon.cInt(gridView1.GetRowCellValue(i, "SizeID").ToString());
                    
                returned.BarCode = gridView1.GetRowCellValue(i, "BarCode") == null ? string.Empty : gridView1.GetRowCellValue(i, "BarCode").ToString();
              
                returned.ItemID = Comon.cLong(txtItemID.Text);
                returned.ItemProfit = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "ItemProfit").ToString());
                returned.PackingQty = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "PackingQty").ToString());
                returned.ArbSizeName =  gridView1.GetRowCellValue(i, "ArbSizeName").ToString();
                
                returned.SalePrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice").ToString());
                returned.CostPrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CostPrice").ToString());
                returned.LastCostPrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "LastCostPrice").ToString());
                returned.LastSalePrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "LastSalePrice").ToString());
               // returned.DIAMOND_W = Comon.cDbl(gridView1.GetRowCellValue(i, "DIAMOND_W").ToString());
               // returned.BAGET_W = Comon.cDbl(gridView1.GetRowCellValue(i, "BAGET_W").ToString());
               // returned.ZIRCON_W = Comon.cDbl(gridView1.GetRowCellValue(i, "ZIRCON_W").ToString());
               // returned.STONE_W = Comon.cDbl(gridView1.GetRowCellValue(i, "STONE_W").ToString());
                returned.MaxLimitQty = Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(i, "MaxLimitQty").ToString());
                returned.MinLimitQty = Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(i, "MinLimitQty").ToString());
                returned.SpecialCostPrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SpecialCostPrice").ToString());
                returned.SpecialSalePrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SpecialSalePrice").ToString());
                //returned.UnitCancel = Comon.cInt(gridView1.GetRowCellValue(i, "UnitCancel").ToString());
                returned.UnitCancel = objRecord.StopeItem;
                returned.AllowedPercentDiscount = Comon.cDbl(gridView1.GetRowCellValue(i, "AllowedPercentDiscount").ToString());
                returned.AverageCostPrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "AverageCostPrice").ToString());
                returned.Serials = "";
                if (returned.PackingQty <= 0 || returned.SizeID <= 0)
                    continue;
                listreturned.Add(returned);
            }
            if (listreturned.Count > 0)
            {
                objRecord.Stc_ItemUnits = listreturned;
                string Result = Stc_itemsDAL.InsertUsingXML(objRecord, IsNewRecord);
                SplashScreenManager.CloseForm(false);
                if (IsNewRecord == true)
                {
                   
                    if (Comon.cInt( Result)>= 1)
                    {
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                        DoNew();
                    }
                    else if (Result == "2627")
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
                    if (Result == "1")
                    {
                       
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                        txtItemID_Validating(null, null);

                    }
                    else if (Result == "2627")
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
        public void Save2Print()
        {
            gridView1.MoveLastVisible();

            int ItemID = Comon.cInt(txtItemID.Text);
            Stc_Items objRecord = new Stc_Items();

            objRecord.BranchID = MySession.GlobalBranchID;
            objRecord.FacilityID = UserInfo.FacilityID;

            objRecord.ItemID = ItemID;
            objRecord.ArbName = txtArbName.Text;
            objRecord.EngName = txtEngName.Text;

            objRecord.GroupID = Comon.cDbl(txtGroupID.Text);
            objRecord.TypeID = Comon.cInt(txtTypeID.Text);

            txtNotes.Text = (txtNotes.Text.Trim() != "" ? txtNotes.Text.Trim() : (UserInfo.Language == iLanguage.English ? "Stock Items " : " شاشة الاصناف "));
            objRecord.Notes = txtNotes.Text.Trim();
            objRecord.IsVAT = chkIsVAT.Checked == true ? 1 : 0;
            objRecord.IsService = chkIsService.Checked == true ? 1 : 0;
            objRecord.StopeItem = chkStopeItem.Checked == true ? 1 : 0;
            objRecord.IsUnbreakable = chkIsUnbreakable.Checked == true ? 1 : 0;
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
                objRecord.EditUserID = UserInfo.ID;
                objRecord.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                objRecord.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
                objRecord.EditComputerInfo = UserInfo.ComputerInfo;
                if (picItemImage.Image != null)
                {
                    byte[] Imagebyte = imageToByteArray(picItemImage.Image);
                    objRecord.picItemImage = Imagebyte;
                }
            }

            if (OpenFileDialog1 != null && (OpenFileDialog1.FileName != ""))
            {

                picItemImage.Image = Image.FromFile(OpenFileDialog1.FileName);
                picItemImage.Visible = true;
                byte[] Imagebyte = imageToByteArray(picItemImage.Image);
                objRecord.picItemImage = Imagebyte;
            }
            else
                objRecord.picItemImage = DefaultImage();



            Stc_ItemUnits returned;
            List<Stc_ItemUnits> listreturned = new List<Stc_ItemUnits>();


            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {


                returned = new Stc_ItemUnits();
                returned.ID = i;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = MySession.GlobalBranchID;
                returned.BarCode = gridView1.GetRowCellValue(i, "BarCode") == null ? string.Empty : gridView1.GetRowCellValue(i, "BarCode").ToString();
                returned.ItemID = Comon.cLong(txtItemID.Text);
                returned.ItemProfit = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "ItemProfit").ToString());
                returned.PackingQty = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "PackingQty").ToString());
                returned.SizeID = Comon.cInt(gridView1.GetRowCellValue(i, "SizeID").ToString());
                returned.SalePrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice").ToString());
                returned.CostPrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CostPrice").ToString());
                returned.LastCostPrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "LastCostPrice").ToString());
                returned.LastSalePrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "LastSalePrice").ToString());
                returned.MaxLimitQty = Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(i, "MaxLimitQty").ToString());
                returned.MinLimitQty = Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(i, "MinLimitQty").ToString());
                returned.SpecialCostPrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SpecialCostPrice").ToString());
                returned.SpecialSalePrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SpecialSalePrice").ToString());
                returned.UnitCancel = Comon.cInt(gridView1.GetRowCellValue(i, "UnitCancel").ToString());
                returned.AllowedPercentDiscount = Comon.cDbl(gridView1.GetRowCellValue(i, "AllowedPercentDiscount").ToString());
                returned.AverageCostPrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "AverageCostPrice").ToString());

                returned.Serials = "";
                if (returned.PackingQty <= 0   || returned.SizeID <= 0)
                    continue;
                listreturned.Add(returned);

            }

            if (listreturned.Count > 0)
            {
                objRecord.Stc_ItemUnits = listreturned;
                string Result = Stc_itemsDAL.InsertUsingXML(objRecord, IsNewRecord);
                SplashScreenManager.CloseForm(false);


                if (IsNewRecord == true)
                {
                    if (Result == "1")
                    {
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                        

                       
                    }
                    else if (Result == "2627")
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


                    if (Result == "1")
                    {

                       

                     
                       
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);

                    }
                    else if (Result == "2627")
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
        public List<Stc_ItemUnits> SortAccounts(List<Stc_ItemUnits> accounts)
        {
            List<Stc_ItemUnits> sortedAccounts = new List<Stc_ItemUnits>();
            Dictionary<double, Stc_ItemUnits> accountMap = new Dictionary<double, Stc_ItemUnits>();

            foreach (var account in accounts)
            {
                accountMap[account.ItemID] = account;
            }
            accounts.Sort((a, b) => a.ItemID.CompareTo(b.ItemID));
            foreach (var account in accounts)
            {
                {
                    sortedAccounts.Add(account);
                    //AddChildAccounts(account, accountMap, sortedAccounts);
                }
            }

            return sortedAccounts;
        }

        private void AddChildAccounts(Stc_ItemUnits parentAccount, Dictionary<double, Stc_ItemUnits> accountMap, List<Stc_ItemUnits> sortedAccounts)
        {
            foreach (var account in accountMap.Values)
            {
                
                {
                    sortedAccounts.Add(account);
                    AddChildAccounts(account, accountMap, sortedAccounts);
                }
            }
        }
        protected override void DoPrint()
        {
            try
            {
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                /******************** Report Body *************************/
                bool IncludeHeader = true;
                ReportName = "‏‏rptItems";
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                if (UserInfo.Language == iLanguage.English)
                    rptFormName = ReportName + "Arb";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);
                /***************** Master *****************************/

                rptForm.RequestParameters = false;
                rptForm.Parameters["FromItemID"].Value =txtItemID.Text.Trim().ToString();
                rptForm.Parameters["ToItemID"].Value = txtItemID.Text.Trim().ToString();
                rptForm.Parameters["ItemStatus"].Value =(chkStopeItem.Checked)?"موقف" :"فعال";
                rptForm.Parameters["GroupID"].Value =txtGroupName.Text.Trim().ToString();
                /********************** Details ****************************/
                var dataTable = new Edex.ModelSystem.dsReports.rptItemsDataTable();
                List<Stc_ItemUnits> ListItems = new List<Stc_ItemUnits>();

                ListItems = Stc_itemsDAL.GetAllData(Comon.cInt(MySession.GlobalBranchID), UserInfo.FacilityID);
                 ListItems = ListItems.FindAll(x => x.ItemID == Comon.cLong(txtItemID.Text));
                //ListItems = SortAccounts(ListItems);
                for (int i = 0; i <= ListItems.Count - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["n_invoice_serial"] = i + 1;
                    row["ItemID"] = ListItems[i].ItemID;
                    row["ArbItemName"] = ListItems[i].Stc_Items.ArbName;
                    row["BarCode"] = ListItems[i].BarCode;
                    DataTable dt = Lip.SelectRecord("select ArbName as GroupParentName , GroupID as GroupParentID from [Stc_ItemsGroups] where [GroupID] in(SELECT  [ParentAccountID] FROM  [Stc_ItemsGroups] where [GroupID]=" + ListItems[i].Stc_Items.GroupID + " and BranchID="+MySession.GlobalBranchID+") and BranchID="+MySession.GlobalBranchID);
                    row["GroupID"] = ListItems[i].Stc_Items.GroupID;
                    row["GroupName"] = ListItems[i].Stc_Items.GroupName;
                    row["GroupParentID"] =dt.Rows[0]["GroupParentID"];
                    row["GroupParentName"] = dt.Rows[0]["GroupParentName"];
                    row["TypeName"] = ListItems[i].Stc_Items.TypeName;
                    row["SizeID"] = ListItems[i].SizeID;
                    row["SizeName"] = ListItems[i].ArbSizeName;
                    dataTable.Rows.Add(row);
                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = "rptBalanceReview";
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
        protected override void DoDelete()
        {
            try
            {


                bool isTrans = Lip.CheckTheItemIsHaveTransactionByItemID(Comon.cLong(txtItemID.Text));
                if (isTrans)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, "لابمكن حذف مادة  لها حركات  مخزنية ");
                    return;
                }
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
                int TempID = Comon.cInt(txtItemID.Text);

                Stc_Items model = new Stc_Items();
                model.ItemID = Comon.cLong(txtItemID.Text);
                model.EditUserID = UserInfo.ID;
                model.BranchID = UserInfo.BRANCHID;
                model.FacilityID = UserInfo.FacilityID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());
                model.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                string Result = Stc_itemsDAL.Delete(model);
                SplashScreenManager.CloseForm(false);
                if (Comon.cInt(Result) > 0)
                {
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);
                    MoveRec(model.ItemID, xMovePrev);
                }
                else 
                {
                    Messages.MsgError(Messages.TitleError, Messages.msgErrorSave + " " + Result);
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
        #endregion

        private void frmItems_Load(object sender, EventArgs e)
        {

        }


        #endregion


        private void btnShow_Click(object sender, EventArgs e)
        {
            if (IsNewRecord == false)
            {
                int fromDate = 0;
                int ToDate = 0;
                cSTORESETTING.STC_STORINGSETTING_COSTTYPE = 3;
                if (txtFromDateTrans.EditValue != null)
                    fromDate = Comon.ConvertDateToSerial(txtFromDateTrans.Text.ToString());
                if (txtToDateTrans.EditValue != null)
                    ToDate = Comon.ConvertDateToSerial(txtToDateTrans.Text.ToString());
                if (cSTORESETTING.STC_STORINGSETTING_COSTTYPE == 3)
                    //المرجح المتوسط
                    gridControlItemTrans.DataSource = GetItemMoving(Comon.cLong(txtItemID.Text), Comon.cInt(txiSizeID.Text), Comon.cDbl(txtStoreTransID.Text), Comon.cInt(txtBranch.Text), Comon.cInt(txtTypeTrans.Text), fromDate, ToDate);
            }
        }
        public static DataTable GetItemMoving(long ItemID = 0, int SizeID = 0, double StoreID = 0, int BranchID = 0, int DocumentType = 0, long FromDate = 0, long ToDate = 0)
        {
            decimal Avg = 0;
            string Filttr = " Where 1=1 and Cancel=0 ";
            decimal CurentBalance = 0;
            decimal TotalQtyToInTrans = 0;
            decimal TotalCostToInTrans = 0;
            decimal TotalSaleToInTrans = 0;

            cSTORESETTING.STC_DecimalNumriceForCostDigits = 3;
            if (SizeID > 0)
                Filttr = Filttr + " And SizeID=" + SizeID;

            if (ItemID > 0)
                Filttr = Filttr + " And ITEMID=" + ItemID;

            if (StoreID > 0)
                Filttr = Filttr + " And STOREID=" + StoreID;

            if (BranchID > 0)
                Filttr = Filttr + " And BRANCHID=" + BranchID;

            if (DocumentType > 0)
                Filttr = Filttr + " And DocumentType=" + DocumentType;

            if (FromDate > 0)
                Filttr = Filttr + " And MoveDate>=" + FromDate;

            if (ToDate > 0)
                Filttr = Filttr + " And MoveDate<=" + ToDate;


            string strSQL = @"select '0' AS  CostaleGood,  QTY,OutPrice, (QTY* OutPrice) AS OutTotal , InPrice,(QTY* InPrice) AS InTotal ,  (QTY* InPrice) AS  TOTAL , QTY AS  OutQTY ,  OutPrice , (QTY* OutPrice) AS   OutTotal , MoveID AS  ID,BONES,0.0 AS Balance, 0.0 AS  TOTALBALANCE , 0.0 AS CurentAverageCostPrice,MoveDate,MoveType,
                                DocumentTypeID, Declaration,StoreID,SizeID   
                                From Stc_ItemsMoviing " + Filttr + "  order by ID";

            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
            {
                dt.Columns["CurentAverageCostPrice"].ReadOnly = false;
                dt.Columns["Balance"].ReadOnly = false;
                dt.Columns["TOTALBALANCE"].ReadOnly = false;
                dt.Columns["OutQTY"].ReadOnly = false;
                dt.Columns["OutPrice"].ReadOnly = false;
                dt.Columns["OutTotal"].ReadOnly = false;
                dt.Rows[0]["CurentAverageCostPrice"] = dt.Rows[0]["InPrice"];
                dt.Rows[0]["Balance"] = dt.Rows[0]["QTY"];
                dt.Rows[0]["TOTALBALANCE"] = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(dt.Rows[0]["BALANCE"].ToString()) * Comon.ConvertToDecimalPrice(dt.Rows[0]["InPrice"].ToString()));
                CurentBalance = Comon.cDec(dt.Rows[0]["TOTALBALANCE"].ToString());
                TotalQtyToInTrans = TotalQtyToInTrans + Comon.cDec(dt.Rows[0]["QTY"].ToString());
                TotalCostToInTrans = TotalCostToInTrans + Comon.cDec((Comon.cDec(dt.Rows[0]["QTY"].ToString()) * Comon.cDec(dt.Rows[0]["InPrice"].ToString())));
                for (int i = 1; i <= dt.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < i; j++)
                        if (dt.Rows[i]["MoveType"].ToString() == "1")
                            Avg = Comon.ConvertToDecimalPrice(dt.Rows[j]["CurentAverageCostPrice"].ToString());
                    if (dt.Rows[i]["MoveType"].ToString() == "1" )
                    {
                        TotalQtyToInTrans = TotalQtyToInTrans + Comon.cDec(dt.Rows[i]["QTY"].ToString());                        
                        TotalCostToInTrans = TotalCostToInTrans + Comon.cDec((Comon.cDec(dt.Rows[i]["QTY"].ToString()) * Comon.cDec(dt.Rows[i]["InPrice"].ToString())));
                        dt.Rows[i]["Balance"] = Comon.ConvertToDecimalPrice(dt.Rows[i - 1]["Balance"].ToString()) + Comon.ConvertToDecimalPrice(dt.Rows[i]["QTY"].ToString());
                        dt.Rows[i]["TOTALBALANCE"] = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(dt.Rows[i]["TOTAL"].ToString()) + Comon.ConvertToDecimalPrice(dt.Rows[i - 1]["TOTALBALANCE"].ToString()));
                        if(Comon.ConvertToDecimalPrice(dt.Rows[i]["Balance"].ToString())!=0)
                           dt.Rows[i]["CurentAverageCostPrice"] = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(dt.Rows[i]["TOTALBALANCE"].ToString()) / Comon.ConvertToDecimalPrice(dt.Rows[i]["Balance"].ToString()));
                    }
                    if (dt.Rows[i]["MoveType"].ToString() == "2")
                    {
                        TotalSaleToInTrans = TotalSaleToInTrans + Comon.cDec((Comon.cDec(dt.Rows[i]["QTY"].ToString()) * Comon.cDec(dt.Rows[i]["OutPrice"].ToString())));
                        dt.Rows[i]["OutQTY"] = dt.Rows[i]["QTY"];
                        dt.Rows[i]["OutPrice"] = Avg;
                        dt.Rows[i]["OutTotal"] = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(dt.Rows[i]["QTY"].ToString()) * Avg);
                        // dt.Rows[i]["TOTAL"] = Comon.ConvertToDecimalCostPrice(Comon.ConvertToDecimalCostPrice(dt.Rows[i]["QTY"].ToString()) * Comon.ConvertToDecimalCostPrice(dt.Rows[i]["COSTPRICE"].ToString()));
                        dt.Rows[i]["CurentAverageCostPrice"] = Comon.ConvertToDecimalPrice(Avg);
                        dt.Rows[i]["Balance"] = Comon.ConvertToDecimalPrice(dt.Rows[i - 1]["Balance"].ToString()) - Comon.ConvertToDecimalPrice(dt.Rows[i]["QTY"].ToString());
                        dt.Rows[i]["TOTALBALANCE"] = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(dt.Rows[i]["Balance"].ToString()) * Comon.ConvertToDecimalPrice(dt.Rows[i]["CurentAverageCostPrice"].ToString()));
                    }
                }
            }
            if (dt.Rows.Count > 0)
            { 
                decimal BalanceQty = Comon.ConvertToDecimalCostPrice(dt.Rows[dt.Rows.Count - 1]["BALANCE"].ToString());
                decimal avgDory = 0;
                if (TotalQtyToInTrans!=0)
                  avgDory = Comon.ConvertToDecimalCostPrice(TotalCostToInTrans / TotalQtyToInTrans);
                //قيمة مخزون اخر المدة 
                decimal TotalValueStore = Comon.ConvertToDecimalPrice(BalanceQty * avgDory);
                //تكلفة البضاعة المباعة
                decimal CostSaleByDory = Comon.ConvertToDecimalCostPrice(TotalCostToInTrans - TotalValueStore);
                decimal Profit = Comon.ConvertToDecimalCostPrice(TotalSaleToInTrans - CostSaleByDory);
           }
            return dt;
        }
        public static decimal GetItemAverageCostPrice(long ItemID = 0, int SizeID = 0, double StoreID = 0, int BranchID = 0, int DocumentType = 0, long FromDate = 0, long ToDate = 0)
        {
            try
            {
                DataTable dtt = frmItems.GetItemMoving(ItemID, SizeID, StoreID, BranchID, 0, 0, 0);
                decimal AverageCost = Comon.cDec(dtt.Rows[dtt.Rows.Count-1]["CurentAverageCostPrice"].ToString());
                return AverageCost;
            }
            catch
            {
                return 0;
            }
        }

       

        
      }
}

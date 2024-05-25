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
using DevExpress.XtraEditors.Repository;
using Edex.ModelSystem;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralClasses;
using Edex.DAL.Stc_itemDAL;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraGrid.Columns;
using Edex.DAL.SalseSystem;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
 
namespace Edex.SalesAndPurchaseObjects.Transactions
{
    public partial class frmPurchaseDaimondDetils : BaseForm
    {
        #region declare
        //list detail
        BindingList<Sales_PurchaseDiamondDetails> lstDetail = new BindingList<Sales_PurchaseDiamondDetails>();
        private string ItemName;
        private string CaptionBarCode;
        private string CaptionItemID;
        private string CaptionItemName;
        private string CaptionPricCarat;
        private string CaptionPricTotal;
        private string PrimaryName;
        private string strSQL;
        private bool IsNewRecord;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        public bool HasColumnErrors = false;
        string FocusedControl = "";
        DataTable dt;
        #endregion
        public frmPurchaseDaimondDetils()
        
        {

            FormAdd = true;
            FormView = true;
            ReportView = true;
            ReportExport = true;
            FormUpdate = true;
            FormDelete = true;
            InitializeComponent();
            ItemName = "ArbItemName";
            CaptionBarCode = "الباركود";
            CaptionItemID = "رقم الصنف";
            CaptionItemName = "اسم الصنف";
            CaptionPricCarat = "السعر";
            CaptionPricTotal = "الإجمالي";
            PrimaryName = "ArbName";
            if (UserInfo.Language == iLanguage.English)
            {
                ItemName = "ArbItemName";
                CaptionBarCode = "BarCode";
                CaptionItemID = "Item ID";
                CaptionItemName = "Item Name";
                CaptionPricCarat = "Price";
                CaptionPricTotal = "Total";
            }
            InitGrid();
            this.txtSupplierID.Validating+=txtSupplierID_Validating;
            this.txtStoreID.Validating+=txtStoreID_Validating;
            this.KeyDown += frmPurchaseDaimond_KeyDown;
            this.gridView1.ValidatingEditor += gridView1_ValidatingEditor;
            this.gridControl.ProcessGridKey += gridControl_ProcessGridKey;

            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", "ArbName", "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
            cmbBranchesID.EditValue = UserInfo.BRANCHID;


            ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[9].Visible = false;
        
        }

       
        void InitGrid()
        {
            lstDetail = new BindingList<Sales_PurchaseDiamondDetails>();
            lstDetail.AllowNew = true;
            lstDetail.AllowEdit = true;
            lstDetail.AllowRemove = true;
            gridControl.DataSource = lstDetail;

            /******************* Columns Visible=false ********************/
                       
            gridView1.Columns["InvoiceID"].Visible = false;           
            gridView1.Columns["FacilityID"].Visible = false;
            gridView1.Columns["StoreID"].Visible = false;
            gridView1.Columns["Cancel"].Visible = false;
          
            gridView1.Columns["ArbItemName"].Visible = gridView1.Columns["ArbItemName"].Name == "col" + ItemName ? true : false;
            

            gridView1.Columns["BarCode"].Visible = true;
         
            /******************* Columns Visible=true *******************/
            gridView1.Columns[ItemName].Visible = true;      
            gridView1.Columns["ItemID"].Visible = false;
        
            gridView1.Columns["SupplierID"].Visible = false;
            gridView1.Columns["BranchID"].Visible = false;
            gridView1.Columns["BarCodeItem"].Visible = false;
            
            gridView1.Columns["BarCode"].Caption = CaptionBarCode;
            gridView1.Columns["PriceCarat"].Caption = CaptionPricCarat;
            gridView1.Columns["TotalPrice"].Caption = CaptionPricTotal;
          
            gridView1.Columns["ItemID"].Caption = CaptionItemID;
            gridView1.Columns[ItemName].Caption = CaptionItemName;
            gridView1.Columns[ItemName].Width = 180;

            gridView1.Columns["WeightOut"].Caption = "إخراج -وزن";
            gridView1.Columns["WeightIn"].Caption = "إدخال-وزن";
            gridView1.Columns["TypeOpration"].Caption = "رقم العملية";
            gridView1.Columns["TypeOpration"].Visible = false;
            gridView1.Focus();
           
           
      


            /************************ Look Up Edit **************************/

            RepositoryItemLookUpEdit rItem = Common.LookUpEditItemName();
            gridView1.Columns[ItemName].ColumnEdit = rItem;
            gridControl.RepositoryItems.Add(rItem);


            RepositoryItemTextEdit txt = new RepositoryItemTextEdit();



            gridView1.Columns["CaptionOpration"].Caption = "نوع العملية";
            gridView1.Columns["CaptionOpration"].Name = "CaptionOpration";
            gridView1.Columns["CaptionOpration"].Width = 200;
        
 
            /////////////////////////Item
            ///

            DataTable dtitems = Lip.SelectRecord("SELECT distinct ArbName AS ArbName FROM Stc_DiamondItemsType");
            string[] companiesitems = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                companiesitems[i] = dtitems.Rows[i]["ArbName"].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(companiesitems);

            gridControl.RepositoryItems.Add(riComboBoxitems);
            gridView1.Columns[ItemName].ColumnEdit = riComboBoxitems;
            ///////////////////////////

            /////////////////////////Description
         
            gridView1.Columns[ItemName].Width = 150;

            gridView1.AddNewRow();
            gridView1.Focus();
            gridView1.MoveNext();
            gridView1.FocusedColumn = gridView1.VisibleColumns[0];
          
 

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
                        if (ColName == "BarCode" || ColName == "PriceCarat"  || ColName == "TotalPrice")
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
                            if (ColName == "TotalPrice"&&
                                view.GetRowCellValue(view.FocusedRowHandle,"TotalPrice").ToString()!=""&&
                                  view.GetRowCellValue(view.FocusedRowHandle,"PriceCarat").ToString()!="")
                            {
                              //  gridView1.AddNewRow();
                                gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                                gridView1.FocusedColumn = gridView1.VisibleColumns[0];
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
        public void ReadRecord(int type, int TypeOpration)
        {
            if (TypeOpration == 1 || TypeOpration == 5)
            {
                gridView1.Columns["WeightOut"].Visible = false;
            }
            else if ( TypeOpration == 6)
            {
                gridView1.Columns["WeightOut"].Visible = true;
                gridView1.Columns["WeightIn"].Visible = false;
            }
            else if (TypeOpration == 7)
            {
                gridView1.Columns["WeightOut"].Visible = false;
                gridView1.Columns["WeightIn"].Visible = true;
            }
            else if (TypeOpration == 9)
            {
                gridView1.Columns["WeightOut"].Visible = false;
                gridView1.Columns["WeightIn"].Visible = true;
            } 
            try
            {
                ClearFields();
                {
                    if (type == 1)
                        dt = Sales_PurchaseDiamondDetailsDAL.frmGetDataDetalByID(Comon.cInt(txtInvoiceID.Text), Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID, txtBarCode.Text, TypeOpration);
                    else
                    {
                        dt = Sales_PurchaseDiamondDetailsDAL.frmGetDataDetalByBarCode(Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID, txtBarCode.Text, TypeOpration);
                        if (dt == null || dt.Rows.Count <= 0)
                            dt = Sales_PurchaseDiamondDetailsDAL.frmGetDataDetalByBarCode(Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID, txtBarCode.Text, 5);
                        else  if (dt == null || dt.Rows.Count <= 0)
                            dt = Sales_PurchaseDiamondDetailsDAL.frmGetDataDetalByBarCode(Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID, txtBarCode.Text, 9);
                    }
                    if (dt != null && dt.Rows.Count > 0)
                    {                       
                        IsNewRecord = false;                
                        //Validate
                        txtStoreID.Text = dt.Rows[0]["StoreID"].ToString();
                        txtSupplierID.Text = dt.Rows[0]["SupplierID"].ToString();
                        txtSupplierID_Validating(null, null);
                        cmbBranchesID.EditValue = Comon.cInt(dt.Rows[0]["BranchID"].ToString());
                        if (Comon.cInt(dt.Rows[0]["TypeOpration"].ToString()) == 1 || Comon.cInt(dt.Rows[0]["TypeOpration"].ToString()) == 5)
                            txtWeight.Text = dt.Rows[0]["WeightIn"].ToString();
                        else
                            txtWeight.Text = dt.Rows[0]["WeightOut"].ToString();
                        //Masterdata
                        txtInvoiceID.Text = dt.Rows[0]["InvoiceID"].ToString();
                        txtStoreID_Validating(null, null);
                        txtSupplierID_Validating(null, null);

                        //GridVeiw
                             gridControl.DataSource = dt;
                            lstDetail.AllowNew = true;
                            lstDetail.AllowEdit = true;
                            lstDetail.AllowRemove = true;
                          
                        Validations.DoReadRipon(this, ribbonControl1);
                       // ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Caption = txtInvoiceID.Text;
                    }
                    gridView1.AddNewRow();
                }
                

                
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        void ClearFields()
        {

       


            lstDetail = new BindingList<Sales_PurchaseDiamondDetails>();
            lstDetail.AllowNew = true;
            lstDetail.AllowEdit = true;
            lstDetail.AllowRemove = true;
            gridControl.DataSource = lstDetail;

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
                if (ColName == "BarCode" || ColName == "PriceCarat" || ColName == "WeightIn" || ColName == "WeightOut" || ColName == "ItemID")
                {
                    if (ColName == "WeightIn" && Comon.cInt(lblTypeOpration.Text) == 6)
                        HasColumnErrors = false;
                    if (ColName == "WeightOut" && Comon.cInt(lblTypeOpration.Text) != 6)
                        HasColumnErrors = false;

                    if (val == null || string.IsNullOrWhiteSpace(val.ToString()))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputIsRequired;
                    }
                    else if (!(double.TryParse(val.ToString(), out num)) && ColName != "BarCode" )
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputShouldBeNumber;
                    }
                    /****************************************/
                    if (ColName == "PriceCarat")
                    {
                        decimal Weight =0;
                        if(Comon.cInt(lblTypeOpration.Text)!=6)
                        Weight = Comon.ConvertToDecimalPrice(gridView1.GetFocusedRowCellValue("WeightIn"));
                        else
                            Weight = Comon.ConvertToDecimalPrice(gridView1.GetFocusedRowCellValue("WeightOut"));
                        decimal PriceCarat = Comon.ConvertToDecimalPrice(val.ToString());
                        gridView1.SetFocusedRowCellValue("TotalPrice", Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalQty(Weight) *Comon.ConvertToDecimalPrice( PriceCarat)).ToString());
      
                    }                                     
                    if (ColName == "BarCode")
                    {
                        DataTable dt;
                        var flagb = false;                        
                            dt = Lip.SelectRecord("select * from Stc_DiamondItemsType where BarCodeDimond='" + val.ToString()+"'");
                        if (dt.Rows.Count == 0)
                        {
                            // e.Valid = false;
                            // HasColumnErrors = true;
                            // e.ErrorText = Messages.msgNoFoundThisBarCode;
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
                }
                else if (ColName == ItemName)
                {
                    DataTable dtItemID = Lip.SelectRecord("select * from Stc_DiamondItemsType where BarCodeDimond='" + val.ToString() + "'");
                    if (dtItemID.Rows.Count > 0)
                    {
                        DataTable dtItem  = Lip.SelectRecord("select * from Stc_DiamondItemsType where BarCodeDimond='" + val.ToString() + "'");
                        if (dtItem.Rows.Count == 0)
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(gridView1.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                        else
                        {                           
                            if (MySession.GlobalAllowUsingDateItems)
                            {
                                MySession.GlobalAllowUsingDateItems = false;
                                FileItemData(dtItem);
                                MySession.GlobalAllowUsingDateItems = true;
                            }
                            else
                                FileItemData(dtItem);
                            e.Valid = true;
                            view.SetColumnError(gridView1.Columns[ColName], "");
                        }
                    }
                    else
                    {
                        //e.Valid = false;
                        //HasColumnErrors = true;
                        //e.ErrorText = Messages.msgNoFoundThisItem;
                        //view.SetColumnError(gridView1.Columns[ColName], Messages.msgNoFoundThisItem);
                    }
                }
              
            }
          
        }
        private void frmPurchaseDaimond_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.F3)
                Find();
            
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
            decimal totalWeight=0;
           
            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                if (Comon.cInt(lblTypeOpration.Text) == 6)
                totalWeight += Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(i, "WeightOut").ToString());
                else
                    totalWeight += Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(i, "WeightIn").ToString());
             }
            if (Comon.ConvertToDecimalQty(totalWeight)!=Comon.ConvertToDecimalQty(txtWeight.Text))
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(Messages.TitleError, " لابد أن يكون إجمالي الأوزان في جميع ألاصناف مساوي للإجمالي الكلي الموضح في الأعلى");
                return;
            }
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];
            Sales_PurchaseDiamondDetails objRecord = new Sales_PurchaseDiamondDetails();
            objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
            
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = UserInfo.FacilityID;
 
            objRecord.StoreID = Comon.cInt(txtStoreID.Text); 
            objRecord.SupplierID = Comon.cDbl(txtSupplierID.Text);
            objRecord.BarCodeItem = txtBarCode.Text;
            objRecord.Cancel = 0;
            if (Comon.cInt(lblTypeOpration.Text) == 1)
               objRecord.TypeOpration = 1;
               
             
            else if (Comon.cInt(lblTypeOpration.Text) == 5)
              objRecord.TypeOpration = 5;
            else if (Comon.cInt(lblTypeOpration.Text) == 6)
                objRecord.TypeOpration = 6;
            else if (Comon.cInt(lblTypeOpration.Text) == 7)
                objRecord.TypeOpration = 7;

            else if (Comon.cInt(lblTypeOpration.Text) == 9)
                objRecord.TypeOpration = 9;


            Sales_PurchaseDiamondDetails returned;
            List<Sales_PurchaseDiamondDetails> listreturned = new List<Sales_PurchaseDiamondDetails>();         
                for (int i = 0; i < gridView1.DataRowCount - 1; i++)
                {

                    returned = new Sales_PurchaseDiamondDetails();                 
                    returned.FacilityID = UserInfo.FacilityID;
                    returned.BranchID =Comon.cInt(cmbBranchesID.EditValue);
                    returned.BarCode = gridView1.GetRowCellValue(i, "BarCode").ToString();
                    returned.ItemID = Comon.cInt(gridView1.GetRowCellValue(i, "ItemID").ToString());
                    returned.BarCodeItem = txtBarCode.Text;
                    returned.ArbItemName=gridView1.GetRowCellValue(i, ItemName).ToString();
                    returned.WeightIn = Comon.cDec(gridView1.GetRowCellValue(i, "WeightIn").ToString());

                    returned.InvoiceID = Comon.cInt(txtInvoiceID.Text);
                    returned.WeightOut = Comon.cDec(gridView1.GetRowCellValue(i, "WeightOut").ToString());
                    if (Comon.cInt(lblTypeOpration.Text) == 1)
                    {
                        returned.TypeOpration = 1;
                        returned.CaptionOpration = "فاتورة مشتريات";
                    }
                    else if (Comon.cInt(lblTypeOpration.Text) == 5)
                    {
                        returned.TypeOpration = 5;
                        returned.CaptionOpration = "بضاعة أول المدة";

                    }
                    else if (Comon.cInt(lblTypeOpration.Text) == 6)
                    {
                        returned.TypeOpration = 6;
                        returned.CaptionOpration = "سند صرف";

                    }
                    else if (Comon.cInt(lblTypeOpration.Text) == 7)
                    {
                        returned.TypeOpration = 7;
                        returned.CaptionOpration = "سند أمانات";

                    }

                    else if (Comon.cInt(lblTypeOpration.Text) == 9)
                    {
                        returned.TypeOpration = 9;
                        returned.CaptionOpration = "توريد مخزني ";

                    }
                    returned.PriceCarat = Comon.cDec(gridView1.GetRowCellValue(i, "PriceCarat").ToString()); 
                    returned.TotalPrice = Comon.cDec(gridView1.GetRowCellValue(i, "TotalPrice").ToString());
                    returned.SupplierID = Comon.cDbl(txtSupplierID.Text);
                    returned.StoreID = Comon.cInt(txtStoreID.Text);

                    if (  returned.StoreID <= 0 || (returned.PriceCarat <= 0 && returned.TotalPrice <= 0) || returned.ItemID <= 0)
                        continue;
                    listreturned.Add(returned);
                }

            if (listreturned.Count > 0)
            {
                objRecord.DiamondDatails = listreturned;
                string Result = Sales_PurchaseDiamondDetailsDAL.InsertUsingXML(objRecord, IsNewRecord).ToString();
               
                SplashScreenManager.CloseForm(false);
                
                if (IsNewRecord == true)
                {
                    if (Comon.cLong(Result) > 0)
                    {
                        IsNewRecord = false;
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                        txtInvoiceID.Text = Result.ToString();
                        if (falgPrint == true)
                            DoPrint();
                    

                    }
                    else
                    {
                        Messages.MsgError(Messages.TitleError, Messages.msgErrorSave + " " + Result);
                    }
                }
                else
                {

                    if (Comon.cLong(Result) >= 0)
                    {
                        //txtInvoiceID_Validating(null, null);
                        //EnabledControl(false);
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
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
        protected override void DoNew()
        {
            ClearFields();
            txtBarCode.Text = "";
            txtInvoiceID.Text = "";
            txtStoreID.Text = "";
            txtSupplierID.Text = "";
            txtStoreID_Validating(null, null);
            txtSupplierID_Validating(null, null);
            txtWeight.Text = "";

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
                    string BarcodeItem = txtBarCode.Text.ToString();
                    SplashScreenManager.CloseForm(false);
                    strSQL = "SELECT TOP 1 * FROM " + Sales_PurchaseDiamondDetailsDAL.TableName + "  Where  Cancel =0 And BranchID=" + Comon.cInt(cmbBranchesID.EditValue);
                    switch (Direction)
                    {
                        case xMoveFirst:
                            {
                                strSQL = strSQL + " ORDER BY " + Sales_PurchaseDiamondDetailsDAL.PremaryKey + " ASC";
                                break;
                            }
                        case xMoveNext:
                            {
                                strSQL = strSQL + " And " + Sales_PurchaseDiamondDetailsDAL.PremaryKey + ">" + PremaryKeyValue + " ORDER BY " + Sales_PurchaseDiamondDetailsDAL.PremaryKey + " asc";
                                break;
                            }
                        case xMovePrev:
                            {
                                strSQL = strSQL + " And " + Sales_PurchaseDiamondDetailsDAL.PremaryKey + "<" + PremaryKeyValue + " ORDER BY " + Sales_PurchaseDiamondDetailsDAL.PremaryKey + " desc";
                                break;
                            }
                        case xMoveLast:
                            {
                                strSQL = strSQL + " ORDER BY " + Sales_PurchaseDiamondDetailsDAL.PremaryKey + " DESC";
                                break;
                            }
                    }
                    dt = Lip.SelectRecord(strSQL);
             
                    if (dt.Rows.Count>0)
                    {
                                   
                        txtInvoiceID.Text = dt.Rows[0]["InvoiceID"].ToString();
                        txtBarCode.Text = dt.Rows[0]["BarCodeItem"].ToString();
                        cmbBranchesID.EditValue = Comon.cInt(dt.Rows[0]["BranchID"].ToString());
                        ReadRecord(1, Comon.cInt(dt.Rows[0]["TypeOpration"]));
                        
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

                Sales_PurchaseDiamondDetails model = new Sales_PurchaseDiamondDetails();
                model.InvoiceID = Comon.cInt(txtInvoiceID.Text);
             
                model.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                model.FacilityID = UserInfo.FacilityID;
                model.BarCodeItem = txtBarCode.Text;

                int Result = Sales_PurchaseDiamondDetailsDAL.Delete(model);
                 
                SplashScreenManager.CloseForm(false);
                if (Comon.cInt(Result) > 0)
                {
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);
                    ClearFields();
                    txtInvoiceID.Text = model.InvoiceID.ToString();
                    //MoveRec(model.InvoiceID, xMovePrev);
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
        protected override void DoEdit()
        {
            DataTable dtItem = new DataTable();
          
            dtItem.Columns.Add("FacilityID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("ItemID", System.Type.GetType("System.String"));
    
         
            dtItem.Columns.Add("StoreID", System.Type.GetType("System.String"));
      
   
        
            dtItem.Columns.Add("Cancel", System.Type.GetType("System.String"));
            dtItem.Columns.Add("BarCode", System.Type.GetType("System.String"));
            dtItem.Columns.Add(ItemName, System.Type.GetType("System.String"));

            dtItem.Columns.Add("BarCodeItem", System.Type.GetType("System.String"));
            dtItem.Columns.Add("BranchID", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("InvoiceID", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("WeightIn", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("WeightOut", System.Type.GetType("System.Decimal"));

            dtItem.Columns.Add("CaptionOpration", System.Type.GetType("System.String"));
            
            dtItem.Columns.Add("TypeOpration", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("PriceCarat", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("TotalPrice", System.Type.GetType("System.Decimal"));
               for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    dtItem.Rows.Add();
                 
                    dtItem.Rows[i]["FacilityID"] = UserInfo.FacilityID; 
                    dtItem.Rows[i]["BarCode"] = gridView1.GetRowCellValue(i, "BarCode").ToString();
                    dtItem.Rows[i]["ItemID"] = Comon.cInt(gridView1.GetRowCellValue(i, "ItemID").ToString());            
                    dtItem.Rows[i][ItemName] = gridView1.GetRowCellValue(i, ItemName).ToString();
                    dtItem.Rows[i]["CaptionOpration"] = gridView1.GetRowCellValue(i, "CaptionOpration").ToString();
                    dtItem.Rows[i]["WeightIn"] = Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(i, "WeightIn").ToString());
                    dtItem.Rows[i]["WeightOut"] = Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(i, "WeightOut").ToString());
                    dtItem.Rows[i]["TypeOpration"] = Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(i, "TypeOpration").ToString());

                    dtItem.Rows[i]["BarCodeItem"] = gridView1.GetRowCellValue(i, "BarCodeItem").ToString();
                    
                    dtItem.Rows[i]["StoreID"] = Comon.cInt(gridView1.GetRowCellValue(i, "StoreID").ToString());                             
                    dtItem.Rows[i]["PriceCarat"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "PriceCarat").ToString());
                    dtItem.Rows[i]["TotalPrice"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "TotalPrice").ToString());
                   
                    dtItem.Rows[i]["Cancel"] = 0;
                }
                gridControl.DataSource = dtItem;

                gridView1.AddNewRow();

                Validations.DoEditRipon(this, ribbonControl1);
            
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

            int length = gridView1.DataRowCount - 1; 
            for (int i = 0; i <=length; i++)
            {
                foreach (GridColumn col in gridView1.Columns)
                {
                    if (col.FieldName == "BarCode"  || col.FieldName == "WeightIn" || col.FieldName == "WeightOut" || col.FieldName == "PriceCarat" || col.FieldName == "ItemID" || col.FieldName == "TotalPrice")
                    {

                        var cellValue = gridView1.GetRowCellValue(i, col);
                        if ((col.FieldName == "WeightIn") && Comon.cInt(lblTypeOpration.Text) == 6)
                            return true;
                        if (col.FieldName == "WeightOut" && Comon.cInt(lblTypeOpration.Text) != 6)
                            return true;

                        if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                        {
                            gridView1.SetColumnError(col, Messages.msgInputIsRequired);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                        if (col.FieldName == "BarCode" )
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
                }
            }
            return true;
        }
        private void txtSupplierID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string strSql;
                DataTable dt;
                if (txtSupplierID.Text != string.Empty && txtSupplierID.Text != "0")
                {
                    strSQL = "SELECT " + PrimaryName + " as CustomerName ,VATID,Mobile FROM Sales_CustomerAnSublierListArb Where    AcountID =" + txtSupplierID.Text;
                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, UserInfo.Language.ToString());

                    dt = Lip.SelectRecord(strSQL);
                    if (dt.Rows.Count > 0)
                    {
                        lblSupplierName.Text = dt.Rows[0]["CustomerName"].ToString();
                       
                    }
                    else
                    {
                        strSql = "SELECT " + PrimaryName + " as CustomerName,SpecialDiscount,VATID, CustomerID,Mobile   FROM Sales_Customers Where  Cancel =0 And  AccountID =" + txtSupplierID.Text;
                        Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSql, UserInfo.Language.ToString());
                        dt = Lip.SelectRecord(strSql);
                        if (dt.Rows.Count > 0)
                        {
                            
                            lblSupplierName.Text = dt.Rows[0]["CustomerName"].ToString();
                           
                        }
                        else
                        {
                            lblSupplierName.Text = "";
                            txtSupplierID.Text = "";
                          
                        }
                    }
                }
                else
                {
                    lblSupplierName.Text = "";
                    txtSupplierID.Text = "";
                  
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
                strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(txtStoreID.Text) + " And Cancel =0";
                CSearch.ControlValidating(txtStoreID, lblStoreName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        protected override void Find()
        {

            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = "Where 1=1";

            FocusedControl = GetIndexFocusedControl();

            if (FocusedControl == null) return;


            if (FocusedControl.Trim() == txtSupplierID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtSupplierID, lblSupplierName, "SublierID", "رقم المـــورد", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtSupplierID, lblSupplierName, "SublierID", "SublierID ID", MySession.GlobalBranchID);
            }

            else if (FocusedControl.Trim() == txtStoreID.Name)
            {
                if (!MySession.GlobalAllowChangefrmPurchaseStoreID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "رقم الـمـســتـودع", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "Store ID", MySession.GlobalBranchID);
            }
            
             
            else if (FocusedControl.Trim() == gridControl.Name)
            {
                if (gridView1.FocusedColumn == null) return;

                if (gridView1.FocusedColumn.Name == "colBarCode")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeDimond", "البـاركـود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeDimond", "BarCode", MySession.GlobalBranchID);
                }
               
            }

           
            GetSelectedSearchValue(cls);
        }
        public void GetSelectedSearchValue(CSearch cls)
        {
          
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {
              
                if (FocusedControl == txtSupplierID.Name)
                {
                    txtSupplierID.Text = cls.PrimaryKeyValue.ToString();
                    txtSupplierID_Validating(null, null);
                }

                else if (FocusedControl == txtStoreID.Name)
                {
                    txtStoreID.Text = cls.PrimaryKeyValue.ToString();
                    txtStoreID_Validating(null, null);
                }

               
                
                else if (FocusedControl == txtInvoiceID.Name)
                {
                    txtInvoiceID.Text = cls.PrimaryKeyValue.ToString();
                   // txtInvoiceID_Validating(null, null);
                }


                
                else if (FocusedControl == gridControl.Name)
                {
                    if (gridView1.FocusedColumn.Name == "colBarCode")
                    {
                        string Barcode = cls.PrimaryKeyValue.ToString();


                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"], Barcode);
                        DataTable dt = Lip.SelectRecord("select * from Stc_DiamondItemsType where BarCodeDimond='" + Barcode+"'");
                        FileItemData(dt);
                        
                        
                    }
                    
                } 
            }
        }
        public void ReadData(string invoiceID,string BarCode,string Weight,string StoreID,string supplierID,int BranchID)
        {
            txtInvoiceID.Text = invoiceID;
            txtBarCode.Text = BarCode;
            txtWeight.Text = Weight;
            txtSupplierID.Text = supplierID;
            txtSupplierID_Validating(null, null);
            txtStoreID.Text = StoreID;
            txtStoreID_Validating(null, null);
            IsNewRecord = true;
            cmbBranchesID.EditValue = BranchID;

        }

        private void FileItemData(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {


                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"], dt.Rows[0]["BarCodeDimond"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCodeItem"],txtBarCode.Text.ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["StoreID"],txtStoreID.Text.ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["InvoiceID"],txtInvoiceID.Text.ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"], dt.Rows[0]["ItemID"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName], dt.Rows[0]["ArbName"].ToString());

                if (Comon.cInt(lblTypeOpration.Text) == 1)
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CaptionOpration"], "فاتورة مشتريات");
                else
                    if (Comon.cInt(lblTypeOpration.Text) == 5)
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CaptionOpration"], "بضاعة أول المدة");
                if (Comon.cInt(lblTypeOpration.Text) == 6)
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CaptionOpration"], "سند صرف");
                if (Comon.cInt(lblTypeOpration.Text) == 7)
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CaptionOpration"], "سند أمانات ");
                if (Comon.cInt(lblTypeOpration.Text) == 8)
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CaptionOpration"], "مردود سند أمانات ");
                if (Comon.cInt(lblTypeOpration.Text) == 9)
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CaptionOpration"], "توريد مخزني ");
                gridView1.AddNewRow();

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
    }
}
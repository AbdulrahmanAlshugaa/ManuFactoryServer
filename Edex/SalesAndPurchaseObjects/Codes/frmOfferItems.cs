using DevExpress.XtraEditors;
using Edex.DAL;
using Edex.DAL.Stc_itemDAL;
using Edex.Model;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralClasses;
using Edex.GeneralObjects.GeneralForms;
using Edex.ModelSystem;
using Edex.SalesAndPurchaseObjects.SalesClasses;
using Edex.StockObjects.StoresClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace Edex.StockObjects.Codes
{
    public partial class frmOfferItems : Edex.GeneralObjects.GeneralForms.BaseForm
    {


        /**************** Declare ************************/
        #region Declare

        private string strSQL;
        private bool IsNewRecord;

        private cPriceItemsOffers cClass = new cPriceItemsOffers();
        public bool IsFromanotherForms = false;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;

        #endregion
        /****************Form Event************************/
        #region Form Event
        private void InitializeFormatDate(DateEdit Obj)
        {
            Obj.Properties.Mask.UseMaskAsDisplayFormat = true;
            Obj.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            Obj.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            Obj.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            Obj.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            Obj.Properties.Mask.EditMask = "dd/MM/yyyy";
            Obj.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;

        }
        public frmOfferItems()
        {
            InitializeComponent();

            ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;
            InitializeFormatDate(txtFromDate);
            InitializeFormatDate(txtToDate);
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("NO", typeof(int)));
            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            DataRow row;
            row = dt.NewRow();
            row["NO"] = 0;
            row["Name"] = "---";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["NO"] = 1;
            row["Name"] = "خصم نسبة";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["NO"] = 2;
            row["Name"] = "خصم قيمة";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["NO"] = 3;
            row["Name"] = "عروض خاصة ";
            dt.Rows.Add(row);


            cmbOfferType.Properties.DataSource = dt.DefaultView;
            cmbOfferType.Properties.DisplayMember = "Name";
            cmbOfferType.Properties.ValueMember = "NO";

            cmbOfferType.Properties.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;



            this.GridView.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.GridView_RowClick);
            this.GridView.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.GridView_FocusedRowChanged);
            this.txtOfferID.Validating += new System.ComponentModel.CancelEventHandler(this.txtGroupID_Validating);
            this.txtOfferID.EditValueChanged += new System.EventHandler(this.txtGroupID_EditValueChanged);
            this.txtDescription.EditValueChanged += new System.EventHandler(this.txtArbName_EditValueChanged);
            this.txtDescription.Validating += new System.ComponentModel.CancelEventHandler(this.txtArbName_Validating);

            DoNew();



          
           

        }
        private void frmItemsGroups_Load(object sender, EventArgs e)
        {
            FillGrid();
        }
        #endregion
        /**********************Function**************************/
        #region Function
        public void FillGrid()
        {

            strSQL = "SELECT  " + cClass.PremaryKey + " as الرقم, Description as [اسم المجموعة] FROM " + cClass.TableName + " ";

            if (UserInfo.Language == iLanguage.English)
                strSQL = "SELECT  " + cClass.PremaryKey + " as ID, Description as [Group Name] FROM " + cClass.TableName + "  ";

            DataTable dt = new DataTable();
            dt = Lip.SelectRecord(strSQL);
            GridView.GridControl.DataSource = dt;

            GridView.Columns[0].Width = 50;
            GridView.Columns[1].Width = 100;

        }
        public void Find()
        {
            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };

            cls.SQLStr = "SELECT  " + cClass.PremaryKey + " as الرقم, ArbName as [اسم المجموعة] FROM " + cClass.TableName + " WHERE Cancel =0  ";

            if (UserInfo.Language == iLanguage.English)
                cls.SQLStr = "SELECT  " + cClass.PremaryKey + " as ID, EngName as [Group Name] FROM " + cClass.TableName + " WHERE Cancel =0  ";

            ColumnWidth = new int[] { 50, 300 };

            if (cls.SQLStr != "")
            {
                frmSearch frm = new frmSearch();
                cls.strFilter = "الرقم";
                if (UserInfo.Language == iLanguage.English)
                    cls.strFilter = "ID";

                frm.AddSearchData(cls);
                frm.ColumnWidth = ColumnWidth;
                frm.ShowDialog();
                GetSelectedSearchValue(cls);
            }
        }
        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {

                //if (this.ActiveControl.Name == txtGroupID.Name)
                //{
                txtOfferID.Text = cls.PrimaryKeyValue.ToString();
                txtGroupID_Validating(null, null);
                //}
            }

        }
        public void ReadRecord()
        {
            try
            {
                IsNewRecord = false;
                ClearFields();
                {
                    txtOfferID.Text = cClass.OfferID.ToString();

                    txtDescription.Text = cClass.Description.ToString();
                    txtFromDate.DateTime = DateTime.ParseExact(Comon.ConvertSerialDateTo(cClass.FromDate.ToString()), "dd/MM/yyyy", new CultureInfo("en-US"));

                    txtToDate.DateTime = DateTime.ParseExact(Comon.ConvertSerialDateTo(cClass.ToDate.ToString()), "dd/MM/yyyy", new CultureInfo("en-US"));

                    txtFromGroupID.Text = cClass.FromGroupID.ToString();
                    txtToGroupID.Text = cClass.ToGroupID.ToString();
                    txtFromItemID.Text = cClass.FromItemID.ToString();

                    txtFromItemID_Validating(null, null);
                    txtFromItemID1.Text = cClass.FromItemID1.ToString();
                    txtFromItemID1_Validating(null, null);

                    txtToItemID.Text = cClass.ToItemID.ToString();
                    txtFromSizeID.Text = cClass.FromSizeID.ToString();
                    txtToSizeID.Text = cClass.ToSizeID.ToString();

                    txtToItemID1.Text = cClass.ToItemID1.ToString();
                    txtFromSizeID1.Text = cClass.FromSizeID1.ToString();
                    txtToSizeID1.Text = cClass.ToSizeID1.ToString();


                    if (cClass.IsPercent > 0)
                        cmbOfferType.EditValue = 1;
                    else if (cClass.IsAmount > 0)
                        cmbOfferType.EditValue = 2;
                    else if (cClass.IsOffers > 0)
                        cmbOfferType.EditValue = 3;
                    else cmbOfferType.EditValue = 0;
                    cmbOfferType_EditValueChanged(null, null);
                    if (cClass.IsTakeOne > 0)
                        IsTakeOne.Checked = true;
                    else if (cClass.IsGetSame > 0)
                        IsGetSame.Checked = true;
                    else if (cClass.IsGetOnther > 0)
                        IsGetOnther.Checked = true;

                    if (cClass.ISRepeat>0)
                        chkISRepeat.Checked = true;
                    else chkISRepeat.Checked = false;

                 


                    txtPercentCost.Text = cClass.PercentCost.ToString();
                    txtAmountCost.Text = cClass.AmountCost.ToString();
                    GetSameAmount.Text = cClass.GetSameAmount.ToString();
                    SetSameAmount.Text = cClass.SetSameAmount.ToString();
                    GetOntherAmount.Text = cClass.GetOntherAmount.ToString();
                    SetOntherAmount.Text = cClass.SetOntherAmount.ToString();
                    txtBarCode.Text = cClass.BarCode.ToString();
                    txtOldBarcodeID_Validating(null, null);
                    ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Item.Caption = txtOfferID.Text;
                   

                  
                    txtFromGroupID_Validating(null, null);
                    txtToGroupID_Validating(null, null);
                  
                    txtToItemID_Validating(null, null);
                    txtFromSizeID_Validating(null, null);
                    txtToSizeID_Validating(null, null);
                    txtToItemID1_Validating(null, null);
                    txtFromSizeID1_Validating(null, null);
                    txtToSizeID1_Validating(null, null);

                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        public void ClearFields()
        {
            try
            {
                txtOfferID.Text = cClass.GetNewID().ToString();

                txtDescription.Text = "";
                txtFromDate.Text = "";
                txtToDate.Text = "";
                txtFromGroupID.Text = "";
                txtToGroupID.Text = "";
                txtFromItemID.Text = "";
                txtToItemID.Text = "";
                txtFromSizeID.Text = "";
                txtToSizeID.Text = "";
                lblFromSizeID.Text = "";
                lblToSizeID.Text = "";
                lblFromGroupID.Text = "";
                lblToGroupID.Text = "";
                lblFromItemID.Text = "";
                lblToItemID.Text = "";
                txtPercentCost.Text = "";
                txtAmountCost.Text = "";
                GetSameAmount.Text = "";
                SetSameAmount.Text = "";
                GetOntherAmount.Text = "";
                SetOntherAmount.Text = "";
                txtBarCode.Text = "";
                txtOldBarcodeID_Validating(null, null);
                txtFromSizeID.Text = "";
                txtToSizeID.Text = "";
                ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Item.Caption = txtOfferID.Text;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        /******************** MoveRec ************************/
        public void MoveRec(long PremaryKeyValue, int Direction)
        {
            try
            {
                #region If
                if (FormView == true)
                {
                    strSQL = "SELECT TOP 1 * FROM " + cClass.TableName + " Where Cancel =0 ";
                    switch (Direction)
                    {
                        case xMoveFirst:
                            {
                                strSQL = strSQL + " ORDER BY " + cClass.PremaryKey + " ASC";
                                break;
                            }

                        case xMoveNext:
                            {
                                strSQL = strSQL + " And " + cClass.PremaryKey + ">" + PremaryKeyValue + " ORDER BY " + cClass.PremaryKey + " asc";
                                break;
                            }

                        case xMovePrev:
                            {
                                strSQL = strSQL + " And " + cClass.PremaryKey + "<" + PremaryKeyValue + " ORDER BY " + cClass.PremaryKey + " desc";
                                break;
                            }

                        case xMoveLast:
                            {
                                strSQL = strSQL + " ORDER BY " + cClass.PremaryKey + " DESC";
                                break;
                            }
                    }

                    cClass.GetRecordSetBySQL(strSQL);
                    if (cClass.FoundResult == true)
                        ReadRecord();
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
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        /*******************Do Functions *************************/
        protected override void DoNew()
        {
            try
            {
                IsNewRecord = true;
                ClearFields();
                txtDescription.Focus();

            }
            catch (Exception ex)
            {
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
                MoveRec(Comon.cInt(txtOfferID.Text), xMoveNext);


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
                MoveRec(Comon.cInt(txtOfferID.Text), xMovePrev);
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
                Find();
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        protected override void DoSave()
        {
            try
            {

                if (IsNewRecord && !FormAdd)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToAddNewRecord);
                    return;

                }
                if (!IsNewRecord)
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
                if (!Validations.IsValidForm(this))
                    return;
                save();


            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public void save()
        {

            PriceItemsOffers model = new PriceItemsOffers();

            model.OfferID = Comon.cInt(txtOfferID.Text);
            if (IsNewRecord == true)
                model.OfferID = 0;
            model.OrderType = 0;
            model.Description = txtDescription.Text;
            model.FromDate = Comon.ConvertDateToSerial(txtFromDate.Text);
            model.ToDate = Comon.ConvertDateToSerial(txtToDate.Text);
            model.FromItemID = Comon.cInt(txtFromItemID.Text);
            model.ToItemID = Comon.cInt(txtToItemID.Text);
            model.FromSizeID = Comon.cInt(txtFromSizeID.Text);
            model.ToSizeID = Comon.cInt(txtToSizeID.Text);
            model.FromItemID1 = Comon.cInt(txtFromItemID1.Text);
            model.ToItemID1 = Comon.cInt(txtToItemID1.Text);
            model.FromSizeID1 = Comon.cInt(txtFromSizeID1.Text);
            model.ToSizeID1 = Comon.cInt(txtToSizeID1.Text);
            model.FromGroupID = Comon.cInt(txtFromGroupID.Text);
            model.ToGroupID = Comon.cInt(txtToGroupID.Text);
            if (chkISRepeat.Checked == true)
                model.ISRepeat = 1;
            else
                model.ISRepeat = 0;
              
            model.QTY = 0;
            model.SetOntherAmount = 0;
            model.GetOntherAmount = 0;
            model.GetSameAmount = 0;
            model.SetSameAmount = 0;
            model.IsAmount = 0;
            model.AmountCost = 0;
            model.IsPercent = 0;
            model.PercentCost = 0;
            model.IsOffers = 0;
            model.IsTakeOne = 0;
            model.IsGetSame = 0;
            model.IsGetOnther = 0;
            switch (Comon.cInt(cmbOfferType.EditValue))
            {
                case (0): return;
                case (1):
                    model.IsPercent = 1;
                    model.PercentCost = Comon.cLong(txtPercentCost.Text);
                    break;
                case (2):
                    model.IsAmount = 1;
                    model.AmountCost = Comon.cLong(txtAmountCost.Text);
                    break;
                case (3):
                    model.IsOffers = 1;
                    if (IsTakeOne.Checked == true)
                    {
                        model.IsTakeOne = 1;
                        break;
                    }
                    else if (IsGetSame.Checked == true)
                    {
                        model.IsGetSame = 1;
                        model.GetSameAmount = Comon.cLong(GetSameAmount.Text);
                        model.SetSameAmount = Comon.cLong(SetSameAmount.Text);
                        break;
                    }
                    else if (IsGetOnther.Checked == true)
                    {
                        model.IsGetOnther = 1;
                        model.GetOntherAmount = Comon.cLong(GetOntherAmount.Text);
                        model.SetOntherAmount = Comon.cLong(SetOntherAmount.Text);
                        model.BarCode = txtBarCode.Text;
                        break;
                    }
                    else { return; }



            }




            int StoreID;

            StoreID = HR_District_DAL.InsertStc_PriceOffers(model);





            if (true)
            {
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                if (IsNewRecord == true)
                    DoNew();

                FillGrid();
            }


        }
        protected override void DoPrint()
        {

            try
            {
                GridView.ShowRibbonPrintPreview();

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }

        protected override void DoDelete()
        {
            try
            {

                //int isTRans = Comon.cInt(Lip.GetValue(" select dbo.[GroupItemID](" + Comon.cInt(txtGroupID.Text) + ")"));
                //if (isTRans > 0)
                //{
                //    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToUpdateRecord);
                //    return;
                //}
                if (!FormDelete)
                {

                    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToUpdateRecord);
                    return;
                }
                else
                {
                    bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, Messages.msgConfirmDelete);
                    if (!Yes)
                        return;
                }


                PriceItemsOffers model = new PriceItemsOffers();



                model.OfferID = Comon.cInt(txtOfferID.Text);
            

           
                int StoreID;
                

                bool Result = HR_District_DAL.DeleteStc_Stores(model);
                if (Result == true)
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);
                MoveRec(model.OfferID, xMovePrev);

                FillGrid();

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        #endregion
        /**********************Event**************************/
        #region Event
        private void txtGroupID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string TempUserID;
                if (int.Parse(txtOfferID.Text.Trim()) > 0)
                {
                    cClass.GetRecordSet(Comon.cInt(txtOfferID.Text));
                    TempUserID = txtOfferID.Text;
                    ClearFields();
                    txtOfferID.Text = TempUserID;
                    if (cClass.FoundResult == true)
                    {
                        if (FormView == true)
                            ReadRecord();
                        else
                        {
                            Messages.MsgStop(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                        }
                    }
                    else if (FormAdd == true)
                        IsNewRecord = true;
                    else
                        return;
                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void txtGroupID_EditValueChanged(object sender, EventArgs e)
        {
            ((TextEdit)sender).Properties.Appearance.BorderColor = Color.Black;
        }
        private void txtArbName_EditValueChanged(object sender, EventArgs e)
        {
            ((TextEdit)sender).Properties.Appearance.BorderColor = Color.Black;
        }
        private void GridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                int rowIndex = e.FocusedRowHandle;

                txtOfferID.Text = GridView.GetRowCellValue(rowIndex, GridView.Columns[0].FieldName).ToString();
                txtGroupID_Validating(null, null);

            }
            catch (Exception)
            {
                return;
            }

        }
        private void GridView_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            int rowIndex = e.RowHandle;
            txtOfferID.Text = GridView.GetRowCellValue(rowIndex, GridView.Columns[0].FieldName).ToString();
            txtGroupID_Validating(null, null);
        }



        private void txtArbName_Validating(object sender, CancelEventArgs e)
        {
            TextEdit obj = (TextEdit)sender;
            //if (UserInfo.Language == iLanguage.Arabic)
            //txtEngName.Text = Translator.ConvertNameToOtherLanguage(obj.Text.Trim().ToString(), UserInfo.Language);
        }
        private void txtEngName_Validating(object sender, CancelEventArgs e)
        {
            TextEdit obj = (TextEdit)sender;

            if (UserInfo.Language == iLanguage.English)
                txtDescription.Text = Translator.ConvertNameToOtherLanguage(obj.Text.Trim().ToString(), UserInfo.Language);
        }
        #endregion

        private void txtTransCost_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbOfferType_EditValueChanged(object sender, EventArgs e)
        {


            pnlPercent.Visible = false;
            pnlAmount.Visible = false;
            pnlOffersItems.Visible = false;
            //txtPercentCost.Text = "";
            //txtAmountCost.Text = "";
            //GetSameAmount.Text = "";
            //SetSameAmount.Text = "";
            //GetOntherAmount.Text = "";
            //SetOntherAmount.Text = "";
            //txtBarCode.Text = "";
            //txtOldBarcodeID_Validating(null, null);
            switch (Comon.cInt(cmbOfferType.EditValue))
            {

                case (1): pnlPercent.Visible = true
                    
              ; break;

                case (2): pnlAmount.Visible = true; break;


                case (3): pnlOffersItems.Visible = true; IsTakeOne.Checked = true;IsTakeOne_CheckedChanged(null,null) ;break;




            }
        }

        private void IsGetSame_CheckedChanged(object sender, EventArgs e)
        {
            if (IsGetSame.Checked == true)
                pnlSameItem.Enabled = true;
            else
                pnlSameItem.Enabled = false;



        }

        private void IsGetOnther_CheckedChanged(object sender, EventArgs e)
        {
            if (IsGetOnther.Checked == true)
                pnlOntherItem.Enabled = true;
            else
                pnlOntherItem.Enabled = false;

        }

        private void txtOldBarcodeID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                //   strSQL = "SELECT ArbName as ItemName FROM Stc_ItemsUnit WHERE SellerID=" + txtBarCode.Text + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
                // CSearch.ControlValidating(txtBarCode, lblBarCodeName, strSQL);
                CSearch.ControlValidating(txtBarCode, lblBarCodeName, GetItemData(txtBarCode.Text));


            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public string GetItemData(string barcode)
        {


            var getItemSQL = " SELECT   TOP (1)  Stc_Items.ArbName AS ItemName  "

             + "   FROM  Stc_Items    "
             + "  RIGHT OUTER JOIN   Sales_PurchaseInvoiceDetails "
             + "  ON Stc_Items.ItemID = Sales_PurchaseInvoiceDetails.ItemID "
             + "  WHERE  (Sales_PurchaseInvoiceDetails.BarCode ='" + barcode + "') AND (Sales_PurchaseInvoiceDetails.Cancel = 0)";

            return Lip.ConvertStrSQLLanguage(getItemSQL, iLanguage.English.ToString());



        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtFromGroupID_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.F3){
            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };

            cls.SQLStr = "SELECT GroupID    as الرقم, ArbName as [اسم المجموعة] FROM Stc_ItemsGroups WHERE Cancel =0  and Notes<>'INS'  ";

            if (UserInfo.Language == iLanguage.English)
                cls.SQLStr = "SELECT GroupID   as ID, EngName as [Group Name] FROM Stc_ItemsGroups  WHERE Cancel =0  and Notes<>'INS' ";

            ColumnWidth = new int[] { 50, 300 };

            if (cls.SQLStr != "")
            {
                frmSearch frm = new frmSearch();
                cls.strFilter = "الرقم";
                if (UserInfo.Language == iLanguage.English)
                    cls.strFilter = "ID";

                frm.AddSearchData(cls);
                frm.ColumnWidth = ColumnWidth;
                frm.ShowDialog();
                if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
                {


                    txtFromGroupID.Text = cls.PrimaryKeyValue.ToString();
                    txtToGroupID.Text = cls.PrimaryKeyValue.ToString();
                    var sr = "SELECT ArbName  FROM Stc_ItemsGroups WHERE Cancel =0    and Notes<>'INS'  and GroupID=" + Comon.cInt(cls.PrimaryKeyValue.ToString());
                    var dr = Lip.SelectRecord(sr);
                    if (dr.Rows.Count > 0)
                    {
                        lblFromGroupID.Text = dr.Rows[0][0].ToString();

                        lblToGroupID.Text = dr.Rows[0][0].ToString();
                    }
                    else
                    {
                        lblFromGroupID.Text = "";
                        txtFromGroupID.Text = "";
                        lblToGroupID.Text = "";
                        txtToGroupID.Text = "";

                    }
                }
            }
            }
        }

        private void txtToGroupID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                CSearch cls = new CSearch();
                int[] ColumnWidth = new int[] { 100, 300 };

                cls.SQLStr = "SELECT GroupID    as الرقم, ArbName as [اسم المجموعة] FROM Stc_ItemsGroups WHERE Cancel =0 and Notes<>'INS'  ";

                if (UserInfo.Language == iLanguage.English)
                    cls.SQLStr = "SELECT GroupID   as ID, EngName as [Group Name] FROM Stc_ItemsGroups  WHERE Cancel =0 and Notes<>'INS'  ";

                ColumnWidth = new int[] { 50, 300 };

                if (cls.SQLStr != "")
                {
                    frmSearch frm = new frmSearch();
                    cls.strFilter = "الرقم";
                    if (UserInfo.Language == iLanguage.English)
                        cls.strFilter = "ID";

                    frm.AddSearchData(cls);
                    frm.ColumnWidth = ColumnWidth;
                    frm.ShowDialog();
                    if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
                    {


                        txtToGroupID.Text = cls.PrimaryKeyValue.ToString();

                        var sr = "SELECT ArbName  FROM Stc_ItemsGroups WHERE Cancel =0  and Notes<>'INS'  and GroupID=" + Comon.cInt(cls.PrimaryKeyValue.ToString());
                        var dr = Lip.SelectRecord(sr);
                        if (dr.Rows.Count > 0)
                            lblToGroupID.Text = dr.Rows[0][0].ToString();
                        else
                        {
                            lblToGroupID.Text = "";
                            txtToGroupID.Text = "";

                        }
                    }
                }
            }
        }

        private void txtFromItemID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                CSearch cls = new CSearch();
                int[] ColumnWidth = new int[] { 100, 300 };

                cls.SQLStr = "SELECT ItemID    as الرقم, ArbName as [اسم المادة] FROM Stc_Items WHERE Cancel =0   ";

                if (UserInfo.Language == iLanguage.English)
                    cls.SQLStr = "SELECT ItemID   as ID, EngName as [Item Name] FROM Stc_Items  WHERE Cancel =0  ";

                if (Comon.cInt(txtFromGroupID.Text.ToString()) > 0)
                    cls.SQLStr =cls.SQLStr + "   and GroupID >= " + Comon.cInt(txtFromGroupID.Text.ToString());
                if (Comon.cInt(txtToGroupID.Text.ToString()) > 0)
                    cls.SQLStr = cls.SQLStr +"    and GroupID <= " + Comon.cInt(txtToGroupID.Text.ToString());


                ColumnWidth = new int[] { 50, 300 };

                if (cls.SQLStr != "")
                {
                    frmSearch frm = new frmSearch();
                    cls.strFilter = "الرقم";
                    if (UserInfo.Language == iLanguage.English)
                        cls.strFilter = "ID";

                    frm.AddSearchData(cls);
                    frm.ColumnWidth = ColumnWidth;
                    frm.ShowDialog();
                    if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
                    {


                        txtFromItemID.Text = cls.PrimaryKeyValue.ToString();
                        txtToItemID.Text = cls.PrimaryKeyValue.ToString();
                        var sr = "SELECT ArbName  FROM Stc_Items  WHERE Cancel =0  and ItemID=" + Comon.cInt(cls.PrimaryKeyValue.ToString());
                        var dr = Lip.SelectRecord(sr);
                        if (dr.Rows.Count > 0)
                        {
                            lblFromItemID.Text = dr.Rows[0][0].ToString();
                            lblToItemID.Text = dr.Rows[0][0].ToString();

                        }
                        else
                        {
                            lblFromItemID.Text = "";
                            txtFromItemID.Text = "";
                            lblToItemID.Text = "";
                            txtToItemID.Text = "";

                        }
                    }
                }
            }
        }

        private void txtToItemID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                CSearch cls = new CSearch();
                int[] ColumnWidth = new int[] { 100, 300 };

                cls.SQLStr = "SELECT ItemID    as الرقم, ArbName as [اسم المادة] FROM Stc_Items WHERE Cancel =0  ";

                if (UserInfo.Language == iLanguage.English)
                    cls.SQLStr = "SELECT ItemID   as ID, EngName as [Item Name] FROM Stc_Items  WHERE Cancel =0  ";

                ColumnWidth = new int[] { 50, 300 };

                if (cls.SQLStr != "")
                {
                    frmSearch frm = new frmSearch();
                    cls.strFilter = "الرقم";
                    if (UserInfo.Language == iLanguage.English)
                        cls.strFilter = "ID";

                    frm.AddSearchData(cls);
                    frm.ColumnWidth = ColumnWidth;
                    frm.ShowDialog();
                    if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
                    {


                        txtToItemID.Text = cls.PrimaryKeyValue.ToString();

                        var sr = "SELECT ArbName  FROM Stc_Items  WHERE Cancel =0  and ItemID=" + Comon.cInt(cls.PrimaryKeyValue.ToString());
                        var dr = Lip.SelectRecord(sr);
                        if (dr.Rows.Count > 0)
                            lblToItemID.Text = dr.Rows[0][0].ToString();
                        else
                        {
                            lblToItemID.Text = "";
                            txtToItemID.Text = "";

                        }
                    }
                }
            }
        }

        private void txtFromSizeID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                CSearch cls = new CSearch();
                int[] ColumnWidth = new int[] { 100, 300 };

                cls.SQLStr = "SELECT SizeID    as الرقم, ArbName as [اسم الوحدة] FROM Stc_SizingUnits WHERE Cancel =0  and Notes<>'0' ";

                if (UserInfo.Language == iLanguage.English)
                    cls.SQLStr = "SELECT SizeID   as ID, EngName as [Size Name] FROM Stc_SizingUnits  WHERE Cancel =0  and Notes<>'0' ";

                ColumnWidth = new int[] { 50, 300 };

                if (cls.SQLStr != "")
                {
                    frmSearch frm = new frmSearch();
                    cls.strFilter = "الرقم";
                    if (UserInfo.Language == iLanguage.English)
                        cls.strFilter = "ID";

                    frm.AddSearchData(cls);
                    frm.ColumnWidth = ColumnWidth;
                    frm.ShowDialog();
                    if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
                    {


                        txtFromSizeID.Text = cls.PrimaryKeyValue.ToString();
                        txtToSizeID.Text = cls.PrimaryKeyValue.ToString();
                        var sr = "SELECT ArbName  FROM Stc_SizingUnits WHERE Cancel =0  and Notes<>'0'  and SizeID=" + Comon.cInt(cls.PrimaryKeyValue.ToString());
                        var dr = Lip.SelectRecord(sr);
                        if (dr.Rows.Count > 0)
                        {
                            lblFromSizeID.Text = dr.Rows[0][0].ToString();
                            lblToSizeID.Text = dr.Rows[0][0].ToString();
                        }
                        else
                        {
                            lblFromSizeID.Text = "";
                            txtFromSizeID.Text = "";

                            lblToSizeID.Text = "";
                            txtToSizeID.Text = "";


                        }
                    }
                }
            }
        }

        private void txtToSizeID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                CSearch cls = new CSearch();
                int[] ColumnWidth = new int[] { 100, 300 };

                cls.SQLStr = "SELECT SizeID    as الرقم, ArbName as [اسم الوحدة] FROM Stc_SizingUnits WHERE Cancel =0  and Notes<>'0' ";

                if (UserInfo.Language == iLanguage.English)
                    cls.SQLStr = "SELECT SizeID   as ID, EngName as [Size Name] FROM Stc_SizingUnits  WHERE Cancel =0  and Notes<>'0' ";

                ColumnWidth = new int[] { 50, 300 };

                if (cls.SQLStr != "")
                {
                    frmSearch frm = new frmSearch();
                    cls.strFilter = "الرقم";
                    if (UserInfo.Language == iLanguage.English)
                        cls.strFilter = "ID";

                    frm.AddSearchData(cls);
                    frm.ColumnWidth = ColumnWidth;
                    frm.ShowDialog();
                    if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
                    {


                        txtToSizeID.Text = cls.PrimaryKeyValue.ToString();

                        var sr = "SELECT ArbName  FROM Stc_SizingUnits WHERE Cancel =0  and Notes<>'0'  and SizeID=" + Comon.cInt(cls.PrimaryKeyValue.ToString());
                        var dr = Lip.SelectRecord(sr);
                        if (dr.Rows.Count > 0)
                            lblToSizeID.Text = dr.Rows[0][0].ToString();
                        else
                        {
                            lblToSizeID.Text = "";
                            txtToSizeID.Text = "";

                        }
                    }
                }
            }
        }

        private void txtToGroupID_Validating(object sender, CancelEventArgs e)
        {
            var sr = "SELECT ArbName  FROM Stc_ItemsGroups WHERE Cancel =0  and Notes<>'INS'  and GroupID=" +Comon.cInt( txtToGroupID.Text.ToString());
            var dr = Lip.SelectRecord(sr);
            if (dr.Rows.Count > 0)
                lblToGroupID.Text = dr.Rows[0][0].ToString();
            else
            {
                lblToGroupID.Text = "";
                txtToGroupID.Text = "";

            }
        }

        private void txtFromGroupID_Validating(object sender, CancelEventArgs e)
        {

            var sr = "SELECT ArbName  FROM Stc_ItemsGroups WHERE Cancel =0    and Notes<>'INS'  and GroupID=" +Comon.cInt( txtFromGroupID.Text.ToString());
            var dr = Lip.SelectRecord(sr);
            if (dr.Rows.Count > 0)
                lblFromGroupID.Text = dr.Rows[0][0].ToString();
            else
            {
                lblFromGroupID.Text = "";
                txtFromGroupID.Text = "";

            }
        }

        private void txtFromItemID_Validating(object sender, CancelEventArgs e)
        {
            var sr = "SELECT ArbName  FROM Stc_Items  WHERE Cancel =0  and ItemID=" + Comon.cInt(txtFromItemID.Text.ToString());
            var dr = Lip.SelectRecord(sr);
            if (dr.Rows.Count > 0)
                lblFromItemID.Text = dr.Rows[0][0].ToString();
            else
            {
                lblFromItemID.Text = "";
                txtFromItemID.Text = "";

            }
        }

        private void txtToItemID_Validating(object sender, CancelEventArgs e)
        {
            var sr = "SELECT ArbName  FROM Stc_Items  WHERE Cancel =0  and ItemID=" + Comon.cInt(txtToItemID.Text.ToString());
            var dr = Lip.SelectRecord(sr);
            if (dr.Rows.Count > 0)
                lblToItemID.Text = dr.Rows[0][0].ToString();
            else
            {
                lblToItemID.Text = "";
                txtToItemID.Text = "";

            }
        }

        private void txtFromSizeID_Validating(object sender, CancelEventArgs e)
        {
            var sr = "SELECT ArbName  FROM Stc_SizingUnits WHERE Cancel =0  and Notes<>'0'  and SizeID=" + Comon.cInt(txtFromSizeID.Text.ToString());
            var dr = Lip.SelectRecord(sr);
            if (dr.Rows.Count > 0)
                lblFromSizeID.Text = dr.Rows[0][0].ToString();
            else
            {
                lblFromSizeID.Text = "";
                txtFromSizeID.Text = "";

            }
        }

        private void txtToSizeID_Validating(object sender, CancelEventArgs e)
        {
            var sr = "SELECT ArbName  FROM Stc_SizingUnits WHERE Cancel =0  and Notes<>'0'  and SizeID=" + Comon.cInt(txtToSizeID.Text.ToString());
            var dr = Lip.SelectRecord(sr);
            if (dr.Rows.Count > 0)
                lblToSizeID.Text = dr.Rows[0][0].ToString();
            else
            {
                lblToSizeID.Text = "";
                txtToSizeID.Text = "";

            }
        }





        private void txtFromItemID1_Validating(object sender, CancelEventArgs e)
        {
            var sr = "SELECT ArbName  FROM Stc_Items  WHERE Cancel =0  and ItemID=" + Comon.cInt(txtFromItemID1.Text.ToString());
            var dr = Lip.SelectRecord(sr);
            if (dr.Rows.Count > 0)
                lblFromItemID1.Text = dr.Rows[0][0].ToString();
            else
            {
                lblFromItemID1.Text = "";
                txtFromItemID1.Text = "";

            }
        }

        private void txtToItemID1_Validating(object sender, CancelEventArgs e)
        {
            var sr = "SELECT ArbName  FROM Stc_Items  WHERE Cancel =0  and ItemID=" + Comon.cInt(txtToItemID1.Text.ToString());
            var dr = Lip.SelectRecord(sr);
            if (dr.Rows.Count > 0)
                lblToItemID1.Text = dr.Rows[0][0].ToString();
            else
            {
                lblToItemID1.Text = "";
                txtToItemID1.Text = "";

            }
        }

        private void txtFromSizeID1_Validating(object sender, CancelEventArgs e)
        {
            var sr = "SELECT ArbName  FROM Stc_SizingUnits WHERE Cancel =0  and Notes<>'0'  and SizeID=" + Comon.cInt(txtFromSizeID1.Text.ToString());
            var dr = Lip.SelectRecord(sr);
            if (dr.Rows.Count > 0)
                lblFromSizeID1.Text = dr.Rows[0][0].ToString();
            else
            {
                lblFromSizeID1.Text = "";
                txtFromSizeID1.Text = "";

            }
        }

        private void txtToSizeID1_Validating(object sender, CancelEventArgs e)
        {
            var sr = "SELECT ArbName  FROM Stc_SizingUnits WHERE Cancel =0  and Notes<>'0'  and SizeID=" + Comon.cInt(txtToSizeID1.Text.ToString());
            var dr = Lip.SelectRecord(sr);
            if (dr.Rows.Count > 0)
                lblToSizeID1.Text = dr.Rows[0][0].ToString();
            else
            {
                lblToSizeID1.Text = "";
                txtToSizeID1.Text = "";

            }
        }


        private void txtFromItemID1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                CSearch cls = new CSearch();
                int[] ColumnWidth = new int[] { 100, 300 };

                cls.SQLStr = "SELECT ItemID    as الرقم, ArbName as [اسم المادة] FROM Stc_Items WHERE Cancel =0   ";

                if (UserInfo.Language == iLanguage.English)
                    cls.SQLStr = "SELECT ItemID   as ID, EngName as [Item Name] FROM Stc_Items  WHERE Cancel =0  ";

              


                ColumnWidth = new int[] { 50, 300 };

                if (cls.SQLStr != "")
                {
                    frmSearch frm = new frmSearch();
                    cls.strFilter = "الرقم";
                    if (UserInfo.Language == iLanguage.English)
                        cls.strFilter = "ID";

                    frm.AddSearchData(cls);
                    frm.ColumnWidth = ColumnWidth;
                    frm.ShowDialog();
                    if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
                    {


                        txtFromItemID1.Text = cls.PrimaryKeyValue.ToString();
                        txtToItemID1.Text = cls.PrimaryKeyValue.ToString();
                        var sr = "SELECT ArbName  FROM Stc_Items  WHERE Cancel =0  and ItemID=" + Comon.cInt(cls.PrimaryKeyValue.ToString());
                        var dr = Lip.SelectRecord(sr);
                        if (dr.Rows.Count > 0)
                        {
                            lblFromItemID1.Text = dr.Rows[0][0].ToString();
                            lblToItemID1.Text = dr.Rows[0][0].ToString();

                        }
                        else
                        {
                            lblFromItemID1.Text = "";
                            txtFromItemID1.Text = "";
                            lblToItemID1.Text = "";
                            txtToItemID1.Text = "";

                        }
                    }
                }
            }
        }

        private void txtToItemID1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                CSearch cls = new CSearch();
                int[] ColumnWidth = new int[] { 100, 300 };

                cls.SQLStr = "SELECT ItemID    as الرقم, ArbName as [اسم المادة] FROM Stc_Items WHERE Cancel =0  ";

                if (UserInfo.Language == iLanguage.English)
                    cls.SQLStr = "SELECT ItemID   as ID, EngName as [Item Name] FROM Stc_Items  WHERE Cancel =0  ";

                ColumnWidth = new int[] { 50, 300 };

                if (cls.SQLStr != "")
                {
                    frmSearch frm = new frmSearch();
                    cls.strFilter = "الرقم";
                    if (UserInfo.Language == iLanguage.English)
                        cls.strFilter = "ID";

                    frm.AddSearchData(cls);
                    frm.ColumnWidth = ColumnWidth;
                    frm.ShowDialog();
                    if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
                    {


                        txtToItemID1.Text = cls.PrimaryKeyValue.ToString();

                        var sr = "SELECT ArbName  FROM Stc_Items  WHERE Cancel =0  and ItemID=" + Comon.cInt(cls.PrimaryKeyValue.ToString());
                        var dr = Lip.SelectRecord(sr);
                        if (dr.Rows.Count > 0)
                            lblToItemID1.Text = dr.Rows[0][0].ToString();
                        else
                        {
                            lblToItemID1.Text = "";
                            txtToItemID1.Text = "";

                        }
                    }
                }
            }
        }

        private void txtFromSizeID1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                CSearch cls = new CSearch();
                int[] ColumnWidth = new int[] { 100, 300 };

                cls.SQLStr = "SELECT SizeID    as الرقم, ArbName as [اسم الوحدة] FROM Stc_SizingUnits WHERE Cancel =0  and Notes<>'0' ";

                if (UserInfo.Language == iLanguage.English)
                    cls.SQLStr = "SELECT SizeID   as ID, EngName as [Size Name] FROM Stc_SizingUnits  WHERE Cancel =0  and Notes<>'0' ";

                ColumnWidth = new int[] { 50, 300 };

                if (cls.SQLStr != "")
                {
                    frmSearch frm = new frmSearch();
                    cls.strFilter = "الرقم";
                    if (UserInfo.Language == iLanguage.English)
                        cls.strFilter = "ID";

                    frm.AddSearchData(cls);
                    frm.ColumnWidth = ColumnWidth;
                    frm.ShowDialog();
                    if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
                    {


                        txtFromSizeID1.Text = cls.PrimaryKeyValue.ToString();
                        txtToSizeID1.Text = cls.PrimaryKeyValue.ToString();
                        var sr = "SELECT ArbName  FROM Stc_SizingUnits WHERE Cancel =0  and Notes<>'0'  and SizeID=" + Comon.cInt(cls.PrimaryKeyValue.ToString());
                        var dr = Lip.SelectRecord(sr);
                        if (dr.Rows.Count > 0)
                        {
                            lblFromSizeID1.Text = dr.Rows[0][0].ToString();
                            lblToSizeID1.Text = dr.Rows[0][0].ToString();
                        }
                        else
                        {
                            lblFromSizeID1.Text = "";
                            txtFromSizeID1.Text = "";

                            lblToSizeID1.Text = "";
                            txtToSizeID1.Text = "";


                        }
                    }
                }
            }
        }

        private void txtToSizeID1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                CSearch cls = new CSearch();
                int[] ColumnWidth = new int[] { 100, 300 };

                cls.SQLStr = "SELECT SizeID    as الرقم, ArbName as [اسم الوحدة] FROM Stc_SizingUnits WHERE Cancel =0  and Notes<>'0' ";

                if (UserInfo.Language == iLanguage.English)
                    cls.SQLStr = "SELECT SizeID   as ID, EngName as [Size Name] FROM Stc_SizingUnits  WHERE Cancel =0  and Notes<>'0' ";

                ColumnWidth = new int[] { 50, 300 };

                if (cls.SQLStr != "")
                {
                    frmSearch frm = new frmSearch();
                    cls.strFilter = "الرقم";
                    if (UserInfo.Language == iLanguage.English)
                        cls.strFilter = "ID";

                    frm.AddSearchData(cls);
                    frm.ColumnWidth = ColumnWidth;
                    frm.ShowDialog();
                    if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
                    {


                        txtToSizeID1.Text = cls.PrimaryKeyValue.ToString();

                        var sr = "SELECT ArbName  FROM Stc_SizingUnits WHERE Cancel =0  and Notes<>'0'  and SizeID=" + Comon.cInt(cls.PrimaryKeyValue.ToString());
                        var dr = Lip.SelectRecord(sr);
                        if (dr.Rows.Count > 0)
                            lblToSizeID1.Text = dr.Rows[0][0].ToString();
                        else
                        {
                            lblToSizeID1.Text = "";
                            txtToSizeID1.Text = "";

                        }
                    }
                }
            }
        }












        private void txtFromDate_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFromDate.Text.Trim()))
                txtFromDate.EditValue = DateTime.Now;
        }

        private void txtToDate_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtToDate.Text.Trim()))
                txtToDate.EditValue = DateTime.Now;
        }

        private void IsTakeOne_CheckedChanged(object sender, EventArgs e)
        {
            if (IsTakeOne.Checked == true)
            {
                pnlOntherItem.Enabled = false;
                pnlOntherItem.Enabled = false;
            }
            else
            {
                pnlOntherItem.Enabled = true;
                pnlOntherItem.Enabled = true;
            }
        }

        private void txtBarCode_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void txtBarCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                CSearch cls = new CSearch();
                int[] ColumnWidth = new int[] { 100, 300 };
                string SearchSql = "";
                string Condition = "Where 1=1";

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Search(txtBarCode, lblBarCodeName, "BarCodeForPurchaseInvoice", "اسـم الـمـادة", "البـاركـود");
                else
                    PrepareSearchQuery.Search(txtBarCode, lblBarCodeName, "BarCodeForPurchaseInvoice", "Item Name", "BarCode");
            }
        }



    }
}

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
using Edex.DAL.ManufacturingDAL;
using Edex.Model;
using DevExpress.XtraSplashScreen;
using Edex.GeneralObjects.GeneralClasses;
using Edex.ModelSystem;
using Edex.DAL.Stc_itemDAL;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using System.IO;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraReports.UI;
using Edex.DAL.Accounting;
using Edex.DAL.SalseSystem.Stc_itemDAL;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.Utils.Drawing;
using DevExpress.XtraEditors.Controls;
using DevExpress.LookAndFeel;
using DevExpress.XtraEditors.Drawing;
using DevExpress.Utils;
using Edex.AccountsObjects.Transactions;
using Edex.Model.Language;
using Edex.HR.Codes;
using Edex.StockObjects.Codes;
using Permissions = Edex.ModelSystem.Permissions;
using System.Globalization;
using Edex.StockObjects.Transactions;

namespace Edex.Manufacturing.Codes
{
    public partial class frmManufactoryAdditional : BaseForm
    {
        //list detail
        BindingList<Menu_FactoryRunCommandSelver> lstDetailAdditional = new BindingList<Menu_FactoryRunCommandSelver>();
        BindingList<Menu_FactoryRunCommandSelver> lstDetailAfterAdditional = new BindingList<Menu_FactoryRunCommandSelver>();
        BindingList<Menu_FactoryOrderDetails> lstOrderDetails = new BindingList<Menu_FactoryOrderDetails>();

        BindingList<Stc_ItemUnits> lstDetailUnit = new BindingList<Stc_ItemUnits>();
        #region Declare 
        public int DocumentTypeAdditionalBefore = 44;
        public int DocumentTypeAdditionalAfter = 45; 
        private Menu_FactoryRunCommandMasterDAL cClass = new Menu_FactoryRunCommandMasterDAL();

        int rowIndex = 0;
        DataTable DataRecord; 
        DataTable DataRecordPolushin; 
        DataTable DataRecordAfterBrntag;
        DataTable DataRecordSelver;
        DataTable DataRecordAdditional; 
        DataTable DataRecordAfterAdditional;

        public CultureInfo culture = new CultureInfo("en-US");
        int indexPrntagerow;
        string FocusedControl = "";
        private bool IsNewRecord;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        string strSQL = "";
        public bool HasColumnErrors = false;
        private string PrimaryName;
        private string ItemName;
        private string SizeName;
        private string CaptionItemName;
        private decimal TotalPrntagLost = 0;
        #endregion
        public frmManufactoryAdditional()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
            InitializeComponent();
            SplashScreenManager.CloseForm();

            //Events

            this.txtCustomerID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCustomerID_Validating);
            this.txtEmpID.Validating += new System.ComponentModel.CancelEventHandler(this.txtEmpFactorID_Validating);
            this.txtEmployeeStokID.Validating += new System.ComponentModel.CancelEventHandler(this.txtEmployeeStokID_Validating);
            this.txtCommandID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCommandID_Validating);
            this.txtTypeOrder.Validating += new System.ComponentModel.CancelEventHandler(this.txtTypeOrder_Validating);
            this.txtGuidanceID.Validating += new System.ComponentModel.CancelEventHandler(this.txtGuidanceID_Validating);

            this.txtOrderID.Validating += new System.ComponentModel.CancelEventHandler(this.txtOrderID_Validating);

            this.txtReferanceID.Validating += txtReferanceID_Validating;
            //Event GridView

            this.gridControlBeforeAdditional.ProcessGridKey += gridControl2_ProcessGridKey;
            this.gridControlAfterAdditional.ProcessGridKey += gridControl2_ProcessGridKey;

            this.GridViewBeforAddition.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.GridViewBeforfactory_ValidatingEditor);
            this.GridViewAfterAddition.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.GridViewAfterfactory_ValidatingEditor);
            this.GridViewBeforAddition.RowUpdated += GridViewBeforfactory_RowUpdated;
            this.GridViewAfterAddition.RowUpdated += GridViewBeforfactory_RowUpdated;

            ItemName = "ArbItemName";
            SizeName = "ArbSizeName";
            PrimaryName = "ArbName";
            CaptionItemName = "اسم الصنف";
            if (UserInfo.Language == iLanguage.English)
            {   
                ItemName = "EngItemName";
                SizeName = "EngSizeName";
                PrimaryName = "EngName";
                CaptionItemName = "Item Name";
            }
            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
            FillCombo.FillComboBox(cmbCurency, "Acc_Currency", "ID", PrimaryName, "", " BranchID="+MySession.GlobalBranchID, (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));

            FillCombo.FillComboBox(cmbTypeStage, "Manu_TypeStages", "ID", PrimaryName, "", "", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
            cmbTypeStage.EditValue = 11;
            cmbTypeStage.ReadOnly = true;
            FillCombo.FillComboBox(cmbStatus, "Manu_TypeStatus", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد الحالة  "));
            cmbStatus.EditValue = MySession.GlobalDefaultProcessPostedStatus;
            cmbCurency.EditValue = MySession.GlobalDefaultSaleCurencyID;
            cmbBranchesID.EditValue = MySession.GlobalBranchID;
            cmbBranchesID.ReadOnly = !MySession.GlobalAllowBranchModificationAllScreens;
            this.GridViewAfterAddition.CellValueChanging+=GridViewAfterAddition_CellValueChanging;
            EnableControlDefult();
        
        }
        void EnableControlDefult()
        {
            cmbCurency.ReadOnly = !MySession.GlobalAllowChangefrmAddtionalCurrncyID;
            txtCommandDate.ReadOnly = !MySession.GlobalAllowChangefrmAddtionalCommandDate;
            txtStoreID.ReadOnly = !MySession.GlobalAllowChangefrmAddtionalStoreID;
            txtAccountID.ReadOnly = !MySession.GlobalAllowChangefrmAddtionalAccountID;
            txtEmpID.ReadOnly = !MySession.GlobalAllowChangefrmAddtionalEmployeeID;

        }
        void SetDefultValue()
        {

            cmbCurency.EditValue =Comon.cInt( MySession.GlobalDefaultAddtionalCurrncyID);
            cmbCurency_EditValueChanged(null, null);
            txtStoreID.Text = MySession.GlobalDefaultAddtionalStoreID;
            txtStoreIDFactory_Validating(null, null);
            txtAccountID.Text = MySession.GlobalDefaultAddtionalAccountID;
            txtAccountIDFactory_Validating(null, null);
            txtEmpID.Text = MySession.GlobalDefaultAddtionalEmployeeID;
            txtEmpIDFactor_Validating(null, null);
        }
        void GridViewAfterAddition_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                if (((view.GetRowCellValue(e.RowHandle, "ItemID") == null) || Comon.cInt(view.GetRowCellValue(e.RowHandle, "ItemID")) <= 0) && e.Column.FieldName == "ShownInNext")
                {
                    Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "الرجاء اضافة صنف ومن ثم تفعيل الخيار" : "Please Add Item and selcet option");
                    return;
                }
                if (e.Column.FieldName == "ShownInNext")
                {
                    if (Comon.cbool(e.Value) == true)
                    {

                        int isShow = Comon.cInt(Lip.GetValue("SELECT [ShowInOrderDetils] FROM [Stc_Items] WHERE [ItemID] = " + view.GetRowCellValue(e.RowHandle, "ItemID") + " AND Cancel = 0 and BranchID="+MySession.GlobalBranchID));

                        if (isShow != 1)
                        {
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgNotSelectShowInDetilsOrder);

                            view.SetRowCellValue(e.RowHandle, "ShownInNext", false);
                        }
                    }
                    SendKeys.Send("\t");
                }
            }
            catch { }

        }
        void txtReferanceID_Validating(object sender, CancelEventArgs e)
        {
            DataTable dt = AuxiliaryMaterialsDAl.frmGetDataDetalByReferance(Comon.cInt(txtReferanceID.Text), Comon.cInt(Comon.cInt(cmbBranchesID.EditValue)), UserInfo.FacilityID);
           
        }


        void GridViewBeforfactory_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            CalculateFactoryLost();

        }



        private void txtCommandID_Validating(object sender, CancelEventArgs e)
        {
            if (FormView == true)
                ReadRecord(Comon.cInt(txtCommandID.Text), true);
            else
            {
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                return;
            }
        }

        private void GridViewAfterfactory_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {

            if (this.GridViewAfterAddition.ActiveEditor is CheckEdit)
            {
                GridView view = sender as GridView;
                string ColName = view.FocusedColumn.FieldName;
                if (ColName == "ShownInNext" && Comon.cbool(e.Value) == true)
                {

                    int isShow = Comon.cInt(Lip.GetValue("SELECT [ShowInOrderDetils]  FROM  [Stc_Items] where [ItemID]=" + view.GetFocusedRowCellValue("ItemID") + "   and Cancel=0 and BranchID=" + MySession.GlobalBranchID));

                    if (isShow != 1)
                    {
                        //Messages.MsgWarning(Messages.TitleWorning, Messages.msgNotSelectShowInDetilsOrder);
                        e.Value = false;
                        return;
                    }

                }
            }
            if (this.GridViewAfterAddition.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                HasColumnErrors = false;
                string ColName = view.FocusedColumn.FieldName;
                int ItemID = 0;
                if (ColName == "EmpID" || ColName == "StoreID" || ColName == "SizeID" || ColName == "ItemID" || ColName == "MachinID" || ColName == "Credit" || ColName == "Debit")
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

                    else if (Comon.cDec(val.ToString()) <= 0)
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputIsGreaterThanZero;
                    }
                    else
                    {
                        e.Valid = true;
                        view.SetColumnError(GridViewAfterAddition.Columns[ColName], "");
                    }

                    if (ColName == "MachinID")
                    {


                        DataTable dtGroupID = Lip.SelectRecord("Select " + PrimaryName + " from Menu_FactoryMachine Where MachineID=" + Comon.cInt(e.Value) + " and BranchID=" + MySession.GlobalBranchID);
                        if (dtGroupID.Rows.Count > 0)
                        {
                            FileDataMachinName(GridViewAfterAddition, "DebitDate", "DebitTime", Comon.cInt(e.Value));

                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = "رقم المكينة غير موجود  ";
                        }
                    }


                    if (ColName == "ItemID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_ItemUnits  Where ItemID=" + Comon.cInt(e.Value) + " and BranchID=" + MySession.GlobalBranchID);
                        if (dtItemID.Rows.Count > 0)
                        {
                            FillItemData(GridViewAfterAddition, gridControlAfterAdditional, "BarcodeAdditional", "Credit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", txtAccountID);
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = "رقم الصنف غير موجود  ";
                        }
                    }
                    if (ColName == "SizeID")
                    {

                        DataTable dtItemID = Lip.SelectRecord("Select  " + PrimaryName + " from Stc_SizingUnits  Where SizeID=" + e.Value + " and BranchID=" + MySession.GlobalBranchID);
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridViewAfterAddition.SetRowCellValue(GridViewAfterAddition.FocusedRowHandle, GridViewAfterAddition.Columns[SizeName], dtItemID.Rows[0][PrimaryName].ToString());
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = "رقم الوحدة غير موجود  ";
                        }
                    }

                    if (ColName == "EmpID")
                    {
                        DataTable dtNameEmp = Lip.SelectRecord("Select " + PrimaryName + " from HR_EmployeeFile  Where EmployeeID=" + Comon.cInt(e.Value) + " and BranchID=" + MySession.GlobalBranchID);


                        e.Valid = true;
                        HasColumnErrors = false;
                        e.ErrorText = "";
                        return;
                        if (dtNameEmp.Rows.Count > 0)
                        {

                            GridViewAfterAddition.SetRowCellValue(GridViewAfterAddition.FocusedRowHandle, GridViewAfterAddition.Columns["EmpName"], dtNameEmp.Rows[0][PrimaryName].ToString());
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = "رقم العامل غير موجود  ";
                        }
                    }
                    else if (ColName == "StoreID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select  " + PrimaryName + " from  Stc_Stores Where Cancel=0 and BranchID="+MySession.GlobalBranchID+"  And StoreID=" + val.ToString() + " And FacilityID=" + UserInfo.FacilityID);
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridViewAfterAddition.SetFocusedRowCellValue("StoreName", dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(GridViewAfterAddition.Columns[ColName], "");

                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(GridViewAfterAddition.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }



                }
                if (ColName == ItemName)
                {

                    DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_Items_Find  Where  LOWER (" + ItemName + ")=LOWER ('" + val.ToString() + "') and BranchID=" + MySession.GlobalBranchID);
                    if (dtItemID.Rows.Count > 0)
                    {
                        FillItemData(GridViewAfterAddition, gridControlAfterAdditional, "BarcodeAdditional", "Credit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", ((TextEdit)txtAccountID));
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = " الصنف غير موجود  ";
                    }
                }

                if (ColName == "MachineName")
                {

                    DataTable dtMachinID = Lip.SelectRecord("Select  MachineID  from Menu_FactoryMachine Where  LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') ");
                    if (dtMachinID.Rows.Count > 0)
                    {
                        GridViewAfterAddition.SetFocusedRowCellValue("MachinID", dtMachinID.Rows[0]["MachineID"].ToString());

                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = " المكينة غير موجوده  ";
                    }
                }
                else if (ColName == "DebitDate")
                {
                    string formattedDate = ((DateTime)e.Value).ToString("yyyy/MM/dd");
                    if (Lip.CheckDateISAvilable(formattedDate))
                    {
                        string serverDate = Lip.GetServerDate(); e.Value = serverDate;
                        GridViewAfterAddition.SetRowCellValue(GridViewAfterAddition.FocusedRowHandle, "DebitDate", serverDate);
                        return;
                    }
                }
                if (ColName == SizeName)
                {
                    string Str = "Select Stc_ItemUnits.SizeID from Stc_ItemUnits inner join Stc_Items on Stc_Items.ItemID=Stc_ItemUnits.ItemID and Stc_Items.BranchID=Stc_ItemUnits.BranchID left outer join  Stc_SizingUnits on Stc_ItemUnits.SizeID=Stc_SizingUnits.SizeID and Stc_ItemUnits.BranchID=Stc_SizingUnits.BranchID Where UnitCancel=0 and Stc_Items.BranchID=" + MySession.GlobalBranchID + " And LOWER (Stc_SizingUnits." + PrimaryName + ")=LOWER ('" + val.ToString() + "') and  Stc_Items.ItemID=" + Comon.cLong(GridViewBeforAddition.GetRowCellValue(GridViewBeforAddition.FocusedRowHandle, "ItemID").ToString()) + "  And Stc_ItemUnits.FacilityID=" + UserInfo.FacilityID;
                    DataTable dtSizeID = Lip.SelectRecord(Str);
                    if (dtSizeID.Rows.Count > 0)
                    {

                        FillItemData(GridViewAfterAddition, gridControlAfterAdditional, "BarcodeAdditional", "Credit", Stc_itemsDAL.GetItemDataByItemID_SizeID(Comon.cInt(GridViewAfterAddition.GetRowCellValue(GridViewAfterAddition.FocusedRowHandle, "ItemID")), Comon.cInt(dtSizeID.Rows[0][0].ToString()), UserInfo.FacilityID), "DebitDate", "DebitTime", ((TextEdit)txtAccountID));
                        GridViewAfterAddition.SetRowCellValue(GridViewAfterAddition.FocusedRowHandle, ColName, e.Value.ToString());
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundSizeForItem;
                        view.SetColumnError(GridViewBeforAddition.Columns[ColName], Messages.msgNoFoundSizeForItem);
                    }

                }
                if (ColName == "EmpName")
                {

                    DataTable dtMachinID = Lip.SelectRecord("Select  EmployeeID  from HR_EmployeeFile Where  LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') and BranchID=" + MySession.GlobalBranchID);
                    if (dtMachinID.Rows.Count > 0)
                    {
                        GridViewAfterAddition.SetFocusedRowCellValue("EmpID", dtMachinID.Rows[0]["EmployeeID"].ToString());

                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = "لا يوجد عامل بهذا الاسم";
                    }
                }
                else if (ColName == "StoreName")
                {
                    DataTable dtItemID = Lip.SelectRecord("Select StoreID from Stc_Stores Where Cancel=0 and BranchID="+MySession.GlobalBranchID+" And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                    if (dtItemID.Rows.Count > 0)
                    {
                        GridViewAfterAddition.SetFocusedRowCellValue("StoreID", dtItemID.Rows[0]["StoreID"]);
                        e.Valid = true;
                        view.SetColumnError(GridViewAfterAddition.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(GridViewAfterAddition.Columns[ColName], Messages.msgNoFoundThisItem);
                    }
                }
            }
        }
        private void FillItemData(GridView Grid, GridControl GridControl, string BarCode, string QTYFildName, DataTable dt, string Date, string Time, TextEdit ObjtxtAccount, string QTY = "")
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Stc_itemsDAL.CheckIfStopItemUnit(dt.Rows[0]["BarCode"].ToString(), MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                {

                    Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                    Grid.DeleteRow(Grid.FocusedRowHandle);
                    return;
                }
                decimal totalQtyBalance = 0;
           
                {
                    if ((((GridView)Grid).Name == GridViewBeforAddition.Name))
                    {
                        totalQtyBalance = Lip.RemindQtyItemByMinUnit(Comon.cLong(dt.Rows[0]["ItemID"].ToString()), Comon.cInt(dt.Rows[0]["SizeID"]), Comon.cDbl(txtStoreID.Text));
                        {
                            decimal qtyCurrent = 0;
                            decimal QtyInCommand = Lip.GetQTYinCommandToThisItem("Menu_FactoryRunCommandSelver", "Menu_FactoryRunCommandMaster", QTYFildName, "ComandID", Comon.cInt(txtCommandID.Text), dt.Rows[0]["ItemID"].ToString(), " and Menu_FactoryRunCommandSelver.TypeOpration=1", BarCode,SizeID:Comon.cInt( dt.Rows[0]["SizeID"].ToString()));
                            qtyCurrent = frmCadFactory.GetQTYToItemFromGridView(Grid, QTYFildName, 0, dt.Rows[0]["ItemID"].ToString(), Comon.cInt(dt.Rows[0]["SizeID"].ToString()), "BarcodeAdditional"); 
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
                }
                if (MySession.AllowNotShowQTYInQtyField == false)
                    totalQtyBalance = 0;
                if (QTY != "")
                    totalQtyBalance = Comon.cDec(QTY);
                if ( (((GridView)Grid).Name ==GridViewBeforAddition.Name))
                    Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[QTYFildName], totalQtyBalance);
                else
                    Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[QTYFildName], 0);
                {
                    Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[Time], DateTime.Now.ToString("hh:mm:tt"));
                    Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[Date], DateTime.Now.ToString("yyyy/MM/dd"));
                }
                //RepositoryItemLookUpEdit rSize = Common.LookUpEditSize(Comon.cDbl(dt.Rows[0]["ItemID"].ToString()));
                //Grid.Columns[SizeName].ColumnEdit = rSize;
                //GridControl.RepositoryItems.Add(rSize);
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[BarCode], dt.Rows[0]["BarCode"].ToString().ToUpper());
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns["ItemID"], dt.Rows[0]["ItemID"].ToString());
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[ItemName], dt.Rows[0][PrimaryName].ToString());
                if (UserInfo.Language == iLanguage.English)
                    Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[SizeName], dt.Rows[0][SizeName].ToString());
                else
                    Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns["SizeID"], dt.Rows[0]["SizeID"].ToString());
                //Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());
            }
            else
            {
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns["ItemID"], "0");
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[ItemName], "");
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[SizeName], "");
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[QTYFildName], "");
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns["BarCode"], "");
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns["SizeID"], "");
            }
        }

        private void GridViewBeforfactory_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {

            if (this.GridViewBeforAddition.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                HasColumnErrors = false;
                string ColName = view.FocusedColumn.FieldName;
                int ItemID = 0;
                if (ColName == "EmpID" || ColName == "StoreID" || ColName == "SizeID" || ColName == "ItemID" || ColName == "MachinID" || ColName == "Credit" || ColName == "Debit")
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

                    else if (Comon.cDec(val.ToString()) <= 0)
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputIsGreaterThanZero;
                    }
                    else
                    {
                        e.Valid = true;
                        view.SetColumnError(GridViewBeforAddition.Columns[ColName], "");
                    }
                    if (ColName == "MachinID")
                    {
                        DataTable dtGroupID = Lip.SelectRecord("Select "+PrimaryName+" from Menu_FactoryMachine Where MachineID=" + Comon.cInt(e.Value));
                        if (dtGroupID.Rows.Count > 0)
                        {
                            e.Valid = true;
                            view.SetColumnError(GridViewBeforAddition.Columns[ColName], "");
                            GridViewBeforAddition.SetRowCellValue(GridViewBeforAddition.FocusedRowHandle, GridViewBeforAddition.Columns["ID"], GridViewBeforAddition.RowCount);

                            FileDataMachinName(GridViewBeforAddition, "DebitDate", "DebitTime", Comon.cInt(e.Value));
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = "رقم المكينة غير موجود  ";
                        }
                    }
                    if (ColName == "ItemID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_ItemUnits  Where ItemID=" + Comon.cInt(e.Value) + " and BranchID=" + MySession.GlobalBranchID );
                        if (dtItemID.Rows.Count > 0)
                        {
                            FillItemData(GridViewBeforAddition, gridControlBeforeAdditional, "BarcodeAdditional", "Debit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", ((TextEdit)txtAccountID));
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = "رقم الصنف غير موجود  ";
                        }
                    }
                    if (ColName == "Debit")
                    {
                        decimal totalQtyBalance = Lip.RemindQtyItemByMinUnit(Comon.cLong(GridViewBeforAddition.GetRowCellValue(GridViewBeforAddition.FocusedRowHandle, "ItemID").ToString()), Comon.cInt(GridViewBeforAddition.GetRowCellValue(GridViewBeforAddition.FocusedRowHandle, "SizeID")), Comon.cDbl(txtStoreID.Text));
                        decimal QtyInCommand = Lip.GetQTYinCommandToThisItem("Menu_FactoryRunCommandSelver", "Menu_FactoryRunCommandMaster", "Debit", "ComandID", Comon.cInt(txtCommandID.Text), GridViewBeforAddition.GetRowCellValue(GridViewBeforAddition.FocusedRowHandle, "ItemID").ToString(), " and Menu_FactoryRunCommandSelver.TypeOpration=1 ","BarcodeAdditional",SizeID:Comon.cInt(GridViewBeforAddition.GetRowCellValue(GridViewBeforAddition.FocusedRowHandle, "SizeID").ToString()));
                        totalQtyBalance += QtyInCommand;
                        decimal qtyCurrent = 0;
                       
                        qtyCurrent = frmCadFactory.GetQTYToItemFromGridView(GridViewBeforAddition, "Debit", Comon.cDec(val.ToString()), GridViewBeforAddition.GetRowCellValue(GridViewBeforAddition.FocusedRowHandle, "ItemID").ToString(), Comon.cInt(GridViewBeforAddition.GetRowCellValue(GridViewBeforAddition.FocusedRowHandle, "SizeID")), "BarcodeAdditional"); 
                        if (qtyCurrent > totalQtyBalance)
                        {
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheQTyinOrderisExceed);
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgQtyisNotAvilable + (totalQtyBalance - (qtyCurrent - Comon.cDec(val.ToString())));
                            view.SetColumnError(GridViewBeforAddition.Columns[ColName], Messages.msgQtyisNotAvilable + totalQtyBalance.ToString());
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
                                    view.SetColumnError(GridViewBeforAddition.Columns[ColName], Messages.msgQtyisNotAvilable + totalQtyBalance.ToString());
                                }
                            }
                            else
                            {
                                e.Valid = false;
                                HasColumnErrors = true;
                                e.ErrorText = Messages.msgNotFoundAnyQtyInStore;
                                view.SetColumnError(GridViewBeforAddition.Columns[ColName], Messages.msgNotFoundAnyQtyInStore);
                            }
                        }
                    }

                    if (ColName == "SizeID")
                    {

                        DataTable dtItemID = Lip.SelectRecord("Select  " + PrimaryName + " from Stc_SizingUnits  Where SizeID=" + e.Value + " and BranchID=" + MySession.GlobalBranchID);
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridViewBeforAddition.SetRowCellValue(GridViewBeforAddition.FocusedRowHandle, GridViewBeforAddition.Columns[SizeName], dtItemID.Rows[0][PrimaryName].ToString());
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = "رقم الوحدة غير موجود  ";
                        }
                    }

                    if (ColName == "EmpID")
                    {
                        DataTable dtNameEmp = Lip.SelectRecord("Select " + PrimaryName + " from HR_EmployeeFile  Where EmployeeID=" + Comon.cInt(e.Value) + " and BranchID=" + MySession.GlobalBranchID );


                        e.Valid = true;
                        HasColumnErrors = false;
                        e.ErrorText = "";
                        return;
                        if (dtNameEmp.Rows.Count > 0)
                        {

                            GridViewBeforAddition.SetRowCellValue(GridViewBeforAddition.FocusedRowHandle, GridViewBeforAddition.Columns["EmpName"], dtNameEmp.Rows[0][PrimaryName].ToString());
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = "رقم العامل غير موجود  ";
                        }
                    }
                    else if (ColName == "StoreID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select  " + PrimaryName + " from  Stc_Stores Where Cancel=0 and BranchID="+MySession.GlobalBranchID +"  And StoreID=" + val.ToString() + " And FacilityID=" + UserInfo.FacilityID);
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridViewBeforAddition.SetFocusedRowCellValue("StoreName", dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(GridViewBeforAddition.Columns[ColName], "");

                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(GridViewBeforAddition.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }

                }
                else if (ColName == "DebitDate")
                {
                    string formattedDate = ((DateTime)e.Value).ToString("yyyy/MM/dd");
                    if (Lip.CheckDateISAvilable(formattedDate))
                    {
                        string serverDate = Lip.GetServerDate(); e.Value = serverDate;
                        GridViewBeforAddition.SetRowCellValue(GridViewBeforAddition.FocusedRowHandle, "DebitDate", serverDate);
                        return;
                    }
                }
                if (ColName == ItemName)
                {
                    DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_Items_Find  Where  LOWER (" + ItemName + ")=LOWER ('" + val.ToString() + "') and BranchID=" + MySession.GlobalBranchID );
                    if (dtItemID.Rows.Count > 0)
                    {
                        FillItemData(GridViewBeforAddition, gridControlBeforeAdditional, "BarcodeAdditional", "Debit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", ((TextEdit)txtAccountID));
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = " الصنف غير موجود  ";
                    }
                }

                if (ColName == "MachineName")
                {

                    DataTable dtMachinID = Lip.SelectRecord("Select  MachineID  from Menu_FactoryMachine Where  LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') ");
                    if (dtMachinID.Rows.Count > 0)
                    {

                        FileDataMachinName(GridViewBeforAddition, "DebitDate", "DebitTime", Comon.cInt(dtMachinID.Rows[0]["MachineID"].ToString()));
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = " المكينة غير موجوده  ";
                    }
                }
                if (ColName == SizeName)
                {

                    DataTable dtBarCode = Lip.SelectRecord("Select  BarCode from Stc_Items_Find   Where  LOWER (" + SizeName + ")=LOWER ('" + e.Value.ToString() + "') and BranchID="+MySession.GlobalBranchID+" and ItemID=" + Comon.cInt(GridViewBeforAddition.GetRowCellValue(GridViewBeforAddition.FocusedRowHandle, "ItemID")) + " ");
                    if (dtBarCode.Rows.Count > 0)
                    {

                        FillItemData(GridViewBeforAddition, gridControlBeforeAdditional, "BarcodeAdditional", "Debit", Stc_itemsDAL.GetItemData1(dtBarCode.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", ((TextEdit)txtAccountID));
                        GridViewBeforAddition.SetRowCellValue(GridViewBeforAddition.FocusedRowHandle, ColName, e.Value.ToString());
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = " الوحده غير موجوده  ";
                    }
                }
                if (ColName == SizeName)
                {
                    string Str = "Select Stc_ItemUnits.SizeID from Stc_ItemUnits inner join Stc_Items on Stc_Items.ItemID=Stc_ItemUnits.ItemID and Stc_Items.BranchID=Stc_ItemUnits.BranchID left outer join  Stc_SizingUnits on Stc_ItemUnits.SizeID=Stc_SizingUnits.SizeID and Stc_ItemUnits.BranchID=Stc_SizingUnits.BranchID Where UnitCancel=0 and Stc_Items.BranchID=" + MySession.GlobalBranchID + " And LOWER (Stc_SizingUnits." + PrimaryName + ")=LOWER ('" + val.ToString() + "') and  Stc_Items.ItemID=" + Comon.cLong(GridViewBeforAddition.GetRowCellValue(GridViewBeforAddition.FocusedRowHandle, "ItemID").ToString()) + "  And Stc_ItemUnits.FacilityID=" + UserInfo.FacilityID;
                    DataTable dtBarCode = Lip.SelectRecord(Str);
                    if (dtBarCode.Rows.Count > 0)
                    {
                        GridViewBeforAddition.SetFocusedRowCellValue("SizeID", dtBarCode.Rows[0]["SizeID"]);
                        frmCadFactory.SetValuseWhenChangeSizeName(GridViewBeforAddition, Comon.cLong(GridViewBeforAddition.GetRowCellValue(GridViewBeforAddition.FocusedRowHandle, "ItemID").ToString()), Comon.cInt(dtBarCode.Rows[0]["SizeID"]), "Menu_FactoryRunCommandSelver", "Menu_FactoryRunCommandMaster", Comon.cDbl(txtStoreID.Text), Comon.cInt(txtCommandID.Text), "ComandID", Where: " and Menu_FactoryRunCommandSelver.TypeOpration=1 ", FildNameQTY: "Debit", FildNameBarCode: "BarcodeAdditional");
                        e.Valid = true;
                        view.SetColumnError(GridViewBeforAddition.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundSizeForItem;
                        view.SetColumnError(GridViewBeforAddition.Columns[ColName], Messages.msgNoFoundSizeForItem);
                    }
                }
                if (ColName == "EmpName")
                {

                    DataTable dtMachinID = Lip.SelectRecord("Select  EmployeeID  from HR_EmployeeFile Where  LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') and BranchID=" + MySession.GlobalBranchID);
                    if (dtMachinID.Rows.Count > 0)
                    {
                        GridViewBeforAddition.SetFocusedRowCellValue("EmpID", dtMachinID.Rows[0]["EmployeeID"].ToString());

                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = "لا يوجد عامل بهذا الاسم";
                    }
                }
                else if (ColName == "StoreName")
                {
                    DataTable dtItemID = Lip.SelectRecord("Select StoreID from Stc_Stores Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') and BranchID="+MySession.GlobalBranchID+" And FacilityID=" + UserInfo.FacilityID);
                    if (dtItemID.Rows.Count > 0)
                    {
                        GridViewBeforAddition.SetFocusedRowCellValue("StoreID", dtItemID.Rows[0]["StoreID"]);
                        e.Valid = true;
                        view.SetColumnError(GridViewBeforAddition.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(GridViewBeforAddition.Columns[ColName], Messages.msgNoFoundThisItem);
                    }
                }
            }
        }

 

 
        private void gridControl2_ProcessGridKey(object sender, KeyEventArgs e)
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
                        if (HasColumnErrors == true)
                            return;
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
        private void gridControl3_ProcessGridKey(object sender, KeyEventArgs e)
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

                        if (HasColumnErrors == true)
                            return;
                        double num;
                        HasColumnErrors = false;
                        var cellValue = view.GetRowCellValue(view.FocusedRowHandle, view.FocusedColumn.FieldName);
                        string ColName = view.FocusedColumn.FieldName;

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
        void SetDetilOrder(string OrderID)
        {

            strSQL = "SELECT * FROM Manu_OrderRestriction WHERE  OrderID ='" + OrderID.Trim() + "' and BranchID=" + MySession.GlobalBranchID;
            Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, UserInfo.Language.ToString());
            DataTable dtt = Lip.SelectRecord(strSQL);
            if (dtt.Rows.Count > 0)
            {

                txtTypeOrder.Text = dtt.Rows[0]["TypeOrdersID"].ToString();
                txtTypeOrder_Validating(null, null);
                txtCustomerID.Text = dtt.Rows[0]["CustomerID"].ToString();
                txtCustomerID_Validating(null, null);
                txtDelegateID.Text = dtt.Rows[0]["DelegateID"].ToString();
                txtDelegateID_Validating(null, null);
                txtGuidanceID.Text = dtt.Rows[0]["GuidanceID"].ToString();
                txtGuidanceID_Validating(null, null);
                txtOrderDate.EditValue = Comon.ConvertSerialToDate(dtt.Rows[0]["OrderDate"].ToString());
                txtOrderDate.ReadOnly = true;

                GetOrderDetail(OrderID);

                //txtReferanceID.Focus();
            }

        }
        public void ReadRecord(int ComandID, bool flag = false)
        {
            try
            {
                ClearFields();

                DataRecord = Menu_FactoryRunCommandMasterDAL.frmGetDataDetalByID(ComandID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID, Comon.cInt(cmbTypeStage.EditValue));

                if (DataRecord != null && DataRecord.Rows.Count > 0)
                {
                    DataRecordPolushin = Menu_FactoryRunCommandSelverDAL.frmGetDataDetalByID(Comon.cLong(ComandID), UserInfo.BRANCHID, UserInfo.FacilityID, 1);
                    DataRecordAfterBrntag = Menu_FactoryRunCommandSelverDAL.frmGetDataDetalByID(Comon.cLong(ComandID), UserInfo.BRANCHID, UserInfo.FacilityID, 2);


                    IsNewRecord = false;
                    txtReferanceID.Text = DataRecord.Rows[0]["DocumentID"].ToString();
                    txtReferanceID_Validating(null, null);
                    txtNotes.Text = DataRecord.Rows[0]["Notes"].ToString();
                    cmbStatus.EditValue = Comon.cInt(DataRecord.Rows[0]["Posted"].ToString());
                    txtGuidanceID.Text = DataRecord.Rows[0]["BrandID"].ToString();
                    txtGuidanceID_Validating(null, null);

                    txtCustomerID.Text = DataRecord.Rows[0]["CustomerID"].ToString();
                    txtCustomerID_Validating(null, null);
                    txtDelegateID.Text = DataRecord.Rows[0]["DelegateID"].ToString();
                    txtDelegateID_Validating(null, null);
                    txtEmpID.Text = DataRecord.Rows[0]["EmpFactorID"].ToString();
                    txtEmpFactorID_Validating(null, null);

                    txtEmployeeStokID.Text = DataRecord.Rows[0]["EmployeeStokID"].ToString();
                    txtEmployeeStokID_Validating(null, null);
                    cmbCurency.EditValue = DataRecord.Rows[0]["CurrencyID"].ToString();

                    //الحسابات
                    txtAccountID.Text = DataRecord.Rows[0]["AccountIDFactory"].ToString();
                    txtAccountIDFactory_Validating(null, null);

                    txtStoreID.Text = DataRecord.Rows[0]["StoreIDFactory"].ToString();
                    txtStoreIDFactory_Validating(null, null);

                    txtEmployeeStokID.Text = DataRecord.Rows[0]["EmployeeStokIDFactory"].ToString();
                    txtEmployeeStokIDFactory_Validating(null, null);

                    txtEmpID.Text = DataRecord.Rows[0]["EmpIDFactor"].ToString();
                    txtEmpIDFactor_Validating(null, null);

                    txtOrderID.Text = DataRecord.Rows[0]["Barcode"].ToString();
                     
                    string OrderID = txtOrderID.Text;
                    SetDetilOrder(txtOrderID.Text);
                    if (Comon.ConvertSerialDateTo(DataRecord.Rows[0]["ComandDate"].ToString()) == "")
                        InitializeFormatDate(txtCommandDate);
                    else
                    txtCommandDate.EditValue = DateTime.ParseExact(Comon.ConvertSerialDateTo(DataRecord.Rows[0]["ComandDate"].ToString()), "dd/MM/yyyy", culture);


                    cmbCurency.EditValue = Comon.cInt(DataRecord.Rows[0]["CurrencyID"].ToString());
                    
                    if (!DataRecord.Rows[0].IsNull("InvoiceImage"))
                    {
                        byte[] imageData1 = (byte[])DataRecord.Rows[0]["InvoiceImage"];
                        using (MemoryStream ms = new MemoryStream(imageData1))
                        {
                            Image image = Image.FromStream(ms);
                            pictureEdit1.Image = image;
                        }
                    }
                    if (!DataRecord.Rows[0].IsNull("InvoiceImage2"))
                    {
                        byte[] imageData2 = (byte[])DataRecord.Rows[0]["InvoiceImage2"];
                        using (MemoryStream ms = new MemoryStream(imageData2))
                        {
                            Image image = Image.FromStream(ms);
                            pictureEdit2.Image = image;
                        }
                    }

                    if (DataRecordPolushin != null)
                        if (DataRecordPolushin.Rows.Count > 0)
                        {
                            gridControlBeforeAdditional.DataSource = DataRecordPolushin;
                            lstDetailAdditional.AllowNew = true;
                            lstDetailAdditional.AllowEdit = true;
                            lstDetailAdditional.AllowRemove = true;
                            //GridViewBeforPrentag.RefreshData();
                        }
                    if (DataRecordAfterBrntag != null)
                        if (DataRecordAfterBrntag.Rows.Count > 0)
                        {
                            gridControlAfterAdditional.DataSource = DataRecordAfterBrntag;

                            lstDetailAfterAdditional.AllowNew = true;
                            lstDetailAfterAdditional.AllowEdit = true;
                            lstDetailAfterAdditional.AllowRemove = true;
                            //GridViewAfterPrentag.RefreshData();
                        }
                    Validations.DoReadRipon(this, ribbonControl1);
                    CalculateFactoryLost();
                    EnabledControl(false);
                    //ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Caption = txtInvoiceID.Text;                    
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        #region initGrids


        void initGridOrderDetails()
        {

            lstOrderDetails = new BindingList<Menu_FactoryOrderDetails>();
            lstOrderDetails.AllowNew = true;
            lstOrderDetails.AllowEdit = true;
            lstOrderDetails.AllowRemove = true;
            gridControlOrderDetails.DataSource = lstOrderDetails;




            GridViewOrderDetails.Columns["ID"].Visible = false;
            GridViewOrderDetails.Columns["ComandID"].Visible = false;
            GridViewOrderDetails.Columns["BarCode"].Visible = false;
            GridViewOrderDetails.Columns["EmpPolishnID"].Visible = false;
            GridViewOrderDetails.Columns["EmpPrentagID"].Visible = false;
            GridViewOrderDetails.Columns["Cancel"].Visible = false;
            GridViewOrderDetails.Columns["BranchID"].Visible = false;
            GridViewOrderDetails.Columns["FacilityID"].Visible = false;
            GridViewOrderDetails.Columns["SizeID"].Visible = false;
            GridViewOrderDetails.Columns["EditUserID"].Visible = false;
            GridViewOrderDetails.Columns["EditDate"].Visible = false;
            GridViewOrderDetails.Columns["EditTime"].Visible = false;
            GridViewOrderDetails.Columns["RegDate"].Visible = false;
            GridViewOrderDetails.Columns["UserID"].Visible = false;

            GridViewOrderDetails.Columns["ComputerInfo"].Visible = false;
            GridViewOrderDetails.Columns["EditComputerInfo"].Visible = false;
            GridViewOrderDetails.Columns["RegTime"].Visible = false;

            GridViewOrderDetails.Columns["Credit"].Visible = false;
            GridViewOrderDetails.Columns["TypeOpration"].Visible = false;
            //GridViewBeforfactory.Columns["SizeID"].Visible = false;
            GridViewOrderDetails.Columns["CostPrice"].Visible = false;

            GridViewOrderDetails.Columns["EmpName"].Width = 120;

            GridViewOrderDetails.Columns["StoreName"].Width = 120;
            GridViewOrderDetails.Columns["EmpID"].Width = 120;
            GridViewOrderDetails.Columns["Signature"].Width = 120;


            GridViewOrderDetails.Columns["EmpID"].Visible = false;
            GridViewOrderDetails.Columns["EmpName"].Visible = false;
            GridViewOrderDetails.Columns["StoreID"].Visible = false;
            GridViewOrderDetails.Columns["StoreName"].Visible = false;
            GridViewOrderDetails.Columns["Signature"].Visible = false;

            GridViewOrderDetails.Columns["DebitTime"].Visible = false;
            GridViewOrderDetails.Columns["DebitDate"].Visible = false;

            if (UserInfo.Language == iLanguage.Arabic)
            {
                GridViewOrderDetails.Columns["EngItemName"].Visible = false;
                GridViewOrderDetails.Columns["EngSizeName"].Visible = false;
                GridViewOrderDetails.Columns["ArbItemName"].Width = 150;
                GridViewOrderDetails.Columns["StoreID"].Caption = "رقم المخزن";
                GridViewOrderDetails.Columns["StoreName"].Caption = "إسم المخزن";
                GridViewOrderDetails.Columns["EmpID"].Caption = "رقم العامل";
                GridViewOrderDetails.Columns["EmpName"].Caption = "إسم العامل";
                GridViewOrderDetails.Columns["QTY"].Caption = "الوزن";
                GridViewOrderDetails.Columns["Credit"].Caption = "دائــن";
                GridViewOrderDetails.Columns["TypeOpration"].Caption = "نوع العملية";
                GridViewOrderDetails.Columns["Signature"].Caption = "التوقيع";
                GridViewOrderDetails.Columns["ItemID"].Caption = "رقم الصنف";
                GridViewOrderDetails.Columns["ArbItemName"].Caption = "اسم الصنف";
                GridViewOrderDetails.Columns["SizeID"].Caption = "رقم الوحده";
                GridViewOrderDetails.Columns["ArbSizeName"].Caption = "الوحده";
                GridViewOrderDetails.Columns["CostPrice"].Caption = "التكلفة";
                GridViewOrderDetails.Columns["PeriodDay"].Caption = "المدة";
                GridViewOrderDetails.Columns["StateName"].Caption = "المرحلة";
                GridViewOrderDetails.Columns["DIAMOND_WG"].Caption = "جم";
                GridViewOrderDetails.Columns["DIAMOND_WC"].Caption = "قيراط";

            }
            else
            {

                GridViewOrderDetails.Columns["ArbItemName"].Visible = false;
                GridViewOrderDetails.Columns["ArbSizeName"].Visible = false;
                GridViewOrderDetails.Columns["EngItemName"].Visible = true;
                GridViewOrderDetails.Columns["EngSizeName"].Visible = true;
                GridViewOrderDetails.Columns["StoreID"].Caption = "Store ID";
                GridViewOrderDetails.Columns["StoreName"].Caption = "Store Name";

            }
            GridViewOrderDetails.OptionsBehavior.ReadOnly = true;
            GridViewOrderDetails.OptionsBehavior.Editable = false;
        }
        void initGridBeforAdditional()
        {

            lstDetailAdditional = new BindingList<Menu_FactoryRunCommandSelver>();
            lstDetailAdditional.AllowNew = true;
            lstDetailAdditional.AllowEdit = true;
            lstDetailAdditional.AllowRemove = true;
            gridControlBeforeAdditional.DataSource = lstDetailAdditional;

            DataTable dtitems = Lip.SelectRecord("SELECT   "+PrimaryName+"   FROM Menu_FactoryMachine ");
            string[] NameMachine = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameMachine[i] = dtitems.Rows[i][PrimaryName].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameMachine);

            gridControlBeforeAdditional.RepositoryItems.Add(riComboBoxitems);


            DataTable dtitems0 = Lip.SelectRecord("SELECT   SizeID," + PrimaryName + "   FROM Stc_SizingUnits where  BranchID=" + MySession.GlobalBranchID );
            string[] NameUnit = new string[dtitems0.Rows.Count];
            for (int i = 0; i <= dtitems0.Rows.Count - 1; i++)
                NameUnit[i] = dtitems0.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems0 = new RepositoryItemComboBox();
            riComboBoxitems0.Items.AddRange(NameUnit);
            gridControlBeforeAdditional.RepositoryItems.Add(riComboBoxitems0);
            GridViewBeforAddition.Columns[SizeName].ColumnEdit = riComboBoxitems0;


            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0 and BranchID=" + MySession.GlobalBranchID );
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();

            riComboBoxitems2.Items.AddRange(StoreName);
            gridControlBeforeAdditional.RepositoryItems.Add(riComboBoxitems2);
            GridViewBeforAddition.Columns["StoreName"].ColumnEdit = riComboBoxitems2;


            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0  and BranchID=" + MySession.GlobalBranchID  );
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlBeforeAdditional.RepositoryItems.Add(riComboBoxitems3);
            GridViewBeforAddition.Columns["EmpName"].ColumnEdit = riComboBoxitems3;

            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0  and BranchID=" + MySession.GlobalBranchID );
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlBeforeAdditional.RepositoryItems.Add(riComboBoxitems4);
            GridViewBeforAddition.Columns[ItemName].ColumnEdit = riComboBoxitems4;

            GridViewBeforAddition.Columns["MachineName"].ColumnEdit = riComboBoxitems;
            GridViewBeforAddition.Columns["ID"].Visible = false;
            GridViewBeforAddition.Columns["ComandID"].Visible = false;
            GridViewBeforAddition.Columns["BarcodeAdditional"].Visible = false;
            GridViewBeforAddition.Columns["EmpAdditionalID"].Visible = false; 
            GridViewBeforAddition.Columns["Cancel"].Visible = false;
            GridViewBeforAddition.Columns["BranchID"].Visible = false;
            GridViewBeforAddition.Columns["FacilityID"].Visible = false;

            GridViewBeforAddition.Columns["EditUserID"].Visible = false;
            GridViewBeforAddition.Columns["EditDate"].Visible = false;
            GridViewBeforAddition.Columns["EditTime"].Visible = false;
            GridViewBeforAddition.Columns["RegDate"].Visible = false;
            GridViewBeforAddition.Columns["UserID"].Visible = false;

            GridViewBeforAddition.Columns["ComputerInfo"].Visible = false;
            GridViewBeforAddition.Columns["EditComputerInfo"].Visible = false;
            GridViewBeforAddition.Columns["RegTime"].Visible = false;

            GridViewBeforAddition.Columns["Credit"].Visible = false;
            GridViewBeforAddition.Columns["TypeOpration"].Visible = false;
            //GridViewBeforPolish.Columns["SizeID"].Visible = false;
            GridViewBeforAddition.Columns["CostPrice"].Visible = false;
            GridViewBeforAddition.Columns["SizeID"].Visible = false;
            // GridViewBeforPolish.Columns["DebitTime"].Visible = false;
            GridViewBeforAddition.Columns["MachinID"].Name = "MachinID";
            //dtItem.Columns["Losed"].ReadOnly = true;
            GridViewBeforAddition.Columns["EmpName"].Width = 120;
            GridViewBeforAddition.Columns["StoreName"].Width = 120;
            GridViewBeforAddition.Columns["EmpID"].Width = 120;
            GridViewBeforAddition.Columns["Signature"].Width = 120;
            GridViewBeforAddition.Columns["DebitDate"].Width = 110;
            GridViewBeforAddition.Columns["DebitTime"].Width = 85;
            GridViewBeforAddition.Columns["EmpID"].Visible = false;
            GridViewBeforAddition.Columns["StoreName"].Visible = false;
            GridViewBeforAddition.Columns["EmpName"].Visible = false;
            GridViewBeforAddition.Columns["StoreID"].Visible = false;
            GridViewBeforAddition.Columns["MachinID"].Visible = false;
            GridViewBeforAddition.Columns["MachineName"].Visible = false;
            GridViewBeforAddition.Columns["Lost"].Visible = false;
            GridViewBeforAddition.Columns["ShownInNext"].Visible = false;
            if (UserInfo.Language == iLanguage.Arabic)
            {

                GridViewBeforAddition.Columns["EngItemName"].Visible = false;
                GridViewBeforAddition.Columns["EngSizeName"].Visible = false;
                GridViewBeforAddition.Columns["ArbItemName"].Width = 150;

                GridViewBeforAddition.Columns["StoreID"].Caption = "رقم المخزن";
                GridViewBeforAddition.Columns["StoreName"].Caption = "إسم المخزن";

                GridViewBeforAddition.Columns["EmpID"].Caption = "رقم العامل";
                GridViewBeforAddition.Columns["EmpName"].Caption = "إسم العامل";

                GridViewBeforAddition.Columns["MachinID"].Caption = "رقم المكينة";
                GridViewBeforAddition.Columns["MachineName"].Caption = "إسم المكينة";
                GridViewBeforAddition.Columns["Debit"].Caption = "الوزن";

                GridViewBeforAddition.Columns["Credit"].Caption = "دائــن";
                GridViewBeforAddition.Columns["TypeOpration"].Caption = "نوع العملية";
                GridViewBeforAddition.Columns["Signature"].Caption = "التوقيع";

                GridViewBeforAddition.Columns["ItemID"].Caption = "رقم الصنف";
                GridViewBeforAddition.Columns["ArbItemName"].Caption = "اسم الصنف";
                GridViewBeforAddition.Columns["SizeID"].Caption = "رقم الوحده";
                GridViewBeforAddition.Columns["ArbSizeName"].Caption = "الوحده";
                GridViewBeforAddition.Columns["CostPrice"].Caption = "التكلفة";
                GridViewBeforAddition.Columns["DebitDate"].Caption = "التاريخ";
                GridViewBeforAddition.Columns["DebitTime"].Caption = "الوقت";
            }
            else
            {
                GridViewBeforAddition.Columns["ArbItemName"].Visible = false;
                GridViewBeforAddition.Columns["ArbSizeName"].Visible = false;
                GridViewBeforAddition.Columns["EngItemName"].Width = 150;
                GridViewBeforAddition.Columns["StoreID"].Caption = "Store ID";
                GridViewBeforAddition.Columns["StoreName"].Caption = "Store Name";
                GridViewBeforAddition.Columns["EngItemName"].Caption = "Item Name";
                GridViewBeforAddition.Columns["MachinID"].Caption = "Machine ID";
                GridViewBeforAddition.Columns["MachineName"].Caption = "Machin Name";
                GridViewBeforAddition.Columns["Debit"].Caption = "debtor ";
                GridViewBeforAddition.Columns["EngSizeName"].Caption = "Unit";
                GridViewBeforAddition.Columns["Credit"].Caption = "Creditor";
                GridViewBeforAddition.Columns["TypeOpration"].Caption = "Type Opration";
                GridViewBeforAddition.Columns["Signature"].Caption = "Signature";
                GridViewBeforAddition.Columns["DebitDate"].Caption = "Date";
                GridViewBeforAddition.Columns["DebitTime"].Caption = "Time";
                GridViewBeforAddition.Columns["EmpID"].Caption = "EmpID";
                GridViewBeforAddition.Columns["EmpName"].Caption = "Name";
            }



        }
        void initGridAfterAdditional()
        {

            lstDetailAfterAdditional = new BindingList<Menu_FactoryRunCommandSelver>();
            lstDetailAfterAdditional.AllowNew = true;
            lstDetailAfterAdditional.AllowEdit = true;
            lstDetailAfterAdditional.AllowRemove = true;
            gridControlAfterAdditional.DataSource = lstDetailAfterAdditional;


            DataTable dtitems0 = Lip.SelectRecord("SELECT   SizeID," + PrimaryName + "   FROM Stc_SizingUnits where BranchID=" + MySession.GlobalBranchID  );
            string[] NameUnit = new string[dtitems0.Rows.Count];
            for (int i = 0; i <= dtitems0.Rows.Count - 1; i++)
                NameUnit[i] = dtitems0.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems0 = new RepositoryItemComboBox();
            riComboBoxitems0.Items.AddRange(NameUnit);
            gridControlAfterAdditional.RepositoryItems.Add(riComboBoxitems0);
            GridViewAfterAddition.Columns[SizeName].ColumnEdit = riComboBoxitems0;


            DataTable dtitems = Lip.SelectRecord("SELECT   "+PrimaryName+"   FROM Menu_FactoryMachine ");
            string[] NameMachine = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameMachine[i] = dtitems.Rows[i][PrimaryName].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameMachine);
            gridControlAfterAdditional.RepositoryItems.Add(riComboBoxitems);



            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0 and BranchID=" + MySession.GlobalBranchID );
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();
            riComboBoxitems2.Items.AddRange(StoreName);
            gridControlAfterAdditional.RepositoryItems.Add(riComboBoxitems2);
            GridViewAfterAddition.Columns["StoreName"].ColumnEdit = riComboBoxitems2;


            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0  and BranchID=" + MySession.GlobalBranchID );
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlAfterAdditional.RepositoryItems.Add(riComboBoxitems3);
            GridViewAfterAddition.Columns["EmpName"].ColumnEdit = riComboBoxitems3;

            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0 and BranchID=" + MySession.GlobalBranchID );
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlAfterAdditional.RepositoryItems.Add(riComboBoxitems4);
            GridViewAfterAddition.Columns[ItemName].ColumnEdit = riComboBoxitems4;
            GridViewAfterAddition.Columns["SizeID"].Visible = false;
            GridViewAfterAddition.Columns["MachineName"].ColumnEdit = riComboBoxitems;
            GridViewAfterAddition.Columns["ID"].Visible = false;
            GridViewAfterAddition.Columns["ComandID"].Visible = false;
            GridViewAfterAddition.Columns["BarcodeAdditional"].Visible = false;
            GridViewAfterAddition.Columns["EmpAdditionalID"].Visible = false; 
            GridViewAfterAddition.Columns["Cancel"].Visible = false;
            GridViewAfterAddition.Columns["BranchID"].Visible = false;
            GridViewAfterAddition.Columns["FacilityID"].Visible = false;

            GridViewAfterAddition.Columns["EditUserID"].Visible = false;
            GridViewAfterAddition.Columns["EditDate"].Visible = false;
            GridViewAfterAddition.Columns["EditTime"].Visible = false;
            GridViewAfterAddition.Columns["RegDate"].Visible = false;
            GridViewAfterAddition.Columns["UserID"].Visible = false;

            GridViewAfterAddition.Columns["ComputerInfo"].Visible = false;
            GridViewAfterAddition.Columns["EditComputerInfo"].Visible = false;
            GridViewAfterAddition.Columns["RegTime"].Visible = false;

            GridViewAfterAddition.Columns["Debit"].Visible = false;
            GridViewAfterAddition.Columns["TypeOpration"].Visible = false;
            //GridViewAfterPolish.Columns["SizeID"].Visible = false;
            GridViewAfterAddition.Columns["CostPrice"].Visible = false;

            // GridViewAfterPolish.Columns["DebitTime"].Visible = false;
            GridViewAfterAddition.Columns["MachinID"].Name = "MachinID";
            //dtItem.Columns["Losed"].ReadOnly = true;
            GridViewAfterAddition.Columns["EmpName"].Width = 120;
            GridViewAfterAddition.Columns["StoreName"].Width = 120;
            GridViewAfterAddition.Columns["EmpID"].Width = 120;
            GridViewAfterAddition.Columns["Signature"].Width = 120;
            GridViewAfterAddition.Columns["DebitDate"].Width = 110;
            GridViewAfterAddition.Columns["DebitTime"].Width = 85;
            GridViewAfterAddition.Columns["EmpID"].Visible = false;
            GridViewAfterAddition.Columns["StoreName"].Visible = false;
            GridViewAfterAddition.Columns["EmpName"].Visible = false;
            GridViewAfterAddition.Columns["StoreID"].Visible = false;
            GridViewAfterAddition.Columns["Credit"].VisibleIndex = GridViewAfterAddition.Columns[SizeName].VisibleIndex + 1;
           

            GridViewAfterAddition.Columns["MachinID"].Visible = false;
            GridViewAfterAddition.Columns["MachineName"].Visible = false;
            GridViewAfterAddition.Columns["Lost"].Visible = false;
            if (UserInfo.Language == iLanguage.Arabic)
            {
                GridViewAfterAddition.Columns["EngItemName"].Visible = false;
                GridViewAfterAddition.Columns["EngSizeName"].Visible = false;
                GridViewAfterAddition.Columns["ArbItemName"].Width = 150;
                GridViewAfterAddition.Columns["StoreID"].Caption = "رقم المخزن";
                GridViewAfterAddition.Columns["StoreName"].Caption = "إسم المخزن";

                GridViewAfterAddition.Columns["EmpID"].Caption = "رقم العامل";
                GridViewAfterAddition.Columns["EmpName"].Caption = "إسم العامل";

                GridViewAfterAddition.Columns["MachinID"].Caption = "رقم المكينة";
                GridViewAfterAddition.Columns["MachineName"].Caption = "إسم المكينة";
                GridViewAfterAddition.Columns["Debit"].Caption = "الوزن";

                GridViewAfterAddition.Columns["Credit"].Caption = "الوزن";
                GridViewAfterAddition.Columns["TypeOpration"].Caption = "نوع العملية";
                GridViewAfterAddition.Columns["Signature"].Caption = "التوقيع";

                GridViewAfterAddition.Columns["ItemID"].Caption = "رقم الصنف";
                GridViewAfterAddition.Columns["ArbItemName"].Caption = "اسم الصنف";
                GridViewAfterAddition.Columns["SizeID"].Caption = "رقم الوحده";
                GridViewAfterAddition.Columns["ArbSizeName"].Caption = "الوحده";
                GridViewAfterAddition.Columns["CostPrice"].Caption = "التكلفة";
                GridViewAfterAddition.Columns["DebitDate"].Caption = "التاريخ";
                GridViewAfterAddition.Columns["DebitTime"].Caption = "الوقت";
                GridViewAfterAddition.Columns["ShownInNext"].Caption = "يظهر في التفاصيل"; 
            }
            else
            {
                GridViewAfterAddition.Columns["ArbItemName"].Visible = false;
                GridViewAfterAddition.Columns["ArbSizeName"].Visible = false;
                GridViewAfterAddition.Columns["EngItemName"].Width = 150;
                GridViewAfterAddition.Columns["StoreID"].Caption = "Store ID";
                GridViewAfterAddition.Columns["StoreName"].Caption = "Store Name";
                GridViewAfterAddition.Columns["EngItemName"].Caption = "Item Name";
                GridViewAfterAddition.Columns["MachinID"].Caption = "Machine ID";
                GridViewAfterAddition.Columns["MachineName"].Caption = "Machin Name";
                GridViewAfterAddition.Columns["Debit"].Caption = "debtor ";
                GridViewAfterAddition.Columns["EngSizeName"].Caption = "Unit";
                GridViewAfterAddition.Columns["Credit"].Caption = "Creditor";
                GridViewAfterAddition.Columns["TypeOpration"].Caption = "Type Opration";
                GridViewAfterAddition.Columns["Signature"].Caption = "Signature";
                GridViewAfterAddition.Columns["DebitDate"].Caption = "Date";
                GridViewAfterAddition.Columns["DebitTime"].Caption = "Time";
                GridViewAfterAddition.Columns["EmpID"].Caption = "EmpID";
                GridViewAfterAddition.Columns["EmpName"].Caption = "Name";
                GridViewAfterAddition.Columns["ShownInNext"].Caption = "Shown In Next"; 
            }



        }



        #endregion
        private void frmManufacturingOrder_Load(object sender, EventArgs e)
        {
            try
            {

                initGridBeforAdditional();
                initGridAfterAdditional();
                initGridOrderDetails();
                DoNew();
                txtCustomerID.ReadOnly = true;
                txtDelegateID.ReadOnly = true;
                txtOrderDate.ReadOnly = true;
                txtGuidanceID.ReadOnly = true;
                txtTypeOrder.ReadOnly = true;

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
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
                    strSQL = "SELECT " + PrimaryName + " as CustomerName  FROM Sales_CustomerAnSublierListArb Where    AcountID =" + txtCustomerID.Text + "   and BranchID=" + MySession.GlobalBranchID ;
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


        private void txtEmpFactorID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + txtEmpID.Text + " And Cancel =0   and BranchID=" + MySession.GlobalBranchID  ;
                CSearch.ControlValidating(txtEmpID, lblEmpName, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void txtTypeOrder_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM Manu_TypeOrders WHERE  ID =" + txtTypeOrder.Text ;
                CSearch.ControlValidating(txtTypeOrder, lblTypeOrderName, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID
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
                strSQL = "SELECT " + PrimaryName + " as UserName  FROM  [Users] where [Cancel]=0  and BranchID="+MySession.GlobalBranchID +"   and [UserID]=" + txtGuidanceID.Text.ToString();
                CSearch.ControlValidating(txtGuidanceID, lblGuidanceName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public void txtOrderID_Validating(object sender, CancelEventArgs e)
        {

            try
            {
                string strSql;
                DataTable dt;
                string txtOrder = "";
                txtOrder = txtOrderID.Text;
                if (txtOrderID.Text != string.Empty && txtOrderID.Text != "0")
                {
                    int CommandIDTemp = 0;
                    CommandIDTemp = Comon.cInt(Lip.GetValue("select ComandID from Menu_FactoryRunCommandMaster where Cancel=0  and BranchID="+MySession.GlobalBranchID +"   and TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + " and ComandID<>" + Comon.cInt(txtCommandID.Text) + " and Barcode='" + txtOrderID.Text + "'"));
                    int CommandIDThis = Comon.cInt(Lip.GetValue("select ComandID from Menu_FactoryRunCommandMaster where Cancel=0  and BranchID="+MySession.GlobalBranchID +"   and TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + " and ComandID=" + Comon.cInt(txtCommandID.Text) + " and Barcode='" + txtOrderID.Text + "'"));

                    if ((MySession.GlobalDefaultCanRepetUseOrderOneOureMoreManufactory == true && CommandIDTemp > 0))
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
                        //txtOrder = txtOrderID.Text;
                        //ClearFields();
                        //string OrderID = txtOrder;
                        //txtOrderID.Text = OrderID;
                        if (CommandIDTemp > 0)
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheOrderAlreadyExists);
                        SetDetilOrder(txtOrderID.Text);
                        //IsNewRecord = true;
                        Validations.DoEditRipon(this, ribbonControl1);
                    }
                    else
                   if ((IsNewRecord))  //&& CommandIDTemp <= 0
                    {
                        if (CommandIDTemp > 0)
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheOrderAlreadyExists);
                        string OrderID = txtOrder;


                        strSQL = "SELECT * FROM Manu_OrderRestriction WHERE  OrderID ='" + OrderID.Trim() + "'  and BranchID=" + MySession.GlobalBranchID  ;
                        Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, UserInfo.Language.ToString());
                        DataTable dtt = Lip.SelectRecord(strSQL);
                        if (dtt.Rows.Count > 0)
                        {
                            SetDetilOrder(OrderID);
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
                            InitializeFormatDateEmptyDate(txtOrderDate);
                            Messages.MsgError("تنبيه", "   لا يوجد طلب بهذا الرقم   ");
                            ClearFields();
                        }
                        return;
                    }




                }
                else
                {
                    txtCustomerID.Text = "";
                    lblCustomerName.Text = "";
                    txtDelegateID.Text = "";
                    lblDelegateName.Text = "";
                    txtGuidanceID.Text = "";
                    lblGuidanceName.Text = "";
                    InitializeFormatDateEmptyDate(txtOrderDate);
                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void GetOrderDetail(string OrderID)
        {

            DataTable dt = Manu_ZirconDiamondFactoryDAL.frmGetDataDetailByOrderIDInAddtional(OrderID, Comon.cInt(MySession.GlobalBranchID), UserInfo.FacilityID, Comon.cInt(cmbTypeStage.EditValue));

            if (dt.Rows.Count > 0)
            {
                gridControlOrderDetails.DataSource = lstOrderDetails;
                if (dt.Rows.Count > 0)
                {
                    gridControlOrderDetails.DataSource = dt;
                }
            }
        }
        private void txtEmployeeStokID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtEmployeeStokID.Text) + " And Cancel =0   and BranchID=" + MySession.GlobalBranchID  ;
                CSearch.ControlValidating(txtEmployeeStokID, txtEmployeeStokName, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }

        private void frmManufacturingCommand_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the F3 key is pressed and call the Find() function if it is
            if (e.KeyCode == Keys.F3)
                Find();
            else if (e.KeyCode == Keys.F2)
                ShortcutOpen();

            // Check if the F9 key is pressed and call the DoSave() function if it is
            if (e.KeyCode == Keys.F9)
                DoSave();
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

            else if (FocusedControl.Trim() == txtEmpID.Name)
            {
                frmEmployeeFiles frm = new frmEmployeeFiles();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                }
                else
                    frm.Dispose();
                
            }
            else if (FocusedControl.Trim() == gridControlBeforeAdditional.Name)
            {

                if (GridViewBeforAddition.FocusedColumn.Name == "colItemID" || GridViewBeforAddition.FocusedColumn.Name == "col" + ItemName || GridViewBeforAddition.FocusedColumn.Name == "colBarCode")
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
                            GridViewBeforAddition.Columns[ItemName].ColumnEdit = rItem;
                            gridControlBeforeAdditional.RepositoryItems.Add(rItem);

                        };
                    }
                    else
                        frm.Dispose();
                }
                else if (GridViewBeforAddition.FocusedColumn.Name == "colSizeName" || GridViewBeforAddition.FocusedColumn.Name == "colSizeID")
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

            
            else if (FocusedControl.Trim() == gridControlAfterAdditional.Name)
            {

                if (GridViewAfterAddition.FocusedColumn.Name == "colItemID" || GridViewAfterAddition.FocusedColumn.Name == "col" + ItemName || GridViewAfterAddition.FocusedColumn.Name == "colBarCode")
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
                            GridViewAfterAddition.Columns[ItemName].ColumnEdit = rItem;
                            gridControlAfterAdditional.RepositoryItems.Add(rItem);

                        };
                    }
                    else
                        frm.Dispose();
                }
                else if (GridViewAfterAddition.FocusedColumn.Name == "colSizeName" || GridViewAfterAddition.FocusedColumn.Name == "colSizeID")
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

        #region Function

        private void CalculateFactoryLost()
        {
            try
            {
                decimal ToatlBeforFactoryQty = 0;
                decimal ToatlAfterFactoryQty = 0;
                decimal TempQTY = 0;
                decimal TempQTYAfter = 0;
                for (int i = 0; i <= GridViewBeforAddition.DataRowCount - 1; i++)
                {
                    if (Comon.cInt(GridViewBeforAddition.GetRowCellValue(i, "SizeID").ToString()) == 2)
                        TempQTY += Comon.cDec(Comon.cDec(GridViewBeforAddition.GetRowCellValue(i, "Debit").ToString()) / 5);
                    else
                        TempQTY += Comon.cDec(Comon.cDec(GridViewBeforAddition.GetRowCellValue(i, "Debit").ToString()));
                }
                for (int i = 0; i <= GridViewAfterAddition.DataRowCount - 1; i++)
                {
                    if (Comon.cInt(GridViewAfterAddition.GetRowCellValue(i, "SizeID").ToString()) == 2)
                        TempQTYAfter += Comon.cDec(Comon.cDec(GridViewAfterAddition.GetRowCellValue(i, "Credit").ToString()) / 5);
                    else
                        TempQTYAfter += Comon.cDec(Comon.cDec(GridViewAfterAddition.GetRowCellValue(i, "Credit").ToString()));
                 }
                txtTotalBefor.Text = TempQTY.ToString();
                txtTotalAfter.Text = TempQTYAfter.ToString();
                lblTotallostFactory.Text = Comon.cDec(TempQTY-TempQTYAfter) + "";
            }
            catch (Exception ex)
            {


            }


        }

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
        public void Find()
        {
            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = " Where 1=1 ";

            FocusedControl = GetIndexFocusedControl();

            if (FocusedControl == null) return;

            else if (FocusedControl.Trim() == txtStoreID.Name)
            {
                if (!MySession.GlobalAllowChangefrmAddtionalStoreID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "رقم الحساب", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "Account ID", Comon.cInt(cmbBranchesID.EditValue));
            }

            else if (FocusedControl.Trim() == txtCommandID.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }
                 if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCommandID, null, "CommandID", "رقم الأمر", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtCommandID, null, "CommandID", "Command ID", Comon.cInt(cmbBranchesID.EditValue));
            }
            //الاصناف

            else if (FocusedControl.Trim() == txtAccountID.Name)
            {
                if (!MySession.GlobalAllowChangefrmAddtionalAccountID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtAccountID, lblAccountName, "AccountID", "رقم الحساب", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtAccountID, lblAccountName, "AccountID", "Account ID", Comon.cInt(cmbBranchesID.EditValue));

            }

            else if (FocusedControl.Trim() == txtOrderID.Name)
            {
                if (MySession.GlobalDefaultCanRepetUseOrderOneOureMoreManufactory == true)
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, txtOrderID, txtOrderID, "OrderID", "رقم الطلب", Comon.cInt(cmbBranchesID.EditValue), "  and OrderID not in(select Barcode as OrderID from Menu_FactoryRunCommandMaster where Cancel=0   and BranchID="+MySession.GlobalBranchID   +"  and TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + ") ");
                    else
                        PrepareSearchQuery.Find(ref cls, txtOrderID, txtOrderID, "OrderID", "Order ID", Comon.cInt(cmbBranchesID.EditValue), "  and OrderID not in(select Barcode as OrderID  from Menu_FactoryRunCommandMaster where Cancel=0  and BranchID="+MySession.GlobalBranchID +"  and TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + ") ");
                }
                else
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, txtOrderID, txtOrderID, "OrderID", "رقم الطلب", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, txtOrderID, txtOrderID, "OrderID", "Order ID", Comon.cInt(cmbBranchesID.EditValue));
                }
            }

          
            //رقم الحساب



            //العامل
            else if (FocusedControl.Trim() == txtEmpID.Name)
            {
                if (!MySession.GlobalAllowChangefrmAddtionalEmployeeID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtEmpID, lblEmpName, "EmployeeID", "رقـم العامل", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtEmpID, lblEmpName, "EmployeeID", "Worker ID", Comon.cInt(cmbBranchesID.EditValue));
            }


            //امين المخزن
            else if (FocusedControl.Trim() == txtEmployeeStokID.Name)
            {
                if (!MySession.GlobalAllowChangefrmAddtionalEmployeeID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtEmployeeStokID, txtEmployeeStokName, "EmployeeID", "رقـم العامل", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtEmployeeStokID, txtEmployeeStokName, "EmployeeID", "Worker ID", Comon.cInt(cmbBranchesID.EditValue));
            }

 
            //الجرايد فيو

            else if (FocusedControl.Trim() == gridControlBeforeAdditional.Name)
            {
                if (GridViewBeforAddition.FocusedColumn.Name == "colBarcodeAdditional" || GridViewBeforAddition.FocusedColumn.Name == "colItemName" || GridViewBeforAddition.FocusedColumn.Name == "colItemID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "BarCode", MySession.GlobalBranchID);
                }
                if (GridViewBeforAddition.FocusedColumn.Name == "colStoreID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "رقم الـمـســتـودع", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "Store ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                
                if (GridViewBeforAddition.FocusedColumn.Name == "MachinID")
                {
                    if (GridViewBeforAddition.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "رقم المكينة", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "Machine ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                else if (GridViewBeforAddition.FocusedColumn.Name == "colSizeID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "رقـم الـوحـــده", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "Size ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                else if (GridViewBeforAddition.FocusedColumn.Name == "colEmpID")
                {
                    if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "رقـم العامل", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "Worker ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                else if (GridViewBeforAddition.FocusedColumn.Name == "colDebit")
                {
                    frmRemindQtyItem frm = new frmRemindQtyItem();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);

                        frm.Show();
                        if (GridViewBeforAddition.GetRowCellValue(GridViewBeforAddition.FocusedRowHandle, "ItemID") != null)
                            frm.SetValueToControl(GridViewBeforAddition.GetRowCellValue(GridViewBeforAddition.FocusedRowHandle, "ItemID").ToString(), txtStoreID.Text.ToString());
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
            else if (FocusedControl.Trim() == gridControlAfterAdditional.Name)
            {
                if (GridViewAfterAddition.FocusedColumn.Name == "colBarcodeAdditional" || GridViewAfterAddition.FocusedColumn.Name == "colItemName" || GridViewAfterAddition.FocusedColumn.Name == "colItemID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "BarCode", MySession.GlobalBranchID);
                }
                if (GridViewAfterAddition.FocusedColumn.Name == "colStoreID")
                {

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "رقم الـمـســتـودع", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "Store ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                 
                if (GridViewAfterAddition.FocusedColumn.Name == "MachinID")
                {
                    if (GridViewAfterAddition.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "رقم المكينة", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "Machine ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                else if (GridViewAfterAddition.FocusedColumn.Name == "colSizeID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "رقـم الـوحـــده", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "Size ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                else if (GridViewAfterAddition.FocusedColumn.Name == "colEmpID")
                {
                    if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "رقـم العامل", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "Worker ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                else if (GridViewAfterAddition.FocusedColumn.Name == "colCredit")
                {
                    frmRemindQtyItem frm = new frmRemindQtyItem();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);

                        frm.Show();
                        if (GridViewAfterAddition.GetRowCellValue(GridViewAfterAddition.FocusedRowHandle, "ItemID") != null)
                            frm.SetValueToControl(GridViewAfterAddition.GetRowCellValue(GridViewAfterAddition.FocusedRowHandle, "ItemID").ToString(), txtStoreID.Text.ToString());
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
        void FileDataMachinName(GridView Grid, string date, string time, int MachinID)
        {
            try
            {
                //Grid.AddNewRow();
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns["MachinID"], MachinID);
                strSQL = "SELECT " + PrimaryName + " FROM Menu_FactoryMachine WHERE MachineID =" + MachinID;
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns["MachineName"], Lip.GetValue(strSQL));
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[time], DateTime.Now.ToString("hh:mm:tt"));
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[date], DateTime.Now.ToString("yyyy/MM/dd"));
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleWorning, "خطأ " + ex.Message);
            }
        }

        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {

                if (FocusedControl.Trim() == txtCommandID.Name)
                {
                    txtCommandID.Text = cls.PrimaryKeyValue.ToString();
                    txtCommandID_Validating(null, null);
                }

                else if (FocusedControl == txtAccountID.Name)
                {
                    txtAccountID.Text = cls.PrimaryKeyValue.ToString();
                    txtAccountIDFactory_Validating(null, null);
                }
               

                else if (FocusedControl.Trim() == txtOrderID.Name)
                {
                    txtOrderID.Text = cls.PrimaryKeyValue.ToString();
                    txtOrderID_Validating(null, null);
                }


               

                //المخزن
                else if (FocusedControl == txtStoreID.Name)
                {
                    txtStoreID.Text = cls.PrimaryKeyValue.ToString();
                    txtStoreIDFactory_Validating(null, null);
                }


                //رقم العامل
                else if (FocusedControl == txtEmpID.Name)
                {
                    txtEmpID.Text = cls.PrimaryKeyValue.ToString();
                    txtEmpFactorID_Validating(null, null);
                }


                else if (FocusedControl == txtAccountID.Name)
                {
                    txtAccountID.Text = cls.PrimaryKeyValue.ToString();
                    txtAccountIDFactory_Validating(null, null);
                }

                //امين الخزنة
                else if (FocusedControl == txtEmployeeStokID.Name)
                {
                    txtEmployeeStokID.Text = cls.PrimaryKeyValue.ToString();
                    txtEmployeeStokID_Validating(null, null);
                }
                //الجرايد فيو
                else if (FocusedControl.Trim() == gridControlBeforeAdditional.Name)
                {
                    if (GridViewBeforAddition.FocusedColumn.Name == "colBarcodeAdditional" || GridViewBeforAddition.FocusedColumn.Name == "colItemName" || GridViewBeforAddition.FocusedColumn.Name == "colItemID")
                    {
                        string Barcode = cls.PrimaryKeyValue.ToString();
                        if (Stc_itemsDAL.CheckIfStopItemUnit(Barcode, MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                        {
                            Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                            return;
                        }
                        GridViewBeforAddition.AddNewRow();
                        GridViewBeforAddition.SetRowCellValue(GridViewBeforAddition.FocusedRowHandle, GridViewBeforAddition.Columns["BarcodeAdditional"], Barcode);
                        FillItemData(GridViewBeforAddition, gridControlBeforeAdditional, "BarcodeAdditional", "Debit", Stc_itemsDAL.GetItemData1(Barcode.ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", txtAccountID);
                    }
                    if (GridViewBeforAddition.FocusedColumn.Name == "colStoreID")
                    {
                        GridViewBeforAddition.SetRowCellValue(GridViewBeforAddition.FocusedRowHandle, GridViewBeforAddition.Columns["StoreID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0  and BranchID=" + MySession.GlobalBranchID ;
                        GridViewBeforAddition.SetRowCellValue(GridViewBeforAddition.FocusedRowHandle, GridViewBeforAddition.Columns["StoreName"], Lip.GetValue(strSQL));
                    }
                     
                    if (GridViewBeforAddition.FocusedColumn.Name == "MachinID")
                    {
                        GridViewBeforAddition.AddNewRow();
                        FileDataMachinName(GridViewBeforAddition, "DebitDate", "DebitTime", Comon.cInt(cls.PrimaryKeyValue.ToString()));
                    }
                    if (GridViewBeforAddition.FocusedColumn.Name == "colSizeID")
                    {
                        GridViewBeforAddition.SetRowCellValue(GridViewBeforAddition.FocusedRowHandle, GridViewBeforAddition.Columns["SizeID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as " + SizeName + " FROM Stc_SizingUnits WHERE SizeID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0  and BranchID=" + MySession.GlobalBranchID ;
                        GridViewBeforAddition.SetRowCellValue(GridViewBeforAddition.FocusedRowHandle, GridViewBeforAddition.Columns[SizeName], Lip.GetValue(strSQL));

                    }
                    if (GridViewBeforAddition.FocusedColumn.Name == "colEmpID")
                    {
                        GridViewBeforAddition.SetRowCellValue(GridViewBeforAddition.FocusedRowHandle, GridViewBeforAddition.Columns["EmpID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(cls.PrimaryKeyValue.ToString()) + " And Cancel =0  and BranchID=" + MySession.GlobalBranchID ;
                        GridViewBeforAddition.SetRowCellValue(GridViewBeforAddition.FocusedRowHandle, GridViewBeforAddition.Columns["EmpName"], Lip.GetValue(strSQL));
                    }
                }
                else if (FocusedControl.Trim() == gridControlAfterAdditional.Name)
                {
                    if (GridViewAfterAddition.FocusedColumn.Name == "colBarcodeAdditional" || GridViewAfterAddition.FocusedColumn.Name == "colItemName" || GridViewAfterAddition.FocusedColumn.Name == "colItemID")
                    {
                        string Barcode = cls.PrimaryKeyValue.ToString();
                        if (Stc_itemsDAL.CheckIfStopItemUnit(Barcode, MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                        {
                            Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                            return;
                        }
                        GridViewAfterAddition.AddNewRow();
                        GridViewAfterAddition.SetRowCellValue(GridViewAfterAddition.FocusedRowHandle, GridViewAfterAddition.Columns["BarcodeAdditional"], Barcode);
                         FillItemData(GridViewAfterAddition, gridControlAfterAdditional, "BarcodeAdditional", "Credit", Stc_itemsDAL.GetItemData1(Barcode.ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", txtAccountID);
                        
                    }
                    if (GridViewAfterAddition.FocusedColumn.Name == "colStoreID")
                    {
                        GridViewAfterAddition.SetRowCellValue(GridViewAfterAddition.FocusedRowHandle, GridViewAfterAddition.Columns["StoreID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID ;
                        GridViewAfterAddition.SetRowCellValue(GridViewAfterAddition.FocusedRowHandle, GridViewAfterAddition.Columns["StoreName"], Lip.GetValue(strSQL));

                    }

               
                    if (GridViewAfterAddition.FocusedColumn.Name == "MachinID")
                    {
                        GridViewAfterAddition.AddNewRow();
                        FileDataMachinName(GridViewAfterAddition, "DebitDate", "DebitTime", Comon.cInt(cls.PrimaryKeyValue.ToString()));
                    }
                    if (GridViewAfterAddition.FocusedColumn.Name == "colSizeID")
                    {
                        GridViewAfterAddition.SetRowCellValue(GridViewAfterAddition.FocusedRowHandle, GridViewAfterAddition.Columns["SizeID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as " + SizeName + " FROM Stc_SizingUnits WHERE SizeID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0  and BranchID=" + MySession.GlobalBranchID ;
                        GridViewAfterAddition.SetRowCellValue(GridViewAfterAddition.FocusedRowHandle, GridViewAfterAddition.Columns[SizeName], Lip.GetValue(strSQL));

                    }
                    if (GridViewAfterAddition.FocusedColumn.Name == "colEmpID")
                    {
                        GridViewAfterAddition.SetRowCellValue(GridViewAfterAddition.FocusedRowHandle, GridViewAfterAddition.Columns["EmpID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID ;
                        GridViewAfterAddition.SetRowCellValue(GridViewAfterAddition.FocusedRowHandle, GridViewAfterAddition.Columns["EmpName"], Lip.GetValue(strSQL));
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

            EnableGridView(GridViewBeforAddition, Value, 1);
            EnableGridView(GridViewAfterAddition, Value, 1);
        }

        void EnableGridView(GridView GridViewObj, bool Value, int flage)
        {
            foreach (GridColumn col in GridViewObj.Columns)
            {

                GridViewObj.Columns[col.FieldName].OptionsColumn.AllowEdit = Value;
                GridViewObj.Columns[col.FieldName].OptionsColumn.AllowFocus = Value;
                GridViewObj.Columns[col.FieldName].OptionsColumn.ReadOnly = !Value;

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
                    strSQL = "SELECT TOP 1 * FROM " + Menu_FactoryRunCommandMasterDAL.TableName + " Where  TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + " and   Cancel =0 and BranchID=" + MySession.GlobalBranchID ;
                    switch (Direction)
                    {
                        case xMoveFirst:
                            {
                                strSQL = strSQL + " ORDER BY " + Menu_FactoryRunCommandMasterDAL.PremaryKey + " ASC";
                                break;
                            }

                        case xMoveNext:
                            {
                                strSQL = strSQL + " And " + Menu_FactoryRunCommandMasterDAL.PremaryKey + ">" + PremaryKeyValue + " ORDER BY " + Menu_FactoryRunCommandMasterDAL.PremaryKey + " asc";
                                break;
                            }

                        case xMovePrev:
                            {
                                strSQL = strSQL + " And " + Menu_FactoryRunCommandMasterDAL.PremaryKey + "<" + PremaryKeyValue + " ORDER BY " + Menu_FactoryRunCommandMasterDAL.PremaryKey + " desc";
                                break;
                            }

                        case xMoveLast:
                            {
                                strSQL = strSQL + " ORDER BY " + Menu_FactoryRunCommandMasterDAL.PremaryKey + " DESC";
                                break;
                            }
                    }

                    int InvoicIDTemp = Comon.cInt(txtCommandID.Text);

                    InvoicIDTemp = cClass.GetRecordSetBySQL(strSQL);

                    if (cClass.FoundResult == true)
                    {
                        txtCommandID.Text = InvoicIDTemp.ToString();
                        txtCommandID_Validating(null, null);
                        //ReadRecord(InvoicIDTemp,true);

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
        protected override void DoNew()
        {
            try
            {
                IsNewRecord = true;
                txtReferanceID.Text = txtCommandID.Text = Menu_FactoryRunCommandMasterDAL.GetNewID(Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(cmbTypeStage.EditValue)).ToString();
                InitializeFormatDate(txtCommandDate);
                InitializeFormatDateEmptyDate(txtOrderDate);

                ClearFields();
                SetDefultValue();
                txtOrderID.Focus();
                EnabledControl(true);

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
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

        private void InitializeFormatDateEmptyDate(DateEdit Obj)
        {
            Obj.Properties.Mask.UseMaskAsDisplayFormat = true;
            Obj.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            Obj.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            Obj.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            Obj.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            Obj.Properties.Mask.EditMask = "dd/MM/yyyy";
            Obj.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
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
        protected override void DoSave()
        {
            try
            {
                if (!Validations.IsValidForm(this))
                    return;
                if (!Validations.IsValidFormCmb(cmbCurency))
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
                if (!Lip.CheckTheProcessesIsPosted("Menu_FactoryRunCommandMaster", Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(cmbStatus.EditValue), Comon.cLong(txtCommandID.Text), PrimeryColName: "ComandID", Where: " and  TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue)))
                {
                    Messages.MsgWarning(Messages.TitleError, Messages.msgTheProcessIsNotUpdateBecuseIsPosted);
                    return;
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

        public XtraReport Manu_FactoryFactorBefor()
        {
            string rptrptManu_FactoryFactorCommendName = "rptManu_FactoryFactorCommendBefore";
            string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\Reports\";
            //rptCompanyHeaderName += (UserInfo.Language == iLanguage.English ? "Eng" : "Arb");
            rptrptManu_FactoryFactorCommendName += "Arb";
            XtraReport rptFactoryFactor = XtraReport.FromFile(Path + rptrptManu_FactoryFactorCommendName + ".repx", true);


            var dataTable = new dsReports.rptManu_FactoryFactorCommendBeforeDataTable();
            for (int i = 0; i <= GridViewBeforAddition.DataRowCount - 1; i++)
            {
                var row = dataTable.NewRow();
                row["#"] = i + 1;
                row["MachinID"] = GridViewBeforAddition.GetRowCellValue(i, "MachinID");
                row["MachineName"] = GridViewBeforAddition.GetRowCellValue(i, "MachineName");
                row["QTY"] = GridViewBeforAddition.GetRowCellValue(i, "Debit");

                row["StoreName"] = GridViewBeforAddition.GetRowCellValue(i, "StoreName");

                row["ItemID"] = GridViewBeforAddition.GetRowCellValue(i, "ItemID");
                row["ItemName"] = GridViewBeforAddition.GetRowCellValue(i, ItemName);
                row["SizeName"] = GridViewBeforAddition.GetRowCellValue(i, SizeName);
                row["Date"] = GridViewBeforAddition.GetRowCellValue(i, "DebitDate");
                row["Time"] = GridViewBeforAddition.GetRowCellValue(i, "DebitTime");
                row["EmpName"] = GridViewBeforAddition.GetRowCellValue(i, "EmpName");

                dataTable.Rows.Add(row);
            }
            rptFactoryFactor.DataSource = dataTable;
            rptFactoryFactor.DataMember = "rptManu_FactoryFactorCommendBefore";
            return rptFactoryFactor;
        }

        public XtraReport Manu_FactoryFactorAfter()
        {
            string rptrptManu_FactoryFactorCommendName = "rptManu_FactoryFactorCommendAfter";
            string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\Reports\";
            //rptCompanyHeaderName += (UserInfo.Language == iLanguage.English ? "Eng" : "Arb");
            rptrptManu_FactoryFactorCommendName += "Arb";
            XtraReport rptFactoryFactor = XtraReport.FromFile(Path + rptrptManu_FactoryFactorCommendName + ".repx", true);


            var dataTable = new dsReports.rptManu_FactoryFactorCommendBeforeDataTable();
            for (int i = 0; i <= GridViewAfterAddition.DataRowCount - 1; i++)
            {
                var row = dataTable.NewRow();
                row["#"] = i + 1;
                row["MachinID"] = GridViewAfterAddition.GetRowCellValue(i, "MachinID");
                row["MachineName"] = GridViewAfterAddition.GetRowCellValue(i, "MachineName");
                row["QTY"] = GridViewAfterAddition.GetRowCellValue(i, "Credit");

                row["StoreName"] = GridViewAfterAddition.GetRowCellValue(i, "StoreName");

                row["ItemID"] = GridViewAfterAddition.GetRowCellValue(i, "ItemID");
                row["ItemName"] = GridViewAfterAddition.GetRowCellValue(i, ItemName);
                row["SizeName"] = GridViewAfterAddition.GetRowCellValue(i, SizeName);
                row["Date"] = GridViewAfterAddition.GetRowCellValue(i, "DebitDate");
                row["Time"] = GridViewAfterAddition.GetRowCellValue(i, "DebitTime");
                row["EmpName"] = GridViewAfterAddition.GetRowCellValue(i, "EmpName");



                dataTable.Rows.Add(row);
            }
            rptFactoryFactor.DataSource = dataTable;
            rptFactoryFactor.DataMember = "rptManu_FactoryFactorCommendAfter";
            return rptFactoryFactor;
        }


     
        #endregion
        List<Manu_AllOrdersDetails> SaveOrderDetials()
        {

            Manu_AllOrdersDetails returned = new Manu_AllOrdersDetails();
            List<Manu_AllOrdersDetails> listreturned = new List<Manu_AllOrdersDetails>();
            for (int i = 0; i <= GridViewBeforAddition.DataRowCount - 1; i++)
            {
                returned = new Manu_AllOrdersDetails();
                returned.ID = i + 1;
                returned.CommandID = Comon.cInt(txtCommandID.Text);
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.BarCode = GridViewBeforAddition.GetRowCellValue(i, "BarcodeAdditional").ToString();
                returned.ItemID = Comon.cInt(GridViewBeforAddition.GetRowCellValue(i, "ItemID").ToString());
                returned.TypeStageID = Comon.cInt(cmbTypeStage.EditValue);
                returned.SizeID = Comon.cInt(GridViewBeforAddition.GetRowCellValue(i, "SizeID").ToString());
                returned.ArbSizeName = GridViewBeforAddition.GetRowCellValue(i, SizeName).ToString();
                returned.EngSizeName = GridViewBeforAddition.GetRowCellValue(i, SizeName).ToString();
                returned.ArbItemName = GridViewBeforAddition.GetRowCellValue(i, ItemName).ToString();
                returned.EngItemName = GridViewBeforAddition.GetRowCellValue(i, ItemName).ToString();
                returned.QTY = Comon.ConvertToDecimalQty(GridViewBeforAddition.GetRowCellValue(i, "Debit").ToString());
                returned.CostPrice = 0;
                returned.TotalCost = 0;
                listreturned.Add(returned);
            }
            int LengBefore = GridViewBeforAddition.DataRowCount + 1;
            for (int i = 0; i <= GridViewAfterAddition.DataRowCount - 1; i++)
            {
                returned = new Manu_AllOrdersDetails();
                returned.ID = LengBefore;
                returned.CommandID = Comon.cInt(txtCommandID.Text);
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.BarCode = GridViewAfterAddition.GetRowCellValue(i, "BarcodeAdditional").ToString();
                returned.ItemID = Comon.cInt(GridViewAfterAddition.GetRowCellValue(i, "ItemID").ToString());
                returned.TypeStageID = Comon.cInt(cmbTypeStage.EditValue);
                returned.SizeID = Comon.cInt(GridViewAfterAddition.GetRowCellValue(i, "SizeID").ToString());
                returned.ArbSizeName = GridViewAfterAddition.GetRowCellValue(i, SizeName).ToString();
                returned.EngSizeName = GridViewAfterAddition.GetRowCellValue(i, SizeName).ToString();
                returned.ArbItemName = GridViewAfterAddition.GetRowCellValue(i, ItemName).ToString();
                returned.EngItemName = GridViewAfterAddition.GetRowCellValue(i, ItemName).ToString();
                returned.ShownInNext = Comon.cbool(GridViewAfterAddition.GetRowCellValue(i, "ShownInNext").ToString());
                returned.QTY = Comon.ConvertToDecimalQty(GridViewAfterAddition.GetRowCellValue(i, "Credit").ToString());
                returned.CostPrice = 0;
                returned.TotalCost = 0;
                listreturned.Add(returned);
                LengBefore++;
            }
            return listreturned;
        }

        private void Save()
        {

            {
                GridViewBeforAddition.MoveLast();
                GridViewAfterAddition.MoveLast();
                Menu_FactoryRunCommandMaster objRecord = new Menu_FactoryRunCommandMaster();
                objRecord.Barcode = txtOrderID.Text.ToString();
                objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                objRecord.BrandID = Comon.cInt(cmbBranchesID.EditValue);
                objRecord.Cancel = 0;
                objRecord.TypeStageID = Comon.cInt(cmbTypeStage.EditValue);
                objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue);
                objRecord.CurrencyName = cmbCurency.Text.ToString();
                objRecord.PeiceName = lblTypeOrderName.Text + "";
                objRecord.ComandID = Comon.cInt(txtCommandID.Text);
                objRecord.CostCenterID = Comon.cInt(MySession.GlobalDefaultCostCenterID);
                objRecord.CustomerID = Comon.cDbl(txtCustomerID.Text);
                objRecord.DocumentID = Comon.cInt(txtReferanceID.Text);
                objRecord.EmpFactorID = Comon.cDbl(txtEmpID.Text);
                objRecord.EmployeeID = Comon.cDbl(txtEmployeeStokID.Text);
                objRecord.EmployeeStokID = Comon.cDbl(txtEmployeeStokID.Text); 
                objRecord.EmpPrentagID = 0;
                objRecord.FacilityID = UserInfo.FacilityID;
                objRecord.ComandDate = Comon.ConvertDateToSerial(txtCommandDate.Text.ToString());
                objRecord.GoldCompundNet = 0;
                objRecord.GroupID = 0;
                objRecord.ItemID = 0;
                objRecord.DelegateID = Comon.cDbl(txtDelegateID.Text);
                //الحسابات
                objRecord.Posted = Comon.cInt(cmbStatus.EditValue);
                objRecord.AccountIDFactory = Comon.cDbl(txtAccountID.Text);
                objRecord.StoreIDFactory = Comon.cDbl(txtStoreID.Text);
                objRecord.EmployeeStokIDFactory = Comon.cDbl(txtEmployeeStokID.Text);
                objRecord.EmpIDFactor = Comon.cDbl(txtEmpID.Text);

                objRecord.AccountIDPrentage = 0;
                objRecord.StoreIDPrentage = 0;
                objRecord.EmployeeStokIDPrentage = 0;
                objRecord.EmpIDPrentage = 0;
                objRecord.AccountIDBeforCompond = 0;
                objRecord.StoreIDBeforComond = 0;
                objRecord.EmployeeStokIDBeforCompond = 0;
                objRecord.EmpIDBeforCompond = 0;
                objRecord.AccountIDAdditions = 0;
                objRecord.StoreIDAdditions = 0;
                objRecord.EmployeeStokIDAdditions = 0;
                objRecord.EmpIDAdditions = 0;
                objRecord.AccountIDPolishing = 0;
                objRecord.StoreIDPolishing = 0;
                objRecord.EmployeeStokIDPolishing = 0;
                objRecord.EmplooyIDPolishing = 0;
                objRecord.AccountIDBarcodeItem = 0;
                objRecord.StoreIDBarcod = 0;
                objRecord.EmployeeStokIDBarcode = 0;
                objRecord.Notes = txtNotes.Text;
                objRecord.SpendAmount = 0;

                if (pictureEdit1.Image != null && pictureEdit1.Image.RawFormat != null)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        pictureEdit1.Image.Save(stream, pictureEdit1.Image.RawFormat);
                        objRecord.InvoiceImage = stream.ToArray();
                    }
                }
                if (pictureEdit2.Image != null && pictureEdit2.Image.RawFormat != null)
                {
                    using (MemoryStream stream2 = new MemoryStream())
                    {
                        pictureEdit2.Image.Save(stream2, pictureEdit2.Image.RawFormat);
                        objRecord.InvoiceImage2 = stream2.ToArray();
                    }
                }
                objRecord.netGoldWeight = 0;
                objRecord.OpretionID = Comon.cInt(txtTypeOrder.Text);
                objRecord.TypeID = Comon.cInt(txtTypeOrder.Text);
                objRecord.ThefactoriID = Comon.cInt(txtCommandID.Text);
                objRecord.TotalLost = Comon.cDbl(lblTotallostFactory.Text);
                objRecord.piece = 1;

                objRecord.GivenDate = Comon.ConvertDateToSerial(txtCommandDate.EditValue.ToString());
                objRecord.GivenTime = 0;


                #region Save Additional

                Menu_FactoryRunCommandSelver returnedAdditional;
                List<Menu_FactoryRunCommandSelver> listreturnedAdditional = new List<Menu_FactoryRunCommandSelver>();

                //تلميع 
                int lengthAdditional = GridViewBeforAddition.DataRowCount;
                int lengthAfterAdditional = GridViewAfterAddition.DataRowCount;
                if (lengthAdditional > 0)
                {
                   
                    {
                        for (int i = 0; i < lengthAdditional; i++)
                        {
                            returnedAdditional = new Menu_FactoryRunCommandSelver();
                            returnedAdditional.ID = i + 1;
                            returnedAdditional.ComandID = Comon.cInt(txtCommandID.Text.ToString());

                            returnedAdditional.Debit = Comon.cDbl(GridViewBeforAddition.GetRowCellValue(i, "Debit").ToString());
                            returnedAdditional.TypeOpration = 1;
                            returnedAdditional.StoreID = Comon.cInt(txtStoreID.Text.ToString());
                            returnedAdditional.StoreName = lblStoreName.Text.ToString();
                            returnedAdditional.BarcodeAdditional = GridViewBeforAddition.GetRowCellValue(i, "BarcodeAdditional").ToString();
                            returnedAdditional.EmpID = txtEmpID.Text.ToString();
                            returnedAdditional.EmpName = lblEmpName.Text.ToString();

                            returnedAdditional.SizeID = Comon.cInt(GridViewBeforAddition.GetRowCellValue(i, "SizeID").ToString());
                            returnedAdditional.ItemID = Comon.cInt(GridViewBeforAddition.GetRowCellValue(i, "ItemID").ToString());
                            returnedAdditional.DebitDate = Comon.cDate(GridViewBeforAddition.GetRowCellValue(i, "DebitDate").ToString());
                            returnedAdditional.DebitTime = GridViewBeforAddition.GetRowCellValue(i, "DebitTime").ToString();
                            returnedAdditional.ArbItemName = GridViewBeforAddition.GetRowCellValue(i, ItemName).ToString();
                            returnedAdditional.EngItemName = GridViewBeforAddition.GetRowCellValue(i, ItemName).ToString();
                            returnedAdditional.ArbSizeName = GridViewBeforAddition.GetRowCellValue(i, SizeName).ToString();
                            returnedAdditional.EngSizeName = GridViewBeforAddition.GetRowCellValue(i, SizeName).ToString();
                            returnedAdditional.BranchID = UserInfo.BRANCHID;
                            returnedAdditional.EmpAdditionalID = Comon.cDbl(txtEmpID.Text);
                            returnedAdditional.Cancel = 0;
                            returnedAdditional.UserID = UserInfo.ID;
                            returnedAdditional.RegDate = Comon.ConvertDateToSerial(Lip.GetServerDate());
                            returnedAdditional.RegTime = Comon.cDbl(Lip.GetServerTimeSerial());
                            returnedAdditional.ComputerInfo = UserInfo.ComputerInfo;
                            returnedAdditional.FacilityID = UserInfo.FacilityID;
                            if (IsNewRecord == false)
                            {
                                returnedAdditional.EditUserID = UserInfo.ID;
                                returnedAdditional.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                                returnedAdditional.EditDate = Comon.ConvertDateToSerial(Lip.GetServerDate());
                                returnedAdditional.EditComputerInfo = UserInfo.ComputerInfo;
                            }
                            listreturnedAdditional.Add(returnedAdditional);
                        }
                        if (lengthAfterAdditional > 0)
                        {
                            for (int i = 0; i < lengthAfterAdditional; i++)
                            {
                                returnedAdditional = new Menu_FactoryRunCommandSelver();
                                returnedAdditional.ID = i + 1;
                                returnedAdditional.ComandID = Comon.cInt(txtCommandID.Text.ToString());
                                 
                                returnedAdditional.Credit = Comon.cDbl(GridViewAfterAddition.GetRowCellValue(i, "Credit").ToString());
                                returnedAdditional.TypeOpration = 2;

                                returnedAdditional.StoreID = Comon.cInt(txtStoreID.Text.ToString());
                                returnedAdditional.StoreName = lblStoreName.Text.ToString();

                                returnedAdditional.EmpID = txtEmpID.Text.ToString();
                                returnedAdditional.EmpName = lblEmpName.Text.ToString();
                                returnedAdditional.BarcodeAdditional = GridViewAfterAddition.GetRowCellValue(i, "BarcodeAdditional").ToString();
                                returnedAdditional.SizeID = Comon.cInt(GridViewAfterAddition.GetRowCellValue(i, "SizeID").ToString());
                                returnedAdditional.ItemID = Comon.cInt(GridViewAfterAddition.GetRowCellValue(i, "ItemID").ToString());
                                returnedAdditional.DebitDate = Comon.cDate(GridViewAfterAddition.GetRowCellValue(i, "DebitDate").ToString());
                                returnedAdditional.ShownInNext = Comon.cbool(GridViewAfterAddition.GetRowCellValue(i, "ShownInNext").ToString());
                                returnedAdditional.DebitTime = GridViewAfterAddition.GetRowCellValue(i, "DebitTime").ToString();
                                returnedAdditional.ArbItemName = GridViewAfterAddition.GetRowCellValue(i, ItemName).ToString();
                                returnedAdditional.EngItemName = GridViewAfterAddition.GetRowCellValue(i, ItemName).ToString();
                                returnedAdditional.ArbSizeName = GridViewAfterAddition.GetRowCellValue(i, SizeName).ToString();
                                returnedAdditional.EngSizeName = GridViewAfterAddition.GetRowCellValue(i, SizeName).ToString(); 
                                returnedAdditional.BranchID = UserInfo.BRANCHID;
                                returnedAdditional.EmpAdditionalID = Comon.cDbl(txtEmpID.Text);
                                returnedAdditional.Cancel = 0;
                                returnedAdditional.UserID = UserInfo.ID;
                                returnedAdditional.RegDate = Comon.ConvertDateToSerial(Lip.GetServerDate());
                                returnedAdditional.RegTime = Comon.cDbl(Lip.GetServerTimeSerial());
                                returnedAdditional.ComputerInfo = UserInfo.ComputerInfo;
                                returnedAdditional.FacilityID = UserInfo.FacilityID;
                                if (IsNewRecord == false)
                                {
                                    returnedAdditional.EditUserID = UserInfo.ID;
                                    returnedAdditional.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                                    returnedAdditional.EditDate = Comon.ConvertDateToSerial(Lip.GetServerDate());
                                    returnedAdditional.EditComputerInfo = UserInfo.ComputerInfo;
                                }
                                listreturnedAdditional.Add(returnedAdditional);
                            }


                        }
                    }
                }
                #endregion

                if (listreturnedAdditional.Count > 0)
                {
                    objRecord.Menu_F_Selver = listreturnedAdditional;

                    objRecord.Manu_OrderDetils = SaveOrderDetials();

                    string Result = Menu_FactoryRunCommandMasterDAL.InsertUsingXML(objRecord, IsNewRecord).ToString();
                    if (Comon.cInt(Result) > 0 && Comon.cInt(cmbStatus.EditValue)>1)
                    {
                        //أوامر الصرف والتوريد الخاص بالتصنيع
                        if (lengthAdditional > 0)
                        {
                            //أوامر الصرف والتوريد الخاص بالبرنتاج
                            //SaveOutOnPolshin(); //حفظ   الصرف المخزني
                            // حفظ الحركة المخزنية 
                            if (Comon.cInt(Result) > 0)
                            {
                                int MoveID = SaveStockMoveingBrntageOut(Comon.cInt(Result));
                                if (MoveID == 0)
                                    Messages.MsgError(Messages.TitleInfo, "خطا في حفظ الحركة المخزنية اضافات - قبل ");
                                //حفظ القيد الالي

                                long VoucherID = SaveVariousVoucherMachinPolshin(Comon.cInt(Result), IsNewRecord);
                                if (VoucherID == 0)
                                    Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                                else
                                    Lip.ExecututeSQL("Update " + Menu_FactoryRunCommandSelverDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Menu_FactoryRunCommandSelverDAL.PremaryKey + " = " + Result + " and BranchID=" + MySession.GlobalBranchID);

                            }
                        }
                        if (lengthAfterAdditional > 0)
                        {
                            //SaveInOnPolshin(); //حفظ   التوريد المخزني
                            // حفظ الحركة المخزنية 
                            if (Comon.cInt(Result) > 0)
                            {
                                bool isNew = true;
                                DataTable dtCount = null;
                                dtCount = Stc_ItemsMoviingDAL.GetCountElementID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(Result), DocumentTypeAdditionalAfter);
                                if (Comon.cInt(dtCount.Rows[0][0]) > 0)
                                    isNew = false;

                                int MoveID = SaveStockMoveingBrntageIn(Comon.cInt(Result));
                                if (MoveID == 0)
                                    Messages.MsgError(Messages.TitleInfo, "خطا في حفظ الحركة المخزنية اضافات - بعد");

                                //حفظ القيد الالي
                                long VoucherID = SaveVariousVoucherMachinInOnPolshin(Comon.cInt(Result), isNew);
                                if (VoucherID == 0)
                                    Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                                else
                                    Lip.ExecututeSQL("Update " + Menu_FactoryRunCommandSelverDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Menu_FactoryRunCommandSelverDAL.PremaryKey + " = " + Result + " and BranchID=" + MySession.GlobalBranchID);

                            }
                        }

                    }
                    if (Comon.cInt(Result) > 0)
                    {

                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                        ClearFields();
                    }
                    else
                    {
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgErrorSave + " " + Result);
                    }
                }
            }
        }

        #region Save In,Out  Factory
       
        long SaveVariousVoucherMachinInOnPolshin(int DocumentID, bool isNew)
        {
            int VoucherID = 0;
            long Result = 0;
            Acc_VariousVoucherMachinMaster objRecord = new Acc_VariousVoucherMachinMaster();
            objRecord.DocumentType = DocumentTypeAdditionalAfter;
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " And DocumentType=" + objRecord.DocumentType + " And BranchID=" + Comon.cInt(cmbBranchesID.EditValue)));

            objRecord.VoucherID = VoucherID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = MySession.GlobalFacilityID;
            //Date
            objRecord.VoucherDate = Comon.ConvertDateToSerial(((DateTime)GridViewAfterAddition.GetRowCellValue(GridViewAfterAddition.DataRowCount - 1, "DebitDate")).ToString("dd/MM/yyyy")).ToString(); 
            
            objRecord.CurrencyID = Comon.cInt(MySession.GlobalDefaultCurencyID);
            //objRecord.CurrencyName = cmbCurency.Text.ToString();
            //objRecord.CurrencyPrice = Comon.cDec(txtCurrncyPrice.Text);
            //objRecord.CurrencyEquivalent = Comon.cDec(lblcurrncyEquvilant.Text);

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
            returned.AccountID = Comon.cLong(txtStoreID.Text);
            returned.VoucherID = VoucherID;
            double txtTotalQty_PolshinAfter = 0;
            for (int i = 0; i < GridViewAfterAddition.DataRowCount; i++)
            {
                txtTotalQty_PolshinAfter += Comon.cDbl(GridViewAfterAddition.GetRowCellValue(i, "Credit").ToString());
            }
            returned.DebitGold = Comon.cDbl(txtTotalQty_PolshinAfter);
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(MySession.GlobalDefaultCostCenterID);

            returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
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
            returned.CreditGold = Comon.cDbl(txtTotalQty_PolshinAfter);
            returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
            returned.CostCenterID = Comon.cInt(MySession.GlobalDefaultCostCenterID);

            returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
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
        long SaveVariousVoucherMachinPolshin(int DocumentID, bool isNew)
        {
            int VoucherID = 0;
            long Result = 0;
            Acc_VariousVoucherMachinMaster objRecord = new Acc_VariousVoucherMachinMaster();
            objRecord.DocumentType = DocumentTypeAdditionalBefore;
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " And DocumentType=" + objRecord.DocumentType + " And BranchID=" + Comon.cInt(cmbBranchesID.EditValue)));

            objRecord.VoucherID = VoucherID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = MySession.GlobalFacilityID;
            //Date
            objRecord.VoucherDate = Comon.ConvertDateToSerial(((DateTime)GridViewBeforAddition.GetRowCellValue(GridViewBeforAddition.DataRowCount - 1, "DebitDate")).ToString("dd/MM/yyyy")).ToString(); 
            
            objRecord.CurrencyID = Comon.cInt(MySession.GlobalDefaultCurencyID);
            //objRecord.CurrencyName = cmbCurency.Text.ToString();
            //objRecord.CurrencyPrice = Comon.cDec(txtCurrncyPrice.Text);
            //objRecord.CurrencyEquivalent = Comon.cDec(lblcurrncyEquvilant.Text);

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
            if (IsNewRecord == false)
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
            double txtTotalQty_PolshinBefore = 0;
            for (int i = 0; i < GridViewBeforAddition.DataRowCount; i++)
            {
                txtTotalQty_PolshinBefore += Comon.cDbl(GridViewBeforAddition.GetRowCellValue(i, "Debit").ToString());
            }
            returned.DebitGold = Comon.cDbl(txtTotalQty_PolshinBefore);
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(MySession.GlobalDefaultCostCenterID);

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
            returned.CreditGold = Comon.cDbl(txtTotalQty_PolshinBefore);
            returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
            returned.CostCenterID = Comon.cInt(MySession.GlobalDefaultCostCenterID);

            returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
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
        //private void SaveOutOnPolshin()
        //{
        //    #region Save Out On
        //    //Save Out On
        //    bool isNew = IsNewRecord;
        //    Stc_ManuFactoryCommendOutOnBail_Master objRecordOutOnMaster = new Stc_ManuFactoryCommendOutOnBail_Master();
        //    if (IsNewRecord)
        //        objRecordOutOnMaster.InvoiceID = Comon.cInt(Stc_ManuFactoryCommendOutOnBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID, 1));
        //    else
        //    {
        //        DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypePloshinBefore);
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
        //    objRecordOutOnMaster.CurrencyID = Comon.cInt(MySession.GlobalDefaultCurencyID);
        //    objRecordOutOnMaster.ReferanceID = Comon.cInt(txtReferanceID.Text);
        //    objRecordOutOnMaster.TypeCommand = 1;
        //    objRecordOutOnMaster.DocumentType = DocumentTypePloshinBefore;
        //    objRecordOutOnMaster.Cancel = 0;
        //    objRecordOutOnMaster.DebitAccount = Comon.cDbl(txtAccountID.Text);
        //    objRecordOutOnMaster.StoreID = Comon.cDbl(txtStoreID.Text);
        //    objRecordOutOnMaster.Notes = txtNotes.Text;
        //    objRecordOutOnMaster.CostCenterID = Comon.cInt(MySession.GlobalDefaultCostCenterID);
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
        //    for (int i = 0; i <= GridViewBeforAddition.DataRowCount - 1; i++)
        //    {
        //        returnedOutOn = new Stc_ManuFactoryCommendOutOnBail_Details();
        //        returnedOutOn.InvoiceID = objRecordOutOnMaster.InvoiceID;
        //        returnedOutOn.CommandID = Comon.cInt(txtCommandID.Text);
        //        returnedOutOn.FacilityID = UserInfo.FacilityID;
        //        returnedOutOn.BranchID = Comon.cInt(cmbBranchesID.EditValue);
        //        returnedOutOn.CommandDate = Comon.cDate(GridViewBeforAddition.GetRowCellValue(i, "DebitDate").ToString());
        //        returnedOutOn.CommandTime = (Comon.cDateTime(GridViewBeforAddition.GetRowCellValue(i, "DebitTime")).ToShortTimeString());
        //        //returnedOutOn.BarCode = GridViewBeforPolish.GetRowCellValue(i, "BarcodeAdditional").ToString();
        //        returnedOutOn.ItemID = Comon.cInt(GridViewBeforAddition.GetRowCellValue(i, "ItemID").ToString());
        //        returnedOutOn.SizeID = Comon.cInt(GridViewBeforAddition.GetRowCellValue(i, "SizeID").ToString());
        //        returnedOutOn.QTY = Comon.cDbl(GridViewBeforAddition.GetRowCellValue(i, "Debit").ToString());
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
        //            long VoucherID = SaveVariousVoucherMachinPolshin(Comon.cInt(objRecordOutOnMaster.InvoiceID), isNew);
        //            if (VoucherID == 0)
        //                Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
        //            else
        //                Lip.ExecututeSQL("Update " + Menu_FactoryRunCommandSelverDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Menu_FactoryRunCommandSelverDAL.PremaryKey + " = " + txtCommandID.Text);
        //        }
        //    }
        //    #endregion
        //}
        //private void SaveInOnPolshin()
        //{
        //    #region Save Out On
        //    //Save Out On
        //    bool isNew = IsNewRecord;
        //    Stc_ManuFactoryCommendOutOnBail_Master objRecordInOnMaster = new Stc_ManuFactoryCommendOutOnBail_Master();
        //    if (IsNewRecord)
        //        objRecordInOnMaster.InvoiceID = Comon.cInt(Stc_ManuFactoryCommendOutOnBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID, 2));
        //    else
        //    {
        //        DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypePolshinAfter);
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

        //    objRecordInOnMaster.CurrencyID = Comon.cInt(MySession.GlobalDefaultCurencyID);
        //    objRecordInOnMaster.ReferanceID = Comon.cInt(txtReferanceID.Text);
        //    objRecordInOnMaster.TypeCommand = 2;
        //    objRecordInOnMaster.DocumentType = DocumentTypePolshinAfter;
        //    objRecordInOnMaster.Cancel = 0;
        //    objRecordInOnMaster.DebitAccount = Comon.cDbl(txtAccountID.Text);
        //    objRecordInOnMaster.StoreID = Comon.cDbl(txtStoreID.Text);
        //    objRecordInOnMaster.Notes = txtNotes.Text;
        //    objRecordInOnMaster.CostCenterID = Comon.cInt(MySession.GlobalDefaultCostCenterID);
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
        //    for (int i = 0; i <= GridViewAfterPolish.DataRowCount - 1; i++)
        //    {
        //        returnedOutOn = new Stc_ManuFactoryCommendOutOnBail_Details();
        //        returnedOutOn.InvoiceID = objRecordInOnMaster.InvoiceID;
        //        returnedOutOn.CommandID = Comon.cInt(txtCommandID.Text);
        //        returnedOutOn.FacilityID = UserInfo.FacilityID;
        //        returnedOutOn.BranchID = Comon.cInt(cmbBranchesID.EditValue);
        //        returnedOutOn.CommandDate = Comon.cDate(GridViewAfterPolish.GetRowCellValue(i, "DebitDate").ToString());
        //        returnedOutOn.CommandTime = (Comon.cDateTime(GridViewAfterPolish.GetRowCellValue(i, "DebitTime")).ToShortTimeString());
        //        returnedOutOn.BarCode = GridViewAfterPolish.GetRowCellValue(i, "BarcodeAdditional").ToString();
        //        returnedOutOn.ItemID = Comon.cInt(GridViewAfterPolish.GetRowCellValue(i, "ItemID").ToString());
        //        returnedOutOn.SizeID = Comon.cInt(GridViewAfterPolish.GetRowCellValue(i, "SizeID").ToString());
        //        returnedOutOn.QTY = Comon.cDbl(GridViewAfterPolish.GetRowCellValue(i, "Credit").ToString());
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
        //            long VoucherID = SaveVariousVoucherMachinInOnPolshin(Comon.cInt(objRecordInOnMaster.InvoiceID), isNew);
        //            if (VoucherID == 0)
        //                Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
        //            else
        //                Lip.ExecututeSQL("Update " + Menu_FactoryRunCommandSelverDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Menu_FactoryRunCommandSelverDAL.PremaryKey + " = " + txtCommandID.Text);
        //        }
        //    }
        //    #endregion
        //}
        private int SaveStockMoveingBrntageOut(int DocumentID)
        {
            Stc_ItemsMoviing objRecord = new Stc_ItemsMoviing();
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.DocumentTypeID = DocumentTypeAdditionalBefore;
            objRecord.MoveType = 2;
            objRecord.MoveID = 0;
            objRecord.TranseID = DocumentID;
            objRecord.Posted = Comon.cInt(cmbStatus.EditValue);
            Stc_ItemsMoviing returned;
            List<Stc_ItemsMoviing> listreturned = new List<Stc_ItemsMoviing>();
            for (int i = 0; i <= GridViewBeforAddition.DataRowCount - 1; i++)
            {
                returned = new Stc_ItemsMoviing();
                returned.ID = i + 1;
                returned.MoveDate = Comon.ConvertDateToSerial(((DateTime)GridViewBeforAddition.GetRowCellValue(i, "DebitDate")).ToString("dd/MM/yyyy")).ToString(); 
                returned.MoveID = 0;
                returned.TranseID = DocumentID;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.DocumentTypeID = DocumentTypeAdditionalBefore;
                returned.MoveType = 2;
                returned.StoreID = Comon.cDbl(txtStoreID.Text.ToString());
                returned.AccountID = Comon.cDbl(txtAccountID.Text);
                returned.BarCode = GridViewBeforAddition.GetRowCellValue(i, "BarcodeAdditional").ToString();
                returned.ItemID = Comon.cInt(GridViewBeforAddition.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(GridViewBeforAddition.GetRowCellValue(i, "SizeID").ToString());
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID + " and BranchID=" + MySession.GlobalBranchID));
                returned.QTY = Comon.cDbl(GridViewBeforAddition.GetRowCellValue(i, "Debit").ToString());
                returned.InPrice = 0;
                returned.OutPrice = 0;
                //returned.Bones = Comon.cDbl(GridViewBeforfactory.GetRowCellValue(i, "Bones").ToString());
                returned.OutPrice = Comon.cDbl(Lip.AverageUnit(Comon.cInt(returned.ItemID), Comon.cInt(returned.SizeID), Comon.cDbl(txtStoreID.Text)));
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
        private int SaveStockMoveingBrntageIn(int DocumentID)
        {
            Stc_ItemsMoviing objRecord = new Stc_ItemsMoviing();
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.DocumentTypeID = DocumentTypeAdditionalAfter;
            objRecord.MoveType = 1;
            objRecord.MoveID = 0;
            objRecord.TranseID = DocumentID;
            objRecord .Posted = Comon.cInt(cmbStatus.EditValue);
            Stc_ItemsMoviing returned;
            List<Stc_ItemsMoviing> listreturned = new List<Stc_ItemsMoviing>();
            for (int i = 0; i <= GridViewAfterAddition.DataRowCount - 1; i++)
            {
                returned = new Stc_ItemsMoviing();
                returned.ID = i + 1;
                returned.MoveDate = Comon.ConvertDateToSerial(((DateTime)GridViewAfterAddition.GetRowCellValue(i, "DebitDate")).ToString("dd/MM/yyyy")).ToString();
                returned.MoveID = 0;
                returned.TranseID = DocumentID;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.DocumentTypeID = DocumentTypeAdditionalAfter;
                returned.MoveType = 1;
                returned.StoreID = Comon.cDbl(txtStoreID.Text.ToString());
                returned.AccountID = Comon.cDbl(txtAccountID.Text);
                returned.BarCode = GridViewAfterAddition.GetRowCellValue(i, "BarcodeAdditional").ToString();
                returned.ItemID = Comon.cInt(GridViewAfterAddition.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(GridViewAfterAddition.GetRowCellValue(i, "SizeID").ToString());
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID + " and BranchID=" + MySession.GlobalBranchID));
                returned.QTY = Comon.cDbl(GridViewAfterAddition.GetRowCellValue(i, "Credit").ToString());
                returned.InPrice = Comon.cDbl(Lip.AverageUnit(Comon.cInt(returned.ItemID), Comon.cInt(returned.SizeID), Comon.cDbl(txtStoreID.Text)));
                //returned.Bones = Comon.cDbl(GridCastingBefore.GetRowCellValue(i, "Bones").ToString());
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
        #endregion



        int DeleteStockMoving(int DocumentID, int DocumentType)
        {
            int Result = -1;
            Stc_ItemsMoviing objRecord = new Stc_ItemsMoviing();
            objRecord.DocumentTypeID = DocumentType;
            objRecord.TranseID = DocumentID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
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
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " And DocumentType=" + objRecord.DocumentType + " And BranchID=" + Comon.cInt(Comon.cInt(cmbBranchesID.EditValue))));
            objRecord.VoucherID = VoucherID;
            objRecord.EditUserID = UserInfo.ID;
            objRecord.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
            objRecord.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecord.EditComputerInfo = UserInfo.ComputerInfo;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
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
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
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

                Menu_FactoryRunCommandMaster model = new Menu_FactoryRunCommandMaster();
                model.ComandID = Comon.cInt(txtCommandID.Text);
                model.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                model.FacilityID = UserInfo.FacilityID;
                model.TypeStageID = Comon.cInt(cmbTypeStage.EditValue);
                model.Barcode = txtOrderID.Text; 
                string Result = Menu_FactoryRunCommandMasterDAL.Delete(model).ToString();
                //حذف الحركة المخزنية 
                if (Comon.cInt(Result) > 0)
                {
                    int MoveID = 0;
                    MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text), DocumentTypeAdditionalAfter);
                    MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text), DocumentTypeAdditionalBefore);
                   
                    if (MoveID < 0)
                        Messages.MsgError(Messages.TitleInfo, "خطا في حذف الحركة  المخزنية");
                }

                #region Delete Voucher Machin
                //حذف القيد الالي
                if (Comon.cInt(Result) > 0)
                {
                    int VoucherID = 0;


                    DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(txtCommandID.Text), DocumentTypeAdditionalBefore);
                    if (dtInvoiceID.Rows.Count > 0)
                    {
                        VoucherID = DeleteVariousVoucherMachin(Comon.cInt(dtInvoiceID.Rows[0][0]), DocumentTypeAdditionalBefore);
                        if (VoucherID == 0)
                            Messages.MsgError(Messages.TitleInfo, "خطا في حذف قيد العملية اضافات - قبل ");
                    }
                    int VoucherIDAfter = 0;
                    DataTable dtInvoiceIDAfter = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(txtCommandID.Text), DocumentTypeAdditionalAfter);
                    if (dtInvoiceIDAfter.Rows.Count > 0)
                    {
                        VoucherIDAfter = DeleteVariousVoucherMachin(Comon.cInt(dtInvoiceIDAfter.Rows[0][0]), DocumentTypeAdditionalAfter);
                        if (VoucherIDAfter == 0)
                            Messages.MsgError(Messages.TitleInfo, "خطا في حذف قيد العملية اضافات -بعد");
                    }

                     
                }
                #endregion

                #region Delete Stock IN Or Out From archive
                //حذف التوريد والصرف من الارشيف
                //if (Comon.cInt(Result) > 0)
                //{
                //    int OutFactoryID = 0;
                //    DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(txtCommandID.Text), DocumentTypeFactoryBefore);
                //    if (dtInvoiceID.Rows.Count > 0)
                //    {
                //        OutFactoryID = DeleteInOnOROutOnBil(Comon.cInt(dtInvoiceID.Rows[0][0]), DocumentTypeAdditionalBefore);
                //        if (OutFactoryID == 0)
                //            Messages.MsgError(Messages.TitleInfo, "خطا في حذف الصرف من الارشيف للعلية تصنيع- قبل  ");
                //    }
                //    int InFactoryID = 0;
                //    DataTable dtInvoiceIDAfter = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(txtCommandID.Text), DocumentTypeFactoryAfter);
                //    if (dtInvoiceIDAfter.Rows.Count > 0)
                //    {
                //        InFactoryID = DeleteInOnOROutOnBil(Comon.cInt(dtInvoiceIDAfter.Rows[0][0]), DocumentTypeAdditionalAfter);
                //        if (InFactoryID == 0)
                //            Messages.MsgError(Messages.TitleInfo, "خطا في حذف التوريد من الارشيف للعملية تصنيع- بعد ");
                //    }

                  
                //}
                #endregion
                SplashScreenManager.CloseForm(false);
                if (Comon.cInt(Result) > 0)
                {
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);
                    ClearFields();
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
        public void ClearFields()
        {
            try
            {

                txtCustomerID.ReadOnly = true;
                txtDelegateID.ReadOnly = true;
                txtOrderDate.ReadOnly = true;
                txtTypeOrder.ReadOnly = true;
                txtGuidanceID.ReadOnly = true;

                lblAccountName.Text = "";
                txtGuidanceID.Text = "";
                txtReferanceID.Text = "";
                lblCustomerName.Text = "";
                txtCustomerID.Text = "";
                txtEmpID.Text = "";
                txtEmployeeStokID.Text = "";
                txtTotalBefor.Text = "";
                txtTotalAfter.Text = "";
                txtTypeOrder.Text = "";
                txtNotes.Text = "";
                txtOrderID.Text = "";
                lblTotallostFactory.Text = "";
                lblEmpName.Text = "";
                txtEmployeeStokName.Text = "";
                lblTypeOrderName.Text = "";
                lblGuidanceName.Text = "";
                //الحسابات
                txtAccountID.Text = "";
                txtStoreID.Text = "";
                txtEmployeeStokID.Text = "";
                txtEmpID.Text = "";
                txtDelegateID.Text = "";
                lblDelegateName.Text = "";
                lblStoreName.Text = "";
                lblEmpName.Text = "";
                lblEmpName.Text = "";
                lblTotallostFactory.Text = "0";
                txtTotalAfter.Text = "0";
                txtTotalBefor.Text = "0";
                cmbCurency.EditValue = MySession.GlobalDefaultSaleCurencyID;
                //جريد فيو
                initGridBeforAdditional();
                initGridAfterAdditional();
                initGridOrderDetails();
                pictureEdit1.Image = null;
                pictureEdit2.Image = null;

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // create OpenFileDialog object
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            // set filter to image files
            openFileDialog1.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.png, *.bmp) | *.jpg; *.jpeg; *.jpe;*.png;*.bmp";
            // open the dialog box
            DialogResult result = openFileDialog1.ShowDialog();
            // if a file is selected and OK button is clicked
            if (result == DialogResult.OK)
            {
                // set the image to pictureEdit
                if (pictureEdit1.Image == null)
                    pictureEdit1.Image = Image.FromFile(openFileDialog1.FileName);
                else
                    pictureEdit2.Image = Image.FromFile(openFileDialog1.FileName);
            }

        }


        GridHitInfo info = null;

        int pressedRowHandle = GridControl.InvalidRowHandle;
        int highlightedRowHandle = GridControl.InvalidRowHandle;


        protected ObjectState GetObjectState(int rowHandle)
        {
            if (rowHandle == pressedRowHandle)
                return ObjectState.Pressed;

            else
                return ObjectState.Normal;
        }
        private EditorButton button;
        protected EditorButton Button
        {
            get
            {
                if (button == null)
                    button = new EditorButton(ButtonPredefines.Ellipsis);
                return button;
            }
        }

        private void DrawButton(GraphicsCache cache, Rectangle bounds, ActiveLookAndFeelStyle lookAndFeel, AppearanceObject appearance, ObjectState state, string Caption)
        {
            EditorButtonObjectInfoArgs args = new EditorButtonObjectInfoArgs(cache, Button, appearance)
            {
                Bounds = bounds
            };
            BaseLookAndFeelPainters painters = LookAndFeelPainterHelper.GetPainter(lookAndFeel);
            args.State = state;
            painters.Button.DrawObject(args);
            args.Bounds = new Rectangle(args.Bounds.Left, args.Bounds.Top, args.Bounds.Width, args.Bounds.Height - 2);
            painters.Button.DrawCaption(args, Caption, appearance.Font, SystemBrushes.ControlText, args.Bounds, null);
        }





        private void gridControl1_MouseDown(object sender, MouseEventArgs e)
        {

        }
        private void GridViewBeforPrentag_RowStyle(object sender,
            DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            GridView View = sender as GridView;

        }
        private void GridViewBeforPrentag_MouseMove(object sender, MouseEventArgs e)
        {
            GridView view = sender as GridView;
            info = view.CalcHitInfo(e.X, e.Y);

        }

        private void label61_Click(object sender, EventArgs e)
        {

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



        private void txtEmpIDFactor_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtEmpID.Text) + " And Cancel =0  and BranchID=" + MySession.GlobalBranchID ;
                CSearch.ControlValidating(txtEmpID, lblEmpName, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }




        private void txtAccountIDFactory_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as AccountName FROM Acc_Accounts WHERE AccountID =" + Comon.cDbl(txtAccountID.Text) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID ;
                CSearch.ControlValidating(txtAccountID, lblAccountName, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void txtStoreIDFactory_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE AccountID =" + Comon.cLong(txtStoreID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(Comon.cInt(cmbBranchesID.EditValue));
                CSearch.ControlValidating(txtStoreID, lblStoreName, strSQL);
                strSQL = "SELECT "+PrimaryName+" as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID in( Select StoreManger from Stc_Stores WHERE AccountID =" + Comon.cLong(txtStoreID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue) + ") And Cancel =0 ";
                string StoreManger = Lip.GetValue(strSQL).ToString();
                lblBeforeStoreManger.Text = StoreManger;
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }



        private void txtEmployeeStokIDFactory_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtEmployeeStokID.Text) + " And Cancel =0   and BranchID=" + MySession.GlobalBranchID ;
                CSearch.ControlValidating(txtEmployeeStokID, txtEmployeeStokName, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }


        private void gridViewAdditional_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName != "ShownInNext")
            {
                e.Graphics.FillRectangle(e.Cache.GetSolidBrush(Color.Beige), e.Bounds);
                e.Graphics.DrawRectangle(e.Cache.GetPen(Color.Black, 1), e.Bounds);
                e.Appearance.DrawString(e.Cache, e.DisplayText, e.Bounds);
                e.Handled = true;
                ((GridView)sender).Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;
                ((GridView)sender).Appearance.Row.TextOptions.VAlignment = VertAlignment.Center;
            }
        }
      
   
        private void btnMachinResractionPolishnBefore_Click(object sender, EventArgs e)
        {
            if (IsNewRecord == true) return;
            int ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" +  txtCommandID.Text + " And DocumentType=" + DocumentTypeAdditionalBefore).ToString());
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

        private void btnMachinResractionPolishnAfter_Click(object sender, EventArgs e)
        {
            if (IsNewRecord == true) return;
             int ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + txtCommandID.Text + " And DocumentType=" + DocumentTypeAdditionalAfter).ToString());
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
 
 

        private void pictureEdit1_Click(object sender, EventArgs e)
        {
            frmViewImage frm = new frmViewImage();
            frm.picInvoiceImage.Image = pictureEdit1.Image;
            frm.Show();
        }

        private void btnFactory_Click(object sender, EventArgs e)
        {
            frmManufacturingCommand frm = new frmManufacturingCommand();
            if (ModelSystem.Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
               
                //frm.cmbPollutionTypeID.EditValue = 2;       
              
                frm.Show();
                //frm.GetValueORderID(this.txtOrderID.Text);
            }
            else
                frm.Dispose();

        }

        private void btnDims_Click(object sender, EventArgs e)
        {
            frmManufacturingDismantOrders frm = new frmManufacturingDismantOrders();
            if (ModelSystem.Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
                frm.txtOrderID.Text = txtOrderID.Text;
                frm.txtOrderID_Validating(null, null);
                frm.SetDataFromStageBefore(txtStoreID.Text, Comon.cInt(cmbTypeStage.EditValue), Comon.cInt(txtCommandID.Text), Comon.cInt(cmbCurency.EditValue));


            }
            else
                frm.Dispose();
        }

        public XtraReport Manu_PrentagdStage(GridView Grid)
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
                row["DateAfter"] = Grid.GetRowCellValue(i, "DebitDate");
                row["EmpName"] = Grid.GetRowCellValue(i, "DebitTime");
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
                ReportName = "rptManu_FactoryAdditionalOpretion";
                bool IncludeHeader = true;
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);
                /********************** Master *****************************/
                rptForm.RequestParameters = false;

                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;

                rptForm.Parameters["CommandID"].Value = txtCommandID.Text;
                rptForm.Parameters["CommandDate"].Value = txtCommandDate.Text;
                rptForm.Parameters["OrderID"].Value = txtOrderID.Text;
                rptForm.Parameters["OrderDate"].Value = txtOrderDate.Text;
                rptForm.Parameters["CustomerName"].Value = lblAccountName.Text;
                rptForm.Parameters["DelegetName"].Value = lblDelegateName.Text;
                rptForm.Parameters["GuidanceName"].Value = lblGuidanceName.Text;
                rptForm.Parameters["TypeOrder"].Value = lblTypeOrderName.Text;

                rptForm.Parameters["BranchesID"].Value = cmbBranchesID.Text;
                rptForm.Parameters["BeforeStoreName"].Value = lblStoreName.Text;
                rptForm.Parameters["BeforeStoreManger"].Value = lblBeforeStoreManger.Text;
                rptForm.Parameters["CostCenterName"].Value = "";

                rptForm.Parameters["FactorName"].Value = txtEmployeeStokName.Text;
                rptForm.Parameters["Curency"].Value = cmbCurency.Text;
                rptForm.Parameters["TypeStage"].Value = cmbTypeStage.Text;
                rptForm.Parameters["BeforeDate"].Value = "";
                rptForm.Parameters["Posted"].Value = cmbStatus.Text.ToString();
                rptForm.Parameters["Notes"].Value = txtNotes.Text;
                rptForm.Parameters["AfterStoreName"].Value = "";
                rptForm.Parameters["AfterStoreManger"].Value = "";

                //rptForm.Parameters["TotalQTY"].Value = txtWhTotalBefor.Text;
                //rptForm.Parameters["TotalLost"].Value = txtWhTotalAfter.Text;
                rptForm.Parameters["NumberCrews"].Value = "";
                //rptForm.Parameters["CupsLost"].Value = lblTotallost.Text; 
                rptForm.Parameters["EstimatedLoss"].Value = "";
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
                subreportBeforeCasting.ReportSource = Manu_PrentagdStage(GridViewBeforAddition);

                /******************** Report Factory ************************/
                XRSubreport subreportFactor = (XRSubreport)rptForm.FindControl("subRptrptManu_FactoryFactorCommendBefore", true);
                subreportFactor.Visible = IncludeHeader;
                subreportFactor.ReportSource = Manu_PrentagdStage(GridViewAfterAddition);


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

        private void cmbCurency_EditValueChanged(object sender, EventArgs e)
        {
            int isLocalCurrncy = Comon.cInt(Lip.GetValue("select TypeCurrency from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0  and BranchID=" + MySession.GlobalBranchID  ));
            if (isLocalCurrncy > 1)
            {
                decimal CurrncyPrice = Comon.cDec(Lip.GetValue("select ExchangeRate from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0 and BranchID=" + MySession.GlobalBranchID ));
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
        
        bool ChekOrderIsFoundInGrid(GridView Grid, string ColBarCode, string OrderID)
        {
            for (int i = 0; i <= Grid.DataRowCount - 1; i++)
            {
                if (Grid.GetRowCellValue(i, ColBarCode) != null && Grid.GetRowCellValue(i, ColBarCode).ToString().Trim() != "")
                if (Grid.GetRowCellValue(i, ColBarCode).ToString() == OrderID)
                    return true;
            }
            if (rowIndex < 0)
            {
                if (Grid.GetRowCellValue(rowIndex, ColBarCode) != null && Grid.GetRowCellValue(rowIndex, ColBarCode).ToString().Trim() != "")
                {
                    string BarCode = Grid.GetRowCellValue(rowIndex, ColBarCode).ToString();
                    if (((string)BarCode) == OrderID)
                        return true;
                }
            }
            return false;
        }
        private void GridViewOrderDetails_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridView view = sender as GridView;

                if (view.GetRowCellValue(view.FocusedRowHandle, "BarCode").ToString().Trim() != "")
                {
                    string BarCode = view.GetRowCellValue(view.FocusedRowHandle, "BarCode").ToString().Trim();
                    DataTable dt;
                    dt = Stc_itemsDAL.GetItemData(BarCode, UserInfo.FacilityID);
                    if (dt.Rows.Count > 0)
                    {
                        GridViewBeforAddition.AddNewRow();
                        if (ChekOrderIsFoundInGrid(GridViewBeforAddition,"BarcodeAdditional", BarCode))
                        {
                            Messages.MsgAsterisk(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "الصنف موجود  لذلك لا يمكن انزاله  اكثر من مرة " : "This Item is Found Table");
                            GridViewBeforAddition.DeleteRow(rowIndex);
                            return;
                        }                      
                        string QTY = view.GetRowCellValue(view.FocusedRowHandle, "QTY").ToString();
                        FillItemData(GridViewBeforAddition, gridControlBeforeAdditional, "BarcodeAdditional", "Debit", Stc_itemsDAL.GetItemData1(BarCode, UserInfo.FacilityID), "DebitDate", "DebitTime", ((TextEdit)txtAccountID), QTY);

                        SendKeys.Send("\t");

                    }

                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void GridViewBeforAddition_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            rowIndex = e.RowHandle;
        }

        private void GridViewBeforAddition_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridView view = sender as GridView;

                if (view.GetRowCellValue(view.FocusedRowHandle, "BarcodeAdditional").ToString().Trim() != "")
                {
                    string BarCode = view.GetRowCellValue(view.FocusedRowHandle, "BarcodeAdditional").ToString().Trim();
                    DataTable dt;
                    dt = Stc_itemsDAL.GetItemData(BarCode, UserInfo.FacilityID);
                    if (dt.Rows.Count > 0)
                    {
                        GridViewAfterAddition.AddNewRow();
                        if (ChekOrderIsFoundInGrid(GridViewAfterAddition, "BarcodeAdditional", BarCode))
                        {
                            Messages.MsgAsterisk(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "الصنف موجود  لذلك لا يمكن انزاله  اكثر من مرة " : "This Item is Found Table");
                            GridViewAfterAddition.DeleteRow(rowIndex);
                            return;
                        }
                      
                        string QTY = view.GetRowCellValue(view.FocusedRowHandle, "Debit").ToString();
                        FillItemData(GridViewAfterAddition, gridControlAfterAdditional, "BarcodeAdditional", "Credit", Stc_itemsDAL.GetItemData1(BarCode, UserInfo.FacilityID), "DebitDate", "DebitTime", txtAccountID, QTY);
                        SendKeys.Send("\t");
                    }

                }
            }
            catch(Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void GridViewAfterAddition_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            rowIndex = e.RowHandle;
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
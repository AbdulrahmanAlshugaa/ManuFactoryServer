using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Edex.GeneralObjects.GeneralForms;
using Edex.ModelSystem;
using DevExpress.XtraSplashScreen;
using Edex.Model;
using Edex.DAL.ManufacturingDAL;
using DevExpress.XtraGrid.Columns;
using Edex.GeneralObjects.GeneralClasses;
using Edex.Model.Language;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;
using System.Drawing.Imaging;
using DevExpress.XtraEditors.Repository;
using Edex.DAL.Stc_itemDAL;
using System.Text.RegularExpressions;
using DevExpress.XtraEditors.Drawing;
using DevExpress.LookAndFeel;
using DevExpress.Utils.Drawing;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Edex.DAL;
using Edex.StockObjects.StoresClasses;
using DevExpress.XtraReports.UI;
using Edex.DAL.SalseSystem.Stc_itemDAL;
using Edex.AccountsObjects.Transactions;
using Edex.DAL.Accounting;
using Permissions = Edex.ModelSystem.Permissions;
using Edex.HR.Codes;
using Edex.StockObjects.Codes;
using System.Globalization;
using Edex.StockObjects.Transactions;

namespace Edex.Manufacturing.Codes
{
    public partial class frmManufacturingTalmee : BaseForm
    {
        //list detail

        BindingList<Menu_FactoryRunCommandTalmee> lstDetailTalmee = new BindingList<Menu_FactoryRunCommandTalmee>();
        BindingList<Menu_FactoryRunCommandTalmee> lstDetailAfterTalmee = new BindingList<Menu_FactoryRunCommandTalmee>();

        BindingList<Menu_FactoryOrderDetails> lstOrderDetails = new BindingList<Menu_FactoryOrderDetails>();
        BindingList<Menu_FactoryRunCommandfactory> lstDetailfactory = new BindingList<Menu_FactoryRunCommandfactory>();
        BindingList<Menu_FactoryRunCommandfactory> lstDetailAfterfactory = new BindingList<Menu_FactoryRunCommandfactory>();
        BindingList<Manu_ProductionExpensesDetails> lstDetailProductionExpenses = new BindingList<Manu_ProductionExpensesDetails>();
        BindingList<Manu_AuxiliaryMaterialsDetails> lstDetailAlcadZircon = new BindingList<Manu_AuxiliaryMaterialsDetails>();
        BindingList<Stc_ItemUnits> lstDetailUnit = new BindingList<Stc_ItemUnits>();
        #region Declare




        int rowIndex = 0;
        public CultureInfo culture = new CultureInfo("en-US");
        public int DocumentTypePloshinBeforeFrist = 40;
        public int DocumentTypePloshinAfterFrist = 41;
        public int DocumentTypePloshinBeforeScand = 42;
        public int DocumentTypePloshinAfterScand = 43;
        public int DocumentTypePloshinBeforeTheerd = 50;
        public int DocumentTypePloshinAfterTheerd = 51;
        private Menu_FactoryRunCommandMasterDAL cClass = new Menu_FactoryRunCommandMasterDAL();
        private string GroupName;
        DataTable DataRecord;
        DataTable DataRecordCommpund = new DataTable();
        DataTable DataRecordAfterCommpund = new DataTable();
        DataTable DataRecordPolushin;
        DataTable DataRecordAfterBrntag;
        DataTable DataRecordSelver;
        DataTable DataRecordTalmee;
        DataTable DataRecordCostDaimond;
        DataTable DataRecordAfterTalmee;
        DataTable DataRecordFactory;
        DataTable DataRecordAfterFactory;
        DataTable DataRecordProductionExpenses;
        int indexPrntagerow ;
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
        public frmManufacturingTalmee()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                InitializeComponent();


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

                this.gridControlBeforePolishing.ProcessGridKey += gridControl2_ProcessGridKey;
                this.gridControlAfterPolishing.ProcessGridKey += gridControl2_ProcessGridKey;
                this.GridViewBeforPolish.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.GridViewBeforfactory_ValidatingEditor);
                this.GridViewAfterPolish.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.GridViewAfterfactory_ValidatingEditor);
                this.GridViewBeforPolish.RowUpdated += GridViewBeforfactory_RowUpdated;
                this.GridViewAfterPolish.RowUpdated += GridViewBeforfactory_RowUpdated;

                ItemName = "ArbItemName";
                SizeName = "ArbSizeName";
                PrimaryName = "ArbName";
                GroupName = "ArbGroupName";
                CaptionItemName = "اسم الصنف";
                if (UserInfo.Language == iLanguage.English)
                {
                    ItemName = "EngItemName";
                    SizeName = "EngSizeName";
                    PrimaryName = "EngName";
                    CaptionItemName = "Item Name";
                    GroupName = "EngGroupName";
                }
                FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
                FillCombo.FillComboBox(cmbCurency, "Acc_Currency", "ID", PrimaryName, "", "BranchID="+MySession.GlobalBranchID, (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));

                FillCombo.FillComboBox(cmbTypeStage, "Manu_TypeStages", "ID", PrimaryName, "", "", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                cmbTypeStage.EditValue = 8;
                cmbTypeStage.ReadOnly = true;

                cmbCurency.EditValue = MySession.GlobalDefaultSaleCurencyID;
                cmbBranchesID.EditValue = MySession.GlobalBranchID;
                FillCombo.FillComboBox(cmbPollutionTypeID, "Manu_TypePollution", "ID", PrimaryName, "", "", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                FillCombo.FillComboBox(cmbStatus, "Manu_TypeStatus", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                cmbStatus.EditValue = MySession.GlobalDefaultProcessPostedStatus;

                this.GridViewAfterPolish.CellValueChanging += GridViewAfterPolish_CellValueChanging;
                EnableControlDefult();

                initGridBeforTalmee();
                initGridAfterTalmee();
                initGridOrderDetails();
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
        void EnableControlDefult()
        {
           
            cmbCurency.ReadOnly = !MySession.GlobalAllowChangefrmPolishnCurrncyID;
            txtCommandDate.ReadOnly = !MySession.GlobalAllowChangefrmPolishnCommandDate;
            if (Comon.cInt(cmbPollutionTypeID.EditValue) == 1)
            {
                txtStoreID.ReadOnly = !MySession.GlobalAllowChangefrmPolishnStoreID;
                txtAccountID.ReadOnly = !MySession.GlobalAllowChangefrmPolishnAccountID;
            }
            else if(Comon.cInt(cmbPollutionTypeID.EditValue)==2)
            {
                txtStoreID.ReadOnly = !MySession.GlobalAllowChangefrmPolishn2StoreID;
                txtAccountID.ReadOnly = !MySession.GlobalAllowChangefrmPolishn2AccountID;
            }
            else if(Comon.cInt(cmbPollutionTypeID.EditValue)==3)
            {
                txtStoreID.ReadOnly = !MySession.GlobalAllowChangefrmPolishn3StoreID;
                txtAccountID.ReadOnly = !MySession.GlobalAllowChangefrmPolishn3AccountID;
            }
            txtEmpID.ReadOnly = !MySession.GlobalAllowChangefrmPolishnEmployeeID;

        }
        void SetDefultValue()
        {

            cmbCurency.EditValue = Comon.cInt(MySession.GlobalDefaultPolishinCurrncyID);
            cmbCurency_EditValueChanged(null, null);
            txtStoreID.Text = MySession.GlobalDefaultPolishinStoreID;
         
            txtAccountID.Text = MySession.GlobalDefaultPolishinAccountID;
            if (Comon.cInt(cmbPollutionTypeID.EditValue) == 2)
            {
                txtStoreID.Text = MySession.GlobalDefaultPolishin2StoreID;
                txtAccountID.Text = MySession.GlobalDefaultPolishin2AccountID;
            }
            else if (Comon.cInt(cmbPollutionTypeID.EditValue) == 3)
            {
                txtStoreID.Text = MySession.GlobalDefaultPolishin3StoreID;
                txtAccountID.Text = MySession.GlobalDefaultPolishin3AccountID;
            }
            txtStoreIDFactory_Validating(null, null);
            txtAccountIDFactory_Validating(null, null);
            txtEmpID.Text = MySession.GlobalDefaultPolishnEmployeeID;
            txtEmpIDFactor_Validating(null, null);
        }
        void GridViewAfterPolish_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
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

                        int isShow = Comon.cInt(Lip.GetValue("SELECT [ShowInOrderDetils] FROM [Stc_Items] WHERE [ItemID] = " + view.GetRowCellValue(e.RowHandle, "ItemID") + " AND Cancel = 0 and BranchID=" + MySession.GlobalBranchID));

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
            lstDetailAlcadZircon.AllowNew = true;
            lstDetailAlcadZircon.AllowEdit = true;
            lstDetailAlcadZircon.AllowRemove = true;

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
            if (this.GridViewAfterPolish.ActiveEditor is CheckEdit)
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

            if (this.GridViewAfterPolish.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                HasColumnErrors = false;
                string ColName = view.FocusedColumn.FieldName;
                int ItemID = 0;
                if (ColName == "EmpID"||ColName=="StoreID" || ColName == "SizeID" || ColName == "ItemID" || ColName == "MachinID" || ColName == "Credit" || ColName == "Debit")
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
                        view.SetColumnError(GridViewAfterPolish.Columns[ColName], "");
                    }

                    if (ColName == "MachinID")
                    {


                        DataTable dtGroupID = Lip.SelectRecord("Select " + PrimaryName + " from Menu_FactoryMachine Where MachineID=" + Comon.cInt(e.Value)+" and BranchID=" + MySession.GlobalBranchID);
                        if (dtGroupID.Rows.Count > 0)
                        {
                            FileDataMachinName(GridViewAfterPolish, "DebitDate", "DebitTime", Comon.cInt(e.Value));

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
                            FillItemData(GridViewAfterPolish, gridControlAfterPolishing, "BarcodeTalmee", "Credit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", txtAccountID);
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
                            GridViewAfterPolish.SetRowCellValue(GridViewAfterPolish.FocusedRowHandle, GridViewAfterPolish.Columns[SizeName], dtItemID.Rows[0][PrimaryName].ToString());
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

                            GridViewAfterPolish.SetRowCellValue(GridViewAfterPolish.FocusedRowHandle, GridViewAfterPolish.Columns["EmpName"], dtNameEmp.Rows[0][PrimaryName].ToString());
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
                        DataTable dtItemID = Lip.SelectRecord("Select  " + PrimaryName + " from  Stc_Stores Where Cancel=0 and BranchID=" + MySession.GlobalBranchID+" And StoreID=" + val.ToString() + " And FacilityID=" + UserInfo.FacilityID);
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridViewAfterPolish.SetFocusedRowCellValue("StoreName", dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(GridViewAfterPolish.Columns[ColName], "");

                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(GridViewAfterPolish.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }



                }
                if (ColName == ItemName)
                {

                    DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_Items_Find  Where  LOWER (" + ItemName + ")=LOWER ('" + val.ToString() + "') and BranchID=" + MySession.GlobalBranchID);
                    if (dtItemID.Rows.Count > 0)
                    {
                        FillItemData(GridViewAfterPolish, gridControlAfterPolishing, "BarcodeTalmee", "Credit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", ((TextEdit)txtAccountID));
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
                        GridViewAfterPolish.SetFocusedRowCellValue("MachinID", dtMachinID.Rows[0]["MachineID"].ToString());

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
                        GridViewAfterPolish.SetRowCellValue(GridViewAfterPolish.FocusedRowHandle, "DebitDate", serverDate);
                        return;
                    }
                }
                if (ColName == SizeName)
                {
                    string Str = "Select Stc_ItemUnits.SizeID from Stc_ItemUnits inner join Stc_Items on Stc_Items.ItemID=Stc_ItemUnits.ItemID  and Stc_Items.BranchID=Stc_ItemUnits.BranchID left outer join  Stc_SizingUnits on Stc_ItemUnits.SizeID=Stc_SizingUnits.SizeID and Stc_ItemUnits.BranchID=Stc_SizingUnits.BranchID Where UnitCancel=0 and BranchID=" + MySession.GlobalBranchID+"  And LOWER (Stc_SizingUnits." + PrimaryName + ")=LOWER ('" + val.ToString() + "') and  Stc_Items.ItemID=" + Comon.cLong(GridViewAfterPolish.GetRowCellValue(GridViewAfterPolish.FocusedRowHandle, "ItemID").ToString()) + "  And Stc_ItemUnits.FacilityID=" + UserInfo.FacilityID;
                    DataTable dtSizeID = Lip.SelectRecord(Str);
                    if (dtSizeID.Rows.Count > 0)
                    {
                        FillItemData(GridViewAfterPolish, gridControlAfterPolishing, "BarcodeTalmee", "Credit", Stc_itemsDAL.GetItemDataByItemID_SizeID(Comon.cInt(GridViewAfterPolish.GetRowCellValue(GridViewAfterPolish.FocusedRowHandle, "ItemID")), Comon.cInt(dtSizeID.Rows[0][0].ToString()), UserInfo.FacilityID), "DebitDate", "DebitTime", ((TextEdit)txtAccountID));
                        GridViewAfterPolish.SetRowCellValue(GridViewAfterPolish.FocusedRowHandle, ColName, e.Value.ToString());
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundSizeForItem;
                        view.SetColumnError(GridViewBeforPolish.Columns[ColName], Messages.msgNoFoundSizeForItem);
                    }

                }
                if (ColName == "EmpName")
                {

                    DataTable dtMachinID = Lip.SelectRecord("Select  EmployeeID  from HR_EmployeeFile Where  LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') and BranchID=" + MySession.GlobalBranchID);
                    if (dtMachinID.Rows.Count > 0)
                    {
                        GridViewAfterPolish.SetFocusedRowCellValue("EmpID", dtMachinID.Rows[0]["EmployeeID"].ToString());

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
                    DataTable dtItemID = Lip.SelectRecord("Select StoreID from Stc_Stores Where Cancel=0 and BranchID=" + MySession.GlobalBranchID+" And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                    if (dtItemID.Rows.Count > 0)
                    {
                        GridViewAfterPolish.SetFocusedRowCellValue("StoreID", dtItemID.Rows[0]["StoreID"]);
                        e.Valid = true;
                        view.SetColumnError(GridViewAfterPolish.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(GridViewAfterPolish.Columns[ColName], Messages.msgNoFoundThisItem);
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
                    if ((((GridView)Grid).Name == GridViewBeforPolish.Name))
                    {
                        totalQtyBalance = Lip.RemindQtyItemByMinUnit(Comon.cLong(dt.Rows[0]["ItemID"].ToString()), Comon.cInt(dt.Rows[0]["SizeID"]), Comon.cDbl(txtStoreID.Text));
                        {
                            decimal qtyCurrent = 0;
                            decimal QtyInCommand = Lip.GetQTYinCommandToThisItem("Menu_FactoryRunCommandTalmee", "Menu_FactoryRunCommandMaster", QTYFildName, "ComandID", Comon.cInt(txtCommandID.Text), dt.Rows[0]["ItemID"].ToString(), " and Menu_FactoryRunCommandTalmee.TypeOpration=1 and Menu_FactoryRunCommandTalmee.PollutionTypeID=" + Comon.cInt(cmbPollutionTypeID.EditValue), BarCode,Comon.cInt(dt.Rows[0]["SizeID"].ToString()));
                            qtyCurrent = frmCadFactory.GetQTYToItemFromGridView(Grid, QTYFildName, 0, dt.Rows[0]["ItemID"].ToString(), Comon.cInt(dt.Rows[0]["SizeID"].ToString()), BarCode); 

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
                //Grid.AddNewRow();
                if (Comon.cInt(cmbPollutionTypeID.EditValue) <= 2)
                {
                    Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns["MachinID"], Comon.cInt(cmbPollutionTypeID.EditValue));
                    strSQL = "SELECT " + PrimaryName + " FROM Menu_FactoryMachine WHERE MachineID =" + Comon.cInt(cmbPollutionTypeID.EditValue);
                    Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns["MachineName"], Lip.GetValue(strSQL));
                }
                else
                {
                    Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns["MachinID"], Comon.cInt(4));
                    strSQL = "SELECT " + PrimaryName + " FROM Menu_FactoryMachine WHERE MachineID =" + Comon.cInt(4);
                    Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns["MachineName"], Lip.GetValue(strSQL));
                }

                {
                    Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[Time], DateTime.Now.ToString("hh:mm:tt"));
                    Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[Date], DateTime.Now.ToString("yyyy/MM/dd"));

                }
           
                if ( (((GridView)Grid).Name ==GridViewBeforPolish.Name))
                    Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[QTYFildName], totalQtyBalance);
                else
                    Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[QTYFildName], 0);

                //RepositoryItemLookUpEdit rSize = Common.LookUpEditSize(Comon.cInt(dt.Rows[0]["ItemID"].ToString()));
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

            if (this.GridViewBeforPolish.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                HasColumnErrors = false;
                string ColName = view.FocusedColumn.FieldName;
                int ItemID = 0;
                if (ColName == "EmpID"||ColName=="StoreID" || ColName == "SizeID" || ColName == "ItemID" || ColName == "MachinID" || ColName == "Credit" || ColName == "Debit")
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
                        view.SetColumnError(GridViewBeforPolish.Columns[ColName], "");
                    }
                    if (ColName == "MachinID")
                    {
                        DataTable dtGroupID = Lip.SelectRecord("Select " + PrimaryName + " from Menu_FactoryMachine Where MachineID=" + Comon.cInt(e.Value));
                        if (dtGroupID.Rows.Count > 0)
                        {                        
                            e.Valid = true;
                            view.SetColumnError(GridViewBeforPolish.Columns[ColName], "");
                            GridViewBeforPolish.SetRowCellValue(GridViewBeforPolish.FocusedRowHandle, GridViewBeforPolish.Columns["ID"], GridViewBeforPolish.RowCount);
                            
                            FileDataMachinName(GridViewBeforPolish,"DebitDate", "DebitTime", Comon.cInt(e.Value));     
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
                            FillItemData(GridViewBeforPolish, gridControlBeforePolishing, "BarcodeTalmee", "Debit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", ((TextEdit)txtAccountID));
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
                        decimal totalQtyBalance = Lip.RemindQtyItemByMinUnit(Comon.cLong(GridViewBeforPolish.GetRowCellValue(GridViewBeforPolish.FocusedRowHandle, "ItemID").ToString()), Comon.cInt(GridViewBeforPolish.GetRowCellValue(GridViewBeforPolish.FocusedRowHandle, "SizeID")), Comon.cDbl(txtStoreID.Text));
                        decimal QtyInCommand = Lip.GetQTYinCommandToThisItem("Menu_FactoryRunCommandTalmee", "Menu_FactoryRunCommandMaster", "Debit", "ComandID", Comon.cInt(txtCommandID.Text), GridViewBeforPolish.GetRowCellValue(GridViewBeforPolish.FocusedRowHandle, "ItemID").ToString(), " and Menu_FactoryRunCommandTalmee.TypeOpration=1 and Menu_FactoryRunCommandTalmee.PollutionTypeID=" + Comon.cInt(cmbPollutionTypeID.EditValue),"BarcodeTalmee",SizeID:Comon.cInt(GridViewBeforPolish.GetRowCellValue(GridViewBeforPolish.FocusedRowHandle, "SizeID").ToString()));
                        totalQtyBalance += QtyInCommand;
                        decimal qtyCurrent = 0;
                         qtyCurrent = frmCadFactory.GetQTYToItemFromGridView(GridViewBeforPolish, "Debit", Comon.cDec(val.ToString()), GridViewBeforPolish.GetRowCellValue(GridViewBeforPolish.FocusedRowHandle, "ItemID").ToString(), Comon.cInt(GridViewBeforPolish.GetRowCellValue(GridViewBeforPolish.FocusedRowHandle, "SizeID")), "BarcodeTalmee"); 

                        if (qtyCurrent > totalQtyBalance)
                        {
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheQTyinOrderisExceed);
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgQtyisNotAvilable + (totalQtyBalance - (qtyCurrent - Comon.cDec(val.ToString())));
                            view.SetColumnError(GridViewBeforPolish.Columns[ColName], Messages.msgQtyisNotAvilable + totalQtyBalance.ToString());
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
                                    view.SetColumnError(GridViewBeforPolish.Columns[ColName], Messages.msgQtyisNotAvilable + totalQtyBalance.ToString());
                                }
                            }
                            else
                            {
                                e.Valid = false;
                                HasColumnErrors = true;
                                e.ErrorText = Messages.msgNotFoundAnyQtyInStore;
                                view.SetColumnError(GridViewBeforPolish.Columns[ColName], Messages.msgNotFoundAnyQtyInStore);
                            }
                        }
                    }
                    if (ColName == "SizeID")
                    {

                        DataTable dtItemID = Lip.SelectRecord("Select  " + PrimaryName + " from Stc_SizingUnits  Where SizeID=" + e.Value + " and BranchID=" + MySession.GlobalBranchID);
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridViewBeforPolish.SetRowCellValue(GridViewBeforPolish.FocusedRowHandle, GridViewBeforPolish.Columns[SizeName], dtItemID.Rows[0][PrimaryName].ToString());
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

                            GridViewBeforPolish.SetRowCellValue(GridViewBeforPolish.FocusedRowHandle, GridViewBeforPolish.Columns["EmpName"], dtNameEmp.Rows[0][PrimaryName].ToString());
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
                        DataTable dtItemID = Lip.SelectRecord("Select  " + PrimaryName + " from  Stc_Stores Where Cancel=0 and BranchID=" + MySession.GlobalBranchID+" And StoreID=" + val.ToString() + " And FacilityID=" + UserInfo.FacilityID);
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridViewBeforPolish.SetFocusedRowCellValue("StoreName", dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(GridViewBeforPolish.Columns[ColName], "");
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(GridViewBeforPolish.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }

                }

                else if (ColName == "DebitDate")
                {
                    string formattedDate = ((DateTime)e.Value).ToString("yyyy/MM/dd");
                    if (Lip.CheckDateISAvilable(formattedDate))
                    {
                        string serverDate = Lip.GetServerDate(); e.Value = serverDate;
                        GridViewBeforPolish.SetRowCellValue(GridViewBeforPolish.FocusedRowHandle, "DebitDate", serverDate);
                        return;
                    }
                }
                if (ColName == ItemName)
                {
                    DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_Items_Find  Where  LOWER (" + ItemName + ")=LOWER ('" + val.ToString() + "') and BranchID=" + MySession.GlobalBranchID);
                    if (dtItemID.Rows.Count > 0)
                    {    
                       FillItemData(GridViewBeforPolish,gridControlBeforePolishing, "BarcodeTalmee", "Debit",Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", ((TextEdit)txtAccountID));
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
                       
                        FileDataMachinName(GridViewBeforPolish, "DebitDate", "DebitTime", Comon.cInt(dtMachinID.Rows[0]["MachineID"].ToString()));
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
                    string Str = "Select Stc_ItemUnits.SizeID from Stc_ItemUnits inner join Stc_Items on Stc_Items.ItemID=Stc_ItemUnits.ItemID and Stc_Items.BranchID=Stc_ItemUnits.BranchID left outer join  Stc_SizingUnits on Stc_ItemUnits.SizeID=Stc_SizingUnits.SizeID and Stc_ItemUnits.BranchID=Stc_SizingUnits.BranchID Where UnitCancel=0 and BranchID=" + MySession.GlobalBranchID+" And LOWER (Stc_SizingUnits." + PrimaryName + ")=LOWER ('" + val.ToString() + "') and  Stc_Items.ItemID=" + Comon.cLong(GridViewBeforPolish.GetRowCellValue(GridViewBeforPolish.FocusedRowHandle, "ItemID").ToString()) + "  And Stc_ItemUnits.FacilityID=" + UserInfo.FacilityID;
                    DataTable dtBarCode = Lip.SelectRecord(Str);
                    if (dtBarCode.Rows.Count > 0)
                    {
                        GridViewBeforPolish.SetFocusedRowCellValue("SizeID", dtBarCode.Rows[0]["SizeID"]);
                        frmCadFactory.SetValuseWhenChangeSizeName(GridViewBeforPolish, Comon.cLong(GridViewBeforPolish.GetRowCellValue(GridViewBeforPolish.FocusedRowHandle, "ItemID").ToString()), Comon.cInt(dtBarCode.Rows[0]["SizeID"]), "Menu_FactoryRunCommandTalmee", "Menu_FactoryRunCommandMaster", Comon.cDbl(txtStoreID.Text), Comon.cInt(txtCommandID.Text), "ComandID", Where: " and Menu_FactoryRunCommandTalmee.TypeOpration=1 and Menu_FactoryRunCommandTalmee.PollutionTypeID=" + Comon.cInt(cmbPollutionTypeID.EditValue), FildNameQTY: "Debit", FildNameBarCode: "BarcodeTalmee");
                        e.Valid = true;
                        view.SetColumnError(GridViewBeforPolish.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundSizeForItem;
                        view.SetColumnError(GridViewBeforPolish.Columns[ColName], Messages.msgNoFoundSizeForItem);
                    }
                }
                if (ColName == "EmpName")
                {

                    DataTable dtMachinID = Lip.SelectRecord("Select  EmployeeID  from HR_EmployeeFile Where  LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') and BranchID=" + MySession.GlobalBranchID);
                    if (dtMachinID.Rows.Count > 0)
                    {
                        GridViewBeforPolish.SetFocusedRowCellValue("EmpID", dtMachinID.Rows[0]["EmployeeID"].ToString());

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
                    DataTable dtItemID = Lip.SelectRecord("Select StoreID from Stc_Stores Where Cancel=0 and BranchID=" + MySession.GlobalBranchID+" And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                    if (dtItemID.Rows.Count > 0)
                    {
                        GridViewBeforPolish.SetFocusedRowCellValue("StoreID", dtItemID.Rows[0]["StoreID"]);
                        e.Valid = true;
                        view.SetColumnError(GridViewBeforPolish.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(GridViewBeforPolish.Columns[ColName], Messages.msgNoFoundThisItem);
                    }
                }
            }
        }
     
        
        
        
        private void gridControl1_ProcessGridKey(object sender, KeyEventArgs e)
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
                DataRecord = Menu_FactoryRunCommandMasterDAL.frmGetDataDetalByIDPollutionTypeID(ComandID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID, Comon.cInt(cmbTypeStage.EditValue),Comon.cInt(cmbPollutionTypeID.EditValue));

                if (DataRecord != null && DataRecord.Rows.Count > 0)
                {

                    DataRecordPolushin = Menu_FactoryRunCommandTalmeeDAL.frmGetDataDetalByID(Comon.cLong(ComandID), UserInfo.BRANCHID, UserInfo.FacilityID, 1, Comon.cInt(cmbPollutionTypeID.EditValue));
                    DataRecordAfterBrntag = Menu_FactoryRunCommandTalmeeDAL.frmGetDataDetalByID(Comon.cLong(ComandID), UserInfo.BRANCHID, UserInfo.FacilityID, 2, Comon.cInt(cmbPollutionTypeID.EditValue));


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
                            gridControlBeforePolishing.DataSource = DataRecordPolushin;
                            lstDetailfactory.AllowNew = true;
                            lstDetailfactory.AllowEdit = true;
                            lstDetailfactory.AllowRemove = true;
                            GridViewBeforPolish.RefreshData();
                        }
                    if (DataRecordAfterBrntag != null)
                        if (DataRecordAfterBrntag.Rows.Count > 0)
                        {
                            gridControlAfterPolishing.DataSource = DataRecordAfterBrntag;
                            lstDetailAfterfactory.AllowNew = true;
                            lstDetailAfterfactory.AllowEdit = true;
                            lstDetailAfterfactory.AllowRemove = true;
                            GridViewAfterPolish.RefreshData();
                        }
                    int CommandIDTemp = 0;
                    CommandIDTemp = Comon.cInt(Lip.GetValue("select ComandID from Menu_FactoryRunCommandMaster where Cancel=0 and BranchID=" + MySession.GlobalBranchID+" and TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + " and ComandID<>" + Comon.cInt(txtCommandID.Text) + " and Barcode='" + txtOrderID.Text + "'"));

                    if (CommandIDTemp > 0)
                        groupBox1.Visible = true;
                    else
                        groupBox1.Visible = false;
                  
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
                GridViewOrderDetails.Columns["EngStateName"].Visible = false;
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
                GridViewOrderDetails.Columns["StateName"].Visible = false;
                GridViewOrderDetails.Columns["StoreID"].Caption = "Store ID";
                GridViewOrderDetails.Columns["StoreName"].Caption = "Store Name";

            }
            GridViewOrderDetails.OptionsBehavior.ReadOnly = true;
            GridViewOrderDetails.OptionsBehavior.Editable = false;
        }
        void initGridBeforTalmee()
        {

            lstDetailTalmee = new BindingList<Menu_FactoryRunCommandTalmee>();
            lstDetailTalmee.AllowNew = true;
            lstDetailTalmee.AllowEdit = true;
            lstDetailTalmee.AllowRemove = true;
            gridControlBeforePolishing.DataSource = lstDetailTalmee;

            DataTable dtitems0 = Lip.SelectRecord("SELECT   SizeID," + PrimaryName + "   FROM Stc_SizingUnits where BranchID=" + MySession.GlobalBranchID);
            string[] NameUnit = new string[dtitems0.Rows.Count];
            for (int i = 0; i <= dtitems0.Rows.Count - 1; i++)
                NameUnit[i] = dtitems0.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems0 = new RepositoryItemComboBox();
            riComboBoxitems0.Items.AddRange(NameUnit);
            gridControlBeforePolishing.RepositoryItems.Add(riComboBoxitems0);
            GridViewBeforPolish.Columns[SizeName].ColumnEdit = riComboBoxitems0;

            DataTable dtitems = Lip.SelectRecord("SELECT   " + PrimaryName + "   FROM Menu_FactoryMachine ");
            string[] NameMachine = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameMachine[i] = dtitems.Rows[i][PrimaryName].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameMachine);

            gridControlBeforePolishing.RepositoryItems.Add(riComboBoxitems);


            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0 and BranchID=" + MySession.GlobalBranchID);
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();

            riComboBoxitems2.Items.AddRange(StoreName);
            gridControlBeforePolishing.RepositoryItems.Add(riComboBoxitems2);
            GridViewBeforPolish.Columns["StoreName"].ColumnEdit = riComboBoxitems2;


            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0 and BranchID=" + MySession.GlobalBranchID);
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlBeforePolishing.RepositoryItems.Add(riComboBoxitems3);
            GridViewBeforPolish.Columns["EmpName"].ColumnEdit = riComboBoxitems3;

            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0 and BranchID=" + MySession.GlobalBranchID);
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlBeforePolishing.RepositoryItems.Add(riComboBoxitems4);
            GridViewBeforPolish.Columns[ItemName].ColumnEdit = riComboBoxitems4;

            GridViewBeforPolish.Columns["MachineName"].ColumnEdit = riComboBoxitems;
            GridViewBeforPolish.Columns["ID"].Visible = false;
            GridViewBeforPolish.Columns["ComandID"].Visible = false;
            GridViewBeforPolish.Columns["BarcodeTalmee"].Visible = false;
            GridViewBeforPolish.Columns["EmpPolishnID"].Visible = false;
            GridViewBeforPolish.Columns["EmpPrentagID"].Visible = false;
            GridViewBeforPolish.Columns["Cancel"].Visible = false;
            GridViewBeforPolish.Columns["BranchID"].Visible = false;
            GridViewBeforPolish.Columns["FacilityID"].Visible = false;
            GridViewBeforPolish.Columns["MachinID"].Visible = false;
            GridViewBeforPolish.Columns["MachineName"].Visible = false;

            GridViewBeforPolish.Columns["EditUserID"].Visible = false;
            GridViewBeforPolish.Columns["EditDate"].Visible = false;
            GridViewBeforPolish.Columns["EditTime"].Visible = false;
            GridViewBeforPolish.Columns["RegDate"].Visible = false;
            GridViewBeforPolish.Columns["UserID"].Visible = false;

            GridViewBeforPolish.Columns["ComputerInfo"].Visible = false;
            GridViewBeforPolish.Columns["EditComputerInfo"].Visible = false;
            GridViewBeforPolish.Columns["RegTime"].Visible = false;

            GridViewBeforPolish.Columns["Credit"].Visible = false;
            GridViewBeforPolish.Columns["TypeOpration"].Visible = false;
            //GridViewBeforPolish.Columns["SizeID"].Visible = false;
            GridViewBeforPolish.Columns["CostPrice"].Visible = false;
            GridViewBeforPolish.Columns["SizeID"].Visible = false;
            // GridViewBeforPolish.Columns["DebitTime"].Visible = false;
            GridViewBeforPolish.Columns["MachinID"].Name = "MachinID";
            //dtItem.Columns["Losed"].ReadOnly = true;
            GridViewBeforPolish.Columns["EmpName"].Width = 120;
            GridViewBeforPolish.Columns["StoreName"].Width = 120;
            GridViewBeforPolish.Columns["EmpID"].Width = 120;
            GridViewBeforPolish.Columns["Signature"].Width = 120;
            GridViewBeforPolish.Columns["DebitDate"].Width = 110;
            GridViewBeforPolish.Columns["DebitTime"].Width = 85;
            GridViewBeforPolish.Columns["EmpID"].Visible = false;
            GridViewBeforPolish.Columns["StoreName"].Visible = false;
            GridViewBeforPolish.Columns["EmpName"].Visible = false;
            GridViewBeforPolish.Columns["StoreID"].Visible = false;
            GridViewBeforPolish.Columns["ShownInNext"].Visible = false;

            if (UserInfo.Language == iLanguage.Arabic)
            {

                GridViewBeforPolish.Columns["EngItemName"].Visible = false;
                GridViewBeforPolish.Columns["EngSizeName"].Visible = false;
                GridViewBeforPolish.Columns["ArbItemName"].Width = 150;

                GridViewBeforPolish.Columns["StoreID"].Caption = "رقم المخزن";
                GridViewBeforPolish.Columns["StoreName"].Caption = "إسم المخزن";

                GridViewBeforPolish.Columns["EmpID"].Caption = "رقم العامل";
                GridViewBeforPolish.Columns["EmpName"].Caption = "إسم العامل";

                GridViewBeforPolish.Columns["MachinID"].Caption = "رقم المكينة";
                GridViewBeforPolish.Columns["MachineName"].Caption = "إسم المكينة";
                GridViewBeforPolish.Columns["Debit"].Caption = "الوزن";

                GridViewBeforPolish.Columns["Credit"].Caption = "دائــن";
                GridViewBeforPolish.Columns["TypeOpration"].Caption = "نوع العملية";
                GridViewBeforPolish.Columns["Signature"].Caption = "التوقيع";

                GridViewBeforPolish.Columns["ItemID"].Caption = "رقم الصنف";
                GridViewBeforPolish.Columns["ArbItemName"].Caption = "اسم الصنف";
                GridViewBeforPolish.Columns["SizeID"].Caption = "رقم الوحده";
                GridViewBeforPolish.Columns["ArbSizeName"].Caption = "الوحده";
                GridViewBeforPolish.Columns["CostPrice"].Caption = "التكلفة";
                GridViewBeforPolish.Columns["DebitDate"].Caption = "التاريخ";
                GridViewBeforPolish.Columns["DebitTime"].Caption = "الوقت";
            }
            else
            {
                GridViewBeforPolish.Columns["ArbItemName"].Visible = false;
                GridViewBeforPolish.Columns["ArbSizeName"].Visible = false;
                GridViewBeforPolish.Columns["EngItemName"].Width = 150;
                GridViewBeforPolish.Columns["StoreID"].Caption = "Store ID";
                GridViewBeforPolish.Columns["StoreName"].Caption = "Store Name";
                GridViewBeforPolish.Columns["EngItemName"].Caption = "Item Name";
                GridViewBeforPolish.Columns["MachinID"].Caption = "Machine ID";
                GridViewBeforPolish.Columns["MachineName"].Caption = "Machin Name";
                GridViewBeforPolish.Columns["Debit"].Caption = "debtor ";
                GridViewBeforPolish.Columns["EngSizeName"].Caption = "Unit";
                GridViewBeforPolish.Columns["Credit"].Caption = "Creditor";
                GridViewBeforPolish.Columns["TypeOpration"].Caption = "Type Opration";
                GridViewBeforPolish.Columns["Signature"].Caption = "Signature";
                GridViewBeforPolish.Columns["DebitDate"].Caption = "Date";
                GridViewBeforPolish.Columns["DebitTime"].Caption = "Time";
                GridViewBeforPolish.Columns["EmpID"].Caption = "EmpID";
                GridViewBeforPolish.Columns["EmpName"].Caption = "Name";
            }



        }
        void initGridAfterTalmee()
        {

            lstDetailAfterTalmee = new BindingList<Menu_FactoryRunCommandTalmee>();
            lstDetailAfterTalmee.AllowNew = true;
            lstDetailAfterTalmee.AllowEdit = true;
            lstDetailAfterTalmee.AllowRemove = true;
            gridControlAfterPolishing.DataSource = lstDetailAfterTalmee;


            DataTable dtitems0 = Lip.SelectRecord("SELECT   SizeID," + PrimaryName + "   FROM Stc_SizingUnits where BranchID=" + MySession.GlobalBranchID);
            string[] NameUnit = new string[dtitems0.Rows.Count];
            for (int i = 0; i <= dtitems0.Rows.Count - 1; i++)
                NameUnit[i] = dtitems0.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems0 = new RepositoryItemComboBox();
            riComboBoxitems0.Items.AddRange(NameUnit);
            gridControlAfterPolishing.RepositoryItems.Add(riComboBoxitems0);
            GridViewAfterPolish.Columns[SizeName].ColumnEdit = riComboBoxitems0;


            DataTable dtitems = Lip.SelectRecord("SELECT   " + PrimaryName + "   FROM Menu_FactoryMachine ");
            string[] NameMachine = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameMachine[i] = dtitems.Rows[i][PrimaryName].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameMachine);
            gridControlAfterPolishing.RepositoryItems.Add(riComboBoxitems);



            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0 and BranchID=" + MySession.GlobalBranchID);
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();
            riComboBoxitems2.Items.AddRange(StoreName);
            gridControlAfterPolishing.RepositoryItems.Add(riComboBoxitems2);
            GridViewAfterPolish.Columns["StoreName"].ColumnEdit = riComboBoxitems2;


            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0 and BranchID=" + MySession.GlobalBranchID);
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlAfterPolishing.RepositoryItems.Add(riComboBoxitems3);
            GridViewAfterPolish.Columns["EmpName"].ColumnEdit = riComboBoxitems3;

            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0 and BranchID=" + MySession.GlobalBranchID);
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlAfterPolishing.RepositoryItems.Add(riComboBoxitems4);
            GridViewAfterPolish.Columns[ItemName].ColumnEdit = riComboBoxitems4;
            GridViewAfterPolish.Columns["SizeID"].Visible = false;
            GridViewAfterPolish.Columns["MachineName"].ColumnEdit = riComboBoxitems;
            GridViewAfterPolish.Columns["ID"].Visible = false;
            GridViewAfterPolish.Columns["ComandID"].Visible = false;
            GridViewAfterPolish.Columns["BarcodeTalmee"].Visible = false;
            GridViewAfterPolish.Columns["EmpPolishnID"].Visible = false;
            GridViewAfterPolish.Columns["EmpPrentagID"].Visible = false;
            GridViewAfterPolish.Columns["Cancel"].Visible = false;
            GridViewAfterPolish.Columns["BranchID"].Visible = false;
            GridViewAfterPolish.Columns["FacilityID"].Visible = false;
            GridViewAfterPolish.Columns["MachinID"].Visible = false;
            GridViewAfterPolish.Columns["MachineName"].Visible = false;

            GridViewAfterPolish.Columns["EditUserID"].Visible = false;
            GridViewAfterPolish.Columns["EditDate"].Visible = false;
            GridViewAfterPolish.Columns["EditTime"].Visible = false;
            GridViewAfterPolish.Columns["RegDate"].Visible = false;
            GridViewAfterPolish.Columns["UserID"].Visible = false;

            GridViewAfterPolish.Columns["ComputerInfo"].Visible = false;
            GridViewAfterPolish.Columns["EditComputerInfo"].Visible = false;
            GridViewAfterPolish.Columns["RegTime"].Visible = false;

            GridViewAfterPolish.Columns["Debit"].Visible = false;
            GridViewAfterPolish.Columns["TypeOpration"].Visible = false;
            //GridViewAfterPolish.Columns["SizeID"].Visible = false;
            GridViewAfterPolish.Columns["CostPrice"].Visible = false;

            // GridViewAfterPolish.Columns["DebitTime"].Visible = false;
            GridViewAfterPolish.Columns["MachinID"].Name = "MachinID";
            //dtItem.Columns["Losed"].ReadOnly = true;
            GridViewAfterPolish.Columns["EmpName"].Width = 120;
            GridViewAfterPolish.Columns["StoreName"].Width = 120;
            GridViewAfterPolish.Columns["EmpID"].Width = 120;
            GridViewAfterPolish.Columns["Signature"].Width = 120;
            GridViewAfterPolish.Columns["DebitDate"].Width = 110;
            GridViewAfterPolish.Columns["DebitTime"].Width = 85;
            GridViewAfterPolish.Columns["EmpID"].Visible = false;
            GridViewAfterPolish.Columns["StoreName"].Visible = false;
            GridViewAfterPolish.Columns["EmpName"].Visible = false;
            GridViewAfterPolish.Columns["StoreID"].Visible = false;


            if (UserInfo.Language == iLanguage.Arabic)
            {
                GridViewAfterPolish.Columns["EngItemName"].Visible = false;
                GridViewAfterPolish.Columns["EngSizeName"].Visible = false;
                GridViewAfterPolish.Columns["ArbItemName"].Width = 150;
                GridViewAfterPolish.Columns["StoreID"].Caption = "رقم المخزن";
                GridViewAfterPolish.Columns["StoreName"].Caption = "إسم المخزن";

                GridViewAfterPolish.Columns["EmpID"].Caption = "رقم العامل";
                GridViewAfterPolish.Columns["EmpName"].Caption = "إسم العامل";

                GridViewAfterPolish.Columns["MachinID"].Caption = "رقم المكينة";
                GridViewAfterPolish.Columns["MachineName"].Caption = "إسم المكينة";
                GridViewAfterPolish.Columns["Debit"].Caption = "الوزن";

                GridViewAfterPolish.Columns["Credit"].Caption = "الوزن";
                GridViewAfterPolish.Columns["TypeOpration"].Caption = "نوع العملية";
                GridViewAfterPolish.Columns["Signature"].Caption = "التوقيع";

                GridViewAfterPolish.Columns["ItemID"].Caption = "رقم الصنف";
                GridViewAfterPolish.Columns["ArbItemName"].Caption = "اسم الصنف";
                GridViewAfterPolish.Columns["SizeID"].Caption = "رقم الوحده";
                GridViewAfterPolish.Columns["ArbSizeName"].Caption = "الوحده";
                GridViewAfterPolish.Columns["CostPrice"].Caption = "التكلفة";
                GridViewAfterPolish.Columns["DebitDate"].Caption = "التاريخ";
                GridViewAfterPolish.Columns["DebitTime"].Caption = "الوقت";
                GridViewAfterPolish.Columns["ShownInNext"].Caption = "يظهر في التفاصيل ";
            }
            else
            {
                GridViewAfterPolish.Columns["ArbItemName"].Visible = false;
                GridViewAfterPolish.Columns["ArbSizeName"].Visible = false;
                GridViewAfterPolish.Columns["EngItemName"].Width = 150;
                GridViewAfterPolish.Columns["StoreID"].Caption = "Store ID";
                GridViewAfterPolish.Columns["StoreName"].Caption = "Store Name";
                GridViewAfterPolish.Columns["EngItemName"].Caption = "Item Name";
                GridViewAfterPolish.Columns["MachinID"].Caption = "Machine ID";
                GridViewAfterPolish.Columns["MachineName"].Caption = "Machin Name";
                GridViewAfterPolish.Columns["Debit"].Caption = "debtor ";
                GridViewAfterPolish.Columns["EngSizeName"].Caption = "Unit";
                GridViewAfterPolish.Columns["Credit"].Caption = "Creditor";
                GridViewAfterPolish.Columns["TypeOpration"].Caption = "Type Opration";
                GridViewAfterPolish.Columns["Signature"].Caption = "Signature";
                GridViewAfterPolish.Columns["DebitDate"].Caption = "Date";
                GridViewAfterPolish.Columns["DebitDate"].Caption = "Time";
                GridViewAfterPolish.Columns["EmpID"].Caption = "EmpID";
                GridViewAfterPolish.Columns["EmpName"].Caption = "Name";
                GridViewAfterPolish.Columns["ShownInNext"].Caption = "Shown In Next";
            }



        }



        #endregion
        private void frmManufacturingOrder_Load(object sender, EventArgs e)
        {
            try
            {
                 
                
                
                if (Comon.cInt(cmbPollutionTypeID.EditValue) == 1)
                    cmbTypeStage.EditValue = 8;
                else if (Comon.cInt(cmbPollutionTypeID.EditValue) == 2)
                    cmbTypeStage.EditValue = 13;
                else if (Comon.cInt(cmbPollutionTypeID.EditValue) == 3)
                    cmbTypeStage.EditValue = 14;
                this.Text = cmbPollutionTypeID.Text.ToString();
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
                    strSQL = "SELECT " + PrimaryName + " as CustomerName  FROM Sales_CustomerAnSublierListArb Where    AcountID =" + txtCustomerID.Text + " and BranchID=" + MySession.GlobalBranchID;
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
                strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + txtEmpID.Text + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID;
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
                strSQL = "SELECT " + PrimaryName + " as UserName  FROM  [Users] where [Cancel]=0 and BranchID=" + MySession.GlobalBranchID+" and [UserID]=" + txtGuidanceID.Text.ToString();
                CSearch.ControlValidating(txtGuidanceID, lblGuidanceName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }


        private void txtOrderID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string strSql;
                DataTable dt;
                string txtOrder   = txtOrderID.Text;
                if (txtOrderID.Text != string.Empty && txtOrderID.Text != "0")
                {
                    int CommandIDTemp = 0;
                    CommandIDTemp = Comon.cInt(Lip.GetValue("select ComandID from Menu_FactoryRunCommandMaster where Cancel=0 and BranchID=" + MySession.GlobalBranchID+" and TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + " and ComandID<>" + Comon.cInt(txtCommandID.Text) + " and Barcode='" + txtOrderID.Text + "'"));
                    int CommandIDThis = Comon.cInt(Lip.GetValue("select ComandID from Menu_FactoryRunCommandMaster where Cancel=0 and BranchID=" + MySession.GlobalBranchID+" and TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + " and ComandID=" + Comon.cInt(txtCommandID.Text) + " and Barcode='" + txtOrderID.Text + "'"));
                    if (CommandIDTemp > 0)
                        groupBox1.Visible = true;
                    else
                        groupBox1.Visible = false;
                  
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
                    else if (IsNewRecord == false && CommandIDTemp > 0 && CommandIDThis != Comon.cInt(txtCommandID.Text))
                    {
                        //txtOrder = txtOrderID.Text;
                        //ClearFields();
                        //string OrderID = txtOrder;
                        //DoNew();
                        //txtOrderID.Text = OrderID;
                        if (CommandIDTemp > 0)
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheOrderAlreadyExists);
                        SetDetilOrder(txtOrderID.Text);
                        //IsNewRecord = true;
                        Validations.DoEditRipon(this, ribbonControl1);
                    }
                    else
                   if ((IsNewRecord))  // && CommandIDTemp <= 0
                    {
                        if (CommandIDTemp > 0)
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheOrderAlreadyExists);
                        string OrderID = txtOrder;


                        strSQL = "SELECT * FROM Manu_OrderRestriction WHERE  OrderID ='" + OrderID.Trim() + "' and BranchID=" + MySession.GlobalBranchID;
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

            DataTable dt = Manu_ZirconDiamondFactoryDAL.frmGetDataDetailByOrderID(OrderID, Comon.cInt(MySession.GlobalBranchID), UserInfo.FacilityID, Comon.cInt(cmbTypeStage.EditValue));

            if (dt.Rows.Count > 0)
            {
                gridControlOrderDetails.DataSource = lstOrderDetails;
                if (dt.Rows.Count > 0)
                {
                    gridControlOrderDetails.DataSource = dt;
                }
            }
        }
        public void GetValueORderID(string TextOrderID)
        {
            this.txtOrderID.Text = TextOrderID;
            txtOrderID_Validating(null, null);
            
        }
        private void txtEmployeeStokID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtEmployeeStokID.Text) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID;
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
            else if (FocusedControl.Trim() == gridControlBeforePolishing.Name)
            {

                if (GridViewBeforPolish.FocusedColumn.Name == "colItemID" || GridViewBeforPolish.FocusedColumn.Name == "col" + ItemName || GridViewBeforPolish.FocusedColumn.Name == "colBarCode")
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
                            GridViewBeforPolish.Columns[ItemName].ColumnEdit = rItem;
                            gridControlBeforePolishing.RepositoryItems.Add(rItem);

                        };
                    }
                    else
                        frm.Dispose();
                }
                else if (GridViewBeforPolish.FocusedColumn.Name == "colSizeName" || GridViewBeforPolish.FocusedColumn.Name == "colSizeID")
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


            else if (FocusedControl.Trim() == gridControlAfterPolishing.Name)
            {
                
                if (GridViewAfterPolish.FocusedColumn.Name == "colItemID" || GridViewAfterPolish.FocusedColumn.Name == "col" + ItemName || GridViewAfterPolish.FocusedColumn.Name == "colBarCode")
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
                            GridViewAfterPolish.Columns[ItemName].ColumnEdit = rItem;
                            gridControlAfterPolishing.RepositoryItems.Add(rItem);

                        };
                    }
                    else
                        frm.Dispose();
                }
                else if (GridViewAfterPolish.FocusedColumn.Name == "colSizeName" || GridViewAfterPolish.FocusedColumn.Name == "colSizeID")
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
                decimal ToatlBeforFactoryAmount = 0;
                decimal ToatlAfterFactoryAmount = 0;
                decimal TempQTY = 0;
                decimal TempQTYAfter = 0;
                for (int i = 0; i <= GridViewBeforPolish.DataRowCount - 1; i++)
                {
                    ToatlBeforFactoryQty += Comon.cDec(GridViewBeforPolish.GetRowCellValue(i, "Debit").ToString());
                    ToatlBeforFactoryAmount += Comon.cDec(GridViewBeforPolish.GetRowCellValue(i, "CostPrice").ToString());
                   
                    if (Comon.cInt(GridViewBeforPolish.GetRowCellValue(i, "SizeID").ToString()) == 2)
                        TempQTY +=Comon.cDec( Comon.cDec(GridViewBeforPolish.GetRowCellValue(i, "Debit").ToString())/ 5);
                    else
                        TempQTY +=Comon.cDec( Comon.cDec(GridViewBeforPolish.GetRowCellValue(i, "Debit").ToString()));
                }
               for (int i = 0; i <= GridViewAfterPolish.DataRowCount - 1; i++)
                {
                    ToatlAfterFactoryQty += Comon.cDec(GridViewAfterPolish.GetRowCellValue(i, "Credit").ToString());
                    ToatlAfterFactoryAmount += Comon.cDec(GridViewAfterPolish.GetRowCellValue(i, "CostPrice").ToString());

                    if (Comon.cInt(GridViewAfterPolish.GetRowCellValue(i, "SizeID").ToString()) == 2)
                        TempQTYAfter += Comon.cDec(Comon.cDec(GridViewAfterPolish.GetRowCellValue(i, "Credit").ToString()) / 5);
                    else
                        TempQTYAfter += Comon.cDec(Comon.cDec(GridViewAfterPolish.GetRowCellValue(i, "Credit").ToString()));
                }
                ToatlQty.Text = TempQTY.ToString();
                ToatlQtyAfter.Text = TempQTYAfter.ToString();
                txtTotalBefor.Text = ToatlBeforFactoryQty.ToString();
                txtTotalAfter.Text = ToatlAfterFactoryQty.ToString();
                lblTotallostFactory.Text = Comon.cDec(TempQTY-TempQTYAfter).ToString();

                txtTotalAmountBefor.Text = ToatlBeforFactoryAmount.ToString();
                txtTotalAmountAfter.Text = ToatlAfterFactoryAmount.ToString();
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

            if(FocusedControl == null) return;

            else if(FocusedControl.Trim() == txtStoreID.Name)
            {
                if (!MySession.GlobalAllowChangefrmPolishnStoreID&& (Comon.cInt(cmbPollutionTypeID.EditValue)==1)) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (!MySession.GlobalAllowChangefrmPolishn2StoreID && (Comon.cInt(cmbPollutionTypeID.EditValue) == 2)) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (!MySession.GlobalAllowChangefrmPolishn3StoreID && (Comon.cInt(cmbPollutionTypeID.EditValue) == 3)) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "رقم الحساب", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "Account ID", Comon.cInt(cmbBranchesID.EditValue));
            }

           

            else if(FocusedControl.Trim() == txtCommandID.Name)
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
                if (!MySession.GlobalAllowChangefrmPolishnAccountID && (Comon.cInt(cmbPollutionTypeID.EditValue) == 1)) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (!MySession.GlobalAllowChangefrmPolishn2AccountID && (Comon.cInt(cmbPollutionTypeID.EditValue) == 2)) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (!MySession.GlobalAllowChangefrmPolishn3AccountID && (Comon.cInt(cmbPollutionTypeID.EditValue) == 3)) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
               
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
                        PrepareSearchQuery.Find(ref cls, txtOrderID, txtOrderID, "OrderID", "رقم الطلب", Comon.cInt(cmbBranchesID.EditValue), "  and OrderID not in(select Barcode as OrderID from Menu_FactoryRunCommandMaster where Cancel=0 and BranchID=" + MySession.GlobalBranchID+" and TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + ") ");
                    else
                        PrepareSearchQuery.Find(ref cls, txtOrderID, txtOrderID, "OrderID", "Order ID", Comon.cInt(cmbBranchesID.EditValue), "  and OrderID not in(select Barcode as OrderID  from Menu_FactoryRunCommandMaster where Cancel=0 and BranchID=" + MySession.GlobalBranchID+"  and TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + ") ");
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
                if (!MySession.GlobalAllowChangefrmPolishnEmployeeID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtEmpID, lblEmpName, "EmployeeID", "رقـم العامل", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtEmpID, lblEmpName, "EmployeeID", "Worker ID", Comon.cInt(cmbBranchesID.EditValue));
            }
            
             
            //امين المخزن
            else if (FocusedControl.Trim() == txtEmployeeStokID.Name)
            {
                if (!MySession.GlobalAllowChangefrmPolishnEmployeeID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtEmployeeStokID, txtEmployeeStokName, "EmployeeID", "رقـم العامل", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtEmployeeStokID, txtEmployeeStokName, "EmployeeID", "Worker ID", Comon.cInt(cmbBranchesID.EditValue));
            }
             
 
            //الجرايد فيو
            
            else if (FocusedControl.Trim() == gridControlBeforePolishing.Name)
            {
                if (GridViewBeforPolish.FocusedColumn.Name == "colBarcodeTalmee" || GridViewBeforPolish.FocusedColumn.Name == "colItemName" || GridViewBeforPolish.FocusedColumn.Name == "colItemID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "BarCode", MySession.GlobalBranchID);
                }
                if (GridViewBeforPolish.FocusedColumn.Name == "colStoreID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "رقم الـمـســتـودع", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "Store ID", Comon.cInt(cmbBranchesID.EditValue));
                }
              
                if (GridViewBeforPolish.FocusedColumn.Name == "MachinID")
                {
                    if (GridViewBeforPolish.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "رقم المكينة", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "Machine ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                else if (GridViewBeforPolish.FocusedColumn.Name == "colSizeID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "رقـم الـوحـــده", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "Size ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                else if (GridViewBeforPolish.FocusedColumn.Name == "colEmpID")
                {
                    if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "رقـم العامل", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "Worker ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                else if (GridViewBeforPolish.FocusedColumn.Name == "colDebit")
                {
                    frmRemindQtyItem frm = new frmRemindQtyItem();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);

                        frm.Show();
                        if (GridViewBeforPolish.GetRowCellValue(GridViewBeforPolish.FocusedRowHandle, "ItemID") != null)
                            frm.SetValueToControl(GridViewBeforPolish.GetRowCellValue(GridViewBeforPolish.FocusedRowHandle, "ItemID").ToString(), txtStoreID.Text.ToString());
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
            else if (FocusedControl.Trim() == gridControlAfterPolishing.Name)
            {
                if (GridViewAfterPolish.FocusedColumn.Name == "colBarcodeTalmee" || GridViewAfterPolish.FocusedColumn.Name == "colItemName" || GridViewAfterPolish.FocusedColumn.Name == "colItemID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "BarCode", MySession.GlobalBranchID);
                }
                if (GridViewAfterPolish.FocusedColumn.Name == "colStoreID")
                {

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "رقم الـمـســتـودع", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "Store ID", Comon.cInt(cmbBranchesID.EditValue));
                }
               
                if (GridViewAfterPolish.FocusedColumn.Name == "MachinID")
                {
                    if (GridViewAfterPolish.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "رقم المكينة", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "Machine ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                else if (GridViewAfterPolish.FocusedColumn.Name == "colSizeID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "رقـم الـوحـــده", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "Size ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                else if (GridViewAfterPolish.FocusedColumn.Name == "colEmpID")
                {
                    if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "رقـم العامل", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "Worker ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                else if (GridViewAfterPolish.FocusedColumn.Name == "colCredit")
                {
                    frmRemindQtyItem frm = new frmRemindQtyItem();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);

                        frm.Show();

                        if (GridViewAfterPolish.GetRowCellValue(GridViewAfterPolish.FocusedRowHandle, "ItemID") != null)
                            frm.SetValueToControl(GridViewAfterPolish.GetRowCellValue(GridViewAfterPolish.FocusedRowHandle, "ItemID").ToString(), txtStoreID.Text.ToString());
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
        void FileDataMachinName(GridView Grid,string date,string time,int MachinID)
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
                

                else if(FocusedControl.Trim() == txtOrderID.Name)
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
                else if (FocusedControl ==  txtEmpID.Name)
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
                else if (FocusedControl.Trim() == gridControlBeforePolishing.Name)
                {
                    if (GridViewBeforPolish.FocusedColumn.Name == "colBarcodeTalmee" || GridViewBeforPolish.FocusedColumn.Name == "colItemName" || GridViewBeforPolish.FocusedColumn.Name == "colItemID")
                    {
                        string Barcode = cls.PrimaryKeyValue.ToString();
                        if (Stc_itemsDAL.CheckIfStopItemUnit(Barcode, MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                        {
                            Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                            return;
                        }
                        GridViewBeforPolish.AddNewRow();
                        GridViewBeforPolish.SetRowCellValue(GridViewBeforPolish.FocusedRowHandle, GridViewBeforPolish.Columns["BarcodePrentag"], Barcode);
                         FillItemData(GridViewBeforPolish, gridControlBeforePolishing, "BarcodeTalmee", "Debit", Stc_itemsDAL.GetItemData1(Barcode.ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", txtAccountID);
                       
                    }
                    if (GridViewBeforPolish.FocusedColumn.Name == "colStoreID")
                    {
                        GridViewBeforPolish.SetRowCellValue(GridViewBeforPolish.FocusedRowHandle, GridViewBeforPolish.Columns["StoreID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID;
                        GridViewBeforPolish.SetRowCellValue(GridViewBeforPolish.FocusedRowHandle, GridViewBeforPolish.Columns["StoreName"], Lip.GetValue(strSQL));
                    }
                     
                    if (GridViewBeforPolish.FocusedColumn.Name == "MachinID")
                    {
                        GridViewBeforPolish.AddNewRow();
                        FileDataMachinName(GridViewBeforPolish, "DebitDate", "DebitTime", Comon.cInt(cls.PrimaryKeyValue.ToString()));
                    }
                    if (GridViewBeforPolish.FocusedColumn.Name == "colSizeID")
                    {
                        GridViewBeforPolish.SetRowCellValue(GridViewBeforPolish.FocusedRowHandle, GridViewBeforPolish.Columns["SizeID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as " + SizeName + " FROM Stc_SizingUnits WHERE SizeID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID;
                        GridViewBeforPolish.SetRowCellValue(GridViewBeforPolish.FocusedRowHandle, GridViewBeforPolish.Columns[SizeName], Lip.GetValue(strSQL));

                    }
                    if (GridViewBeforPolish.FocusedColumn.Name == "colEmpID")
                    {
                        GridViewBeforPolish.SetRowCellValue(GridViewBeforPolish.FocusedRowHandle, GridViewBeforPolish.Columns["EmpID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(cls.PrimaryKeyValue.ToString()) + " And Cancel =0  and BranchID=" + MySession.GlobalBranchID;
                        GridViewBeforPolish.SetRowCellValue(GridViewBeforPolish.FocusedRowHandle, GridViewBeforPolish.Columns["EmpName"], Lip.GetValue(strSQL));
                    }
                }
                else if (FocusedControl.Trim() == gridControlAfterPolishing.Name)
                {
                    if (GridViewAfterPolish.FocusedColumn.Name == "colBarcodeTalmee" || GridViewAfterPolish.FocusedColumn.Name == "colItemName" || GridViewAfterPolish.FocusedColumn.Name == "colItemID")
                    {
                        string Barcode = cls.PrimaryKeyValue.ToString();
                        if (Stc_itemsDAL.CheckIfStopItemUnit(Barcode, MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                        {
                            Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                            return;
                        }
                        GridViewAfterPolish.AddNewRow();
                        GridViewAfterPolish.SetRowCellValue(GridViewAfterPolish.FocusedRowHandle, GridViewAfterPolish.Columns["BarcodePrentag"], Barcode);
                         FillItemData(GridViewAfterPolish, gridControlAfterPolishing, "BarcodeTalmee", "Credit", Stc_itemsDAL.GetItemData1(Barcode.ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", txtAccountID);
                        
                    }
                    if (GridViewAfterPolish.FocusedColumn.Name == "colStoreID")
                    {
                        GridViewAfterPolish.SetRowCellValue(GridViewAfterPolish.FocusedRowHandle, GridViewAfterPolish.Columns["StoreID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID;
                        GridViewAfterPolish.SetRowCellValue(GridViewAfterPolish.FocusedRowHandle, GridViewAfterPolish.Columns["StoreName"], Lip.GetValue(strSQL));

                    }
 
                    if (GridViewAfterPolish.FocusedColumn.Name == "MachinID")
                    {
                        GridViewAfterPolish.AddNewRow();
                        FileDataMachinName(GridViewAfterPolish, "DebitDate", "DebitTime", Comon.cInt(cls.PrimaryKeyValue.ToString()));
                    }
                    if (GridViewAfterPolish.FocusedColumn.Name == "colSizeID")
                    {
                        GridViewAfterPolish.SetRowCellValue(GridViewAfterPolish.FocusedRowHandle, GridViewAfterPolish.Columns["SizeID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as " + SizeName + " FROM Stc_SizingUnits WHERE SizeID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID;
                        GridViewAfterPolish.SetRowCellValue(GridViewAfterPolish.FocusedRowHandle, GridViewAfterPolish.Columns[SizeName], Lip.GetValue(strSQL));

                    }
                    if (GridViewAfterPolish.FocusedColumn.Name == "colEmpID")
                    {
                        GridViewAfterPolish.SetRowCellValue(GridViewAfterPolish.FocusedRowHandle, GridViewAfterPolish.Columns["EmpID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID;
                        GridViewAfterPolish.SetRowCellValue(GridViewAfterPolish.FocusedRowHandle, GridViewAfterPolish.Columns["EmpName"], Lip.GetValue(strSQL));
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
             
            EnableGridView(GridViewBeforPolish, Value,1);
            EnableGridView(GridViewAfterPolish, Value,1);
        }
        
        void EnableGridView( GridView GridViewObj, bool Value, int flage)
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
                    strSQL = "SELECT TOP 1 * FROM " + Menu_FactoryRunCommandMasterDAL.TableName + " Where  TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + " and   Cancel =0 and BranchID=" + MySession.GlobalBranchID;
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
        #endregion
         List<Manu_AllOrdersDetails> SaveOrderDetials()
         {

             Manu_AllOrdersDetails returned = new Manu_AllOrdersDetails();
             List<Manu_AllOrdersDetails> listreturned = new List<Manu_AllOrdersDetails>();
             for (int i = 0; i <= GridViewBeforPolish.DataRowCount - 1; i++)
             {
                 returned = new Manu_AllOrdersDetails();
                 returned.ID = i + 1;
                 returned.CommandID = Comon.cInt(txtCommandID.Text);
                 returned.FacilityID = UserInfo.FacilityID;
                 returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                 returned.BarCode = GridViewBeforPolish.GetRowCellValue(i, "BarcodeTalmee").ToString();
                 returned.ItemID = Comon.cInt(GridViewBeforPolish.GetRowCellValue(i, "ItemID").ToString());
                 returned.TypeStageID = Comon.cInt(cmbTypeStage.EditValue);
                 returned.SizeID = Comon.cInt(GridViewBeforPolish.GetRowCellValue(i, "SizeID").ToString());
                 returned.ArbSizeName = GridViewBeforPolish.GetRowCellValue(i, SizeName).ToString();
                 returned.EngSizeName = GridViewBeforPolish.GetRowCellValue(i, SizeName).ToString();
                 returned.ArbItemName = GridViewBeforPolish.GetRowCellValue(i, ItemName).ToString();
                 returned.EngItemName = GridViewBeforPolish.GetRowCellValue(i, ItemName).ToString();
                 returned.QTY = Comon.ConvertToDecimalQty(GridViewBeforPolish.GetRowCellValue(i, "Debit").ToString());
                 returned.CostPrice = 0;
                 returned.TotalCost = 0;
                 listreturned.Add(returned);
             }
             int LengBefore = GridViewBeforPolish.DataRowCount + 1;
             for (int i = 0; i <= GridViewAfterPolish.DataRowCount - 1; i++)
             {
                 returned = new Manu_AllOrdersDetails();
                 returned.ID = LengBefore;
                 returned.CommandID = Comon.cInt(txtCommandID.Text);
                 returned.FacilityID = UserInfo.FacilityID;
                 returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                 returned.BarCode = GridViewAfterPolish.GetRowCellValue(i, "BarcodeTalmee").ToString();
                 returned.ItemID = Comon.cInt(GridViewAfterPolish.GetRowCellValue(i, "ItemID").ToString());
                 returned.TypeStageID = Comon.cInt(cmbTypeStage.EditValue);
                 returned.SizeID = Comon.cInt(GridViewAfterPolish.GetRowCellValue(i, "SizeID").ToString());
                 returned.ArbSizeName = GridViewAfterPolish.GetRowCellValue(i, SizeName).ToString();
                 returned.EngSizeName = GridViewAfterPolish.GetRowCellValue(i, SizeName).ToString();
                 returned.ArbItemName = GridViewAfterPolish.GetRowCellValue(i, ItemName).ToString();
                 returned.EngItemName = GridViewAfterPolish.GetRowCellValue(i, ItemName).ToString();
                 returned.QTY = Comon.ConvertToDecimalQty(GridViewAfterPolish.GetRowCellValue(i, "Credit").ToString());
                 returned.ShownInNext = Comon.cbool(GridViewAfterPolish.GetRowCellValue(i, "ShownInNext").ToString());
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
                GridViewBeforPolish.MoveLast();
                GridViewAfterPolish.MoveLast();
                Menu_FactoryRunCommandMaster objRecord = new Menu_FactoryRunCommandMaster();
                objRecord.Barcode = txtOrderID.Text.ToString();
                objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                objRecord.BrandID = Comon.cInt(cmbBranchesID.EditValue);
                objRecord.PollutionTypeID = Comon.cInt(cmbPollutionTypeID.EditValue);
                objRecord.Cancel = 0;
                objRecord.Posted = Comon.cInt(cmbStatus.EditValue);
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
                objRecord.EmpPolishnID = 0;
                objRecord.EmpPrentagID = 0;
                objRecord.FacilityID = UserInfo.FacilityID;
                objRecord.ComandDate = Comon.ConvertDateToSerial(txtCommandDate.Text.ToString());
                objRecord.GoldCompundNet = 0;
                objRecord.GroupID = 0;
                objRecord.ItemID = 0;
                objRecord.DelegateID = Comon.cDbl(txtDelegateID.Text);
                //الحسابات

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


                #region Save Talmee

                Menu_FactoryRunCommandTalmee returnedTalmee;
                List<Menu_FactoryRunCommandTalmee> listreturnedTalmee = new List<Menu_FactoryRunCommandTalmee>();
                //تلميع 
                int lengthTalmee = GridViewBeforPolish.DataRowCount;
                int lengthAfterTalmee = GridViewAfterPolish.DataRowCount;
                if (lengthTalmee > 0)
                {
                    
                    {
                        for (int i = 0; i < lengthTalmee; i++)
                        {
                            returnedTalmee = new Menu_FactoryRunCommandTalmee();
                            returnedTalmee.ID = i + 1;
                            returnedTalmee.ComandID = Comon.cInt(txtCommandID.Text.ToString());

                            returnedTalmee.MachinID = Comon.cInt(GridViewBeforPolish.GetRowCellValue(i, "MachinID").ToString());
                            returnedTalmee.Debit = Comon.cDbl(GridViewBeforPolish.GetRowCellValue(i, "Debit").ToString());
                            returnedTalmee.TypeOpration = 1;
                            returnedTalmee.StoreID = Comon.cInt(txtStoreID.Text.ToString());
                            returnedTalmee.StoreName = lblStoreName.Text.ToString();
                            returnedTalmee.BarcodeTalmee= GridViewBeforPolish.GetRowCellValue(i, "BarcodeTalmee").ToString();
                            returnedTalmee.EmpID = txtEmpID.Text.ToString();
                            returnedTalmee.EmpName = lblEmpName.Text.ToString();

                            returnedTalmee.SizeID = Comon.cInt(GridViewBeforPolish.GetRowCellValue(i, "SizeID").ToString());
                            returnedTalmee.ItemID = Comon.cInt(GridViewBeforPolish.GetRowCellValue(i, "ItemID").ToString());
                            returnedTalmee.DebitDate = Comon.cDate(GridViewBeforPolish.GetRowCellValue(i, "DebitDate").ToString());
                            returnedTalmee.DebitTime = GridViewBeforPolish.GetRowCellValue(i, "DebitTime").ToString();
                            returnedTalmee.ArbItemName = GridViewBeforPolish.GetRowCellValue(i, ItemName).ToString();
                            returnedTalmee.EngItemName = GridViewBeforPolish.GetRowCellValue(i, ItemName).ToString();
                            returnedTalmee.ArbSizeName = GridViewBeforPolish.GetRowCellValue(i, SizeName).ToString();
                            returnedTalmee.EngSizeName = GridViewBeforPolish.GetRowCellValue(i, SizeName).ToString();
                            returnedTalmee.MachineName = GridViewBeforPolish.GetRowCellValue(i, "MachineName").ToString();
                            returnedTalmee.BranchID = UserInfo.BRANCHID;
                            returnedTalmee.EmpPolishnID = Comon.cDbl(txtEmpID.Text);
                            returnedTalmee.Cancel = 0;
                            returnedTalmee.UserID = UserInfo.ID;
                            returnedTalmee.RegDate = Comon.ConvertDateToSerial(Lip.GetServerDate());
                            returnedTalmee.RegTime = Comon.cDbl(Lip.GetServerTimeSerial());
                            returnedTalmee.ComputerInfo = UserInfo.ComputerInfo;
                            returnedTalmee.FacilityID = UserInfo.FacilityID;
                            if (IsNewRecord == false)
                            {
                                returnedTalmee.EditUserID = UserInfo.ID;
                                returnedTalmee.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                                returnedTalmee.EditDate = Comon.ConvertDateToSerial(Lip.GetServerDate());
                                returnedTalmee.EditComputerInfo = UserInfo.ComputerInfo;
                            }
                            listreturnedTalmee.Add(returnedTalmee);
                        }
                        if (lengthAfterTalmee > 0)
                        {

                            for (int i = 0; i < lengthAfterTalmee; i++)
                            {
                                returnedTalmee = new Menu_FactoryRunCommandTalmee();
                                returnedTalmee.ID = i + 1;
                                returnedTalmee.ComandID = Comon.cInt(txtCommandID.Text.ToString());

                                returnedTalmee.MachinID = Comon.cInt(GridViewAfterPolish.GetRowCellValue(i, "MachinID").ToString());
                                returnedTalmee.Credit = Comon.cDbl(GridViewAfterPolish.GetRowCellValue(i, "Credit").ToString());
                                returnedTalmee.TypeOpration = 2;

                                returnedTalmee.StoreID = Comon.cInt(txtStoreID.Text.ToString());
                                returnedTalmee.StoreName = lblStoreName.Text.ToString();
                                returnedTalmee.EmpID = txtEmpID.Text.ToString();
                                returnedTalmee.EmpName = lblEmpName.Text.ToString();
                                returnedTalmee.BarcodeTalmee = GridViewAfterPolish.GetRowCellValue(i, "BarcodeTalmee").ToString();
                                returnedTalmee.SizeID = Comon.cInt(GridViewAfterPolish.GetRowCellValue(i, "SizeID").ToString());
                                returnedTalmee.ItemID = Comon.cInt(GridViewAfterPolish.GetRowCellValue(i, "ItemID").ToString());
                                returnedTalmee.DebitDate = Comon.cDate(GridViewAfterPolish.GetRowCellValue(i, "DebitDate").ToString());
                                returnedTalmee.DebitTime = GridViewAfterPolish.GetRowCellValue(i, "DebitTime").ToString();
                                returnedTalmee.ArbItemName = GridViewAfterPolish.GetRowCellValue(i, ItemName).ToString();
                                returnedTalmee.EngItemName = GridViewAfterPolish.GetRowCellValue(i, ItemName).ToString();
                                returnedTalmee.ArbSizeName = GridViewAfterPolish.GetRowCellValue(i, SizeName).ToString();
                                returnedTalmee.EngSizeName = GridViewAfterPolish.GetRowCellValue(i, SizeName).ToString();
                                returnedTalmee.MachineName = GridViewAfterPolish.GetRowCellValue(i, "MachineName").ToString();


                                returnedTalmee.ShownInNext =Comon.cbool( GridViewAfterPolish.GetRowCellValue(i, "ShownInNext").ToString());
                                returnedTalmee.BranchID = UserInfo.BRANCHID;
                                returnedTalmee.EmpPolishnID = Comon.cDbl(txtEmpID.Text);
                                returnedTalmee.Cancel = 0;
                                returnedTalmee.UserID = UserInfo.ID;
                                returnedTalmee.RegDate = Comon.ConvertDateToSerial(Lip.GetServerDate());
                                returnedTalmee.RegTime = Comon.cDbl(Lip.GetServerTimeSerial());
                                returnedTalmee.ComputerInfo = UserInfo.ComputerInfo;
                                returnedTalmee.FacilityID = UserInfo.FacilityID;
                                if (IsNewRecord == false)
                                {
                                    returnedTalmee.EditUserID = UserInfo.ID;
                                    returnedTalmee.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                                    returnedTalmee.EditDate = Comon.ConvertDateToSerial(Lip.GetServerDate());
                                    returnedTalmee.EditComputerInfo = UserInfo.ComputerInfo;
                                }
                                listreturnedTalmee.Add(returnedTalmee);
                            }


                        }
                    }
                }
                #endregion
                if (listreturnedTalmee.Count > 0)
                {
                    objRecord.Menu_F_Talmee = listreturnedTalmee;

                    objRecord.Manu_OrderDetils = SaveOrderDetials();
                    string Result = Menu_FactoryRunCommandMasterDAL.InsertUsingXML(objRecord, IsNewRecord).ToString();
                    if (Comon.cInt(Result) > 0 && Comon.cInt(cmbStatus.EditValue)>1)
                    {
                        //أوامر الصرف والتوريد الخاص بالتصنيع
                        if (lengthTalmee > 0)
                        {
                            //أوامر الصرف والتوريد الخاص بالبرنتاج
                            //SaveOutOnPolshin(); //حفظ   الصرف المخزني
                            // حفظ الحركة المخزنية 
                            if (Comon.cInt(Result) > 0)
                            {
                                int MoveID = SaveStockMoveingBrntageOut(Comon.cInt(Result));
                                if (MoveID == 0)
                                    Messages.MsgError(Messages.TitleInfo, "خطا في حفظ الحركة المخزنية تلميع - قبل ");

                                //حفظ القيد الالي
                                long VoucherID = SaveVariousVoucherMachinPolshin(Comon.cInt(Result), IsNewRecord);
                                if (VoucherID == 0)
                                    Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                                else
                                    Lip.ExecututeSQL("Update " + Menu_FactoryRunCommandTalmeeDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Menu_FactoryRunCommandTalmeeDAL.PremaryKey + " = " + Result + " and BranchID=" + MySession.GlobalBranchID);
                            }
                        }
                        if (lengthAfterTalmee > 0)
                        {
                            //SaveInOnPolshin(); //حفظ   التوريد المخزني
                            // حفظ الحركة المخزنية 
                            if (Comon.cInt(Result) > 0)
                            {
                                bool isNew = true;
                                DataTable dtCount = null;
                                if (Comon.cInt(cmbPollutionTypeID.EditValue) == 1)
                                    dtCount = Stc_ItemsMoviingDAL.GetCountElementID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(Result), DocumentTypePloshinAfterFrist);
                                else if (Comon.cInt(cmbPollutionTypeID.EditValue) == 2)
                                    dtCount = Stc_ItemsMoviingDAL.GetCountElementID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(Result), DocumentTypePloshinAfterScand);
                                else if (Comon.cInt(cmbPollutionTypeID.EditValue) == 3)
                                    dtCount = Stc_ItemsMoviingDAL.GetCountElementID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(Result), DocumentTypePloshinAfterTheerd);
                                
                                if (Comon.cInt(dtCount.Rows[0][0]) > 0)
                                    isNew = false;

                                int MoveID = SaveStockMoveingBrntageIn(Comon.cInt(Result));
                                if (MoveID == 0)
                                    Messages.MsgError(Messages.TitleInfo, "خطا في حفظ الحركة المخزنية تلميع - بعد");

                                //حفظ القيد الالي
                                long VoucherID = SaveVariousVoucherMachinInOnPolshin(Comon.cInt(Result), isNew);
                                if (VoucherID == 0)
                                    Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                                else
                                    Lip.ExecututeSQL("Update " + Menu_FactoryRunCommandTalmeeDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Menu_FactoryRunCommandTalmeeDAL.PremaryKey + " = " + Result + " and BranchID=" + MySession.GlobalBranchID);

                            }
                        }
                    }
                    if (Comon.cInt(Result) > 0)
                    {

                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                        //ClearFields();
                        DoNew();
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
            if(Comon.cInt(cmbPollutionTypeID.EditValue)==1)
              objRecord.DocumentType = DocumentTypePloshinAfterFrist;
            else if (Comon.cInt(cmbPollutionTypeID.EditValue) == 2)
                objRecord.DocumentType = DocumentTypePloshinAfterScand;

            else if (Comon.cInt(cmbPollutionTypeID.EditValue) == 3)
                objRecord.DocumentType = DocumentTypePloshinAfterTheerd;

            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " And DocumentType=" + objRecord.DocumentType + " And BranchID=" + Comon.cInt(cmbBranchesID.EditValue)));

            objRecord.VoucherID = VoucherID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = MySession.GlobalFacilityID;
            //Date
            objRecord.VoucherDate = Comon.ConvertDateToSerial(((DateTime)GridViewAfterPolish.GetRowCellValue(GridViewAfterPolish.DataRowCount - 1, "DebitDate")).ToString("dd/MM/yyyy")).ToString(); 
            
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

            returned.DebitGold = Comon.cDbl(ToatlQtyAfter.Text.ToString());
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
            returned.CreditGold = Comon.cDbl(ToatlQtyAfter.Text.ToString());
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
            
             if (Comon.cInt(cmbPollutionTypeID.EditValue) == 1)
                objRecord.DocumentType = DocumentTypePloshinBeforeFrist;
            else if (Comon.cInt(cmbPollutionTypeID.EditValue) == 2)
                objRecord.DocumentType = DocumentTypePloshinBeforeScand;
             else if (Comon.cInt(cmbPollutionTypeID.EditValue) == 3)
                 objRecord.DocumentType = DocumentTypePloshinBeforeTheerd;
            
            
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " And DocumentType=" + objRecord.DocumentType + " And BranchID=" + Comon.cInt(cmbBranchesID.EditValue)));

            objRecord.VoucherID = VoucherID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = MySession.GlobalFacilityID;
            //Date

            objRecord.VoucherDate = Comon.ConvertDateToSerial(((DateTime)GridViewBeforPolish.GetRowCellValue(GridViewBeforPolish.DataRowCount - 1, "DebitDate")).ToString("dd/MM/yyyy")).ToString(); 
            
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
            
            returned.DebitGold = Comon.cDbl(ToatlQty.Text.ToString());
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
            returned.CreditGold = Comon.cDbl(ToatlQty.Text.ToString());
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
        
        private int SaveStockMoveingBrntageOut(int DocumentID)
        {
            Stc_ItemsMoviing objRecord = new Stc_ItemsMoviing();
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            if (Comon.cInt(cmbPollutionTypeID.EditValue) == 1)
                objRecord.DocumentTypeID = DocumentTypePloshinBeforeFrist;
            else if (Comon.cInt(cmbPollutionTypeID.EditValue) == 2)
                objRecord.DocumentTypeID = DocumentTypePloshinBeforeScand;
            else if (Comon.cInt(cmbPollutionTypeID.EditValue) == 3)
                objRecord.DocumentTypeID = DocumentTypePloshinBeforeTheerd;
            objRecord.MoveType = 2;
            objRecord.MoveID = 0;
            objRecord.TranseID = DocumentID;
            objRecord.Posted = Comon.cInt(cmbStatus.EditValue);
            Stc_ItemsMoviing returned;
            List<Stc_ItemsMoviing> listreturned = new List<Stc_ItemsMoviing>();
            for (int i = 0; i <= GridViewBeforPolish.DataRowCount - 1; i++)
            {
                returned = new Stc_ItemsMoviing();
                returned.ID = i + 1;
                returned.MoveDate = Comon.ConvertDateToSerial(((DateTime)GridViewBeforPolish.GetRowCellValue(i, "DebitDate")).ToString("dd/MM/yyyy")).ToString(); 
                returned.MoveID = 0;
                returned.TranseID = DocumentID;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);     
                if (Comon.cInt(cmbPollutionTypeID.EditValue) == 1)
                    returned.DocumentTypeID = DocumentTypePloshinBeforeFrist;
                else if (Comon.cInt(cmbPollutionTypeID.EditValue) == 2)
                    returned.DocumentTypeID = DocumentTypePloshinBeforeScand;
                else if (Comon.cInt(cmbPollutionTypeID.EditValue) == 3)
                    returned.DocumentTypeID = DocumentTypePloshinBeforeTheerd;
                returned.MoveType = 2;
                returned.StoreID = Comon.cDbl(txtStoreID.Text.ToString());
                returned.AccountID = Comon.cDbl(txtAccountID.Text);
                returned.BarCode = GridViewBeforPolish.GetRowCellValue(i, "BarcodeTalmee").ToString();
                returned.ItemID = Comon.cInt(GridViewBeforPolish.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(GridViewBeforPolish.GetRowCellValue(i, "SizeID").ToString());
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID + " and BranchID=" + MySession.GlobalBranchID));
                returned.QTY = Comon.cDbl(GridViewBeforPolish.GetRowCellValue(i, "Debit").ToString());
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
           
            if (Comon.cInt(cmbPollutionTypeID.EditValue) == 1)
                objRecord.DocumentTypeID = DocumentTypePloshinAfterFrist;
            else if (Comon.cInt(cmbPollutionTypeID.EditValue) == 2)
                objRecord.DocumentTypeID = DocumentTypePloshinAfterScand;
            else if (Comon.cInt(cmbPollutionTypeID.EditValue) == 3)
                objRecord.DocumentTypeID = DocumentTypePloshinAfterTheerd;
            objRecord.MoveType = 1;
            objRecord.MoveID = 0;
            objRecord.TranseID = DocumentID;
            objRecord.Posted = Comon.cInt(cmbStatus.EditValue);
            Stc_ItemsMoviing returned;
            List<Stc_ItemsMoviing> listreturned = new List<Stc_ItemsMoviing>();
            for (int i = 0; i <= GridViewAfterPolish.DataRowCount - 1; i++)
            {
                returned = new Stc_ItemsMoviing();
                returned.ID = i + 1;
                returned.MoveDate = Comon.ConvertDateToSerial(((DateTime)GridViewAfterPolish.GetRowCellValue(i, "DebitDate")).ToString("dd/MM/yyyy")).ToString(); 
                returned.MoveID = 0;
                returned.TranseID = DocumentID;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            
                if (Comon.cInt(cmbPollutionTypeID.EditValue) == 1)
                    returned.DocumentTypeID = DocumentTypePloshinAfterFrist;
                else if (Comon.cInt(cmbPollutionTypeID.EditValue) == 2)
                    returned.DocumentTypeID = DocumentTypePloshinAfterScand;
                else if (Comon.cInt(cmbPollutionTypeID.EditValue) == 3)
                    returned.DocumentTypeID = DocumentTypePloshinAfterTheerd;
                returned.MoveType = 1;
                returned.StoreID = Comon.cDbl(txtStoreID.Text.ToString());
                returned.AccountID = Comon.cDbl(txtAccountID.Text);
                returned.BarCode = GridViewAfterPolish.GetRowCellValue(i, "BarcodeTalmee").ToString();
                returned.ItemID = Comon.cInt(GridViewAfterPolish.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(GridViewAfterPolish.GetRowCellValue(i, "SizeID").ToString());
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID + " and BranchID=" + MySession.GlobalBranchID));
                returned.QTY = Comon.cDbl(GridViewAfterPolish.GetRowCellValue(i, "Credit").ToString());
                returned.InPrice = Comon.cDbl(Lip.AverageUnit(Comon.cInt(returned.ItemID), Comon.cInt(returned.SizeID), Comon.cDbl(txtStoreID.Text)));
                //returned.Bones = Comon.cDbl(GridCastingBefore.GetRowCellValue(i, "Bones").ToString());
                returned.OutPrice = 0;
                returned.CostCenterID = Comon.cInt ( MySession.GlobalDefaultCostCenterID);
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
                    if (Comon.cInt(cmbPollutionTypeID.EditValue) == 1)
                    {
                        MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text), DocumentTypePloshinBeforeFrist);
                        MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text), DocumentTypePloshinAfterFrist);
                    }
                    else if(Comon.cInt(cmbPollutionTypeID.EditValue)==2)
                    {
                        MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text), DocumentTypePloshinBeforeScand);
                        MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text), DocumentTypePloshinAfterScand);
                    }
                    else if (Comon.cInt(cmbPollutionTypeID.EditValue) == 3)
                    {
                        MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text), DocumentTypePloshinBeforeTheerd);
                        MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text), DocumentTypePloshinAfterTheerd);
                    }
                     if (MoveID <0)
                         Messages.MsgError(Messages.TitleInfo, "خطا في حذف الحركة  المخزنية");
                 }

                 #region Delete Voucher Machin
                 //حذف القيد الالي
                 if (Comon.cInt(Result) > 0)
                 {
                     
                     int VoucherIDPolishnBefore = 0;
                    int VoucherIDPolishbAfter = 0;
                    if (Comon.cInt(cmbPollutionTypeID.EditValue) == 1)
                    {
                        VoucherIDPolishnBefore = DeleteVariousVoucherMachin(Comon.cInt(txtCommandID.Text), DocumentTypePloshinBeforeFrist);
                        VoucherIDPolishbAfter = DeleteVariousVoucherMachin(Comon.cInt(txtCommandID.Text), DocumentTypePloshinAfterFrist);

                    }
                    else if (Comon.cInt(cmbPollutionTypeID.EditValue) == 2)
                    {
                        VoucherIDPolishnBefore = DeleteVariousVoucherMachin(Comon.cInt(txtCommandID.Text), DocumentTypePloshinBeforeScand);
                        VoucherIDPolishbAfter = DeleteVariousVoucherMachin(Comon.cInt(txtCommandID.Text), DocumentTypePloshinAfterScand);
                    }
                    else if (Comon.cInt(cmbPollutionTypeID.EditValue) == 3)
                    {
                        VoucherIDPolishnBefore = DeleteVariousVoucherMachin(Comon.cInt(txtCommandID.Text), DocumentTypePloshinBeforeTheerd);
                        VoucherIDPolishbAfter = DeleteVariousVoucherMachin(Comon.cInt(txtCommandID.Text), DocumentTypePloshinAfterTheerd);

                    }


                    VoucherIDPolishbAfter = DeleteVariousVoucherMachin(Comon.cInt(txtCommandID.Text), DocumentTypePloshinAfterFrist);
                         

                    if (VoucherIDPolishnBefore == 0)
                        Messages.MsgError(Messages.TitleInfo, "خطا في حذف قيد العملية تلميع-قبل");
                    if (VoucherIDPolishbAfter == 0)
                        Messages.MsgError(Messages.TitleInfo, "خطا في حذف قيد العملية تلميع-بعد");
                }
                 #endregion

                 #region Delete Stock IN Or Out From archive
                 ////حذف التوريد والصرف من الارشيف
                 //if (Comon.cInt(Result) > 0)
                 //{
                    

                 //    int OutPolishnID = 0;
                 //    DataTable dtInvoiceIDPolishnBefor = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(txtCommandID.Text), DocumentTypePloshinBeforeFrist);
                 //    if (dtInvoiceIDPolishnBefor.Rows.Count > 0)
                 //    {
                 //        OutPolishnID = DeleteInOnOROutOnBil(Comon.cInt(dtInvoiceIDPolishnBefor.Rows[0][0]), DocumentTypePloshinBeforeFrist);
                 //        if (OutPolishnID == 0)
                 //            Messages.MsgError(Messages.TitleInfo, "خطا في حذف الصرف من الارشيف للعلية تلميع- قبل  ");
                 //    }
                 //    int InPolishnID = 0;
                 //    DataTable dtInvoiceIDPolishnAfter = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(txtCommandID.Text), DocumentTypePloshinAfterFrist);
                 //    if (dtInvoiceIDPolishnAfter.Rows.Count > 0)
                 //    {
                 //        InPolishnID = DeleteInOnOROutOnBil(Comon.cInt(dtInvoiceIDPolishnAfter.Rows[0][0]), DocumentTypePloshinAfterFrist);
                 //        if (InPolishnID == 0)
                 //            Messages.MsgError(Messages.TitleInfo, "خطا في حذف التوريد من الارشيف للعملية تلميع- بعد ");
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
                cmbCurency.EditValue = Comon.cInt(MySession.GlobalDefaultPolishinCurrncyID);
                //جريد فيو
                initGridBeforTalmee();
                initGridAfterTalmee();
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
                 if (pictureEdit1.Image==null)
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

                strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtEmpID.Text) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID;
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
                strSQL = "SELECT " + PrimaryName + " as AccountName FROM Acc_Accounts WHERE AccountID =" + Comon.cDbl(txtAccountID.Text) + " And Cancel =0  and BranchID=" + MySession.GlobalBranchID;
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

                strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID in( Select StoreManger from Stc_Stores WHERE AccountID =" + Comon.cLong(txtStoreID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue) + ") And Cancel =0 ";
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

                strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtEmployeeStokID.Text) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID;
                CSearch.ControlValidating(txtEmployeeStokID, txtEmployeeStokName, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

              

        private void GridViewBeforfactory_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
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
        private void GridViewBeforPolish_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName != "Signature")
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
            int ID = 0;
            if(Comon.cInt(cmbPollutionTypeID.EditValue)==1)
              ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" +  txtCommandID.Text + " And DocumentType=" + DocumentTypePloshinBeforeFrist).ToString());
            else if (Comon.cInt(cmbPollutionTypeID.EditValue) == 2)
                ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + txtCommandID.Text + " And DocumentType=" + DocumentTypePloshinBeforeScand).ToString());
            else if (Comon.cInt(cmbPollutionTypeID.EditValue) == 3)
                ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + txtCommandID.Text + " And DocumentType=" + DocumentTypePloshinBeforeTheerd).ToString());

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
            int ID = 0;
            if (Comon.cInt(cmbPollutionTypeID.EditValue) == 1)
                ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + txtCommandID.Text + " And DocumentType=" + DocumentTypePloshinAfterFrist).ToString());
            else if (Comon.cInt(cmbPollutionTypeID.EditValue) == 2)
                ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + txtCommandID.Text + " And DocumentType=" + DocumentTypePloshinAfterScand).ToString());
            else if (Comon.cInt(cmbPollutionTypeID.EditValue) == 3)
                ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + txtCommandID.Text + " And DocumentType=" + DocumentTypePloshinAfterTheerd).ToString());
            
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
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
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
                ReportName = "rptManu_FactoryTalmeOpretion";
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

                rptForm.Parameters["TotalQTY"].Value = txtTotalBefor.Text;
                rptForm.Parameters["TotalLost"].Value = txtTotalAfter.Text;
                rptForm.Parameters["NumberCrews"].Value = "";
                rptForm.Parameters["CupsLost"].Value = lblTotallostFactory.Text; ;
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
                subreportBeforeCasting.ReportSource = Manu_PrentagdStage(GridViewBeforPolish);

                /******************** Report Factory ************************/
                XRSubreport subreportFactor = (XRSubreport)rptForm.FindControl("subRptrptManu_FactoryFactorCommendBefore", true);
                subreportFactor.Visible = IncludeHeader;
                subreportFactor.ReportSource = Manu_PrentagdStage(GridViewAfterPolish);


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
                        GridViewBeforPolish.AddNewRow();
                        if (ChekOrderIsFoundInGrid(GridViewBeforPolish, "BarcodeTalmee", BarCode))
                        {
                            Messages.MsgAsterisk(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "الصنف موجود  لذلك لا يمكن انزاله  اكثر من مرة " : "This Item is Found Table");
                            GridViewBeforPolish.DeleteRow(rowIndex);
                            return;
                        }
                       
                        string QTY = view.GetRowCellValue(view.FocusedRowHandle, "QTY").ToString();
                        FillItemData(GridViewBeforPolish, gridControlBeforePolishing, "BarcodeTalmee", "Debit", Stc_itemsDAL.GetItemData1(BarCode, UserInfo.FacilityID), "DebitDate", "DebitTime", ((TextEdit)txtAccountID), QTY);


                        SendKeys.Send("\t");

                    }

                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void GridViewBeforPolish_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            rowIndex = e.RowHandle;
        }

        private void GridViewBeforPolish_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                if (view.GetRowCellValue(view.FocusedRowHandle, "BarcodeTalmee").ToString().Trim() != "")
                {
                    string BarCode = view.GetRowCellValue(view.FocusedRowHandle, "BarcodeTalmee").ToString().Trim();
                    DataTable dt;
                    dt = Stc_itemsDAL.GetItemData(BarCode, UserInfo.FacilityID);
                    if (dt.Rows.Count > 0)
                    {
                        GridViewAfterPolish.AddNewRow();
                        if (ChekOrderIsFoundInGrid(GridViewAfterPolish, "BarcodeTalmee", BarCode))
                        {
                            Messages.MsgAsterisk(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "الصنف موجود  لذلك لا يمكن انزاله  اكثر من مرة " : "This Item is Found Table");
                            GridViewAfterPolish.DeleteRow(rowIndex);
                            return;
                        }                       
                        string QTY = view.GetRowCellValue(view.FocusedRowHandle, "Debit").ToString();
                        FillItemData(GridViewAfterPolish, gridControlAfterPolishing, "BarcodeTalmee", "Credit", Stc_itemsDAL.GetItemData1(BarCode, UserInfo.FacilityID), "DebitDate", "DebitTime", txtAccountID, QTY);
                        SendKeys.Send("\t");
                    }
                }
            }
            catch(Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void GridViewAfterPolish_InitNewRow(object sender, InitNewRowEventArgs e)
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

        private void btnDims_Click(object sender, EventArgs e)
        {
            frmTransferMultipleStoresGold frm = new frmTransferMultipleStoresGold();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
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

                dtItem.Columns.Add("Caliber", System.Type.GetType("System.Decimal"));
                dtItem.Columns.Add("ExpiryDate", System.Type.GetType("System.String"));
                dtItem.Columns.Add("Bones", System.Type.GetType("System.Decimal"));
                dtItem.Columns.Add("SalePrice", System.Type.GetType("System.Decimal"));

                dtItem.Columns.Add("GroupID", System.Type.GetType("System.String"));

                dtItem.Columns.Add(GroupName, System.Type.GetType("System.String"));
                dtItem.Columns.Add("StoreAccountID", System.Type.GetType("System.String"));

                dtItem.Columns.Add("StoreName", System.Type.GetType("System.String"));
                dtItem.Columns.Add("Equivalen", System.Type.GetType("System.Decimal"));
                dtItem.Columns.Add("CaliberEquivalen", System.Type.GetType("System.Decimal"));
                dtItem.Columns.Add("TotalCost", System.Type.GetType("System.Decimal"));
                for (int i = 0; i <= GridViewAfterPolish.DataRowCount - 1; i++)
                {
                    dtItem.Rows.Add();
                    dtItem.Rows[i]["ID"] = i;
                    dtItem.Rows[i]["FacilityID"] = UserInfo.FacilityID;
                    dtItem.Rows[i]["BarCode"] = GridViewAfterPolish.GetRowCellValue(i, "BarcodeTalmee").ToString();
                    dtItem.Rows[i]["ItemID"] = Comon.cInt(GridViewAfterPolish.GetRowCellValue(i, "ItemID").ToString());
                    DataTable dt = Lip.SelectRecord("SELECT   [GroupID]  ," + PrimaryName + "  FROM  [Stc_ItemsGroups] where Cancel=0 and BranchID=" + MySession.GlobalBranchID+" and [GroupID] in(select [GroupID] from Stc_Items where ItemID=" + dtItem.Rows[i]["ItemID"] + " and Cancel=0 and BranchID=" + MySession.GlobalBranchID+")  ");
                    dtItem.Rows[i]["GroupID"] = Comon.cDbl(dt.Rows[0]["GroupID"]);
                    dtItem.Rows[i][GroupName] = dt.Rows[0][PrimaryName].ToString();

                    dtItem.Rows[i]["SizeID"] = Comon.cInt(GridViewAfterPolish.GetRowCellValue(i, "SizeID").ToString());
                    dtItem.Rows[i][ItemName] = GridViewAfterPolish.GetRowCellValue(i, ItemName).ToString();
                    dtItem.Rows[i][SizeName] = GridViewAfterPolish.GetRowCellValue(i, SizeName).ToString();
                    dtItem.Rows[i]["QTY"] = Comon.ConvertToDecimalQty(GridViewAfterPolish.GetRowCellValue(i, "Credit").ToString());
                    //  dtItem.Rows[i]["PackingQty"] = Comon.ConvertToDecimalPrice(GridViewAfterPolish.GetRowCellValue(i, "PackingQty").ToString());
                    dtItem.Rows[i]["SalePrice"] = 0;

                    dtItem.Rows[i]["Description"] = UserInfo.Language == iLanguage.Arabic ? "تحويل من مرحلة  "+this.Text : "Transfer from manufacturing";

                    dtItem.Rows[i]["StoreAccountID"] = Comon.cDbl(txtStoreID.Text);
                    dtItem.Rows[i]["StoreName"] =lblStoreName.Text.ToString();
                    dtItem.Rows[i]["Caliber"] = 18;

                    dtItem.Rows[i]["CostPrice"] = Comon.ConvertToDecimalPrice(GridViewAfterPolish.GetRowCellValue(i, "CostPrice").ToString());
                    dtItem.Rows[i]["TotalCost"] = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(dtItem.Rows[i]["CostPrice"]) * Comon.cDec(dtItem.Rows[i]["QTY"]));

                    dtItem.Rows[i]["Equivalen"] = 0;
                    dtItem.Rows[i]["CaliberEquivalen"] = 18;

                    dtItem.Rows[i]["Cancel"] = 0;

                }

                frm.ReadRecordFromOutScreen(dtItem);

            }
            else
                frm.Dispose();
        }

        private void btnToPrev_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtOrderID.Text) != true)
            {
                strSQL = "SELECT TOP 1 ComandID FROM " + Menu_FactoryRunCommandMasterDAL.TableName + " Where  TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + " and BranchID=" + MySession.GlobalBranchID+" and  Cancel =0 and  ComandID<" + Comon.cLong(txtCommandID.Text) + " and Barcode=" + txtOrderID.Text;
                int commandID = Comon.cInt(Lip.GetValue(strSQL));
                if (commandID > 0)
                {
                    txtCommandID.Text = commandID.ToString();
                    txtCommandID_Validating(null, null);
                }
            } 
        }

        private void btnToNext_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtOrderID.Text) != true)
            {
                strSQL = "SELECT TOP 1 ComandID FROM " + Menu_FactoryRunCommandMasterDAL.TableName + " Where  TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + " and  Cancel =0 and BranchID=" + MySession.GlobalBranchID+" and  ComandID>" + Comon.cLong(txtCommandID.Text) + " and Barcode=" + txtOrderID.Text;
                int commandID = Comon.cInt(Lip.GetValue(strSQL));
                if (commandID > 0)
                {
                    txtCommandID.Text = commandID.ToString();
                    txtCommandID_Validating(null, null);

                }
            } 
        }
    }


}
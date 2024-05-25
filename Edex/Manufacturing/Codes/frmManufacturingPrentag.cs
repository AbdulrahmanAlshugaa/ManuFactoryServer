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
using DevExpress.Map.Native;
using Edex.HR.Codes;
using Edex.StockObjects.Codes;
using System.Globalization;
using Edex.StockObjects.Transactions;

namespace Edex.Manufacturing.Codes
{
    public partial class frmManufacturingPrentag : BaseForm
    {
        //list detail
        BindingList<Menu_FactoryRunCommandPrentagAndPulishn> lstDetailAfterPrentage = new BindingList<Menu_FactoryRunCommandPrentagAndPulishn>();
        BindingList<Menu_FactoryRunCommandPrentagAndPulishn> lstDetailPrentage = new BindingList<Menu_FactoryRunCommandPrentagAndPulishn>();


        BindingList<Menu_FactoryOrderDetails> lstOrderDetails = new BindingList<Menu_FactoryOrderDetails>();
        BindingList<Menu_FactoryRunCommandfactory> lstDetailfactory = new BindingList<Menu_FactoryRunCommandfactory>();
        BindingList<Menu_FactoryRunCommandfactory> lstDetailAfterfactory = new BindingList<Menu_FactoryRunCommandfactory>();
        BindingList<Manu_ProductionExpensesDetails> lstDetailProductionExpenses = new BindingList<Manu_ProductionExpensesDetails>();
        BindingList<Manu_AuxiliaryMaterialsDetails> lstDetailAlcadZircon = new BindingList<Manu_AuxiliaryMaterialsDetails>();
        BindingList<Stc_ItemUnits> lstDetailUnit = new BindingList<Stc_ItemUnits>();
        #region Declare 
        public int DocumentTypeBrntageBeforeFrist = 34;
        public int DocumentTypeBrntageAfterFrist = 35;

        int rowIndex = 0;
        public int DocumentTypeBrntageBeforeScand = 36;
        public int DocumentTypeBrntageAfterScand = 37;
        private Menu_FactoryRunCommandMasterDAL cClass = new Menu_FactoryRunCommandMasterDAL();

        public CultureInfo culture = new CultureInfo("en-US");
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
          private string GroupName;
        #endregion
        public frmManufacturingPrentag()
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

                this.gridControlBeforPrentag.ProcessGridKey += gridControl2_ProcessGridKey;
                this.gridControlAfterPrentage.ProcessGridKey += gridControl2_ProcessGridKey;

                this.GridViewBeforPrentag.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.GridViewBeforfactory_ValidatingEditor);
                this.GridViewAfterPrentag.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.GridViewAfterfactory_ValidatingEditor);
                this.GridViewBeforPrentag.RowUpdated += GridViewBeforfactory_RowUpdated;
                this.GridViewAfterPrentag.RowUpdated += GridViewBeforfactory_RowUpdated;

                ItemName = "ArbItemName";
                SizeName = "ArbSizeName";
                PrimaryName = "ArbName";
                CaptionItemName = "اسم الصنف";
                  GroupName = "ArbGroupName";
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
                cmbTypeStage.EditValue = 7;
                cmbTypeStage.ReadOnly = true;

                cmbCurency.EditValue = 0;
                cmbBranchesID.EditValue = MySession.GlobalBranchID;
                cmbBranchesID.ReadOnly = !MySession.GlobalAllowBranchModificationAllScreens;
                FillCombo.FillComboBox(cmbPrntageTypeID, "Manu_TypePrntage", "ID", PrimaryName, "", "", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                FillCombo.FillComboBox(cmbStatus, "Manu_TypeStatus", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                cmbStatus.EditValue = MySession.GlobalDefaultProcessPostedStatus;


                this.GridViewAfterPrentag.CellValueChanging += GridViewAfterPrentag_CellValueChanging;
                EnableControlDefult();
                initGridBeforPrentage();
                initGridAfterPrentage();
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

            cmbCurency.ReadOnly = !MySession.GlobalAllowChangefrmPrntageCurrncyID;
            txtCommandDate.ReadOnly = !MySession.GlobalAllowChangefrmPrntageCommandDate;
          
            if (Comon.cInt(cmbPrntageTypeID.EditValue) == 2)
            {
                txtStoreID.ReadOnly = !MySession.GlobalAllowChangefrmPrntage2StoreID;
                txtAccountID.ReadOnly = !MySession.GlobalAllowChangefrmPrntage2AccountID;
            }
            else
            {
                txtStoreID.ReadOnly = !MySession.GlobalAllowChangefrmPrntageStoreID;
                txtAccountID.ReadOnly = !MySession.GlobalAllowChangefrmPrntageAccountID;
            }
            txtEmpID.ReadOnly = !MySession.GlobalAllowChangefrmPrntageEmployeeID;

        }
        void SetDefultValue()
        {

            cmbCurency.EditValue =Comon.cInt( MySession.GlobalDefaultPrntageCurrncyID);
            cmbCurency_EditValueChanged(null, null);
            txtStoreID.Text = MySession.GlobalDefaultPrntageStoreID;
          
            txtAccountID.Text = MySession.GlobalDefaultPrntageAccountID;
            if (Comon.cInt(cmbPrntageTypeID.EditValue) == 2)
            {
                txtStoreID.Text = MySession.GlobalDefaultPrntage2StoreID;
                txtAccountID.Text = MySession.GlobalDefaultPrntage2AccountID;
            }
            txtStoreIDFactory_Validating(null, null);
            txtAccountIDFactory_Validating(null, null);
            txtEmpID.Text = MySession.GlobalDefaultPrntageEmployeeID;
            txtEmpIDFactor_Validating(null, null);
        }
        void GridViewAfterPrentag_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                if (((view.GetRowCellValue(e.RowHandle, "ItemID") == null)||Comon.cInt(view.GetRowCellValue(e.RowHandle, "ItemID"))<=0) && e.Column.FieldName == "ShownInNext")
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
            if (this.GridViewAfterPrentag.ActiveEditor is CheckEdit)
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
            if (this.GridViewAfterPrentag.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                HasColumnErrors = false;
                string ColName = view.FocusedColumn.FieldName;
                int ItemID = 0;
                if (ColName == "EmpID"||ColName=="StoreID" || ColName == "SizeID" || ColName == "ItemID" || ColName == "MachinID" || ColName == "PrentagCredit" || ColName == "PrentagDebit")
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
                        view.SetColumnError(GridViewAfterPrentag.Columns[ColName], "");
                    }

                    if (ColName == "MachinID")
                    {


                        DataTable dtGroupID = Lip.SelectRecord("Select " + PrimaryName + " from Menu_FactoryMachine Where MachineID=" + Comon.cInt(e.Value));
                        if (dtGroupID.Rows.Count > 0)
                        {
                            FileDataMachinName(GridViewAfterPrentag, "PrentagDebitDate", "PrentagDebitTime", Comon.cInt(e.Value));

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
                            FillItemData(GridViewAfterPrentag, gridControlAfterPrentage, "BarcodePrentag", "PrentagCredit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "PrentagDebitDate", "PrentagDebitTime", txtAccountID);
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
                            GridViewAfterPrentag.SetRowCellValue(GridViewAfterPrentag.FocusedRowHandle, GridViewAfterPrentag.Columns[SizeName], dtItemID.Rows[0][PrimaryName].ToString());
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

                            GridViewAfterPrentag.SetRowCellValue(GridViewAfterPrentag.FocusedRowHandle, GridViewAfterPrentag.Columns["EmpName"], dtNameEmp.Rows[0][PrimaryName].ToString());
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
                            GridViewAfterPrentag.SetFocusedRowCellValue("StoreName", dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(GridViewAfterPrentag.Columns[ColName], "");

                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(GridViewAfterPrentag.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }



                }
                if (ColName == ItemName)
                {

                    DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_Items_Find  Where  LOWER (" + ItemName + ")=LOWER ('" + val.ToString() + "') and BranchID=" + MySession.GlobalBranchID);
                    if (dtItemID.Rows.Count > 0)
                    {
                        FillItemData(GridViewAfterPrentag, gridControlAfterPrentage, "BarcodePrentag", "PrentagCredit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "PrentagDebitDate", "PrentagDebitTime", ((TextEdit)txtAccountID));
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = " الصنف غير موجود  ";
                    }
                }
                else if (ColName == "PrentagDebitDate")
                {
                        string formattedDate = ((DateTime)e.Value).ToString("yyyy/MM/dd");
                        if (Lip.CheckDateISAvilable(formattedDate))
                        {
                            string serverDate = Lip.GetServerDate(); e.Value = serverDate;
                            GridViewAfterPrentag.SetRowCellValue(GridViewAfterPrentag.FocusedRowHandle, "PrentagDebitDate", serverDate);
                            return;
                        }
                }
                if (ColName == "MachineName")
                {

                    DataTable dtMachinID = Lip.SelectRecord("Select  MachineID  from Menu_FactoryMachine Where  LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') ");
                    if (dtMachinID.Rows.Count > 0)
                    {
                        GridViewAfterPrentag.SetFocusedRowCellValue("MachinID", dtMachinID.Rows[0]["MachineID"].ToString());

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
                    string Str = "Select Stc_ItemUnits.SizeID from Stc_ItemUnits inner join Stc_Items on Stc_Items.ItemID=Stc_ItemUnits.ItemID and Stc_Items.BranchID=Stc_ItemUnits.BranchID left outer join  Stc_SizingUnits on Stc_ItemUnits.SizeID=Stc_SizingUnits.SizeID and Stc_ItemUnits.BranchID=Stc_SizingUnits.BranchID Where UnitCancel=0 and BranchID=" + MySession.GlobalBranchID+" And LOWER (Stc_SizingUnits." + PrimaryName + ")=LOWER ('" + val.ToString() + "') and  Stc_Items.ItemID=" + Comon.cLong(GridViewAfterPrentag.GetRowCellValue(GridViewAfterPrentag.FocusedRowHandle, "ItemID").ToString()) + "  And Stc_ItemUnits.FacilityID=" + UserInfo.FacilityID;
                    DataTable dtSizeID = Lip.SelectRecord(Str);
                    if (dtSizeID.Rows.Count > 0)
                    {
                        FillItemData(GridViewAfterPrentag, gridControlAfterPrentage, "BarcodePrentag", "PrentagCredit", Stc_itemsDAL.GetItemDataByItemID_SizeID(Comon.cInt(GridViewAfterPrentag.GetRowCellValue(GridViewAfterPrentag.FocusedRowHandle, "ItemID")), Comon.cInt(dtSizeID.Rows[0][0].ToString()), UserInfo.FacilityID), "PrentagDebitDate", "PrentagDebitTime", ((TextEdit)txtAccountID));
                        GridViewAfterPrentag.SetRowCellValue(GridViewAfterPrentag.FocusedRowHandle, ColName, e.Value.ToString());
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundSizeForItem;
                        view.SetColumnError(GridViewBeforPrentag.Columns[ColName], Messages.msgNoFoundSizeForItem);
                    }

                }
                if (ColName == "EmpName")
                {

                    DataTable dtMachinID = Lip.SelectRecord("Select  EmployeeID  from HR_EmployeeFile Where  LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') and BranchID=" + MySession.GlobalBranchID);
                    if (dtMachinID.Rows.Count > 0)
                    {
                        GridViewAfterPrentag.SetFocusedRowCellValue("EmpID", dtMachinID.Rows[0]["EmployeeID"].ToString());

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
                        GridViewAfterPrentag.SetFocusedRowCellValue("StoreID", dtItemID.Rows[0]["StoreID"]);
                        e.Valid = true;
                        view.SetColumnError(GridViewAfterPrentag.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(GridViewAfterPrentag.Columns[ColName], Messages.msgNoFoundThisItem);
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
                    if (((GridView)Grid).Name == GridViewBeforPrentag.Name)
                    {
                        totalQtyBalance = Lip.RemindQtyItemByMinUnit(Comon.cLong(dt.Rows[0]["ItemID"].ToString()), Comon.cInt(dt.Rows[0]["SizeID"]), Comon.cDbl(txtStoreID.Text));
                        {
                            decimal qtyCurrent = 0;
                            decimal QtyInCommand = Lip.GetQTYinCommandToThisItem("Menu_FactoryRunCommandPrentagAndPulishn", "Menu_FactoryRunCommandMaster", QTYFildName, "ComandID", Comon.cInt(txtCommandID.Text), dt.Rows[0]["ItemID"].ToString(), " and Menu_FactoryRunCommandPrentagAndPulishn.TypeOpration=1 and Menu_FactoryRunCommandPrentagAndPulishn.PrntageTypeID=" + Comon.cInt(cmbPrntageTypeID.EditValue), BarCode,SizeID:Comon.cInt(dt.Rows[0]["SizeID"].ToString()));
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
                
                if ( (((GridView)Grid).Name ==GridViewBeforPrentag.Name))
                    Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[QTYFildName], totalQtyBalance);
                else
                    Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[QTYFildName], 0);

                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[Time], DateTime.Now.ToString("hh:mm:tt"));
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[Date], DateTime.Now.ToString("yyyy/MM/dd"));
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns["MachinID"], 3);
                strSQL = "SELECT " + PrimaryName + " FROM Menu_FactoryMachine WHERE MachineID =3";
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns["MachineName"], Lip.GetValue(strSQL));

                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns["ID"], 0);
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[BarCode], dt.Rows[0]["BarCode"].ToString().ToUpper());
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns["ItemID"], dt.Rows[0]["ItemID"].ToString());
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[ItemName], dt.Rows[0][PrimaryName].ToString());
                if(UserInfo.Language==iLanguage.English)
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

            if (this.GridViewBeforPrentag.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                HasColumnErrors = false;
                string ColName = view.FocusedColumn.FieldName;
                int ItemID = 0;
                if (ColName == "EmpID"||ColName=="StoreID" || ColName == "SizeID" || ColName == "ItemID" || ColName == "MachinID" || ColName == "PrentagCredit" || ColName == "PrentagDebit")
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
                        view.SetColumnError(GridViewBeforPrentag.Columns[ColName], "");
                    }
                    if (ColName == "MachinID")
                    {
                        DataTable dtGroupID = Lip.SelectRecord("Select " + PrimaryName + " from Menu_FactoryMachine Where MachineID=" + Comon.cInt(e.Value));
                        if (dtGroupID.Rows.Count > 0)
                        {                        
                            e.Valid = true;
                            view.SetColumnError(GridViewBeforPrentag.Columns[ColName], "");
                            GridViewBeforPrentag.SetRowCellValue(GridViewBeforPrentag.FocusedRowHandle, GridViewBeforPrentag.Columns["ID"], GridViewBeforPrentag.RowCount);
                            
                            FileDataMachinName(GridViewBeforPrentag,"PrentagDebitDate", "PrentagDebitTime", Comon.cInt(e.Value));     
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
                            FillItemData(GridViewBeforPrentag, gridControlBeforPrentag, "BarcodePrentag", "PrentagDebit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "PrentagDebitDate", "PrentagDebitTime", ((TextEdit)txtAccountID));
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
                            GridViewBeforPrentag.SetRowCellValue(GridViewBeforPrentag.FocusedRowHandle, GridViewBeforPrentag.Columns[SizeName], dtItemID.Rows[0][PrimaryName].ToString());
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = "رقم الوحدة غير موجود  ";
                        }
                    }
                    if (ColName == "PrentagDebit") 
                    {
                        decimal totalQtyBalance = Lip.RemindQtyItemByMinUnit(Comon.cLong(GridViewBeforPrentag.GetRowCellValue(GridViewBeforPrentag.FocusedRowHandle, "ItemID").ToString()), Comon.cInt(GridViewBeforPrentag.GetRowCellValue(GridViewBeforPrentag.FocusedRowHandle, "SizeID")), Comon.cDbl(txtStoreID.Text));
                        decimal QtyInCommand = Lip.GetQTYinCommandToThisItem("Menu_FactoryRunCommandPrentagAndPulishn", "Menu_FactoryRunCommandMaster", "PrentagDebit", "ComandID", Comon.cInt(txtCommandID.Text), GridViewBeforPrentag.GetRowCellValue(GridViewBeforPrentag.FocusedRowHandle, "ItemID").ToString(), " and Menu_FactoryRunCommandPrentagAndPulishn.TypeOpration=1 and Menu_FactoryRunCommandPrentagAndPulishn.PrntageTypeID=" + Comon.cInt(cmbPrntageTypeID.EditValue),"BarcodePrentag",SizeID:Comon.cInt(GridViewBeforPrentag.GetRowCellValue(GridViewBeforPrentag.FocusedRowHandle, "SizeID").ToString()));
                        totalQtyBalance += QtyInCommand;
                        decimal qtyCurrent = 0;
                         qtyCurrent = frmCadFactory.GetQTYToItemFromGridView(GridViewBeforPrentag, "PrentagDebit", Comon.cDec(val.ToString()), GridViewBeforPrentag.GetRowCellValue(GridViewBeforPrentag.FocusedRowHandle, "ItemID").ToString(), Comon.cInt(GridViewBeforPrentag.GetRowCellValue(GridViewBeforPrentag.FocusedRowHandle, "SizeID")), "BarcodePrentag");
                    
                        if (qtyCurrent > totalQtyBalance)
                        {
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheQTyinOrderisExceed);
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgQtyisNotAvilable + (totalQtyBalance - (qtyCurrent - Comon.cDec(val.ToString())));
                            view.SetColumnError(GridViewBeforPrentag.Columns[ColName], Messages.msgQtyisNotAvilable + totalQtyBalance.ToString());
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
                                    view.SetColumnError(GridViewBeforPrentag.Columns[ColName], Messages.msgQtyisNotAvilable + totalQtyBalance.ToString());
                                }
                            }
                            else
                            {
                                e.Valid = false;
                                HasColumnErrors = true;
                                e.ErrorText = Messages.msgNotFoundAnyQtyInStore;
                                view.SetColumnError(GridViewBeforPrentag.Columns[ColName], Messages.msgNotFoundAnyQtyInStore);
                            }
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
                            GridViewBeforPrentag.SetRowCellValue(GridViewBeforPrentag.FocusedRowHandle, GridViewBeforPrentag.Columns["EmpName"], dtNameEmp.Rows[0][PrimaryName].ToString());
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
                            GridViewBeforPrentag.SetFocusedRowCellValue("StoreName", dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(GridViewBeforPrentag.Columns[ColName], "");
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(GridViewBeforPrentag.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }
                }
                else if (ColName == "PrentagDebitDate")
                {
                    {
                        string formattedDate = ((DateTime)e.Value).ToString("yyyy/MM/dd");
                        if (Lip.CheckDateISAvilable(formattedDate))
                        {
                            string serverDate = Lip.GetServerDate(); e.Value = serverDate;
                            GridViewBeforPrentag.SetRowCellValue(GridViewBeforPrentag.FocusedRowHandle, "PrentagDebitDate", serverDate);
                            return;
                        }
                    }
                }
                if (ColName == ItemName)
                {
                    DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_Items_Find  Where  LOWER (" + ItemName + ")=LOWER ('" + val.ToString() + "') and BranchID=" + MySession.GlobalBranchID);
                    if (dtItemID.Rows.Count > 0)
                    {    
                       FillItemData(GridViewBeforPrentag,gridControlBeforPrentag, "BarcodePrentag", "PrentagDebit",Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "PrentagDebitDate", "PrentagDebitTime", ((TextEdit)txtAccountID));
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
                        FileDataMachinName(GridViewBeforPrentag, "PrentagDebitDate", "PrentagDebitTime", Comon.cInt(dtMachinID.Rows[0]["MachineID"].ToString()));
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

                    string Str = "Select Stc_ItemUnits.SizeID from Stc_ItemUnits inner join Stc_Items on Stc_Items.ItemID=Stc_ItemUnits.ItemID and Stc_Items.BranchID=Stc_ItemUnits.BranchID left outer join  Stc_SizingUnits on Stc_ItemUnits.SizeID=Stc_SizingUnits.SizeID and Stc_ItemUnits.BranchID=Stc_SizingUnits.BranchID Where UnitCancel=0 And LOWER (Stc_SizingUnits." + PrimaryName + ")=LOWER ('" + val.ToString() + "') and  Stc_Items.ItemID=" + Comon.cLong(GridViewBeforPrentag.GetRowCellValue(GridViewBeforPrentag.FocusedRowHandle, "ItemID").ToString()) + "  And Stc_ItemUnits.FacilityID=" + UserInfo.FacilityID;
                    DataTable dtBarCode = Lip.SelectRecord(Str);
                    if (dtBarCode.Rows.Count > 0)
                    {
                        GridViewBeforPrentag.SetFocusedRowCellValue("SizeID", dtBarCode.Rows[0]["SizeID"]);
                        frmCadFactory.SetValuseWhenChangeSizeName(GridViewBeforPrentag, Comon.cLong(GridViewBeforPrentag.GetRowCellValue(GridViewBeforPrentag.FocusedRowHandle, "ItemID").ToString()), Comon.cInt(dtBarCode.Rows[0]["SizeID"]), "Menu_FactoryRunCommandPrentagAndPulishn", "Menu_FactoryRunCommandMaster", Comon.cDbl(txtStoreID.Text), Comon.cInt(txtCommandID.Text), "ComandID", Where: " and Menu_FactoryRunCommandPrentagAndPulishn.TypeOpration=1 and Menu_FactoryRunCommandPrentagAndPulishn.PrntageTypeID=" + Comon.cInt(cmbPrntageTypeID.EditValue), FildNameQTY: "PrentagDebit", FildNameBarCode: "BarcodePrentag");
                        e.Valid = true;
                        view.SetColumnError(GridViewBeforPrentag.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundSizeForItem;
                        view.SetColumnError(GridViewBeforPrentag.Columns[ColName], Messages.msgNoFoundSizeForItem);
                    }
                }
                if (ColName == "EmpName")
                {
                    DataTable dtMachinID = Lip.SelectRecord("Select  EmployeeID  from HR_EmployeeFile Where  LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') and BranchID=" + MySession.GlobalBranchID);
                    if (dtMachinID.Rows.Count > 0)
                    {
                        GridViewBeforPrentag.SetFocusedRowCellValue("EmpID", dtMachinID.Rows[0]["EmployeeID"].ToString());
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
                    DataTable dtItemID = Lip.SelectRecord("Select StoreID from Stc_Stores Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') and BranchID=" + MySession.GlobalBranchID+" And FacilityID=" + UserInfo.FacilityID);
                    if (dtItemID.Rows.Count > 0)
                    {
                        GridViewBeforPrentag.SetFocusedRowCellValue("StoreID", dtItemID.Rows[0]["StoreID"]);
                        e.Valid = true;
                        view.SetColumnError(GridViewBeforPrentag.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(GridViewBeforPrentag.Columns[ColName], Messages.msgNoFoundThisItem);
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
        public void ReadRecord(int ComandID, bool flag = false)
        {
            try
            {
                ClearFields();

                DataRecord = Menu_FactoryRunCommandMasterDAL.frmGetDataDetalByID(ComandID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID, Comon.cInt(cmbTypeStage.EditValue));

                if (DataRecord != null && DataRecord.Rows.Count > 0)
                {

                    DataRecordPolushin = Menu_FactoryRunCommandPrentagAndPulishnDAL.frmGetDataDetalByIDPrntageTypeID(Comon.cLong(ComandID), UserInfo.BRANCHID, UserInfo.FacilityID, 1, Comon.cInt(cmbPrntageTypeID.EditValue));
                    DataRecordAfterBrntag = Menu_FactoryRunCommandPrentagAndPulishnDAL.frmGetDataDetalByIDPrntageTypeID(Comon.cLong(ComandID), UserInfo.BRANCHID, UserInfo.FacilityID, 2, Comon.cInt(cmbPrntageTypeID.EditValue));

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
                            gridControlBeforPrentag.DataSource = DataRecordPolushin;
                            lstDetailfactory.AllowNew = true;
                            lstDetailfactory.AllowEdit = true;
                            lstDetailfactory.AllowRemove = true;
                            GridViewBeforPrentag.RefreshData();
                        }
                    if (DataRecordAfterBrntag != null)
                        if (DataRecordAfterBrntag.Rows.Count > 0)
                        {
                            gridControlAfterPrentage.DataSource = DataRecordAfterBrntag;
                            lstDetailAfterfactory.AllowNew = true;
                            lstDetailAfterfactory.AllowEdit = true;
                            lstDetailAfterfactory.AllowRemove = true;
                            GridViewAfterPrentag.RefreshData();
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

            GridViewOrderDetails.Columns["DebitTime"].Visible = false;
            GridViewOrderDetails.Columns["DebitDate"].Visible = false;

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
        void initGridBeforPrentage()
        {
            lstDetailPrentage = new BindingList<Menu_FactoryRunCommandPrentagAndPulishn>();
            lstDetailPrentage.AllowNew = true;
            lstDetailPrentage.AllowEdit = true;
            lstDetailPrentage.AllowRemove = true;
            gridControlBeforPrentag.DataSource = lstDetailPrentage;


            DataTable dtitems0 = Lip.SelectRecord("SELECT   SizeID," + PrimaryName + "   FROM Stc_SizingUnits where BranchID=" + MySession.GlobalBranchID);
            string[] NameUnit = new string[dtitems0.Rows.Count];
            for (int i = 0; i <= dtitems0.Rows.Count - 1; i++)
                NameUnit[i] = dtitems0.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems0 = new RepositoryItemComboBox();
            riComboBoxitems0.Items.AddRange(NameUnit);
            gridControlBeforPrentag.RepositoryItems.Add(riComboBoxitems0);
            GridViewBeforPrentag.Columns[SizeName].ColumnEdit = riComboBoxitems0;

            DataTable dtitems = Lip.SelectRecord("SELECT   " + PrimaryName + "   FROM Menu_FactoryMachine ");
            string[] NameMachine = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameMachine[i] = dtitems.Rows[i][PrimaryName].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameMachine);

            gridControlBeforPrentag.RepositoryItems.Add(riComboBoxitems);
            GridViewBeforPrentag.Columns["MachineName"].ColumnEdit = riComboBoxitems;

            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0 and BranchID=" + MySession.GlobalBranchID);
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();
            riComboBoxitems2.Items.AddRange(StoreName);
            gridControlBeforPrentag.RepositoryItems.Add(riComboBoxitems2);
            GridViewBeforPrentag.Columns["StoreName"].ColumnEdit = riComboBoxitems2;

            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0 and BranchID=" + MySession.GlobalBranchID);
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlBeforPrentag.RepositoryItems.Add(riComboBoxitems3);
            GridViewBeforPrentag.Columns["EmpName"].ColumnEdit = riComboBoxitems3;

            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0 and BranchID=" + MySession.GlobalBranchID);
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlBeforPrentag.RepositoryItems.Add(riComboBoxitems4);
            GridViewBeforPrentag.Columns[ItemName].ColumnEdit = riComboBoxitems4;


            GridViewBeforPrentag.Columns["PrentagDebitTime"].Visible = false;
            GridViewBeforPrentag.Columns["PrSignature"].Visible = false;

            GridViewBeforPrentag.Columns["MachinID"].Visible = false;
            GridViewBeforPrentag.Columns["MachineName"].Visible = false;

            GridViewBeforPrentag.Columns["ID"].Visible = false;
            GridViewBeforPrentag.Columns["ComandID"].Visible = false;
            GridViewBeforPrentag.Columns["BarcodePrentag"].Visible = false;
            GridViewBeforPrentag.Columns["EmpPolishnID"].Visible = false;
            GridViewBeforPrentag.Columns["EmpPrentagID"].Visible = false;
            GridViewBeforPrentag.Columns["Cancel"].Visible = false;
            GridViewBeforPrentag.Columns["BranchID"].Visible = false;
            GridViewBeforPrentag.Columns["FacilityID"].Visible = false;

            GridViewBeforPrentag.Columns["EditUserID"].Visible = false;
            GridViewBeforPrentag.Columns["EditDate"].Visible = false;
            GridViewBeforPrentag.Columns["EditTime"].Visible = false;
            GridViewBeforPrentag.Columns["RegDate"].Visible = false;
            GridViewBeforPrentag.Columns["UserID"].Visible = false;
            GridViewBeforPrentag.Columns["SizeID"].Visible = false;
            GridViewBeforPrentag.Columns["ComputerInfo"].Visible = false;
            GridViewBeforPrentag.Columns["EditComputerInfo"].Visible = false;
            GridViewBeforPrentag.Columns["RegTime"].Visible = false;

            GridViewBeforPrentag.Columns["PrentagCredit"].Visible = false;
            GridViewBeforPrentag.Columns["TypeOpration"].Visible = false;
            //GridViewBeforPrentag.Columns["SizeID"].Visible = false;
            GridViewBeforPrentag.Columns["CostPrice"].Visible = false;

            // GridViewBeforPrentag.Columns["PrentagDebitTime"].Visible = false;
            GridViewBeforPrentag.Columns["MachinID"].Name = "MachinID";
            //dtItem.Columns["Losed"].ReadOnly = true;


            GridViewBeforPrentag.Columns["EmpName"].Width = 150;
            GridViewBeforPrentag.Columns["EmpID"].Width = 120;
            GridViewBeforPrentag.Columns["StoreName"].Width = 100;
            GridViewBeforPrentag.Columns["PrSignature"].Width = 85;
            GridViewBeforPrentag.Columns["PrentagDebitDate"].Width = 110;
            GridViewBeforPrentag.Columns["PrentagDebitTime"].Width = 85;
            GridViewBeforPrentag.Columns["EmpID"].Visible = false;
            GridViewBeforPrentag.Columns["StoreName"].Visible = false;
            GridViewBeforPrentag.Columns["EmpName"].Visible = false;
            GridViewBeforPrentag.Columns["StoreID"].Visible = false;
            GridViewBeforPrentag.Columns["ShownInNext"].Visible = false;
            if (UserInfo.Language == iLanguage.Arabic)
            {

                GridViewBeforPrentag.Columns["EngItemName"].Visible = false;
                GridViewBeforPrentag.Columns["EngSizeName"].Visible = false;
                GridViewBeforPrentag.Columns["ArbItemName"].Width = 150;

                GridViewBeforPrentag.Columns["StoreID"].Caption = "رقم المخزن";
                GridViewBeforPrentag.Columns["StoreName"].Caption = "إسم المخزن";

                GridViewBeforPrentag.Columns["EmpID"].Caption = "رقم العامل";
                GridViewBeforPrentag.Columns["EmpName"].Caption = "إسم العامل";

                GridViewBeforPrentag.Columns["MachinID"].Caption = "رقم المكينة";
                GridViewBeforPrentag.Columns["MachineName"].Caption = "إسم المكينة";
                GridViewBeforPrentag.Columns["PrentagDebit"].Caption = "الوزن";

                GridViewBeforPrentag.Columns["PrentagCredit"].Caption = "دائــن";
                GridViewBeforPrentag.Columns["TypeOpration"].Caption = "نوع العملية";
                GridViewBeforPrentag.Columns["PrSignature"].Caption = "التوقيع";

                GridViewBeforPrentag.Columns["ItemID"].Caption = "رقم الصنف";
                GridViewBeforPrentag.Columns["ArbItemName"].Caption = "اسم الصنف";
                GridViewBeforPrentag.Columns["SizeID"].Caption = "رقم الوحده";
                GridViewBeforPrentag.Columns["ArbSizeName"].Caption = "الوحده";
                GridViewBeforPrentag.Columns["CostPrice"].Caption = "التكلفة";
                GridViewBeforPrentag.Columns["PrentagDebitDate"].Caption = "التاريخ";
                GridViewBeforPrentag.Columns["PrentagDebitTime"].Caption = "الوقت";
            }
            else
            {
                GridViewBeforPrentag.Columns["ArbItemName"].Visible = false;
                GridViewBeforPrentag.Columns["ArbSizeName"].Visible = false;
                GridViewBeforPrentag.Columns["EngItemName"].Width = 150;
                GridViewBeforPrentag.Columns["StoreID"].Caption = "Store ID";
                GridViewBeforPrentag.Columns["StoreName"].Caption = "Store Name";
                GridViewBeforPrentag.Columns["EngItemName"].Caption = "Item Name";
                GridViewBeforPrentag.Columns["MachinID"].Caption = "Machine ID";
                GridViewBeforPrentag.Columns["MachineName"].Caption = "Machin Name";
                GridViewBeforPrentag.Columns["PrentagDebit"].Caption = "debtor ";
                GridViewBeforPrentag.Columns["EngSizeName"].Caption = "Unit";
                GridViewBeforPrentag.Columns["PrentagCredit"].Caption = "PrentagCreditor";
                GridViewBeforPrentag.Columns["TypeOpration"].Caption = "Type Opration";
                GridViewBeforPrentag.Columns["PrSignature"].Caption = "Signature";
                GridViewBeforPrentag.Columns["PrentagDebitDate"].Caption = "Date";
                GridViewBeforPrentag.Columns["PrentagDebitTime"].Caption = "Time";
                GridViewBeforPrentag.Columns["EmpID"].Caption = "EmpID";
                GridViewBeforPrentag.Columns["EmpName"].Caption = "Name";
            }
            //GridViewBeforPrentag.Columns["MachinID"].OptionsColumn.AllowFocus = false;
            //GridViewBeforPrentag.Columns["MachinID"].OptionsColumn.AllowEdit = false;

            //GridViewBeforPrentag.Columns["MachineName"].OptionsColumn.AllowFocus = false;
            //GridViewBeforPrentag.Columns["MachineName"].OptionsColumn.AllowEdit = false;



        }
        void initGridAfterPrentage()
        {

            lstDetailAfterPrentage = new BindingList<Menu_FactoryRunCommandPrentagAndPulishn>();
            lstDetailAfterPrentage.AllowNew = true;
            lstDetailAfterPrentage.AllowEdit = true;
            lstDetailAfterPrentage.AllowRemove = true;
            gridControlAfterPrentage.DataSource = lstDetailAfterPrentage;

            DataTable dtitems0 = Lip.SelectRecord("SELECT   SizeID," + PrimaryName + "   FROM Stc_SizingUnits where BranchID=" + MySession.GlobalBranchID);
            string[] NameUnit = new string[dtitems0.Rows.Count];
            for (int i = 0; i <= dtitems0.Rows.Count - 1; i++)
                NameUnit[i] = dtitems0.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems0 = new RepositoryItemComboBox();
            riComboBoxitems0.Items.AddRange(NameUnit);
            gridControlAfterPrentage.RepositoryItems.Add(riComboBoxitems0);
            GridViewAfterPrentag.Columns[SizeName].ColumnEdit = riComboBoxitems0;

            DataTable dtitems = Lip.SelectRecord("SELECT   " + PrimaryName + "   FROM Menu_FactoryMachine ");
            string[] NameMachine = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameMachine[i] = dtitems.Rows[i][PrimaryName].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameMachine);

            gridControlAfterPrentage.RepositoryItems.Add(riComboBoxitems);


            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0  and BranchID=" + MySession.GlobalBranchID);
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();

            riComboBoxitems2.Items.AddRange(StoreName);
            gridControlAfterPrentage.RepositoryItems.Add(riComboBoxitems2);
            GridViewAfterPrentag.Columns["StoreName"].ColumnEdit = riComboBoxitems2;

            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0  and BranchID=" + MySession.GlobalBranchID);
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlAfterPrentage.RepositoryItems.Add(riComboBoxitems3);
            GridViewAfterPrentag.Columns["EmpName"].ColumnEdit = riComboBoxitems3;

            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0 and BranchID=" + MySession.GlobalBranchID);
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlAfterPrentage.RepositoryItems.Add(riComboBoxitems4);
            GridViewAfterPrentag.Columns[ItemName].ColumnEdit = riComboBoxitems4;



            GridViewAfterPrentag.Columns["MachineName"].ColumnEdit = riComboBoxitems;
            GridViewAfterPrentag.Columns["ID"].Visible = false;
            GridViewAfterPrentag.Columns["ComandID"].Visible = false;
            GridViewAfterPrentag.Columns["BarcodePrentag"].Visible = false;
            GridViewAfterPrentag.Columns["EmpPolishnID"].Visible = false;
            GridViewAfterPrentag.Columns["EmpPrentagID"].Visible = false;
            GridViewAfterPrentag.Columns["Cancel"].Visible = false;
            GridViewAfterPrentag.Columns["BranchID"].Visible = false;
            GridViewAfterPrentag.Columns["FacilityID"].Visible = false;

            GridViewAfterPrentag.Columns["PrentagDebitTime"].Visible = false;
            GridViewAfterPrentag.Columns["PrSignature"].Visible = false;
            GridViewAfterPrentag.Columns["MachinID"].Visible = false;
            GridViewAfterPrentag.Columns["MachineName"].Visible = false;

            GridViewAfterPrentag.Columns["EditUserID"].Visible = false;
            GridViewAfterPrentag.Columns["EditDate"].Visible = false;
            GridViewAfterPrentag.Columns["EditTime"].Visible = false;
            GridViewAfterPrentag.Columns["RegDate"].Visible = false;
            GridViewAfterPrentag.Columns["UserID"].Visible = false;

            GridViewAfterPrentag.Columns["ComputerInfo"].Visible = false;
            GridViewAfterPrentag.Columns["EditComputerInfo"].Visible = false;
            GridViewAfterPrentag.Columns["RegTime"].Visible = false;
            GridViewAfterPrentag.Columns["SizeID"].Visible = false;
            GridViewAfterPrentag.Columns["PrentagDebit"].Visible = false;
            GridViewAfterPrentag.Columns["TypeOpration"].Visible = false;
            //GridViewAfterPrentag.Columns["SizeID"].Visible = false;
            GridViewAfterPrentag.Columns["CostPrice"].Visible = false;

            // GridViewAfterPrentag.Columns["PrentagDebitTime"].Visible = false;
            GridViewAfterPrentag.Columns["MachinID"].Name = "MachinID";
            //dtItem.Columns["Losed"].ReadOnly = true;
            GridViewAfterPrentag.Columns["EmpName"].Width = 150;
            GridViewAfterPrentag.Columns["EmpID"].Width = 120;
            GridViewAfterPrentag.Columns["StoreName"].Width = 100;
            GridViewAfterPrentag.Columns["PrSignature"].Width = 85;
            GridViewAfterPrentag.Columns["PrentagDebitDate"].Width = 110;
            GridViewAfterPrentag.Columns["PrentagDebitTime"].Width = 85;
            GridViewAfterPrentag.Columns["EmpID"].Visible = false;
            GridViewAfterPrentag.Columns["StoreName"].Visible = false;
            GridViewAfterPrentag.Columns["EmpName"].Visible = false;
            GridViewAfterPrentag.Columns["StoreID"].Visible = false;
            if (UserInfo.Language == iLanguage.Arabic)
            {

                GridViewAfterPrentag.Columns["EngItemName"].Visible = false;
                GridViewAfterPrentag.Columns["EngSizeName"].Visible = false;
                GridViewAfterPrentag.Columns["ArbItemName"].Width = 150;
                GridViewAfterPrentag.Columns["StoreID"].Caption = "رقم المخزن";
                GridViewAfterPrentag.Columns["StoreName"].Caption = "إسم المخزن";

                GridViewAfterPrentag.Columns["EmpID"].Caption = "رقم العامل";
                GridViewAfterPrentag.Columns["EmpName"].Caption = "إسم العامل";

                GridViewAfterPrentag.Columns["MachinID"].Caption = "رقم المكينة";
                GridViewAfterPrentag.Columns["MachineName"].Caption = "إسم المكينة";
                GridViewAfterPrentag.Columns["PrentagDebit"].Caption = "Debit";

                GridViewAfterPrentag.Columns["PrentagCredit"].Caption = "الوزن";
                GridViewAfterPrentag.Columns["TypeOpration"].Caption = "نوع العملية ";
                GridViewAfterPrentag.Columns["PrSignature"].Caption = "التوقيع";
                GridViewAfterPrentag.Columns["ItemID"].Caption = "رقم الصنف";
                GridViewAfterPrentag.Columns["ArbItemName"].Caption = "اسم الصنف";
                GridViewAfterPrentag.Columns["SizeID"].Caption = "رقم الوحده";
                GridViewAfterPrentag.Columns["ArbSizeName"].Caption = "الوحده";
                GridViewAfterPrentag.Columns["CostPrice"].Caption = "التكلفة";
                GridViewAfterPrentag.Columns["PrentagDebitDate"].Caption = "التاريخ";
                GridViewAfterPrentag.Columns["PrentagDebitTime"].Caption = "الوقت";
                GridViewAfterPrentag.Columns["ShownInNext"].Caption = "يظهر في التفاصيل";
            }
            else
            {
                GridViewAfterPrentag.Columns["ArbItemName"].Visible = false;
                GridViewAfterPrentag.Columns["ArbSizeName"].Visible = false;
                GridViewAfterPrentag.Columns["EngItemName"].Width = 150;
                GridViewAfterPrentag.Columns["StoreID"].Caption = "Store ID";
                GridViewAfterPrentag.Columns["StoreName"].Caption = "Store Name";
                GridViewAfterPrentag.Columns["EngItemName"].Caption = "Item Name";
                GridViewAfterPrentag.Columns["MachinID"].Caption = "Machine ID";
                GridViewAfterPrentag.Columns["MachineName"].Caption = "Machin Name";
                GridViewAfterPrentag.Columns["PrentagDebit"].Caption = "debtor ";
                GridViewAfterPrentag.Columns["EngSizeName"].Caption = "Unit";
                GridViewAfterPrentag.Columns["PrentagCredit"].Caption = "PrentagCreditor";
                GridViewAfterPrentag.Columns["TypeOpration"].Caption = "Type Opration";
                GridViewAfterPrentag.Columns["PrSignature"].Caption = "Signature";
                GridViewAfterPrentag.Columns["PrentagDebitDate"].Caption = "Date";
                GridViewAfterPrentag.Columns["PrentagDebitTime"].Caption = "Time";
                GridViewAfterPrentag.Columns["EmpID"].Caption = "EmpID";
                GridViewAfterPrentag.Columns["EmpName"].Caption = "Name";
                GridViewAfterPrentag.Columns["ShownInNext"].Caption = "Shown In Next";
            }
          


        }



        #endregion
        private void frmManufacturingOrder_Load(object sender, EventArgs e)
        {
            try
            {                 
                            
               
                if (Comon.cInt(cmbPrntageTypeID.EditValue) == 1)
                    cmbTypeStage.EditValue = 7;
                else if (Comon.cInt(cmbPrntageTypeID.EditValue) == 2)
                    cmbTypeStage.EditValue = 12;
                this.Text = cmbPrntageTypeID.Text.ToString();
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
                strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + txtEmpID.Text + " And Cancel =0  and BranchID=" + MySession.GlobalBranchID;
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
                strSQL = "SELECT "+PrimaryName+ " as EmployeeName FROM Manu_TypeOrders WHERE  ID =" + txtTypeOrder.Text  ;
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
                    else if (IsNewRecord == false && CommandIDTemp > 0&& CommandIDThis!=Comon.cInt(txtCommandID.Text))
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
                    if ((IsNewRecord)) //&& CommandIDTemp <= 0
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
        private void txtEmployeeStokID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtEmployeeStokID.Text) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID;
                CSearch.ControlValidating(txtEmployeeStokID, txtEmployeeStokName, strSQL); 
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
            else if (FocusedControl.Trim() == gridControlBeforPrentag.Name)
            {
                
                if (GridViewBeforPrentag.FocusedColumn.Name == "colItemID" || GridViewBeforPrentag.FocusedColumn.Name == "col" + ItemName || GridViewBeforPrentag.FocusedColumn.Name == "colBarCode")
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
                            GridViewBeforPrentag.Columns[ItemName].ColumnEdit = rItem;
                            gridControlBeforPrentag.RepositoryItems.Add(rItem);
                           
                        };
                    }
                    else
                        frm.Dispose();
                }
                else if (GridViewBeforPrentag.FocusedColumn.Name == "colSizeName" || GridViewBeforPrentag.FocusedColumn.Name == "colSizeID")
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


            else if (FocusedControl.Trim() == gridControlAfterPrentage.Name)
            {
                
                if (GridViewAfterPrentag.FocusedColumn.Name == "colItemID" || GridViewAfterPrentag.FocusedColumn.Name == "col" + ItemName || GridViewAfterPrentag.FocusedColumn.Name == "colBarCode")
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
                            GridViewAfterPrentag.Columns[ItemName].ColumnEdit = rItem;
                            gridControlAfterPrentage.RepositoryItems.Add(rItem);

                        };
                    }
                    else
                        frm.Dispose();
                }
                else if (GridViewAfterPrentag.FocusedColumn.Name == "colSizeName" || GridViewAfterPrentag.FocusedColumn.Name == "colSizeID")
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
                for (int i = 0; i <= GridViewBeforPrentag.DataRowCount - 1; i++)
                {
                 
                    //ToatlBeforFactoryAmount += Comon.cDec(GridViewBeforPrentag.GetRowCellValue(i, "CostPrice").ToString());
                    if (Comon.cInt(GridViewBeforPrentag.GetRowCellValue(i, "SizeID").ToString()) == 2)
                        TempQTY += Comon.cDec(Comon.cDec(GridViewBeforPrentag.GetRowCellValue(i, "PrentagDebit").ToString()) / 5);
                    else
                        TempQTY += Comon.cDec(Comon.cDec(GridViewBeforPrentag.GetRowCellValue(i, "PrentagDebit").ToString()));
                    //ToatlBeforFactoryQty += Comon.cDec(GridViewBeforPrentag.GetRowCellValue(i, "PrentagDebit").ToString());
                }

                for (int i = 0; i <= GridViewAfterPrentag.DataRowCount - 1; i++)
                {
                    if (Comon.cInt(GridViewAfterPrentag.GetRowCellValue(i, "SizeID").ToString()) == 2)
                        TempQTYAfter += Comon.cDec(Comon.cDec(GridViewAfterPrentag.GetRowCellValue(i, "PrentagCredit").ToString()) / 5);
                    else
                        TempQTYAfter += Comon.cDec(Comon.cDec(GridViewAfterPrentag.GetRowCellValue(i, "PrentagCredit").ToString()));
                    //ToatlAfterFactoryQty += Comon.cDec(GridViewAfterPrentag.GetRowCellValue(i, "PrentagCredit").ToString());
                    //ToatlAfterFactoryAmount += Comon.cDec(GridViewAfterPrentag.GetRowCellValue(i, "CostPrice").ToString());
                }
                txtTotalBefor.Text = TempQTY.ToString();
                txtTotalAfter.Text = TempQTYAfter.ToString();
                txtTotalAmountBefor.Text = ToatlBeforFactoryAmount.ToString();
                txtTotalAmountAfter.Text = ToatlAfterFactoryAmount.ToString();

                lblTotallostFactory.Text = Comon.cDec(TempQTY-TempQTYAfter ) + "";
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
                if (!MySession.GlobalAllowChangefrmPrntageStoreID&&  (Comon.cInt(cmbPrntageTypeID.EditValue) == 1)) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (!MySession.GlobalAllowChangefrmPrntage2StoreID && (Comon.cInt(cmbPrntageTypeID.EditValue) == 2)) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
              
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
                if (!MySession.GlobalAllowChangefrmPrntageAccountID &&   (Comon.cInt(cmbPrntageTypeID.EditValue) == 1)) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (!MySession.GlobalAllowChangefrmPrntage2AccountID && (Comon.cInt(cmbPrntageTypeID.EditValue) == 2)) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
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
                        PrepareSearchQuery.Find(ref cls, txtOrderID, txtOrderID, "OrderID", "Order ID", Comon.cInt(cmbBranchesID.EditValue), "  and OrderID not in(select Barcode as OrderID  from Menu_FactoryRunCommandMaster where Cancel=0 and BranchID=" + MySession.GlobalBranchID+" and TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + ") ");
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
                if (!MySession.GlobalAllowChangefrmPrntageEmployeeID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtEmpID, lblEmpName, "EmployeeID", "رقـم العامل", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtEmpID, lblEmpName, "EmployeeID", "Worker ID", Comon.cInt(cmbBranchesID.EditValue));
            }
            
             
            //امين المخزن
            else if (FocusedControl.Trim() == txtEmployeeStokID.Name)
            {
                if (!MySession.GlobalAllowChangefrmPrntageEmployeeID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtEmployeeStokID, txtEmployeeStokName, "EmployeeID", "رقـم العامل", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtEmployeeStokID, txtEmployeeStokName, "EmployeeID", "Worker ID", Comon.cInt(cmbBranchesID.EditValue));
            }
             
 
            //الجرايد فيو
            
            else if (FocusedControl.Trim() == gridControlBeforPrentag.Name)
            {
                if (GridViewBeforPrentag.FocusedColumn.Name == "colBarcodePrentag" || GridViewBeforPrentag.FocusedColumn.Name == "colItemName" || GridViewBeforPrentag.FocusedColumn.Name == "colItemID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "BarCode", MySession.GlobalBranchID);
                }
                if (GridViewBeforPrentag.FocusedColumn.Name == "colStoreID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "رقم الـمـســتـودع", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "Store ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                
                if (GridViewBeforPrentag.FocusedColumn.Name == "MachinID")
                {
                    if (GridViewBeforPrentag.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "رقم المكينة", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "Machine ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                else if (GridViewBeforPrentag.FocusedColumn.Name == "colSizeID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "رقـم الـوحـــده", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "Size ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                else if (GridViewBeforPrentag.FocusedColumn.Name == "colEmpID")
                {
                    if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "رقـم العامل", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "Worker ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                else if (GridViewBeforPrentag.FocusedColumn.Name == "colPrentagDebit")
                {
                    frmRemindQtyItem frm = new frmRemindQtyItem();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);

                        frm.Show();
                        if (GridViewBeforPrentag.GetRowCellValue(GridViewBeforPrentag.FocusedRowHandle, "ItemID") != null)
                             frm.SetValueToControl(GridViewBeforPrentag.GetRowCellValue(GridViewBeforPrentag.FocusedRowHandle, "ItemID").ToString(), txtStoreID.Text.ToString());
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
            else if (FocusedControl.Trim() == gridControlAfterPrentage.Name)
            {
                if (GridViewAfterPrentag.FocusedColumn.Name == "colBarcodePrentag" || GridViewAfterPrentag.FocusedColumn.Name == "colItemName" || GridViewAfterPrentag.FocusedColumn.Name == "colItemID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "BarCode", MySession.GlobalBranchID);
                }
                if (GridViewAfterPrentag.FocusedColumn.Name == "colStoreID")
                {

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "رقم الـمـســتـودع", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "Store ID", Comon.cInt(cmbBranchesID.EditValue));
                }
            
                if (GridViewAfterPrentag.FocusedColumn.Name == "MachinID")
                {
                    if (GridViewAfterPrentag.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "رقم المكينة", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "Machine ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                else if (GridViewAfterPrentag.FocusedColumn.Name == "colSizeID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "رقـم الـوحـــده", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "Size ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                else if (GridViewAfterPrentag.FocusedColumn.Name == "colEmpID")
                {
                    if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "رقـم العامل", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "Worker ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                else if (GridViewAfterPrentag.FocusedColumn.Name == "colPrentagCredit")
                {
                    frmRemindQtyItem frm = new frmRemindQtyItem();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);

                        frm.Show();
                        if (GridViewAfterPrentag.GetRowCellValue(GridViewAfterPrentag.FocusedRowHandle, "ItemID") != null)
                          frm.SetValueToControl(GridViewAfterPrentag.GetRowCellValue(GridViewAfterPrentag.FocusedRowHandle, "ItemID").ToString(), txtStoreID.Text.ToString());
                        else
                        {
                            Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "ارجاء اختيار صنف ومن ثم اعادة عرض الكمية المتبقية" : "Please select an item and re-display the remaining quantity");
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
                else if (FocusedControl.Trim() == gridControlBeforPrentag.Name)
                {
                    if (GridViewBeforPrentag.FocusedColumn.Name == "colBarcodePrentag" || GridViewBeforPrentag.FocusedColumn.Name == "colItemName" || GridViewBeforPrentag.FocusedColumn.Name == "colItemID")
                    {
                        string Barcode = cls.PrimaryKeyValue.ToString();
                        if (Stc_itemsDAL.CheckIfStopItemUnit(Barcode, MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                        {
                            Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                            return;
                        }
                        GridViewBeforPrentag.AddNewRow();
                        GridViewBeforPrentag.SetRowCellValue(GridViewBeforPrentag.FocusedRowHandle, GridViewBeforPrentag.Columns["BarcodePrentag"], Barcode);
                        FillItemData(GridViewBeforPrentag, gridControlBeforPrentag, "BarcodePrentag", "PrentagDebit", Stc_itemsDAL.GetItemData1(Barcode, UserInfo.FacilityID), "PrentagDebitDate", "PrentagDebitTime", txtAccountID);
                       
                    }
                    if (GridViewBeforPrentag.FocusedColumn.Name == "colStoreID")
                    {
                        GridViewBeforPrentag.SetRowCellValue(GridViewBeforPrentag.FocusedRowHandle, GridViewBeforPrentag.Columns["StoreID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        GridViewBeforPrentag.SetRowCellValue(GridViewBeforPrentag.FocusedRowHandle, GridViewBeforPrentag.Columns["StoreName"], Lip.GetValue(strSQL));
                    }
                   
                    if (GridViewBeforPrentag.FocusedColumn.Name == "MachinID")
                    {
                        GridViewBeforPrentag.AddNewRow();
                        FileDataMachinName(GridViewBeforPrentag, "PrentagDebitDate", "PrentagDebitTime", Comon.cInt(cls.PrimaryKeyValue.ToString()));
                    }
                    if (GridViewBeforPrentag.FocusedColumn.Name == "colSizeID")
                    {
                        GridViewBeforPrentag.SetRowCellValue(GridViewBeforPrentag.FocusedRowHandle, GridViewBeforPrentag.Columns["SizeID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as " + SizeName + " FROM Stc_SizingUnits WHERE SizeID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        GridViewBeforPrentag.SetRowCellValue(GridViewBeforPrentag.FocusedRowHandle, GridViewBeforPrentag.Columns[SizeName], Lip.GetValue(strSQL));

                    }
                    if (GridViewBeforPrentag.FocusedColumn.Name == "colEmpID")
                    {
                        GridViewBeforPrentag.SetRowCellValue(GridViewBeforPrentag.FocusedRowHandle, GridViewBeforPrentag.Columns["EmpID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        GridViewBeforPrentag.SetRowCellValue(GridViewBeforPrentag.FocusedRowHandle, GridViewBeforPrentag.Columns["EmpName"], Lip.GetValue(strSQL));
                    }
                }
                else if (FocusedControl.Trim() == gridControlAfterPrentage.Name)
                {
                    if (GridViewAfterPrentag.FocusedColumn.Name == "colBarcodePrentag" || GridViewAfterPrentag.FocusedColumn.Name == "colItemName" || GridViewAfterPrentag.FocusedColumn.Name == "colItemID")
                    {
                        string Barcode = cls.PrimaryKeyValue.ToString();
                        if (Stc_itemsDAL.CheckIfStopItemUnit(Barcode, MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                        {
                            Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                            return;
                        }
                        GridViewAfterPrentag.AddNewRow();
                        GridViewAfterPrentag.SetRowCellValue(GridViewAfterPrentag.FocusedRowHandle, GridViewAfterPrentag.Columns["BarcodePrentag"], Barcode);
                         FillItemData(GridViewAfterPrentag, gridControlAfterPrentage, "BarcodePrentag", "PrentagCredit", Stc_itemsDAL.GetItemData1(Barcode.ToString(), UserInfo.FacilityID), "PrentagDebitDate", "PrentagDebitTime", txtAccountID);
                        
                    }
                    if (GridViewAfterPrentag.FocusedColumn.Name == "colStoreID")
                    {
                        GridViewAfterPrentag.SetRowCellValue(GridViewAfterPrentag.FocusedRowHandle, GridViewAfterPrentag.Columns["StoreID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        GridViewAfterPrentag.SetRowCellValue(GridViewAfterPrentag.FocusedRowHandle, GridViewAfterPrentag.Columns["StoreName"], Lip.GetValue(strSQL));

                    }

                  
                    if (GridViewAfterPrentag.FocusedColumn.Name == "MachinID")
                    {
                        GridViewAfterPrentag.AddNewRow();
                        FileDataMachinName(GridViewAfterPrentag, "PrentagDebitDate", "PrentagDebitTime", Comon.cInt(cls.PrimaryKeyValue.ToString()));
                    }
                    if (GridViewAfterPrentag.FocusedColumn.Name == "colSizeID")
                    {
                        GridViewAfterPrentag.SetRowCellValue(GridViewAfterPrentag.FocusedRowHandle, GridViewAfterPrentag.Columns["SizeID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as " + SizeName + " FROM Stc_SizingUnits WHERE SizeID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        GridViewAfterPrentag.SetRowCellValue(GridViewAfterPrentag.FocusedRowHandle, GridViewAfterPrentag.Columns[SizeName], Lip.GetValue(strSQL));

                    }
                    if (GridViewAfterPrentag.FocusedColumn.Name == "colEmpID")
                    {
                        GridViewAfterPrentag.SetRowCellValue(GridViewAfterPrentag.FocusedRowHandle, GridViewAfterPrentag.Columns["EmpID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        GridViewAfterPrentag.SetRowCellValue(GridViewAfterPrentag.FocusedRowHandle, GridViewAfterPrentag.Columns["EmpName"], Lip.GetValue(strSQL));
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

            EnableGridView(GridViewBeforPrentag, Value, 1);
            EnableGridView(GridViewAfterPrentag, Value, 1);

            txtCustomerID.ReadOnly = Value ^ true;
            txtDelegateID.ReadOnly = Value ^ true;
            txtOrderDate.ReadOnly = Value ^ true;
            txtTypeOrder.ReadOnly = Value ^ true;
            txtGuidanceID.ReadOnly = Value ^ true;
            txtOrderDate.ReadOnly = Value ^ true;
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
                    strSQL = "SELECT TOP 1 * FROM " + Menu_FactoryRunCommandMasterDAL.TableName + " Where  TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + " and   Cancel =0 ";
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
             for (int i = 0; i <= GridViewBeforPrentag.DataRowCount - 1; i++)
             {
                 returned = new Manu_AllOrdersDetails();
                 returned.ID = i + 1;
                 returned.CommandID = Comon.cInt(txtCommandID.Text);
                 returned.FacilityID = UserInfo.FacilityID;
                 returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                 returned.BarCode = GridViewBeforPrentag.GetRowCellValue(i, "BarcodePrentag").ToString();
                 returned.ItemID = Comon.cInt(GridViewBeforPrentag.GetRowCellValue(i, "ItemID").ToString());
                 returned.TypeStageID = Comon.cInt(cmbTypeStage.EditValue);
                 returned.SizeID = Comon.cInt(GridViewBeforPrentag.GetRowCellValue(i, "SizeID").ToString());
                 returned.ArbSizeName = GridViewBeforPrentag.GetRowCellValue(i, SizeName).ToString();
                 returned.EngSizeName = GridViewBeforPrentag.GetRowCellValue(i, SizeName).ToString();
                 returned.ArbItemName = GridViewBeforPrentag.GetRowCellValue(i, ItemName).ToString();
                 returned.EngItemName = GridViewBeforPrentag.GetRowCellValue(i, ItemName).ToString();
                 returned.QTY = Comon.ConvertToDecimalQty(GridViewBeforPrentag.GetRowCellValue(i, "PrentagDebit").ToString());
                 returned.CostPrice = 0;
                 returned.TotalCost = 0;
                 listreturned.Add(returned);
             }
             int LengBefore = GridViewAfterPrentag.DataRowCount + 1;
             for (int i = 0; i <= GridViewAfterPrentag.DataRowCount - 1; i++)
             {
                 returned = new Manu_AllOrdersDetails();
                 returned.ID = LengBefore;
                 returned.CommandID = Comon.cInt(txtCommandID.Text);
                 returned.FacilityID = UserInfo.FacilityID;
                 returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                 returned.BarCode = GridViewAfterPrentag.GetRowCellValue(i, "BarcodePrentag").ToString();
                 returned.ItemID = Comon.cInt(GridViewAfterPrentag.GetRowCellValue(i, "ItemID").ToString());
                 returned.TypeStageID = Comon.cInt(cmbTypeStage.EditValue);
                 returned.SizeID = Comon.cInt(GridViewAfterPrentag.GetRowCellValue(i, "SizeID").ToString());
                 returned.ArbSizeName = GridViewAfterPrentag.GetRowCellValue(i, SizeName).ToString();
                 returned.EngSizeName = GridViewAfterPrentag.GetRowCellValue(i, SizeName).ToString();
                 returned.ArbItemName = GridViewAfterPrentag.GetRowCellValue(i, ItemName).ToString();
                 returned.EngItemName = GridViewAfterPrentag.GetRowCellValue(i, ItemName).ToString();
                 returned.ShownInNext = Comon.cbool(GridViewAfterPrentag.GetRowCellValue(i, "ShownInNext").ToString());
                 returned.QTY = Comon.ConvertToDecimalQty(GridViewAfterPrentag.GetRowCellValue(i, "PrentagCredit").ToString());
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
                GridViewBeforPrentag.MoveLast();
                GridViewAfterPrentag.MoveLast();

                Menu_FactoryRunCommandMaster objRecord = new Menu_FactoryRunCommandMaster();
                objRecord.Barcode = txtOrderID.Text.ToString();
                objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                objRecord.BrandID = Comon.cInt(cmbBranchesID.EditValue);
                objRecord.Cancel = 0;

                objRecord.PrntageTypeID = Comon.cInt(cmbPrntageTypeID.EditValue);
                objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue);
                objRecord.CurrencyName = cmbCurency.Text.ToString();
                objRecord.Posted = Comon.cInt(cmbStatus.EditValue);
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
             
                objRecord.TypeStageID = Comon.cInt(cmbTypeStage.EditValue);
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


                #region Save Prentag
                Menu_FactoryRunCommandPrentagAndPulishn returned;
                List<Menu_FactoryRunCommandPrentagAndPulishn> listreturned = new List<Menu_FactoryRunCommandPrentagAndPulishn>();
                int lengthPrentage = GridViewBeforPrentag.DataRowCount;
                int lengthAfterPrentage = GridViewAfterPrentag.DataRowCount;
                if (lengthPrentage > 0)
                {
                    for (int i = 0; i < lengthPrentage; i++)
                    {

                        returned = new Menu_FactoryRunCommandPrentagAndPulishn();
                        returned.ID = i + 1;
                        returned.ComandID = Comon.cInt(txtCommandID.Text.ToString());
                        returned.PrentagCredit = 0;
                        returned.MachinID = Comon.cInt(GridViewBeforPrentag.GetRowCellValue(i, "MachinID").ToString());
                        returned.MachineName = GridViewBeforPrentag.GetRowCellValue(i, "MachineName").ToString();
                        returned.BarcodePrentag = GridViewBeforPrentag.GetRowCellValue(i, "BarcodePrentag").ToString();
                        //====حقول مضافة
                        returned.StoreID = Comon.cInt(txtStoreID.Text.ToString());
                        returned.StoreName = lblStoreName.Text.ToString();
                        returned.EmpID = txtEmpID.Text.ToString();
                        returned.EmpName = lblEmpName.Text.ToString();
                        returned.ItemID = Comon.cInt(GridViewBeforPrentag.GetRowCellValue(i, "ItemID").ToString());
                        returned.ArbItemName = GridViewBeforPrentag.GetRowCellValue(i, ItemName).ToString();
                        returned.EngItemName = GridViewBeforPrentag.GetRowCellValue(i, ItemName).ToString();
                        returned.SizeID = Comon.cInt(GridViewBeforPrentag.GetRowCellValue(i, "SizeID").ToString());
                        returned.ArbSizeName = GridViewBeforPrentag.GetRowCellValue(i, SizeName).ToString();
                        returned.EngSizeName = GridViewBeforPrentag.GetRowCellValue(i, SizeName).ToString();
                        returned.PrentagDebitTime = "0";
                        returned.PrentagDebitDate = Comon.cDate(GridViewBeforPrentag.GetRowCellValue(i, "PrentagDebitDate").ToString());
                        //====
                        returned.PrentagDebit = Comon.cDbl(GridViewBeforPrentag.GetRowCellValue(i, "PrentagDebit").ToString());
                        returned.TypeOpration = 1;
                        returned.PrSignature = "";
                        returned.EmpPrentagID = Comon.cDbl(txtEmpID.Text);
                        returned.BranchID = UserInfo.BRANCHID;
                        returned.Cancel = 0;
                        returned.UserID = UserInfo.ID;
                        returned.RegDate = Comon.ConvertDateToSerial(Lip.GetServerDate());
                        returned.RegTime = Comon.cDbl(Lip.GetServerTimeSerial());

                        returned.ComputerInfo = UserInfo.ComputerInfo;
                        if (IsNewRecord == false)
                        {

                            returned.EditUserID = UserInfo.ID;
                            returned.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                            returned.EditDate = Comon.ConvertDateToSerial(Lip.GetServerDate());
                            returned.EditComputerInfo = UserInfo.ComputerInfo;
                        }
                        listreturned.Add(returned);
                    }
                }
                if (lengthAfterPrentage > 0)
                {
                    for (int i = 0; i < lengthAfterPrentage; i++)
                    {

                        returned = new Menu_FactoryRunCommandPrentagAndPulishn();
                        returned.ID = i + 1;
                        returned.ComandID = Comon.cInt(txtCommandID.Text.ToString());
                        returned.PrentagCredit = 0;
                        returned.MachinID = Comon.cInt(GridViewAfterPrentag.GetRowCellValue(i, "MachinID").ToString());
                        returned.MachineName = GridViewAfterPrentag.GetRowCellValue(i, "MachineName").ToString();

                        //====حقول مضافة
                        returned.StoreID = Comon.cInt(txtStoreID.Text.ToString());
                        returned.StoreName = lblStoreName.Text.ToString();
                        returned.EmpID = txtEmpID.Text.ToString();
                        returned.EmpName = lblEmpName.Text.ToString();
                        returned.ItemID = Comon.cInt(GridViewAfterPrentag.GetRowCellValue(i, "ItemID").ToString());
                        returned.ArbItemName = GridViewAfterPrentag.GetRowCellValue(i, ItemName).ToString();
                        returned.EngItemName = GridViewAfterPrentag.GetRowCellValue(i, ItemName).ToString();
                        returned.BarcodePrentag = GridViewAfterPrentag.GetRowCellValue(i, "BarcodePrentag").ToString();
                        returned.SizeID = Comon.cInt(GridViewAfterPrentag.GetRowCellValue(i, "SizeID").ToString());

                        returned.ShownInNext = Comon.cbool(GridViewAfterPrentag.GetRowCellValue(i, "ShownInNext").ToString());
                        returned.ArbSizeName = GridViewAfterPrentag.GetRowCellValue(i, SizeName).ToString();
                        returned.EngSizeName = GridViewAfterPrentag.GetRowCellValue(i, SizeName).ToString();
                        returned.PrentagDebitTime = "0";
                        returned.PrentagDebitDate = Comon.cDate(GridViewAfterPrentag.GetRowCellValue(i, "PrentagDebitDate").ToString());
                        //====
                        returned.PrentagCredit = Comon.cDbl(GridViewAfterPrentag.GetRowCellValue(i, "PrentagCredit").ToString());
                        returned.TypeOpration = 2;
                        returned.PrSignature = "";
                        returned.EmpPrentagID = Comon.cDbl(txtEmpID.Text);
                        returned.BranchID = UserInfo.BRANCHID;
                        returned.Cancel = 0;
                        returned.UserID = UserInfo.ID;
                        returned.RegDate = Comon.ConvertDateToSerial(Lip.GetServerDate());
                        returned.RegTime = Comon.cDbl(Lip.GetServerTimeSerial());

                        returned.ComputerInfo = UserInfo.ComputerInfo;
                        if (IsNewRecord == false)
                        {

                            returned.EditUserID = UserInfo.ID;
                            returned.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                            returned.EditDate = Comon.ConvertDateToSerial(Lip.GetServerDate());
                            returned.EditComputerInfo = UserInfo.ComputerInfo;
                        }
                        listreturned.Add(returned);
                    }

                }
                #endregion 

                if (listreturned.Count > 0)
                {
                    objRecord.Menu_F_Prentag = listreturned;

                    objRecord.Manu_OrderDetils = SaveOrderDetials();

                    string Result = Menu_FactoryRunCommandMasterDAL.InsertUsingXML(objRecord, IsNewRecord).ToString();
                    if (Comon.cInt(Result) > 0 && Comon.cInt(cmbStatus.EditValue)>1)
                    {
                        //أوامر الصرف والتوريد الخاص بالتصنيع
                        if (lengthPrentage > 0)
                        {
                             //SaveOutOnBrntage(); //حفظ   الصرف المخزني
                            // حفظ الحركة المخزنية 
                            if (Comon.cInt(Result) > 0)
                            {
                                int MoveID = SaveStockMoveingBrntageOut(Comon.cInt(Result));
                                if (MoveID == 0)
                                    Messages.MsgError(Messages.TitleInfo, "خطا في حفظ الحركة المخزنية برنتاج - قبل ");

                                //حفظ القيد الالي
                                long VoucherID = SaveVariousVoucherMachinBrntage(Comon.cInt(Result), IsNewRecord);
                                if (VoucherID == 0)
                                    Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                                else
                                    Lip.ExecututeSQL("Update " + Menu_FactoryRunCommandPrentagAndPulishnDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Menu_FactoryRunCommandPrentagAndPulishnDAL.PremaryKey + " = " + Result + " and BranchID=" + MySession.GlobalBranchID);

                            }
                          }
                        if (lengthAfterPrentage > 0)
                        {
                            //SaveInOnBrntage(); //حفظ   التوريد المخزني
                            // حفظ الحركة المخزنية 
                            if (Comon.cInt(Result) > 0)
                            {
                                bool isNew = true;
                                DataTable dtCount = null;
                                if (Comon.cInt(cmbPrntageTypeID.EditValue) == 1)
                                    dtCount = Stc_ItemsMoviingDAL.GetCountElementID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(Result), DocumentTypeBrntageAfterFrist);
                                else if (Comon.cInt(cmbPrntageTypeID.EditValue) == 2)
                                    dtCount = Stc_ItemsMoviingDAL.GetCountElementID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(Result), DocumentTypeBrntageAfterScand);
                                if (Comon.cInt(dtCount.Rows[0][0]) > 0)
                                    isNew = false;

                                int MoveID = SaveStockMoveingBrntageIn(Comon.cInt(Result));
                                if (MoveID == 0)
                                    Messages.MsgError(Messages.TitleInfo, "خطا في حفظ الحركة المخزنية برنتاج - بعد");

                                //حفظ القيد الالي
                                long VoucherID = SaveVariousVoucherMachinInOnBrntage(Comon.cInt(Result), isNew);
                                if (VoucherID == 0)
                                    Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                                else
                                    Lip.ExecututeSQL("Update " + Menu_FactoryRunCommandPrentagAndPulishnDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Menu_FactoryRunCommandPrentagAndPulishnDAL.PremaryKey + " = " + Result + " and BranchID=" + MySession.GlobalBranchID);

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
        long SaveVariousVoucherMachinBrntage(int DocumentID, bool isNew)
        {
            int VoucherID = 0;
            long Result = 0;
            Acc_VariousVoucherMachinMaster objRecord = new Acc_VariousVoucherMachinMaster();
            if(Comon.cInt(cmbPrntageTypeID.EditValue)==1)
               objRecord.DocumentType = DocumentTypeBrntageBeforeFrist;
            else if (Comon.cInt(cmbPrntageTypeID.EditValue) == 2)
                objRecord.DocumentType = DocumentTypeBrntageBeforeScand;
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " And DocumentType=" + objRecord.DocumentType + " And BranchID=" + Comon.cInt(cmbBranchesID.EditValue)));

            objRecord.VoucherID = VoucherID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = MySession.GlobalFacilityID;
            //Date

            objRecord.VoucherDate = Comon.ConvertDateToSerial(((DateTime)GridViewBeforPrentag.GetRowCellValue(GridViewBeforPrentag.DataRowCount - 1, "PrentagDebitDate")).ToString("dd/MM/yyyy")).ToString(); 
            
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
            returned.AccountID = Comon.cDbl(txtAccountID.Text);
            returned.VoucherID = VoucherID;
            double txtTotalQty_BrntageBefore = 0;
            for (int i = 0; i < GridViewBeforPrentag.DataRowCount; i++)
            {
                txtTotalQty_BrntageBefore += Comon.cDbl(GridViewBeforPrentag.GetRowCellValue(i, "PrentagDebit").ToString());
            }
            returned.DebitGold = Comon.cDbl(txtTotalQty_BrntageBefore);
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
            returned.CreditGold = Comon.cDbl(txtTotalQty_BrntageBefore);
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
       
        long SaveVariousVoucherMachinInOnBrntage(int DocumentID, bool isNew)
        {
            int VoucherID = 0;
            long Result = 0;
            Acc_VariousVoucherMachinMaster objRecord = new Acc_VariousVoucherMachinMaster();
           
            if (Comon.cInt(cmbPrntageTypeID.EditValue) == 1)
                objRecord.DocumentType = DocumentTypeBrntageAfterFrist;
            else if (Comon.cInt(cmbPrntageTypeID.EditValue) == 2)
                objRecord.DocumentType = DocumentTypeBrntageAfterScand;
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " And DocumentType=" + objRecord.DocumentType + " And BranchID=" + Comon.cInt(cmbBranchesID.EditValue)));

            objRecord.VoucherID = VoucherID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = MySession.GlobalFacilityID;
            //Date

            objRecord.VoucherDate = Comon.ConvertDateToSerial(((DateTime)GridViewAfterPrentag.GetRowCellValue(GridViewAfterPrentag.DataRowCount - 1, "PrentagDebitDate")).ToString("dd/MM/yyyy")).ToString(); 
            
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
            double txtTotalQty_BrntageAfter = 0;
            for (int i = 0; i < GridViewAfterPrentag.DataRowCount; i++)
            {
                txtTotalQty_BrntageAfter += Comon.cDbl(GridViewAfterPrentag.GetRowCellValue(i, "PrentagCredit").ToString());
            }

            returned.DebitGold = Comon.cDbl(txtTotalQty_BrntageAfter);
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
            returned.CreditGold = Comon.cDbl(txtTotalQty_BrntageAfter);
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
        //private void SaveOutOnBrntage()
        //{
        //    #region Save Out On
        //    //Save Out On
        //    bool isNew = IsNewRecord;
        //    Stc_ManuFactoryCommendOutOnBail_Master objRecordOutOnMaster = new Stc_ManuFactoryCommendOutOnBail_Master();

        //    if (IsNewRecord)
        //        objRecordOutOnMaster.InvoiceID = Comon.cInt(Stc_ManuFactoryCommendOutOnBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID, 1));
        //    else
        //    {
        //        DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeBrntageBeforeFrist);
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
        //    objRecordOutOnMaster.DocumentType = DocumentTypeBrntageBeforeFrist;
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
        //    for (int i = 0; i <= GridViewBeforPrentag.DataRowCount - 1; i++)
        //    {
        //        returnedOutOn = new Stc_ManuFactoryCommendOutOnBail_Details();
        //        returnedOutOn.InvoiceID = objRecordOutOnMaster.InvoiceID;
        //        returnedOutOn.CommandID = Comon.cInt(txtCommandID.Text);
        //        returnedOutOn.FacilityID = UserInfo.FacilityID;
        //        returnedOutOn.BranchID = Comon.cInt(cmbBranchesID.EditValue);
        //        returnedOutOn.CommandDate = Comon.cDate(GridViewBeforPrentag.GetRowCellValue(i, "PrentagDebitDate").ToString());
        //        returnedOutOn.CommandTime = "0";
        //        returnedOutOn.BarCode = GridViewBeforPrentag.GetRowCellValue(i, "BarcodePrentag").ToString();
        //        returnedOutOn.ItemID = Comon.cInt(GridViewBeforPrentag.GetRowCellValue(i, "ItemID").ToString());
        //        returnedOutOn.SizeID = Comon.cInt(GridViewBeforPrentag.GetRowCellValue(i, "SizeID").ToString());
        //        returnedOutOn.QTY = Comon.cDbl(GridViewBeforPrentag.GetRowCellValue(i, "PrentagDebit").ToString());
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
        //            long VoucherID = SaveVariousVoucherMachinBrntage(Comon.cInt(objRecordOutOnMaster.InvoiceID), isNew);
        //            if (VoucherID == 0)
        //                Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
        //            else
        //                Lip.ExecututeSQL("Update " + Menu_FactoryRunCommandPrentagAndPulishnDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Menu_FactoryRunCommandPrentagAndPulishnDAL.PremaryKey + " = " + txtCommandID.Text);
        //        }
        //    }
        //    #endregion
        //}
        //private void SaveInOnBrntage()
        //{
        //    #region Save Out On
        //    //Save Out On
        //    bool isNew = IsNewRecord;
        //    Stc_ManuFactoryCommendOutOnBail_Master objRecordInOnMaster = new Stc_ManuFactoryCommendOutOnBail_Master();
        //    if (IsNewRecord)
        //        objRecordInOnMaster.InvoiceID = Comon.cInt(Stc_ManuFactoryCommendOutOnBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID, 2));
        //    else
        //    {
        //        DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeBrntageAfterFrist);
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
        //    objRecordInOnMaster.DocumentType = DocumentTypeBrntageAfterFrist;
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
        //    for (int i = 0; i <= GridViewAfterPrentag.DataRowCount - 1; i++)
        //    {
        //        returnedOutOn = new Stc_ManuFactoryCommendOutOnBail_Details();
        //        returnedOutOn.InvoiceID = objRecordInOnMaster.InvoiceID;
        //        returnedOutOn.CommandID = Comon.cInt(txtCommandID.Text);
        //        returnedOutOn.FacilityID = UserInfo.FacilityID;
        //        returnedOutOn.BranchID = Comon.cInt(cmbBranchesID.EditValue);
        //        returnedOutOn.CommandDate = Comon.cDate(GridViewAfterPrentag.GetRowCellValue(i, "PrentagDebitDate").ToString());
        //        returnedOutOn.CommandTime ="0";
        //        returnedOutOn.BarCode = GridViewAfterPrentag.GetRowCellValue(i, "BarcodePrentag").ToString();
        //        returnedOutOn.ItemID = Comon.cInt(GridViewAfterPrentag.GetRowCellValue(i, "ItemID").ToString());
        //        returnedOutOn.SizeID = Comon.cInt(GridViewAfterPrentag.GetRowCellValue(i, "SizeID").ToString());
        //        returnedOutOn.QTY = Comon.cDbl(GridViewAfterPrentag.GetRowCellValue(i, "PrentagCredit").ToString());
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
        //            long VoucherID = SaveVariousVoucherMachinInOnBrntage(Comon.cInt(objRecordInOnMaster.InvoiceID), isNew);
        //            if (VoucherID == 0)
        //                Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
        //            else
        //                Lip.ExecututeSQL("Update " + Menu_FactoryRunCommandPrentagAndPulishnDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Menu_FactoryRunCommandPrentagAndPulishnDAL.PremaryKey + " = " + txtCommandID.Text);
        //        }
        //    }
        //    #endregion
        //}
        private int SaveStockMoveingBrntageOut(int DocumentID)
        {
            Stc_ItemsMoviing objRecord = new Stc_ItemsMoviing();
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue); 
            if (Comon.cInt(cmbPrntageTypeID.EditValue)==1)
               objRecord.DocumentTypeID = DocumentTypeBrntageBeforeFrist;
            else if (Comon.cInt(cmbPrntageTypeID.EditValue) == 2)
                objRecord.DocumentTypeID = DocumentTypeBrntageBeforeScand;
            objRecord.MoveType = 2;
            objRecord.MoveID = 0;
            objRecord.TranseID = DocumentID;
            objRecord.Posted = Comon.cInt(cmbStatus.EditValue);
            Stc_ItemsMoviing returned;
            List<Stc_ItemsMoviing> listreturned = new List<Stc_ItemsMoviing>();
            for (int i = 0; i <= GridViewBeforPrentag.DataRowCount - 1; i++)
            {
                returned = new Stc_ItemsMoviing();
                returned.ID = i + 1;
                returned.MoveDate = Comon.ConvertDateToSerial(((DateTime)GridViewBeforPrentag.GetRowCellValue(i, "PrentagDebitDate")).ToString("dd/MM/yyyy")).ToString(); 
                returned.MoveID = 0;
                returned.TranseID = DocumentID;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                if (Comon.cInt(cmbPrntageTypeID.EditValue) == 1)
                    returned.DocumentTypeID = DocumentTypeBrntageBeforeFrist;
                else if (Comon.cInt(cmbPrntageTypeID.EditValue) == 2)
                    returned.DocumentTypeID = DocumentTypeBrntageBeforeScand;
                returned.MoveType = 2;
                returned.StoreID = Comon.cDbl(txtStoreID.Text.ToString());
                returned.AccountID = Comon.cDbl(txtAccountID.Text);
                returned.BarCode = GridViewBeforPrentag.GetRowCellValue(i, "BarcodePrentag").ToString();
                returned.ItemID = Comon.cInt(GridViewBeforPrentag.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(GridViewBeforPrentag.GetRowCellValue(i, "SizeID").ToString());
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID + " and BranchID=" + MySession.GlobalBranchID));
                returned.QTY = Comon.cDbl(GridViewBeforPrentag.GetRowCellValue(i, "PrentagDebit").ToString());
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
           
            if (Comon.cInt(cmbPrntageTypeID.EditValue) == 1)
                objRecord.DocumentTypeID = DocumentTypeBrntageAfterFrist;
            else if (Comon.cInt(cmbPrntageTypeID.EditValue) == 2)
                objRecord.DocumentTypeID =DocumentTypeBrntageAfterScand ;
            objRecord.MoveType = 1;
            objRecord.MoveID = 0;
            objRecord.TranseID = DocumentID;
            objRecord.Posted = Comon.cInt(cmbStatus.EditValue);
            Stc_ItemsMoviing returned;
            List<Stc_ItemsMoviing> listreturned = new List<Stc_ItemsMoviing>();
            for (int i = 0; i <= GridViewAfterPrentag.DataRowCount - 1; i++)
            {
                returned = new Stc_ItemsMoviing();
                returned.ID = i + 1;
                returned.MoveDate = Comon.ConvertDateToSerial(((DateTime)GridViewAfterPrentag.GetRowCellValue(i, "PrentagDebitDate")).ToString("dd/MM/yyyy")).ToString(); 
                returned.MoveID = 0;
                returned.TranseID = DocumentID;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                if (Comon.cInt(cmbPrntageTypeID.EditValue) == 1)
                    returned.DocumentTypeID = DocumentTypeBrntageAfterFrist;
                else if (Comon.cInt(cmbPrntageTypeID.EditValue) == 2)
                    returned.DocumentTypeID = DocumentTypeBrntageAfterScand;
                returned.MoveType = 1;
                returned.StoreID = Comon.cDbl(txtStoreID.Text.ToString());
                returned.AccountID = Comon.cDbl(txtAccountID.Text);
                returned.BarCode = GridViewAfterPrentag.GetRowCellValue(i, "BarcodePrentag").ToString();
                returned.ItemID = Comon.cInt(GridViewAfterPrentag.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(GridViewAfterPrentag.GetRowCellValue(i, "SizeID").ToString());
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM  Stc_Items where [ItemID]=" + returned.ItemID + " and BranchID=" + MySession.GlobalBranchID));
                returned.QTY = Comon.cDbl(GridViewAfterPrentag.GetRowCellValue(i, "PrentagCredit").ToString());
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

                    if (Comon.cInt(cmbPrntageTypeID.EditValue) == 1) {
                        MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text), DocumentTypeBrntageBeforeFrist);
                        MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text), DocumentTypeBrntageAfterFrist);
                    }
                    else if (Comon.cInt(cmbPrntageTypeID.EditValue) == 2)
                    {
                        MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text), DocumentTypeBrntageBeforeScand);
                        MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text), DocumentTypeBrntageAfterScand);
                    }

                        
                     if (MoveID <0)
                         Messages.MsgError(Messages.TitleInfo, "خطا في حذف الحركة  المخزنية");
                 }

                 #region Delete Voucher Machin
                 //حذف القيد الالي
                 if (Comon.cInt(Result) > 0)
                 {
                     int VoucherID = 0;
                    
                     

                     int VoucherIDBrntageBrfore = 0;
                    int VoucherIDBrntageAfter = 0;
                    if (Comon.cInt(cmbPrntageTypeID.EditValue) == 1)
                    {
                        VoucherIDBrntageBrfore = DeleteVariousVoucherMachin(Comon.cInt(txtCommandID.Text), DocumentTypeBrntageBeforeFrist);
                        VoucherIDBrntageAfter = DeleteVariousVoucherMachin(Comon.cInt(txtCommandID.Text), DocumentTypeBrntageAfterFrist);

                    }
                    else if (Comon.cInt(cmbPrntageTypeID.EditValue) == 2) 
                    {
                        VoucherIDBrntageBrfore = DeleteVariousVoucherMachin(Comon.cInt(txtCommandID.Text), DocumentTypeBrntageBeforeScand);
                        VoucherIDBrntageAfter = DeleteVariousVoucherMachin(Comon.cInt(txtCommandID.Text), DocumentTypeBrntageAfterScand);

                    }
                    if (VoucherIDBrntageBrfore == 0)
                             Messages.MsgError(Messages.TitleInfo, "خطا في حذف قيد العملية برنتاج-قبل");
                    if (VoucherIDBrntageAfter == 0)
                             Messages.MsgError(Messages.TitleInfo, "خطا في حذف قيد العملية برنتاج-بعد");
                     
                    
                     
                 }
                 #endregion

                 #region Delete Stock IN Or Out From archive
                 ////حذف التوريد والصرف من الارشيف
                 //if (Comon.cInt(Result) > 0)
                 //{
                   

                 //    int OutBrntageID = 0;
                 //    DataTable dtInvoiceIDBrntageBefor = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(txtCommandID.Text), DocumentTypeBrntageBeforeFrist);
                 //    if (dtInvoiceIDBrntageBefor.Rows.Count > 0)
                 //    {
                 //        OutBrntageID = DeleteInOnOROutOnBil(Comon.cInt(dtInvoiceIDBrntageBefor.Rows[0][0]), DocumentTypeBrntageBeforeFrist);
                 //        if (OutBrntageID == 0)
                 //            Messages.MsgError(Messages.TitleInfo, "خطا في حذف الصرف من الارشيف للعلية برنتاج- قبل  ");
                 //    }
                 //    int InBrntageID = 0;
                 //    DataTable dtInvoiceIDBrntageAfter = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(txtCommandID.Text), DocumentTypeBrntageAfterFrist);
                 //    if (dtInvoiceIDBrntageAfter.Rows.Count > 0)
                 //    {
                 //        InBrntageID = DeleteInOnOROutOnBil(Comon.cInt(dtInvoiceIDBrntageAfter.Rows[0][0]), DocumentTypeBrntageAfterFrist);
                 //        if (InBrntageID == 0)
                 //            Messages.MsgError(Messages.TitleInfo, "خطا في حذف التوريد من الارشيف للعملية برنتاج- بعد ");
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
                cmbCurency.EditValue = Comon.cInt(MySession.GlobalDefaultPrntageCurrncyID);
                //جريد فيو
                initGridBeforPrentage();
                initGridAfterPrentage();
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
                strSQL = "SELECT " + PrimaryName + " as AccountName FROM Acc_Accounts WHERE AccountID =" + Comon.cDbl(txtAccountID.Text) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID;
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

        private void gridViewAdditional_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
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
        private void GridViewAfterPrentag_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName != "PrSignature")
            {
                e.Graphics.FillRectangle(e.Cache.GetSolidBrush(Color.Beige), e.Bounds);
                e.Graphics.DrawRectangle(e.Cache.GetPen(Color.Black, 1), e.Bounds);
                e.Appearance.DrawString(e.Cache, e.DisplayText, e.Bounds);
                e.Handled = true;
                ((GridView)sender).Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;
                ((GridView)sender).Appearance.Row.TextOptions.VAlignment = VertAlignment.Center;

            }
        }

        private void btnMachinResractionBrntageBefore_Click(object sender, EventArgs e)
        {
            if (IsNewRecord == true) return;
            int ID = 0;
            if (Comon.cInt(cmbPrntageTypeID.EditValue) == 1)
                 ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + txtCommandID.Text + " And DocumentType=" + DocumentTypeBrntageBeforeFrist).ToString());
            else if (Comon.cInt(cmbPrntageTypeID.EditValue) == 2)
                ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + txtCommandID.Text + " And DocumentType=" + DocumentTypeBrntageBeforeScand).ToString());
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

        private void btnMachinResractionBrntageAfter_Click(object sender, EventArgs e)
        {
            if (IsNewRecord == true) return;
            int ID = 0;
            if (Comon.cInt(cmbPrntageTypeID.EditValue) == 1)
                 ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + txtCommandID.Text + " And DocumentType=" + DocumentTypeBrntageAfterFrist).ToString());
            else if (Comon.cInt(cmbPrntageTypeID.EditValue) == 2)
                ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + txtCommandID.Text + " And DocumentType=" + DocumentTypeBrntageAfterScand).ToString());

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

        private void labelControl37_Click(object sender, EventArgs e)
        {

        }

        private void cmbBranchesID_EditValueChanged(object sender, EventArgs e)
        {

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
                ReportName = "rptManu_FactoryPrentagOpretion";
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
                subreportBeforeCasting.ReportSource = Manu_PrentagdStage(GridViewBeforPrentag);

                /******************** Report Factory ************************/
                XRSubreport subreportFactor = (XRSubreport)rptForm.FindControl("subRptrptManu_FactoryFactorCommendBefore", true);
                subreportFactor.Visible = IncludeHeader;
                subreportFactor.ReportSource = Manu_PrentagdStage(GridViewAfterPrentag);


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

        private void txtCurrncyPrice_EditValueChanged(object sender, EventArgs e)
        {

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
                if ( Grid.GetRowCellValue(i, ColBarCode) != null && Grid.GetRowCellValue(i, ColBarCode).ToString().Trim()!="")
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
                        GridViewBeforPrentag.AddNewRow();
                        if (ChekOrderIsFoundInGrid(GridViewBeforPrentag,"BarcodePrentag", BarCode))
                        {
                            Messages.MsgAsterisk(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "الصنف موجود  لذلك لا يمكن انزاله  اكثر من مرة " : "This Item is Found Table");
                            GridViewBeforPrentag.DeleteRow(rowIndex);
                            return;
                        }
                      
                        string QTY = view.GetRowCellValue(view.FocusedRowHandle, "QTY").ToString();
                        FillItemData(GridViewBeforPrentag, gridControlBeforPrentag, "BarcodePrentag", "PrentagDebit", Stc_itemsDAL.GetItemData1(BarCode, UserInfo.FacilityID), "PrentagDebitDate", "PrentagDebitTime", ((TextEdit)txtAccountID), QTY);


                        SendKeys.Send("\t");

                    }

                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void GridViewBeforPrentag_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            rowIndex = e.RowHandle;
        }

        private void GridViewBeforPrentag_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridView view = sender as GridView;

                if (view.GetRowCellValue(view.FocusedRowHandle, "BarcodePrentag").ToString().Trim() != "")
                {
                    string BarCode = view.GetRowCellValue(view.FocusedRowHandle, "BarcodePrentag").ToString().Trim();
                    DataTable dt;
                    dt = Stc_itemsDAL.GetItemData(BarCode, UserInfo.FacilityID);
                    if (dt.Rows.Count > 0)
                    {
                        GridViewAfterPrentag.AddNewRow();
                        if (ChekOrderIsFoundInGrid(GridViewAfterPrentag, "BarcodePrentag", BarCode))
                        {
                            Messages.MsgAsterisk(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "الصنف موجود  لذلك لا يمكن انزاله  اكثر من مرة " : "This Item is Found Table");
                            GridViewAfterPrentag.DeleteRow(rowIndex);
                            return;
                        }                       
                        string QTY = view.GetRowCellValue(view.FocusedRowHandle, "PrentagDebit").ToString();
                        FillItemData(GridViewAfterPrentag, gridControlAfterPrentage, "BarcodePrentag", "PrentagCredit", Stc_itemsDAL.GetItemData1(BarCode, UserInfo.FacilityID), "PrentagDebitDate", "PrentagDebitTime", txtAccountID, QTY);
                        SendKeys.Send("\t");
                    }

                }
            }
            catch(Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void GridViewAfterPrentag_InitNewRow(object sender, InitNewRowEventArgs e)
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
                for (int i = 0; i <= GridViewAfterPrentag.DataRowCount - 1; i++)
                {
                    dtItem.Rows.Add();
                    dtItem.Rows[i]["ID"] = i;
                    dtItem.Rows[i]["FacilityID"] = UserInfo.FacilityID;
                    dtItem.Rows[i]["BarCode"] = GridViewAfterPrentag.GetRowCellValue(i, "BarcodePrentag").ToString();
                    dtItem.Rows[i]["ItemID"] = Comon.cInt(GridViewAfterPrentag.GetRowCellValue(i, "ItemID").ToString());
                    DataTable dt = Lip.SelectRecord("SELECT   [GroupID]  ," + PrimaryName + "  FROM  [Stc_ItemsGroups] where Cancel=0 and BranchID=" + MySession.GlobalBranchID+" and [GroupID] in(select [GroupID] from Stc_Items where ItemID=" + dtItem.Rows[i]["ItemID"] + " and Cancel=0 and BranchID=" + MySession.GlobalBranchID+" ) ");
                    dtItem.Rows[i]["GroupID"] = Comon.cDbl(dt.Rows[0]["GroupID"]);
                    dtItem.Rows[i][GroupName] = dt.Rows[0][PrimaryName].ToString();

                    dtItem.Rows[i]["SizeID"] = Comon.cInt(GridViewAfterPrentag.GetRowCellValue(i, "SizeID").ToString());
                    dtItem.Rows[i][ItemName] = GridViewAfterPrentag.GetRowCellValue(i, ItemName).ToString();
                    dtItem.Rows[i][SizeName] = GridViewAfterPrentag.GetRowCellValue(i, SizeName).ToString();
                    dtItem.Rows[i]["QTY"] = Comon.ConvertToDecimalQty(GridViewAfterPrentag.GetRowCellValue(i, "PrentagCredit").ToString());
                    //  dtItem.Rows[i]["PackingQty"] = Comon.ConvertToDecimalPrice(GridViewAfterPrentag.GetRowCellValue(i, "PackingQty").ToString());
                    dtItem.Rows[i]["SalePrice"] = 0;

                    dtItem.Rows[i]["Description"] = UserInfo.Language == iLanguage.Arabic ? "تحويل من مرحلة  "+this.Text : "Transfer from manufacturing";

                    dtItem.Rows[i]["StoreAccountID"] = Comon.cDbl(txtStoreID.Text);
                    dtItem.Rows[i]["StoreName"] =lblStoreName.Text.ToString();
                    dtItem.Rows[i]["Caliber"] = 18;

                    dtItem.Rows[i]["CostPrice"] =GridViewAfterPrentag.GetRowCellValue(i, "CostPrice")!=null? Comon.ConvertToDecimalPrice(GridViewAfterPrentag.GetRowCellValue(i, "CostPrice").ToString()):0;
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
                strSQL = "SELECT TOP 1 ComandID FROM " + Menu_FactoryRunCommandMasterDAL.TableName + " Where  TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + " and BranchID=" + MySession.GlobalBranchID+" and  Cancel =0 and  ComandID>" + Comon.cLong(txtCommandID.Text) + " and Barcode=" + txtOrderID.Text;
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
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
using DevExpress.DashboardCommon.Viewer;
using DevExpress.XtraGrid.Views.BandedGrid;
using Edex.AccountsObjects.Codes;
using Edex.HR.Codes;
using Edex.StockObjects.Codes;
using Edex.StockObjects.Transactions;
using System.Globalization;

namespace Edex.Manufacturing.Codes
{
    public partial class frmManufacturingCommand : BaseForm
    {
        //list detail
        //list detail

        BindingList<Menu_FactoryOrderDetails> lstOrderDetails = new BindingList<Menu_FactoryOrderDetails>();
        BindingList<Menu_FactoryRunCommandfactory> lstDetailfactory = new BindingList<Menu_FactoryRunCommandfactory>();
        BindingList<Menu_FactoryRunCommandfactory> lstDetailAfterfactory = new BindingList<Menu_FactoryRunCommandfactory>();
        BindingList<Manu_ProductionExpensesDetails> lstDetailProductionExpenses = new BindingList<Manu_ProductionExpensesDetails>();
        BindingList<Manu_AuxiliaryMaterialsDetails> lstDetailAlcadZircon = new BindingList<Manu_AuxiliaryMaterialsDetails>();
        BindingList<Stc_ItemUnits> lstDetailUnit = new BindingList<Stc_ItemUnits>();
        #region Declare
        public int DocumentTypeFactoryBefore = 32;
        public int DocumentTypeFactoryAfter = 33;
      
        private Menu_FactoryRunCommandMasterDAL cClass = new Menu_FactoryRunCommandMasterDAL();
        int rowIndex = 0;
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
        public CultureInfo culture = new CultureInfo("en-US");
        #endregion
        public frmManufacturingCommand()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
            InitializeComponent();
            SplashScreenManager.CloseForm();

            //Events
             
            this.txtCustomerID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCustomerID_Validating);
            this.txtEmpIDFactor.Validating += new System.ComponentModel.CancelEventHandler(this.txtEmpFactorID_Validating);
            //this.txtEmployeeStokIDFactory.Validating += new System.ComponentModel.CancelEventHandler(this.txtEmployeeStokID_Validating);
            this.txtCommandID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCommandID_Validating);
            this.txtTypeOrder.Validating += new System.ComponentModel.CancelEventHandler(this.txtTypeOrder_Validating);
            this.txtGuidanceID.Validating += new System.ComponentModel.CancelEventHandler(this.txtGuidanceID_Validating);

            this.txtOrderID.Validating += new System.ComponentModel.CancelEventHandler(this.txtOrderID_Validating);

            this.txtReferanceID.Validating += txtReferanceID_Validating;
            //Event GridView
 
            this.gridControlfactroOpretion.ProcessGridKey += gridControl2_ProcessGridKey;
            this.gridControlAfterFactory.ProcessGridKey += gridControl2_ProcessGridKey;

            this.GridViewBeforfactory.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.GridViewBeforfactory_ValidatingEditor);
            this.GridViewAfterfactory.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.GridViewAfterfactory_ValidatingEditor);
            this.GridViewBeforfactory.RowUpdated += GridViewBeforfactory_RowUpdated;
            this.GridViewAfterfactory.RowUpdated += GridViewBeforfactory_RowUpdated;
           
            ItemName = "ArbItemName";
            SizeName = "ArbSizeName";
            PrimaryName = "ArbName";
            GroupName = "ArbGroupName";
            CaptionItemName = "اسم الصنف";
            if (UserInfo.Language == iLanguage.English)
            {
                GroupName = "EngGroupName";
                ItemName = "EngItemName";
                SizeName = "EngSizeName";
                PrimaryName = "EngName";
                CaptionItemName = "Item Name";
            }

            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
            FillCombo.FillComboBox(cmbCurency, "Acc_Currency", "ID", PrimaryName, "", " BranchID="+MySession.GlobalBranchID, (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
            FillCombo.FillComboBox(cmbTypeStage, "Manu_TypeStages", "ID", PrimaryName, "", "", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
            cmbTypeStage.EditValue = 6;
            cmbTypeStage.ReadOnly = true;
            FillCombo.FillComboBox(cmbStatus, "Manu_TypeStatus", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
            cmbStatus.EditValue = MySession.GlobalDefaultProcessPostedStatus;
            cmbCurency.EditValue = MySession.GlobalDefaultSaleCurencyID;
            cmbBranchesID.EditValue = MySession.GlobalBranchID;
              GridViewAfterfactory.CellValueChanging += GridViewAfterfactory_CellValueChanging;
              EnableControlDefult();

        }
        void EnableControlDefult()
        {
         
            cmbCurency.ReadOnly = !MySession.GlobalAllowChangefrmManufactoryCurrncyID;
            txtCommandDate.ReadOnly = !MySession.GlobalAllowChangefrmManufactoryCommandDate;
            txtStoreIDFactory.ReadOnly = !MySession.GlobalAllowChangefrmManufactoryStoreID;
            txtAccountIDFactory.ReadOnly = !MySession.GlobalAllowChangefrmManufatoryAccountID;
            txtEmpIDFactor.ReadOnly = !MySession.GlobalAllowChangefrmManufactoryEmployeeID;

        }
        void SetDefultValue()
        {

            cmbCurency.EditValue =Comon.cInt( MySession.GlobalDefaultManufactoryCurrncyID);
            cmbCurency_EditValueChanged(null, null);
            txtStoreIDFactory.Text = MySession.GlobalDefaultManufactoryStoreID;
            txtStoreIDFactory_Validating(null, null);
            txtAccountIDFactory.Text = MySession.GlobalDefaultManufactoryAccountID;
           txtAccountIDFactory_Validating(null, null);
           txtEmpIDFactor.Text = MySession.GlobalDefaultManufatoryEmployeeID;
           txtEmpIDFactor_Validating(null, null);
        }

    void GridViewAfterfactory_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
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
            else
                if (e.Column.FieldName == "HimLost")
                {
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
        
        public void txtCommandID_Validating(object sender, CancelEventArgs e)
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

            if (this.GridViewAfterfactory.ActiveEditor is CheckEdit)
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
            if (this.GridViewAfterfactory.ActiveEditor is TextEdit)
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
                        view.SetColumnError(GridViewAfterfactory.Columns[ColName], "");
                    }

                    if (ColName == "MachinID")
                    {
                        DataTable dtGroupID = Lip.SelectRecord("Select " + PrimaryName + " from Menu_FactoryMachine Where MachineID=" + Comon.cInt(e.Value) + " and BranchID=" + MySession.GlobalBranchID);
                        if (dtGroupID.Rows.Count > 0)
                        {
                            FileDataMachinName(GridViewAfterfactory, "DebitDate", "DebitTime", Comon.cInt(e.Value));
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
                            FillItemData(GridViewAfterfactory, gridControlAfterFactory, "BarCode", "Credit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", txtAccountIDFactory);
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
                            GridViewAfterfactory.SetRowCellValue(GridViewAfterfactory.FocusedRowHandle, GridViewAfterfactory.Columns[SizeName], dtItemID.Rows[0][PrimaryName].ToString());
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

                            GridViewAfterfactory.SetRowCellValue(GridViewAfterfactory.FocusedRowHandle, GridViewAfterfactory.Columns["EmpName"], dtNameEmp.Rows[0][PrimaryName].ToString());
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
                        DataTable dtItemID = Lip.SelectRecord("Select  " + PrimaryName + " from  Stc_Stores Where Cancel=0 And StoreID=" + val.ToString() + " and BranchID="+MySession.GlobalBranchID+" And FacilityID=" + UserInfo.FacilityID);
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridViewAfterfactory.SetFocusedRowCellValue("StoreName", dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(GridViewAfterfactory.Columns[ColName], "");

                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(GridViewAfterfactory.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }

                }
                if (ColName == ItemName)
                {

                    DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_Items_Find  Where  LOWER (" + ItemName + ")=LOWER ('" + val.ToString() + "')  and BranchID=" + MySession.GlobalBranchID);
                    if (dtItemID.Rows.Count > 0)
                    {
                        FillItemData(GridViewAfterfactory, gridControlAfterFactory, "BarCode", "Credit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", ((TextEdit)txtAccountIDFactory));
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
                        GridViewAfterfactory.SetFocusedRowCellValue("MachinID", dtMachinID.Rows[0]["MachineID"].ToString());

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
                    {
                        string formattedDate = ((DateTime)e.Value).ToString("yyyy/MM/dd");
                        if (Lip.CheckDateISAvilable(formattedDate))
                        {
                            string serverDate = Lip.GetServerDate(); e.Value = serverDate;
                            GridViewAfterfactory.SetRowCellValue(GridViewAfterfactory.FocusedRowHandle, "DebitDate", serverDate);
                            return;
                        }
                    }
                }
                if (ColName == SizeName)
                {
                    string Str = "Select Stc_ItemUnits.SizeID from Stc_ItemUnits inner join Stc_Items on Stc_Items.ItemID=Stc_ItemUnits.ItemID and Stc_Items.BranchID=Stc_ItemUnits.BranchID left outer join  Stc_SizingUnits on Stc_ItemUnits.SizeID=Stc_SizingUnits.SizeID  and Stc_ItemUnits.BranchID=Stc_SizingUnits.BranchID Where UnitCancel=0 and BranchID="+MySession.GlobalBranchID+" And LOWER (Stc_SizingUnits." + PrimaryName + ")=LOWER ('" + val.ToString() + "') and  Stc_Items.ItemID=" + Comon.cLong(GridViewAfterfactory.GetRowCellValue(GridViewAfterfactory.FocusedRowHandle, "ItemID").ToString()) + "  And Stc_ItemUnits.FacilityID=" + UserInfo.FacilityID;
                    DataTable dtSizeID = Lip.SelectRecord(Str);
                    if (dtSizeID.Rows.Count > 0)
                    {

                        FillItemData(GridViewAfterfactory, gridControlAfterFactory, "BarCode", "Credit", Stc_itemsDAL.GetItemDataByItemID_SizeID(Comon.cInt(GridViewAfterfactory.GetRowCellValue(GridViewAfterfactory.FocusedRowHandle, "ItemID")), Comon.cInt(dtSizeID.Rows[0][0].ToString()), UserInfo.FacilityID), "DebitDate", "DebitTime", ((TextEdit)txtAccountIDFactory));
                        GridViewAfterfactory.SetRowCellValue(GridViewAfterfactory.FocusedRowHandle, ColName, e.Value.ToString());

                      
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundSizeForItem;
                        view.SetColumnError(GridViewAfterfactory.Columns[ColName], Messages.msgNoFoundSizeForItem);
                    }

                }
                if (ColName == "EmpName")
                {

                    DataTable dtMachinID = Lip.SelectRecord("Select  EmployeeID  from HR_EmployeeFile Where  LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') and BranchID=" + MySession.GlobalBranchID);
                    if (dtMachinID.Rows.Count > 0)
                    {
                        GridViewAfterfactory.SetFocusedRowCellValue("EmpID", dtMachinID.Rows[0]["EmployeeID"].ToString());

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
                        GridViewAfterfactory.SetFocusedRowCellValue("StoreID", dtItemID.Rows[0]["StoreID"]);
                        e.Valid = true;
                        view.SetColumnError(GridViewAfterfactory.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(GridViewAfterfactory.Columns[ColName], Messages.msgNoFoundThisItem);
                    }
                }
            }
        }

        private void FillItemData(GridView Grid,GridControl GridControl,string BarCode,string QTYFildName, DataTable dt,string Date,string Time,TextEdit ObjtxtAccount,string QTY="")
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
                    if ((((GridView)Grid).Name == GridViewBeforfactory.Name))
                    {
                        totalQtyBalance = Lip.RemindQtyItemByMinUnit(Comon.cLong(dt.Rows[0]["ItemID"].ToString()), Comon.cInt(dt.Rows[0]["SizeID"]), Comon.cDbl(txtStoreIDFactory.Text));
                        {
                            decimal qtyCurrent = 0;
                            decimal QtyInCommand = Lip.GetQTYinCommandToThisItem("Menu_FactoryRunCommandfactory", "Menu_FactoryRunCommandMaster", "Debit", "ComandID", Comon.cInt(txtCommandID.Text), dt.Rows[0]["ItemID"].ToString(), " and Menu_FactoryRunCommandfactory.TypeOpration=1", BarCode, SizeID: Comon.cInt(dt.Rows[0]["SizeID"].ToString()));
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
                if ( (((GridView)Grid).Name == GridViewBeforfactory.Name))
                    Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[QTYFildName], totalQtyBalance);
                else
                    Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[QTYFildName], 0);

                //RepositoryItemLookUpEdit rSize = Common.LookUpEditSize(Comon.cInt(dt.Rows[0]["ItemID"].ToString()));
                //Grid.Columns[SizeName].ColumnEdit = rSize;
                //GridControl.RepositoryItems.Add(rSize);
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[SizeName], dt.Rows[0]["SizeName"].ToString()); 
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[Time], DateTime.Now.ToString("hh:mm:tt"));
                DateTime currentDate = DateTime.Now;
                string formattedDate = currentDate.ToString("dd/MM/yyyy");

                Grid.SetRowCellValue(Grid.FocusedRowHandle, Date, currentDate);
                Grid.Columns[Date].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                Grid.Columns[Date].DisplayFormat.FormatString = "dd/MM/yyyy";

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

            if (this.GridViewBeforfactory.ActiveEditor is TextEdit)
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
                        view.SetColumnError(GridViewBeforfactory.Columns[ColName], "");
                    }
                    if (ColName == "MachinID")
                    {
                        DataTable dtGroupID = Lip.SelectRecord("Select " + PrimaryName + " from Menu_FactoryMachine Where MachineID=" + Comon.cInt(e.Value));
                        if (dtGroupID.Rows.Count > 0)
                        {                        
                            e.Valid = true;
                            view.SetColumnError(GridViewBeforfactory.Columns[ColName], "");
                            GridViewBeforfactory.SetRowCellValue(GridViewBeforfactory.FocusedRowHandle, GridViewBeforfactory.Columns["ID"], GridViewBeforfactory.RowCount);
                            FileDataMachinName(GridViewBeforfactory,"DebitDate", "DebitTime", Comon.cInt(e.Value));     
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
                            FillItemData(GridViewBeforfactory, gridControlfactroOpretion, "BarCode", "Debit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", ((TextEdit)txtAccountIDFactory));
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
                        decimal totalQtyBalance = Lip.RemindQtyItemByMinUnit(Comon.cLong(GridViewBeforfactory.GetRowCellValue(GridViewBeforfactory.FocusedRowHandle, "ItemID").ToString()), Comon.cInt(GridViewBeforfactory.GetRowCellValue(GridViewBeforfactory.FocusedRowHandle, "SizeID")), Comon.cDbl(txtStoreIDFactory.Text));
                        decimal QtyInCommand = Lip.GetQTYinCommandToThisItem("Menu_FactoryRunCommandfactory", "Menu_FactoryRunCommandMaster", "Debit", "ComandID", Comon.cInt(txtCommandID.Text), GridViewBeforfactory.GetRowCellValue(GridViewBeforfactory.FocusedRowHandle, "ItemID").ToString(), " and Menu_FactoryRunCommandfactory.TypeOpration=1",SizeID:Comon.cInt( GridViewBeforfactory.GetRowCellValue(GridViewBeforfactory.FocusedRowHandle, "SizeID").ToString()));
                        totalQtyBalance += QtyInCommand;
                        decimal qtyCurrent = 0;
                       qtyCurrent = frmCadFactory.GetQTYToItemFromGridView(GridViewBeforfactory, "Debit", Comon.cDec(val.ToString()), GridViewBeforfactory.GetRowCellValue(GridViewBeforfactory.FocusedRowHandle, "ItemID").ToString(), Comon.cInt(GridViewBeforfactory.GetRowCellValue(GridViewBeforfactory.FocusedRowHandle, "SizeID")), "BarCode"); 

                        if (qtyCurrent > totalQtyBalance)
                        {
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheQTyinOrderisExceed);
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgQtyisNotAvilable + (totalQtyBalance - (qtyCurrent - Comon.cDec(val.ToString())));
                            view.SetColumnError(GridViewBeforfactory.Columns[ColName], Messages.msgQtyisNotAvilable + totalQtyBalance.ToString());
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
                                    view.SetColumnError(GridViewBeforfactory.Columns[ColName], Messages.msgQtyisNotAvilable + totalQtyBalance.ToString());
                                }
                            }
                            else
                            {
                                e.Valid = false;
                                HasColumnErrors = true;
                                e.ErrorText = Messages.msgNotFoundAnyQtyInStore;
                                view.SetColumnError(GridViewBeforfactory.Columns[ColName], Messages.msgNotFoundAnyQtyInStore);
                            }
                        }
                    }
                    if (ColName == "SizeID")
                    {

                        DataTable dtItemID = Lip.SelectRecord("Select  " + PrimaryName + " from Stc_SizingUnits  Where SizeID=" + e.Value + " and BranchID=" + MySession.GlobalBranchID);
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridViewBeforfactory.SetRowCellValue(GridViewBeforfactory.FocusedRowHandle, GridViewBeforfactory.Columns[SizeName], dtItemID.Rows[0][PrimaryName].ToString());
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

                            GridViewBeforfactory.SetRowCellValue(GridViewBeforfactory.FocusedRowHandle, GridViewBeforfactory.Columns["EmpName"], dtNameEmp.Rows[0][PrimaryName].ToString());
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
                        DataTable dtItemID = Lip.SelectRecord("Select  " + PrimaryName + " from  Stc_Stores Where Cancel=0 And StoreID=" + val.ToString() + " and BranchID="+MySession.GlobalBranchID+" And FacilityID=" + UserInfo.FacilityID);
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridViewBeforfactory.SetFocusedRowCellValue("StoreName", dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(GridViewBeforfactory.Columns[ColName], "");

                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(GridViewBeforfactory.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }

                }
                if (ColName == ItemName)
                {
                    DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_Items_Find  Where  LOWER (" + ItemName + ")=LOWER ('" + val.ToString() + "') and BranchID=" + MySession.GlobalBranchID);
                    if (dtItemID.Rows.Count > 0)
                    {    
                       FillItemData(GridViewBeforfactory,gridControlfactroOpretion,"BarCode","Debit",Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", ((TextEdit)txtAccountIDFactory));
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = " الصنف غير موجود  ";
                    }
                }
                else if (ColName == "DebitDate")
                {
                    {
                        string formattedDate = ((DateTime)e.Value).ToString("yyyy/MM/dd");
                        if (Lip.CheckDateISAvilable(formattedDate))
                        {

                            string serverDate = Lip.GetServerDate(); e.Value = serverDate;
                            GridViewBeforfactory.SetRowCellValue(GridViewBeforfactory.FocusedRowHandle, "DebitDate", serverDate);
                            return;
                        }
                    }
                }
                if (ColName == "MachineName")
                {

                    DataTable dtMachinID = Lip.SelectRecord("Select  MachineID  from Menu_FactoryMachine Where  LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') ");
                    if (dtMachinID.Rows.Count > 0)
                    {
                       
                        FileDataMachinName(GridViewBeforfactory, "DebitDate", "DebitTime", Comon.cInt(dtMachinID.Rows[0]["MachineID"].ToString()));
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

                    string Str = "Select Stc_ItemUnits.SizeID from Stc_ItemUnits inner join Stc_Items on Stc_Items.ItemID=Stc_ItemUnits.ItemID  and  Stc_Items.BranchID=Stc_ItemUnits.BranchID left outer join  Stc_SizingUnits on Stc_ItemUnits.SizeID=Stc_SizingUnits.SizeID and Stc_ItemUnits.BranchID=Stc_SizingUnits.BranchID Where UnitCancel=0 and BranchID="+MySession.GlobalBranchID+" And LOWER (Stc_SizingUnits." + PrimaryName + ")=LOWER ('" + val.ToString() + "') and  Stc_Items.ItemID=" + Comon.cLong(GridViewBeforfactory.GetRowCellValue(GridViewBeforfactory.FocusedRowHandle, "ItemID").ToString()) + "  And Stc_ItemUnits.FacilityID=" + UserInfo.FacilityID;
                    DataTable dtBarCode = Lip.SelectRecord(Str);
                    if (dtBarCode.Rows.Count > 0)
                    {
                        GridViewBeforfactory.SetFocusedRowCellValue("SizeID", dtBarCode.Rows[0]["SizeID"]);
                        frmCadFactory.SetValuseWhenChangeSizeName(GridViewBeforfactory, Comon.cLong(GridViewBeforfactory.GetRowCellValue(GridViewBeforfactory.FocusedRowHandle, "ItemID").ToString()), Comon.cInt(dtBarCode.Rows[0]["SizeID"]), "Menu_FactoryRunCommandfactory", "Menu_FactoryRunCommandMaster", Comon.cDbl(txtStoreIDFactory.Text), Comon.cInt(txtCommandID.Text), "ComandID", Where: " and Menu_FactoryRunCommandfactory.TypeOpration=1", FildNameQTY: "Debit");
                        e.Valid = true;
                        view.SetColumnError(GridViewBeforfactory.Columns[ColName], "");
                      }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundSizeForItem;
                        view.SetColumnError(GridViewBeforfactory.Columns[ColName], Messages.msgNoFoundSizeForItem);
                    }
                }
                if (ColName == "EmpName")
                {

                    DataTable dtMachinID = Lip.SelectRecord("Select  EmployeeID  from HR_EmployeeFile Where  LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') and BranchID=" + MySession.GlobalBranchID);
                    if (dtMachinID.Rows.Count > 0)
                    {
                        GridViewBeforfactory.SetFocusedRowCellValue("EmpID", dtMachinID.Rows[0]["EmployeeID"].ToString());

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
                        GridViewBeforfactory.SetFocusedRowCellValue("StoreID", dtItemID.Rows[0]["StoreID"]);
                        e.Valid = true;
                        view.SetColumnError(GridViewBeforfactory.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(GridViewBeforfactory.Columns[ColName], Messages.msgNoFoundThisItem);
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
        public void SetDetilOrder(string OrderID)
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
                IsNewRecord = false;
                if (DataRecord != null && DataRecord.Rows.Count > 0)
                {
                    DataRecordFactory = Menu_FactoryRunCommandfactoryDAL.frmGetDataDetalByID(Comon.cLong(ComandID), Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID,1);
                    DataRecordAfterFactory = Menu_FactoryRunCommandfactoryDAL.frmGetDataDetalByID(Comon.cLong(ComandID), Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID, 2);
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
                    txtEmpIDFactor.Text = DataRecord.Rows[0]["EmpFactorID"].ToString();
                    txtEmpFactorID_Validating(null, null);

                    //txtEmployeeStokIDFactory.Text = DataRecord.Rows[0]["EmployeeStokID"].ToString();
                    //txtEmployeeStokID_Validating(null, null);
                    
                    //الحسابات
                    txtAccountIDFactory.Text = DataRecord.Rows[0]["AccountIDFactory"].ToString();
                    txtAccountIDFactory_Validating(null, null);

                    txtStoreIDFactory.Text = DataRecord.Rows[0]["StoreIDFactory"].ToString();
                    txtStoreIDFactory_Validating(null, null);

                    //txtEmployeeStokIDFactory.Text = DataRecord.Rows[0]["EmployeeStokIDFactory"].ToString();
                    //txtEmployeeStokIDFactory_Validating(null, null);

                    //txtEmpIDFactor.Text = DataRecord.Rows[0]["EmpIDFactor"].ToString();
                    //txtEmpIDFactor_Validating(null, null);
                       
                    txtOrderID.Text = DataRecord.Rows[0]["Barcode"].ToString();

                    SetDetilOrder(txtOrderID.Text);

                    if (Comon.ConvertSerialDateTo(DataRecord.Rows[0]["ComandDate"].ToString()) == "")
                        InitializeFormatDate(txtCommandDate);
                    else
                       txtCommandDate.EditValue = DateTime.ParseExact(Comon.ConvertSerialDateTo(DataRecord.Rows[0]["ComandDate"].ToString()), "dd/MM/yyyy", culture);  
                    cmbCurency.EditValue =Comon.cInt( DataRecord.Rows[0]["CurrencyID"].ToString());

                    //int TheType = Comon.cInt( DataRecord.Rows[0]["TheType"].ToString());
                    //if(TheType==0)
                    //{
                    //    txtOrderStat.Text = "غير مرحلة";
                    //}


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


                    if (DataRecordFactory != null)
                        if (DataRecordFactory.Rows.Count > 0)
                        {
                            gridControlfactroOpretion.DataSource = DataRecordFactory;
                            lstDetailfactory.AllowNew = true;
                            lstDetailfactory.AllowEdit = true;
                            lstDetailfactory.AllowRemove = true;
                            GridViewBeforfactory.RefreshData();
                        }
                    if (DataRecordAfterFactory != null)
                        if (DataRecordAfterFactory.Rows.Count > 0)
                        {
                            gridControlAfterFactory.DataSource = DataRecordAfterFactory;
                            lstDetailAfterfactory.AllowNew = true;
                            lstDetailAfterfactory.AllowEdit = true;
                            lstDetailAfterfactory.AllowRemove = true;
                            GridViewAfterfactory.RefreshData();
                        }
                    int CommandIDTemp = 0;
                    CommandIDTemp = Comon.cInt(Lip.GetValue("select ComandID from Menu_FactoryRunCommandMaster where Cancel=0 and BranchID="+MySession.GlobalBranchID+" and TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + " and ComandID<>" + Comon.cInt(txtCommandID.Text) + " and Barcode='" + txtOrderID.Text + "'"));
                    
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
            GridViewOrderDetails.Columns["DebitDate"].Visible = false;
            GridViewOrderDetails.Columns["DebitTime"].Visible = false;
            GridViewOrderDetails.Columns["TypeOpration"].Visible = false;
             

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

                GridViewOrderDetails.Columns["StateName"].Visible = false;
                GridViewOrderDetails.Columns["EngItemName"].Visible = true;
                GridViewOrderDetails.Columns["EngSizeName"].Visible = true;

                GridViewOrderDetails.Columns["StoreID"].Caption = "Store ID";
                GridViewOrderDetails.Columns["StoreName"].Caption = "Store Name";
            }
            GridViewOrderDetails.OptionsBehavior.ReadOnly = true;
            GridViewOrderDetails.OptionsBehavior.Editable = false;
        }
        void initGridFactory()
        {

            lstDetailfactory = new BindingList<Menu_FactoryRunCommandfactory>();
            lstDetailfactory.AllowNew = true;
            lstDetailfactory.AllowEdit = true;
            lstDetailfactory.AllowRemove = true;
            gridControlfactroOpretion.DataSource = lstDetailfactory;

            DataTable dtitems = Lip.SelectRecord("SELECT  " + PrimaryName + "  FROM Menu_FactoryMachine ");
            string[] NameMachine = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameMachine[i] = dtitems.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameMachine);
            gridControlfactroOpretion.RepositoryItems.Add(riComboBoxitems);

            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0 and BranchID=" + MySession.GlobalBranchID);
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();
            riComboBoxitems2.Items.AddRange(StoreName);
            gridControlfactroOpretion.RepositoryItems.Add(riComboBoxitems2);
            GridViewBeforfactory.Columns["StoreName"].ColumnEdit = riComboBoxitems2;


            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0 and BranchID=" + MySession.GlobalBranchID);
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlfactroOpretion.RepositoryItems.Add(riComboBoxitems3);
            GridViewBeforfactory.Columns["EmpName"].ColumnEdit = riComboBoxitems3;

            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0 and BranchID=" + MySession.GlobalBranchID);
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlfactroOpretion.RepositoryItems.Add(riComboBoxitems4);
            GridViewBeforfactory.Columns[ItemName].ColumnEdit = riComboBoxitems4;

            RepositoryItemLookUpEdit rSize = Common.LookUpEditSize();
            GridViewBeforfactory.Columns[SizeName].ColumnEdit = rSize;
            gridControlfactroOpretion.RepositoryItems.Add(rSize);
          
             
            GridViewBeforfactory.Columns["ID"].Visible = false;
            GridViewBeforfactory.Columns["ComandID"].Visible = false;
            GridViewBeforfactory.Columns["BarCode"].Visible = false;
            GridViewBeforfactory.Columns["EmpPolishnID"].Visible = false;
            GridViewBeforfactory.Columns["EmpPrentagID"].Visible = false;
            GridViewBeforfactory.Columns["Cancel"].Visible = false;
            GridViewBeforfactory.Columns["BranchID"].Visible = false;
            GridViewBeforfactory.Columns["FacilityID"].Visible = false;
            GridViewBeforfactory.Columns["SizeID"].Visible = false;
            GridViewBeforfactory.Columns["EditUserID"].Visible = false;
            GridViewBeforfactory.Columns["EditDate"].Visible = false;
            GridViewBeforfactory.Columns["EditTime"].Visible = false;
            GridViewBeforfactory.Columns["RegDate"].Visible = false;
            GridViewBeforfactory.Columns["UserID"].Visible = false;

            GridViewBeforfactory.Columns["ComputerInfo"].Visible = false;
            GridViewBeforfactory.Columns["EditComputerInfo"].Visible = false;
            GridViewBeforfactory.Columns["RegTime"].Visible = false;

            GridViewBeforfactory.Columns["Credit"].Visible = false;
            GridViewBeforfactory.Columns["TypeOpration"].Visible = false;
            //GridViewBeforfactory.Columns["SizeID"].Visible = false;
            GridViewBeforfactory.Columns["CostPrice"].Visible = false;

            GridViewBeforfactory.Columns["EmpName"].Width = 120;

            GridViewBeforfactory.Columns["StoreName"].Width = 120;
            GridViewBeforfactory.Columns["EmpID"].Width = 120;
            GridViewBeforfactory.Columns["Signature"].Width = 120;
            GridViewBeforfactory.Columns["DebitDate"].Width = 110;
            GridViewBeforfactory.Columns["DebitTime"].Width = 85;
            GridViewBeforfactory.Columns["HimLost"].Visible = false;
            GridViewBeforfactory.Columns["EmpID"].Visible = false;
            GridViewBeforfactory.Columns["EmpName"].Visible = false;
            GridViewBeforfactory.Columns["StoreID"].Visible = false;
            GridViewBeforfactory.Columns["StoreName"].Visible = false;
            GridViewBeforfactory.Columns["Signature"].Visible = false;
            GridViewBeforfactory.Columns["ShownInNext"].Visible = false;
            if (UserInfo.Language == iLanguage.Arabic)
            {
                GridViewBeforfactory.Columns["EngItemName"].Visible = false;
                GridViewBeforfactory.Columns["EngSizeName"].Visible = false;
                GridViewBeforfactory.Columns["ArbItemName"].Width = 150;
                GridViewBeforfactory.Columns["StoreID"].Caption = "رقم المخزن";
                GridViewBeforfactory.Columns["StoreName"].Caption = "إسم المخزن";

                GridViewBeforfactory.Columns["EmpID"].Caption = "رقم العامل";
                GridViewBeforfactory.Columns["EmpName"].Caption = "إسم العامل";
                 
                GridViewBeforfactory.Columns["Debit"].Caption = "الوزن";

                GridViewBeforfactory.Columns["Credit"].Caption = "دائــن";
                GridViewBeforfactory.Columns["TypeOpration"].Caption = "نوع العملية";
                GridViewBeforfactory.Columns["Signature"].Caption = "التوقيع";

                GridViewBeforfactory.Columns["ItemID"].Caption = "رقم الصنف";
                GridViewBeforfactory.Columns["ArbItemName"].Caption = "اسم الصنف";
                GridViewBeforfactory.Columns["SizeID"].Caption = "رقم الوحده";
                GridViewBeforfactory.Columns[SizeName].Caption = "الوحده";
                GridViewBeforfactory.Columns["CostPrice"].Caption = "التكلفة";
                GridViewBeforfactory.Columns["DebitDate"].Caption = "التاريخ";
                GridViewBeforfactory.Columns["DebitTime"].Caption = "الوقت";
            }
            else
            {

                GridViewBeforfactory.Columns["ArbItemName"].Visible = false;
                GridViewBeforfactory.Columns["ArbSizeName"].Visible = false;


                GridViewBeforfactory.Columns["EngItemName"].Visible = true;
                GridViewBeforfactory.Columns["EngSizeName"].Visible = true;


                GridViewBeforfactory.Columns["StoreID"].Caption = "Store ID";
                GridViewBeforfactory.Columns["StoreName"].Caption = "Store Name";
                
            }
        }
        void initGridAfterFactory()
        {

            lstDetailAfterfactory = new BindingList<Menu_FactoryRunCommandfactory>();
            lstDetailAfterfactory.AllowNew = true;
            lstDetailAfterfactory.AllowEdit = true;
            lstDetailAfterfactory.AllowRemove = true;
            gridControlAfterFactory.DataSource = lstDetailAfterfactory;

            //

            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0 and BranchID=" + MySession.GlobalBranchID);
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();
            riComboBoxitems2.Items.AddRange(StoreName);
            gridControlAfterFactory.RepositoryItems.Add(riComboBoxitems2);
            GridViewAfterfactory.Columns["StoreName"].ColumnEdit = riComboBoxitems2;

            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0 and BranchID=" + MySession.GlobalBranchID);
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlAfterFactory.RepositoryItems.Add(riComboBoxitems3);
            GridViewAfterfactory.Columns["EmpName"].ColumnEdit = riComboBoxitems3;


            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0 and BranchID=" + MySession.GlobalBranchID);
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlAfterFactory.RepositoryItems.Add(riComboBoxitems4);
            GridViewAfterfactory.Columns[ItemName].ColumnEdit = riComboBoxitems4;

            RepositoryItemLookUpEdit rSize = Common.LookUpEditSize();
            GridViewAfterfactory.Columns[SizeName].ColumnEdit = rSize;
            gridControlAfterFactory.RepositoryItems.Add(rSize);
            GridViewAfterfactory.Columns[SizeName].OptionsColumn.AllowEdit = true;
            GridViewAfterfactory.Columns[SizeName].OptionsColumn.AllowFocus = true;
             
            GridViewBeforfactory.Columns[SizeName].ColumnEdit = rSize;
            gridControlAfterFactory.RepositoryItems.Add(rSize);
            GridViewAfterfactory.Columns[SizeName].OptionsColumn.AllowEdit = true;
            GridViewAfterfactory.Columns[SizeName].OptionsColumn.AllowFocus = true;
            //
            GridViewAfterfactory.Columns["ID"].Visible = false;
            GridViewAfterfactory.Columns["ComandID"].Visible = false;
            GridViewAfterfactory.Columns["BarCode"].Visible = false;
            GridViewAfterfactory.Columns["EmpPolishnID"].Visible = false;
            GridViewAfterfactory.Columns["EmpPrentagID"].Visible = false;
            GridViewAfterfactory.Columns["Cancel"].Visible = false;
            GridViewAfterfactory.Columns["BranchID"].Visible = false;
            GridViewAfterfactory.Columns["FacilityID"].Visible = false;
            GridViewAfterfactory.Columns["SizeID"].Visible = false;
            GridViewAfterfactory.Columns["EditUserID"].Visible = false;
            GridViewAfterfactory.Columns["EditDate"].Visible = false;
            GridViewAfterfactory.Columns["EditTime"].Visible = false;
            GridViewAfterfactory.Columns["RegDate"].Visible = false;
            GridViewAfterfactory.Columns["UserID"].Visible = false;

            GridViewAfterfactory.Columns["ComputerInfo"].Visible = false;
            GridViewAfterfactory.Columns["EditComputerInfo"].Visible = false;
            GridViewAfterfactory.Columns["RegTime"].Visible = false;

            GridViewAfterfactory.Columns["Debit"].Visible = false;
            GridViewAfterfactory.Columns["TypeOpration"].Visible = false;
            //GridViewAfterfactory.Columns["SizeID"].Visible = false;
            GridViewAfterfactory.Columns["CostPrice"].Visible = false;
             
            GridViewAfterfactory.Columns["EmpName"].Width = 120;
            GridViewAfterfactory.Columns["EmpID"].Width = 120;
            GridViewAfterfactory.Columns["StoreName"].Width = 100;
            GridViewAfterfactory.Columns["Signature"].Width = 120;
            GridViewAfterfactory.Columns["DebitDate"].Width = 110;
            GridViewAfterfactory.Columns["DebitTime"].Width = 85;

            GridViewAfterfactory.Columns["EmpID"].Visible = false;
            GridViewAfterfactory.Columns["EmpName"].Visible = false;
            GridViewAfterfactory.Columns["StoreID"].Visible = false;
            GridViewAfterfactory.Columns["StoreName"].Visible = false;
            GridViewAfterfactory.Columns["Signature"].Visible = false;

            if (UserInfo.Language == iLanguage.Arabic)
            {
                GridViewAfterfactory.Columns["EngItemName"].Visible = false;
                GridViewAfterfactory.Columns["EngSizeName"].Visible = false;
                GridViewAfterfactory.Columns["ArbItemName"].Width = 150;
                GridViewAfterfactory.Columns["StoreID"].Caption = "رقم المخزن";
                GridViewAfterfactory.Columns["StoreName"].Caption = "إسم المخزن";
                GridViewAfterfactory.Columns["EmpID"].Caption = "رقم العامل";
                GridViewAfterfactory.Columns["EmpName"].Caption = "إسم العامل";
                GridViewAfterfactory.Columns["Debit"].Caption = "الوزن";
                GridViewAfterfactory.Columns["Credit"].Caption = "الـوزن";
                GridViewAfterfactory.Columns["TypeOpration"].Caption = "نوع العملية";
                GridViewAfterfactory.Columns["Signature"].Caption = "التوقيع";
                GridViewAfterfactory.Columns["ItemID"].Caption = "رقم الصنف";
                GridViewAfterfactory.Columns["ArbItemName"].Caption = "اسم الصنف";
                GridViewAfterfactory.Columns["SizeID"].Caption = "رقم الوحده";
                GridViewAfterfactory.Columns["ArbSizeName"].Caption = "الوحده";
                GridViewAfterfactory.Columns["CostPrice"].Caption = "التكلفة";
                GridViewAfterfactory.Columns["DebitDate"].Caption = "التاريخ";
                GridViewAfterfactory.Columns["DebitTime"].Caption = "الوقت";
                GridViewAfterfactory.Columns["HimLost"].Caption = "علية فاقد ";

                GridViewAfterfactory.Columns["ShownInNext"].Caption = "يظهر في التفاصيل"; 
            }
            else
            {
                GridViewAfterfactory.Columns["ArbItemName"].Visible = false;
                GridViewAfterfactory.Columns["ArbSizeName"].Visible = false;
                GridViewAfterfactory.Columns["EngItemName"].Width = 150;
                GridViewAfterfactory.Columns["StoreID"].Caption = "Store ID";
                GridViewAfterfactory.Columns["StoreName"].Caption = "Store Name";
                GridViewAfterfactory.Columns["EngItemName"].Caption = "Item Name";
                //GridViewAfterfactory.Columns["MachinID"].Caption = "Machine ID";
                //GridViewAfterfactory.Columns["MachineName"].Caption = "Machin Name";
                GridViewAfterfactory.Columns["Debit"].Caption = "debtor ";
                GridViewAfterfactory.Columns["EngSizeName"].Caption = "Unit";
                GridViewAfterfactory.Columns["Credit"].Caption = "QTY";
                GridViewAfterfactory.Columns["TypeOpration"].Caption = "Type Opration";
                GridViewAfterfactory.Columns["Signature"].Caption = "Signature";
                GridViewAfterfactory.Columns["DebitDate"].Caption = "Date";
                GridViewAfterfactory.Columns["DebitTime"].Caption = "Time";
                GridViewAfterfactory.Columns["EmpID"].Caption = "EmpID";
                GridViewAfterfactory.Columns["EmpName"].Caption = "Name";
                GridViewAfterfactory.Columns["HimLost"].Caption = "HimLost";

                GridViewAfterfactory.Columns["ShownInNext"].Caption = "Shown In Next"; 
            }
        }



        #endregion
        private void frmManufacturingOrder_Load(object sender, EventArgs e)
        {
            try
            {
             
                initGridFactory();
                initGridAfterFactory();
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
                strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + txtEmpIDFactor.Text + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID;
                CSearch.ControlValidating(txtEmpIDFactor, lblEmpNameFactor, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID
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
                strSQL = "SELECT " + PrimaryName + " as UserName  FROM  [Users] where [Cancel]=0 and BranchID="+MySession.GlobalBranchID+" and [UserID]=" + txtGuidanceID.Text.ToString();
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
                string txtOrder = "";
                txtOrder = txtOrderID.Text;
                if (txtOrderID.Text != string.Empty && txtOrderID.Text != "0")
                {
                    int CommandIDTemp = 0;
                    CommandIDTemp = Comon.cInt(Lip.GetValue("select ComandID from Menu_FactoryRunCommandMaster where Cancel=0 and BranchID="+MySession.GlobalBranchID+" and TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + " and ComandID<>" + Comon.cInt(txtCommandID.Text) + " and Barcode='" + txtOrderID.Text + "'"));
                    int CommandIDThis = Comon.cInt(Lip.GetValue("select ComandID from Menu_FactoryRunCommandMaster where Cancel=0 and BranchID="+MySession.GlobalBranchID+" and TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + " and ComandID=" + Comon.cInt(txtCommandID.Text) + " and Barcode='" + txtOrderID.Text + "'"));
                    if(CommandIDTemp>0)
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
                    if (IsNewRecord )   //&& CommandIDTemp <= 0
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
            //try
            //{
            //    strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtEmployeeStokIDFactory.Text) + " And Cancel =0 ";
            //    CSearch.ControlValidating(txtEmployeeStokIDFactory, txtEmployeeStokNameFactory, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID
            //}
            //catch (Exception ex)
            //{
            //    Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            //}

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


            if (FocusedControl.Trim() == txtStoreIDFactory.Name  )
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
         
            else if (FocusedControl.Trim() == txtEmpIDFactor.Name)
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
            else if (FocusedControl.Trim() == gridControlfactroOpretion.Name)
            {

                if (GridViewBeforfactory.FocusedColumn.Name == "colItemID" || GridViewBeforfactory.FocusedColumn.Name == "col" + ItemName || GridViewBeforfactory.FocusedColumn.Name == "colBarCode")
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
                            GridViewBeforfactory.Columns[ItemName].ColumnEdit = rItem;
                            gridControlfactroOpretion.RepositoryItems.Add(rItem);

                        };
                    }
                    else
                        frm.Dispose();
                }
                else if (GridViewBeforfactory.FocusedColumn.Name == "colSizeName" || GridViewBeforfactory.FocusedColumn.Name == "colSizeID")
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


            else if (FocusedControl.Trim() == gridControlAfterFactory.Name)
            {
                
                if (GridViewAfterfactory.FocusedColumn.Name == "colItemID" || GridViewAfterfactory.FocusedColumn.Name == "col" + ItemName || GridViewAfterfactory.FocusedColumn.Name == "colBarCode")
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
                            GridViewAfterfactory.Columns[ItemName].ColumnEdit = rItem;
                            gridControlAfterFactory.RepositoryItems.Add(rItem);

                        };
                    }
                    else
                        frm.Dispose();
                }
                else if (GridViewAfterfactory.FocusedColumn.Name == "colSizeName" || GridViewAfterfactory.FocusedColumn.Name == "colSizeID")
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
                decimal TotalQTYUSed = 0;
                decimal TotalQTYIsNotUsed = 0;
                decimal TempQTY = 0;

                for (int i = 0; i <= GridViewBeforfactory.DataRowCount - 1; i++)
                {
                    if (Comon.cInt(GridViewBeforfactory.GetRowCellValue(i, "SizeID").ToString()) == 2)
                        TempQTY += Comon.cDec(Comon.cDec(GridViewBeforfactory.GetRowCellValue(i, "Debit").ToString()) / 5);
                    else
                        TempQTY += Comon.cDec(Comon.cDec(GridViewBeforfactory.GetRowCellValue(i, "Debit").ToString()));
                    //ToatlBeforFactoryQty += Comon.cDec(GridViewBeforfactory.GetRowCellValue(i, "Debit").ToString());
                }
                for (int i = 0; i <= GridViewAfterfactory.DataRowCount - 1; i++)
                {
                    if (Comon.cbool(GridViewAfterfactory.GetRowCellValue(i, "HimLost").ToString()))
                    {
                        if (Comon.cInt(GridViewAfterfactory.GetRowCellValue(i, "SizeID").ToString()) == 2)
                            TotalQTYUSed += Comon.cDec(Comon.cDec(GridViewAfterfactory.GetRowCellValue(i, "Credit").ToString()) / 5);
                        else
                            TotalQTYUSed += Comon.cDec(Comon.cDec(GridViewAfterfactory.GetRowCellValue(i, "Credit").ToString()));
                        //TotalQTYUSed += Comon.cDec(GridViewAfterfactory.GetRowCellValue(i, "Credit").ToString());
                    }
                    else
                    {
                        if (Comon.cInt(GridViewAfterfactory.GetRowCellValue(i, "SizeID").ToString()) == 2)
                            TotalQTYIsNotUsed += Comon.cDec(Comon.cDec(GridViewAfterfactory.GetRowCellValue(i, "Credit").ToString()) / 5);
                        else
                            TotalQTYIsNotUsed += Comon.cDec(Comon.cDec(GridViewAfterfactory.GetRowCellValue(i, "Credit").ToString()));
                        //TotalQTYIsNotUsed += Comon.cDec(GridViewAfterfactory.GetRowCellValue(i, "Credit").ToString());
                    }
                    if (Comon.cInt(GridViewAfterfactory.GetRowCellValue(i, "SizeID").ToString()) == 2)
                        ToatlAfterFactoryQty += Comon.cDec(Comon.cDec(GridViewAfterfactory.GetRowCellValue(i, "Credit").ToString()) / 5);
                    else
                        ToatlAfterFactoryQty += Comon.cDec(Comon.cDec(GridViewAfterfactory.GetRowCellValue(i, "Credit").ToString()));
                    //ToatlAfterFactoryQty += Comon.cDec(GridViewAfterfactory.GetRowCellValue(i, "Credit").ToString());
                }
                txtTotalBefor.Text = TempQTY.ToString();
                txtTotalAfter.Text = ToatlAfterFactoryQty.ToString();


                lblTotallostFactory.Text = Comon.cDec((TempQTY - TotalQTYIsNotUsed) - TotalQTYUSed) + "";
                txtQTYIsUsed.Text = TotalQTYUSed.ToString();
                txtQtyReturn.Text = TotalQTYIsNotUsed.ToString();
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

            else if(FocusedControl.Trim() == txtStoreIDFactory.Name)
            {
                if (!MySession.GlobalAllowChangefrmManufactoryStoreID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                 if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtStoreIDFactory, lblStoreNameFactory, "StoreID", "رقم الحساب", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtStoreIDFactory, lblStoreNameFactory, "StoreID", "Account ID", Comon.cInt(cmbBranchesID.EditValue));
            }

          


            else if(FocusedControl.Trim() == txtCommandID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCommandID, null, "CommandID", "رقم الأمر", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtCommandID, null, "CommandID", "Command ID", Comon.cInt(cmbBranchesID.EditValue));
            }
            //الاصناف

            else if (FocusedControl.Trim() == txtAccountIDFactory.Name)
            {
                if (!MySession.GlobalAllowChangefrmManufatoryAccountID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtAccountIDFactory, lblAccountNameFactory, "AccountID", "رقم الحساب", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtAccountIDFactory, lblAccountNameFactory, "AccountID", "Account ID", Comon.cInt(cmbBranchesID.EditValue));
            }
            else if (FocusedControl.Trim() == txtOrderID.Name)
            {
                if (MySession.GlobalDefaultCanRepetUseOrderOneOureMoreManufactory == true)
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, txtOrderID, txtOrderID, "OrderID", "رقم الطلب", Comon.cInt(cmbBranchesID.EditValue), "  and OrderID not in(select Barcode as OrderID from Menu_FactoryRunCommandMaster where Cancel=0 and BranchID="+MySession.GlobalBranchID+" and TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + ") ");
                    else
                        PrepareSearchQuery.Find(ref cls, txtOrderID, txtOrderID, "OrderID", "Order ID", Comon.cInt(cmbBranchesID.EditValue), "  and OrderID not in(select Barcode as OrderID  from Menu_FactoryRunCommandMaster where Cancel=0 and BranchID="+MySession.GlobalBranchID+" and TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + ") ");
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
            else if (FocusedControl.Trim() == txtEmpIDFactor.Name)
            {
                if (!MySession.GlobalAllowChangefrmManufactoryEmployeeID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtEmpIDFactor, lblEmpNameFactor, "EmployeeID", "رقـم العامل", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtEmpIDFactor, lblEmpNameFactor, "EmployeeID", "Worker ID", Comon.cInt(cmbBranchesID.EditValue));
            }
         
            //الجرايد فيو
            
            else if (FocusedControl.Trim() == gridControlfactroOpretion.Name)
            {
                if (GridViewBeforfactory.FocusedColumn.Name == "colBarCode" || GridViewBeforfactory.FocusedColumn.Name == "colItemName" || GridViewBeforfactory.FocusedColumn.Name == "colItemID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "BarCode", MySession.GlobalBranchID);
                }
                if (GridViewBeforfactory.FocusedColumn.Name == "colStoreID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "رقم الـمـســتـودع", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "Store ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                //if (GridViewBeforfactory.FocusedColumn.Name == "colItemID")
                //{
                //    if (GridViewBeforfactory.FocusedColumn == null) return;
                //    if (UserInfo.Language == iLanguage.Arabic)
                //        PrepareSearchQuery.Find(ref cls, null, null, "ItemID", "رقم المادة", Comon.cInt(cmbBranchesID.EditValue));
                //    else
                //        PrepareSearchQuery.Find(ref cls, null, null, "ItemID", "Item ID", Comon.cInt(cmbBranchesID.EditValue));
                //}
                if (GridViewBeforfactory.FocusedColumn.Name == "MachinID")
                {
                    if (GridViewBeforfactory.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "رقم المكينة", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "Machine ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                else if (GridViewBeforfactory.FocusedColumn.Name == "colSizeID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "رقـم الـوحـــده", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "Size ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                else if (GridViewBeforfactory.FocusedColumn.Name == "colEmpID")
                {
                    if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "رقـم العامل", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "Worker ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                else if (GridViewBeforfactory.FocusedColumn.Name == "colDebit")
                {
                    frmRemindQtyItem frm = new frmRemindQtyItem();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);
                        frm.Show();
                        if (GridViewBeforfactory.GetRowCellValue(GridViewBeforfactory.FocusedRowHandle, "ItemID") != null)
                            frm.SetValueToControl(GridViewBeforfactory.GetRowCellValue(GridViewBeforfactory.FocusedRowHandle, "ItemID").ToString(),txtStoreIDFactory.Text.ToString());
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
            else if (FocusedControl.Trim() == gridControlAfterFactory.Name)
            {

                if (GridViewAfterfactory.FocusedColumn.Name == "colBarCode" || GridViewAfterfactory.FocusedColumn.Name == "colItemName" || GridViewAfterfactory.FocusedColumn.Name == "colItemID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "BarCode", MySession.GlobalBranchID);
                }
                if (GridViewAfterfactory.FocusedColumn.Name == "colStoreID")
                {

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "رقم الـمـســتـودع", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "Store ID", Comon.cInt(cmbBranchesID.EditValue));
                }
               // if (GridViewAfterfactory.FocusedColumn.Name == "colItemID")
               // {
               //     if (GridViewAfterfactory.FocusedColumn == null) return;
               //     if (UserInfo.Language == iLanguage.Arabic)
               //         PrepareSearchQuery.Find(ref cls, null, null, "ItemID", "رقم المادة", Comon.cInt(cmbBranchesID.EditValue));
               //     else
               //         PrepareSearchQuery.Find(ref cls, null, null, "ItemID", "Item ID", Comon.cInt(cmbBranchesID.EditValue));
               //}
                if (GridViewAfterfactory.FocusedColumn.Name == "MachinID")
                {
                    if (GridViewAfterfactory.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "رقم المكينة", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "Machine ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                else if (GridViewAfterfactory.FocusedColumn.Name == "colSizeID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "رقـم الـوحـــده", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "Size ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                else if (GridViewAfterfactory.FocusedColumn.Name == "colEmpID")
                {
                    if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "رقـم العامل", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "Worker ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                else if (GridViewAfterfactory.FocusedColumn.Name == "colCredit")
                {
                    frmRemindQtyItem frm = new frmRemindQtyItem();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);

                        frm.Show();
                        if (GridViewAfterfactory.GetRowCellValue(GridViewAfterfactory.FocusedRowHandle, "ItemID")!=null)
                            frm.SetValueToControl(GridViewAfterfactory.GetRowCellValue(GridViewAfterfactory.FocusedRowHandle, "ItemID").ToString(),txtStoreIDFactory.Text.ToString());
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

                else if (FocusedControl == txtAccountIDFactory.Name)
                {
                    txtAccountIDFactory.Text = cls.PrimaryKeyValue.ToString();
                    txtAccountIDFactory_Validating(null, null);
                }
                else if(FocusedControl.Trim() == txtTypeOrder.Name)
                {
                    txtTypeOrder.Text = cls.PrimaryKeyValue.ToString();
                    txtTypeOrder_Validating(null, null);
                }

                else if(FocusedControl.Trim() == txtOrderID.Name)
                {
                    txtOrderID.Text = cls.PrimaryKeyValue.ToString();
                    txtOrderID_Validating(null, null);
                }
                 
                 
                
                

                //المخزن
                else if (FocusedControl == txtStoreIDFactory.Name)
                {
                    txtStoreIDFactory.Text = cls.PrimaryKeyValue.ToString();
                    txtStoreIDFactory_Validating(null, null);
                }
                

                //رقم العامل
                else if (FocusedControl ==  txtEmpIDFactor.Name)
                    {
                        txtEmpIDFactor.Text = cls.PrimaryKeyValue.ToString();
                        txtEmpFactorID_Validating(null, null);
                    }
                 
                
                else if (FocusedControl == txtAccountIDFactory.Name)
                {
                    txtAccountIDFactory.Text = cls.PrimaryKeyValue.ToString();
                    txtAccountIDFactory_Validating(null, null);
                }
                 
                //امين الخزنة
                //else if (FocusedControl == txtEmployeeStokIDFactory.Name)
                //{
                //    txtEmployeeStokIDFactory.Text = cls.PrimaryKeyValue.ToString();
                //    txtEmployeeStokID_Validating(null, null);
                //}

                //الجرايد فيو
                else if (FocusedControl.Trim() == gridControlfactroOpretion.Name)
                {
                    if (GridViewBeforfactory.FocusedColumn.Name == "colBarCode" || GridViewBeforfactory.FocusedColumn.Name == "colItemName" || GridViewBeforfactory.FocusedColumn.Name == "colItemID")
                    {
                        string Barcode = cls.PrimaryKeyValue.ToString();
                        if (Stc_itemsDAL.CheckIfStopItemUnit(Barcode, MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                        {

                            Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                            return;
                        }
                        GridViewBeforfactory.AddNewRow();
                        GridViewBeforfactory.SetRowCellValue(GridViewBeforfactory.FocusedRowHandle, GridViewBeforfactory.Columns["BarCode"], Barcode);
                        FillItemData(GridViewBeforfactory, gridControlfactroOpretion, "BarCode", "Debit", Stc_itemsDAL.GetItemData1(Barcode, UserInfo.FacilityID), "DebitDate", "DebitTime", txtAccountIDFactory);
                    
                    }
                    if (GridViewBeforfactory.FocusedColumn.Name == "colStoreID")
                    {
                        GridViewBeforfactory.SetRowCellValue(GridViewBeforfactory.FocusedRowHandle, GridViewBeforfactory.Columns["StoreID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID;
                        GridViewBeforfactory.SetRowCellValue(GridViewBeforfactory.FocusedRowHandle, GridViewBeforfactory.Columns["StoreName"], Lip.GetValue(strSQL));
                    }
                   
                    if (GridViewBeforfactory.FocusedColumn.Name == "MachinID")
                    {
                        GridViewBeforfactory.AddNewRow();
                        FileDataMachinName(GridViewBeforfactory, "DebitDate", "DebitTime", Comon.cInt(cls.PrimaryKeyValue.ToString()));
                    }
                    if (GridViewBeforfactory.FocusedColumn.Name == "colSizeID")
                    {
                        GridViewBeforfactory.SetRowCellValue(GridViewBeforfactory.FocusedRowHandle, GridViewBeforfactory.Columns["SizeID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as " + SizeName + " FROM Stc_SizingUnits WHERE SizeID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID;
                        GridViewBeforfactory.SetRowCellValue(GridViewBeforfactory.FocusedRowHandle, GridViewBeforfactory.Columns[SizeName], Lip.GetValue(strSQL));

                    }
                    if (GridViewBeforfactory.FocusedColumn.Name == "colEmpID")
                    {
                        GridViewBeforfactory.SetRowCellValue(GridViewBeforfactory.FocusedRowHandle, GridViewBeforfactory.Columns["EmpID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID;
                        GridViewBeforfactory.SetRowCellValue(GridViewBeforfactory.FocusedRowHandle, GridViewBeforfactory.Columns["EmpName"], Lip.GetValue(strSQL));
                    }
                }
                else if (FocusedControl.Trim() == gridControlAfterFactory.Name)
                {
                    if (GridViewAfterfactory.FocusedColumn.Name == "colBarCode" || GridViewAfterfactory.FocusedColumn.Name == "colItemName" || GridViewAfterfactory.FocusedColumn.Name == "colItemID")
                    {
                        string Barcode = cls.PrimaryKeyValue.ToString();
                        if (Stc_itemsDAL.CheckIfStopItemUnit(Barcode, MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                        {

                            Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                            return;
                        }
                        GridViewAfterfactory.AddNewRow();
                        GridViewAfterfactory.SetRowCellValue(GridViewAfterfactory.FocusedRowHandle, GridViewAfterfactory.Columns["BarCode"], Barcode);
                        FillItemData(GridViewAfterfactory, gridControlAfterFactory, "BarCode", "Credit", Stc_itemsDAL.GetItemData1(Barcode, UserInfo.FacilityID), "DebitDate", "DebitTime", txtAccountIDFactory);
                    }
                    if (GridViewAfterfactory.FocusedColumn.Name == "colStoreID")
                    {
                        GridViewAfterfactory.SetRowCellValue(GridViewAfterfactory.FocusedRowHandle, GridViewAfterfactory.Columns["StoreID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID;
                        GridViewAfterfactory.SetRowCellValue(GridViewAfterfactory.FocusedRowHandle, GridViewAfterfactory.Columns["StoreName"], Lip.GetValue(strSQL));

                    }

                    
                    if (GridViewAfterfactory.FocusedColumn.Name == "MachinID")
                    {
                        GridViewAfterfactory.AddNewRow();
                        FileDataMachinName(GridViewAfterfactory, "DebitDate", "DebitTime", Comon.cInt(cls.PrimaryKeyValue.ToString()));
                    }
                    if (GridViewAfterfactory.FocusedColumn.Name == "colSizeID")
                    {
                        GridViewAfterfactory.SetRowCellValue(GridViewAfterfactory.FocusedRowHandle, GridViewAfterfactory.Columns["SizeID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as " + SizeName + " FROM Stc_SizingUnits WHERE SizeID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID;
                        GridViewAfterfactory.SetRowCellValue(GridViewAfterfactory.FocusedRowHandle, GridViewAfterfactory.Columns[SizeName], Lip.GetValue(strSQL));

                    }
                    if (GridViewAfterfactory.FocusedColumn.Name == "colEmpID")
                    {
                        GridViewAfterfactory.SetRowCellValue(GridViewAfterfactory.FocusedRowHandle, GridViewAfterfactory.Columns["EmpID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID;
                        GridViewAfterfactory.SetRowCellValue(GridViewAfterfactory.FocusedRowHandle, GridViewAfterfactory.Columns["EmpName"], Lip.GetValue(strSQL));
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
             
            EnableGridView(GridViewBeforfactory, Value,1);
            EnableGridView(GridViewAfterfactory, Value,1);

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
               
                {
                    GridViewObj.Columns[col.FieldName].OptionsColumn.AllowEdit = Value;
                    GridViewObj.Columns[col.FieldName].OptionsColumn.AllowFocus = Value;
                    GridViewObj.Columns[col.FieldName].OptionsColumn.ReadOnly = !Value;
                }
                
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
                    strSQL = "SELECT TOP 1 * FROM " + Menu_FactoryRunCommandMasterDAL.TableName + " Where  TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + " and  Cancel =0 and BranchID=" + MySession.GlobalBranchID;
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
        bool IsValidGrid(GridView Grid)
        {
            double num;

            if (HasColumnErrors)
            {
                Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                return !HasColumnErrors;
            }

            Grid.MoveLast();

            int length = Grid.RowCount - 1;
            if (length <= 0)
            {
                Messages.MsgError(Messages.TitleError, Messages.msgThereIsNoRecordInput);
                return false;
            }
            for (int i = 0; i < length; i++)
            {
                foreach (GridColumn col in Grid.Columns)
                {
                    if (col.FieldName == "BarCode" || col.FieldName == "ItemID" || col.FieldName == "QTY" || col.FieldName == "SizeID")
                    {

                        var cellValue = Grid.GetRowCellValue(i, col);

                        if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                        {
                            Grid.SetColumnError(col, Messages.msgInputIsRequired);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                        if (col.FieldName == "BarCode")
                            return true;
                        else if (!(double.TryParse(cellValue.ToString(), out num)))
                        {
                            Grid.SetColumnError(col, Messages.msgInputShouldBeNumber);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                        else if (Comon.cDbl(cellValue.ToString()) <= 0)
                        {
                            Grid.SetColumnError(col, Messages.msgInputIsGreaterThanZero);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                    }
                }
            }
            return true;
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
                if (!IsValidGrid(GridViewBeforfactory))
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
         List<Manu_AllOrdersDetails> SaveOrderDetials( )
         {

             Manu_AllOrdersDetails returned = new Manu_AllOrdersDetails();
             List<Manu_AllOrdersDetails> listreturned = new List<Manu_AllOrdersDetails>();
             for (int i = 0; i <= GridViewBeforfactory.DataRowCount - 1; i++)
             {
                 returned = new Manu_AllOrdersDetails();
                 returned.ID = i + 1;
                 returned.CommandID = Comon.cInt(txtCommandID.Text);
                 returned.FacilityID = UserInfo.FacilityID;
                 returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                 returned.BarCode = GridViewBeforfactory.GetRowCellValue(i, "BarCode").ToString();
                 returned.ItemID = Comon.cInt(GridViewBeforfactory.GetRowCellValue(i, "ItemID").ToString());
                 returned.TypeStageID = Comon.cInt(cmbTypeStage.EditValue);
                 returned.SizeID = Comon.cInt(GridViewBeforfactory.GetRowCellValue(i, "SizeID").ToString());
                 returned.ArbSizeName = GridViewBeforfactory.GetRowCellValue(i, SizeName).ToString();
                 returned.EngSizeName = GridViewBeforfactory.GetRowCellValue(i, SizeName).ToString();
                 returned.ArbItemName = GridViewBeforfactory.GetRowCellValue(i, ItemName).ToString();
                 returned.EngItemName = GridViewBeforfactory.GetRowCellValue(i, ItemName).ToString();
                 returned.QTY = Comon.ConvertToDecimalQty(GridViewBeforfactory.GetRowCellValue(i, "Debit").ToString());
                 returned.CostPrice = 0;
                 returned.TotalCost = 0;
                 listreturned.Add(returned);
             }

             int LengBefore = GridViewBeforfactory.DataRowCount + 1;
             for (int i = 0; i <=GridViewAfterfactory.DataRowCount - 1; i++)
             {
                 returned = new Manu_AllOrdersDetails();
                 returned.ID = LengBefore ;
                 returned.CommandID = Comon.cInt(txtCommandID.Text);
                 returned.FacilityID = UserInfo.FacilityID;
                 returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                 returned.BarCode = GridViewAfterfactory.GetRowCellValue(i, "BarCode").ToString();
                 returned.ItemID = Comon.cInt(GridViewAfterfactory.GetRowCellValue(i, "ItemID").ToString());
                 returned.TypeStageID = Comon.cInt(cmbTypeStage.EditValue);
                 returned.SizeID = Comon.cInt(GridViewAfterfactory.GetRowCellValue(i, "SizeID").ToString());
                 returned.ArbSizeName = GridViewAfterfactory.GetRowCellValue(i, SizeName).ToString();
                 returned.EngSizeName = GridViewAfterfactory.GetRowCellValue(i, SizeName).ToString();
                 returned.ArbItemName = GridViewAfterfactory.GetRowCellValue(i, ItemName).ToString();
                 returned.EngItemName = GridViewAfterfactory.GetRowCellValue(i, ItemName).ToString();
                 returned.QTY = Comon.ConvertToDecimalQty(GridViewAfterfactory.GetRowCellValue(i, "Credit").ToString());
                 returned.ShownInNext = Comon.cbool(GridViewAfterfactory.GetRowCellValue(i, "ShownInNext").ToString());
                  returned.CostPrice =0;
                  returned.TotalCost = 0;
                 listreturned.Add(returned);
                 LengBefore++;
             }
             return listreturned;
         }
        private void Save()
        {

            {
                GridViewBeforfactory.MoveLast();
                GridViewAfterfactory.MoveLast();
            
                Menu_FactoryRunCommandMaster objRecord = new Menu_FactoryRunCommandMaster();

                objRecord.Barcode = txtOrderID.Text.ToString();
                objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                objRecord.BrandID = Comon.cInt(cmbBranchesID.EditValue);
                objRecord.Cancel = 0;
                objRecord.PeiceName = lblTypeOrderName.Text + "";
                objRecord.ComandID = Comon.cInt(txtCommandID.Text);
                objRecord.CostCenterID = 0;
                objRecord.CustomerID = Comon.cDbl(txtCustomerID.Text);
                objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue);
                objRecord.DocumentID = Comon.cInt(txtReferanceID.Text);
                objRecord.EmpFactorID = Comon.cDbl(txtEmpIDFactor.Text);
                objRecord.Posted = Comon.cInt(cmbStatus.EditValue);
                //objRecord.EmployeeID = Comon.cDbl(txtEmployeeStokIDFactory.Text);
                //objRecord.EmployeeStokID = Comon.cDbl(txtEmployeeStokIDFactory.Text);
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

                objRecord.AccountIDFactory = Comon.cDbl(txtAccountIDFactory.Text);
                objRecord.StoreIDFactory = Comon.cDbl(txtStoreIDFactory.Text);
                //objRecord.EmployeeStokIDFactory = Comon.cDbl(txtEmployeeStokIDFactory.Text);
                objRecord.EmpIDFactor = Comon.cDbl(txtEmpIDFactor.Text);

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


                #region Save Factory
                //التصنيع
                Menu_FactoryRunCommandfactory returnedGold;
                List<Menu_FactoryRunCommandfactory> listreturnedFactory = new List<Menu_FactoryRunCommandfactory>();
                int lengthfactry = GridViewBeforfactory.DataRowCount;
                int lengthAfterfactry = GridViewAfterfactory.DataRowCount;
                if (lengthfactry > 0)
                {
                    for (int i = 0; i < lengthfactry; i++)
                    {
                        returnedGold = new Menu_FactoryRunCommandfactory();
                        returnedGold.ID = i + 1;
                        returnedGold.ComandID = Comon.cInt(txtCommandID.Text.ToString());
                        //returnedGold.Credit = Comon.cDbl(GridViewBeforfactory.GetRowCellValue(i, "Credit").ToString());
                        //====حقول مضافة

                        returnedGold.StoreID = Comon.cInt(txtStoreIDFactory.Text.ToString());
                        returnedGold.StoreName = lblStoreNameFactory.Text.ToString();
                        returnedGold.BarCode= GridViewBeforfactory.GetRowCellValue(i, "BarCode").ToString();
                        returnedGold.EmpID = txtEmpIDFactor.Text.ToString();
                        returnedGold.EmpName = lblEmpNameFactor.Text.ToString();
                        returnedGold.ItemID = Comon.cInt(GridViewBeforfactory.GetRowCellValue(i, "ItemID").ToString());
                        returnedGold.ArbItemName = GridViewBeforfactory.GetRowCellValue(i, ItemName).ToString();

                        returnedGold.SizeID = Comon.cInt(GridViewBeforfactory.GetRowCellValue(i, "SizeID").ToString());
                        returnedGold.ArbSizeName = GridViewBeforfactory.GetRowCellValue(i, SizeName).ToString();

                        returnedGold.DebitTime = GridViewBeforfactory.GetRowCellValue(i, "DebitTime").ToString();
                        returnedGold.DebitDate = Comon.cDate(GridViewBeforfactory.GetRowCellValue(i, "DebitDate").ToString());
                        //====
                        returnedGold.Debit = Comon.cDbl(GridViewBeforfactory.GetRowCellValue(i, "Debit").ToString());
                       
                        returnedGold.TypeOpration = 1;
                        //returnedGold.Signature = GridViewBeforfactory.GetRowCellValue(i, "Signature").ToString();

                        returnedGold.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                        returnedGold.Cancel = 0;
                        returnedGold.UserID = UserInfo.ID;
                        returnedGold.RegDate = Comon.ConvertDateToSerial(Lip.GetServerDate());
                        returnedGold.RegTime = Comon.cDbl(Lip.GetServerTimeSerial());

                        returnedGold.ComputerInfo = UserInfo.ComputerInfo;
                        if (IsNewRecord == false)
                        {
                            returnedGold.EditUserID = UserInfo.ID;
                            returnedGold.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                            returnedGold.EditDate = Comon.ConvertDateToSerial(Lip.GetServerDate());
                            returnedGold.EditComputerInfo = UserInfo.ComputerInfo;
                        }
                        listreturnedFactory.Add(returnedGold);
                    }

                    if (lengthAfterfactry > 0)
                    {
                        for (int i = 0; i < lengthAfterfactry; i++)
                        {
                            returnedGold = new Menu_FactoryRunCommandfactory();
                            returnedGold.ID = i + 1;
                            returnedGold.ComandID = Comon.cInt(txtCommandID.Text.ToString());

                            //====حقول مضافة

                            returnedGold.StoreID = Comon.cInt(txtStoreIDFactory.Text.ToString());
                            returnedGold.StoreName = lblStoreNameFactory.Text.ToString();

                            returnedGold.EmpID = txtEmpIDFactor.Text.ToString();
                            returnedGold.EmpName = lblEmpNameFactor.Text.ToString();
                            returnedGold.ItemID = Comon.cInt(GridViewAfterfactory.GetRowCellValue(i, "ItemID").ToString());
                            returnedGold.ArbItemName = GridViewAfterfactory.GetRowCellValue(i, ItemName).ToString();
                            returnedGold.SizeID = Comon.cInt(GridViewAfterfactory.GetRowCellValue(i, "SizeID").ToString());
                            returnedGold.ArbSizeName = GridViewAfterfactory.GetRowCellValue(i, SizeName).ToString();
                            returnedGold.BarCode = GridViewAfterfactory.GetRowCellValue(i, "BarCode").ToString();
                            returnedGold.DebitTime = GridViewAfterfactory.GetRowCellValue(i, "DebitTime").ToString();
                            returnedGold.DebitDate = Comon.cDate(GridViewAfterfactory.GetRowCellValue(i, "DebitDate").ToString());
                            returnedGold.ShownInNext = Comon.cbool(GridViewAfterfactory.GetRowCellValue(i, "ShownInNext").ToString());
                            returnedGold.HimLost = Comon.cbool(GridViewAfterfactory.GetRowCellValue(i, "HimLost").ToString());
                            //====
                            returnedGold.Credit = Comon.cDbl(GridViewAfterfactory.GetRowCellValue(i, "Credit").ToString());
                            returnedGold.TypeOpration = 2;
                            //returnedGold.Signature = GridViewBeforfactory.GetRowCellValue(i, "Signature").ToString();

                            returnedGold.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                            returnedGold.Cancel = 0;
                            returnedGold.UserID = UserInfo.ID;
                            returnedGold.RegDate = Comon.ConvertDateToSerial(Lip.GetServerDate());
                            returnedGold.RegTime = Comon.cDbl(Lip.GetServerTimeSerial());

                            returnedGold.ComputerInfo = UserInfo.ComputerInfo;
                            if (IsNewRecord == false)
                            {
                                returnedGold.EditUserID = UserInfo.ID;
                                returnedGold.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                                returnedGold.EditDate = Comon.ConvertDateToSerial(Lip.GetServerDate());
                                returnedGold.EditComputerInfo = UserInfo.ComputerInfo;
                            }
                            listreturnedFactory.Add(returnedGold);
                        }

                    }

                }

                #endregion


                if (listreturnedFactory.Count > 0)
                {
                    objRecord.Menu_F_Factory = listreturnedFactory;
                    objRecord.Manu_OrderDetils = SaveOrderDetials();


                    string Result = Menu_FactoryRunCommandMasterDAL.InsertUsingXML(objRecord, IsNewRecord).ToString();
                    if (Comon.cInt(Result) > 0 && Comon.cInt(cmbStatus.EditValue)>1)
                    {
                        //أوامر الصرف والتوريد الخاص بالتصنيع
                        if (lengthfactry > 0)
                        {
                            //SaveOutOn(); //حفظ   الصرف المخزني

                            if (Comon.cInt(Result) > 0)
                            {
                                bool isNew = true;
                                DataTable dtCount = null;
                                dtCount = Stc_ItemsMoviingDAL.GetCountElementID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(Result), DocumentTypeFactoryBefore);
                                if (Comon.cInt(dtCount.Rows[0][0]) > 0)
                                    isNew = false;
                                // حفظ الحركة المخزنية 
                                int MoveID = SaveStockMoveingOut(Comon.cInt(Result));
                                if (MoveID == 0)
                                    Messages.MsgError(Messages.TitleInfo, "  خطا في حفظ الحركة المخزنية تصنيع- قبل");

                                //حفظ القيد الالي

                                long VoucherID = SaveVariousVoucherMachin(Comon.cInt(Result), isNew);
                                if (VoucherID == 0)
                                    Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                                else
                                    Lip.ExecututeSQL("Update " + Menu_FactoryRunCommandfactoryDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Menu_FactoryRunCommandfactoryDAL.PremaryKey + " = " + Result+" and BranchID="+MySession.GlobalBranchID);
                            }
                        }
                        if (lengthAfterfactry > 0)
                        {
                            //SaveInOn(); //حفظ   التوريد المخزني
                            // حفظ الحركة المخزنية 
                            if (Comon.cInt(Result) > 0)
                            {
                                bool isNew = true;
                                DataTable dtCount = Stc_ItemsMoviingDAL.GetCountElementID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(Result), DocumentTypeFactoryAfter);
                                if (Comon.cInt(dtCount.Rows[0][0]) > 0)
                                    isNew = false;
                                int MoveID = SaveStockMoveingIn(Comon.cInt(Result));
                                if (MoveID == 0)
                                    Messages.MsgError(Messages.TitleInfo, "خطا في حفظ الحركة المخزنية تصنيع - بعد ");

                                //حفظ القيد الالي
                                long VoucherID = SaveVariousVoucherMachinInOn(Comon.cInt(Result), isNew);
                                if (VoucherID == 0)
                                    Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                                else
                                    Lip.ExecututeSQL("Update " + Menu_FactoryRunCommandfactoryDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Menu_FactoryRunCommandfactoryDAL.PremaryKey + " = " + Result + " and BranchID=" + MySession.GlobalBranchID);
                            }
                        }
                        // Save The Closing 
                        //if (Comon.cInt(Result) > 0 && IsNewRecord)
                        //{
                        //    Lip.NewFields();
                        //    Lip.Table = "Manu_ArrangingClosingOrders";
                        //    Lip.AddNumericField("ID", (Comon.cInt(Lip.GetValue("select max(ID) from Manu_ArrangingClosingOrders where OrderID=" + txtOrderID.Text)) + 1).ToString());
                        //    Lip.AddNumericField("BranchID", MySession.GlobalBranchID);
                        //    Lip.AddNumericField("FacilityID", UserInfo.FacilityID);
                        //    Lip.AddStringField("OrderID", txtOrderID.Text);
                        //    Lip.AddNumericField("StageID",  cmbTypeStage.EditValue.ToString());
                        //    Lip.AddNumericField("CommandID",txtCommandID.Text);
                        //    Lip.AddNumericField("RepetID", (Comon.cInt(Lip.GetValue("select max(RepetID) from Manu_ArrangingClosingOrders where OrderID=" + txtOrderID.Text.ToString()+" and StageID="+cmbTypeStage.EditValue.ToString())) + 1));
                        //    Lip.AddNumericField("Cancel",0); 
                        //    Lip.ExecuteInsert();
                        //}


                    }
                    if (Comon.cInt(Result) > 0)
                    {

                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                        if (IsNewRecord == true)
                            DoNew();
                        else
                            txtCommandID_Validating(null, null);
                    }
                    else
                    {
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgErrorSave + " " + Result);
                    }
                }
            }
        }

        #region Save In,Out  Factory
        long SaveVariousVoucherMachin(int DocumentID, bool isNew)
        {
            int VoucherID = 0;
            long Result = 0;
            Acc_VariousVoucherMachinMaster objRecord = new Acc_VariousVoucherMachinMaster();
            objRecord.DocumentType = DocumentTypeFactoryBefore;
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " And DocumentType=" + objRecord.DocumentType + " And BranchID=" + Comon.cInt(cmbBranchesID.EditValue)));

            objRecord.VoucherID = VoucherID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = MySession.GlobalFacilityID;
           
            objRecord.VoucherDate =Comon.ConvertDateToSerial( ((DateTime) GridViewBeforfactory.GetRowCellValue(GridViewBeforfactory.DataRowCount - 1, "DebitDate")).ToString("dd/MM/yyyy")).ToString(); 
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
            returned.AccountID = Comon.cDbl(txtAccountIDFactory.Text);
            returned.VoucherID = VoucherID;
            double txtTotalQty_FactoryBefore = 0;
            for (int i = 0; i < GridViewBeforfactory.DataRowCount; i++)
            {
                txtTotalQty_FactoryBefore += Comon.cDbl(GridViewBeforfactory.GetRowCellValue(i, "Debit").ToString());
            }
            returned.DebitGold = Comon.cDbl(txtTotalQty_FactoryBefore);
            returned.Declaration = txtNotes.Text; 
            returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
            returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
            returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Debit) * Comon.cDbl(returned.CurrencyPrice)); 
            listreturned.Add(returned);

            //Credit Matirial      
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 1;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cLong(txtStoreIDFactory.Text);
            returned.VoucherID = VoucherID;
            returned.CreditGold = Comon.cDbl(txtTotalQty_FactoryBefore);
            returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
            returned.CostCenterID = 1;
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
        long SaveVariousVoucherMachinInOn(int DocumentID,bool isNew)
        {
            int VoucherID = 0;
            long Result = 0;
            Acc_VariousVoucherMachinMaster objRecord = new Acc_VariousVoucherMachinMaster();
            objRecord.DocumentType = DocumentTypeFactoryAfter;
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " And DocumentType=" + objRecord.DocumentType + " And BranchID=" + Comon.cInt(cmbBranchesID.EditValue)));

            objRecord.VoucherID = VoucherID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = MySession.GlobalFacilityID;
            //Date

            objRecord.VoucherDate = Comon.ConvertDateToSerial(((DateTime)GridViewAfterfactory.GetRowCellValue(GridViewAfterfactory.DataRowCount - 1, "DebitDate")).ToString("dd/MM/yyyy")).ToString(); 
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
            returned.AccountID = Comon.cLong(txtStoreIDFactory.Text);
            returned.VoucherID = VoucherID;
            double txtTotalQty_FactorAfter = 0;
            for (int i = 0; i < GridViewAfterfactory.DataRowCount; i++)
            {
                txtTotalQty_FactorAfter += Comon.cDbl(GridViewAfterfactory.GetRowCellValue(i, "Credit").ToString());
            }
            
            returned.DebitGold = Comon.cDbl(txtTotalQty_FactorAfter);
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = 1;

            returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
            returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
            returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Debit) * Comon.cDbl(returned.CurrencyPrice)); 
            listreturned.Add(returned);

            //Credit Matirial      
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 1;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(txtAccountIDFactory.Text);
            returned.VoucherID = VoucherID;
            returned.CreditGold = Comon.cDbl(txtTotalQty_FactorAfter);
            returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
            returned.CostCenterID =1;

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
      
        private int SaveStockMoveingOut(int DocumentID)
        {
            Stc_ItemsMoviing objRecord = new Stc_ItemsMoviing();
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.DocumentTypeID = DocumentTypeFactoryBefore;
            objRecord.MoveType = 2;
            objRecord.MoveID = 0;
            objRecord.TranseID = DocumentID;
            objRecord.Posted = Comon.cInt(cmbStatus.EditValue);
            Stc_ItemsMoviing returned;
            List<Stc_ItemsMoviing> listreturned = new List<Stc_ItemsMoviing>();
            for (int i = 0; i <= GridViewBeforfactory.DataRowCount - 1; i++)
            {
                returned = new Stc_ItemsMoviing();
                returned.ID = i + 1;
                returned.MoveDate = Comon.ConvertDateToSerial(((DateTime)GridViewBeforfactory.GetRowCellValue(i, "DebitDate")).ToString("dd/MM/yyyy")).ToString(); 

                returned.MoveID = 0;
                returned.TranseID = DocumentID;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.DocumentTypeID = DocumentTypeFactoryBefore;
                returned.MoveType = 2;
                returned.StoreID = Comon.cDbl(txtStoreIDFactory.Text.ToString());
                returned.AccountID = Comon.cDbl(txtAccountIDFactory.Text);
                returned.BarCode = GridViewBeforfactory.GetRowCellValue(i, "BarCode").ToString();
                returned.ItemID = Comon.cInt(GridViewBeforfactory.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(GridViewBeforfactory.GetRowCellValue(i, "SizeID").ToString());
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID));
                returned.QTY = Comon.cDbl(GridViewBeforfactory.GetRowCellValue(i, "Debit").ToString());
                returned.InPrice = 0;
          
                //returned.Bones = Comon.cDbl(GridViewBeforfactory.GetRowCellValue(i, "Bones").ToString());
                returned.OutPrice = Comon.cDbl(Lip.AverageUnit(Comon.cInt(returned.ItemID), Comon.cInt(returned.SizeID), Comon.cDbl(txtStoreIDFactory.Text)));
                returned.CostCenterID = 1;
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
        private int SaveStockMoveingIn(int DocumentID)
        {
            Stc_ItemsMoviing objRecord = new Stc_ItemsMoviing();
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.DocumentTypeID = DocumentTypeFactoryAfter;
            objRecord.MoveType = 1;
            objRecord.MoveID = 0;
            objRecord.TranseID = DocumentID;
            objRecord.Posted = Comon.cInt(cmbStatus.EditValue);
            Stc_ItemsMoviing returned;
            List<Stc_ItemsMoviing> listreturned = new List<Stc_ItemsMoviing>();
            for (int i = 0; i <= GridViewAfterfactory.DataRowCount - 1; i++)
            {
                returned = new Stc_ItemsMoviing();
                returned.ID = i + 1;

                returned.MoveDate = Comon.ConvertDateToSerial(((DateTime)GridViewAfterfactory.GetRowCellValue(i, "DebitDate")).ToString("dd/MM/yyyy")).ToString(); 

                returned.MoveID = 0;
                returned.TranseID = DocumentID;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.DocumentTypeID = DocumentTypeFactoryAfter;
                returned.MoveType = 1;
                returned.StoreID = Comon.cDbl(txtStoreIDFactory.Text.ToString());
                returned.AccountID = Comon.cDbl(txtAccountIDFactory.Text);
                returned.BarCode = GridViewAfterfactory.GetRowCellValue(i, "BarCode").ToString();
                returned.ItemID = Comon.cInt(GridViewAfterfactory.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(GridViewAfterfactory.GetRowCellValue(i, "SizeID").ToString());
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID + " and BranchID=" + MySession.GlobalBranchID));
                returned.QTY = Comon.cDbl(GridViewAfterfactory.GetRowCellValue(i, "Credit").ToString());
                returned.InPrice = Comon.cDbl(Lip.AverageUnit(Comon.cInt(returned.ItemID), Comon.cInt(returned.SizeID), Comon.cDbl(txtStoreIDFactory.Text)));
                //returned.Bones = Comon.cDbl(GridCastingBefore.GetRowCellValue(i, "Bones").ToString());
                returned.OutPrice = 0;
                returned.CostCenterID =1;
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
                       MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text), DocumentTypeFactoryBefore);                    
                       MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text), DocumentTypeFactoryAfter);
                    
                     if (MoveID <0)
                         Messages.MsgError(Messages.TitleInfo, "خطا في حذف الحركة  المخزنية");
                 }

                 #region Delete Voucher Machin
                 //حذف القيد الالي
                 if (Comon.cInt(Result) > 0)
                 {
                     int VoucherID = 0;
                      VoucherID = DeleteVariousVoucherMachin(Comon.cInt(txtCommandID.Text), DocumentTypeFactoryBefore);
                         if (VoucherID == 0)
                             Messages.MsgError(Messages.TitleInfo, "خطا في حذف قيد العملية تصنيع - قبل ");
                      
                     int VoucherIDAfter = 0;
                      VoucherIDAfter = DeleteVariousVoucherMachin(Comon.cInt(txtCommandID.Text), DocumentTypeFactoryAfter);
                         if (VoucherIDAfter == 0)
                             Messages.MsgError(Messages.TitleInfo, "خطا في حذف قيد العملية تصنيع -بعد");
                     
                 }
                #endregion

                #region Delete Stock IN Or Out From archive
                ////حذف التوريد والصرف من الارشيف
                //if (Comon.cInt(Result) > 0)
                //{
                //    int OutFactoryID = 0;
                //    DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(txtCommandID.Text), DocumentTypeFactoryBefore);
                //    if (dtInvoiceID.Rows.Count > 0)
                //    {
                //        OutFactoryID = DeleteInOnOROutOnBil(Comon.cInt(dtInvoiceID.Rows[0][0]), DocumentTypeFactoryBefore);
                //        if (OutFactoryID == 0)
                //            Messages.MsgError(Messages.TitleInfo, "خطا في حذف الصرف من الارشيف للعلية تصنيع- قبل  ");
                //    }
                //    int InFactoryID = 0;
                //    DataTable dtInvoiceIDAfter = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(txtCommandID.Text), DocumentTypeFactoryAfter);
                //    if (dtInvoiceIDAfter.Rows.Count > 0)
                //    {
                //        InFactoryID = DeleteInOnOROutOnBil(Comon.cInt(dtInvoiceIDAfter.Rows[0][0]), DocumentTypeFactoryAfter);
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
                

                lblAccountNameFactory.Text = "";
                txtGuidanceID.Text = "";
                txtReferanceID.Text = "";
                lblCustomerName.Text = "";
                txtCustomerID.Text = "";
                txtEmpIDFactor.Text = ""; 
                txtTotalBefor.Text = "";
                txtTotalAfter.Text = "";
                txtTypeOrder.Text = "";
                txtNotes.Text = "";
                txtOrderID.Text = "";
                lblTotallostFactory.Text = "";
                lblEmpNameFactor.Text = ""; 
                lblTypeOrderName.Text = "";
                lblGuidanceName.Text = "";
                //الحسابات
                txtAccountIDFactory.Text = "";
                txtStoreIDFactory.Text = "";
                 
                txtEmpIDFactor.Text = "";
                txtDelegateID.Text = "";
                lblDelegateName.Text = "";
                lblStoreNameFactory.Text = "";
                lblEmpNameFactor.Text = "";
                lblEmpNameFactor.Text = "";
                lblTotallostFactory.Text = "0";
                txtTotalAfter.Text = "0";
                txtTotalBefor.Text = "0";
                //
                //جريد فيو
                cmbCurency.EditValue = Comon.cInt(MySession.GlobalDefaultManufactoryCurrncyID);
                initGridFactory();
                initGridAfterFactory();
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

                strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtEmpIDFactor.Text) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID;
                CSearch.ControlValidating(txtEmpIDFactor, lblEmpNameFactor, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID

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
                strSQL = "SELECT " + PrimaryName + " as AccountName FROM Acc_Accounts WHERE AccountID =" + Comon.cDbl(txtAccountIDFactory.Text) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID;
                CSearch.ControlValidating(txtAccountIDFactory, lblAccountNameFactory, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID

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
                strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE AccountID =" + Comon.cLong(txtStoreIDFactory.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(Comon.cInt(cmbBranchesID.EditValue));
                CSearch.ControlValidating(txtStoreIDFactory, lblStoreNameFactory, strSQL);
                strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID in( Select StoreManger from Stc_Stores WHERE AccountID =" + Comon.cLong(txtStoreIDFactory.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue) + ") And Cancel =0 ";
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
            //try
            //{

            //    strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtEmployeeStokIDFactory.Text) + " And Cancel =0 ";
            //    CSearch.ControlValidating(txtEmployeeStokIDFactory, txtEmployeeStokNameFactory, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID

            //}
            //catch (Exception ex)
            //{
            //    Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            //}
        }

              

        private void GridViewBeforfactory_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName != "HimLost" && e.Column.FieldName != "ShownInNext")
            {
                e.Graphics.FillRectangle(e.Cache.GetSolidBrush(Color.Beige), e.Bounds);
                e.Graphics.DrawRectangle(e.Cache.GetPen(Color.Black, 1), e.Bounds);
                e.Appearance.DrawString(e.Cache, e.DisplayText, e.Bounds);
                e.Handled = true;

                ((GridView)sender).Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;
                ((GridView)sender).Appearance.Row.TextOptions.VAlignment = VertAlignment.Center;

            }
        }
 

    

    
   
 

        private void btnMachinResractionFactoryAfter_Click(object sender, EventArgs e)
        {
            if (IsNewRecord == true) return;
            int ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + Comon.cInt(txtCommandID.Text) + " And DocumentType=" + DocumentTypeFactoryAfter).ToString());
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

        private void btnMachinResractionFactoryBefore_Click(object sender, EventArgs e)
        {
            if (IsNewRecord == true) return;
            int ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + Comon.cInt(txtCommandID.Text) + " And DocumentType=" + DocumentTypeFactoryBefore).ToString());
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
            

        }
        private void btnPrentage_Click(object sender, EventArgs e)
        { 
            frmManufacturingPrentag frm = new frmManufacturingPrentag();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.cmbPrntageTypeID.EditValue = 1;
                frm.Show();
                frm.txtOrderID.Text = txtOrderID.Text;
                frm.txtOrderID_Validating(null, null);
            }
            else
                frm.Dispose();
        }

        private void btnCompond_Click(object sender, EventArgs e)
        {

            frmManufactoryAdditional frm = new frmManufactoryAdditional();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
                frm.txtOrderID.Text = txtOrderID.Text;
                frm.txtOrderID_Validating(null, null);
            }
            else
                frm.Dispose();
        }

        private void btnPolisheingOne_Click(object sender, EventArgs e)
        {
            frmManufacturingTalmee frm = new frmManufacturingTalmee();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.cmbPollutionTypeID.EditValue = 1;
                frm.Show();
                frm.GetValueORderID(this.txtOrderID.Text);
            }
            else
                frm.Dispose();
        }

        private void btnPolishTow_Click(object sender, EventArgs e)
        {
            frmManufacturingTalmee frm = new frmManufacturingTalmee();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.cmbPollutionTypeID.EditValue = 2;          
                frm.Show();
                frm.GetValueORderID(this.txtOrderID.Text);
            }
            else
                frm.Dispose();
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
                for (int i = 0; i <= GridViewAfterfactory.DataRowCount - 1; i++)
                {
                    dtItem.Rows.Add();
                    dtItem.Rows[i]["ID"] = i;
                    dtItem.Rows[i]["FacilityID"] = UserInfo.FacilityID;
                    dtItem.Rows[i]["BarCode"] = GridViewAfterfactory.GetRowCellValue(i, "BarCode").ToString();
                     dtItem.Rows[i]["ItemID"] = Comon.cInt(GridViewAfterfactory.GetRowCellValue(i, "ItemID").ToString());
                     DataTable dt = Lip.SelectRecord("SELECT   [GroupID]  ," + PrimaryName + "  FROM  [Stc_ItemsGroups] where Cancel=0 and [GroupID] in(select [GroupID] from Stc_Items where ItemID=" + dtItem.Rows[i]["ItemID"] + " and Cancel=0) and BranchID=" + MySession.GlobalBranchID);
                    dtItem.Rows[i]["GroupID"] = Comon.cDbl(dt.Rows[0]["GroupID"]) ;
                    dtItem.Rows[i][GroupName] = dt.Rows[0][ PrimaryName].ToString();
                   
                    dtItem.Rows[i]["SizeID"] = Comon.cInt(GridViewAfterfactory.GetRowCellValue(i, "SizeID").ToString());
                    dtItem.Rows[i][ItemName] = GridViewAfterfactory.GetRowCellValue(i, ItemName).ToString();
                    dtItem.Rows[i][SizeName] = GridViewAfterfactory.GetRowCellValue(i, SizeName).ToString();
                    dtItem.Rows[i]["QTY"] = Comon.ConvertToDecimalQty(GridViewAfterfactory.GetRowCellValue(i, "Credit").ToString());
                    //  dtItem.Rows[i]["PackingQty"] = Comon.ConvertToDecimalPrice(GridViewAfterfactory.GetRowCellValue(i, "PackingQty").ToString());
                    dtItem.Rows[i]["SalePrice"] = 0;

                    dtItem.Rows[i]["Description"] = UserInfo.Language == iLanguage.Arabic ? "تحويل من مرحلة التصنيع " : "Transfer from manufacturing";

                    dtItem.Rows[i]["StoreAccountID"] = Comon.cDbl(txtStoreIDFactory.Text);
                    dtItem.Rows[i]["StoreName"] = lblStoreNameFactory.Text.ToString();
                    dtItem.Rows[i]["Caliber"] = 18;
                    
                    dtItem.Rows[i]["CostPrice"] = Comon.ConvertToDecimalPrice(GridViewAfterfactory.GetRowCellValue(i, "CostPrice").ToString());
                    dtItem.Rows[i]["TotalCost"] =Comon.ConvertToDecimalPrice( Comon.ConvertToDecimalPrice(dtItem.Rows[i]["CostPrice"])*Comon.cDec(dtItem.Rows[i]["QTY"]));

                    dtItem.Rows[i]["Equivalen"] = 0;
                    dtItem.Rows[i]["CaliberEquivalen"] = 18;

                    dtItem.Rows[i]["Cancel"] = 0;

                }

                frm.ReadRecordFromOutScreen(dtItem);

            }
            else
                frm.Dispose();
        }

        private void btnCostOrder_Click(object sender, EventArgs e)
        {

            frmManufacturingPrentag frm = new frmManufacturingPrentag();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.cmbPrntageTypeID.EditValue = 2;
                frm.Show();
                frm.txtOrderID.Text = txtOrderID.Text;
                frm.txtOrderID_Validating(null, null);
            }
            else
                frm.Dispose();
        }

        public XtraReport Manu_CommandStage(GridView Grid)
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
                ReportName = "rptManu_FactoryCommandOpretion";
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
                rptForm.Parameters["CustomerName"].Value = lblAccountNameFactory.Text;
                rptForm.Parameters["DelegetName"].Value = lblDelegateName.Text;
                rptForm.Parameters["GuidanceName"].Value = lblGuidanceName.Text;
                rptForm.Parameters["TypeOrder"].Value = lblTypeOrderName.Text;

                rptForm.Parameters["BranchesID"].Value = cmbBranchesID.Text;
                rptForm.Parameters["BeforeStoreName"].Value = lblStoreNameFactory.Text;
                rptForm.Parameters["BeforeStoreManger"].Value = lblBeforeStoreManger.Text;
                rptForm.Parameters["CostCenterName"].Value = "";

                rptForm.Parameters["FactorName"].Value = lblEmpNameFactor.Text;
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
                rptForm.Parameters["CupsLost"].Value = lblTotallostFactory.Text;
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
                subreportBeforeCasting.ReportSource = Manu_CommandStage(GridViewBeforfactory);

                /******************** Report Factory ************************/
                XRSubreport subreportFactor = (XRSubreport)rptForm.FindControl("subRptrptManu_FactoryFactorCommendBefore", true);
                subreportFactor.Visible = IncludeHeader;
                subreportFactor.ReportSource = Manu_CommandStage(GridViewAfterfactory);


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

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            frmTransferMultipleStoresGold frm = new frmTransferMultipleStoresGold();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();

            }
            else
                frm.Dispose();
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            frmManufacturingDismantOrders frm = new frmManufacturingDismantOrders();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
                frm.txtOrderID.Text = txtOrderID.Text;
                frm.txtOrderID_Validating(null, null);
            }
            else
                frm.Dispose();
        }

        private void gridControlfactroOpretion_Click(object sender, EventArgs e)
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
        bool ChekOrderIsFoundInGrid(GridView Grid,string ColBarCode, string OrderID)
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
                        GridViewBeforfactory.AddNewRow();
                        if (ChekOrderIsFoundInGrid(GridViewBeforfactory,"BarCode", BarCode))
                        {
                            Messages.MsgAsterisk(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "الصنف موجود  لذلك لا يمكن انزاله  اكثر من مرة " : "This Item is Found Table");
                            GridViewBeforfactory.DeleteRow(rowIndex);
                            return;
                        }
                    
                        string QTY = view.GetRowCellValue(view.FocusedRowHandle, "QTY").ToString();
                        FillItemData(GridViewBeforfactory,gridControlfactroOpretion, "BarCode", "Debit", Stc_itemsDAL.GetItemData1(BarCode, UserInfo.FacilityID), "DebitDate", "DebitTime", txtAccountIDFactory, QTY);

                    
                        SendKeys.Send("\t");
                    }

                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void GridViewBeforfactory_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            rowIndex = e.RowHandle;
        }

        private void GridViewBeforfactory_DoubleClick(object sender, EventArgs e)
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
                        GridViewAfterfactory.AddNewRow();
                        if (ChekOrderIsFoundInGrid(GridViewAfterfactory,"BarCode",BarCode))
                        {
                            Messages.MsgAsterisk(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "الصنف موجود  لذلك لا يمكن انزاله  اكثر من مرة " : "This Item is Found Table");
                            GridViewAfterfactory.DeleteRow(rowIndex);
                            return;
                        }
                     
                        string QTY = view.GetRowCellValue(view.FocusedRowHandle, "Debit").ToString();
                        FillItemData(GridViewAfterfactory,gridControlAfterFactory, "BarCode", "Credit", Stc_itemsDAL.GetItemData1(BarCode, UserInfo.FacilityID), "DebitDate", "DebitTime", txtAccountIDFactory, QTY);
                        SendKeys.Send("\t");
                    }
                }
            }
            catch(Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void GridViewAfterfactory_InitNewRow(object sender, InitNewRowEventArgs e)
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

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            frmManufacturingTalmee frm = new frmManufacturingTalmee();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.cmbPollutionTypeID.EditValue = 3;
                frm.Show();
                frm.GetValueORderID(this.txtOrderID.Text);
            }
            else
                frm.Dispose();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtOrderID.Text) != true)
            {
                strSQL = "SELECT TOP 1 ComandID FROM " + Menu_FactoryRunCommandMasterDAL.TableName + " Where  TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + " and  Cancel =0 and  ComandID>" + Comon.cLong(txtCommandID.Text) + " and Barcode=" + txtOrderID.Text + " and BranchID=" + MySession.GlobalBranchID;
                int commandID = Comon.cInt(Lip.GetValue(strSQL));
                if(commandID>0)
                {
                    txtCommandID.Text = commandID.ToString();
                    txtCommandID_Validating(null, null);
                  
                }
            }         
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtOrderID.Text) != true)
            {
                strSQL = "SELECT TOP 1 ComandID FROM " + Menu_FactoryRunCommandMasterDAL.TableName + " Where  TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + " and  Cancel =0 and  ComandID<" + Comon.cLong(txtCommandID.Text) + " and BranchID="+MySession.GlobalBranchID+"  and Barcode=" + txtOrderID.Text;
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
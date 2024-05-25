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
using Edex.StockObjects.Codes;
using Edex.HR.Codes;
using Edex.StockObjects.Transactions;
using System.Globalization;

namespace Edex.Manufacturing.Codes
{
    public partial class frmManufacturingCompond : BaseForm
    {
        //list detail
        BindingList<Menu_FactoryRunCommandCompund> lstDetailCompund = new BindingList<Menu_FactoryRunCommandCompund>();
        BindingList<Menu_FactoryRunCommandCompund> lstDetailAfterCompund = new BindingList<Menu_FactoryRunCommandCompund>();

        BindingList<Menu_FactoryOrderDetails> lstOrderDetails = new BindingList<Menu_FactoryOrderDetails>();
        BindingList<Manu_AuxiliaryMaterialsDetails> lstDetailAlcadZircon = new BindingList<Manu_AuxiliaryMaterialsDetails>();
        BindingList<Stc_ItemUnits> lstDetailUnit = new BindingList<Stc_ItemUnits>();
        #region Declare 
        public int DocumentTypeCommpoundBefore = 38;
        public int DocumentTypeCommpoundAfter = 39; 
        private Menu_FactoryRunCommandMasterDAL cClass = new Menu_FactoryRunCommandMasterDAL();
        
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
        private string GroupName;
        int rowIndex = 0;
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
        public CultureInfo culture = new CultureInfo("en-US");
        #endregion
        public frmManufacturingCompond()
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
 
            this.gridControlBeforCompond.ProcessGridKey += gridControl2_ProcessGridKey;
            this.gridControlAfterCompond.ProcessGridKey += gridControl2_ProcessGridKey;

            this.gridViewBeforCompond.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridViewBeforCompond_ValidatingEditor);
            this.gridViewAfterCompond.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.GridViewAfterfactory_ValidatingEditor);
            this.gridViewBeforCompond.RowUpdated += gridViewBeforCompond_RowUpdated;
            this.gridViewAfterCompond.RowUpdated += gridViewBeforCompond_RowUpdated;

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
            FillCombo.FillComboBox(cmbCurency, "Acc_Currency", "ID", PrimaryName, "", " BranchID=" + MySession.GlobalBranchID, (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));

            FillCombo.FillComboBox(cmbTypeStage, "Manu_TypeStages", "ID", PrimaryName, "", "", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
            cmbTypeStage.EditValue = 9;
            cmbTypeStage.ReadOnly = true;
            FillCombo.FillComboBox(cmbStatus, "Manu_TypeStatus", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : " حدد الحالة"));
            cmbStatus.EditValue = MySession.GlobalDefaultProcessPostedStatus;
            cmbCurency.EditValue = MySession.GlobalDefaultSaleCurencyID;
            cmbBranchesID.EditValue = MySession.GlobalBranchID;
            cmbBranchesID.ReadOnly = MySession.GlobalAllowBranchModificationAllScreens;

        this.gridViewAfterCompond.CellValueChanging+=gridViewAfterCompond_CellValueChanging;
        EnableControlDefult();
        
        }
        void EnableControlDefult()
        {

            cmbCurency.ReadOnly = !MySession.GlobalAllowChangefrmCompundCurrncyID;
            txtCommandDate.ReadOnly = !MySession.GlobalAllowChangefrmCompundCommandDate;
            txtStoreID.ReadOnly = !MySession.GlobalAllowChangefrmCompoundStoreID;
            txtAccountID.ReadOnly = !MySession.GlobalAllowChangefrmCompoundAccountID;
            txtEmpID.ReadOnly = !MySession.GlobalAllowChangefrmCompoundEmployeeID;

        }
        void SetDefultValue()
        {
            cmbCurency.EditValue = Comon.cInt(MySession.GlobalDefaultCommpundCurrncyID);
            cmbCurency_EditValueChanged(null, null);
            txtStoreID.Text = MySession.GlobalDefaultCompoundStoreID;
            txtStoreIDFactory_Validating(null, null);
            txtAccountID.Text = MySession.GlobalDefaultCompoundAccountID;
            txtAccountIDFactory_Validating(null, null);
            txtEmpID.Text = MySession.GlobalDefaultCompoundEmployeeID;
            txtEmpIDFactor_Validating(null, null);
        }
        void gridViewAfterCompond_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
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

                        int isShow = Comon.cInt(Lip.GetValue("SELECT [ShowInOrderDetils] FROM [Stc_Items] WHERE [ItemID] = " + view.GetRowCellValue(e.RowHandle, "ItemID") + " and BranchID=" + MySession.GlobalBranchID+" AND Cancel = 0"));

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

        
        void gridViewBeforCompond_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
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
            if (this.gridViewAfterCompond.ActiveEditor is CheckEdit)
            {
                GridView view = sender as GridView;
                string ColName = view.FocusedColumn.FieldName;
                if (ColName == "ShownInNext" && Comon.cbool(e.Value) == true)
                {

                    int isShow = Comon.cInt(Lip.GetValue("SELECT [ShowInOrderDetils]  FROM  [Stc_Items] where [ItemID]=" + view.GetFocusedRowCellValue("ItemID") + " and BranchID=" + MySession.GlobalBranchID+"  and Cancel=0"));

                    if (isShow != 1)
                    {
                        //Messages.MsgWarning(Messages.TitleWorning, Messages.msgNotSelectShowInDetilsOrder);
                        e.Value = false;
                        return;
                    }

                }
            }
            if (this.gridViewAfterCompond.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                HasColumnErrors = false;
                string ColName = view.FocusedColumn.FieldName;
                int ItemID = 0;
                if (ColName == "EmpID"||ColName=="StoreID" || ColName == "SizeID" || ColName == "ItemID" || ColName == "MachinID" || ColName == "Credit" )
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
                        view.SetColumnError(gridViewAfterCompond.Columns[ColName], "");
                    }

                    if (ColName == "MachinID")
                    {

                    
                        DataTable dtGroupID = Lip.SelectRecord("Select "+PrimaryName+" from Menu_FactoryMachine Where MachineID=" + Comon.cInt(e.Value));
                        if (dtGroupID.Rows.Count > 0)
                        {
                            FileDataMachinName(gridViewAfterCompond, "DebitDate", "DebitTime", Comon.cInt(e.Value));

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
                            FillItemData(gridViewAfterCompond, gridControlAfterCompond, "BarcodCompond", "Credit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", txtAccountID);
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
                            gridViewAfterCompond.SetRowCellValue(gridViewAfterCompond.FocusedRowHandle, gridViewAfterCompond.Columns[SizeName], dtItemID.Rows[0][PrimaryName].ToString());
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

                            gridViewAfterCompond.SetRowCellValue(gridViewAfterCompond.FocusedRowHandle, gridViewAfterCompond.Columns["EmpName"], dtNameEmp.Rows[0][PrimaryName].ToString());
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
                        DataTable dtItemID = Lip.SelectRecord("Select  " + PrimaryName + " from  Stc_Stores Where Cancel=0 and BranchID=" + MySession.GlobalBranchID+"  And StoreID=" + val.ToString() + " And FacilityID=" + UserInfo.FacilityID);
                        if (dtItemID.Rows.Count > 0)
                        {
                            gridViewAfterCompond.SetFocusedRowCellValue("StoreName", dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(gridViewAfterCompond.Columns[ColName], "");

                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(gridViewAfterCompond.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }



                }
                else if (ColName == "DebitDate")
                {
                    string formattedDate = ((DateTime)e.Value).ToString("yyyy/MM/dd");
                    if (Lip.CheckDateISAvilable(formattedDate))
                    {
                        string serverDate = Lip.GetServerDate(); e.Value = serverDate;
                        gridViewAfterCompond.SetRowCellValue(gridViewAfterCompond.FocusedRowHandle, "DebitDate", serverDate);
                        return;
                    }
                }
                if (ColName == ItemName)
                {

                    DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_Items_Find  Where  LOWER (" + ItemName + ")=LOWER ('" + val.ToString() + "') and BranchID=" + MySession.GlobalBranchID);
                    if (dtItemID.Rows.Count > 0)
                    {
                        FillItemData(gridViewAfterCompond, gridControlAfterCompond, "BarcodCompond", "Credit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", ((TextEdit)txtAccountID));
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
                        gridViewAfterCompond.SetFocusedRowCellValue("MachinID", dtMachinID.Rows[0]["MachineID"].ToString());

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
                    string Str = "Select Stc_ItemUnits.SizeID from Stc_ItemUnits inner join Stc_Items on Stc_Items.ItemID=Stc_ItemUnits.ItemID and Stc_Items.BranchID=Stc_ItemUnits.BranchID left outer join  Stc_SizingUnits on Stc_ItemUnits.SizeID=Stc_SizingUnits.SizeID and  Stc_ItemUnits.BranchID=Stc_SizingUnits.BranchID Where UnitCancel=0 and BranchID=" + MySession.GlobalBranchID+" And LOWER (Stc_SizingUnits." + PrimaryName + ")=LOWER ('" + val.ToString() + "') and  Stc_Items.ItemID=" + Comon.cLong(gridViewBeforCompond.GetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "ItemID").ToString()) + "  And Stc_ItemUnits.FacilityID=" + UserInfo.FacilityID;
                    DataTable dtSizeID = Lip.SelectRecord(Str);
                    if (dtSizeID.Rows.Count > 0)
                    {

                        FillItemData(gridViewAfterCompond, gridControlAfterCompond, "BarcodCompond", "Credit", Stc_itemsDAL.GetItemDataByItemID_SizeID(Comon.cInt(gridViewAfterCompond.GetRowCellValue(gridViewAfterCompond.FocusedRowHandle, "ItemID")), Comon.cInt(dtSizeID.Rows[0][0].ToString()), UserInfo.FacilityID), "DebitDate", "DebitTime", ((TextEdit)txtAccountID));
                        gridViewAfterCompond.SetRowCellValue(gridViewAfterCompond.FocusedRowHandle, ColName, e.Value.ToString());
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundSizeForItem;
                        view.SetColumnError(gridViewBeforCompond.Columns[ColName], Messages.msgNoFoundSizeForItem);
                    }

                }
                if (ColName == "EmpName")
                {

                    DataTable dtMachinID = Lip.SelectRecord("Select  EmployeeID  from HR_EmployeeFile Where  LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') and BranchID=" + MySession.GlobalBranchID);
                    if (dtMachinID.Rows.Count > 0)
                    {
                        gridViewAfterCompond.SetFocusedRowCellValue("EmpID", dtMachinID.Rows[0]["EmployeeID"].ToString());

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
                        gridViewAfterCompond.SetFocusedRowCellValue("StoreID", dtItemID.Rows[0]["StoreID"]);
                        e.Valid = true;
                        view.SetColumnError(gridViewAfterCompond.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(gridViewAfterCompond.Columns[ColName], Messages.msgNoFoundThisItem);
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
                    if ((((GridView)Grid).Name == gridViewBeforCompond.Name))
                    {
                        totalQtyBalance = Lip.RemindQtyItemByMinUnit(Comon.cLong(dt.Rows[0]["ItemID"].ToString()), Comon.cInt(dt.Rows[0]["SizeID"]), Comon.cDbl(txtStoreID.Text));
                        {
                            decimal qtyCurrent = 0;
                            decimal QtyInCommand = Lip.GetQTYinCommandToThisItem("Menu_FactoryRunCommandCompund", "Menu_FactoryRunCommandMaster", "ComWeightSton", "ComandID", Comon.cInt(txtCommandID.Text), dt.Rows[0]["ItemID"].ToString(), " and Menu_FactoryRunCommandCompund.TypeOpration=1","BarcodCompond",SizeID:Comon.cInt(dt.Rows[0]["SizeID"].ToString()));
                            qtyCurrent = frmCadFactory.GetQTYToItemFromGridView(Grid, "ComWeightSton", 0, dt.Rows[0]["ItemID"].ToString(), Comon.cInt(dt.Rows[0]["SizeID"].ToString()), "BarcodCompond");
                       
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
                if (  (((GridView)Grid).Name ==gridViewBeforCompond.Name))
                    Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[QTYFildName], totalQtyBalance);
                else
                    Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[QTYFildName], 0);

                //RepositoryItemLookUpEdit rSize = Common.LookUpEditSize(Comon.cInt(dt.Rows[0]["ItemID"].ToString()));
                //Grid.Columns[SizeName].ColumnEdit = rSize;
                //GridControl.RepositoryItems.Add(rSize);

                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[Time], DateTime.Now.ToString("hh:mm:tt"));
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[Date], DateTime.Now.ToString("yyyy/MM/dd"));
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns["BarcodCompond"], dt.Rows[0]["BarCode"].ToString().ToUpper());
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
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns["BarcodCompond"], dt.Rows[0]["BarCode"].ToString().ToUpper());
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns["BarCode"], "");
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns["SizeID"], "");
            }
        }

        private void gridViewBeforCompond_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {

            if (this.gridViewBeforCompond.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                HasColumnErrors = false;
                string ColName = view.FocusedColumn.FieldName;
                int ItemID = 0;
                if (ColName == "EmpID"||ColName=="StoreID" || ColName == "SizeID" || ColName == "ItemID" || ColName == "MachinID" || ColName == "Credit" )
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
                        view.SetColumnError(gridViewBeforCompond.Columns[ColName], "");
                    }
                    if (ColName == "MachinID")
                    {
                        DataTable dtGroupID = Lip.SelectRecord("Select "+PrimaryName+" from Menu_FactoryMachine Where MachineID=" + Comon.cInt(e.Value));
                        if (dtGroupID.Rows.Count > 0)
                        {                        
                            e.Valid = true;
                            view.SetColumnError(gridViewBeforCompond.Columns[ColName], "");
                            gridViewBeforCompond.SetRowCellValue(gridViewBeforCompond.FocusedRowHandle, gridViewBeforCompond.Columns["ID"], gridViewBeforCompond.RowCount);
                            
                            FileDataMachinName(gridViewBeforCompond,"DebitDate", "DebitTime", Comon.cInt(e.Value));     
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
                            FillItemData(gridViewBeforCompond, gridControlBeforCompond, "BarcodCompond", "ComWeightSton", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", ((TextEdit)txtAccountID));
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
                            gridViewBeforCompond.SetRowCellValue(gridViewBeforCompond.FocusedRowHandle, gridViewBeforCompond.Columns[SizeName], dtItemID.Rows[0][PrimaryName].ToString());
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

                            gridViewBeforCompond.SetRowCellValue(gridViewBeforCompond.FocusedRowHandle, gridViewBeforCompond.Columns["EmpName"], dtNameEmp.Rows[0][PrimaryName].ToString());
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
                            gridViewBeforCompond.SetFocusedRowCellValue("StoreName", dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(gridViewBeforCompond.Columns[ColName], "");

                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(gridViewBeforCompond.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }

                }
                if (ColName == ItemName)
                {
                    DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_Items_Find  Where  LOWER (" + ItemName + ")=LOWER ('" + val.ToString() + "') and BranchID=" + MySession.GlobalBranchID);
                    if (dtItemID.Rows.Count > 0)
                    {
                        FillItemData(gridViewBeforCompond, gridControlBeforCompond, "BarcodCompond", "ComWeightSton", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", ((TextEdit)txtAccountID));
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = " الصنف غير موجود  ";
                    }
                }
                if (ColName == "ComWeightSton")
                {
                    decimal totalQtyBalance = Lip.RemindQtyItemByMinUnit(Comon.cLong(gridViewBeforCompond.GetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "ItemID").ToString()), Comon.cInt(gridViewBeforCompond.GetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "SizeID")), Comon.cDbl(txtStoreID.Text));
                    decimal QtyInCommand = Lip.GetQTYinCommandToThisItem("Menu_FactoryRunCommandCompund", "Menu_FactoryRunCommandMaster", "ComWeightSton", "ComandID", Comon.cInt(txtCommandID.Text), gridViewBeforCompond.GetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "ItemID").ToString(), " and Menu_FactoryRunCommandCompund.TypeOpration=1","BarcodCompond",SizeID:Comon.cInt( gridViewBeforCompond.GetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "SizeID").ToString()));
                    totalQtyBalance += QtyInCommand;
                    decimal qtyCurrent = 0;
                    qtyCurrent = frmCadFactory.GetQTYToItemFromGridView(gridViewBeforCompond, "ComWeightSton",Comon.cDec(val.ToString()), gridViewBeforCompond.GetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "ItemID").ToString(), Comon.cInt(gridViewBeforCompond.GetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "SizeID")), "BarcodCompond");
                       
                    if (qtyCurrent > totalQtyBalance)
                    {
                        Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheQTyinOrderisExceed);
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgQtyisNotAvilable + (totalQtyBalance - (qtyCurrent - Comon.cDec(val.ToString())));
                        view.SetColumnError(gridViewBeforCompond.Columns[ColName], Messages.msgQtyisNotAvilable + totalQtyBalance.ToString());
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
                                view.SetColumnError(gridViewBeforCompond.Columns[ColName], Messages.msgQtyisNotAvilable + totalQtyBalance.ToString());
                            }
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNotFoundAnyQtyInStore;
                            view.SetColumnError(gridViewBeforCompond.Columns[ColName], Messages.msgNotFoundAnyQtyInStore);
                        }
                    }
                }
                if (ColName == "MachineName")
                {

                    DataTable dtMachinID = Lip.SelectRecord("Select  MachineID  from Menu_FactoryMachine Where  LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') and BranchID=" + MySession.GlobalBranchID);
                    if (dtMachinID.Rows.Count > 0)
                    {
                       
                        FileDataMachinName(gridViewBeforCompond, "DebitDate", "DebitTime", Comon.cInt(dtMachinID.Rows[0]["MachineID"].ToString()));
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
                        gridViewBeforCompond.SetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "DebitDate", serverDate);
                        return;
                    }
                }
                if (ColName == SizeName)
                {
                    string Str = "Select Stc_ItemUnits.SizeID from Stc_ItemUnits inner join Stc_Items on Stc_Items.ItemID=Stc_ItemUnits.ItemID and Stc_Items.BranchID=Stc_ItemUnits.BranchID left outer join  Stc_SizingUnits on Stc_ItemUnits.SizeID=Stc_SizingUnits.SizeID and Stc_ItemUnits.BranchID=Stc_SizingUnits.BranchID Where UnitCancel=0 and BranchID=" + MySession.GlobalBranchID+" And LOWER (Stc_SizingUnits." + PrimaryName + ")=LOWER ('" + val.ToString() + "') and  Stc_Items.ItemID=" + Comon.cLong(gridViewBeforCompond.GetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "ItemID").ToString()) + "  And Stc_ItemUnits.FacilityID=" + UserInfo.FacilityID;
                    DataTable dtBarCode = Lip.SelectRecord(Str);
                    if (dtBarCode.Rows.Count > 0)
                    {
                        gridViewBeforCompond.SetFocusedRowCellValue("SizeID", dtBarCode.Rows[0]["SizeID"]);
                        frmCadFactory.SetValuseWhenChangeSizeName(gridViewBeforCompond, Comon.cLong(gridViewBeforCompond.GetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "ItemID").ToString()), Comon.cInt(dtBarCode.Rows[0]["SizeID"]), "Menu_FactoryRunCommandCompund", "Menu_FactoryRunCommandMaster", Comon.cDbl(txtStoreID.Text), Comon.cInt(txtCommandID.Text), "ComandID", Where: " and Menu_FactoryRunCommandCompund.TypeOpration=1", FildNameQTY: "ComWeightSton", FildNameBarCode: "BarcodCompond");
                        e.Valid = true;
                        view.SetColumnError(gridViewBeforCompond.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundSizeForItem;
                        view.SetColumnError(gridViewBeforCompond.Columns[ColName], Messages.msgNoFoundSizeForItem);
                    }
                }
                if (ColName == "EmpName")
                {

                    DataTable dtMachinID = Lip.SelectRecord("Select  EmployeeID  from HR_EmployeeFile Where  LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') and BranchID=" + MySession.GlobalBranchID);
                    if (dtMachinID.Rows.Count > 0)
                    {
                        gridViewBeforCompond.SetFocusedRowCellValue("EmpID", dtMachinID.Rows[0]["EmployeeID"].ToString());

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
                        gridViewBeforCompond.SetFocusedRowCellValue("StoreID", dtItemID.Rows[0]["StoreID"]);
                        e.Valid = true;
                        view.SetColumnError(gridViewBeforCompond.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(gridViewBeforCompond.Columns[ColName], Messages.msgNoFoundThisItem);
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
        public void ReadRecord(int ComandID, bool flag = false)
        {
            try
            {
                ClearFields();

                DataRecord = Menu_FactoryRunCommandMasterDAL.frmGetDataDetalByID(ComandID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID, Comon.cInt(cmbTypeStage.EditValue));

                if (DataRecord != null && DataRecord.Rows.Count > 0)
                {

                    DataRecordCommpund = Menu_FactoryRunCommandCompundDAL.frmGetDataDetalByID(Comon.cLong(ComandID), UserInfo.BRANCHID, UserInfo.FacilityID, 1);
                    DataRecordAfterCommpund = Menu_FactoryRunCommandCompundDAL.frmGetDataDetalByID(Comon.cLong(ComandID), UserInfo.BRANCHID, UserInfo.FacilityID, 2);
                    IsNewRecord = false;
                    txtReferanceID.Text = DataRecord.Rows[0]["DocumentID"].ToString();
                    txtReferanceID_Validating(null, null);
                    txtNotes.Text = DataRecord.Rows[0]["Notes"].ToString();

                    txtGuidanceID.Text = DataRecord.Rows[0]["BrandID"].ToString();
                    txtGuidanceID_Validating(null, null);
                    cmbStatus.EditValue = Comon.cInt(DataRecord.Rows[0]["Posted"].ToString());
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
                     

                    if (DataRecordCommpund != null)
                        if (DataRecordCommpund.Rows.Count > 0)
                        {
                            gridControlBeforCompond.DataSource = DataRecordCommpund;
                            lstDetailCompund.AllowNew = true;
                            lstDetailCompund.AllowEdit = true;
                            lstDetailCompund.AllowRemove = true;
                            gridViewBeforCompond.RefreshData();
                        }
                    if (DataRecordAfterCommpund != null)
                        if (DataRecordAfterCommpund.Rows.Count > 0)
                        {
                            gridControlAfterCompond.DataSource = DataRecordAfterCommpund;
                            lstDetailAfterCompund.AllowNew = true;
                            lstDetailAfterCompund.AllowEdit = true;
                            lstDetailAfterCompund.AllowRemove = true;
                            gridViewAfterCompond.RefreshData();
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
            //gridViewBeforCompond.Columns["SizeID"].Visible = false;
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

        void initGridBeforCompent()
        {
            lstDetailCompund = new BindingList<Menu_FactoryRunCommandCompund>();
            lstDetailCompund.AllowNew = true;
            lstDetailCompund.AllowEdit = true;
            lstDetailCompund.AllowRemove = true;
            gridControlBeforCompond.DataSource = lstDetailCompund;

            gridViewBeforCompond.Columns["ID"].Visible = false;
            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0 and BranchID=" + MySession.GlobalBranchID);
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlBeforCompond.RepositoryItems.Add(riComboBoxitems3);
            gridViewBeforCompond.Columns["EmpCompundName"].ColumnEdit = riComboBoxitems3;
            RepositoryItemLookUpEdit rAccountName = Common.LookUpEditAccountName();
            gridControlBeforCompond.RepositoryItems.Add(rAccountName);
            gridViewBeforCompond.Columns["FromAccountName"].ColumnEdit = rAccountName;


            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0 and BranchID=" + MySession.GlobalBranchID);
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlAfterCompond.RepositoryItems.Add(riComboBoxitems4);
            gridViewBeforCompond.Columns[ItemName].ColumnEdit = riComboBoxitems4;

            RepositoryItemLookUpEdit rSize = Common.LookUpEditSize();
            gridViewBeforCompond.Columns[SizeName].ColumnEdit = rSize;
           gridControlBeforCompond.RepositoryItems.Add(rSize);

            gridViewBeforCompond.Columns[SizeName].OptionsColumn.AllowEdit = true;
            gridViewBeforCompond.Columns[SizeName].OptionsColumn.AllowFocus = true;

            gridViewBeforCompond.Columns["ComSignature"].Visible = false;
            gridViewBeforCompond.Columns["CostPrice"].Visible = false;
            gridViewBeforCompond.Columns["DebitTime"].Visible = false;
            gridViewBeforCompond.Columns["ComandID"].Visible = false;
            gridViewBeforCompond.Columns["Cancel"].Visible = false;
            gridViewBeforCompond.Columns["BranchID"].Visible = false;
            gridViewBeforCompond.Columns["FacilityID"].Visible = false;
            gridViewBeforCompond.Columns["SizeID"].Visible = false;
            gridViewBeforCompond.Columns["RegTime"].Visible = false;
            gridViewBeforCompond.Columns["RegDate"].Visible = false;
            gridViewBeforCompond.Columns["InvoiceImage"].Visible = false;
            gridViewBeforCompond.Columns["TypeID"].Visible = false;

            gridViewBeforCompond.Columns["EditUserID"].Visible = false;
            gridViewBeforCompond.Columns["EditDate"].Visible = false;
            gridViewBeforCompond.Columns["EditTime"].Visible = false;
            gridViewBeforCompond.Columns["UserID"].Visible = false;
            gridViewBeforCompond.Columns["TypeOpration"].Visible = false;
            gridViewBeforCompond.Columns["ComputerInfo"].Visible = false;
            gridViewBeforCompond.Columns["EditComputerInfo"].Visible = false;
            gridViewBeforCompond.Columns["GoldCompundNet"].Visible = false;

            gridViewBeforCompond.Columns["FromAccountID"].Name = "FromAccountID";
            gridViewBeforCompond.Columns["BarcodCompond"].Name = "BarcodCompond";
            gridViewBeforCompond.Columns["EmpCompondID"].Name = "EmpCompondID";
            gridViewBeforCompond.Columns["EmpCompundName"].Width = 120;
            gridViewBeforCompond.Columns["FromAccountName"].Width = 120;
            gridViewBeforCompond.Columns["ComSignature"].Width = 45;
            gridViewBeforCompond.Columns["GoldDebit"].Visible = false;
            gridViewBeforCompond.Columns["FromAccountID"].Width = 120;
            gridViewBeforCompond.Columns["EmpCompondID"].Width = 120;
            gridViewBeforCompond.Columns["ComStoneNumin"].Visible = false;
            gridViewBeforCompond.Columns["ComWeightStonin"].Visible = false;
            gridViewBeforCompond.Columns["ComStoneNumout"].Visible = false;
            gridViewBeforCompond.Columns["ComWeightStonOUt"].Visible = false;
            gridViewBeforCompond.Columns["FromAccountID"].Visible = false;
            gridViewBeforCompond.Columns["EmpCompondID"].Visible = false;
            gridViewBeforCompond.Columns["FromAccountName"].Visible = false;
            gridViewBeforCompond.Columns["EmpCompundName"].Visible = false;
            gridViewBeforCompond.Columns["ComStoneNumlas"].Visible = false;
            gridViewBeforCompond.Columns["ComWeightStonLas"].Visible = false;
            gridViewBeforCompond.Columns["TypeSton"].Visible = false;
            gridViewBeforCompond.Columns["BarcodCompond"].Visible = false;

            gridViewBeforCompond.Columns["ComWeightStonAfter"].Visible = false;
            // بيانات الذهب
            gridViewBeforCompond.Columns["GoldDebit"].Visible = false;
            gridViewBeforCompond.Columns["GoldCredit"].Visible = false;
            //الاحجار المسلمة
            gridViewBeforCompond.Columns["ComStoneNumin"].Visible = false;
            gridViewBeforCompond.Columns["ComWeightStonin"].Visible = false;

            //الاحجار المرجعة
            gridViewBeforCompond.Columns["ComStoneNumout"].Visible = false;
            gridViewBeforCompond.Columns["ComWeightStonOUt"].Visible = false;

            //الاحجار الفاقدة
            gridViewBeforCompond.Columns["ComStoneNumlas"].Visible = false;
            gridViewBeforCompond.Columns["ComWeightStonLas"].Visible = false;
            //احجار مركبة
            gridViewBeforCompond.Columns["ComStoneCom"].Visible = true;
            gridViewBeforCompond.Columns["ComWeightSton"].Visible = true;

            gridViewBeforCompond.Columns["SalePrice"].Visible = false;

            gridViewBeforCompond.Columns["ShownInNext"].Visible = false;
            if (UserInfo.Language == iLanguage.Arabic)
            {

                gridViewBeforCompond.Columns["EngSizeName"].Visible = false;
                gridViewBeforCompond.Columns["EngItemName"].Visible = false;
                gridViewBeforCompond.Columns["SizeID"].Caption = "رقم الوحده";
                gridViewBeforCompond.Columns[SizeName].Caption = "الوحده";
                gridViewBeforCompond.Columns["ItemID"].Caption = "رقم الصنف";
                gridViewBeforCompond.Columns["BarcodCompond"].Caption = "الكود";
                gridViewBeforCompond.Columns["TypeSton"].Caption = "نوع الحجر ";
                gridViewBeforCompond.Columns[ItemName].Caption = "اسم الصنف";
                gridViewBeforCompond.Columns["CostPrice"].Caption = "سعر التكلفة";
                gridViewBeforCompond.Columns["FromAccountName"].Caption = "اسم الحساب ";
                gridViewBeforCompond.Columns["EmpCompundName"].Caption = "اسم المركب ";
                // بيانات الذهب
                gridViewBeforCompond.Columns["GoldDebit"].Caption = "مسلم";
                gridViewBeforCompond.Columns["GoldCredit"].Caption = "الوزن ";
                //الاحجار المسلمة
                gridViewBeforCompond.Columns["ComStoneNumin"].Caption = "عدد";
                gridViewBeforCompond.Columns["ComWeightStonin"].Caption = "الوزن";

                //الاحجار المرجعة
                gridViewBeforCompond.Columns["ComStoneNumout"].Caption = "عدد";
                gridViewBeforCompond.Columns["ComWeightStonOUt"].Caption = "الوزن";

                //الاحجار الفاقدة
                gridViewBeforCompond.Columns["ComStoneNumlas"].Caption = "عدد";
                gridViewBeforCompond.Columns["ComWeightStonLas"].Caption = "الوزن";
                //احجار مركبة
                gridViewBeforCompond.Columns["ComStoneCom"].Caption = "عدد";
                gridViewBeforCompond.Columns["ComWeightSton"].Caption = "الوزن";


                gridViewBeforCompond.Columns["ComWeightStonAfter"].Caption = "الوزن بعد";
                gridViewBeforCompond.Columns["FromAccountID"].Caption = "من حساب";

                gridViewBeforCompond.Columns["EmpCompondID"].Caption = "رقم المركب";
                gridViewBeforCompond.Columns["ComSignature"].Caption = "التوقيع";
                gridViewBeforCompond.Columns["SalePrice"].Caption = "سعر البيع";
                gridViewBeforCompond.Columns["DebitDate"].Caption = "التاريخ";
                gridViewBeforCompond.Columns["DebitTime"].Caption = "الوقت";
                
            }
            else
            {
                gridViewBeforCompond.Columns["ArbSizeName"].Visible = false;
                gridViewBeforCompond.Columns["ArbItemName"].Visible = false;
                gridViewBeforCompond.Columns["SizeID"].Caption = "Size ID";
                gridViewBeforCompond.Columns[SizeName].Caption = "Size Name";
                gridViewBeforCompond.Columns["ItemID"].Caption = "Item ID";
                gridViewBeforCompond.Columns["BarcodCompond"].Caption = "Barcod Compond";
                gridViewBeforCompond.Columns["TypeSton"].Caption = "Type Stone";
                gridViewBeforCompond.Columns[ItemName].Caption = "Item Name";
                gridViewBeforCompond.Columns["CostPrice"].Caption = "Cost Price";
                gridViewBeforCompond.Columns["FromAccountName"].Caption = "Acount Name";
                gridViewBeforCompond.Columns["EmpCompundName"].Caption = "Compund Name";
                // بيانات الذهب
                gridViewBeforCompond.Columns["GoldDebit"].Caption = "Debit";
                gridViewBeforCompond.Columns["GoldCredit"].Caption = "Credit";
                //الاحجار المسلمة
                gridViewBeforCompond.Columns["ComStoneNumin"].Caption = "Count";
                gridViewBeforCompond.Columns["ComWeightStonin"].Caption = "Weight";

                //الاحجار المرجعة
                gridViewBeforCompond.Columns["ComStoneNumout"].Caption = "Count";
                gridViewBeforCompond.Columns["ComWeightStonOUt"].Caption = "Weight";

                //الاحجار الفاقدة
                gridViewBeforCompond.Columns["ComStoneNumlas"].Caption = "Count";
                gridViewBeforCompond.Columns["ComWeightStonLas"].Caption = "Weight";
                //احجار مركبة
                gridViewBeforCompond.Columns["ComStoneCom"].Caption = "Count";
                gridViewBeforCompond.Columns["ComWeightSton"].Caption = "Weight";
                gridViewBeforCompond.Columns["ComWeightStonAfter"].Caption = "Weight After";
                gridViewBeforCompond.Columns["FromAccountID"].Caption = "From Account";

                gridViewBeforCompond.Columns["EmpCompondID"].Caption = "Compond ID";
                gridViewBeforCompond.Columns["ComSignature"].Caption = "Signature";
                gridViewBeforCompond.Columns["SalePrice"].Caption = "Sale Price";
                gridViewBeforCompond.Columns["DebitDate"].Caption = "Date";
                gridViewBeforCompond.Columns["DebitTime"].Caption = "Time";
            }

        }
         
        void initGridAfterCompent()
        {
            lstDetailAfterCompund = new BindingList<Menu_FactoryRunCommandCompund>();
            lstDetailAfterCompund.AllowNew = true;
            lstDetailAfterCompund.AllowEdit = true;
            lstDetailAfterCompund.AllowRemove = true;
            gridControlAfterCompond.DataSource = lstDetailAfterCompund;

            gridViewAfterCompond.Columns["ID"].Visible = false;
            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0 and BranchID=" + MySession.GlobalBranchID);
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlAfterCompond.RepositoryItems.Add(riComboBoxitems3);
            gridViewAfterCompond.Columns["EmpCompundName"].ColumnEdit = riComboBoxitems3;
            RepositoryItemLookUpEdit rAccountName = Common.LookUpEditAccountName();
            gridControlAfterCompond.RepositoryItems.Add(rAccountName);
            gridViewAfterCompond.Columns["FromAccountName"].ColumnEdit = rAccountName;


            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0 and BranchID=" + MySession.GlobalBranchID);
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlAfterCompond.RepositoryItems.Add(riComboBoxitems4);
            gridViewAfterCompond.Columns[ItemName].ColumnEdit = riComboBoxitems4;


            RepositoryItemLookUpEdit rSize = Common.LookUpEditSize();
            gridViewAfterCompond.Columns[SizeName].ColumnEdit = rSize;
            gridControlAfterCompond.RepositoryItems.Add(rSize);
            gridViewAfterCompond.Columns[SizeName].OptionsColumn.AllowEdit = true;
            gridViewAfterCompond.Columns[SizeName].OptionsColumn.AllowFocus = true;


            gridViewAfterCompond.Columns["ComSignature"].Visible = false;
            gridViewAfterCompond.Columns["CostPrice"].Visible = false;
            gridViewAfterCompond.Columns["DebitTime"].Visible = false;
            gridViewAfterCompond.Columns["ComandID"].Visible = false;
            gridViewAfterCompond.Columns["Cancel"].Visible = false;
            gridViewAfterCompond.Columns["BranchID"].Visible = false;
            gridViewAfterCompond.Columns["FacilityID"].Visible = false;
            gridViewAfterCompond.Columns["SizeID"].Visible = false;
            gridViewAfterCompond.Columns["RegTime"].Visible = false;
            gridViewAfterCompond.Columns["RegDate"].Visible = false;
            gridViewAfterCompond.Columns["InvoiceImage"].Visible = false;
            gridViewAfterCompond.Columns["TypeID"].Visible = false;

            gridViewAfterCompond.Columns["EditUserID"].Visible = false;
            gridViewAfterCompond.Columns["EditDate"].Visible = false;
            gridViewAfterCompond.Columns["EditTime"].Visible = false;
            gridViewAfterCompond.Columns["UserID"].Visible = false;
            gridViewAfterCompond.Columns["TypeOpration"].Visible = false;
            gridViewAfterCompond.Columns["ComputerInfo"].Visible = false;
            gridViewAfterCompond.Columns["EditComputerInfo"].Visible = false;
            gridViewAfterCompond.Columns["GoldCompundNet"].Visible = false;
            
            gridViewAfterCompond.Columns["FromAccountID"].Name = "FromAccountID";
            gridViewAfterCompond.Columns["BarcodCompond"].Name = "BarcodCompond";
            gridViewAfterCompond.Columns["EmpCompondID"].Name = "EmpCompondID";
            gridViewAfterCompond.Columns["EmpCompundName"].Width = 120;
            gridViewAfterCompond.Columns["FromAccountName"].Width = 120;
            gridViewAfterCompond.Columns["ComSignature"].Width = 45;
            gridViewAfterCompond.Columns["GoldDebit"].Visible = false;
            gridViewAfterCompond.Columns["FromAccountID"].Width = 120;
            gridViewAfterCompond.Columns["EmpCompondID"].Width = 120;
            gridViewAfterCompond.Columns["ComStoneNumin"].Visible = false;
            gridViewAfterCompond.Columns["ComWeightStonin"].Visible = false;
            gridViewAfterCompond.Columns["ComStoneNumout"].Visible = false;
            gridViewAfterCompond.Columns["ComWeightStonOUt"].Visible = false;
            gridViewAfterCompond.Columns["FromAccountID"].Visible = false;
            gridViewAfterCompond.Columns["EmpCompondID"].Visible = false;
            gridViewAfterCompond.Columns["FromAccountName"].Visible = false;
            gridViewAfterCompond.Columns["EmpCompundName"].Visible = false;
            gridViewAfterCompond.Columns["ComStoneNumlas"].Visible = false;
            gridViewAfterCompond.Columns["ComWeightStonLas"].Visible = false;
            gridViewAfterCompond.Columns["TypeSton"].Visible = false;
            gridViewAfterCompond.Columns["BarcodCompond"].Visible = false;

            gridViewAfterCompond.Columns["ComWeightStonAfter"].Visible = false;
            // بيانات الذهب
            gridViewAfterCompond.Columns["GoldDebit"].Visible = false;
            gridViewAfterCompond.Columns["GoldCredit"].Visible = false;
            //الاحجار المسلمة
            gridViewAfterCompond.Columns["ComStoneNumin"].Visible = false;
            gridViewAfterCompond.Columns["ComWeightStonin"].Visible = false;

            //الاحجار المرجعة
            gridViewAfterCompond.Columns["ComStoneNumout"].Visible = false;
            gridViewAfterCompond.Columns["ComWeightStonOUt"].Visible = false;

            //الاحجار الفاقدة
            gridViewAfterCompond.Columns["ComStoneNumlas"].Visible = false;
            gridViewAfterCompond.Columns["ComWeightStonLas"].Visible = false;
            //احجار مركبة
            gridViewAfterCompond.Columns["ComStoneCom"].Visible = true;
            gridViewAfterCompond.Columns["ComWeightSton"].Visible = true;

            gridViewAfterCompond.Columns["SalePrice"].Visible = false;
            if (UserInfo.Language == iLanguage.Arabic)
            {

                gridViewAfterCompond.Columns["EngSizeName"].Visible = false;
                gridViewAfterCompond.Columns["EngItemName"].Visible = false;
                gridViewAfterCompond.Columns["SizeID"].Caption = "رقم الوحده";
                gridViewAfterCompond.Columns[SizeName].Caption = "الوحده";
                gridViewAfterCompond.Columns["ItemID"].Caption = "رقم الصنف";
                gridViewAfterCompond.Columns["BarcodCompond"].Caption = "الكود";
                gridViewAfterCompond.Columns["TypeSton"].Caption = "نوع الحجر ";
                gridViewAfterCompond.Columns[ItemName].Caption = "اسم الصنف";
                gridViewAfterCompond.Columns["CostPrice"].Caption = "سعر التكلفة";
                gridViewAfterCompond.Columns["FromAccountName"].Caption = "اسم الحساب ";
                gridViewAfterCompond.Columns["EmpCompundName"].Caption = "اسم المركب ";
                // بيانات الذهب
                gridViewAfterCompond.Columns["GoldDebit"].Caption = "مسلم";
                gridViewAfterCompond.Columns["GoldCredit"].Caption = "الوزن ";
                //الاحجار المسلمة
                gridViewAfterCompond.Columns["ComStoneNumin"].Caption = "عدد";
                gridViewAfterCompond.Columns["ComWeightStonin"].Caption = "الوزن";

                //الاحجار المرجعة
                gridViewAfterCompond.Columns["ComStoneNumout"].Caption = "عدد";
                gridViewAfterCompond.Columns["ComWeightStonOUt"].Caption = "الوزن";

                //الاحجار الفاقدة
                gridViewAfterCompond.Columns["ComStoneNumlas"].Caption = "عدد";
                gridViewAfterCompond.Columns["ComWeightStonLas"].Caption = "الوزن";
                //احجار مركبة
                gridViewAfterCompond.Columns["ComStoneCom"].Caption = "عدد";
                gridViewAfterCompond.Columns["ComWeightSton"].Caption = "الوزن";
                gridViewAfterCompond.Columns["ComWeightStonAfter"].Caption = "الوزن بعد";
                gridViewAfterCompond.Columns["FromAccountID"].Caption = "من حساب";

                gridViewAfterCompond.Columns["EmpCompondID"].Caption = "رقم المركب";
                gridViewAfterCompond.Columns["ComSignature"].Caption = "التوقيع";
                gridViewAfterCompond.Columns["SalePrice"].Caption = "سعر البيع";
                gridViewAfterCompond.Columns["DebitDate"].Caption = "التاريخ";
                gridViewAfterCompond.Columns["DebitTime"].Caption = "الوقت";

                gridViewAfterCompond.Columns["ShownInNext"].Caption = "يظهر في التفاصيل ";
            }
            else
            {
                gridViewAfterCompond.Columns["ArbSizeName"].Visible = false;
                gridViewAfterCompond.Columns["ArbItemName"].Visible = false;
                gridViewAfterCompond.Columns["SizeID"].Caption = "Size ID";
                gridViewAfterCompond.Columns[SizeName].Caption = "Size Name";
                gridViewAfterCompond.Columns["ItemID"].Caption = "Item ID";
                gridViewAfterCompond.Columns["BarcodCompond"].Caption = "Barcod Compond";
                gridViewAfterCompond.Columns["TypeSton"].Caption = "Type Stone";
                gridViewAfterCompond.Columns[ItemName].Caption = "Item Name";
                gridViewAfterCompond.Columns["CostPrice"].Caption = "Cost Price";
                gridViewAfterCompond.Columns["FromAccountName"].Caption = "Acount Name";
                gridViewAfterCompond.Columns["EmpCompundName"].Caption = "Compund Name";
                // بيانات الذهب
                gridViewAfterCompond.Columns["GoldDebit"].Caption = "Debit";
                gridViewAfterCompond.Columns["GoldCredit"].Caption = "Credit";
                //الاحجار المسلمة
                gridViewAfterCompond.Columns["ComStoneNumin"].Caption = "Count";
                gridViewAfterCompond.Columns["ComWeightStonin"].Caption = "Weight";

                //الاحجار المرجعة
                gridViewAfterCompond.Columns["ComStoneNumout"].Caption = "Count";
                gridViewAfterCompond.Columns["ComWeightStonOUt"].Caption = "Weight";

                //الاحجار الفاقدة
                gridViewAfterCompond.Columns["ComStoneNumlas"].Caption = "Count";
                gridViewAfterCompond.Columns["ComWeightStonLas"].Caption = "Weight";
                //احجار مركبة
                gridViewAfterCompond.Columns["ComStoneCom"].Caption = "Count";
                gridViewAfterCompond.Columns["ComWeightSton"].Caption = "Weight";
                gridViewAfterCompond.Columns["ComWeightStonAfter"].Caption = "Weight After";
                gridViewAfterCompond.Columns["FromAccountID"].Caption = "From Account";

                gridViewAfterCompond.Columns["EmpCompondID"].Caption = "Compond ID";
                gridViewAfterCompond.Columns["ComSignature"].Caption = "Signature";
                gridViewAfterCompond.Columns["SalePrice"].Caption = "Sale Price";
                gridViewAfterCompond.Columns["DebitDate"].Caption = "Date";
                gridViewAfterCompond.Columns["DebitTime"].Caption = "Time";
                gridViewAfterCompond.Columns["ShownInNext"].Caption = "Shown In Next";
            }

        }



        #endregion
        private void frmManufacturingOrder_Load(object sender, EventArgs e)
        {
            try
            {
                initGridBeforCompent();
                initGridAfterCompent();
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
                CSearch.ControlValidating(txtEmployeeStokID, txtEmployeeStokName, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        
        private void frmManufacturingCommand_KeyDown(object sender, KeyEventArgs e)
        { 
            if (e.KeyCode == Keys.F3)
                Find();

            else if (e.KeyCode == Keys.F2)
                ShortcutOpen();
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
            else if (FocusedControl.Trim() == gridControlBeforCompond.Name)
            {

                if (gridViewBeforCompond.FocusedColumn.Name == "colItemID" || gridViewBeforCompond.FocusedColumn.Name == "col" + ItemName || gridViewBeforCompond.FocusedColumn.Name == "colBarCode")
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
                        frm.Dispose();
                        if (frm.IsDisposed)
                        {
                            RepositoryItemLookUpEdit rItem = Common.LookUpEditItemName();
                            gridViewBeforCompond.Columns[ItemName].ColumnEdit = rItem;
                            gridControlBeforCompond.RepositoryItems.Add(rItem);

                        };
                    }
                    else
                        frm.Dispose();
                }
                
                else if (gridViewBeforCompond.FocusedColumn.Name == "colSizeName" || gridViewBeforCompond.FocusedColumn.Name == "colSizeID")
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

            
            else if (FocusedControl.Trim() == gridControlAfterCompond.Name)
            {

                if (gridViewAfterCompond.FocusedColumn.Name == "colItemID" || gridViewAfterCompond.FocusedColumn.Name == "col" + ItemName || gridViewAfterCompond.FocusedColumn.Name == "colBarCode")
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
                            gridViewAfterCompond.Columns[ItemName].ColumnEdit = rItem;
                            gridControlAfterCompond.RepositoryItems.Add(rItem);

                        };
                    }
                    else
                        frm.Dispose();
                }
                else if (gridViewAfterCompond.FocusedColumn.Name == "colSizeName" || gridViewAfterCompond.FocusedColumn.Name == "colSizeID")
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

                decimal ToatlSton   = 0;
                decimal ToatlStonBefore = 0;
                int CountStonBefore = 0;
                int CountStonAfter = 0;
                for (int i = 0; i <= gridViewBeforCompond.DataRowCount - 1; i++)
                {
                    if (Comon.cInt(gridViewBeforCompond.GetRowCellValue(i, "SizeID").ToString()) !=2)
                        ToatlBeforFactoryQty += Comon.cDec(gridViewBeforCompond.GetRowCellValue(i, "ComWeightSton").ToString());
                    else if (Comon.cInt(gridViewBeforCompond.GetRowCellValue(i, "SizeID").ToString()) == 2)
                    {
                        ToatlStonBefore += Comon.cDec(gridViewBeforCompond.GetRowCellValue(i, "ComWeightSton").ToString());
                        CountStonBefore += Comon.cInt(gridViewBeforCompond.GetRowCellValue(i, "ComStoneCom").ToString());
                    }


                }
                for (int i = 0; i <= gridViewAfterCompond.DataRowCount - 1; i++)
                {
                    if (Comon.cInt(gridViewAfterCompond.GetRowCellValue(i, "SizeID").ToString()) != 2)
                        ToatlAfterFactoryQty += Comon.cDec(gridViewAfterCompond.GetRowCellValue(i, "ComWeightSton").ToString());

                    if (Comon.cInt(gridViewAfterCompond.GetRowCellValue(i, "SizeID").ToString()) == 2)
                    {
                        ToatlSton += Comon.cDec(gridViewAfterCompond.GetRowCellValue(i, "ComWeightSton").ToString());
                        CountStonAfter += Comon.cInt(gridViewAfterCompond.GetRowCellValue(i, "ComStoneCom").ToString());
                    }
                }

              

                decimal Qerat = Comon.ConvertToDecimalPriceTree(ToatlSton / 5);
                decimal QeratBefore = Comon.ConvertToDecimalPriceTree(ToatlStonBefore / 5);

                decimal Worder = Comon.ConvertToDecimalPriceTree(ToatlAfterFactoryQty + Qerat);

                txtCountStonBefore.Text = CountStonBefore.ToString();
                txtCountStonAfter.Text = CountStonAfter.ToString();
                txtWeghtStonBefore.Text =ToatlStonBefore.ToString();
                txtWeghtStonAfter.Text = ToatlSton.ToString();
                txtTotalQerateAfter.Text =Qerat.ToString();
                txtTotalQerateBefor.Text = QeratBefore.ToString();
                txtTotalAfter.Text = Worder.ToString();
                txtTotalBefore.Text = Comon.ConvertToDecimalPriceTree(ToatlBeforFactoryQty + QeratBefore).ToString();

                lblTotallostFactory.Text = (ToatlBeforFactoryQty - ToatlAfterFactoryQty).ToString()  ;

                txtLostCount.Text = (CountStonBefore - CountStonAfter).ToString();
                txtLostWeght.Text = (ToatlStonBefore - ToatlSton).ToString();
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
                if (!MySession.GlobalAllowChangefrmCompoundStoreID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "رقم الحساب", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "Account ID", Comon.cInt(cmbBranchesID.EditValue));
            }

     

            else if(FocusedControl.Trim() == txtCommandID.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCommandID, null, "CommandID", "رقم الأمر", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtCommandID, null, "CommandID", "Command ID", Comon.cInt(cmbBranchesID.EditValue));
            }
            //الاصناف

            else if (FocusedControl.Trim() == txtAccountID.Name)
            {
                if (!MySession.GlobalAllowChangefrmCompoundAccountID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
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
                if (!MySession.GlobalAllowChangefrmCompoundEmployeeID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtEmpID, lblEmpName, "EmployeeID", "رقـم العامل", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtEmpID, lblEmpName, "EmployeeID", "Worker ID", Comon.cInt(cmbBranchesID.EditValue));
            }
            
             
            //امين المخزن
            else if (FocusedControl.Trim() == txtEmployeeStokID.Name)
            {
                if (!MySession.GlobalAllowChangefrmCompoundEmployeeID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtEmployeeStokID, txtEmployeeStokName, "EmployeeID", "رقـم العامل", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtEmployeeStokID, txtEmployeeStokName, "EmployeeID", "Worker ID", Comon.cInt(cmbBranchesID.EditValue));
            }
             

       
            //الجرايد فيو
            
            else if (FocusedControl.Trim() == gridControlBeforCompond.Name)
            {
                if (gridViewBeforCompond.FocusedColumn.Name == "colBarcodCompond" || gridViewBeforCompond.FocusedColumn.Name == "colItemName" || gridViewBeforCompond.FocusedColumn.Name == "colItemID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "BarCode", MySession.GlobalBranchID);
                }
                if (gridViewBeforCompond.FocusedColumn.Name == "colStoreID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "رقم الـمـســتـودع", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "Store ID", Comon.cInt(cmbBranchesID.EditValue));
                }
             
              
                if (gridViewBeforCompond.FocusedColumn.Name == "MachinID")
                {
                    if (gridViewBeforCompond.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "رقم المكينة", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "Machine ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                else if (gridViewBeforCompond.FocusedColumn.Name == "colSizeID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "رقـم الـوحـــده", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "Size ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                else if (gridViewBeforCompond.FocusedColumn.Name == "colEmpID")
                {
                    if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "رقـم العامل", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "Worker ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                else if (gridViewBeforCompond.FocusedColumn.Name == "colComWeightSton")
                {
                    frmRemindQtyItem frm = new frmRemindQtyItem();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);
                        frm.Show();
                        if (gridViewBeforCompond.GetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "ItemID") != null)
                            frm.SetValueToControl(gridViewBeforCompond.GetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "ItemID").ToString(), txtStoreID.Text.ToString());
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
            else if (FocusedControl.Trim() == gridControlAfterCompond.Name)
            {
                if (gridViewAfterCompond.FocusedColumn.Name == "colBarcodCompond" || gridViewAfterCompond.FocusedColumn.Name == "colItemName" || gridViewAfterCompond.FocusedColumn.Name == "colItemID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "BarCode", MySession.GlobalBranchID);
                }
                if (gridViewAfterCompond.FocusedColumn.Name == "colStoreID")
                {

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "رقم الـمـســتـودع", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "Store ID", Comon.cInt(cmbBranchesID.EditValue));
                }
               
                if (gridViewAfterCompond.FocusedColumn.Name == "MachinID")
                {
                    if (gridViewAfterCompond.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "رقم المكينة", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "Machine ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                else if (gridViewAfterCompond.FocusedColumn.Name == "colSizeID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "رقـم الـوحـــده", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "Size ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                else if (gridViewAfterCompond.FocusedColumn.Name == "colEmpID")
                {
                    if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "رقـم العامل", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "Worker ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                else if (gridViewAfterCompond.FocusedColumn.Name == "colComWeightSton")
                {
                    frmRemindQtyItem frm = new frmRemindQtyItem();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);

                        frm.Show();
                        if (gridViewAfterCompond.GetRowCellValue(gridViewAfterCompond.FocusedRowHandle, "ItemID") != null)
                            frm.SetValueToControl(gridViewAfterCompond.GetRowCellValue(gridViewAfterCompond.FocusedRowHandle, "ItemID").ToString(), txtStoreID.Text.ToString());
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
                else if (FocusedControl.Trim() == gridControlBeforCompond.Name)
                {
                    if (gridViewBeforCompond.FocusedColumn.Name == "colBarcodCompond" || gridViewBeforCompond.FocusedColumn.Name == "colItemName" || gridViewBeforCompond.FocusedColumn.Name == "colItemID")
                    {
                        string Barcode = cls.PrimaryKeyValue.ToString();
                        if (Stc_itemsDAL.CheckIfStopItemUnit(Barcode, MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                        {
                            Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                            return;
                        }
                        gridViewBeforCompond.AddNewRow();
                        gridViewBeforCompond.SetRowCellValue(gridViewBeforCompond.FocusedRowHandle, gridViewBeforCompond.Columns["BarCode"], Barcode);
                         FillItemData(gridViewBeforCompond, gridControlBeforCompond, "BarcodCompond", "ComWeightSton", Stc_itemsDAL.GetItemData1(Barcode, UserInfo.FacilityID), "DebitDate", "DebitTime", txtAccountID);
                       
                    }
                    if (gridViewBeforCompond.FocusedColumn.Name == "colStoreID")
                    {
                        gridViewBeforCompond.SetRowCellValue(gridViewBeforCompond.FocusedRowHandle, gridViewBeforCompond.Columns["StoreID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID;
                        gridViewBeforCompond.SetRowCellValue(gridViewBeforCompond.FocusedRowHandle, gridViewBeforCompond.Columns["StoreName"], Lip.GetValue(strSQL));
                    }
                    if (gridViewBeforCompond.FocusedColumn.Name == "colItemID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_ItemUnits  Where ItemID=" + Comon.cInt(cls.PrimaryKeyValue) + " and BranchID=" + MySession.GlobalBranchID);
                        if (dtItemID.Rows.Count > 0)
                        {
                            gridViewBeforCompond.AddNewRow();
                        }
                    }
                    if (gridViewBeforCompond.FocusedColumn.Name == "MachinID")
                    {
                        gridViewBeforCompond.AddNewRow();
                        FileDataMachinName(gridViewBeforCompond, "DebitDate", "DebitTime", Comon.cInt(cls.PrimaryKeyValue.ToString()));
                    }
                    if (gridViewBeforCompond.FocusedColumn.Name == "colSizeID")
                    {
                        gridViewBeforCompond.SetRowCellValue(gridViewBeforCompond.FocusedRowHandle, gridViewBeforCompond.Columns["SizeID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as " + SizeName + " FROM Stc_SizingUnits WHERE SizeID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID;
                        gridViewBeforCompond.SetRowCellValue(gridViewBeforCompond.FocusedRowHandle, gridViewBeforCompond.Columns[SizeName], Lip.GetValue(strSQL));

                    }
                    if (gridViewBeforCompond.FocusedColumn.Name == "colEmpID")
                    {
                        gridViewBeforCompond.SetRowCellValue(gridViewBeforCompond.FocusedRowHandle, gridViewBeforCompond.Columns["EmpID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID;
                        gridViewBeforCompond.SetRowCellValue(gridViewBeforCompond.FocusedRowHandle, gridViewBeforCompond.Columns["EmpName"], Lip.GetValue(strSQL));
                    }
                }
                else if (FocusedControl.Trim() == gridControlAfterCompond.Name)
                {
                    if (gridViewAfterCompond.FocusedColumn.Name == "colStoreID")
                    {
                        gridViewAfterCompond.SetRowCellValue(gridViewAfterCompond.FocusedRowHandle, gridViewAfterCompond.Columns["StoreID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0  and BranchID=" + MySession.GlobalBranchID;
                        gridViewAfterCompond.SetRowCellValue(gridViewAfterCompond.FocusedRowHandle, gridViewAfterCompond.Columns["StoreName"], Lip.GetValue(strSQL));

                    }

                    
                    if (gridViewAfterCompond.FocusedColumn.Name == "colBarcodCompond" || gridViewAfterCompond.FocusedColumn.Name == "colItemName" || gridViewAfterCompond.FocusedColumn.Name == "colItemID")
                    {
                        string Barcode = cls.PrimaryKeyValue.ToString();
                        if (Stc_itemsDAL.CheckIfStopItemUnit(Barcode, MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                        {
                            Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                            return;
                        }
                        gridViewAfterCompond.AddNewRow();
                        gridViewAfterCompond.SetRowCellValue(gridViewAfterCompond.FocusedRowHandle, gridViewAfterCompond.Columns["BarCode"], Barcode);
                        FillItemData(gridViewAfterCompond, gridControlBeforCompond, "BarcodCompond", "ComWeightSton", Stc_itemsDAL.GetItemData1(Barcode, UserInfo.FacilityID), "DebitDate", "DebitTime", txtAccountID);

                    }
                    if (gridViewAfterCompond.FocusedColumn.Name == "MachinID")
                    {
                        gridViewAfterCompond.AddNewRow();
                        FileDataMachinName(gridViewAfterCompond, "DebitDate", "DebitTime", Comon.cInt(cls.PrimaryKeyValue.ToString()));
                    }
                    if (gridViewAfterCompond.FocusedColumn.Name == "colSizeID")
                    {
                        gridViewAfterCompond.SetRowCellValue(gridViewAfterCompond.FocusedRowHandle, gridViewAfterCompond.Columns["SizeID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as " + SizeName + " FROM Stc_SizingUnits WHERE SizeID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID;
                        gridViewAfterCompond.SetRowCellValue(gridViewAfterCompond.FocusedRowHandle, gridViewAfterCompond.Columns[SizeName], Lip.GetValue(strSQL));

                    }
                    if (gridViewAfterCompond.FocusedColumn.Name == "colEmpID")
                    {
                        gridViewAfterCompond.SetRowCellValue(gridViewAfterCompond.FocusedRowHandle, gridViewAfterCompond.Columns["EmpID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(cls.PrimaryKeyValue.ToString()) + " And Cancel =0  and BranchID=" + MySession.GlobalBranchID;
                        gridViewAfterCompond.SetRowCellValue(gridViewAfterCompond.FocusedRowHandle, gridViewAfterCompond.Columns["EmpName"], Lip.GetValue(strSQL));
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
             
            EnableGridView(gridViewBeforCompond, Value,1);
            EnableGridView(gridViewAfterCompond, Value,1);
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
                 ReportName = "rptManu_FactoryCombondOpretion";
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

                 rptForm.Parameters["TotalQTY"].Value = txtTotalQerateBefor.Text;
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
                 subreportBeforeCasting.ReportSource = Manu_CommandStage(gridViewBeforCompond);

                 /******************** Report Factory ************************/
                 XRSubreport subreportFactor = (XRSubreport)rptForm.FindControl("subRptrptManu_FactoryFactorCommendBefore", true);
                 subreportFactor.Visible = IncludeHeader;
                 subreportFactor.ReportSource = Manu_CommandStage(gridViewAfterCompond);


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
        #endregion

         List<Manu_AllOrdersDetails> SaveOrderDetials()
         {

             Manu_AllOrdersDetails returned = new Manu_AllOrdersDetails();
             List<Manu_AllOrdersDetails> listreturned = new List<Manu_AllOrdersDetails>();
             for (int i = 0; i <= gridViewBeforCompond.DataRowCount - 1; i++)
             {
                 returned = new Manu_AllOrdersDetails();
                 returned.ID = i + 1;
                 returned.CommandID = Comon.cInt(txtCommandID.Text);
                 returned.FacilityID = UserInfo.FacilityID;
                 returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                 returned.BarCode = gridViewBeforCompond.GetRowCellValue(i, "BarcodCompond").ToString();
                 returned.ItemID = Comon.cInt(gridViewBeforCompond.GetRowCellValue(i, "ItemID").ToString());
                 returned.TypeStageID = Comon.cInt(cmbTypeStage.EditValue);
                 returned.SizeID = Comon.cInt(gridViewBeforCompond.GetRowCellValue(i, "SizeID").ToString());
                 returned.ArbSizeName = gridViewBeforCompond.GetRowCellValue(i, SizeName).ToString();
                 returned.EngSizeName = gridViewBeforCompond.GetRowCellValue(i, SizeName).ToString();
                 returned.ArbItemName = gridViewBeforCompond.GetRowCellValue(i, ItemName).ToString();
                 returned.EngItemName = gridViewBeforCompond.GetRowCellValue(i, ItemName).ToString();
                 returned.QTY = Comon.ConvertToDecimalQty(gridViewBeforCompond.GetRowCellValue(i, "ComWeightSton").ToString());
                 returned.CostPrice = 0;
                 returned.TotalCost = 0;
                 listreturned.Add(returned);
             }            
             int LengBefore = gridViewBeforCompond.DataRowCount + 1;
             for (int i = 0; i <= gridViewAfterCompond.DataRowCount - 1; i++)
             {
                 returned = new Manu_AllOrdersDetails();
                 returned.ID = LengBefore;
                 returned.CommandID = Comon.cInt(txtCommandID.Text);
                 returned.FacilityID = UserInfo.FacilityID;
                 returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                 returned.BarCode = gridViewAfterCompond.GetRowCellValue(i, "BarcodCompond").ToString();
                 returned.ItemID = Comon.cInt(gridViewAfterCompond.GetRowCellValue(i, "ItemID").ToString());
                 returned.TypeStageID = Comon.cInt(cmbTypeStage.EditValue);
                 returned.SizeID = Comon.cInt(gridViewAfterCompond.GetRowCellValue(i, "SizeID").ToString());
                 returned.ArbSizeName = gridViewAfterCompond.GetRowCellValue(i, SizeName).ToString();
                 returned.EngSizeName = gridViewAfterCompond.GetRowCellValue(i, SizeName).ToString();
                 returned.ArbItemName = gridViewAfterCompond.GetRowCellValue(i, ItemName).ToString();
                 returned.EngItemName = gridViewAfterCompond.GetRowCellValue(i, ItemName).ToString();
                 returned.QTY = Comon.ConvertToDecimalQty(gridViewAfterCompond.GetRowCellValue(i, "ComWeightSton").ToString());
                 returned.ShownInNext = Comon.cbool(gridViewAfterCompond.GetRowCellValue(i, "ShownInNext").ToString());
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
                gridViewBeforCompond.MoveLast();
                gridViewAfterCompond.MoveLast();
                Menu_FactoryRunCommandMaster objRecord = new Menu_FactoryRunCommandMaster();
                objRecord.Barcode = txtOrderID.Text.ToString();
                objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                objRecord.BrandID = Comon.cInt(cmbBranchesID.EditValue);
                objRecord.Cancel = 0;
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
                objRecord.TypeStageID = Comon.cInt(cmbTypeStage.EditValue);

                objRecord.Posted = Comon.cInt(cmbStatus.EditValue);
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


                #region Save Compund
                Menu_FactoryRunCommandCompund returnedCompund;
                List<Menu_FactoryRunCommandCompund> listreturnedCompund = new List<Menu_FactoryRunCommandCompund>();
                int lengthCompund = gridViewBeforCompond.DataRowCount;
                int lengthAfterCompund = gridViewAfterCompond.DataRowCount;
                if (lengthCompund > 0)
                {

                    for (int i = 0; i <=lengthCompund-1; i++)
                    {
                        if (gridViewBeforCompond.GetRowCellValue(i, "BarcodCompond") == null)
                        {
                            Messages.MsgError(Messages.msgErrorSave, "الرجاء إدخال باركود القطعة للتركيب ");
                             

                        }
                        else
                        {
                            returnedCompund = new Menu_FactoryRunCommandCompund();
                            returnedCompund.ID = i + 1;
                            returnedCompund.ComandID = Comon.cInt(txtCommandID.Text.ToString());
                            returnedCompund.BarcodCompond = gridViewBeforCompond.GetRowCellValue(i, "BarcodCompond").ToString();
                            returnedCompund.ArbItemName = gridViewBeforCompond.GetRowCellValue(i, ItemName).ToString();
                            returnedCompund.EngItemName = gridViewBeforCompond.GetRowCellValue(i, ItemName).ToString();
                            returnedCompund.EngSizeName = gridViewBeforCompond.GetRowCellValue(i, SizeName).ToString();
                            returnedCompund.ArbSizeName = gridViewBeforCompond.GetRowCellValue(i, SizeName).ToString();
                            returnedCompund.SizeID = Comon.cInt(gridViewBeforCompond.GetRowCellValue(i, "SizeID").ToString());
                            returnedCompund.ItemID = Comon.cInt(gridViewBeforCompond.GetRowCellValue(i, "ItemID").ToString());
                            returnedCompund.DebitTime = gridViewBeforCompond.GetRowCellValue(i, "DebitTime").ToString();
                            returnedCompund.DebitDate = Comon.cDate(gridViewBeforCompond.GetRowCellValue(i, "DebitDate").ToString());
                            //returnedCompund.TypeSton = gridViewBeforCompond.GetRowCellValue(i, "TypeSton").ToString();
                            returnedCompund.CostPrice = Comon.cDec(gridViewBeforCompond.GetRowCellValue(i, "CostPrice").ToString());
                            returnedCompund.TypeID = Comon.cInt(gridViewBeforCompond.GetRowCellValue(i, "TypeID").ToString());
                            returnedCompund.GoldDebit = Comon.cLong(gridViewBeforCompond.GetRowCellValue(i, "GoldDebit").ToString());
                            returnedCompund.GoldCredit = Comon.cLong(gridViewBeforCompond.GetRowCellValue(i, "GoldCredit").ToString());
                            returnedCompund.ComStoneNumin = Comon.cDbl(gridViewBeforCompond.GetRowCellValue(i, "ComStoneNumin").ToString());
                            returnedCompund.ComWeightStonin = Comon.cLong(gridViewBeforCompond.GetRowCellValue(i, "ComWeightStonin").ToString());
                            returnedCompund.ComStoneNumout = Comon.cLong(gridViewBeforCompond.GetRowCellValue(i, "ComStoneNumout").ToString());
                            returnedCompund.ComWeightStonOUt = Comon.cDbl(gridViewBeforCompond.GetRowCellValue(i, "ComWeightStonOUt").ToString());

                            returnedCompund.ComStoneNumlas = Comon.cLong(gridViewBeforCompond.GetRowCellValue(i, "ComStoneNumlas").ToString());
                            returnedCompund.ComWeightStonLas = Comon.cDbl(gridViewBeforCompond.GetRowCellValue(i, "ComWeightStonLas").ToString());

                            returnedCompund.ComStoneCom = Comon.cLong(gridViewBeforCompond.GetRowCellValue(i, "ComStoneCom").ToString());
                            returnedCompund.ComWeightSton = Comon.cDec(gridViewBeforCompond.GetRowCellValue(i, "ComWeightSton").ToString());
                            
                            //returnedCompund.FromAccountID = Comon.cLong(gridViewBeforCompond.GetRowCellValue(i, "FromAccountID").ToString());
                            //returnedCompund.EmpCompondID = Comon.cLong(gridViewBeforCompond.GetRowCellValue(i, "EmpCompondID").ToString());

                            //returnedCompund.FromAccountName =  gridViewBeforCompond.GetRowCellValue(i, "FromAccountName").ToString();
                            //returnedCompund.EmpCompundName =  gridViewBeforCompond.GetRowCellValue(i, "EmpCompundName").ToString();

                            returnedCompund.SalePrice = Comon.cLong(gridViewBeforCompond.GetRowCellValue(i, "SalePrice").ToString());
                            returnedCompund.FacilityID = UserInfo.FacilityID;
                            returnedCompund.BranchID = UserInfo.BRANCHID;
                            returnedCompund.Cancel = 0;
                            returnedCompund.TypeOpration = 1;
                            //  returnedCompund.ComSignature = gridViewBeforCompond.GetRowCellValue(i, "ComSignature").ToString();
                            returnedCompund.RegDate = Comon.ConvertDateToSerial(Lip.GetServerDate());
                            returnedCompund.RegTime = Comon.cDbl(Lip.GetServerTimeSerial());
                            returnedCompund.ComputerInfo = UserInfo.ComputerInfo;
                            returnedCompund.UserID = UserInfo.ID;
                            if (IsNewRecord == false)
                            {

                                returnedCompund.EditUserID = UserInfo.ID;
                                returnedCompund.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                                returnedCompund.EditDate = Comon.ConvertDateToSerial(Lip.GetServerDate());
                                returnedCompund.EditComputerInfo = UserInfo.ComputerInfo;
                            }
                            listreturnedCompund.Add(returnedCompund);
                        }

                    }
                    if (lengthAfterCompund > 0)
                    {

                        for (int i = 0; i <=lengthAfterCompund-1; i++)
                        {
                            if (gridViewAfterCompond.GetRowCellValue(i, "BarcodCompond") == null)
                            {
                                Messages.MsgError(Messages.msgErrorSave, "الرجاء إدخال باركود القطعة للتركيب ");

                            }
                            else
                            {
                                returnedCompund = new Menu_FactoryRunCommandCompund();
                                returnedCompund.ID = i + 1;
                                returnedCompund.ComandID = Comon.cInt(txtCommandID.Text.ToString());
                                returnedCompund.BarcodCompond = gridViewAfterCompond.GetRowCellValue(i, "BarcodCompond").ToString();
                                returnedCompund.ArbItemName = gridViewAfterCompond.GetRowCellValue(i, ItemName).ToString();
                                returnedCompund.EngItemName = gridViewAfterCompond.GetRowCellValue(i, ItemName).ToString();
                                returnedCompund.EngSizeName = gridViewAfterCompond.GetRowCellValue(i, SizeName).ToString();
                                returnedCompund.ArbSizeName = gridViewAfterCompond.GetRowCellValue(i, SizeName).ToString();
                                returnedCompund.SizeID = Comon.cInt(gridViewAfterCompond.GetRowCellValue(i, "SizeID").ToString());
                                returnedCompund.ItemID = Comon.cInt(gridViewAfterCompond.GetRowCellValue(i, "ItemID").ToString());
                                returnedCompund.DebitTime = gridViewAfterCompond.GetRowCellValue(i, "DebitTime").ToString();
                                returnedCompund.DebitDate = Comon.cDate(gridViewAfterCompond.GetRowCellValue(i, "DebitDate").ToString());
                                //returnedCompund.TypeSton = gridViewAfterCompond.GetRowCellValue(i, "TypeSton").ToString();
                                returnedCompund.CostPrice = Comon.cDec(gridViewAfterCompond.GetRowCellValue(i, "CostPrice").ToString());
                                returnedCompund.TypeID = Comon.cInt(gridViewAfterCompond.GetRowCellValue(i, "TypeID").ToString());
                                returnedCompund.GoldDebit = Comon.cLong(gridViewAfterCompond.GetRowCellValue(i, "GoldDebit").ToString());
                                returnedCompund.GoldCredit = Comon.cLong(gridViewAfterCompond.GetRowCellValue(i, "GoldCredit").ToString());

                                returnedCompund.ComStoneNumin = Comon.cDbl(gridViewAfterCompond.GetRowCellValue(i, "ComStoneNumin").ToString());
                                returnedCompund.ComWeightStonin = Comon.cLong(gridViewAfterCompond.GetRowCellValue(i, "ComWeightStonin").ToString());

                                returnedCompund.ComStoneNumout = Comon.cLong(gridViewAfterCompond.GetRowCellValue(i, "ComStoneNumout").ToString());
                                returnedCompund.ComWeightStonOUt = Comon.cDbl(gridViewAfterCompond.GetRowCellValue(i, "ComWeightStonOUt").ToString());

                                returnedCompund.ComStoneNumlas = Comon.cLong(gridViewAfterCompond.GetRowCellValue(i, "ComStoneNumlas").ToString());
                                returnedCompund.ComWeightStonLas = Comon.cDbl(gridViewAfterCompond.GetRowCellValue(i, "ComWeightStonLas").ToString());

                                returnedCompund.ComStoneCom = Comon.cLong(gridViewAfterCompond.GetRowCellValue(i, "ComStoneCom").ToString());
                                returnedCompund.ComWeightSton = Comon.cDec(gridViewAfterCompond.GetRowCellValue(i, "ComWeightSton").ToString());
                                returnedCompund.ShownInNext = Comon.cbool(gridViewAfterCompond.GetRowCellValue(i, "ShownInNext").ToString());
                                returnedCompund.ComWeightStonAfter = Comon.cDec(gridViewAfterCompond.GetRowCellValue(i, "ComWeightStonAfter").ToString());
                                //returnedCompund.FromAccountID = Comon.cLong(gridViewAfterCompond.GetRowCellValue(i, "FromAccountID").ToString());
                                //returnedCompund.EmpCompondID = Comon.cLong(gridViewAfterCompond.GetRowCellValue(i, "EmpCompondID").ToString());

                                //returnedCompund.FromAccountName = gridViewAfterCompond.GetRowCellValue(i, "FromAccountName").ToString();
                                //returnedCompund.EmpCompundName = gridViewAfterCompond.GetRowCellValue(i, "EmpCompundName").ToString();

                                returnedCompund.SalePrice = Comon.cLong(gridViewAfterCompond.GetRowCellValue(i, "SalePrice").ToString());
                                returnedCompund.FacilityID = UserInfo.FacilityID;
                                returnedCompund.BranchID = UserInfo.BRANCHID;
                                returnedCompund.Cancel = 0;
                                returnedCompund.TypeOpration = 2;
                                //  returnedCompund.ComSignature = gridViewBeforCompond.GetRowCellValue(i, "ComSignature").ToString();
                                returnedCompund.RegDate = Comon.ConvertDateToSerial(Lip.GetServerDate());
                                returnedCompund.RegTime = Comon.cDbl(Lip.GetServerTimeSerial());
                                returnedCompund.ComputerInfo = UserInfo.ComputerInfo;
                                returnedCompund.UserID = UserInfo.ID;
                                if (IsNewRecord == false)
                                {
                                    returnedCompund.EditUserID = UserInfo.ID;
                                    returnedCompund.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                                    returnedCompund.EditDate = Comon.ConvertDateToSerial(Lip.GetServerDate());
                                    returnedCompund.EditComputerInfo = UserInfo.ComputerInfo;
                                }
                                listreturnedCompund.Add(returnedCompund);
                            }
                        }
                    }
                }
                #endregion

                if (listreturnedCompund.Count > 0)
                {
                    objRecord.Menu_F_Compund = listreturnedCompund;

                    objRecord.Manu_OrderDetils = SaveOrderDetials();

                    string Result = Menu_FactoryRunCommandMasterDAL.InsertUsingXML(objRecord, IsNewRecord).ToString();
                    if (Comon.cInt(Result) > 0 && Comon.cInt(cmbStatus.EditValue)>1)
                    {
                        //أوامر الصرف والتوريد الخاص بالتصنيع
                        if (lengthCompund > 0)
                        {
                            //أوامر الصرف والتوريد الخاص بالبرنتاج
                            //SaveOutOnBrntage(); //حفظ   الصرف المخزني
                            // حفظ الحركة المخزنية 
                            if (Comon.cInt(Result) > 0)
                            {
                                bool isNew = true;
                                DataTable dtCount = null;
                                dtCount = Stc_ItemsMoviingDAL.GetCountElementID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(Result), DocumentTypeCommpoundBefore);
                                if (Comon.cInt(dtCount.Rows[0][0]) > 0)
                                    isNew = false;

                                int MoveID = SaveStockMoveingBrntageOut(Comon.cInt(Result));
                                if (MoveID == 0)
                                    Messages.MsgError(Messages.TitleInfo, "خطا في حفظ الحركة المخزنية برنتاج - قبل ");

                                //حفظ القيد الالي
                                long VoucherID = SaveVariousVoucherMachinBrntage(Comon.cInt(Result), isNew);
                                if (VoucherID == 0)
                                    Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                                else
                                    Lip.ExecututeSQL("Update " + Menu_FactoryRunCommandPrentagAndPulishnDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Menu_FactoryRunCommandPrentagAndPulishnDAL.PremaryKey + " = " + Result + " and BranchID=" + MySession.GlobalBranchID);

                            }
                        }
                        if (lengthAfterCompund > 0)
                        {
                            //SaveInOnBrntage(); //حفظ   التوريد المخزني
                            // حفظ الحركة المخزنية 
                            if (Comon.cInt(Result) > 0)
                            {
                                bool isNew = true;
                                DataTable dtCount = null;
                                dtCount = Stc_ItemsMoviingDAL.GetCountElementID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(Result), DocumentTypeCommpoundAfter);
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
            objRecord.DocumentType = DocumentTypeCommpoundBefore;
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " And DocumentType=" + objRecord.DocumentType + " And BranchID=" + Comon.cInt(cmbBranchesID.EditValue)));

            objRecord.VoucherID = VoucherID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = MySession.GlobalFacilityID;
            //Date

            objRecord.VoucherDate = Comon.ConvertDateToSerial(((DateTime)gridViewBeforCompond.GetRowCellValue(gridViewBeforCompond.DataRowCount - 1, "DebitDate")).ToString("dd/MM/yyyy")).ToString();
            
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

            returned.DebitGold = Comon.cDbl(Comon.cDbl(txtTotalBefore.Text) - Comon.cDbl(txtTotalQerateBefor.Text));
            returned.DebitDiamond = Comon.cDbl( txtWeghtStonBefore.Text);
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
            returned.CreditGold = Comon.cDbl(Comon.cDbl(txtTotalBefore.Text) - Comon.cDbl(txtTotalQerateBefor.Text));
            returned.CreditDiamond = Comon.cDbl(txtWeghtStonBefore.Text);
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
            objRecord.DocumentType = DocumentTypeCommpoundAfter;
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " And DocumentType=" + objRecord.DocumentType + " And BranchID=" + Comon.cInt(cmbBranchesID.EditValue)));

            objRecord.VoucherID = VoucherID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = MySession.GlobalFacilityID;
            //Date
           objRecord.VoucherDate = Comon.ConvertDateToSerial(((DateTime)gridViewAfterCompond.GetRowCellValue(gridViewAfterCompond.DataRowCount - 1, "DebitDate")).ToString("dd/MM/yyyy")).ToString(); 
            
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
            returned.DebitGold = Comon.cDbl(Comon.cDbl(txtTotalAfter.Text) - Comon.cDbl(txtTotalQerateAfter.Text));
            returned.DebitDiamond = Comon.cDbl(txtWeghtStonAfter.Text);
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
            returned.CreditGold = Comon.cDbl(Comon.cDbl(txtTotalAfter.Text) - Comon.cDbl(txtTotalQerateAfter.Text));
            returned.CreditDiamond = Comon.cDbl(txtWeghtStonAfter.Text);
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
            objRecord.DocumentTypeID = DocumentTypeCommpoundBefore;
            objRecord.MoveType = 2;
            objRecord.MoveID = 0;
            objRecord.TranseID = DocumentID;
            objRecord.Posted = Comon.cInt(cmbStatus.EditValue);
            Stc_ItemsMoviing returned;
            List<Stc_ItemsMoviing> listreturned = new List<Stc_ItemsMoviing>();
            for (int i = 0; i <= gridViewBeforCompond.DataRowCount - 1; i++)
            {
                returned = new Stc_ItemsMoviing();
                returned.ID = i + 1;

                returned.MoveDate = Comon.ConvertDateToSerial(((DateTime)gridViewBeforCompond.GetRowCellValue(i, "DebitDate")).ToString("dd/MM/yyyy")).ToString(); 

                returned.MoveID = 0;
                returned.TranseID = DocumentID;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.DocumentTypeID = DocumentTypeCommpoundBefore;
                returned.MoveType = 2;
                returned.StoreID = Comon.cDbl(txtStoreID.Text.ToString());
                returned.AccountID = Comon.cDbl(txtAccountID.Text);
                returned.BarCode = gridViewBeforCompond.GetRowCellValue(i, "BarcodCompond").ToString();
                returned.ItemID = Comon.cInt(gridViewBeforCompond.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(gridViewBeforCompond.GetRowCellValue(i, "SizeID").ToString());
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID + " and BranchID=" + MySession.GlobalBranchID));
                returned.QTY = Comon.cDbl(gridViewBeforCompond.GetRowCellValue(i, "ComWeightStonin").ToString());
                returned.InPrice = 0;
                returned.OutPrice = 0;
                //returned.Bones = Comon.cDbl(gridViewBeforCompond.GetRowCellValue(i, "Bones").ToString());
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
            objRecord.DocumentTypeID = DocumentTypeCommpoundAfter;
            objRecord.MoveType = 1;
            objRecord.MoveID = 0;
            objRecord.TranseID = DocumentID;
            objRecord.Posted = Comon.cInt(cmbStatus.EditValue);
            Stc_ItemsMoviing returned;
            List<Stc_ItemsMoviing> listreturned = new List<Stc_ItemsMoviing>();
            for (int i = 0; i <= gridViewAfterCompond.DataRowCount - 1; i++)
            {
                returned = new Stc_ItemsMoviing();
                returned.ID = i + 1;
                returned.MoveDate = Comon.ConvertDateToSerial(((DateTime)gridViewAfterCompond.GetRowCellValue(i, "DebitDate")).ToString("dd/MM/yyyy")).ToString(); 
                returned.MoveID = 0;
                returned.TranseID = DocumentID;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.DocumentTypeID = DocumentTypeCommpoundAfter;
                returned.MoveType = 1;
                returned.StoreID = Comon.cDbl(txtStoreID.Text.ToString());
                returned.AccountID = Comon.cDbl(txtAccountID.Text);
                returned.BarCode = gridViewAfterCompond.GetRowCellValue(i, "BarcodCompond").ToString();
                returned.ItemID = Comon.cInt(gridViewAfterCompond.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(gridViewAfterCompond.GetRowCellValue(i, "SizeID").ToString());
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID + " and BranchID=" + MySession.GlobalBranchID));
                returned.QTY = Comon.cDbl(gridViewAfterCompond.GetRowCellValue(i, "ComWeightSton").ToString());
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
                       
                       MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text), DocumentTypeCommpoundBefore);
                       MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text),DocumentTypeCommpoundAfter); 
                     if (MoveID <0)
                         Messages.MsgError(Messages.TitleInfo, "خطا في حذف الحركة  المخزنية");
                 }

                 #region Delete Voucher Machin
                 //حذف القيد الالي
                 if (Comon.cInt(Result) > 0)
                 {
                     
                     int VoucherIDCompoundBefore = 0;
                     DataTable dtInvoiceIDCompoundBefore = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(txtCommandID.Text), DocumentTypeCommpoundBefore);
                     if (dtInvoiceIDCompoundBefore.Rows.Count > 0)
                     {
                         VoucherIDCompoundBefore = DeleteVariousVoucherMachin(Comon.cInt(dtInvoiceIDCompoundBefore.Rows[0][0]), DocumentTypeCommpoundBefore);
                         if (VoucherIDCompoundBefore == 0)
                             Messages.MsgError(Messages.TitleInfo, "خطا في حذف قيد العملية تركيب-قبل");
                     }
                     int VoucherIDCompoundAfter = 0;
                     DataTable dtInvoiceIDCompoundAfter = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(txtCommandID.Text), DocumentTypeCommpoundAfter);
                     if (dtInvoiceIDCompoundAfter.Rows.Count > 0)
                     {
                         VoucherIDCompoundAfter = DeleteVariousVoucherMachin(Comon.cInt(dtInvoiceIDCompoundAfter.Rows[0][0]), DocumentTypeCommpoundAfter);
                         if (VoucherIDCompoundAfter == 0)
                             Messages.MsgError(Messages.TitleInfo, "خطا في حذف قيد العملية تركيب-بعد");
                     }
                    
                 }
                 #endregion

                 #region Delete Stock IN Or Out From archive
                 //حذف التوريد والصرف من الارشيف
                 //if (Comon.cInt(Result) > 0)
                 //{
                      
                 //    int OutCompundID = 0;
                 //    DataTable dtInvoiceIDCompundBefor = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(txtCommandID.Text), DocumentTypeCommpoundBefore);
                 //    if (dtInvoiceIDCompundBefor.Rows.Count > 0)
                 //    {
                 //        OutCompundID = DeleteInOnOROutOnBil(Comon.cInt(dtInvoiceIDCompundBefor.Rows[0][0]), DocumentTypeCommpoundBefore);
                 //        if (OutCompundID == 0)
                 //            Messages.MsgError(Messages.TitleInfo, "خطا في حذف الصرف من الارشيف للعلية تركيب- قبل  ");
                 //    }
                 //    int InCompundID = 0;
                 //    DataTable dtInvoiceIDCompundAfter = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(txtCommandID.Text), DocumentTypeCommpoundAfter);
                 //    if (dtInvoiceIDCompundAfter.Rows.Count > 0)
                 //    {
                 //        InCompundID = DeleteInOnOROutOnBil(Comon.cInt(dtInvoiceIDCompundAfter.Rows[0][0]), DocumentTypeCommpoundAfter);
                 //        if (InCompundID == 0)
                 //            Messages.MsgError(Messages.TitleInfo, "خطا في حذف التوريد من الارشيف للعملية تركيب- بعد ");
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
                txtTotalQerateBefor.Text = "";
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
                txtTotalQerateBefor.Text = "0";
                cmbCurency.EditValue = Comon.cInt(MySession.GlobalDefaultCommpundCurrncyID);
                //جريد فيو
                initGridBeforCompent();
                initGridAfterCompent();
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

                strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtEmpID.Text) + " And Cancel =0  and BranchID=" + MySession.GlobalBranchID;
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

        private void gridViewBeforCompond_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
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

        private void GridViewBeforPolish_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            
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
 
 
     
        private void btnMachinResractionCommpondBefore_Click(object sender, EventArgs e)
        {
            if (IsNewRecord == true) return;
            int ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" +  txtCommandID.Text + " And DocumentType=" + DocumentTypeCommpoundBefore).ToString());
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

        private void btnMachinResractionCompondAfter_Click(object sender, EventArgs e)
        {
            if (IsNewRecord == true) return;
             int ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" +  txtCommandID.Text  + " And DocumentType=" + DocumentTypeCommpoundAfter).ToString());
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

        private void gridControlOrderDetails_Click(object sender, EventArgs e)
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

        private void btnCostOrder_Click(object sender, EventArgs e)
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
                for (int i = 0; i <= gridViewAfterCompond.DataRowCount - 1; i++)
                {
                    dtItem.Rows.Add();
                    dtItem.Rows[i]["ID"] = i;
                    dtItem.Rows[i]["FacilityID"] = UserInfo.FacilityID;
                    dtItem.Rows[i]["BarCode"] = gridViewAfterCompond.GetRowCellValue(i, "BarcodCompond").ToString();
                    dtItem.Rows[i]["ItemID"] = Comon.cInt(gridViewAfterCompond.GetRowCellValue(i, "ItemID").ToString());
                    DataTable dt = Lip.SelectRecord("SELECT   [GroupID]  ," + PrimaryName + "  FROM  [Stc_ItemsGroups] where Cancel=0 and BranchID=" + MySession.GlobalBranchID+" and [GroupID] in(select [GroupID] from Stc_Items where ItemID=" + dtItem.Rows[i]["ItemID"] + " and Cancel=0 and BranchID=" + MySession.GlobalBranchID+") ");
                    dtItem.Rows[i]["GroupID"] = Comon.cDbl(dt.Rows[0]["GroupID"]);
                    dtItem.Rows[i][GroupName] = dt.Rows[0][PrimaryName].ToString();

                    dtItem.Rows[i]["SizeID"] = Comon.cInt(gridViewAfterCompond.GetRowCellValue(i, "SizeID").ToString());
                    dtItem.Rows[i][ItemName] = gridViewAfterCompond.GetRowCellValue(i, ItemName).ToString();
                    dtItem.Rows[i][SizeName] = gridViewAfterCompond.GetRowCellValue(i, SizeName).ToString();
                    dtItem.Rows[i]["QTY"] = Comon.ConvertToDecimalQty(gridViewAfterCompond.GetRowCellValue(i, "ComWeightSton").ToString());
                    //  dtItem.Rows[i]["PackingQty"] = Comon.ConvertToDecimalPrice(gridViewAfterCompond.GetRowCellValue(i, "PackingQty").ToString());
                    dtItem.Rows[i]["SalePrice"] = 0;

                    dtItem.Rows[i]["Description"] = UserInfo.Language == iLanguage.Arabic ? "تحويل من مرحلة التركيب " : "Transfer from compound";

                    dtItem.Rows[i]["StoreAccountID"] = Comon.cDbl(txtStoreID.Text);
                    dtItem.Rows[i]["StoreName"] =lblStoreName.Text.ToString();
                    dtItem.Rows[i]["Caliber"] = 18;

                    dtItem.Rows[i]["CostPrice"] = Comon.ConvertToDecimalPrice(gridViewAfterCompond.GetRowCellValue(i, "CostPrice").ToString());
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

        private void btnDims_Click(object sender, EventArgs e)
        {
            frmManufacturingDismantOrders frm = new frmManufacturingDismantOrders();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
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
                        gridViewBeforCompond.AddNewRow();
                        if (ChekOrderIsFoundInGrid(gridViewBeforCompond,"BarcodCompond",BarCode))
                        {
                            Messages.MsgAsterisk(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "الصنف موجود  لذلك لا يمكن انزاله  اكثر من مرة " : "This Item is Found Table");
                            gridViewBeforCompond.DeleteRow(rowIndex);
                            return;
                        }
                   
                        string QTY = view.GetRowCellValue(view.FocusedRowHandle, "QTY").ToString();
                        FillItemData(gridViewBeforCompond, gridControlBeforCompond, "BarcodCompond", "ComWeightSton", Stc_itemsDAL.GetItemData1(BarCode, UserInfo.FacilityID), "DebitDate", "DebitTime", ((TextEdit)txtAccountID), QTY);                  
                        SendKeys.Send("\t");

                    }

                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void gridViewBeforCompond_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            rowIndex = e.RowHandle;
        }

        private void gridViewAfterCompond_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            rowIndex = e.RowHandle;
        }

        private void gridViewBeforCompond_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridView view = sender as GridView;

                if (view.GetRowCellValue(view.FocusedRowHandle, "BarcodCompond").ToString().Trim() != "")
                {
                    string BarCode = view.GetRowCellValue(view.FocusedRowHandle, "BarcodCompond").ToString().Trim();
                    DataTable dt;
                    dt = Stc_itemsDAL.GetItemData(BarCode, UserInfo.FacilityID);
                    if (dt.Rows.Count > 0)
                    {
                        gridViewAfterCompond.AddNewRow();
                        if (ChekOrderIsFoundInGrid(gridViewAfterCompond, "BarcodCompond", BarCode))
                        {
                            Messages.MsgAsterisk(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "الصنف موجود  لذلك لا يمكن انزاله  اكثر من مرة " : "This Item is Found Table");
                            gridViewAfterCompond.DeleteRow(rowIndex);
                            return;
                        }
                       
                        string QTY = view.GetRowCellValue(view.FocusedRowHandle, "ComWeightSton").ToString();
                        FillItemData(gridViewAfterCompond, gridControlAfterCompond, "BarcodCompond", "ComWeightSton", Stc_itemsDAL.GetItemData1(BarCode, UserInfo.FacilityID), "DebitDate", "DebitTime", txtAccountID, QTY);
                        SendKeys.Send("\t");
                    }

                }
            }
            catch(Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
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

        private void simpleButton1_Click(object sender, EventArgs e)
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

        private void btnToPrev_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtOrderID.Text) != true)
            {
                strSQL = "SELECT TOP 1 ComandID FROM " + Menu_FactoryRunCommandMasterDAL.TableName + " Where  TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + " and BranchID=" + MySession.GlobalBranchID +" and  Cancel =0 and  ComandID<" + Comon.cLong(txtCommandID.Text) + " and Barcode=" + txtOrderID.Text;
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
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
namespace Edex.Manufacturing.Codes
{
    public partial class frmManufacturingCommandOld : BaseForm
    {
        //list detail
        BindingList<Menu_FactoryRunCommandPrentagAndPulishn> lstDetailPrentage = new BindingList<Menu_FactoryRunCommandPrentagAndPulishn>();
        BindingList<Menu_FactoryRunCommandPrentagAndPulishn> lstDetailAfterPrentage = new BindingList<Menu_FactoryRunCommandPrentagAndPulishn>();
        BindingList<Menu_FactoryRunCommandTalmee> lstDetailTalmee = new BindingList<Menu_FactoryRunCommandTalmee>();
        BindingList<Menu_FactoryRunCommandTalmee> lstDetailAfterTalmee = new BindingList<Menu_FactoryRunCommandTalmee>();
        BindingList<Menu_FactoryRunCommandCompund> lstDetailCompund = new BindingList<Menu_FactoryRunCommandCompund>();
        BindingList<Menu_FactoryRunCommandCompund> lstDetailAfterCompund = new BindingList<Menu_FactoryRunCommandCompund>();

        BindingList<Menu_FactoryRunCommandCompund> lstDetailCostDaimond = new BindingList<Menu_FactoryRunCommandCompund>();

        BindingList<Menu_FactoryRunCommandSelver> lstDetailSelver = new BindingList<Menu_FactoryRunCommandSelver>();

        BindingList<Menu_FactoryRunCommandfactory> lstDetailfactory = new BindingList<Menu_FactoryRunCommandfactory>();

        BindingList<Menu_FactoryRunCommandfactory> lstDetailAfterfactory = new BindingList<Menu_FactoryRunCommandfactory>();
        BindingList<Manu_ProductionExpensesDetails> lstDetailProductionExpenses = new BindingList<Manu_ProductionExpensesDetails>();
        BindingList<Manu_AuxiliaryMaterialsDetails> lstDetailAlcadZircon = new BindingList<Manu_AuxiliaryMaterialsDetails>();
        BindingList<Stc_ItemUnits> lstDetailUnit = new BindingList<Stc_ItemUnits>();
        #region Declare
        public int DocumentTypeFactoryBefore = 31;
        public int DocumentTypeFactoryAfter = 32;
        public int DocumentTypeBrntageBefore = 33;
        public int DocumentTypeBrntageAfter = 34;
        public int DocumentTypeCommpoundBefore = 35;
        public int DocumentTypeCommpoundAfter = 36;
        public int DocumentTypePloshinBefore = 37;
        public int DocumentTypePolshinAfter = 38;
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
        public frmManufacturingCommandOld()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
            InitializeComponent();
            SplashScreenManager.CloseForm();

            //Events
            this.txtGroupID.Validating += new System.ComponentModel.CancelEventHandler(this.txtGroupID_Validating); 
            this.txtBrandID.Validating += new System.ComponentModel.CancelEventHandler(this.txtBrandID_Validating);
            this.txtTypeID.Validating += new System.ComponentModel.CancelEventHandler(this.txtTypeID_Validating);
            this.txtCustomerID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCustomerID_Validating);
            this.txtEmpIDFactor.Validating += new System.ComponentModel.CancelEventHandler(this.txtEmpFactorID_Validating);
            this.txtEmplooyID.Validating += new System.ComponentModel.CancelEventHandler(this.txtEmplooyBrntageID_Validating);
            this.txtEmplooyIDPolishing.Validating += new System.ComponentModel.CancelEventHandler(this.txtEmplooyPolishingID_Validating);
            this.txtEmployeeStokIDFactory.Validating += new System.ComponentModel.CancelEventHandler(this.txtEmployeeStokID_Validating);
            this.txtCommandID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCommandID_Validating);

            this.txtReferanceID.Validating += txtReferanceID_Validating;
            //Event GridView
 
            this.gridControlBeforPrentag.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.gridControl1_ProcessGridKey);
            this.gridControlAdditional.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.gridControl2_ProcessGridKey);
            this.gridControlBeforCompond.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.gridControl2_ProcessGridKey);
            this.gridControlAfterCompond.ProcessGridKey += gridControl2_ProcessGridKey;
            this.gridControlBeforePolishing.ProcessGridKey += gridControl3_ProcessGridKey;
            this.gridControlAfterPolishing.ProcessGridKey += gridControl2_ProcessGridKey;
            this.gridControlfactroOpretion.ProcessGridKey += gridControl2_ProcessGridKey;
            this.gridControlAfterFactory.ProcessGridKey += gridControl2_ProcessGridKey;
            this.gridControlProductionExpenses.ProcessGridKey += gridControl2_ProcessGridKey;

            this.GridViewBeforfactory.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.GridViewBeforfactory_ValidatingEditor);
            this.GridViewAfterfactory.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.GridViewAfterfactory_ValidatingEditor);
            this.GridViewBeforfactory.RowUpdated += GridViewBeforfactory_RowUpdated;
            this.GridViewAfterfactory.RowUpdated += GridViewBeforfactory_RowUpdated;

            this.GridViewBeforPrentag.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.GridViewBeforPrentag_ValidatingEditor);
            this.GridViewAfterPrentag.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.GridViewAfterPrentag_ValidatingEditor);
            this.GridViewBeforPrentag.RowUpdated += GridViewBeforPrentag_RowUpdated;
            this.GridViewAfterPrentag.RowUpdated += GridViewBeforPrentag_RowUpdated;

            this.gridViewBeforCompond.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridViewBeforCompond_ValidatingEditor);       
            this.gridViewAfterCompond.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridViewAfterCompond_ValidatingEditor);
            this.gridViewBeforCompond.RowUpdated += gridView2_RowUpdated;
            this.gridViewAfterCompond.RowUpdated += gridView2_RowUpdated;

            this.GridViewBeforPolish.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.GridViewBeforPolish_ValidatingEditor);
            this.GridViewAfterPolish.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.GridViewAfterPolish_ValidatingEditor);
            this.GridViewBeforPolish.RowUpdated += GridViewBeforPolish_RowUpdated;
            this.GridViewAfterPolish.RowUpdated += GridViewBeforPolish_RowUpdated;

            this.gridViewAdditional.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridViewAdditional_ValidatingEditor);
            this.gridViewAdditional.RowUpdated+=gridViewAdditional_RowUpdated;

            this.GridProductionExpenses.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.GridProductionExpenses_ValidatingEditor);
            this.GridProductionExpenses.RowUpdated += GridProductionExpenses_RowUpdated;
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
            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", "ArbName", "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));           
        }
        void GridProductionExpenses_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            int lenght = GridAlcadZircone.DataRowCount;
            decimal TotalCost = 0;
            for (int i = 0; i < lenght; i++)
            {
                TotalCost += Comon.ConvertToDecimalPrice(GridAlcadZircone.GetRowCellValue(i,"TotalCost"));
            }
             int lenghtProductionExp = GridProductionExpenses.DataRowCount;
            decimal TotalOrderCostPercentage = 0;
            for (int i = 0; i < lenghtProductionExp; i++)
            {
                TotalOrderCostPercentage += Comon.ConvertToDecimalPrice(GridProductionExpenses.GetRowCellValue(i,"OrderCostPercentage"));      
            }
            lblToatalCostOrder.Text = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(TotalCost) + Comon.ConvertToDecimalPrice(TotalOrderCostPercentage)) + "";
            if (Comon.cDec( txtTotalGoldWithStone.Text)>0)
            lblCostPerGram.Text = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(lblToatalCostOrder.Text) / Comon.ConvertToDecimalPrice(txtTotalGoldWithStone.Text)) + "";
            lblTotalSale.Text = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(lblRevenuePerGram.Text) * Comon.ConvertToDecimalPrice(txtTotalGoldWithStone.Text)) + "";
            lblProfits.Text = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(lblTotalSale.Text) - Comon.ConvertToDecimalPrice(lblToatalCostOrder.Text)) + "";
        }

        void txtReferanceID_Validating(object sender, CancelEventArgs e)
        {
            DataTable dt = AuxiliaryMaterialsDAl.frmGetDataDetalByReferance(Comon.cInt(txtReferanceID.Text), Comon.cInt(MySession.GlobalBranchID), UserInfo.FacilityID);

            gridControlCostAlcadZircone.DataSource = dt;
            lstDetailAlcadZircon.AllowNew = true;
            lstDetailAlcadZircon.AllowEdit = true;
            lstDetailAlcadZircon.AllowRemove = true;
        }

        void GridViewBeforPolish_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            CalculatePolishnLost();

        }
        void GridViewBeforfactory_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            CalculateFactoryLost();

        }
        void gridViewAdditional_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
             CalculateAdditionalQTY();

        }
        void GridViewBeforPrentag_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            CalculatePrentageLost();
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

        void gridView2_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            decimal GoldDebit = 0;
            decimal GoldCredit = 0;
            decimal ComStoneNumin = 0;
            decimal ComWeightStonin = 0;
            decimal ComStoneNumout = 0;
            decimal ComWeightStonOUt = 0;
            decimal ComStoneNumlas = 0;
            decimal ComWeightStonLas = 0;
            decimal ComStoneCom = 0;
            decimal ComWeightSton = 0;
             int lenght = gridViewBeforCompond.RowCount;
             int LengAfter = gridViewAfterCompond.RowCount;
             for (int i = 0; i < lenght; i++)
             {
                    GoldDebit += Comon.cDec(gridViewBeforCompond.GetRowCellValue(i, "GoldDebit"));
                    ComStoneNumin += Comon.cDec(gridViewBeforCompond.GetRowCellValue(i, "ComStoneNumin"));
                    ComWeightStonin += Comon.cDec(gridViewBeforCompond.GetRowCellValue(i, "ComWeightStonin"));
                    ComStoneNumout += Comon.cDec(gridViewBeforCompond.GetRowCellValue(i, "ComStoneNumout"));
                    ComWeightStonOUt += Comon.cDec(gridViewBeforCompond.GetRowCellValue(i, "ComWeightStonOUt"));
             }
             for (int i = 0; i < LengAfter; i++)
             {
                 GoldCredit += Comon.cDec(gridViewAfterCompond.GetRowCellValue(i, "GoldCredit"));
                 ComStoneNumlas += Comon.cDec(gridViewAfterCompond.GetRowCellValue(i, "ComStoneNumlas"));
                 ComWeightStonLas += Comon.cDec(gridViewAfterCompond.GetRowCellValue(i, "ComWeightStonLas"));
                 ComStoneCom += Comon.cDec(gridViewAfterCompond.GetRowCellValue(i, "ComStoneCom"));
                 ComWeightSton += Comon.cDec(gridViewAfterCompond.GetRowCellValue(i, "ComWeightStonAfter"));
             }
                txtCompondGoldDebit.Text = GoldDebit.ToString();
                txtCompoundGoldCredit.Text = GoldCredit.ToString();
                txtCountStonesNumin.Text = ComStoneNumin.ToString();
                txtWeightStonesin.Text = ComWeightStonin.ToString();
                txtCountStonesNumOut.Text = ComStoneNumout.ToString();
                txtWeightStonesOut.Text = ComWeightStonOUt.ToString();
                txtCountStonesNumLos.Text = ComStoneNumlas.ToString();
                txtWeightStonesLos.Text = ComWeightStonLas.ToString();
                txtCountStonesNumCom.Text = ComStoneCom.ToString();
                txtWeightStonesCom.Text =Comon.cDec( ComWeightSton).ToString();
                txtTotalGoldWithStone.Text=Comon.cDec(Comon.cDec( ComWeightSton)+Comon.cDec( GoldCredit))+"";
                lblToatalLostCompound.Text = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(txtWeightStonesin.Text) / 5 + Comon.ConvertToDecimalPrice(txtCompondGoldDebit.Text)) - Comon.ConvertToDecimalPrice(txtTotalGoldWithStone.Text)) + "";
            }

        #region Events 
        private void gridViewBeforCompond_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
          
                if (this.gridViewBeforCompond.ActiveEditor is TextEdit)
                {
                    GridView view = sender as GridView;
                    double num;
                    object val = e.Value;
                    e.Valid = true;
                    HasColumnErrors = false;
                    string ColName = view.FocusedColumn.FieldName;

                    if (e.Value != null)
                    {
                        if (ColName == "ComStoneNumin")
                            gridViewBeforCompond.SetFocusedRowCellValue("ComStoneCom", Comon.cDbl(e.Value) - Comon.cDbl(Comon.cDbl(gridViewBeforCompond.GetFocusedRowCellValue("ComStoneNumout")) + Comon.cDbl(gridViewBeforCompond.GetFocusedRowCellValue("ComStoneNumlas"))));
                        else if (ColName == "ComWeightStonin")
                            gridViewBeforCompond.SetFocusedRowCellValue("ComWeightSton", Comon.cDbl(e.Value) - Comon.cDbl(Comon.cDbl(gridViewBeforCompond.GetFocusedRowCellValue("ComWeightStonOUt")) + Comon.cDbl(gridViewBeforCompond.GetFocusedRowCellValue("ComWeightStonLas"))));
                        else if (ColName == "ComStoneNumout")
                            gridViewBeforCompond.SetFocusedRowCellValue("ComStoneCom", Comon.cDbl(Comon.cDbl(gridViewBeforCompond.GetFocusedRowCellValue("ComStoneNumin")) - Comon.cDbl(Comon.cDbl(e.Value) + Comon.cDbl(gridViewBeforCompond.GetFocusedRowCellValue("ComStoneNumlas")))));
                        else if (ColName == "ComWeightStonOUt")
                            gridViewBeforCompond.SetFocusedRowCellValue("ComWeightSton", Comon.cDbl(Comon.cDbl(gridViewBeforCompond.GetFocusedRowCellValue("ComWeightStonin")) - Comon.cDbl(Comon.cDbl(e.Value) + Comon.cDbl(gridViewBeforCompond.GetFocusedRowCellValue("ComWeightStonLas")))));
                        else if (ColName == "ComStoneNumlas")
                            gridViewBeforCompond.SetFocusedRowCellValue("ComStoneCom", Comon.cDbl(Comon.cDbl(gridViewBeforCompond.GetFocusedRowCellValue("ComStoneNumin")) - Comon.cDbl(Comon.cDbl(e.Value) + Comon.cDbl(gridViewBeforCompond.GetFocusedRowCellValue("ComStoneNumout")))));
                        else if (ColName == "ComWeightStonLas")
                            gridViewBeforCompond.SetFocusedRowCellValue("ComWeightSton", Comon.cDbl(Comon.cDbl(gridViewBeforCompond.GetFocusedRowCellValue("ComWeightStonin")) - Comon.cDbl(Comon.cDbl(e.Value) + Comon.cDbl(gridViewBeforCompond.GetFocusedRowCellValue("ComWeightStonOUt")))));                       
                    }
                    if (ColName != "BarcodCompond" && ColName != "TypeSton" && ColName != ItemName && ColName != SizeName && ColName != "DebitTime" && ColName != "DebitDate" && ColName != "FromAccountName" && ColName != "EmpCompundName" && ColName != "ComSignature")
                    {
                        if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgInputIsRequired;
                        }
                        else if (!(double.TryParse(e.Value.ToString(), out num)) && (ColName != "BarcodCompond" && ColName != "TypeSton" && ColName != ItemName && ColName != "FromAccountName" && ColName != "EmpCompundName" && ColName != "ComSignature"))
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgInputShouldBeNumber;
                        }
                        else if (Comon.ConvertToDecimalPrice(e.Value.ToString()) < 0 && (ColName != "BarcodCompond" && ColName != "TypeSton" && ColName != ItemName && ColName != "FromAccountName" && ColName != "EmpCompundName" && ColName != "ComSignature"))
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgInputIsGreaterThanZero;
                        }
                    }
                    else if (ColName == "BarcodCompond")
                    {
                        DataTable dtGroupID = Lip.SelectRecord("Select ArbItemName,ArbItemType from Stc_Items_Find where BarCode='" + e.Value.ToString() + "'");
                        if (dtGroupID.Rows.Count > 0)
                        {
                            //gridViewBeforCompond.SetFocusedRowCellValue("TypeSton", dtGroupID.Rows[0]["ArbItemType"]);
                            gridViewBeforCompond.SetFocusedRowCellValue(ItemName, dtGroupID.Rows[0]["ArbItemName"]);        
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = "لا يوجد  حجر  تملك هذا الباركود";
                            view.SetColumnError(view.Columns[ColName], "لا يوجد حجر تملك هذا الباركود");
                        }
                    }
                    if (ColName == ItemName)
                    {

                        DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_Items_Find  Where  LOWER (" + ItemName + ")=LOWER ('" + val.ToString() + "') ");
                        if (dtItemID.Rows.Count > 0)
                        {
                            FillItemData(gridViewBeforCompond, gridControlBeforCompond, "BarcodCompond", "GoldDebit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", ((TextEdit)txtAccountIDBeforCompond));
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = " الصنف غير موجود  ";
                        }
                    }
                    if (ColName == SizeName)
                    {

                        DataTable dtSizeID = Lip.SelectRecord("Select  SizeID  from Stc_SizingUnits Where  LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') ");
                        if (dtSizeID.Rows.Count > 0)
                        {

                            FillItemData(gridViewBeforCompond, gridControlBeforCompond, "BarcodCompond", "GoldDebit", Stc_itemsDAL.GetItemDataByItemID_SizeID(Comon.cInt(gridViewBeforCompond.GetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "ItemID")), Comon.cInt(dtSizeID.Rows[0][0].ToString()), UserInfo.FacilityID), "DebitDate", "DebitTime", ((TextEdit)txtAccountIDBeforCompond));
                            gridViewBeforCompond.SetRowCellValue(gridViewBeforCompond.FocusedRowHandle, ColName, e.Value.ToString());
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = " الوحده غير موجوده  ";
                        }

                    }
                    if (ColName == "ItemID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_ItemUnits  Where ItemID=" + Comon.cInt(e.Value));
                        if (dtItemID.Rows.Count > 0)
                        {
                            FillItemData(gridViewBeforCompond, gridControlBeforCompond, "BarcodCompond", "GoldDebit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", txtAccountIDBeforCompond);
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = "رقم الصنف غير موجود  ";
                        }
                    }
                    if (ColName == "ComStoneNumout")
                    {

                        if(Comon.cDbl( e.Value)>Comon.cDbl(gridViewBeforCompond.GetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "ComStoneNumin").ToString()))
                        {
                            e.Valid = false;
                            HasColumnErrors = true;

                            e.ErrorText = Messages.msgNumStounOutGreterThanNumStonin;
                            view.SetColumnError(view.Columns[ColName], Messages.msgNumStounOutGreterThanNumStonin);
                        }
                        
                        if ((Comon.cDbl(e.Value) + Comon.cDbl(gridViewBeforCompond.GetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "ComStoneNumlas")!=null?gridViewBeforCompond.GetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "ComStoneNumlas").ToString():0+"")) > Comon.cDbl(gridViewBeforCompond.GetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "ComStoneNumin").ToString()))
                            {
                                e.Valid = false;
                                HasColumnErrors = true;
                                e.ErrorText = Messages.msgNumStounOutNumStonLosGreterThenNumStonin;
                                view.SetColumnError(view.Columns[ColName], Messages.msgNumStounOutNumStonLosGreterThenNumStonin);
                            }

                        else
                        {
                            e.Valid = true;
                            HasColumnErrors = false;
                            e.ErrorText = "";
                            view.SetColumnError(view.Columns[ColName], "");

                        }

                    }
                    if (ColName == "ComWeightStonOUt")
                    {

                        if (Comon.cDbl(e.Value) > Comon.cDbl(gridViewBeforCompond.GetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "ComWeightStonin").ToString()))
                        {
                            e.Valid = false;
                            HasColumnErrors = true;

                            e.ErrorText = Messages.msgWeightStounOutGreterThanWeightStonin;
                            view.SetColumnError(view.Columns[ColName], Messages.msgWeightStounOutGreterThanWeightStonin);
                        }

                        if ((Comon.cDbl(e.Value) + Comon.cDbl(gridViewBeforCompond.GetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "ComWeightStonLas") != null ? gridViewBeforCompond.GetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "ComWeightStonLas").ToString() : 0 + "")) > Comon.cDbl(gridViewBeforCompond.GetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "ComWeightStonin").ToString()))
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgWeightStounOutWeightStonLosGreterThenWeightStonin;
                            view.SetColumnError(view.Columns[ColName], Messages.msgWeightStounOutWeightStonLosGreterThenWeightStonin);
                        }

                        else
                        {
                            e.Valid = true;
                            HasColumnErrors = false;
                            e.ErrorText = "";
                            view.SetColumnError(view.Columns[ColName], "");

                        }

                    }
                    if (ColName == "ComStoneNumlas")
                    {

                        if (Comon.cDbl(e.Value) > Comon.cDbl(gridViewBeforCompond.GetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "ComStoneNumin").ToString()))
                        {
                            e.Valid = false;
                            HasColumnErrors = true;

                            e.ErrorText = Messages.msgNumStounLostGreterThanNumStonin;
                            view.SetColumnError(view.Columns[ColName], Messages.msgNumStounLostGreterThanNumStonin);
                        }

                        if ((Comon.cDbl(e.Value) + Comon.cDbl(gridViewBeforCompond.GetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "ComStoneNumout") != null ? gridViewBeforCompond.GetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "ComStoneNumout").ToString() : 0 + "")) > Comon.cDbl(gridViewBeforCompond.GetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "ComStoneNumin").ToString()))
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNumStounOutNumStonLosGreterThenNumStonin;
                            view.SetColumnError(view.Columns[ColName], Messages.msgNumStounOutNumStonLosGreterThenNumStonin);
                        }

                        else
                        {
                            e.Valid = true;
                            HasColumnErrors = false;
                            e.ErrorText = "";
                            view.SetColumnError(view.Columns[ColName], "");

                        }

                    }
                    if (ColName == "ComWeightStonLas")
                    {

                        if (Comon.cDbl(e.Value) > Comon.cDbl(gridViewBeforCompond.GetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "ComWeightStonin").ToString()))
                        {
                            e.Valid = false;
                            HasColumnErrors = true;


                            e.ErrorText = Messages.msgWeightStounLostGreterThanWeightStonin;
                            view.SetColumnError(view.Columns[ColName], Messages.msgWeightStounLostGreterThanWeightStonin);
                        }
                        if ((Comon.cDbl(e.Value) + Comon.cDbl(gridViewBeforCompond.GetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "ComWeightStonOUt") != null ? gridViewBeforCompond.GetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "ComWeightStonOUt").ToString() : 0 + "")) > Comon.cDbl(gridViewBeforCompond.GetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "ComWeightStonin").ToString()))
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgWeightStounOutWeightStonLosGreterThenWeightStonin;
                            view.SetColumnError(view.Columns[ColName], Messages.msgWeightStounOutWeightStonLosGreterThenWeightStonin);
                        }
                        else
                        {
                            e.Valid = true;
                            HasColumnErrors = false;
                            e.ErrorText = "";
                            view.SetColumnError(view.Columns[ColName], "");
                        }
                    }
                    else if (ColName == "FromAccountID")
                    {

                        DataTable dtGroupID = Lip.SelectRecord("Select ArbName from Acc_Accounts WHERE Cancel =0 And  AccountID=" + e.Value.ToString());
                        if (dtGroupID.Rows.Count > 0)
                        {
                            gridViewBeforCompond.SetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "FromAccountName", dtGroupID.Rows[0]["ArbName"]);

                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = "لا يوجد حساب بهذا الرقم  ";
                            view.SetColumnError(view.Columns[ColName], "لا يوجد حساب بهذا الرقم");
                        }
                    }

                    if (ColName == "EmpCompondID")
                    {

                        DataTable dtGroupID = Lip.SelectRecord("Select ArbName from HR_EmployeeFile WHERE Cancel =0 And  EmployeeID=" + e.Value);
                        if (dtGroupID.Rows.Count > 0)
                        {
                            gridViewBeforCompond.SetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "EmpCompundName", dtGroupID.Rows[0]["ArbName"]);

                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = "لا يوجد حساب بهذا الرقم  ";
                            view.SetColumnError(view.Columns[ColName], "لا يوجد حساب بهذا الرقم");
                        }
                    }
                    else if (ColName == "FromAccountName")
                    {

                        DataTable dtGroupID = Lip.SelectRecord("Select AccountID  from Acc_Accounts WHERE Cancel =0 And  LOWER (" + PrimaryName + ")=LOWER ('" +val.ToString() + "')");
                        if (dtGroupID.Rows.Count > 0)
                        {
                            gridViewBeforCompond.SetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "FromAccountID", dtGroupID.Rows[0]["AccountID"]);

                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = "لا يوجد حساب بهذا الاسم  ";
                            view.SetColumnError(view.Columns[ColName], "لا يوجد حساب بهذا الإسم");
                        }
                    }

                    if (ColName == "EmpCompundName")
                    {

                        DataTable dtGroupID = Lip.SelectRecord("Select EmployeeID  from HR_EmployeeFile WHERE Cancel =0 And  LOWER (" + PrimaryName + ")=LOWER ('" +val.ToString() + "')");
                        if (dtGroupID.Rows.Count > 0)
                        {
                            gridViewBeforCompond.SetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "EmpCompondID", dtGroupID.Rows[0]["EmployeeID"]);

                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = "لا يوجد حساب بهذا الاسم  ";
                            view.SetColumnError(view.Columns[ColName], "لا يوجد حساب بهذا الإسم");
                        }
                    }
                    

                }           
        }

        private void gridViewAfterCompond_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {

            if (this.gridViewAfterCompond.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                e.Valid = true;
                HasColumnErrors = false;
                string ColName = view.FocusedColumn.FieldName;

                if (e.Value != null)
                {


                    if (ColName == "ComStoneNumin")
                        gridViewAfterCompond.SetFocusedRowCellValue("ComStoneCom", Comon.cDbl(e.Value) - Comon.cDbl(Comon.cDbl(gridViewAfterCompond.GetFocusedRowCellValue("ComStoneNumout")) + Comon.cDbl(gridViewAfterCompond.GetFocusedRowCellValue("ComStoneNumlas"))));
                    else if (ColName == "ComWeightStonin")
                    {
                        gridViewAfterCompond.SetFocusedRowCellValue("ComWeightSton", (Comon.cDbl(e.Value) - Comon.cDbl(Comon.cDbl(gridViewAfterCompond.GetFocusedRowCellValue("ComWeightStonOUt")) + Comon.cDbl(gridViewAfterCompond.GetFocusedRowCellValue("ComWeightStonLas")))));
                        gridViewAfterCompond.SetFocusedRowCellValue("ComWeightStonAfter", ((Comon.cDbl(e.Value) - Comon.cDbl(Comon.cDbl(gridViewAfterCompond.GetFocusedRowCellValue("ComWeightStonOUt")) + Comon.cDbl(gridViewAfterCompond.GetFocusedRowCellValue("ComWeightStonLas")))) / 5));
                    }
                    else if (ColName == "ComStoneNumout")
                        gridViewAfterCompond.SetFocusedRowCellValue("ComStoneCom", Comon.cDbl(Comon.cDbl(gridViewAfterCompond.GetFocusedRowCellValue("ComStoneNumin")) - Comon.cDbl(Comon.cDbl(e.Value) + Comon.cDbl(gridViewAfterCompond.GetFocusedRowCellValue("ComStoneNumlas")))));
                    else if (ColName == "ComWeightStonOUt")
                    {
                        gridViewAfterCompond.SetFocusedRowCellValue("ComWeightSton", (Comon.cDbl(Comon.cDbl(gridViewAfterCompond.GetFocusedRowCellValue("ComWeightStonin")) - Comon.cDbl(Comon.cDbl(e.Value) + Comon.cDbl(gridViewAfterCompond.GetFocusedRowCellValue("ComWeightStonLas"))))));
                        gridViewAfterCompond.SetFocusedRowCellValue("ComWeightStonAfter", ( (Comon.cDbl(Comon.cDbl(gridViewAfterCompond.GetFocusedRowCellValue("ComWeightStonin")) - Comon.cDbl(Comon.cDbl(e.Value) + Comon.cDbl(gridViewAfterCompond.GetFocusedRowCellValue("ComWeightStonLas"))))) / 5));

                    }
                    else if (ColName == "ComStoneNumlas")
                        gridViewAfterCompond.SetFocusedRowCellValue("ComStoneCom", Comon.cDbl(Comon.cDbl(gridViewAfterCompond.GetFocusedRowCellValue("ComStoneNumin")) - Comon.cDbl(Comon.cDbl(e.Value) + Comon.cDbl(gridViewAfterCompond.GetFocusedRowCellValue("ComStoneNumout")))));
                    else if (ColName == "ComWeightStonLas")
                    {
                        gridViewAfterCompond.SetFocusedRowCellValue("ComWeightSton", (Comon.cDbl(Comon.cDbl(gridViewAfterCompond.GetFocusedRowCellValue("ComWeightStonin")) - Comon.cDbl(Comon.cDbl(e.Value) + Comon.cDbl(gridViewAfterCompond.GetFocusedRowCellValue("ComWeightStonOUt"))))));
                        gridViewAfterCompond.SetFocusedRowCellValue("ComWeightStonAfter", ((Comon.cDbl(Comon.cDbl(gridViewAfterCompond.GetFocusedRowCellValue("ComWeightStonin")) - Comon.cDbl(Comon.cDbl(e.Value) + Comon.cDbl(gridViewAfterCompond.GetFocusedRowCellValue("ComWeightStonOUt"))))) / 5));

                    }
                  
                    

                  
                }
                if (ColName != "BarcodCompond" && ColName != "TypeSton" && ColName != ItemName && ColName != SizeName && ColName != "DebitTime" && ColName != "DebitDate" && ColName != "FromAccountName" && ColName != "EmpCompundName" && ColName != "ComSignature")
                {
                    if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputIsRequired;
                    }
                    else if (!(double.TryParse(e.Value.ToString(), out num)) && (ColName != "BarcodCompond" && ColName != "TypeSton" && ColName != ItemName && ColName != "FromAccountName" && ColName != "EmpCompundName" && ColName != "ComSignature"))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputShouldBeNumber;
                    }
                    else if (Comon.ConvertToDecimalPrice(e.Value.ToString()) < 0 && (ColName != "BarcodCompond" && ColName != "TypeSton" && ColName != ItemName && ColName != "FromAccountName" && ColName != "EmpCompundName" && ColName != "ComSignature"))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputIsGreaterThanZero;
                    }

                }
                else if (ColName == "BarcodCompond")
                {
                    DataTable dtGroupID = Lip.SelectRecord("Select ArbItemName,ArbItemType from Stc_Items_Find where BarCode='" + e.Value.ToString() + "'");
                    if (dtGroupID.Rows.Count > 0)
                    {
                        //gridViewAfterCompond.SetFocusedRowCellValue("TypeSton", dtGroupID.Rows[0]["ArbItemType"]);
                        gridViewAfterCompond.SetFocusedRowCellValue(ItemName, dtGroupID.Rows[0]["ArbItemName"]);
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = "لا يوجد  حجر  تملك هذا الباركود  ";
                        view.SetColumnError(view.Columns[ColName], "لا يوجد حجر تملك هذا الباركود  ");
                    }
                }
                if (ColName == ItemName)
                {

                    DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_Items_Find  Where  LOWER (" + ItemName + ")=LOWER ('" + val.ToString() + "') ");
                    if (dtItemID.Rows.Count > 0)
                    {
                        FillItemData(gridViewAfterCompond, gridControlAfterCompond, "BarcodCompond", "GoldCredit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", ((TextEdit)txtAccountIDBeforCompond));
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = " الصنف غير موجود  ";
                    }
                }
                if (ColName == SizeName)
                {

                    DataTable dtSizeID = Lip.SelectRecord("Select  SizeID  from Stc_SizingUnits Where  LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') ");
                    if (dtSizeID.Rows.Count > 0)
                    {

                        FillItemData(gridViewAfterCompond, gridControlAfterCompond, "BarcodCompond", "GoldCredit", Stc_itemsDAL.GetItemDataByItemID_SizeID(Comon.cInt(gridViewAfterCompond.GetRowCellValue(gridViewAfterCompond.FocusedRowHandle, "ItemID")), Comon.cInt(dtSizeID.Rows[0][0].ToString()), UserInfo.FacilityID), "DebitDate", "DebitTime", ((TextEdit)txtAccountIDBeforCompond));
                        gridViewAfterCompond.SetRowCellValue(gridViewAfterCompond.FocusedRowHandle, ColName, e.Value.ToString());
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = " الوحده غير موجوده  ";
                    }

                }
                if (ColName == "ItemID")
                {
                    DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_ItemUnits  Where ItemID=" + Comon.cInt(e.Value));
                    if (dtItemID.Rows.Count > 0)
                    {
                        FillItemData(gridViewAfterCompond, gridControlAfterCompond, "BarcodCompond", "GoldCredit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", txtAccountIDBeforCompond);
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = "رقم الصنف غير موجود  ";
                    }
                }
                if (ColName == "ComStoneNumout")
                {

                    if (Comon.cDbl(e.Value) > Comon.cDbl(gridViewAfterCompond.GetRowCellValue(gridViewAfterCompond.FocusedRowHandle, "ComStoneNumin").ToString()))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;

                        e.ErrorText = Messages.msgNumStounOutGreterThanNumStonin;
                        view.SetColumnError(view.Columns[ColName], Messages.msgNumStounOutGreterThanNumStonin);
                    }

                    if ((Comon.cDbl(e.Value) + Comon.cDbl(gridViewAfterCompond.GetRowCellValue(gridViewAfterCompond.FocusedRowHandle, "ComStoneNumlas") != null ? gridViewAfterCompond.GetRowCellValue(gridViewAfterCompond.FocusedRowHandle, "ComStoneNumlas").ToString() : 0 + "")) > Comon.cDbl(gridViewAfterCompond.GetRowCellValue(gridViewAfterCompond.FocusedRowHandle, "ComStoneNumin").ToString()))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNumStounOutNumStonLosGreterThenNumStonin;
                        view.SetColumnError(view.Columns[ColName], Messages.msgNumStounOutNumStonLosGreterThenNumStonin);
                    }

                    else
                    {
                        e.Valid = true;
                        HasColumnErrors = false;
                        e.ErrorText = "";
                        view.SetColumnError(view.Columns[ColName], "");

                    }

                }
                if (ColName == "ComWeightStonOUt")
                {

                    if (Comon.cDbl(e.Value) > Comon.cDbl(gridViewAfterCompond.GetRowCellValue(gridViewAfterCompond.FocusedRowHandle, "ComWeightStonin").ToString()))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;

                        e.ErrorText = Messages.msgWeightStounOutGreterThanWeightStonin;
                        view.SetColumnError(view.Columns[ColName], Messages.msgWeightStounOutGreterThanWeightStonin);
                    }

                    if ((Comon.cDbl(e.Value) + Comon.cDbl(gridViewAfterCompond.GetRowCellValue(gridViewAfterCompond.FocusedRowHandle, "ComWeightStonLas") != null ? gridViewAfterCompond.GetRowCellValue(gridViewAfterCompond.FocusedRowHandle, "ComWeightStonLas").ToString() : 0 + "")) > Comon.cDbl(gridViewAfterCompond.GetRowCellValue(gridViewAfterCompond.FocusedRowHandle, "ComWeightStonin").ToString()))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgWeightStounOutWeightStonLosGreterThenWeightStonin;
                        view.SetColumnError(view.Columns[ColName], Messages.msgWeightStounOutWeightStonLosGreterThenWeightStonin);
                    }

                    else
                    {
                        e.Valid = true;
                        HasColumnErrors = false;
                        e.ErrorText = "";
                        view.SetColumnError(view.Columns[ColName], "");

                    }

                }
                if (ColName == "ComStoneNumlas")
                {

                    if (Comon.cDbl(e.Value) > Comon.cDbl(gridViewAfterCompond.GetRowCellValue(gridViewAfterCompond.FocusedRowHandle, "ComStoneNumin").ToString()))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;

                        e.ErrorText = Messages.msgNumStounLostGreterThanNumStonin;
                        view.SetColumnError(view.Columns[ColName], Messages.msgNumStounLostGreterThanNumStonin);
                    }

                    if ((Comon.cDbl(e.Value) + Comon.cDbl(gridViewAfterCompond.GetRowCellValue(gridViewAfterCompond.FocusedRowHandle, "ComStoneNumout") != null ? gridViewAfterCompond.GetRowCellValue(gridViewAfterCompond.FocusedRowHandle, "ComStoneNumout").ToString() : 0 + "")) > Comon.cDbl(gridViewAfterCompond.GetRowCellValue(gridViewAfterCompond.FocusedRowHandle, "ComStoneNumin").ToString()))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNumStounOutNumStonLosGreterThenNumStonin;
                        view.SetColumnError(view.Columns[ColName], Messages.msgNumStounOutNumStonLosGreterThenNumStonin);
                    }

                    else
                    {
                        e.Valid = true;
                        HasColumnErrors = false;
                        e.ErrorText = "";
                        view.SetColumnError(view.Columns[ColName], "");

                    }

                }
                if (ColName == "ComWeightStonLas")
                {

                    if (Comon.cDbl(e.Value) > Comon.cDbl(gridViewAfterCompond.GetRowCellValue(gridViewAfterCompond.FocusedRowHandle, "ComWeightStonin").ToString()))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;


                        e.ErrorText = Messages.msgWeightStounLostGreterThanWeightStonin;
                        view.SetColumnError(view.Columns[ColName], Messages.msgWeightStounLostGreterThanWeightStonin);
                    }
                    if ((Comon.cDbl(e.Value) + Comon.cDbl(gridViewAfterCompond.GetRowCellValue(gridViewAfterCompond.FocusedRowHandle, "ComWeightStonOUt") != null ? gridViewAfterCompond.GetRowCellValue(gridViewAfterCompond.FocusedRowHandle, "ComWeightStonOUt").ToString() : 0 + "")) > Comon.cDbl(gridViewAfterCompond.GetRowCellValue(gridViewAfterCompond.FocusedRowHandle, "ComWeightStonin").ToString()))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgWeightStounOutWeightStonLosGreterThenWeightStonin;
                        view.SetColumnError(view.Columns[ColName], Messages.msgWeightStounOutWeightStonLosGreterThenWeightStonin);
                    }
                    else
                    {
                        e.Valid = true;
                        HasColumnErrors = false;
                        e.ErrorText = "";
                        view.SetColumnError(view.Columns[ColName], "");
                    }
                }
                else if (ColName == "FromAccountID")
                {

                    DataTable dtGroupID = Lip.SelectRecord("Select ArbName from Acc_Accounts WHERE Cancel =0 And  AccountID=" + e.Value.ToString());
                    if (dtGroupID.Rows.Count > 0)
                    {
                        gridViewAfterCompond.SetRowCellValue(gridViewAfterCompond.FocusedRowHandle, "FromAccountName", dtGroupID.Rows[0]["ArbName"]);

                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = "لا يوجد حساب بهذا الرقم  ";
                        view.SetColumnError(view.Columns[ColName], "لا يوجد حساب بهذا الرقم");
                    }
                }

                if (ColName == "EmpCompondID")
                {

                    DataTable dtGroupID = Lip.SelectRecord("Select ArbName from HR_EmployeeFile WHERE Cancel =0 And  EmployeeID=" + e.Value);
                    if (dtGroupID.Rows.Count > 0)
                    {
                        gridViewAfterCompond.SetRowCellValue(gridViewAfterCompond.FocusedRowHandle, "EmpCompundName", dtGroupID.Rows[0]["ArbName"]);

                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = "لا يوجد حساب بهذا الرقم  ";
                        view.SetColumnError(view.Columns[ColName], "لا يوجد حساب بهذا الرقم");
                    }
                }
                else if (ColName == "FromAccountName")
                {

                    DataTable dtGroupID = Lip.SelectRecord("Select AccountID  from Acc_Accounts WHERE Cancel =0 And  LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "')");
                    if (dtGroupID.Rows.Count > 0)
                    {
                        gridViewAfterCompond.SetRowCellValue(gridViewAfterCompond.FocusedRowHandle, "FromAccountID", dtGroupID.Rows[0]["AccountID"]);

                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = "لا يوجد حساب بهذا الاسم  ";
                        view.SetColumnError(view.Columns[ColName], "لا يوجد حساب بهذا الإسم");
                    }
                }

                if (ColName == "EmpCompundName")
                {

                    DataTable dtGroupID = Lip.SelectRecord("Select EmployeeID  from HR_EmployeeFile WHERE Cancel =0 And  LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "')");
                    if (dtGroupID.Rows.Count > 0)
                    {
                        gridViewAfterCompond.SetRowCellValue(gridViewAfterCompond.FocusedRowHandle, "EmpCompondID", dtGroupID.Rows[0]["EmployeeID"]);

                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = "لا يوجد حساب بهذا الاسم  ";
                        view.SetColumnError(view.Columns[ColName], "لا يوجد حساب بهذا الإسم");
                    }
                }

            }
        }
        private void GridProductionExpenses_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {

            if (this.GridProductionExpenses.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                e.Valid = true;
                HasColumnErrors = false;
                string ColName = view.FocusedColumn.FieldName;
               
                if (e.Value != null)
                {

                    if (ColName == "MainValue")
                        GridProductionExpenses.SetFocusedRowCellValue("Installment", Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(e.Value)/12 / 30));
                    else if (ColName == "PeriodInDays")
                    {
                        GridProductionExpenses.SetFocusedRowCellValue("AverageHoursPerDay", Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(e.Value) * Comon.ConvertToDecimalPrice(1.5)));
                        GridProductionExpenses.SetFocusedRowCellValue("OrderCostPercentage", Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(GridProductionExpenses.GetFocusedRowCellValue("Installment")) / Comon.ConvertToDecimalPrice(GridProductionExpenses.GetFocusedRowCellValue("AverageHoursPerDay"))));
                    }
                    else if (ColName == "DepreciationPercentage")
                    {
                        if (Comon.ConvertToDecimalPrice(e.Value) > 0)
                            GridProductionExpenses.SetFocusedRowCellValue("OrderCostPercentage", Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(GridProductionExpenses.GetFocusedRowCellValue("Installment")) / Comon.ConvertToDecimalPrice(GridProductionExpenses.GetFocusedRowCellValue("AverageHoursPerDay"))) * Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(e.Value) / Comon.ConvertToDecimalPrice(100))));
                        else
                        {
                            if (Comon.ConvertToDecimalPrice(GridProductionExpenses.GetFocusedRowCellValue("AverageHoursPerDay")) != 0)
                             GridProductionExpenses.SetFocusedRowCellValue("OrderCostPercentage", Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(GridProductionExpenses.GetFocusedRowCellValue("Installment")) / Comon.ConvertToDecimalPrice(GridProductionExpenses.GetFocusedRowCellValue("AverageHoursPerDay"))));
                        }
                    }
                    if (ColName == "MainValue" && Comon.ConvertToDecimalPrice(GridProductionExpenses.GetFocusedRowCellValue("AverageHoursPerDay")) != 0)
                        GridProductionExpenses.SetFocusedRowCellValue("OrderCostPercentage", Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(e.Value) / 12 / 30)) / Comon.ConvertToDecimalPrice(GridProductionExpenses.GetFocusedRowCellValue("AverageHoursPerDay"))) * Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(GridProductionExpenses.GetFocusedRowCellValue("DepreciationPercentage")) / Comon.ConvertToDecimalPrice(100))));
                     
                }
                if (ColName == "AccountID" || ColName == "AccountName"||ColName=="AverageHoursPerDay")
                {
                    if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputIsRequired;
                    }
                    else if (!(double.TryParse(e.Value.ToString(), out num)) && (ColName != "AccountName"))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputShouldBeNumber;
                    }
                    else if (Comon.ConvertToDecimalPrice(e.Value.ToString()) < 0 && (ColName != "AccountName"))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputIsGreaterThanZero;
                    }
                }
                
                if (ColName == "AccountID")
                {

                    DataTable dtGroupID = Lip.SelectRecord("Select ArbName from Acc_Accounts WHERE Cancel =0 And  AccountID=" + e.Value.ToString());
                    if (dtGroupID.Rows.Count > 0)
                    {
                        GridProductionExpenses.SetRowCellValue(GridProductionExpenses.FocusedRowHandle, "AccountName", dtGroupID.Rows[0]["ArbName"]);
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = "لا يوجد حساب بهذا الرقم  ";
                        view.SetColumnError(view.Columns[ColName], "لا يوجد حساب بهذا الرقم");
                    }
                }   
                 if (ColName == "AccountName")
                {
                    DataTable dtGroupID = Lip.SelectRecord("Select AccountID  from Acc_Accounts WHERE Cancel =0 And  LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "')");
                    if (dtGroupID.Rows.Count > 0)
                    {
                        GridProductionExpenses.SetRowCellValue(GridProductionExpenses.FocusedRowHandle, "AccountID", dtGroupID.Rows[0]["AccountID"]);
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = "لا يوجد حساب بهذا الاسم  ";
                        view.SetColumnError(view.Columns[ColName], "لا يوجد حساب بهذا الإسم");
                    }
                }

             

            }
        }
        private void GridViewAfterfactory_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
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

                    
                        DataTable dtGroupID = Lip.SelectRecord("Select ArbName from Menu_FactoryMachine Where MachineID=" + Comon.cInt(e.Value));
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
                        DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_ItemUnits  Where ItemID=" + Comon.cInt(e.Value));
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

                        DataTable dtItemID = Lip.SelectRecord("Select  ArbName from Stc_SizingUnits  Where SizeID=" + e.Value);
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridViewAfterfactory.SetRowCellValue(GridViewAfterfactory.FocusedRowHandle, GridViewAfterfactory.Columns[SizeName], dtItemID.Rows[0]["ArbName"].ToString());
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
                        DataTable dtNameEmp = Lip.SelectRecord("Select " + PrimaryName + " from HR_EmployeeFile  Where EmployeeID=" + Comon.cInt(e.Value));


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
                        DataTable dtItemID = Lip.SelectRecord("Select  " + PrimaryName + " from  Stc_Stores Where Cancel=0 And StoreID=" + val.ToString() + " And FacilityID=" + UserInfo.FacilityID);
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
                   
                    DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_Items_Find  Where  LOWER (" + ItemName + ")=LOWER ('" + val.ToString() + "') ");
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
                 
                if (ColName == SizeName)
                {
                    DataTable dtSizeID = Lip.SelectRecord("Select  SizeID  from Stc_SizingUnits Where  LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') ");
                    if (dtSizeID.Rows.Count > 0)
                    {

                        FillItemData(GridViewAfterfactory, gridControlAfterFactory, "BarCode", "Credit", Stc_itemsDAL.GetItemDataByItemID_SizeID(Comon.cInt(GridViewAfterfactory.GetRowCellValue(GridViewAfterfactory.FocusedRowHandle, "ItemID")), Comon.cInt(dtSizeID.Rows[0][0].ToString()), UserInfo.FacilityID), "DebitDate", "DebitTime", ((TextEdit)txtAccountIDFactory));
                        GridViewAfterfactory.SetRowCellValue(GridViewAfterfactory.FocusedRowHandle, ColName, e.Value.ToString());
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = " الوحده غير موجوده  ";
                    }

                }
                if (ColName == "EmpName")
                {

                    DataTable dtMachinID = Lip.SelectRecord("Select  EmployeeID  from HR_EmployeeFile Where  LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') ");
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
                    DataTable dtItemID = Lip.SelectRecord("Select StoreID from Stc_Stores Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
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

        private void FillItemData(GridView Grid,GridControl GridControl,string BarCode,string QTYFildName, DataTable dt,string Date,string Time,TextEdit ObjtxtAccount)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                if (ObjtxtAccount != txtAccountIDBeforCompond)
                {
                    if (Comon.cDbl(ObjtxtAccount.Text) > 0)
                    {
                        DataTable dtMachine = Lip.SelectRecord("SELECT   [MachineID] ,[ArbName] FROM  [Menu_FactoryMachine] where [AccountID]='" + Comon.cDbl(ObjtxtAccount.Text) + "'  and Cancel=0");
                        if (dtMachine.Rows.Count > 0)
                        {
                            // Grid.AddNewRow();
                            FileDataMachinName(Grid, Date, Time, Comon.cInt(dtMachine.Rows[0]["MachineID"].ToString()));
                        }
                        else
                        {
                            Messages.MsgNone(Messages.TitleWorning, Messages.msgAccountNotHaveMachine);
                            ObjtxtAccount.Text = "";
                            ObjtxtAccount.Focus();
                            return;
                        }
                    }
                    else
                    {
                        Messages.MsgAsterisk(Messages.msgInputIsRequired, "  الرجاء إدخال رقم الحساب الخاص بهذه المرحلة ");
                        ObjtxtAccount.Focus();
                        return;
                    }
                }
                else  
                {
                    Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[Time], DateTime.Now.ToString("hh:mm:tt"));
                    Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[Date], DateTime.Now.ToString("yyyy/MM/dd"));

                }
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[QTYFildName], 0);
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[BarCode], dt.Rows[0]["BarCode"].ToString().ToUpper());
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns["ItemID"], dt.Rows[0]["ItemID"].ToString());
                Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[ItemName], dt.Rows[0]["ArbName"].ToString());

                RepositoryItemLookUpEdit rSize = Common.LookUpEditSize(Comon.cDbl(dt.Rows[0]["ItemID"].ToString()));
                Grid.Columns[SizeName].ColumnEdit = rSize;
                GridControl.RepositoryItems.Add(rSize);
             
                Grid.Columns[SizeName].OptionsColumn.AllowEdit = true;
                Grid.Columns[SizeName].OptionsColumn.AllowFocus = true;
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
                        DataTable dtGroupID = Lip.SelectRecord("Select ArbName from Menu_FactoryMachine Where MachineID=" + Comon.cInt(e.Value));
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
                        DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_ItemUnits  Where ItemID=" + Comon.cInt(e.Value));
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

                    if (ColName == "SizeID")
                    {

                        DataTable dtItemID = Lip.SelectRecord("Select  ArbName from Stc_SizingUnits  Where SizeID=" + e.Value);
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridViewBeforfactory.SetRowCellValue(GridViewBeforfactory.FocusedRowHandle, GridViewBeforfactory.Columns[SizeName], dtItemID.Rows[0]["ArbName"].ToString());
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
                        DataTable dtNameEmp = Lip.SelectRecord("Select " + PrimaryName + " from HR_EmployeeFile  Where EmployeeID=" + Comon.cInt(e.Value));


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
                        DataTable dtItemID = Lip.SelectRecord("Select  " + PrimaryName + " from  Stc_Stores Where Cancel=0 And StoreID=" + val.ToString() + " And FacilityID=" + UserInfo.FacilityID);
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
                    DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_Items_Find  Where  LOWER (" + ItemName + ")=LOWER ('" + val.ToString() + "') ");
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

                    DataTable dtBarCode = Lip.SelectRecord("Select  BarCode from Stc_Items_Find   Where  LOWER (" + SizeName + ")=LOWER ('" + e.Value.ToString() + "') and ItemID=" + Comon.cInt(GridViewBeforfactory.GetRowCellValue(GridViewBeforfactory.FocusedRowHandle, "ItemID")) + " ");
                    if (dtBarCode.Rows.Count > 0)
                    {
                      
                        FillItemData(GridViewBeforfactory, gridControlfactroOpretion, "BarCode", "Debit", Stc_itemsDAL.GetItemData1(dtBarCode.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", ((TextEdit)txtAccountIDFactory));
                        GridViewBeforfactory.SetRowCellValue(GridViewBeforfactory.FocusedRowHandle, ColName, e.Value.ToString());
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = " الوحده غير موجوده  ";
                    }
                }
                if (ColName == "EmpName")
                {

                    DataTable dtMachinID = Lip.SelectRecord("Select  EmployeeID  from HR_EmployeeFile Where  LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') ");
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
                    DataTable dtItemID = Lip.SelectRecord("Select StoreID from Stc_Stores Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
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
     
        private void GridViewBeforPrentag_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {

            if (this.GridViewBeforPrentag.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                HasColumnErrors = false;
                string ColName = view.FocusedColumn.FieldName;
                int ItemID = 0;
                if (ColName == "EmpID" || ColName == "SizeID" || ColName=="StoreID"||  ColName == "ItemID" || ColName == "MachinID" || ColName == "PrentagCredit" || ColName == "PrentagDebit")
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
                        DataTable dtMachinID = Lip.SelectRecord("Select ArbName from Menu_FactoryMachine Where MachineID=" + Comon.cInt(e.Value));
                        if (dtMachinID.Rows.Count > 0)
                        {
                           
                            FileDataMachinName(GridViewBeforPrentag, "PrentagDebitDate", "PrentagDebitTime", Comon.cInt(e.Value));
                    
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
                        DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_ItemUnits  Where ItemID=" + Comon.cInt(e.Value));
                        if (dtItemID.Rows.Count > 0)
                        {

                            FillItemData(GridViewBeforPrentag, gridControlBeforPrentag, "BarcodePrentag", "PrentagDebit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "PrentagDebitDate", "PrentagDebitTime", txtAccountIDPrentage);
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
                        DataTable dtItemID = Lip.SelectRecord("Select  ArbName from Stc_SizingUnits  Where SizeID=" + e.Value);
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridViewBeforPrentag.SetRowCellValue(GridViewBeforPrentag.FocusedRowHandle, GridViewBeforPrentag.Columns[SizeName], dtItemID.Rows[0]["ArbName"].ToString());
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
                        DataTable dtNameEmp = Lip.SelectRecord("Select "+PrimaryName+" from HR_EmployeeFile  Where EmployeeID=" + Comon.cInt(e.Value));
                        e.Valid = true ;
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
                        DataTable dtItemID = Lip.SelectRecord("Select  " + PrimaryName + " from  Stc_Stores Where Cancel=0 And StoreID=" + val.ToString() + " And FacilityID=" + UserInfo.FacilityID);
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
                if (ColName == ItemName)
                {
                    
                    DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_Items_Find  Where  LOWER (" + ItemName + ")=LOWER ('" + val.ToString() + "') ");
                    if (dtItemID.Rows.Count > 0)
                    {

                        FillItemData(GridViewBeforPrentag, gridControlBeforPrentag, "BarcodePrentag", "PrentagDebit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "PrentagDebitDate", "PrentagDebitTime", ((TextEdit)txtAccountIDPrentage));
                      
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = "رقم الصنف غير موجود  ";
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
                    DataTable dtSizeID = Lip.SelectRecord("Select  SizeID  from Stc_SizingUnits Where  LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') ");
                    if (dtSizeID.Rows.Count > 0)
                    {

                        FillItemData(GridViewBeforPrentag, gridControlBeforPrentag, "BarcodePrentag", "PrentagDebit", Stc_itemsDAL.GetItemDataByItemID_SizeID(Comon.cInt(GridViewBeforPrentag.GetRowCellValue(GridViewBeforPrentag.FocusedRowHandle, "ItemID")), Comon.cInt(dtSizeID.Rows[0][0].ToString()), UserInfo.FacilityID), "PrentagDebitDate", "PrentagDebitTime", ((TextEdit)txtAccountIDPrentage));
                        GridViewBeforPrentag.SetRowCellValue(GridViewBeforPrentag.FocusedRowHandle, ColName, e.Value.ToString());
                    }                 
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = " الوحده غير موجوده  ";
                    }
                }
                if (ColName == "EmpName")
                {
                    DataTable dtMachinID = Lip.SelectRecord("Select  EmployeeID  from HR_EmployeeFile Where  LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') ");
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
                    DataTable dtItemID = Lip.SelectRecord("Select StoreID from Stc_Stores Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
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
        private void GridViewAfterPrentag_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {

            if (this.GridViewAfterPrentag.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                HasColumnErrors = false;
                string ColName = view.FocusedColumn.FieldName;
                int ItemID = 0;
                if (ColName == "EmpID" || ColName == "SizeID" || ColName == "StoreID" || ColName == "ItemID" || ColName == "MachinID" || ColName == "PrentagCredit" || ColName == "PrentagDebit")
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
                        DataTable dtGroupID = Lip.SelectRecord("Select ArbName from Menu_FactoryMachine Where MachineID=" + Comon.cInt(e.Value));
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

                        DataTable dtItemID = Lip.SelectRecord("Select BarCode from Stc_ItemUnits  Where ItemID=" + Comon.cInt(e.Value));
                        
                        if (dtItemID.Rows.Count > 0)
                        {

                            FillItemData(GridViewAfterPrentag, gridControlAfterPrentage, "BarcodePrentag", "PrentagCredit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "PrentagDebitDate", "PrentagDebitTime", txtAccountIDPrentage);
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
                        DataTable dtItemID = Lip.SelectRecord("Select  ArbName from Stc_SizingUnits  Where SizeID=" + e.Value);
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridViewAfterPrentag.SetRowCellValue(GridViewAfterPrentag.FocusedRowHandle, GridViewAfterPrentag.Columns[SizeName], dtItemID.Rows[0]["ArbName"].ToString());
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = "رقم الوحدة  غير موجود  ";
                        }
                    }
                    if (ColName == "EmpID")
                    {
                        DataTable dtNameEmp = Lip.SelectRecord("Select " + PrimaryName + " from HR_EmployeeFile  Where EmployeeID=" + Comon.cInt(e.Value));
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
                        DataTable dtItemID = Lip.SelectRecord("Select  " + PrimaryName + " from  Stc_Stores Where Cancel=0 And StoreID=" + val.ToString() + " And FacilityID=" + UserInfo.FacilityID);
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

                    DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_Items_Find  Where  LOWER (" + ItemName + ")=LOWER ('" + val.ToString() + "') ");
                    if (dtItemID.Rows.Count > 0)
                    {

                        FillItemData(GridViewAfterPrentag, gridControlAfterPrentage, "BarcodePrentag", "PrentagDebit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "PrentagDebitDate", "PrentagDebitTime", ((TextEdit)txtAccountIDPrentage));

                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = "رقم الصنف غير موجود  ";
                    }
                }

                if (ColName == "MachineName")
                {

                    DataTable dtMachinID = Lip.SelectRecord("Select  MachineID  from Menu_FactoryMachine Where  LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') ");
                    if (dtMachinID.Rows.Count > 0)
                    {

                        FileDataMachinName(GridViewAfterPrentag, "PrentagDebitDate", "PrentagDebitTime", Comon.cInt(dtMachinID.Rows[0]["MachineID"].ToString()));
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

                    DataTable dtSizeID = Lip.SelectRecord("Select  SizeID  from Stc_SizingUnits Where  LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') ");
                    if (dtSizeID.Rows.Count > 0)
                    {

                        FillItemData(GridViewAfterPrentag, gridControlAfterPrentage, "BarcodePrentag", "PrentagDebit", Stc_itemsDAL.GetItemDataByItemID_SizeID(Comon.cInt(GridViewAfterPrentag.GetRowCellValue(GridViewAfterPrentag.FocusedRowHandle, "ItemID")), Comon.cInt(dtSizeID.Rows[0][0].ToString()), UserInfo.FacilityID), "PrentagDebitDate", "PrentagDebitTime", ((TextEdit)txtAccountIDPrentage));
                        GridViewAfterPrentag.SetRowCellValue(GridViewAfterPrentag.FocusedRowHandle, ColName, e.Value.ToString());
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = " الوحده غير موجوده  ";
                    }
                   
                }
                if (ColName == "EmpName")
                {

                    DataTable dtMachinID = Lip.SelectRecord("Select  EmployeeID  from HR_EmployeeFile Where  LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') ");
                    if (dtMachinID.Rows.Count > 0)
                    {
                        GridViewAfterPrentag.SetFocusedRowCellValue("EmpID", dtMachinID.Rows[0]["EmployeeID"].ToString());

                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = "العامل غير موجود  ";
                    }
                }
                else if (ColName == "StoreName")
                {
                    DataTable dtItemID = Lip.SelectRecord("Select StoreID from Stc_Stores Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
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
        private void GridViewBeforPolish_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {

            if (this.GridViewBeforPolish.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                HasColumnErrors = false;
                string ColName = view.FocusedColumn.FieldName;

                if (ColName == "MachinID" || ColName == "Debit" || ColName == "StoreID"   || ColName == "SizeID" || ColName == "ItemID" || ColName == "EmpID"  )
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
                        DataTable dtGroupID = Lip.SelectRecord("Select ArbName from Menu_FactoryMachine Where MachineID=" + Comon.cInt(e.Value));
                        if (dtGroupID.Rows.Count > 0)
                        {

                            FileDataMachinName(GridViewBeforPolish, "DebitDate", "DebitTime", Comon.cInt(e.Value));  
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = "رقم المكينة غير موجود  ";
                        }
                    }
                    else if (ColName == "SizeID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select " + PrimaryName + " from Stc_SizingUnits Where Cancel=0 And LOWER (SizeID)=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                        if (dtItemID.Rows.Count > 0)
                        {

                            GridViewBeforPolish.SetFocusedRowCellValue(SizeName, dtItemID.Rows[0][PrimaryName]);
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
                    else if (ColName == "StoreID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select  " + PrimaryName + " from  Stc_Stores Where Cancel=0 And StoreID=" + val.ToString() + " And FacilityID=" + UserInfo.FacilityID);
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

                    else if (ColName == "EmpID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("SELECT   " + PrimaryName + "  FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(val.ToString()) + " And Cancel =0 ");
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridViewBeforPolish.SetFocusedRowCellValue("EmpName", dtItemID.Rows[0][PrimaryName]);
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
                    if (ColName == "ItemID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_ItemUnits  Where ItemID=" + Comon.cInt(e.Value));
                        if (dtItemID.Rows.Count > 0)
                        {
                            FillItemData(GridViewBeforPolish, gridControlBeforePolishing, "BarcodeTalmee", "Debit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", txtAccountIDPolishing);
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = "رقم الصنف غير موجود  ";
                        }
                    }
                    else if (ColName == "StoreID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select  " + PrimaryName + " from  Stc_Stores Where Cancel=0 And StoreID=" + val.ToString() + " And FacilityID=" + UserInfo.FacilityID);
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
                else if (ColName == SizeName)
                {
                    DataTable dtSizeID = Lip.SelectRecord("Select  SizeID  from Stc_SizingUnits Where  LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') ");
                    if (dtSizeID.Rows.Count > 0)
                    {

                        FillItemData(GridViewBeforPolish, gridControlBeforePolishing, "BarcodeTalmee", "Debit", Stc_itemsDAL.GetItemDataByItemID_SizeID(Comon.cInt(GridViewBeforPolish.GetRowCellValue(GridViewBeforPolish.FocusedRowHandle, "ItemID")), Comon.cInt(dtSizeID.Rows[0][0].ToString()), UserInfo.FacilityID), "DebitDate", "DebitTime", ((TextEdit)txtAccountIDPolishing));
                        GridViewBeforPolish.SetRowCellValue(GridViewBeforPolish.FocusedRowHandle, ColName, e.Value.ToString());
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = " الوحده غير موجوده  ";
                    }
                }
                else if (ColName == "StoreName")
                {
                    DataTable dtItemID = Lip.SelectRecord("Select StoreID from Stc_Stores Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
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

                else if (ColName == "EmpName")
                {
                    DataTable dtItemID = Lip.SelectRecord("Select EmployeeID as EmpID from HR_EmployeeFile Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') ");
                    if (dtItemID.Rows.Count > 0)
                    {
                        GridViewBeforPolish.SetFocusedRowCellValue("EmpID", dtItemID.Rows[0]["EmpID"]);
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
                if (ColName == ItemName)
                {

                    DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_Items_Find  Where  LOWER (" + ItemName + ")=LOWER ('" + val.ToString() + "') ");
                    if (dtItemID.Rows.Count > 0)
                    {

                        FillItemData(GridViewBeforPolish, gridControlBeforePolishing, "BarcodeTalmee", "Debit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", ((TextEdit)txtAccountIDPolishing));

                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = "رقم الصنف غير موجود  ";
                    }
                }
                else if (ColName == "StoreName")
                {
                    DataTable dtItemID = Lip.SelectRecord("Select StoreID from Stc_Stores Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
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
        private void GridViewAfterPolish_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {

            if (this.GridViewAfterPolish.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                HasColumnErrors = false;
                string ColName = view.FocusedColumn.FieldName;

                if (ColName == "MachinID" || ColName == "StoreID" ||   ColName == "Credit" || ColName == "SizeID" || ColName == "ItemID" || ColName == "EmpID")
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
                        DataTable dtGroupID = Lip.SelectRecord("Select ArbName from Menu_FactoryMachine Where MachineID=" + Comon.cInt(e.Value));
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
                    else if (ColName == "SizeID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select " + PrimaryName + " from Stc_SizingUnits Where Cancel=0 And LOWER (SizeID)=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                        if (dtItemID.Rows.Count > 0)
                        {

                            GridViewAfterPolish.SetFocusedRowCellValue(SizeName, dtItemID.Rows[0][PrimaryName]);
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
                    else if (ColName == "StoreID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select  " + PrimaryName + " from  Stc_Stores Where Cancel=0 And StoreID=" + val.ToString() + " And FacilityID=" + UserInfo.FacilityID);
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
                    else if (ColName == "EmpID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("SELECT   " + PrimaryName + "  FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(val.ToString()) + " And Cancel =0 ");
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridViewAfterPolish.SetFocusedRowCellValue("EmpName", dtItemID.Rows[0][PrimaryName]);
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
                    if (ColName == "ItemID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_ItemUnits  Where ItemID=" + Comon.cInt(e.Value));
                        if (dtItemID.Rows.Count > 0)
                        {
                            FillItemData(GridViewAfterPolish, gridControlAfterPolishing, "BarcodeTalmee", "Credit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", txtAccountIDPolishing);
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = "رقم الصنف غير موجود  ";
                        }
                    }
                }
                else if (ColName == SizeName)
                {
                    DataTable dtSizeID = Lip.SelectRecord("Select  SizeID  from Stc_SizingUnits Where  LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') ");
                    if (dtSizeID.Rows.Count > 0)
                    {

                        FillItemData(GridViewAfterPolish, gridControlAfterPolishing, "BarcodeTalmee", "Credit", Stc_itemsDAL.GetItemDataByItemID_SizeID(Comon.cInt(GridViewAfterPolish.GetRowCellValue(GridViewAfterPolish.FocusedRowHandle, "ItemID")), Comon.cInt(dtSizeID.Rows[0][0].ToString()), UserInfo.FacilityID), "DebitDate", "DebitTime", ((TextEdit)txtAccountIDPolishing));
                        GridViewAfterPolish.SetRowCellValue(GridViewAfterPolish.FocusedRowHandle, ColName, e.Value.ToString());
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = " الوحده غير موجوده  ";
                    }
                }                
                if (ColName == "MachineName")
                {

                    DataTable dtMachinID = Lip.SelectRecord("Select  MachineID  from Menu_FactoryMachine Where  LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') ");
                    if (dtMachinID.Rows.Count > 0)
                    {
                        FileDataMachinName(GridViewAfterPolish, "DebitDate", "DebitTime", Comon.cInt(dtMachinID.Rows[0]["MachineID"].ToString()));
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = " المكينة غير موجوده  ";
                    }
                }

                else if (ColName == "EmpName")
                {
                    DataTable dtItemID = Lip.SelectRecord("Select EmployeeID as EmpID from HR_EmployeeFile Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') ");
                    if (dtItemID.Rows.Count > 0)
                    {
                        GridViewAfterPolish.SetFocusedRowCellValue("EmpID", dtItemID.Rows[0]["EmpID"]);
                        e.Valid = true;
                        view.SetColumnError(GridViewAfterPolish.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText ="العامل غير موجود";
                        view.SetColumnError(GridViewAfterPolish.Columns[ColName], "العامل غير موجود");
                    }
                }
                if (ColName == ItemName)
                {

                    DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_Items_Find  Where  LOWER (" + ItemName + ")=LOWER ('" + val.ToString() + "') ");
                    if (dtItemID.Rows.Count > 0)
                    {

                        FillItemData(GridViewAfterPolish, gridControlAfterPolishing, "BarcodeTalmee", "Credit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", ((TextEdit)txtAccountIDPolishing));

                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = "رقم الصنف غير موجود  ";
                    }
                }
                else if (ColName == "StoreName")
                {
                    DataTable dtItemID = Lip.SelectRecord("Select StoreID from Stc_Stores Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
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

        private void gridViewAdditional_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {

            if (this.gridViewAdditional.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                HasColumnErrors = false;
                string ColName = view.FocusedColumn.FieldName;

                if (ColName == "MachinID" || ColName == "StoreID" || ColName == "PrentagCredit" || ColName == "PrentagDebit" || ColName == "SizeID" || ColName == "ItemID" || ColName == "EmpID" || ColName == SizeName)
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
                        view.SetColumnError(gridViewAdditional.Columns[ColName], "");
                    }

                    if (ColName == "MachinID")
                    {
                        DataTable dtGroupID = Lip.SelectRecord("Select ArbName from Menu_FactoryMachine Where MachineID=" + Comon.cInt(e.Value));
                        if (dtGroupID.Rows.Count > 0)
                        {
                            gridViewAdditional.SetRowCellValue(gridViewAdditional.FocusedRowHandle, gridViewAdditional.Columns["MachineName"], dtGroupID.Rows[0]["ArbName"].ToString());
                            e.Valid = true;
                            view.SetColumnError(gridViewAdditional.Columns[ColName], "");
                            gridViewAdditional.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                            gridViewAdditional.FocusedColumn = gridViewAdditional.VisibleColumns[0];
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = "رقم المكينة غير موجود  ";
                        }
                    }
                    else if (ColName == "SizeID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select " + PrimaryName + " from Stc_SizingUnits Where Cancel=0 And LOWER (SizeID)=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                        if (dtItemID.Rows.Count > 0)
                        {

                            gridViewAdditional.SetFocusedRowCellValue(SizeName, dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(gridViewAdditional.Columns[ColName], "");

                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(gridViewAdditional.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }
                    else if (ColName == "StoreID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select  " + PrimaryName + " from  Stc_Stores Where Cancel=0 And StoreID=" + val.ToString() + " And FacilityID=" + UserInfo.FacilityID);
                        if (dtItemID.Rows.Count > 0)
                        {
                            gridViewAdditional.SetFocusedRowCellValue("StoreName", dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(gridViewAdditional.Columns[ColName], "");

                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(gridViewAdditional.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }

                    else if (ColName == "EmpID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("SELECT   " + PrimaryName + "  FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(val.ToString()) + " And Cancel =0 ");
                        if (dtItemID.Rows.Count > 0)
                        {
                            gridViewAdditional.SetFocusedRowCellValue("EmpName", dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(gridViewAdditional.Columns[ColName], "");
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(gridViewAdditional.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }
                    if (ColName == "ItemID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where ItemID=" + Comon.cInt(e.Value));
                        if (dtItemID.Rows.Count > 0)
                        {
                            gridViewAdditional.SetRowCellValue(gridViewAdditional.FocusedRowHandle, gridViewAdditional.Columns["DebitTime"], DateTime.Now.ToString("hh:mm:tt"));
                            gridViewAdditional.SetRowCellValue(gridViewAdditional.FocusedRowHandle, gridViewAdditional.Columns[ItemName], dtItemID.Rows[0][PrimaryName].ToString());
                            gridViewAdditional.SetRowCellValue(gridViewAdditional.FocusedRowHandle, gridViewAdditional.Columns["DebitDate"], DateTime.Now.ToString("yyyy/MM/dd"));
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = "رقم الصنف غير موجود  ";
                        }
                    }
                }
                else if (ColName == SizeName)
                {
                    DataTable dtItemID = Lip.SelectRecord("Select SizeID from Stc_SizingUnits Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                    if (dtItemID.Rows.Count > 0)
                    {

                        gridViewAdditional.SetFocusedRowCellValue("SizeID", dtItemID.Rows[0]["SizeID"]);
                        e.Valid = true;
                        view.SetColumnError(gridViewAdditional.Columns[ColName], "");

                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(gridViewAdditional.Columns[ColName], Messages.msgNoFoundThisItem);
                    }
                }

                if (ColName == "MachineName")
                {

                    DataTable dtMachinID = Lip.SelectRecord("Select  MachineID  from Menu_FactoryMachine Where  LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') ");
                    if (dtMachinID.Rows.Count > 0)
                    {
                        gridViewAdditional.SetFocusedRowCellValue("MachinID", dtMachinID.Rows[0]["MachineID"].ToString());

                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = "المكينة غير موجوده  ";
                    }
                }

                else if (ColName == "EmpName")
                {
                    DataTable dtItemID = Lip.SelectRecord("Select EmployeeID as EmpID from HR_EmployeeFile Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') ");
                    if (dtItemID.Rows.Count > 0)
                    {
                        gridViewAdditional.SetFocusedRowCellValue("EmpID", dtItemID.Rows[0]["EmpID"]);
                        e.Valid = true;
                        view.SetColumnError(gridViewAdditional.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = "العامل غير موجود";
                        view.SetColumnError(gridViewAdditional.Columns[ColName], "العامل غير موجود");
                    }
                }
                if (ColName == ItemName)
                {
                    DataTable dtItemID = Lip.SelectRecord("Select  ItemID from Stc_Items  Where Cancel =0 and LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') ");
                    if (dtItemID.Rows.Count > 0)
                    {
                        gridViewAdditional.SetRowCellValue(gridViewAdditional.FocusedRowHandle, gridViewAdditional.Columns["DebitTime"], DateTime.Now.ToString("hh:mm:tt"));
                        gridViewAdditional.SetRowCellValue(gridViewAdditional.FocusedRowHandle, gridViewAdditional.Columns["ItemID"], dtItemID.Rows[0]["ItemID"].ToString());
                        gridViewAdditional.SetRowCellValue(gridViewAdditional.FocusedRowHandle, gridViewAdditional.Columns["DebitDate"], DateTime.Now.ToString("yyyy/MM/dd"));
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = " الصنف غير موجوده  ";
                    }
                }
                else if (ColName == "StoreName")
                {
                    DataTable dtItemID = Lip.SelectRecord("Select StoreID from Stc_Stores Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                    if (dtItemID.Rows.Count > 0)
                    {
                        gridViewAdditional.SetFocusedRowCellValue("StoreID", dtItemID.Rows[0]["StoreID"]);
                        e.Valid = true;
                        view.SetColumnError(gridViewAdditional.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(gridViewAdditional.Columns[ColName], Messages.msgNoFoundThisItem);
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
                        if (ColName == "MachinID")
                        {
                            if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                            {
                                HasColumnErrors = true;
                                view.SetColumnError(GridViewBeforPrentag.Columns[ColName], Messages.msgInputIsRequired);
                            }
                            else if (!(double.TryParse(cellValue.ToString(), out num)) && ColName != "MachinID")
                            {
                                HasColumnErrors = true;
                                view.SetColumnError(GridViewBeforPrentag.Columns[ColName], Messages.msgInputShouldBeNumber);
                            }
                            else if (Comon.ConvertToDecimalPrice(cellValue.ToString()) <= 0 && ColName != "MachinID")
                            {
                                HasColumnErrors = true;
                                view.SetColumnError(GridViewBeforPrentag.Columns[ColName], Messages.msgInputIsGreaterThanZero);
                            }
                            else
                            {
                                view.SetColumnError(GridViewBeforPrentag.Columns[ColName], "");
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
                        if (ColName == "MachinID") 
                        {
                            if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                            {
                                HasColumnErrors = true;
                                view.SetColumnError(GridViewBeforPolish.Columns[ColName], Messages.msgInputIsRequired);

                            }
                            else if (!(double.TryParse(cellValue.ToString(), out num)) && ColName != "MachinID")
                            {

                                HasColumnErrors = true;
                                view.SetColumnError(GridViewBeforPolish.Columns[ColName], Messages.msgInputShouldBeNumber);
                            }
                            else if (Comon.ConvertToDecimalPrice(cellValue.ToString()) <= 0 && ColName != "MachinID")
                            {

                                HasColumnErrors = true;
                                view.SetColumnError(GridViewBeforPolish.Columns[ColName], Messages.msgInputIsGreaterThanZero);
                            }
                            else
                            {
                                view.SetColumnError(GridViewBeforPolish.Columns[ColName], "");
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
        public void ReadRecord(int ComandID, bool flag = false)
        {
            try
            {
                ClearFields();
                cmbBranchesID.EditValue = MySession.GlobalBranchID;
                DataRecord = Menu_FactoryRunCommandMasterDAL.frmGetDataDetalByID(ComandID, UserInfo.BRANCHID, UserInfo.FacilityID,6);

                if (DataRecord != null && DataRecord.Rows.Count > 0)
                {
                    DataRecordCommpund = Menu_FactoryRunCommandCompundDAL.frmGetDataDetalByID(Comon.cLong(ComandID), UserInfo.BRANCHID, UserInfo.FacilityID,1);
                    DataRecordAfterCommpund = Menu_FactoryRunCommandCompundDAL.frmGetDataDetalByID(Comon.cLong(ComandID), UserInfo.BRANCHID, UserInfo.FacilityID,2);
                    DataRecordPolushin = Menu_FactoryRunCommandPrentagAndPulishnDAL.frmGetDataDetalByID(Comon.cLong(ComandID), UserInfo.BRANCHID, UserInfo.FacilityID,1);
                    DataRecordAfterBrntag = Menu_FactoryRunCommandPrentagAndPulishnDAL.frmGetDataDetalByID(Comon.cLong(ComandID), UserInfo.BRANCHID, UserInfo.FacilityID, 2);
                    DataRecordSelver = Menu_FactoryRunCommandSelverDAL.frmGetDataDetalByID(Comon.cLong(ComandID),  UserInfo.BRANCHID, UserInfo.FacilityID, 1);
                    DataRecordTalmee = Menu_FactoryRunCommandTalmeeDAL.frmGetDataDetalByID(Comon.cLong(ComandID), UserInfo.BRANCHID, UserInfo.FacilityID,1);
                    DataRecordAfterTalmee = Menu_FactoryRunCommandTalmeeDAL.frmGetDataDetalByID(Comon.cLong(ComandID), UserInfo.BRANCHID, UserInfo.FacilityID,2);
                    DataRecordFactory = Menu_FactoryRunCommandfactoryDAL.frmGetDataDetalByID(Comon.cLong(ComandID), UserInfo.BRANCHID, UserInfo.FacilityID,1);
                    DataRecordAfterFactory = Menu_FactoryRunCommandfactoryDAL.frmGetDataDetalByID(Comon.cLong(ComandID), UserInfo.BRANCHID, UserInfo.FacilityID, 2);
                    //DataRecordProductionExpenses = Menu_ProductionExpensesMasterDAL.frmGetDataDetalByID(Comon.cLong(ComandID), UserInfo.BRANCHID, UserInfo.FacilityID,1);

                    DataRecordCostDaimond = Menu_FactoryRunCommandCompundDAL.frmGetDataDetalByID(Comon.cLong(ComandID), UserInfo.BRANCHID, UserInfo.FacilityID, 2);
                    IsNewRecord = false;
                    txtBarCode.Text = DataRecord.Rows[0]["BarCode"].ToString();
                    txtReferanceID.Text = DataRecord.Rows[0]["DocumentID"].ToString();
                    txtReferanceID_Validating(null, null);
                    txtItemID.Text = DataRecord.Rows[0]["ItemID"].ToString();
                    txtItemID_Validating(null, null);
                    txtCostCenterID.Text = DataRecord.Rows[0]["CostCenterID"].ToString();
                    txtCostCenterID_Validating(null, null);
                    txtNotes.Text = DataRecord.Rows[0]["Notes"].ToString();
                    txtOpretionID.Text = DataRecord.Rows[0]["OpretionID"].ToString();

                    txtBrandID.Text = DataRecord.Rows[0]["BrandID"].ToString();
                    txtBrandID_Validating(null, null);

                    txtCustomerID.Text = DataRecord.Rows[0]["CustomerID"].ToString();
                    txtCustomerID_Validating(null, null);
                    txtDelegateID.Text = DataRecord.Rows[0]["DelegateID"].ToString();
                    txtDelegateID_Validating(null, null);
                    txtEmpIDFactor.Text = DataRecord.Rows[0]["EmpFactorID"].ToString();
                    txtEmpFactorID_Validating(null, null);

                    txtEmplooyID.Text = DataRecord.Rows[0]["EmpPrentagID"].ToString();
                    txtEmplooyBrntageID_Validating(null, null);

                    txtEmplooyIDPolishing.Text = DataRecord.Rows[0]["EmpPolishnID"].ToString();
                    txtEmplooyPolishingID_Validating(null, null);

                    txtEmployeeStokIDFactory.Text = DataRecord.Rows[0]["EmployeeStokID"].ToString();
                    txtEmployeeStokID_Validating(null, null);
                    
                    txtTypeID.Text = DataRecord.Rows[0]["TypeID"].ToString();
                    txtTypeID_Validating(null, null);

                    txtGroupID.Text = DataRecord.Rows[0]["GroupID"].ToString();
                    txtGroupID_Validating(null, null);
                    //الحسابات
                    txtAccountIDFactory.Text = DataRecord.Rows[0]["AccountIDFactory"].ToString();
                    txtAccountIDFactory_Validating(null, null);

                    txtStoreIDFactory.Text = DataRecord.Rows[0]["StoreIDFactory"].ToString();
                    txtStoreIDFactory_Validating(null, null);

                    txtEmployeeStokIDFactory.Text = DataRecord.Rows[0]["EmployeeStokIDFactory"].ToString();
                    txtEmployeeStokIDFactory_Validating(null, null);

                    txtEmpIDFactor.Text = DataRecord.Rows[0]["EmpIDFactor"].ToString();
                    txtEmpIDFactor_Validating(null, null);

                    txtAccountIDPrentage.Text = DataRecord.Rows[0]["AccountIDPrentage"].ToString();
                    txtAccountIDPrentage_Validating(null, null);

                    txtStoreIDPrentage.Text = DataRecord.Rows[0]["StoreIDPrentage"].ToString();
                    txtStoreIDPrentage_Validating(null, null);

                    txtEmployeeStokIDPrentage.Text = DataRecord.Rows[0]["EmployeeStokIDPrentage"].ToString();
                    txtEmployeeStokIDPrentage_Validating(null, null);

                    txtEmpIDPrentage.Text = DataRecord.Rows[0]["EmpIDPrentage"].ToString();
                    txtEmpIDPrentage_Validating(null, null);

                    txtAccountIDBeforCompond.Text = DataRecord.Rows[0]["AccountIDBeforCompond"].ToString();
                    txtAccountIDBeforCompond_Validating(null, null);

                    txtStoreIDBeforComond.Text = DataRecord.Rows[0]["StoreIDBeforComond"].ToString();
                    txtStoreIDBeforComond_Validating(null, null);

                    txtEmployeeStokIDBeforCompond.Text = DataRecord.Rows[0]["EmployeeStokIDBeforCompond"].ToString();
                    txtEmployeeStokIDBeforCompond_Validating(null, null);

                    txtEmpIDBeforCompond.Text = DataRecord.Rows[0]["EmpIDBeforCompond"].ToString();
                    txtEmpIDBeforCompond_Validating(null, null);

                    txtAccountIDAdditions.Text = DataRecord.Rows[0]["AccountIDAdditions"].ToString();
                    txtAccountIDAdditions_Validating(null, null);

                    txtStoreIDAdditions.Text = DataRecord.Rows[0]["StoreIDAdditions"].ToString();
                    txtStoreIDAdditions_Validating(null, null);

                    txtEmployeeStokIDAdditions.Text = DataRecord.Rows[0]["EmployeeStokIDAdditions"].ToString();
                    txtEmployeeStokIDAdditions_Validating(null, null);

                    txtEmpIDAdditions.Text = DataRecord.Rows[0]["EmpIDAdditions"].ToString();
                    txtEmpIDAdditions_Validating(null, null);

                    txtAccountIDPolishing.Text = DataRecord.Rows[0]["AccountIDPolishing"].ToString();
                    txtAccountIDPolishing_Validating(null, null);

                    txtStoreIDPolishing.Text = DataRecord.Rows[0]["StoreIDPolishing"].ToString();
                    txtStoreIDPolishing_Validating(null, null);

                    txtEmployeeStokIDPolishing.Text = DataRecord.Rows[0]["EmployeeStokIDPolishing"].ToString();
                    txtEmployeeStokIDPolishing_Validating(null, null);

                    txtEmplooyIDPolishing.Text = DataRecord.Rows[0]["EmplooyIDPolishing"].ToString();
                    txtEmplooyIDPolishing_Validating(null, null);

                    txtAccountIDBarcodeItem.Text = DataRecord.Rows[0]["AccountIDBarcodeItem"].ToString();
                    txtAccountIDBarcodeItem_Validating(null, null);

                    txtStoreIDBarcod.Text = DataRecord.Rows[0]["StoreIDBarcod"].ToString();
                    txtStoreIDProducts_Validating(null, null);

                    txtEmployeeStokIDBarcode.Text = DataRecord.Rows[0]["EmployeeStokIDBarcode"].ToString();
                    txtEmployeeStokIDBarcode_Validating(null, null);

                    txtThefactoriID.Text = DataRecord.Rows[0]["ThefactoriID"].ToString();
                    //txtTimeRecive.Text = DataRecord.Rows[0]["TimeRecive"].ToString();
                    txtGivenTime.Text = DataRecord.Rows[0]["GivenTime"].ToString();
                    //txtReciveDate.EditValue = Comon.ConvertSerialToDate(DataRecord.Rows[0]["ReciveDate"].ToString());
                    txtGivenDate.EditValue = Comon.ConvertSerialToDate(DataRecord.Rows[0]["GivenDate"].ToString());
                    txtCommandDate.EditValue = Comon.ConvertSerialToDate(DataRecord.Rows[0]["ComandDate"].ToString());
                    txtSpendAmount.Text = DataRecord.Rows[0]["SpendAmount"].ToString();
                    if (Comon.cInt(DataRecord.Rows[0]["piece"].ToString()) == 1)
                        radioButton1.Checked = true;
                    else if (Comon.cInt(DataRecord.Rows[0]["piece"].ToString()) == 2)
                        radioButton1.Checked = true;
                    else if (Comon.cInt(DataRecord.Rows[0]["piece"].ToString()) == 3)
                        radioButton1.Checked = true;

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
                    if (DataRecordPolushin != null)
                        if (DataRecordPolushin.Rows.Count > 0)
                        {
                            gridControlBeforPrentag.DataSource = DataRecordPolushin;
                            lstDetailPrentage.AllowNew = true;
                            lstDetailPrentage.AllowEdit = true;
                            lstDetailPrentage.AllowRemove = true;
                            GridViewBeforPrentag.RefreshData();
                        }
                    if (DataRecordAfterBrntag != null)
                        if (DataRecordAfterBrntag.Rows.Count > 0)
                        {
                            gridControlAfterPrentage.DataSource = DataRecordAfterBrntag;

                            lstDetailAfterPrentage.AllowNew = true;
                            lstDetailAfterPrentage.AllowEdit = true;
                            lstDetailAfterPrentage.AllowRemove = true;
                            GridViewAfterPrentag.RefreshData();
                        }
                    if (DataRecordSelver != null)
                        if (DataRecordSelver.Rows.Count > 0)
                        {
                            gridControlAdditional.DataSource = DataRecordSelver;
                            lstDetailSelver.AllowNew = true;
                            lstDetailSelver.AllowEdit = true;
                            lstDetailSelver.AllowRemove = true;
                            gridViewAdditional.RefreshData();
                        }
                    if (DataRecordTalmee != null)
                        if (DataRecordTalmee.Rows.Count > 0)
                        {
                            gridControlBeforePolishing.DataSource = DataRecordTalmee;

                            lstDetailTalmee.AllowNew = true;
                            lstDetailTalmee.AllowEdit = true;
                            lstDetailTalmee.AllowRemove = true;
                            GridViewBeforPolish.RefreshData();
                        }
                    if (DataRecordAfterTalmee != null)
                        if (DataRecordAfterTalmee.Rows.Count > 0)
                        {
                            gridControlAfterPolishing.DataSource = DataRecordAfterTalmee;

                            lstDetailAfterTalmee.AllowNew = true;
                            lstDetailAfterTalmee.AllowEdit = true;
                            lstDetailAfterTalmee.AllowRemove = true;
                            GridViewAfterPolish.RefreshData();
                        }
                    if (DataRecordFactory != null)
                        if (DataRecordFactory.Rows.Count > 0)
                        {
                            gridControlfactroOpretion.DataSource = DataRecordFactory;
                            lstDetailfactory.AllowNew = true;
                            lstDetailfactory.AllowEdit = true;
                            lstDetailfactory.AllowRemove = true;
                            gridViewBeforCompond.RefreshData();
                        }
                    if (DataRecordAfterFactory != null)
                        if (DataRecordAfterFactory.Rows.Count > 0)
                        {
                            gridControlAfterFactory.DataSource = DataRecordAfterFactory;
                            lstDetailAfterfactory.AllowNew = true;
                            lstDetailAfterfactory.AllowEdit = true;
                            lstDetailAfterfactory.AllowRemove = true;
                            gridViewBeforCompond.RefreshData();
                        }

                    if (DataRecordProductionExpenses != null)
                        if (DataRecordProductionExpenses.Rows.Count > 0)
                        {
                            gridControlProductionExpenses.DataSource = DataRecordProductionExpenses;
                            lstDetailProductionExpenses.AllowNew = true;
                            lstDetailProductionExpenses.AllowEdit = true;
                            lstDetailProductionExpenses.AllowRemove = true;
                            GridProductionExpenses.RefreshData();
                        }
                    if (DataRecordCostDaimond != null)
                        if (DataRecordCostDaimond.Rows.Count > 0)
                        {
                            gridControlCostDaimond.DataSource = DataRecordCostDaimond;
                            lstDetailCostDaimond.AllowNew = true;
                            lstDetailCostDaimond.AllowEdit = true;
                            lstDetailCostDaimond.AllowRemove = true;
                            GridCostDaimond.RefreshData();
                        }
                    Validations.DoReadRipon(this, ribbonControl1);
                    CalculateFactoryLost();
                    CalculatePrentageLost();
                    CalculatePolishnLost();
                    CalculateAdditionalQTY();
                    gridView2_RowUpdated(null, null);
                    GridProductionExpenses_RowUpdated(null, null);
                    if (DataRecordProductionExpenses != null)
                   if (DataRecordProductionExpenses.Rows.Count > 0)
                    {
                    CreateCoding();
                    }
                
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
        void initGridBeforPrentage()
        {
            lstDetailPrentage = new BindingList<Menu_FactoryRunCommandPrentagAndPulishn>();
            lstDetailPrentage.AllowNew = true;
            lstDetailPrentage.AllowEdit = true;
            lstDetailPrentage.AllowRemove = true;
            gridControlBeforPrentag.DataSource = lstDetailPrentage;

            DataTable dtitems = Lip.SelectRecord("SELECT   "+PrimaryName+"   FROM Menu_FactoryMachine ");
            string[] NameMachine = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameMachine[i] = dtitems.Rows[i]["ArbName"].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameMachine);

            gridControlBeforPrentag.RepositoryItems.Add(riComboBoxitems);
            GridViewBeforPrentag.Columns["MachineName"].ColumnEdit = riComboBoxitems;

            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0 ");
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();
            riComboBoxitems2.Items.AddRange(StoreName);
            gridControlBeforPrentag.RepositoryItems.Add(riComboBoxitems2);
            GridViewBeforPrentag.Columns["StoreName"].ColumnEdit = riComboBoxitems2;

            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0 ");
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlBeforPrentag.RepositoryItems.Add(riComboBoxitems3);
            GridViewBeforPrentag.Columns["EmpName"].ColumnEdit = riComboBoxitems3;

            DataTable dtItemname = Lip.SelectRecord("Select "+PrimaryName +" from Stc_Items  Where Cancel=0" );
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4= new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlBeforPrentag.RepositoryItems.Add(riComboBoxitems4);
            GridViewBeforPrentag.Columns[ItemName].ColumnEdit = riComboBoxitems4;



            
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
                GridViewBeforPrentag.Columns["PrentagCredit"].Caption = "Creditor";
                GridViewBeforPrentag.Columns["TypeOpration"].Caption = "Type Opration";
                GridViewBeforPrentag.Columns["PrSignature"].Caption = "Signature";
                GridViewBeforPrentag.Columns["PrentagDebitDate"].Caption = "Date";
                GridViewBeforPrentag.Columns["PrentagDebitTime"].Caption = "Time";
                GridViewBeforPrentag.Columns["EmpID"].Caption = "EmpID";
                GridViewBeforPrentag.Columns["EmpName"].Caption = "Name";
            }
            GridViewBeforPrentag.Columns["MachinID"].OptionsColumn.AllowFocus = false;
            GridViewBeforPrentag.Columns["MachinID"].OptionsColumn.AllowEdit = false;

            GridViewBeforPrentag.Columns["MachineName"].OptionsColumn.AllowFocus = false;
            GridViewBeforPrentag.Columns["MachineName"].OptionsColumn.AllowEdit = false;


           
        }
        void initGridAfterPrentage()
        {

            lstDetailAfterPrentage = new BindingList<Menu_FactoryRunCommandPrentagAndPulishn>();
            lstDetailAfterPrentage.AllowNew = true;
            lstDetailAfterPrentage.AllowEdit = true;
            lstDetailAfterPrentage.AllowRemove = true;
            gridControlAfterPrentage.DataSource = lstDetailAfterPrentage;

            DataTable dtitems = Lip.SelectRecord("SELECT   ArbName   FROM Menu_FactoryMachine ");
            string[] NameMachine = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameMachine[i] = dtitems.Rows[i]["ArbName"].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameMachine);

            gridControlAfterPrentage.RepositoryItems.Add(riComboBoxitems);


            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0 ");
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();

            riComboBoxitems2.Items.AddRange(StoreName);
            gridControlAfterPrentage.RepositoryItems.Add(riComboBoxitems2);
            GridViewAfterPrentag.Columns["StoreName"].ColumnEdit = riComboBoxitems2;

            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0 ");
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlAfterPrentage.RepositoryItems.Add(riComboBoxitems3);
            GridViewAfterPrentag.Columns["EmpName"].ColumnEdit = riComboBoxitems3;

            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0");
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
                GridViewAfterPrentag.Columns["PrentagDebit"].Caption = "الوزن";

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
                GridViewAfterPrentag.Columns["PrentagCredit"].Caption = "Creditor";
                GridViewAfterPrentag.Columns["TypeOpration"].Caption = "Type Opration";
                GridViewAfterPrentag.Columns["PrSignature"].Caption = "Signature";
                GridViewAfterPrentag.Columns["PrentagDebitDate"].Caption = "Date";
                GridViewAfterPrentag.Columns["PrentagDebitTime"].Caption = "Time";
                GridViewAfterPrentag.Columns["EmpID"].Caption = "EmpID";
                GridViewAfterPrentag.Columns["EmpName"].Caption = "Name";
            }



        }
        void initGridBeforTalmee()
        {

            lstDetailTalmee = new BindingList<Menu_FactoryRunCommandTalmee>();
            lstDetailTalmee.AllowNew = true;
            lstDetailTalmee.AllowEdit = true;
            lstDetailTalmee.AllowRemove = true;
            gridControlBeforePolishing.DataSource = lstDetailTalmee;

            DataTable dtitems = Lip.SelectRecord("SELECT   ArbName   FROM Menu_FactoryMachine ");
            string[] NameMachine = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameMachine[i] = dtitems.Rows[i]["ArbName"].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameMachine);

            gridControlBeforePolishing.RepositoryItems.Add(riComboBoxitems);


            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0 ");
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();

            riComboBoxitems2.Items.AddRange(StoreName);
            gridControlBeforePolishing.RepositoryItems.Add(riComboBoxitems2);
            GridViewBeforPolish.Columns["StoreName"].ColumnEdit = riComboBoxitems2;


            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0 ");
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlBeforePolishing.RepositoryItems.Add(riComboBoxitems3);
            GridViewBeforPolish.Columns["EmpName"].ColumnEdit = riComboBoxitems3;

            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0");
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
            // GridViewBeforPolish.Columns["PrentagDebitTime"].Visible = false;
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
                GridViewBeforPolish.Columns["PrSignature"].Caption = "Signature";
                GridViewBeforPolish.Columns["DebitDate"].Caption = "Date";
                GridViewBeforPolish.Columns["DebitTime"].Caption = "Time";
                GridViewBeforPolish.Columns["EmpID"].Caption = "EmpID";
                GridViewBeforfactory.Columns["EmpName"].Caption = "Name";
            }



        }
        void initGridAfterTalmee()
        {

            lstDetailAfterTalmee = new BindingList<Menu_FactoryRunCommandTalmee>();
            lstDetailAfterTalmee.AllowNew = true;
            lstDetailAfterTalmee.AllowEdit = true;
            lstDetailAfterTalmee.AllowRemove = true;
            gridControlAfterPolishing.DataSource = lstDetailAfterTalmee;

            DataTable dtitems = Lip.SelectRecord("SELECT   ArbName   FROM Menu_FactoryMachine ");
            string[] NameMachine = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameMachine[i] = dtitems.Rows[i]["ArbName"].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameMachine);
            gridControlAfterPolishing.RepositoryItems.Add(riComboBoxitems);



            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0 ");
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();
            riComboBoxitems2.Items.AddRange(StoreName);
            gridControlAfterPolishing.RepositoryItems.Add(riComboBoxitems2);
            GridViewAfterPolish.Columns["StoreName"].ColumnEdit = riComboBoxitems2;


            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0 ");
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlAfterPolishing.RepositoryItems.Add(riComboBoxitems3);
            GridViewAfterPolish.Columns["EmpName"].ColumnEdit = riComboBoxitems3;

            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0");
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

            // GridViewAfterPolish.Columns["PrentagDebitTime"].Visible = false;
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
            }



        }
        void initGridSelver()
        {

            lstDetailSelver = new BindingList<Menu_FactoryRunCommandSelver>();
            lstDetailSelver.AllowNew = true;
            lstDetailSelver.AllowEdit = true;
            lstDetailSelver.AllowRemove = true;
            gridControlAdditional.DataSource = lstDetailSelver;

            DataTable dtitems = Lip.SelectRecord("SELECT   ArbName   FROM Menu_FactoryMachine ");
            string[] NameMachine = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameMachine[i] = dtitems.Rows[i]["ArbName"].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameMachine);

            gridControlAdditional.RepositoryItems.Add(riComboBoxitems);

            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0 ");
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();
            riComboBoxitems2.Items.AddRange(StoreName);
            gridControlAdditional.RepositoryItems.Add(riComboBoxitems2);
            gridViewAdditional.Columns["StoreName"].ColumnEdit = riComboBoxitems2;

            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0 ");
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlAdditional.RepositoryItems.Add(riComboBoxitems3);
            gridViewAdditional.Columns["EmpName"].ColumnEdit = riComboBoxitems3;


            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0");
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlAdditional.RepositoryItems.Add(riComboBoxitems4);
            gridViewAdditional.Columns[ItemName].ColumnEdit = riComboBoxitems4;


            gridViewAdditional.Columns["MachineName"].ColumnEdit = riComboBoxitems;
            gridViewAdditional.Columns["ID"].Visible = false;
            gridViewAdditional.Columns["ComandID"].Visible = false;
            gridViewAdditional.Columns["BarcodeAdditional"].Visible = false;
       
           
            gridViewAdditional.Columns["Cancel"].Visible = false;
            gridViewAdditional.Columns["BranchID"].Visible = false;
            gridViewAdditional.Columns["FacilityID"].Visible = false;

            gridViewAdditional.Columns["EditUserID"].Visible = false;
            gridViewAdditional.Columns["EditDate"].Visible = false;
            gridViewAdditional.Columns["EditTime"].Visible = false;
            gridViewAdditional.Columns["RegDate"].Visible = false;
            gridViewAdditional.Columns["UserID"].Visible = false;

            gridViewAdditional.Columns["ComputerInfo"].Visible = false;
            gridViewAdditional.Columns["EditComputerInfo"].Visible = false;
            gridViewAdditional.Columns["RegTime"].Visible = false;

            gridViewAdditional.Columns["Credit"].Visible = false;
            gridViewAdditional.Columns["Lost"].Visible = false;
            //gridView4.Columns["SizeID"].Visible = false;
            gridViewAdditional.Columns["CostPrice"].Visible = false;

            // gridView4.Columns["PrentagDebitTime"].Visible = false;
            gridViewAdditional.Columns["MachinID"].Name = "MachinID";
            //dtItem.Columns["Losed"].ReadOnly = true;
            gridViewAdditional.Columns["EmpName"].Width = 140;
            gridViewAdditional.Columns["SizeID"].Visible = false;
            gridViewAdditional.Columns["StoreName"].Width = 120;
            gridViewAdditional.Columns["EmpID"].Width = 100;
            gridViewAdditional.Columns["Signature"].Width = 120;
            gridViewAdditional.Columns["DebitDate"].Width = 110;
            gridViewAdditional.Columns["DebitTime"].Width = 85;


            if (UserInfo.Language == iLanguage.Arabic)
            {

                gridViewAdditional.Columns["EngItemName"].Visible = false;
                gridViewAdditional.Columns["EngSizeName"].Visible = false;
                gridViewAdditional.Columns["ArbItemName"].Width = 150;


                gridViewAdditional.Columns["StoreID"].Caption = "رقم المخزن";
                gridViewAdditional.Columns["StoreName"].Caption = "إسم المخزن";

                gridViewAdditional.Columns["EmpID"].Caption = "رقم العامل";
                gridViewAdditional.Columns["EmpName"].Caption = "إسم العامل";

                gridViewAdditional.Columns["MachinID"].Caption = "رقم المكينة";
                gridViewAdditional.Columns["MachineName"].Caption = "إسم المكينة";
                gridViewAdditional.Columns["Debit"].Caption = "الوزن";

                gridViewAdditional.Columns["Credit"].Caption = "دائــن";
                gridViewAdditional.Columns["Lost"].Caption = "الفاقــد";
                gridViewAdditional.Columns["Signature"].Caption = "التوقيع";

                gridViewAdditional.Columns["ItemID"].Caption = "رقم الصنف";
                gridViewAdditional.Columns["ArbItemName"].Caption = "اسم الصنف";
                gridViewAdditional.Columns["SizeID"].Caption = "رقم الوحده";
                gridViewAdditional.Columns["ArbSizeName"].Caption = "الوحده";
                gridViewAdditional.Columns["CostPrice"].Caption = "التكلفة";
                gridViewAdditional.Columns["DebitDate"].Caption = "التاريخ";
                gridViewAdditional.Columns["DebitTime"].Caption = "الوقت";
            }
            else
            {
                gridViewAdditional.Columns["ArbItemName"].Visible = false;
                gridViewAdditional.Columns["ArbSizeName"].Visible = false;
                gridViewAdditional.Columns["EngItemName"].Width = 150;

                gridViewAdditional.Columns["StoreID"].Caption = "Store ID";
                gridViewAdditional.Columns["StoreName"].Caption = "Store Name";
                gridViewAdditional.Columns["EngItemName"].Caption = "Item Name";
                gridViewAdditional.Columns["MachinID"].Caption = "Machine ID";
                gridViewAdditional.Columns["MachineName"].Caption = "Machin Name";
                gridViewAdditional.Columns["Debit"].Caption = "debtor ";
                gridViewAdditional.Columns["EngSizeName"].Caption = "Unit";
                gridViewAdditional.Columns["Credit"].Caption = "Creditor";
                gridViewAdditional.Columns["Lost"].Caption = "Lost";
                gridViewAdditional.Columns["Signature"].Caption = "Signature";
                gridViewAdditional.Columns["DebitDate"].Caption = "Date";
                gridViewAdditional.Columns["DebitTime"].Caption = "Time";
                gridViewAdditional.Columns["EmpID"].Caption = "EmpID";
                gridViewAdditional.Columns["EmpName"].Caption = "Name";
            }



        }
        void initGridFactory()
        {

            lstDetailfactory = new BindingList<Menu_FactoryRunCommandfactory>();
            lstDetailfactory.AllowNew = true;
            lstDetailfactory.AllowEdit = true;
            lstDetailfactory.AllowRemove = true;
            gridControlfactroOpretion.DataSource = lstDetailfactory;

            DataTable dtitems = Lip.SelectRecord("SELECT   ArbName   FROM Menu_FactoryMachine ");
            string[] NameMachine = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameMachine[i] = dtitems.Rows[i]["ArbName"].ToString();
            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameMachine);
            gridControlfactroOpretion.RepositoryItems.Add(riComboBoxitems);

            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0 ");
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();
            riComboBoxitems2.Items.AddRange(StoreName);
            gridControlfactroOpretion.RepositoryItems.Add(riComboBoxitems2);
            GridViewBeforfactory.Columns["StoreName"].ColumnEdit = riComboBoxitems2;


            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0 ");
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlfactroOpretion.RepositoryItems.Add(riComboBoxitems3);
            GridViewBeforfactory.Columns["EmpName"].ColumnEdit = riComboBoxitems3;

            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0");
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
          

            GridViewBeforfactory.Columns["MachineName"].ColumnEdit = riComboBoxitems;
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

            // GridViewBeforfactory.Columns["PrentagDebitTime"].Visible = false;
            GridViewBeforfactory.Columns["MachinID"].Name = "MachinID";
            //dtItem.Columns["Losed"].ReadOnly = true;
            GridViewBeforfactory.Columns["EmpName"].Width = 120;

            GridViewBeforfactory.Columns["StoreName"].Width = 120;
            GridViewBeforfactory.Columns["EmpID"].Width = 120;
            GridViewBeforfactory.Columns["Signature"].Width = 120;
            GridViewBeforfactory.Columns["DebitDate"].Width = 110;
            GridViewBeforfactory.Columns["DebitTime"].Width = 85;

            GridViewBeforfactory.Columns["EmpID"].Visible = false;
            GridViewBeforfactory.Columns["EmpName"].Visible = false;
            GridViewBeforfactory.Columns["StoreID"].Visible = false;
            GridViewBeforfactory.Columns["StoreName"].Visible = false; 
          
            if (UserInfo.Language == iLanguage.Arabic)
            {
                GridViewBeforfactory.Columns["EngItemName"].Visible = false;
                GridViewBeforfactory.Columns["EngSizeName"].Visible = false;
                GridViewBeforfactory.Columns["ArbItemName"].Width = 150;
                GridViewBeforfactory.Columns["StoreID"].Caption = "رقم المخزن";
                GridViewBeforfactory.Columns["StoreName"].Caption = "إسم المخزن";

                GridViewBeforfactory.Columns["EmpID"].Caption = "رقم العامل";
                GridViewBeforfactory.Columns["EmpName"].Caption = "إسم العامل";

                GridViewBeforfactory.Columns["MachinID"].Caption = "رقم المكينة";
                GridViewBeforfactory.Columns["MachineName"].Caption = "إسم المكينة";
                GridViewBeforfactory.Columns["Debit"].Caption = "الوزن";

                GridViewBeforfactory.Columns["Credit"].Caption = "دائــن";
                GridViewBeforfactory.Columns["TypeOpration"].Caption = "نوع العملية";
                GridViewBeforfactory.Columns["Signature"].Caption = "التوقيع";

                GridViewBeforfactory.Columns["ItemID"].Caption = "رقم الصنف";
                GridViewBeforfactory.Columns["ArbItemName"].Caption = "اسم الصنف";
                GridViewBeforfactory.Columns["SizeID"].Caption = "رقم الوحده";
                GridViewBeforfactory.Columns["ArbSizeName"].Caption = "الوحده";
                GridViewBeforfactory.Columns["CostPrice"].Caption = "التكلفة";
                GridViewBeforfactory.Columns["DebitDate"].Caption = "التاريخ";
                GridViewBeforfactory.Columns["DebitTime"].Caption = "الوقت";
            }
            else
            {
                GridViewBeforPrentag.Columns["ArbItemName"].Visible = false;
                GridViewBeforPrentag.Columns["ArbSizeName"].Visible = false;

                GridViewBeforfactory.Columns["StoreID"].Caption = "Store ID";
                GridViewBeforfactory.Columns["StoreName"].Caption = "Store Name";
                GridViewBeforPrentag.Columns["EngItemName"].Width = 150;
                GridViewBeforPrentag.Columns["EngItemName"].Caption = "Item Name";
                GridViewBeforPrentag.Columns["MachinID"].Caption = "Machine ID";
                GridViewBeforPrentag.Columns["MachineName"].Caption = "Machin Name";
                GridViewBeforPrentag.Columns["Debit"].Caption = "debtor ";
                GridViewBeforPrentag.Columns["EngSizeName"].Caption = "Unit";
                GridViewBeforPrentag.Columns["PrentagCredit"].Caption = "Creditor";
                GridViewBeforPrentag.Columns["TypeOpration"].Caption = "Type Opration";
                GridViewBeforPrentag.Columns["Signature"].Caption = "Signature";
                GridViewBeforPrentag.Columns["DebitDate"].Caption = "Date";
                GridViewBeforPrentag.Columns["DebitTime"].Caption = "Time";
                GridViewBeforPrentag.Columns["EmpID"].Caption = "EmpID";
                GridViewBeforPrentag.Columns["EmpName"].Caption = "Name";
            }
        }
        void initGridAfterFactory()
        {

            lstDetailAfterfactory = new BindingList<Menu_FactoryRunCommandfactory>();
            lstDetailAfterfactory.AllowNew = true;
            lstDetailAfterfactory.AllowEdit = true;
            lstDetailAfterfactory.AllowRemove = true;
            gridControlAfterFactory.DataSource = lstDetailAfterfactory;

            DataTable dtitems = Lip.SelectRecord("SELECT   ArbName   FROM Menu_FactoryMachine ");
            string[] NameMachine = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameMachine[i] = dtitems.Rows[i]["ArbName"].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameMachine);

            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0 ");
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();
            riComboBoxitems2.Items.AddRange(StoreName);
            gridControlAfterFactory.RepositoryItems.Add(riComboBoxitems2);
            GridViewAfterfactory.Columns["StoreName"].ColumnEdit = riComboBoxitems2;

            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0 ");
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlAfterFactory.RepositoryItems.Add(riComboBoxitems3);
            GridViewAfterfactory.Columns["EmpName"].ColumnEdit = riComboBoxitems3;


            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0");
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlAfterFactory.RepositoryItems.Add(riComboBoxitems4);
            GridViewAfterfactory.Columns[ItemName].ColumnEdit = riComboBoxitems4;

            gridControlAfterFactory.RepositoryItems.Add(riComboBoxitems);
            GridViewAfterfactory.Columns["MachineName"].ColumnEdit = riComboBoxitems;
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

            // GridViewAfterfactory.Columns["PrentagDebitTime"].Visible = false;
            GridViewAfterfactory.Columns["MachinID"].Name = "MachinID";
            //dtItem.Columns["Losed"].ReadOnly = true;
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
            if (UserInfo.Language == iLanguage.Arabic)
            {
                GridViewAfterfactory.Columns["EngItemName"].Visible = false;
                GridViewAfterfactory.Columns["EngSizeName"].Visible = false;
                GridViewAfterfactory.Columns["ArbItemName"].Width = 150;
                GridViewAfterfactory.Columns["StoreID"].Caption = "رقم المخزن";
                GridViewAfterfactory.Columns["StoreName"].Caption = "إسم المخزن";
                GridViewAfterfactory.Columns["EmpID"].Caption = "رقم العامل";
                GridViewAfterfactory.Columns["EmpName"].Caption = "إسم العامل";
                GridViewAfterfactory.Columns["MachinID"].Caption = "رقم المكينة";
                GridViewAfterfactory.Columns["MachineName"].Caption = "إسم المكينة";
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
            }
            else
            {
                GridViewAfterfactory.Columns["ArbItemName"].Visible = false;
                GridViewAfterfactory.Columns["ArbSizeName"].Visible = false;
                GridViewAfterfactory.Columns["EngItemName"].Width = 150;
                GridViewAfterfactory.Columns["StoreID"].Caption = "Store ID";
                GridViewAfterfactory.Columns["StoreName"].Caption = "Store Name";
                GridViewAfterfactory.Columns["EngItemName"].Caption = "Item Name";
                GridViewAfterfactory.Columns["MachinID"].Caption = "Machine ID";
                GridViewAfterfactory.Columns["MachineName"].Caption = "Machin Name";
                GridViewAfterfactory.Columns["Debit"].Caption = "debtor ";
                GridViewAfterfactory.Columns["EngSizeName"].Caption = "Unit";
                GridViewAfterfactory.Columns["Credit"].Caption = "QTY";
                GridViewAfterfactory.Columns["TypeOpration"].Caption = "Type Opration";
                GridViewAfterfactory.Columns["Signature"].Caption = "Signature";
                GridViewAfterfactory.Columns["DebitDate"].Caption = "Date";
                GridViewAfterfactory.Columns["DebitTime"].Caption = "Time";
                GridViewAfterfactory.Columns["EmpID"].Caption = "EmpID";
                GridViewAfterfactory.Columns["EmpName"].Caption = "Name";
            }
        }
        void initGridBeforCompent()
        {
            lstDetailCompund = new BindingList<Menu_FactoryRunCommandCompund>();
            lstDetailCompund.AllowNew = true;
            lstDetailCompund.AllowEdit = true;
            lstDetailCompund.AllowRemove = true;

            gridControlBeforCompond.DataSource = lstDetailCompund;
            gridViewBeforCompond.Columns["ID"].Visible = false;
            gridViewBeforCompond.Columns["ComandID"].Visible = false;

            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0 ");
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

            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0");
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlBeforCompond.RepositoryItems.Add(riComboBoxitems4);
            gridViewBeforCompond.Columns[ItemName].ColumnEdit = riComboBoxitems4;


            gridViewBeforCompond.Columns["Cancel"].Visible = false;
            gridViewBeforCompond.Columns["BranchID"].Visible = false;
            gridViewBeforCompond.Columns["FacilityID"].Visible = false;          
            gridViewBeforCompond.Columns["RegTime"].Visible = false;
            gridViewBeforCompond.Columns["RegDate"].Visible = false;
            gridViewBeforCompond.Columns["InvoiceImage"].Visible = false;
            gridViewBeforCompond.Columns["EditUserID"].Visible = false;
            gridViewBeforCompond.Columns["EditDate"].Visible = false;
            gridViewBeforCompond.Columns["EditTime"].Visible = false;       
            gridViewBeforCompond.Columns["UserID"].Visible = false;
            gridViewBeforCompond.Columns["SizeID"].Visible = false;
            gridViewBeforCompond.Columns["ComputerInfo"].Visible = false;
            gridViewBeforCompond.Columns["EditComputerInfo"].Visible = false;
            gridViewBeforCompond.Columns["GoldCompundNet"].Visible = false;
            gridViewBeforCompond.Columns["TypeOpration"].Visible=false;
            gridViewBeforCompond.Columns["TypeID"].Visible = false;
            gridViewBeforCompond.Columns["ComWeightStonAfter"].Visible = false;
            gridViewBeforCompond.Columns["FromAccountID"].Name = "FromAccountID";
            gridViewBeforCompond.Columns["BarcodCompond"].Name = "BarcodCompond";
            gridViewBeforCompond.Columns["EmpCompondID"].Name = "EmpCompondID";
            gridViewBeforCompond.Columns["EmpCompundName"].Width = 120;
            gridViewBeforCompond.Columns["FromAccountName"].Width = 120;
            gridViewBeforCompond.Columns["FromAccountID"].Width = 120;
            gridViewBeforCompond.Columns["EmpCompondID"].Width = 120;
            gridViewBeforCompond.Columns["ComSignature"].Width = 45;
            gridViewBeforCompond.Columns["ComStoneCom"].OptionsColumn.AllowFocus = false;
            gridViewBeforCompond.Columns["ComStoneCom"].OptionsColumn.AllowEdit = false;
            gridViewBeforCompond.Columns["ComWeightSton"].OptionsColumn.AllowFocus = false;
            gridViewBeforCompond.Columns["ComWeightSton"].OptionsColumn.AllowEdit = false;
            gridViewBeforCompond.Columns["ComStoneCom"].Visible = false;
            gridViewBeforCompond.Columns["ComWeightSton"].Visible = false;
            gridViewBeforCompond.Columns["GoldCredit"].Visible = false;
            gridViewBeforCompond.Columns["TypeSton"].Visible = false;
            gridViewBeforCompond.Columns["ComStoneNumout"].Visible = false;
            gridViewBeforCompond.Columns["ComWeightStonOUt"].Visible = false;
            gridViewBeforCompond.Columns["ComStoneNumlas"].Visible = false;
            gridViewBeforCompond.Columns["ComWeightStonLas"].Visible = false;
            gridViewBeforCompond.Columns["FromAccountName"].Visible = false;
            gridViewBeforCompond.Columns["EmpCompundName"].Visible = false;
            gridViewBeforCompond.Columns["FromAccountID"].Visible = false;
            gridViewBeforCompond.Columns["EmpCompondID"].Visible = false;
            gridViewBeforCompond.Columns["ComSignature"].VisibleIndex = gridViewBeforCompond.Columns["DebitTime"].VisibleIndex + 1;
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
                gridViewBeforCompond.Columns["GoldDebit"].Caption = "الوزن";
                gridViewBeforCompond.Columns["GoldCredit"].Caption = "مستلم";
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
            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0 ");
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


            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0");
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlAfterCompond.RepositoryItems.Add(riComboBoxitems4);
            gridViewAfterCompond.Columns[ItemName].ColumnEdit = riComboBoxitems4;


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
            //gridViewAfterCompond.Columns["ComStoneCom"].OptionsColumn.AllowFocus = false;
            //gridViewAfterCompond.Columns["ComStoneCom"].OptionsColumn.AllowEdit = false;
            //gridViewAfterCompond.Columns["ComWeightSton"].OptionsColumn.AllowFocus = false;
            //gridViewAfterCompond.Columns["ComWeightSton"].OptionsColumn.AllowEdit = false;
            //gridViewAfterCompond.Columns["ComWeightStonAfter"].OptionsColumn.AllowEdit = false;
            //gridViewAfterCompond.Columns["ComWeightStonAfter"].OptionsColumn.AllowFocus = false;
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
            gridViewAfterCompond.Columns["ComSignature"].VisibleIndex = gridViewAfterCompond.Columns["DebitTime"].VisibleIndex + 1;
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
            }

        }

        void initGridCostDaimond()
        {
            lstDetailCostDaimond = new BindingList<Menu_FactoryRunCommandCompund>();
            lstDetailCostDaimond.AllowNew = true;
            lstDetailCostDaimond.AllowEdit = true;
            lstDetailCostDaimond.AllowRemove = true;
            gridControlCostDaimond.DataSource = lstDetailCostDaimond;
            GridCostDaimond.Columns["ID"].Visible = false;
            GridCostDaimond.Columns["ComandID"].Visible = false;
            GridCostDaimond.Columns["TypeID"].Visible = false;
            GridCostDaimond.Columns["Cancel"].Visible = false;
            GridCostDaimond.Columns["BranchID"].Visible = false;
            GridCostDaimond.Columns["FacilityID"].Visible = false;
            GridCostDaimond.Columns["SizeID"].Visible = false;
            GridCostDaimond.Columns["RegTime"].Visible = false;
            GridCostDaimond.Columns["RegDate"].Visible = false;
            GridCostDaimond.Columns["InvoiceImage"].Visible = false;
            GridCostDaimond.Columns["EditUserID"].Visible = false;
            GridCostDaimond.Columns["EditDate"].Visible = false;
            GridCostDaimond.Columns["EditTime"].Visible = false;
            GridCostDaimond.Columns["UserID"].Visible = false;
            GridCostDaimond.Columns["TypeOpration"].Visible = false;
            GridCostDaimond.Columns["ComputerInfo"].Visible = false;
            GridCostDaimond.Columns["EditComputerInfo"].Visible = false;
            GridCostDaimond.Columns["GoldCompundNet"].Visible = false;
            GridCostDaimond.Columns["ComStoneCom"].OptionsColumn.AllowFocus = false;
            GridCostDaimond.Columns["ComStoneCom"].OptionsColumn.AllowEdit = false;
            GridCostDaimond.Columns["ComWeightSton"].OptionsColumn.AllowFocus = false;
            GridCostDaimond.Columns["ComWeightSton"].OptionsColumn.AllowEdit = false;
            GridCostDaimond.Columns["ComWeightStonAfter"].Visible = false;
            GridCostDaimond.Columns["EmpCompundName"].Width = 120;
            GridCostDaimond.Columns["FromAccountName"].Width = 120;
            GridCostDaimond.Columns["ComSignature"].Width = 45;
            GridCostDaimond.Columns["GoldDebit"].Visible = false;
            GridCostDaimond.Columns["ComStoneNumout"].Visible = false;
            GridCostDaimond.Columns["ComWeightStonOUt"].Visible = false;
            GridCostDaimond.Columns["EmpCompondID"].Visible = false;
            GridCostDaimond.Columns["ComSignature"].Visible = false;
            GridCostDaimond.Columns["SalePrice"].Visible = false;
            GridCostDaimond.Columns["FromAccountID"].Visible = false;
            GridCostDaimond.Columns["FromAccountName"].Visible = false;
            GridCostDaimond.Columns["EmpCompundName"].Visible = false;
            GridCostDaimond.Columns["GoldDebit"].Visible = false;
            GridCostDaimond.Columns["ComStoneNumin"].Visible = false;
            GridCostDaimond.Columns["ComWeightStonin"].Visible = false;
            GridCostDaimond.Columns["ComStoneNumlas"].Visible = false;
            GridCostDaimond.Columns["ComWeightStonLas"].Visible = false;
            GridCostDaimond.Columns["BarcodCompond"].Visible = false;
            GridCostDaimond.Columns["TypeSton"].Visible = false;
            GridCostDaimond.Columns["DebitTime"].Visible = false;
            GridCostDaimond.Columns["DebitDate"].Visible = false;
            GridCostDaimond.Columns["CostPrice"].VisibleIndex = GridCostDaimond.Columns["ComWeightSton"].VisibleIndex + 1;
            if (UserInfo.Language == iLanguage.Arabic)
            {
                GridCostDaimond.Columns["EngSizeName"].Visible = false;
                GridCostDaimond.Columns["EngItemName"].Visible = false;
                GridCostDaimond.Columns["SizeID"].Caption = "رقم الوحده";
                GridCostDaimond.Columns[SizeName].Caption = "الوحده";
                GridCostDaimond.Columns["ItemID"].Caption = "رقم الصنف";

                GridCostDaimond.Columns["BarcodCompond"].Caption = "الكود";
                GridCostDaimond.Columns["TypeSton"].Caption = "نوع الحجر ";
                GridCostDaimond.Columns[ItemName].Caption = "اسم الصنف";
                GridCostDaimond.Columns["CostPrice"].Caption = "سعر التكلفة";
                GridCostDaimond.Columns["FromAccountName"].Caption = "اسم الحساب ";
                GridCostDaimond.Columns["EmpCompundName"].Caption = "اسم المركب ";
                // بيانات الذهب
                GridCostDaimond.Columns["GoldDebit"].Caption = "مسلم";
                GridCostDaimond.Columns["GoldCredit"].Caption = "مستلم";
                //الاحجار المسلمة
                GridCostDaimond.Columns["ComStoneNumin"].Caption = "عدد";
                GridCostDaimond.Columns["ComWeightStonin"].Caption = "الوزن";

                //الاحجار المرجعة
                GridCostDaimond.Columns["ComStoneNumout"].Caption = "عدد";
                GridCostDaimond.Columns["ComWeightStonOUt"].Caption = "الوزن";

                //الاحجار الفاقدة
                GridCostDaimond.Columns["ComStoneNumlas"].Caption = "عدد";
                GridCostDaimond.Columns["ComWeightStonLas"].Caption = "الوزن";
                //احجار مركبة
                GridCostDaimond.Columns["ComStoneCom"].Caption = "عدد";
                GridCostDaimond.Columns["ComWeightSton"].Caption = "الوزن";

                GridCostDaimond.Columns["FromAccountID"].Caption = "من حساب";

                GridCostDaimond.Columns["EmpCompondID"].Caption = "رقم المركب";
                GridCostDaimond.Columns["ComSignature"].Caption = "التوقيع";
                GridCostDaimond.Columns["SalePrice"].Caption = "سعر البيع";
                GridCostDaimond.Columns["DebitDate"].Caption = "التاريخ";
                GridCostDaimond.Columns["DebitTime"].Caption = "الوقت";

            }
            else
            {

                GridCostDaimond.Columns["ArbSizeName"].Visible = false;
                GridCostDaimond.Columns["ArbItemName"].Visible = false;
                GridCostDaimond.Columns["SizeID"].Caption = "Size ID";
                GridCostDaimond.Columns[SizeName].Caption = "Size Name";
                GridCostDaimond.Columns["ItemID"].Caption = "Item ID";
                GridCostDaimond.Columns["BarcodCompond"].Caption = "Barcod Compond";
                GridCostDaimond.Columns["TypeSton"].Caption = "Type Stone";
                GridCostDaimond.Columns[ItemName].Caption = "Item Name";
                GridCostDaimond.Columns["CostPrice"].Caption = "Cost Price";
                GridCostDaimond.Columns["FromAccountName"].Caption = "Acount Name";
                GridCostDaimond.Columns["EmpCompundName"].Caption = "Compund Name";
                // بيانات الذهب
                GridCostDaimond.Columns["GoldDebit"].Caption = "Debit";
                GridCostDaimond.Columns["GoldCredit"].Caption = "Credit";
                //الاحجار المسلمة
                GridCostDaimond.Columns["ComStoneNumin"].Caption = "Count";
                GridCostDaimond.Columns["ComWeightStonin"].Caption = "Weight";

                //الاحجار المرجعة
                GridCostDaimond.Columns["ComStoneNumout"].Caption = "Count";
                GridCostDaimond.Columns["ComWeightStonOUt"].Caption = "Weight";

                //الاحجار الفاقدة
                GridCostDaimond.Columns["ComStoneNumlas"].Caption = "Count";
                GridCostDaimond.Columns["ComWeightStonLas"].Caption = "Weight";
                //احجار مركبة
                GridCostDaimond.Columns["ComStoneCom"].Caption = "Count";
                GridCostDaimond.Columns["ComWeightSton"].Caption = "Weight";

                GridCostDaimond.Columns["FromAccountID"].Caption = "From Account";

                GridCostDaimond.Columns["EmpCompondID"].Caption = "Compond ID";
                GridCostDaimond.Columns["ComSignature"].Caption = "Signature";
                GridCostDaimond.Columns["SalePrice"].Caption = "Sale Price";
                GridCostDaimond.Columns["DebitDate"].Caption = "Date";
                GridCostDaimond.Columns["DebitTime"].Caption = "Time";
            }

        }
        void initGridProductionExpenses()
        {

            lstDetailProductionExpenses = new BindingList<Manu_ProductionExpensesDetails>();
            lstDetailProductionExpenses.AllowNew = true;
            lstDetailProductionExpenses.AllowEdit = true;
            lstDetailProductionExpenses.AllowRemove = true;
            gridControlProductionExpenses.DataSource = lstDetailProductionExpenses;

            RepositoryItemLookUpEdit rAccountName = Common.LookUpEditAccountName();
            gridControlProductionExpenses.RepositoryItems.Add(rAccountName);
            GridProductionExpenses.Columns["AccountName"].ColumnEdit = rAccountName;

            GridProductionExpenses.Columns["ComandID"].Visible = false;
            GridProductionExpenses.Columns["Cancel"].Visible = false;
            GridProductionExpenses.Columns["BranchID"].Visible = false;
            GridProductionExpenses.Columns["FacilityID"].Visible = false;

            GridProductionExpenses.Columns["EditUserID"].Visible = false;
            GridProductionExpenses.Columns["EditDate"].Visible = false;
            GridProductionExpenses.Columns["EditTime"].Visible = false;
            GridProductionExpenses.Columns["RegDate"].Visible = false;
            GridProductionExpenses.Columns["UserID"].Visible = false;

            GridProductionExpenses.Columns["ComputerInfo"].Visible = false;
            GridProductionExpenses.Columns["EditComputerInfo"].Visible = false;
            GridProductionExpenses.Columns["RegTime"].Visible = false;
            GridProductionExpenses.Columns["Installment"].OptionsColumn.AllowEdit = false;
            GridProductionExpenses.Columns["Installment"].OptionsColumn.AllowFocus = false;
            GridProductionExpenses.Columns["AverageHoursPerDay"].OptionsColumn.AllowEdit = false;
            GridProductionExpenses.Columns["AverageHoursPerDay"].OptionsColumn.AllowFocus = false;
            GridProductionExpenses.Columns["OrderCostPercentage"].OptionsColumn.AllowEdit = false;
            GridProductionExpenses.Columns["OrderCostPercentage"].OptionsColumn.AllowFocus = false;
            GridProductionExpenses.Columns["OrderCostPercentage"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            GridProductionExpenses.Columns["OrderCostPercentage"].SummaryItem.DisplayFormat = "{0:0.00}";
            if (UserInfo.Language == iLanguage.Arabic)
            {
                GridProductionExpenses.Columns["AccountID"].Caption = "رقم الحســـاب ";
                GridProductionExpenses.Columns["AccountName"].Caption = "إسم الحســـاب";

                GridProductionExpenses.Columns["MainValue"].Caption = "القيمة الاساسية";
                GridProductionExpenses.Columns["Installment"].Caption = "القسط المقابل";

                GridProductionExpenses.Columns["PeriodInDays"].Caption = "الفترة بالايام";
                GridProductionExpenses.Columns["AverageHoursPerDay"].Caption = "معدل 1.5 ساعة في اليوم";
                GridProductionExpenses.Columns["DepreciationPercentage"].Caption = " نسبة الإهلاك % ";

                GridProductionExpenses.Columns["OrderCostPercentage"].Caption = "نسبة تكلفة الطلبية  ";
            
            }
            else
            {
                GridProductionExpenses.Columns["AccountID"].Caption = "Account ID ";
                GridProductionExpenses.Columns["AccountName"].Caption = "Account Name";

                GridProductionExpenses.Columns["MainValue"].Caption = "Main Value";
                GridProductionExpenses.Columns["Installment"].Caption = "Installment";

                GridProductionExpenses.Columns["PeriodInDays"].Caption = "Period In Days";
                GridProductionExpenses.Columns["AverageHoursPerDay"].Caption = "Average Hours Per Day";
                GridProductionExpenses.Columns["DepreciationPercentage"].Caption = "Depreciation Percentage %";
                GridProductionExpenses.Columns["OrderCostPercentage"].Caption = "Order Cost Percentage  ";
            }
        }

        void initGridAlcadZircone()
        {

            lstDetailAlcadZircon = new BindingList<Manu_AuxiliaryMaterialsDetails>();
            lstDetailAlcadZircon.AllowNew = true;
            lstDetailAlcadZircon.AllowEdit = true;
            lstDetailAlcadZircon.AllowRemove = true;

            gridControlCostAlcadZircone.DataSource = lstDetailAlcadZircon;

           

            GridAlcadZircone.Columns["ArbItemName"].Visible = GridAlcadZircone.Columns["ArbItemName"].Name == "col" + ItemName ? true : false;
            GridAlcadZircone.Columns["EngItemName"].Visible = GridAlcadZircone.Columns["EngItemName"].Name == "col" + ItemName ? true : false;
            GridAlcadZircone.Columns[ItemName].Visible = true;
            GridAlcadZircone.Columns[ItemName].Caption = CaptionItemName;

  
            GridAlcadZircone.Columns["CommandID"].Visible = false;

            GridAlcadZircone.Columns["BranchID"].Visible = false;
            GridAlcadZircone.Columns["FacilityID"].Visible = false;

            GridAlcadZircone.Columns["TypeOpration"].Visible = false;

            GridAlcadZircone.Columns["TotalCost"].OptionsColumn.ReadOnly = true;
            GridAlcadZircone.Columns["TotalCost"].OptionsColumn.AllowFocus = false;
            GridAlcadZircone.Columns["DateROrD"].OptionsColumn.ReadOnly = true;
            GridAlcadZircone.Columns["DateROrD"].OptionsColumn.AllowFocus = false;
            GridAlcadZircone.Columns["TimeROrD"].OptionsColumn.ReadOnly = true;
            GridAlcadZircone.Columns["TimeROrD"].OptionsColumn.AllowFocus = false;
            GridAlcadZircone.Columns["Fingerprint"].Visible = false;
            GridAlcadZircone.Columns["StoreName"].Visible = false;
            GridAlcadZircone.Columns["EmpFactorID"].Visible = false;
            GridAlcadZircone.Columns["StoreID"].Visible = false;
            GridAlcadZircone.Columns["EmpFactorName"].Visible = false;
            GridAlcadZircone.Columns["DateROrD"].Visible = false;
            GridAlcadZircone.Columns["TimeROrD"].Visible = false;
            GridAlcadZircone.Columns["SizeID"].Visible = false;
            GridAlcadZircone.Columns[ItemName].Width = 150;
            GridAlcadZircone.Columns[SizeName].Width = 120;
            GridAlcadZircone.Columns["TotalCost"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            GridAlcadZircone.Columns["TotalCost"].SummaryItem.DisplayFormat = "{0:0.00}";
            if (UserInfo.Language == iLanguage.Arabic)
            {
                GridAlcadZircone.Columns["EngItemName"].Visible = false;
                GridAlcadZircone.Columns["EngSizeName"].Visible = false;
                GridAlcadZircone.Columns["SizeID"].Caption = "رقم الوحدة";
                GridAlcadZircone.Columns["ItemID"].Caption = "رقم الصنــف";
                GridAlcadZircone.Columns["BarCode"].Caption = "باركود الصنف";
                GridAlcadZircone.Columns[SizeName].Caption = "الوحدة ";
                GridAlcadZircone.Columns["QTY"].Caption = "الكمية ";
                GridAlcadZircone.Columns["CostPrice"].Caption = "القيمة";
                GridAlcadZircone.Columns["TotalCost"].Caption = "الإجمالي ";
                GridAlcadZircone.Columns["Fingerprint"].Caption = "البصمــة";
                GridAlcadZircone.Columns["DateROrD"].Caption = "التاريــخ";
                GridAlcadZircone.Columns["TimeROrD"].Caption = "الوقـــت";
                GridAlcadZircone.Columns["StoreID"].Caption = "رقم المخزن ";
                GridAlcadZircone.Columns["StoreName"].Caption = "إسم المخزن";
                GridAlcadZircone.Columns["EmpFactorID"].Caption = " رقم العامل";
                GridAlcadZircone.Columns["EmpFactorName"].Caption = "إسم العــامل ";
            }
            else
            {
                GridAlcadZircone.Columns["ArbItemName"].Visible = false;
                GridAlcadZircone.Columns["ArbSizeName"].Visible = false;
                GridAlcadZircone.Columns["SizeID"].Caption = "Unit ID";
                GridAlcadZircone.Columns["ItemID"].Caption = "Item ID";
                GridAlcadZircone.Columns["BarCode"].Caption = "BarCode";
                GridAlcadZircone.Columns["MachineName"].Caption = "Machin Name";
                GridAlcadZircone.Columns[SizeName].Caption = "Unit Name ";
                GridAlcadZircone.Columns["CostPrice"].Caption = "Cost Price";
                GridAlcadZircone.Columns["QTY"].Caption = "QTY";
                GridAlcadZircone.Columns["TotalCost"].Caption = "Total Cost ";
                GridAlcadZircone.Columns["DateRorD"].Caption = "Date";
                GridAlcadZircone.Columns["Fingerprint"].Caption = "Fingerprint";

                GridAlcadZircone.Columns["TimeROrD"].Caption = "Time";
                GridAlcadZircone.Columns["StoreID"].Caption = "Store ID ";
                GridAlcadZircone.Columns["StoreName"].Caption = "Store Name";
                GridAlcadZircone.Columns["EmpFactorID"].Caption = "Emp Factor ID";
                GridAlcadZircone.Columns["EmpFactorName"].Caption = "Emp Factor Name";
            }

        }

        void initGridCodeing()
        {
            lstDetailUnit = new BindingList<Stc_ItemUnits>();
            lstDetailUnit.AllowNew = true;
            lstDetailUnit.AllowEdit = true;
            lstDetailUnit.AllowRemove = true;
            gridControlCodeing.DataSource = lstDetailUnit;
            GridViewCodeing.Columns["ItemID"].VisibleIndex = 0;
            GridViewCodeing.Columns["BarCode"].VisibleIndex = 1;
            GridViewCodeing.Columns["SizeID"].Visible = false;
            GridViewCodeing.Columns["ID"].Visible = false;
            GridViewCodeing.Columns["ItemID"].Visible = false;
            GridViewCodeing.Columns["AverageCostPrice"].Visible = false;      
            GridViewCodeing.Columns["Equivalen"].Visible = false;
            GridViewCodeing.Columns["Height"].Visible = false;
            GridViewCodeing.Columns["Width"].Visible = false;
            GridViewCodeing.Columns["Serials"].Visible = false;
            GridViewCodeing.Columns["Stc_Items"].Visible = false;
            GridViewCodeing.Columns["BranchID"].Visible = false;
            GridViewCodeing.Columns["FacilityID"].Visible = false;
            GridViewCodeing.Columns["UnitCancel"].Visible = false;
            GridViewCodeing.Columns["SizeID"].Visible = false;
            GridViewCodeing.Columns["SpecialSalePrice"].Visible = false;
            GridViewCodeing.Columns["SpecialCostPrice"].Visible = false;
            GridViewCodeing.Columns["LastCostPrice"].Visible = false;
            GridViewCodeing.Columns["LastSalePrice"].Visible = false;
            GridViewCodeing.Columns["ItemProfit"].Visible = false;
            GridViewCodeing.Columns["LastSalePrice"].Visible = false;
            GridViewCodeing.Columns["PackingQty"].Visible = true;
            GridViewCodeing.Columns["MinLimitQty"].Visible = true;
            GridViewCodeing.Columns["MaxLimitQty"].Visible = false;
            GridViewCodeing.Columns["SpecialCostPrice"].Visible = false;
            GridViewCodeing.Columns["ItemProfit"].Visible = false;
            GridViewCodeing.Columns["AllowedPercentDiscount"].Visible = false;
            GridViewCodeing.Columns["Color"].Visible = false;
            GridViewCodeing.Columns["CLARITY"].Visible = false;
            GridViewCodeing.Columns["PackingQty"].Visible = false;
            GridViewCodeing.Columns["ArbSizeName"].Visible = GridViewCodeing.Columns["ArbSizeName"].Name == "col" + SizeName ? true : false;
            GridViewCodeing.Columns["EngSizeName"].Visible = GridViewCodeing.Columns["EngSizeName"].Name == "col" + SizeName ? true : false;
            if (UserInfo.Language == iLanguage.Arabic)
            {
                GridViewCodeing.Columns["STONE_W"].Caption = "وزن الأحجار";
                GridViewCodeing.Columns["BAGET_W"].Caption = "الباجيت";
                GridViewCodeing.Columns["DIAMOND_W"].Caption = "وزن الالماس";
                GridViewCodeing.Columns["ZIRCON_W"].Caption = "وزن الزركون";
                GridViewCodeing.Columns["Caliber"].Caption = "العيـار";
                //GridViewCodeing.Columns["ArbName"].Caption = "اسم الصنــف";
                GridViewCodeing.Columns["CostPrice"].Caption = "سعر التكلفة ";
                GridViewCodeing.Columns["MinLimitQty"].Caption = "وزن الذهب ";
                GridViewCodeing.Columns["ItemID"].Caption = "رقم الصنف";
                GridViewCodeing.Columns["BarCode"].Caption = "الباركود";
                GridViewCodeing.Columns["SalePrice"].Caption = "سعر البيع";
                GridViewCodeing.Columns["ArbSizeName"].Caption = "اسم المجموعة";
                GridViewCodeing.Columns["SizeID"].Caption = "رقم المجموعة";      
            }
            else
            {
                GridViewCodeing.Columns["STONE_W"].Caption = "STONE ";
                GridViewCodeing.Columns["Caliber"].Caption = "Caliber";
                GridViewCodeing.Columns["BAGET_W"].Caption = "BAGET";
                GridViewCodeing.Columns["ZIRCON_W"].Caption = "ZIRCON";
                GridViewCodeing.Columns["DIAMOND_W"].Caption = "DIAMOND";
                GridViewCodeing.Columns["BarcodeSecound"].Caption = "Barcode Secound";
                GridViewCodeing.Columns["ArbName"].Caption = "Name Item ";
                GridViewCodeing.Columns["CostPrice"].Caption = "Cost Price ";
                GridViewCodeing.Columns["Qty"].Caption = "Qty";
                GridViewCodeing.Columns["ArbSizeName"].Caption = "Size Name  ";
                GridViewCodeing.Columns["ItemID"].Caption = "Item ID";
                GridViewCodeing.Columns["BarCode"].Caption = "BarCode";
                GridViewCodeing.Columns["SalePrice"].Caption = "Sale Price";
                GridViewCodeing.Columns["SizeID"].Caption = "Size ID ";
            }



        }
        #endregion
        private void frmManufacturingOrder_Load(object sender, EventArgs e)
        {
            try
            {
                initGridBeforPrentage();
                initGridAfterPrentage();
                initGridBeforCompent();
                initGridAfterCompent();
                initGridBeforTalmee();
                initGridAfterTalmee();
                initGridSelver();
                initGridFactory();
                initGridAfterFactory();
                initGridProductionExpenses();
                initGridAlcadZircone();
                initGridCostDaimond();
                initGridCodeing();

                DoNew();

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
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
                strSQL = "SELECT ArbName as GroupName FROM Stc_ItemsGroups WHERE GroupID =" +txtGroupID.Text + " And Cancel =0 ";
                // Execute SQL query to fetch GroupName based on the entered GroupID
                CSearch.ControlValidating(txtGroupID, txtGroupName, strSQL);//  Call This  Function For Set  GroupName to txtGroupName when The user Select GroupID
                if (IsNewRecord == true)
                {
                    var groupId = Comon.cDbl(txtGroupID.Text);
                    var dtGroups = Lip.SelectRecord("SELECT Notes FROM Stc_ItemsGroups WHERE GroupID = " + groupId + " AND Cancel = 0");
                    double ItemG = 0;
                    cItems Item = new cItems();
                    Boolean IsNewItem = false;
                    long ItemID = Comon.cInt(Lip.GetValue(" Select ItemID from Stc_ItemUnits  where BarCode='" +txtBarCode.Text.Trim() + "'"));
                    if (ItemID == 0)
                    {
                        ItemID = Item.GetNewID();
                        ItemG = Lip.GetNewID(groupId);
                        IsNewItem = true;
                    }
                    else
                        ItemG = Comon.cInt(Lip.GetValue(" Select ItemGroupID from Stc_Items Where ItemID=" + ItemID).ToString());


                    if (dtGroups.Rows.Count > 0)
                    {
                        var groupName = dtGroups.Rows[0]["Notes"].ToString();
                        var dtMaxBarcode = Lip.SelectRecord("SELECT Max(ItemGroupID)+1 FROM Stc_Items Where GroupID = " + groupId + " AND BaseID<>1 and TypeID<>" + 4);

                        strSQL = "Select Notes From Stc_ItemsGroups where GroupID=" + groupId;
                        DataTable dtGroup = Lip.SelectRecord(strSQL);
                        string GroupName = dtGroup.Rows[0]["Notes"].ToString();
                         txtBarCode.Text = GroupName + ItemG.ToString().PadLeft(4, '0');
                       
                      

                    }

                }
            }
            catch (Exception ex)
            {
                // If there is an error, show an error message
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

           
                 
        }
        private void txtBrandID_Validating(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBrandID.Text))
            try
            {   //This Stetment To Select Name when the validat txtBrandID
                strSQL = "SELECT ArbName as BrandName,InvoiceImage FROM Stc_ItemsBrands WHERE BrandID =" + Comon.cInt(txtBrandID.Text) + " And Cancel =0 ";
                CSearch.ControlValidating(txtBrandID, txtBrandName, strSQL);
                DataTable dt = Lip.SelectRecord(strSQL);
                if (!dt.Rows[0].IsNull("InvoiceImage"))
                {
                    byte[] imageData1 = (byte[])DataRecord.Rows[0]["InvoiceImage"];
                    using (MemoryStream ms = new MemoryStream(imageData1))
                    {
                        Image image = Image.FromStream(ms);
                        pictureEdit1.Image = image;
                    }
                }
            }
            catch (Exception ex)
            {
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
                    strSQL = "SELECT ArbName as CustomerName  FROM Sales_CustomerAnSublierListArb Where    AcountID =" + txtCustomerID.Text;
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
        /// <summary>
        /// This Event To txtTypeID Validating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTypeID_Validating(object sender, CancelEventArgs e)
        {
            string strSQL;
            strSQL = "SELECT "+PrimaryName+" as TypeName FROM Stc_ItemTypes WHERE TypeID =" + Comon.cInt(txtTypeID.Text) + " And Cancel =0 ";
            CSearch.ControlValidating(txtTypeID, txtTypeName, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID
            //CSearch.ControlValidating(txtTypeID, txtItemName, strSQL);
          

        }
        private void txtEmpFactorID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT "+PrimaryName+" as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + txtEmpIDFactor.Text + " And Cancel =0 ";
                CSearch.ControlValidating(txtEmpIDFactor, lblEmpNameFactor, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void txtEmplooyBrntageID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtEmplooyID.Text) + " And Cancel =0 ";
                CSearch.ControlValidating(txtEmplooyID, lblEmplooyName, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }

        private void txtEmployeeStokID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtEmployeeStokIDFactory.Text) + " And Cancel =0 ";
                CSearch.ControlValidating(txtEmployeeStokIDFactory, txtEmployeeStokNameFactory, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        private void txtEmplooyPolishingID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtEmplooyIDPolishing.Text) + " And Cancel =0 ";
                CSearch.ControlValidating(txtEmplooyIDPolishing,lblEmpolyeePolishingName, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID
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

            // Check if the F9 key is pressed and call the DoSave() function if it is
            if (e.KeyCode == Keys.F9)
                DoSave();
        }
        #endregion

        #region Function
        private void CalculatePrentageLost()
        {
            decimal ToatlBeforPrntageQty = 0;
            decimal ToatlAfterPrntageQty = 0;
            for (int i = 0; i <= GridViewBeforPrentag.DataRowCount - 1; i++)
                ToatlBeforPrntageQty += Comon.cDec(GridViewBeforPrentag.GetRowCellValue(i, "PrentagDebit").ToString());
            for (int i = 0; i <= GridViewAfterPrentag.DataRowCount - 1; i++)
                ToatlAfterPrntageQty += Comon.cDec(GridViewAfterPrentag.GetRowCellValue(i, "PrentagCredit").ToString());

            txtTotalLostPrntage.Text = Comon.cDec(ToatlBeforPrntageQty - ToatlAfterPrntageQty) + "";

        }
        private void CalculatePolishnLost()
        {
            decimal ToatlBeforPolishnQty = 0;
            decimal ToatlAfterPolishnQty = 0;

            for (int i = 0; i <= GridViewBeforPolish.DataRowCount - 1; i++)
                ToatlBeforPolishnQty += Comon.cDec(GridViewBeforPolish.GetRowCellValue(i, "Debit").ToString());
            for (int i = 0; i <= GridViewAfterPolish.DataRowCount - 1; i++)
                ToatlAfterPolishnQty += Comon.cDec(GridViewAfterPolish.GetRowCellValue(i, "Credit").ToString());

            txtTotalLostPolish.Text = Comon.cDec(ToatlBeforPolishnQty - ToatlAfterPolishnQty) + "";

        }
        private void CalculateFactoryLost()
        {
            decimal ToatlBeforFactoryQty = 0;
            decimal ToatlAfterFactoryQty = 0;

            for (int i = 0; i <= GridViewBeforfactory.DataRowCount - 1; i++)
                ToatlBeforFactoryQty += Comon.cDec(GridViewBeforfactory.GetRowCellValue(i, "Debit").ToString());
            for (int i = 0; i <= GridViewAfterfactory.DataRowCount - 1; i++)
                ToatlAfterFactoryQty += Comon.cDec(GridViewAfterfactory.GetRowCellValue(i, "Credit").ToString());

            lblTotallostFactory.Text = Comon.cDec(ToatlBeforFactoryQty - ToatlAfterFactoryQty) + "";

        }
        private void CalculateAdditionalQTY()
        {
            decimal ToatlBeforAdditionalQty = 0;
            for (int i = 0; i <= gridViewAdditional.DataRowCount - 1; i++)
                ToatlBeforAdditionalQty += Comon.cDec(gridViewAdditional.GetRowCellValue(i, "Debit").ToString());

            txtTotalQtyAdditional.Text = Comon.cDec(ToatlBeforAdditionalQty) + "";

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
            else if (FocusedControl.Trim() == txtCostCenterID.Name)
            {
                if (!MySession.GlobalAllowChangefrmSaleCostCenterID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "رقم مركز التكلفة", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "Cost Center ID", MySession.GlobalBranchID);
            }
            if (FocusedControl.Trim() == txtStoreIDFactory.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtStoreIDFactory, lblStoreNameFactory, "StoreID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtStoreIDFactory, lblStoreNameFactory, "StoreID", "Account ID", MySession.GlobalBranchID);
            }
            if (FocusedControl.Trim() == txtStoreIDPrentage.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtStoreIDPrentage,lblStoreNamePrentage  , "StoreID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtStoreIDPrentage, lblStoreNamePrentage, "StoreID", "Account ID", MySession.GlobalBranchID);
            }
            if (FocusedControl.Trim() == txtStoreIDBeforComond.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtStoreIDBeforComond, lblStoreNameBeforCompond, "StoreID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtStoreIDBeforComond, lblStoreNameBeforCompond, "StoreID", "Account ID", MySession.GlobalBranchID);
            }
            if (FocusedControl.Trim() == txtStoreIDPolishing.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtStoreIDPolishing, lblStoreNamePolishin, "StoreID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtStoreIDPolishing, lblStoreNamePolishin, "StoreID", "Account ID", MySession.GlobalBranchID);
            }
            if (FocusedControl.Trim() == txtCommandID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCommandID, null, "CommandID", "رقم الأمر", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtCommandID, null, "CommandID", "Command ID", MySession.GlobalBranchID);
            }
            //الاصناف
            else if (FocusedControl.Trim() == txtItemID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtItemID, null, "Items", "رقـم الـمــادة", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtItemID, null, "Items", "Item ID", MySession.GlobalBranchID);

            }
            else if (FocusedControl.Trim() ==txtGroupID.Name )
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtGroupID, txtGroupName, "GroupID", "رقـم المجـمـوعة", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtGroupID, txtGroupName, "GroupID", "Group ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtTypeID.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtTypeID,txtTypeName, "TypeID", "الــــنـــــــــــوع", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtTypeID, txtTypeName, "TypeID", "Type ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtBrandID.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtBrandID, txtBrandName, "BrandID", "رقـم المودل", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtBrandID, txtBrandName, "BrandID", "Brand ID", MySession.GlobalBranchID);
            }
            //رقم الحساب
            else if(FocusedControl.Trim() == txtAccountIDFactory.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtAccountIDFactory, lblAccountNamePrentage, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtAccountIDFactory, lblAccountNamePrentage, "AccountID", "Account ID", MySession.GlobalBranchID);
           
            }
            else if (FocusedControl.Trim() == txtAccountIDPrentage.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls,txtAccountIDPrentage ,lblAccountNamePrentage    , "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtAccountIDPrentage, lblAccountNamePrentage, "AccountID", "Account ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtAccountIDBeforCompond.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtAccountIDBeforCompond, lblAccountNameBeforCompond, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtAccountIDBeforCompond, lblAccountNameBeforCompond, "AccountID", "Account ID", MySession.GlobalBranchID);
            }
            
            else if (FocusedControl.Trim() == txtAccountIDAdditions.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtAccountIDAdditions, lblAccountNameAdditions, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtAccountIDAdditions, lblAccountNameAdditions, "AccountID", "Account ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtAccountIDPolishing.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtAccountIDPolishing, lblAccountNamePolishin, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtAccountIDPolishing, lblAccountNamePolishin, "AccountID", "Account ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtAccountIDBarcodeItem.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtAccountIDBarcodeItem, lblAccountNameBarcodeItem, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtAccountIDBarcodeItem, lblAccountNameBarcodeItem, "AccountID", "Account ID", MySession.GlobalBranchID);
            }

            //العامل
            else if (FocusedControl.Trim() == txtEmpIDFactor.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtEmpIDFactor, lblEmpNameFactor, "EmployeeID", "رقـم العامل", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtEmpIDFactor, lblEmpNameFactor, "EmployeeID", "Worker ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtEmplooyID.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtEmplooyID, lblEmplooyName, "EmployeeID", "رقـم العامل", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtEmplooyID, lblEmplooyName, "EmployeeID", "Worker ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtEmplooyIDPolishing.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtEmplooyIDPolishing, lblEmpolyeePolishingName, "EmployeeID", "رقـم العامل", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtEmplooyIDPolishing, lblEmpolyeePolishingName, "EmployeeID", "Worker ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtEmpIDPrentage.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtEmpIDPrentage, lblEmpNamePrentage, "EmployeeID", "رقـم العامل", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtEmpIDPrentage, lblEmpNamePrentage, "EmployeeID", "Worker ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtEmpIDBeforCompond.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtEmpIDBeforCompond, lblEmpNameBeforCompond, "EmployeeID", "رقـم العامل", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtEmpIDBeforCompond, lblEmpNameBeforCompond, "EmployeeID", "Worker ID", MySession.GlobalBranchID);
            }
             
            else if (FocusedControl.Trim() == txtEmpIDAdditions.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtEmpIDAdditions, lblEmpNameAdditions, "EmployeeID", "رقـم العامل", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtEmpIDAdditions, lblEmpNameAdditions, "EmployeeID", "Worker ID", MySession.GlobalBranchID);
            }
            //امين المخزن
            else if (FocusedControl.Trim() == txtEmployeeStokIDFactory.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return ; }

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtEmployeeStokIDFactory, txtEmployeeStokNameFactory, "EmployeeID", "رقـم العامل", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtEmployeeStokIDFactory, txtEmployeeStokNameFactory, "EmployeeID", "Worker ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtEmployeeStokIDPrentage.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtEmployeeStokIDPrentage, lblEmployeeStokName, "EmployeeID", "رقـم العامل", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtEmployeeStokIDPrentage, lblEmployeeStokName, "EmployeeID", "Worker ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtEmployeeStokIDBeforCompond.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtEmployeeStokIDBeforCompond, lblEmployeeStokBeforCompond, "EmployeeID", "رقـم العامل", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtEmployeeStokIDBeforCompond, lblEmployeeStokBeforCompond, "EmployeeID", "Worker ID", MySession.GlobalBranchID);
            }
             
            else if (FocusedControl.Trim() == txtEmployeeStokIDAdditions.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtEmployeeStokIDAdditions, lblEmployeeNameStokAdditions, "EmployeeID", "رقـم العامل", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtEmployeeStokIDAdditions, lblEmployeeNameStokAdditions, "EmployeeID", "Worker ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtEmployeeStokIDPolishing.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtEmployeeStokIDPolishing, lblEmployeeNameStokPolishin, "EmployeeID", "رقـم العامل", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtEmployeeStokIDPolishing, lblEmployeeNameStokPolishin, "EmployeeID", "Worker ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtEmployeeStokIDBarcode.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtEmployeeStokIDBarcode, lblEmployeeNameStokBarcodeItem, "EmployeeID", "رقـم العامل", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtEmployeeStokIDBarcode, lblEmployeeNameStokBarcodeItem, "EmployeeID", "Worker ID", MySession.GlobalBranchID);
            }

            //العميل والمندوب
            else if (FocusedControl.Trim() == txtDelegateID.Name)
            {
              if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtDelegateID, lblDelegateName, "SaleDelegateID", "رقم مندوب المبيعات", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtDelegateID, lblDelegateName, "SaleDelegateID", "Delegate ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtCustomerID.Name)
            {
                
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCustomerID, lblCustomerName, "CustomerIDAndSublierID", "رقم الــعـــمـــيــل", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtCustomerID, lblCustomerName, "CustomerIDAndSublierID", "SublierID ID", MySession.GlobalBranchID);

            }
            //الجرايد فيو
            else if (FocusedControl.Trim() == gridControlBeforPrentag.Name)
            {
                if (GridViewBeforPrentag.FocusedColumn.Name == "colStoreID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "رقم الـمـســتـودع", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "Store ID", MySession.GlobalBranchID);
                }
                if (GridViewBeforPrentag.FocusedColumn.Name == "colItemID")
                {
                    if (GridViewBeforPrentag.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "ItemID", "رقم المادة", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "ItemID", "Item ID", MySession.GlobalBranchID);
                }
                if (GridViewBeforPrentag.FocusedColumn.Name == "MachinID")
                {
                    if (GridViewBeforPrentag.FocusedColumn == null) return ;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "رقم المكينة", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "Machine ID", MySession.GlobalBranchID);
                }
                else if (GridViewBeforPrentag.FocusedColumn.Name == "colSizeID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "رقـم الـوحـــده", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "Size ID", MySession.GlobalBranchID);
                }
                else if (GridViewBeforPrentag.FocusedColumn.Name == "colEmpID")
                {
                    if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "رقـم العامل", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "Worker ID", MySession.GlobalBranchID);
                }
            }
            else if (FocusedControl.Trim() == gridControlfactroOpretion.Name)
            {
                if (GridViewBeforfactory.FocusedColumn.Name == "colStoreID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "رقم الـمـســتـودع", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "Store ID", MySession.GlobalBranchID);
                }
                if (GridViewBeforfactory.FocusedColumn.Name == "colItemID")
                {
                    if (GridViewBeforfactory.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "ItemID", "رقم المادة", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "ItemID", "Item ID", MySession.GlobalBranchID);
                }
                if (GridViewBeforfactory.FocusedColumn.Name == "MachinID")
                {
                    if (GridViewBeforfactory.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "رقم المكينة", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "Machine ID", MySession.GlobalBranchID);
                }
                else if (GridViewBeforfactory.FocusedColumn.Name == "colSizeID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "رقـم الـوحـــده", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "Size ID", MySession.GlobalBranchID);
                }
                else if (GridViewBeforfactory.FocusedColumn.Name == "colEmpID")
                {
                    if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "رقـم العامل", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "Worker ID", MySession.GlobalBranchID);
                }
            }
            else if (FocusedControl.Trim() == gridControlAfterFactory.Name)
            {
                if (GridViewAfterfactory.FocusedColumn.Name == "colStoreID")
                {

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "رقم الـمـســتـودع", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "Store ID", MySession.GlobalBranchID);
                }
                if (GridViewAfterfactory.FocusedColumn.Name == "colItemID")
                {
                    if (GridViewAfterfactory.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "ItemID", "رقم المادة", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "ItemID", "Item ID", MySession.GlobalBranchID);
                }
                if (GridViewAfterfactory.FocusedColumn.Name == "MachinID")
                {
                    if (GridViewAfterfactory.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "رقم المكينة", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "Machine ID", MySession.GlobalBranchID);
                }
                else if (GridViewAfterfactory.FocusedColumn.Name == "colSizeID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "رقـم الـوحـــده", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "Size ID", MySession.GlobalBranchID);
                }
                else if (GridViewAfterfactory.FocusedColumn.Name == "colEmpID")
                {
                    if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "رقـم العامل", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "Worker ID", MySession.GlobalBranchID);
                }
            }
            else if (FocusedControl.Trim() == gridControlAfterPrentage.Name)
            {
                if (GridViewAfterPrentag.FocusedColumn.Name == "colStoreID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "رقم الـمـســتـودع", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "Store ID", MySession.GlobalBranchID);
                }
                if (GridViewAfterPrentag.FocusedColumn.Name == "colItemID")
                {
                    if (GridViewAfterPrentag.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "ItemID", "رقم المادة", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "ItemID", "Item ID", MySession.GlobalBranchID);
                    //if (UserInfo.Language == iLanguage.Arabic)
                    //    PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                    //else
                    //    PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                }
                if (GridViewAfterPrentag.FocusedColumn.Name == "MachinID")
                {
                    if (GridViewAfterPrentag.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "رقم المكينة", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "Machine ID", MySession.GlobalBranchID);
                }
                else if (GridViewAfterPrentag.FocusedColumn.Name == "colSizeID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "رقـم الـوحـــده", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "Size ID", MySession.GlobalBranchID);
                }
                else if (GridViewAfterPrentag.FocusedColumn.Name == "colEmpID")
                {
                    if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "رقـم العامل", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "Worker ID", MySession.GlobalBranchID);
                }
            }

            else if (FocusedControl.Trim() == gridControlBeforCompond.Name)
            {

                if (gridViewBeforCompond.FocusedColumn.Name == "BarcodCompond")
                {
                    if (gridViewBeforCompond.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "ItemBarcode", "البـاركـود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "ItemBarcode", "BarCode", MySession.GlobalBranchID);
                }
                if (gridViewBeforCompond.FocusedColumn.Name == "colItemID")
                {
                    if (gridViewBeforCompond.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "ItemID", "رقم المادة", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "ItemID", "Item ID", MySession.GlobalBranchID);
                }
                if (gridViewBeforCompond.FocusedColumn.Name == "FromAccountID")
                {
                    if (gridViewBeforCompond.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                }
                if (gridViewBeforCompond.FocusedColumn.Name == "EmpCompondID")
                {
                    if (gridViewBeforCompond.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "رقـم العامل", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "Worker ID", MySession.GlobalBranchID);
                }
            }
            else if (FocusedControl.Trim() == gridControlAfterCompond.Name)
            {
                if (gridViewAfterCompond.FocusedColumn.Name == "BarcodCompond")
                {
                    if (gridViewAfterCompond.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "ItemBarcode", "البـاركـود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "ItemBarcode", "BarCode", MySession.GlobalBranchID);
                }
                if (gridViewAfterCompond.FocusedColumn.Name == "colItemID")
                {
                    if (gridViewAfterCompond.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "ItemID", "رقم المادة", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "ItemID", "Item ID", MySession.GlobalBranchID);
                }
                if (gridViewAfterCompond.FocusedColumn.Name == "FromAccountID")
                {
                    if (gridViewAfterCompond.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                }
                if (gridViewAfterCompond.FocusedColumn.Name == "EmpCompondID")
                {
                    if (gridViewAfterCompond.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "رقـم العامل", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "Worker ID", MySession.GlobalBranchID);
                }
            }
            else if (FocusedControl.Trim() == gridControlBeforePolishing.Name)
            {
                if (GridViewBeforPolish.FocusedColumn.Name == "colStoreID")
                {

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "رقم الـمـســتـودع", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "Store ID", MySession.GlobalBranchID);
                }
                if (GridViewBeforPolish.FocusedColumn.Name == "colItemID")
                {
                    if (gridViewBeforCompond.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "ItemID", "رقم المادة", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "ItemID", "Item ID", MySession.GlobalBranchID);
                }
                if (GridViewBeforPolish.FocusedColumn.Name == "MachinID")
                {
                    if (GridViewBeforPolish.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "رقم المكينة", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "Machine ID", MySession.GlobalBranchID);
                }
                else if (GridViewBeforPolish.FocusedColumn.Name == "colSizeID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "رقـم الـوحـــده", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "Size ID", MySession.GlobalBranchID);
                }
                else if (GridViewBeforPolish.FocusedColumn.Name == "colEmpID")
                {
                    if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "رقـم العامل", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "Worker ID", MySession.GlobalBranchID);
                }
            }
            else if (FocusedControl.Trim() == gridControlAfterPolishing.Name)
            {
                if (GridViewAfterPolish.FocusedColumn.Name == "colStoreID")
                {

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "رقم الـمـســتـودع", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "Store ID", MySession.GlobalBranchID);
                }
                if (GridViewAfterPolish.FocusedColumn.Name == "colItemID")
                {
                    if (GridViewAfterPolish.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "ItemID", "رقم المادة", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "ItemID", "Item ID", MySession.GlobalBranchID);
                }
                if (GridViewAfterPolish.FocusedColumn.Name == "MachinID")
                {
                    if (GridViewAfterPolish.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "رقم المكينة", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "Machine ID", MySession.GlobalBranchID);
                }
                else if (GridViewAfterPolish.FocusedColumn.Name == "colSizeID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "رقـم الـوحـــده", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "Size ID", MySession.GlobalBranchID);
                }
                else if (GridViewAfterPolish.FocusedColumn.Name == "colEmpID")
                {
                    if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "رقـم العامل", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "Worker ID", MySession.GlobalBranchID);
                }
            }
            else if (FocusedControl.Trim() == gridControlAdditional.Name)
            {
                if (gridViewAdditional.FocusedColumn.Name == "colStoreID")
                {

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "رقم الـمـســتـودع", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "Store ID", MySession.GlobalBranchID);
                }
                if (gridViewAdditional.FocusedColumn.Name == "colItemID")
                {
                    if (gridViewAdditional.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "ItemID", "رقم المادة", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "ItemID", "Item ID", MySession.GlobalBranchID);
                }
                if (gridViewAdditional.FocusedColumn.Name == "MachinID")
                {
                    if (gridViewAdditional.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "رقم المكينة", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "MachineID", "Machine ID", MySession.GlobalBranchID);
                }

                if (gridViewAdditional.FocusedColumn.Name == "BarcodSelver")
                {
                    if (gridViewAdditional.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "ItemBarcode", "البـاركـود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "ItemBarcode", "BarCode", MySession.GlobalBranchID);
                }
                
                else if (gridViewAdditional.FocusedColumn.Name == "colSizeID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "رقـم الـوحـــده", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "Size ID", MySession.GlobalBranchID);
                }
                else if (gridViewAdditional.FocusedColumn.Name == "colEmpID")
                {
                    if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "رقـم العامل", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "Worker ID", MySession.GlobalBranchID);
                }
            }
            else if (FocusedControl.Trim() == gridControlProductionExpenses.Name)
            {
                if (GridProductionExpenses.FocusedColumn.Name == "colAccountID")
                {
                    if (GridProductionExpenses.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
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
        private void txtCostCenterID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as CostCenterName FROM Acc_CostCenters WHERE CostCenterID =" + Comon.cInt(txtCostCenterID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtCostCenterID, lblCostCenterName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
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
                else if (FocusedControl == txtCostCenterID.Name)
                {
                    txtCostCenterID.Text = cls.PrimaryKeyValue.ToString();
                    txtCostCenterID_Validating(null, null);
                }
                //الصنف
                else  if (FocusedControl == txtTypeID.Name)
                {
                    txtTypeID.Text = cls.PrimaryKeyValue.ToString();
                    txtTypeID_Validating(null, null);
                }
                else if (FocusedControl == txtGroupID.Name)
                {
                    txtGroupID.Text = cls.PrimaryKeyValue.ToString();
                    txtGroupID_Validating(null, null);
                }
                else if (FocusedControl == txtBrandID.Name)
                {
                    txtBrandID.Text = cls.PrimaryKeyValue.ToString();
                    txtBrandID_Validating(null, null);
                }
                else if (FocusedControl == txtItemID.Name)
                {
                    txtItemID.Text = cls.PrimaryKeyValue.ToString();
                    txtItemID_Validating(null, null);
                }

                //المخزن
                else if (FocusedControl == txtStoreIDFactory.Name)
                {
                    txtStoreIDFactory.Text = cls.PrimaryKeyValue.ToString();
                    txtStoreIDFactory_Validating(null, null);
                }
                //المخزن
                else if (FocusedControl == txtStoreIDPrentage.Name)
                {
                    txtStoreIDPrentage.Text = cls.PrimaryKeyValue.ToString();
                    txtStoreIDPrentage_Validating(null, null);
                }
                
               
                else if (FocusedControl == txtStoreIDBeforComond.Name)
                {
                    txtStoreIDBeforComond.Text = cls.PrimaryKeyValue.ToString();
                    txtStoreIDBeforComond_Validating(null, null);
                }
                 
                else if (FocusedControl == txtStoreIDPolishing.Name)
                {
                    txtStoreIDPolishing.Text = cls.PrimaryKeyValue.ToString();
                    txtStoreIDPolishing_Validating(null, null);
                }
                else if (FocusedControl == txtStoreIDAdditions.Name)
                {
                    txtStoreIDAdditions.Text = cls.PrimaryKeyValue.ToString();
                    txtStoreIDAdditions_Validating(null, null);
                }
                else if (FocusedControl == txtStoreIDBarcod.Name)
                {
                    txtStoreIDBarcod.Text = cls.PrimaryKeyValue.ToString();
                    txtStoreIDProducts_Validating(null, null);
                }


                //رقم العامل
                else if (FocusedControl ==  txtEmpIDFactor.Name)
                    {
                        txtEmpIDFactor.Text = cls.PrimaryKeyValue.ToString();
                        txtEmpFactorID_Validating(null, null);
                    }
                else if (FocusedControl == txtEmplooyID.Name)
                {
                    txtEmplooyID.Text = cls.PrimaryKeyValue.ToString();
                    txtEmplooyBrntageID_Validating(null, null);
                }
                else if (FocusedControl == txtEmplooyIDPolishing.Name)
                {
                    txtEmplooyIDPolishing.Text = cls.PrimaryKeyValue.ToString();
                    txtEmplooyPolishingID_Validating(null, null);
                }
                else if (FocusedControl == txtEmpIDPrentage.Name)
                {
                    txtEmpIDPrentage.Text = cls.PrimaryKeyValue.ToString();
                    txtEmpIDPrentage_Validating(null, null);
                }
                else if (FocusedControl == txtEmpIDBeforCompond.Name)
                {
                    txtEmpIDBeforCompond.Text = cls.PrimaryKeyValue.ToString();
                    txtEmpIDBeforCompond_Validating(null, null);
                }
                 
                else if (FocusedControl == txtEmpIDAdditions.Name)
                {
                    txtEmpIDAdditions.Text = cls.PrimaryKeyValue.ToString();
                    txtEmpIDAdditions_Validating(null, null);
                }

                //رقم الحساب
                else if (FocusedControl == txtAccountIDPrentage.Name)
                {
                    txtAccountIDPrentage.Text = cls.PrimaryKeyValue.ToString();
                    txtAccountIDPrentage_Validating(null, null);
                }
                else if (FocusedControl == txtAccountIDFactory.Name)
                {
                    txtAccountIDFactory.Text = cls.PrimaryKeyValue.ToString();
                    txtAccountIDFactory_Validating(null, null);
                }
                else if (FocusedControl == txtAccountIDBeforCompond.Name)
                {
                    txtAccountIDBeforCompond.Text = cls.PrimaryKeyValue.ToString();
                    txtAccountIDBeforCompond_Validating(null, null);
                }
                 
                else if (FocusedControl == txtAccountIDPolishing.Name)
                {
                    txtAccountIDPolishing.Text = cls.PrimaryKeyValue.ToString();
                    txtAccountIDPolishing_Validating(null, null);
                }
                else if (FocusedControl == txtAccountIDAdditions.Name)
                {
                    txtAccountIDAdditions.Text = cls.PrimaryKeyValue.ToString();
                    txtAccountIDAdditions_Validating(null, null);
                }
                else if (FocusedControl == txtAccountIDBarcodeItem.Name)
                {
                    txtAccountIDBarcodeItem.Text = cls.PrimaryKeyValue.ToString();
                    txtAccountIDBarcodeItem_Validating(null, null);
                }

                //امين الخزنة
                else if (FocusedControl == txtEmployeeStokIDFactory.Name)
                {
                    txtEmployeeStokIDFactory.Text = cls.PrimaryKeyValue.ToString();
                    txtEmployeeStokID_Validating(null, null);
                }
                else if (FocusedControl == txtEmployeeStokIDPrentage.Name)
                {
                    txtEmployeeStokIDPrentage.Text = cls.PrimaryKeyValue.ToString();
                    txtEmployeeStokIDPrentage_Validating(null, null);
                }
                else if (FocusedControl == txtEmployeeStokIDBeforCompond.Name)
                {
                    txtEmployeeStokIDBeforCompond.Text = cls.PrimaryKeyValue.ToString();
                    txtEmployeeStokIDBeforCompond_Validating(null, null);
                }
                 
                else if (FocusedControl == txtEmployeeStokIDAdditions.Name)
                {
                    txtEmployeeStokIDAdditions.Text = cls.PrimaryKeyValue.ToString();
                    txtEmployeeStokIDAdditions_Validating(null, null);
                }
                else if (FocusedControl == txtEmployeeStokIDPolishing.Name)
                {
                    txtEmployeeStokIDPolishing.Text = cls.PrimaryKeyValue.ToString();
                    txtEmployeeStokIDPolishing_Validating(null, null);
                }
                else if (FocusedControl == txtEmployeeStokIDBarcode.Name)
                {
                    txtEmployeeStokIDBarcode.Text = cls.PrimaryKeyValue.ToString();
                    txtEmployeeStokIDBarcode_Validating(null, null);
                }


                //المندوب والعميل
                else if (FocusedControl == txtDelegateID.Name)
                {
                    txtDelegateID.Text = cls.PrimaryKeyValue.ToString();
                    txtDelegateID_Validating(null, null);
                }
                else if (FocusedControl == txtCustomerID.Name)
                {
                    txtCustomerID.Text = cls.PrimaryKeyValue.ToString();
                    txtCustomerID_Validating(null, null);
                }
                
                //الجرايد فيو
                else if (FocusedControl.Trim() == gridControlBeforCompond.Name)
                {
                   
                    if (gridViewBeforCompond.FocusedColumn.Name == "BarcodCompond")
                    {
                        gridViewBeforCompond.AddNewRow();

                        DataTable dtGroupID = Lip.SelectRecord("Select ArbItemName,ArbItemType,TypeID,CostPrice from Stc_Items_Find where BarCode='" + cls.PrimaryKeyValue.ToString() + "'");
                        if (dtGroupID.Rows.Count > 0)
                        {
                            gridViewBeforCompond.SetRowCellValue(gridViewBeforCompond.FocusedRowHandle, gridViewBeforCompond.Columns["BarcodCompond"], cls.PrimaryKeyValue);
                            //gridViewBeforCompond.SetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "TypeSton", dtGroupID.Rows[0]["ArbItemType"]);
                            gridViewBeforCompond.SetRowCellValue(gridViewBeforCompond.FocusedRowHandle, ItemName, dtGroupID.Rows[0]["ArbItemName"]);
                            gridViewBeforCompond.SetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "TypeID",Comon.cInt(dtGroupID.Rows[0]["TypeID"]));
                            gridViewBeforCompond.SetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "CostPrice", dtGroupID.Rows[0]["CostPrice"]);
                        }                     
                    }
                    if (gridViewBeforCompond.FocusedColumn.Name == "colItemID")
                    {

                        DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_ItemUnits  Where ItemID=" + Comon.cInt(cls.PrimaryKeyValue));
                        if (dtItemID.Rows.Count > 0)
                        {
                            gridViewBeforCompond.AddNewRow();
                            FillItemData(gridViewBeforCompond, gridControlBeforCompond, "BarcodCompond", "GoldDebit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", txtAccountIDBeforCompond);
                        }
                    }

                    else if (gridViewBeforCompond.FocusedColumn.Name == "EmpCompondID")
                    {
                        gridViewBeforCompond.SetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "EmpCompondID", cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        gridViewBeforCompond.SetRowCellValue(gridViewBeforCompond.FocusedRowHandle, gridViewBeforCompond.Columns["EmpCompundName"], Lip.GetValue(strSQL));
                    }
                    else if (gridViewBeforCompond.FocusedColumn.Name == "FromAccountID")
                    {
                        gridViewBeforCompond.SetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "FromAccountID", cls.PrimaryKeyValue.ToString());
                        DataTable dtt = new Acc_AccountsDAL().GetAcc_AccountsByLevel(MySession.GlobalBranchID, MySession.GlobalFacilityID);
                        DataRow[] row = dtt.Select("AccountID=" + cls.PrimaryKeyValue.ToString());
                        if (row.Length > 0)
                        {
                            gridViewBeforCompond.SetRowCellValue(gridViewBeforCompond.FocusedRowHandle, "FromAccountName", row[0]["ArbName"].ToString());
                        }
                    }
                }
                else if (FocusedControl.Trim() == gridControlAfterCompond.Name)
                {

                    if (gridViewAfterCompond.FocusedColumn.Name == "BarcodCompond")
                    {
                        gridViewAfterCompond.AddNewRow();
                        DataTable dtGroupID = Lip.SelectRecord("Select ArbItemName,ArbItemType,TypeID,CostPrice from Stc_Items_Find where BarCode='" + cls.PrimaryKeyValue.ToString() + "'");
                        if (dtGroupID.Rows.Count > 0)
                        {
                            gridViewAfterCompond.SetRowCellValue(gridViewAfterCompond.FocusedRowHandle, gridViewAfterCompond.Columns["BarcodCompond"], cls.PrimaryKeyValue);
                            //gridViewAfterCompond.SetRowCellValue(gridViewAfterCompond.FocusedRowHandle, "TypeSton", dtGroupID.Rows[0]["ArbItemType"]);
                            gridViewAfterCompond.SetRowCellValue(gridViewAfterCompond.FocusedRowHandle, ItemName, dtGroupID.Rows[0]["ArbItemName"]);
                            gridViewAfterCompond.SetRowCellValue(gridViewAfterCompond.FocusedRowHandle, "TypeID", dtGroupID.Rows[0]["TypeID"]);
                            gridViewAfterCompond.SetRowCellValue(gridViewAfterCompond.FocusedRowHandle, "CostPrice", dtGroupID.Rows[0]["CostPrice"]);
                        }
                    }
                    if (gridViewAfterCompond.FocusedColumn.Name == "colItemID")
                    {

                        DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_ItemUnits  Where ItemID=" + Comon.cInt(cls.PrimaryKeyValue));
                        if (dtItemID.Rows.Count > 0)
                        {
                            gridViewAfterCompond.AddNewRow();
                            FillItemData(gridViewAfterCompond, gridControlAfterCompond, "BarcodCompond", "GoldCredit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", txtAccountIDBeforCompond);
                        }
                    }
                    else if (gridViewAfterCompond.FocusedColumn.Name == "EmpCompondID")
                    {
                        gridViewAfterCompond.SetFocusedRowCellValue("EmpCompondID", cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        gridViewAfterCompond.SetRowCellValue(gridViewAfterCompond.FocusedRowHandle, gridViewAfterCompond.Columns["EmpCompundName"], Lip.GetValue(strSQL));
                    }
                    else if (gridViewAfterCompond.FocusedColumn.Name == "FromAccountID")
                    {
                        gridViewAfterCompond.SetRowCellValue(gridViewAfterCompond.FocusedRowHandle, "FromAccountID", cls.PrimaryKeyValue.ToString());
                        DataTable dtt = new Acc_AccountsDAL().GetAcc_AccountsByLevel(MySession.GlobalBranchID, MySession.GlobalFacilityID);
                        DataRow[] row = dtt.Select("AccountID=" + cls.PrimaryKeyValue.ToString());

                        if (row.Length > 0)
                        {
                            gridViewAfterCompond.SetRowCellValue(gridViewAfterCompond.FocusedRowHandle, "FromAccountName", row[0]["ArbName"].ToString());
                        }
                    }
                }
                else if (FocusedControl.Trim() == gridControlfactroOpretion.Name)
                {
                    if (GridViewBeforfactory.FocusedColumn.Name == "colStoreID")
                    {
                        GridViewBeforfactory.SetRowCellValue(GridViewBeforfactory.FocusedRowHandle, GridViewBeforfactory.Columns["StoreID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        GridViewBeforfactory.SetRowCellValue(GridViewBeforfactory.FocusedRowHandle, GridViewBeforfactory.Columns["StoreName"], Lip.GetValue(strSQL));
                    }
                    if (GridViewBeforfactory.FocusedColumn.Name == "colItemID")
                    { 
                        DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_ItemUnits  Where ItemID=" + Comon.cInt(cls.PrimaryKeyValue));
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridViewBeforfactory.AddNewRow();
                            FillItemData(GridViewBeforfactory, gridControlfactroOpretion, "BarCode", "Debit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", txtAccountIDFactory);
                        }
                    }
                    if (GridViewBeforfactory.FocusedColumn.Name == "MachinID")
                    {
                        GridViewBeforfactory.AddNewRow();
                        FileDataMachinName(GridViewBeforfactory, "DebitDate", "DebitTime", Comon.cInt(cls.PrimaryKeyValue.ToString()));
                    }
                    if (GridViewBeforfactory.FocusedColumn.Name == "colSizeID")
                    {
                        GridViewBeforfactory.SetRowCellValue(GridViewBeforfactory.FocusedRowHandle, GridViewBeforfactory.Columns["SizeID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as " + SizeName + " FROM Stc_SizingUnits WHERE SizeID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        GridViewBeforfactory.SetRowCellValue(GridViewBeforfactory.FocusedRowHandle, GridViewBeforfactory.Columns[SizeName], Lip.GetValue(strSQL));

                    }
                    if (GridViewBeforfactory.FocusedColumn.Name == "colEmpID")
                    {
                        GridViewBeforfactory.SetRowCellValue(GridViewBeforfactory.FocusedRowHandle, GridViewBeforfactory.Columns["EmpID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        GridViewBeforfactory.SetRowCellValue(GridViewBeforfactory.FocusedRowHandle, GridViewBeforfactory.Columns["EmpName"], Lip.GetValue(strSQL));
                    }
                }
                else if (FocusedControl.Trim() == gridControlAfterFactory.Name)
                {
                    if (GridViewAfterfactory.FocusedColumn.Name == "colStoreID")
                    {
                        GridViewAfterfactory.SetRowCellValue(GridViewAfterfactory.FocusedRowHandle, GridViewAfterfactory.Columns["StoreID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        GridViewAfterfactory.SetRowCellValue(GridViewAfterfactory.FocusedRowHandle, GridViewAfterfactory.Columns["StoreName"], Lip.GetValue(strSQL));

                    }

                    if (GridViewAfterfactory.FocusedColumn.Name == "colItemID")
                    {
                         
                        DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_ItemUnits  Where ItemID=" + Comon.cInt(cls.PrimaryKeyValue));
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridViewAfterfactory.AddNewRow();
                            FillItemData(GridViewAfterfactory, gridControlAfterFactory, "BarCode", "Credit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", txtAccountIDFactory);
                        }
                    }
                    if (GridViewAfterfactory.FocusedColumn.Name == "MachinID")
                    {
                        GridViewAfterfactory.AddNewRow();
                        FileDataMachinName(GridViewAfterfactory, "DebitDate", "DebitTime", Comon.cInt(cls.PrimaryKeyValue.ToString()));
                    }
                    if (GridViewAfterfactory.FocusedColumn.Name == "colSizeID")
                    {
                        GridViewAfterfactory.SetRowCellValue(GridViewAfterfactory.FocusedRowHandle, GridViewAfterfactory.Columns["SizeID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as " + SizeName + " FROM Stc_SizingUnits WHERE SizeID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        GridViewAfterfactory.SetRowCellValue(GridViewAfterfactory.FocusedRowHandle, GridViewAfterfactory.Columns[SizeName], Lip.GetValue(strSQL));

                    }
                    if (GridViewAfterfactory.FocusedColumn.Name == "colEmpID")
                    {
                        GridViewAfterfactory.SetRowCellValue(GridViewAfterfactory.FocusedRowHandle, GridViewAfterfactory.Columns["EmpID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        GridViewAfterfactory.SetRowCellValue(GridViewAfterfactory.FocusedRowHandle, GridViewAfterfactory.Columns["EmpName"], Lip.GetValue(strSQL));
                    }
                }
                else if (FocusedControl.Trim() == gridControlBeforPrentag.Name)
                {
                    if (GridViewBeforPrentag.FocusedColumn.Name == "colStoreID")
                    {
                        GridViewBeforPrentag.SetRowCellValue(GridViewBeforPrentag.FocusedRowHandle, GridViewBeforPrentag.Columns["StoreID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        GridViewBeforPrentag.SetRowCellValue(GridViewBeforPrentag.FocusedRowHandle, GridViewBeforPrentag.Columns["StoreName"], Lip.GetValue(strSQL));                       
                    }
                    if (GridViewBeforPrentag.FocusedColumn.Name == "colItemID")
                    {      
                        DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_ItemUnits  Where ItemID=" + Comon.cInt(cls.PrimaryKeyValue));
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridViewBeforPrentag.AddNewRow();
                            FillItemData(GridViewBeforPrentag, gridControlBeforPrentag, "BarcodePrentag", "PrentagDebit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "PrentagDebitDate", "PrentagDebitTime", txtAccountIDPrentage);
                        }
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
                        strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        GridViewBeforPrentag.SetRowCellValue(GridViewBeforPrentag.FocusedRowHandle, GridViewBeforPrentag.Columns["EmpName"], Lip.GetValue(strSQL));
                    }
                }

                else if (FocusedControl.Trim() == gridControlAfterPrentage.Name)
                {
                    if (GridViewAfterPrentag.FocusedColumn.Name == "colStoreID")
                    {
                        GridViewAfterPrentag.SetRowCellValue(GridViewAfterPrentag.FocusedRowHandle, GridViewAfterPrentag.Columns["StoreID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        GridViewAfterPrentag.SetRowCellValue(GridViewAfterPrentag.FocusedRowHandle, GridViewAfterPrentag.Columns["StoreName"], Lip.GetValue(strSQL));

                    }
                    if (GridViewAfterPrentag.FocusedColumn.Name == "colItemID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_ItemUnits  Where ItemID=" + Comon.cInt(cls.PrimaryKeyValue));
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridViewAfterPrentag.AddNewRow();
                            FillItemData(GridViewAfterPrentag, gridControlAfterPrentage, "BarcodePrentag", "PrentagCredit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "PrentagDebitDate", "PrentagDebitTime", txtAccountIDPrentage);
                        }                                
                    }
                    if (GridViewAfterPrentag.FocusedColumn.Name == "MachinID")
                    {
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
                        strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        GridViewAfterPrentag.SetRowCellValue(GridViewAfterPrentag.FocusedRowHandle, GridViewAfterPrentag.Columns["EmpName"], Lip.GetValue(strSQL));
                    }
                }
                else if (FocusedControl.Trim() == gridControlBeforePolishing.Name)
                {
                    if (GridViewBeforPolish.FocusedColumn.Name == "colStoreID")
                    {
                        GridViewBeforPolish.SetRowCellValue(GridViewBeforPolish.FocusedRowHandle, GridViewBeforPolish.Columns["StoreID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        GridViewBeforPolish.SetRowCellValue(GridViewBeforPolish.FocusedRowHandle, GridViewBeforPolish.Columns["StoreName"], Lip.GetValue(strSQL));
                    }
                    if (GridViewBeforPolish.FocusedColumn.Name == "colItemID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_ItemUnits  Where ItemID=" + Comon.cInt(cls.PrimaryKeyValue));
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridViewBeforPolish.AddNewRow();
                            FillItemData(GridViewBeforPolish, gridControlBeforePolishing, "BarcodeTalmee", "Debit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", txtAccountIDPolishing);
                        }
                    }
                    if (GridViewBeforPolish.FocusedColumn.Name == "MachinID")
                    {
                        GridViewBeforPolish.AddNewRow();                  
                        FileDataMachinName(GridViewBeforPolish, "DebitDate", "DebitTime", Comon.cInt(cls.PrimaryKeyValue.ToString()));  
                    }
                    if (GridViewBeforPolish.FocusedColumn.Name == "colSizeID")
                    {
                        GridViewBeforPolish.SetRowCellValue(GridViewBeforPolish.FocusedRowHandle, GridViewBeforPolish.Columns["SizeID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as " + SizeName + " FROM Stc_SizingUnits WHERE SizeID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        GridViewBeforPolish.SetRowCellValue(GridViewBeforPolish.FocusedRowHandle, GridViewBeforPolish.Columns[SizeName], Lip.GetValue(strSQL));

                    }
                    if (GridViewBeforPolish.FocusedColumn.Name == "colEmpID")
                    {
                        GridViewBeforPolish.SetRowCellValue(GridViewBeforPolish.FocusedRowHandle, GridViewBeforPolish.Columns["EmpID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        GridViewBeforPolish.SetRowCellValue(GridViewBeforPolish.FocusedRowHandle, GridViewBeforPolish.Columns["EmpName"], Lip.GetValue(strSQL));
                    }
                }
                else if (FocusedControl.Trim() ==gridControlAfterPolishing.Name)
                {
                    if (GridViewAfterPolish.FocusedColumn.Name == "colStoreID")
                    {
                        GridViewAfterPolish.SetRowCellValue(GridViewAfterPolish.FocusedRowHandle, GridViewAfterPolish.Columns["StoreID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        GridViewAfterPolish.SetRowCellValue(GridViewAfterPolish.FocusedRowHandle, GridViewAfterPolish.Columns["StoreName"], Lip.GetValue(strSQL));

                    }
                    if (GridViewAfterPolish.FocusedColumn.Name == "colItemID")
                    {                     
                        DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_ItemUnits  Where ItemID=" + Comon.cInt(cls.PrimaryKeyValue));
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridViewAfterPolish.AddNewRow();
                            FillItemData(GridViewAfterPolish, gridControlAfterPolishing, "BarcodeTalmee", "Credit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", txtAccountIDPolishing);
                        }
                    }
                    if (GridViewAfterPolish.FocusedColumn.Name == "MachinID")
                    {                     
                        GridViewAfterPolish.AddNewRow();
                        FileDataMachinName(GridViewAfterPolish, "DebitDate", "DebitTime", Comon.cInt(cls.PrimaryKeyValue.ToString()));  

                    }
                    if (GridViewAfterPolish.FocusedColumn.Name == "colSizeID")
                    {
                        GridViewAfterPolish.SetRowCellValue(GridViewAfterPolish.FocusedRowHandle, GridViewAfterPolish.Columns["SizeID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as " + SizeName + " FROM Stc_SizingUnits WHERE SizeID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        GridViewAfterPolish.SetRowCellValue(GridViewAfterPolish.FocusedRowHandle, GridViewAfterPolish.Columns[SizeName], Lip.GetValue(strSQL));

                    }
                    if (GridViewAfterPolish.FocusedColumn.Name == "colEmpID")
                    {
                        GridViewAfterPolish.SetRowCellValue(GridViewAfterPolish.FocusedRowHandle, GridViewAfterPolish.Columns["EmpID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        GridViewAfterPolish.SetRowCellValue(GridViewAfterPolish.FocusedRowHandle, GridViewAfterPolish.Columns["EmpName"], Lip.GetValue(strSQL));
                    }
                }
                else if (FocusedControl.Trim() == gridControlAdditional.Name)
                {
                    if (gridViewAdditional.FocusedColumn.Name == "colStoreID")
                    {
                        gridViewAdditional.SetRowCellValue(gridViewAdditional.FocusedRowHandle, gridViewAdditional.Columns["StoreID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        gridViewAdditional.SetRowCellValue(gridViewAdditional.FocusedRowHandle, gridViewAdditional.Columns["StoreName"], Lip.GetValue(strSQL));

                    }
                    if (gridViewAdditional.FocusedColumn.Name == "colItemID")
                    {
                         DataTable dtItemID = Lip.SelectRecord("Select  BarCode from Stc_ItemUnits  Where ItemID=" + Comon.cInt(cls.PrimaryKeyValue));
                         if (dtItemID.Rows.Count > 0)
                         {
                             GridViewAfterPolish.AddNewRow();
                             FillItemData(gridViewAdditional, gridControlAdditional, "BarcodSelver", "Credit", Stc_itemsDAL.GetItemData1(dtItemID.Rows[0][0].ToString(), UserInfo.FacilityID), "DebitDate", "DebitTime", txtAccountIDAdditions);
                         }
                    }
                    if (gridViewAdditional.FocusedColumn.Name == "MachinID")
                    {
                     
                        GridViewAfterPolish.AddNewRow();
                        FileDataMachinName(gridViewAdditional, "DebitDate", "DebitTime", Comon.cInt(cls.PrimaryKeyValue.ToString()));  
                    }
                    if (gridViewAdditional.FocusedColumn.Name == "BarcodSelver")
                    {
                        gridViewAdditional.AddNewRow();
                        gridViewAdditional.SetRowCellValue(gridViewAdditional.FocusedRowHandle, gridViewAdditional.Columns["BarcodSelver"], cls.PrimaryKeyValue.ToString());
                      
                     }
                    if (gridViewAdditional.FocusedColumn.Name == "colSizeID")
                    {
                        gridViewAdditional.SetRowCellValue(gridViewAdditional.FocusedRowHandle, gridViewAdditional.Columns["SizeID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as " + SizeName + " FROM Stc_SizingUnits WHERE SizeID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        gridViewAdditional.SetRowCellValue(gridViewAdditional.FocusedRowHandle, gridViewAdditional.Columns[SizeName], Lip.GetValue(strSQL));

                    }
                    if (gridViewAdditional.FocusedColumn.Name == "colEmpID")
                    {
                        gridViewAdditional.SetRowCellValue(gridViewAdditional.FocusedRowHandle, gridViewAdditional.Columns["EmpID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        gridViewAdditional.SetRowCellValue(gridViewAdditional.FocusedRowHandle, gridViewAdditional.Columns["EmpName"], Lip.GetValue(strSQL));
                    }
                }
                else if (FocusedControl.Trim() == gridControlProductionExpenses.Name)
                {
                    if (GridProductionExpenses.FocusedColumn.Name == "colAccountID")
                    {
                        GridProductionExpenses.AddNewRow();
                        GridProductionExpenses.SetRowCellValue(GridProductionExpenses.FocusedRowHandle, "AccountID", cls.PrimaryKeyValue.ToString());
                        DataTable dtt = new Acc_AccountsDAL().GetAcc_AccountsByLevel(MySession.GlobalBranchID, MySession.GlobalFacilityID);
                        DataRow[] row = dtt.Select("AccountID=" + cls.PrimaryKeyValue.ToString());
                        if (row.Length > 0)
                        {
                            GridProductionExpenses.SetRowCellValue(GridProductionExpenses.FocusedRowHandle, "AccountName", row[0]["ArbName"].ToString());
                        }
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
            EnableGridView(GridViewBeforPrentag, Value,1);
            EnableGridView(GridViewAfterPrentag, Value, 1);
            EnableGridView(gridViewBeforCompond, Value,0);
            EnableGridView(GridViewBeforPolish, Value,1);
            EnableGridView(GridViewAfterPolish, Value,1);
            EnableGridView(gridViewAdditional, Value,0);
            EnableGridView(gridViewAfterCompond, Value,0);
            EnableGridView(GridViewBeforfactory, Value,1);
            EnableGridView(GridViewAfterfactory, Value,1);
        }
        
        void EnableGridView( GridView GridViewObj, bool Value, int flage)
        {
            foreach (GridColumn col in GridViewObj.Columns)
            {

                GridViewObj.Columns[col.FieldName].OptionsColumn.AllowEdit = Value;
                GridViewObj.Columns[col.FieldName].OptionsColumn.AllowFocus = Value;
                GridViewObj.Columns[col.FieldName].OptionsColumn.ReadOnly = !Value;
                
            }
            if (flage == 1)
            {
                GridViewObj.Columns["MachinID"].OptionsColumn.AllowFocus = false;
                GridViewObj.Columns["MachinID"].OptionsColumn.AllowEdit = false;
                GridViewObj.Columns["MachineName"].OptionsColumn.AllowFocus = false;
                GridViewObj.Columns["MachineName"].OptionsColumn.AllowEdit = false;
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
                    strSQL = "SELECT TOP 1 * FROM " + Menu_FactoryRunCommandMasterDAL.TableName + " Where    Cancel =0 ";
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
               txtReferanceID.Text= txtCommandID.Text = Menu_FactoryRunCommandMasterDAL.GetNewID(1,1).ToString();
                InitializeFormatDate(txtCommandDate);
                InitializeFormatDate(txtGivenDate);               
                ClearFields();
                txtCustomerID.Focus();
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
             for (int i = 0; i <= GridViewBeforfactory.DataRowCount - 1; i++)
             {
                 var row = dataTable.NewRow();
                 row["#"] = i + 1;
                 row["MachinID"] = GridViewBeforfactory.GetRowCellValue(i, "MachinID");
                 row["MachineName"] = GridViewBeforfactory.GetRowCellValue(i, "MachineName");
                 row["QTY"] = GridViewBeforfactory.GetRowCellValue(i, "Debit");

                 row["StoreName"] = GridViewBeforfactory.GetRowCellValue(i, "StoreName");

                 row["ItemID"] = GridViewBeforfactory.GetRowCellValue(i, "ItemID");
                 row["ItemName"] = GridViewBeforfactory.GetRowCellValue(i, ItemName);
                 row["SizeName"] = GridViewBeforfactory.GetRowCellValue(i,SizeName);
                 row["Date"] = GridViewBeforfactory.GetRowCellValue(i, "DebitDate");
                 row["Time"] = GridViewBeforfactory.GetRowCellValue(i, "DebitTime");
                 row["EmpName"] = GridViewBeforfactory.GetRowCellValue(i, "EmpName");
               
                 dataTable.Rows.Add(row);
             }
             rptFactoryFactor.DataSource = dataTable;
             rptFactoryFactor.DataMember = "rptManu_FactoryFactorCommendBefore";
             return rptFactoryFactor;
         }
           public XtraReport Manu_FactoryBrntageBefor()
           {
               string rptrptManu_FactoryFactorCommendName = "rptManu_FactoryBrntageCommendBefore";
               string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\Reports\";
               //rptCompanyHeaderName += (UserInfo.Language == iLanguage.English ? "Eng" : "Arb");
               rptrptManu_FactoryFactorCommendName += "Arb";
               XtraReport rptFactoryFactor = XtraReport.FromFile(Path + rptrptManu_FactoryFactorCommendName + ".repx", true);


               var dataTable = new dsReports.rptManu_FactoryFactorCommendBeforeDataTable();
               for (int i = 0; i <= GridViewBeforPrentag.DataRowCount - 1; i++)
               {
                   var row = dataTable.NewRow();
                   row["#"] = i + 1;
                   row["MachinID"] = GridViewBeforPrentag.GetRowCellValue(i, "MachinID");
                   row["MachineName"] = GridViewBeforPrentag.GetRowCellValue(i, "MachineName");
                   row["QTY"] = GridViewBeforPrentag.GetRowCellValue(i, "PrentagDebit");

                   row["StoreName"] = GridViewBeforPrentag.GetRowCellValue(i, "StoreName");

                   row["ItemID"] = GridViewBeforPrentag.GetRowCellValue(i, "ItemID");
                   row["ItemName"] = GridViewBeforPrentag.GetRowCellValue(i, ItemName);
                   row["SizeName"] = GridViewBeforPrentag.GetRowCellValue(i, SizeName);
                   row["Date"] = GridViewBeforPrentag.GetRowCellValue(i, "PrentagDebitDate");
                   row["Time"] = GridViewBeforPrentag.GetRowCellValue(i, "PrentagDebitTime");
                   row["EmpName"] = GridViewBeforPrentag.GetRowCellValue(i, "EmpName");
                   dataTable.Rows.Add(row);
               }
               rptFactoryFactor.DataSource = dataTable;
               rptFactoryFactor.DataMember = "rptManu_FactoryBrntageCommendBefore";
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
             for (int i = 0; i <= GridViewAfterfactory.DataRowCount - 1; i++)
             {
                 var row = dataTable.NewRow();
                 row["#"] = i + 1;
                 row["MachinID"] = GridViewAfterfactory.GetRowCellValue(i, "MachinID");
                 row["MachineName"] = GridViewAfterfactory.GetRowCellValue(i, "MachineName");
                 row["QTY"] = GridViewAfterfactory.GetRowCellValue(i, "Credit");

                 row["StoreName"] = GridViewAfterfactory.GetRowCellValue(i, "StoreName");

                 row["ItemID"] = GridViewAfterfactory.GetRowCellValue(i, "ItemID");
                 row["ItemName"] = GridViewAfterfactory.GetRowCellValue(i, ItemName);
                 row["SizeName"] = GridViewAfterfactory.GetRowCellValue(i, SizeName);
                 row["Date"] = GridViewAfterfactory.GetRowCellValue(i, "DebitDate");
                 row["Time"] = GridViewAfterfactory.GetRowCellValue(i, "DebitTime");
                 row["EmpName"] = GridViewAfterfactory.GetRowCellValue(i, "EmpName");



                 dataTable.Rows.Add(row);
             }
             rptFactoryFactor.DataSource = dataTable;
             rptFactoryFactor.DataMember = "rptManu_FactoryFactorCommendAfter";
             return rptFactoryFactor;
         }
           public XtraReport Manu_FactoryBrntageAfter()
           {
               string rptrptManu_FactoryFactorCommendName = "rptManu_FactoryBrntageCommendAfter";
               string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\Reports\";
               //rptCompanyHeaderName += (UserInfo.Language == iLanguage.English ? "Eng" : "Arb");
               rptrptManu_FactoryFactorCommendName += "Arb";
               XtraReport rptFactoryFactor = XtraReport.FromFile(Path + rptrptManu_FactoryFactorCommendName + ".repx", true);


               var dataTable = new dsReports.rptManu_FactoryFactorCommendBeforeDataTable();
               for (int i = 0; i <= GridViewAfterPrentag.DataRowCount - 1; i++)
               {
                   var row = dataTable.NewRow();
                   row["#"] = i + 1;
                   row["MachinID"] = GridViewAfterPrentag.GetRowCellValue(i, "MachinID");
                   row["MachineName"] = GridViewAfterPrentag.GetRowCellValue(i, "MachineName");
                   row["QTY"] = GridViewAfterPrentag.GetRowCellValue(i, "PrentagCredit");

                   row["StoreName"] = GridViewAfterPrentag.GetRowCellValue(i, "StoreName");

                   row["ItemID"] = GridViewAfterPrentag.GetRowCellValue(i, "ItemID");
                   row["ItemName"] = GridViewAfterPrentag.GetRowCellValue(i, ItemName);
                   row["SizeName"] = GridViewAfterPrentag.GetRowCellValue(i, SizeName);
                   row["Date"] = GridViewAfterPrentag.GetRowCellValue(i, "PrentagDebitDate");
                   row["Time"] = GridViewAfterPrentag.GetRowCellValue(i, "PrentagDebitTime");
                   row["EmpName"] = GridViewAfterPrentag.GetRowCellValue(i, "EmpName");
                   dataTable.Rows.Add(row);
               }
               rptFactoryFactor.DataSource = dataTable;
               rptFactoryFactor.DataMember = "rptManu_FactoryBrntageCommendAfter";
               return rptFactoryFactor;
           }
           public XtraReport Manu_FactoryTalmeeBefor()
           {
               string rptrptManu_FactoryFactorCommendName = "rptManu_FactoryTalmeeCommendBefore";
               string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\Reports\";
               //rptCompanyHeaderName += (UserInfo.Language == iLanguage.English ? "Eng" : "Arb");
               rptrptManu_FactoryFactorCommendName += "Arb";
               XtraReport rptFactoryFactor = XtraReport.FromFile(Path + rptrptManu_FactoryFactorCommendName + ".repx", true);


               var dataTable = new dsReports.rptManu_FactoryFactorCommendBeforeDataTable();
               for (int i = 0; i <= GridViewBeforPolish.DataRowCount - 1; i++)
               {
                   var row = dataTable.NewRow();
                   row["#"] = i + 1;
                   row["MachinID"] = GridViewBeforPolish.GetRowCellValue(i, "MachinID");
                   row["MachineName"] = GridViewBeforPolish.GetRowCellValue(i, "MachineName");
                   row["QTY"] = GridViewBeforPolish.GetRowCellValue(i, "Debit");

                   row["StoreName"] = GridViewBeforPolish.GetRowCellValue(i, "StoreName");

                   row["ItemID"] = GridViewBeforPolish.GetRowCellValue(i, "ItemID");
                   row["ItemName"] = GridViewBeforPolish.GetRowCellValue(i, ItemName);
                   row["SizeName"] = GridViewBeforPolish.GetRowCellValue(i, SizeName);
                   row["Date"] = GridViewBeforPolish.GetRowCellValue(i, "DebitDate");
                   row["Time"] = GridViewBeforPolish.GetRowCellValue(i, "DebitTime");
                   row["EmpName"] = GridViewBeforPolish.GetRowCellValue(i, "EmpName");
                   dataTable.Rows.Add(row);
               }
               rptFactoryFactor.DataSource = dataTable;
               rptFactoryFactor.DataMember = "rptManu_FactoryTalmeeCommendBefore";
               return rptFactoryFactor;
           }
           public XtraReport Manu_FactoryTalmeeAfter()
           {
               string rptrptManu_FactoryFactorCommendName = "rptManu_FactoryTalmeeCommendAfter";
               string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\Reports\";
               //rptCompanyHeaderName += (UserInfo.Language == iLanguage.English ? "Eng" : "Arb");
               rptrptManu_FactoryFactorCommendName += "Arb";
               XtraReport rptFactoryFactor = XtraReport.FromFile(Path + rptrptManu_FactoryFactorCommendName + ".repx", true);
               

               var dataTable = new dsReports.rptManu_FactoryFactorCommendBeforeDataTable();
               for (int i = 0; i <= GridViewAfterPolish.DataRowCount - 1; i++)
               {
                   var row = dataTable.NewRow();
                   row["#"] = i + 1;
                   row["MachinID"] = GridViewAfterPolish.GetRowCellValue(i, "MachinID");
                   row["MachineName"] = GridViewAfterPolish.GetRowCellValue(i, "MachineName");
                   row["QTY"] = GridViewAfterPolish.GetRowCellValue(i, "Credit");

                   row["StoreName"] = GridViewAfterPolish.GetRowCellValue(i, "StoreName");
                   row["ItemID"] = GridViewAfterPolish.GetRowCellValue(i, "ItemID");
                   row["ItemName"] = GridViewAfterPolish.GetRowCellValue(i, ItemName);
                   row["SizeName"] = GridViewAfterPolish.GetRowCellValue(i, SizeName);
                   row["Date"] = GridViewAfterPolish.GetRowCellValue(i, "DebitDate");
                   row["Time"] = GridViewAfterPolish.GetRowCellValue(i, "DebitTime");
                   row["EmpName"] = GridViewAfterPolish.GetRowCellValue(i, "EmpName");
                   dataTable.Rows.Add(row);
               }
               rptFactoryFactor.DataSource = dataTable;
               rptFactoryFactor.DataMember = "rptManu_FactoryTalmeeCommendAfter";
               return rptFactoryFactor;
           }
           public XtraReport Manu_FactoryAddtional()
           {
               string rptrptManu_FactoryFactorCommendName = "rptManu_FactoryAddtionalCommend";
               string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\Reports\";
               //rptCompanyHeaderName += (UserInfo.Language == iLanguage.English ? "Eng" : "Arb");
               rptrptManu_FactoryFactorCommendName += "Arb";
               XtraReport rptFactoryFactor = XtraReport.FromFile(Path + rptrptManu_FactoryFactorCommendName + ".repx", true);


               var dataTable = new dsReports.rptManu_FactoryFactorCommendBeforeDataTable();
               for (int i = 0; i <= gridViewAdditional.DataRowCount - 1; i++)
               {
                   var row = dataTable.NewRow();
                   row["#"] = i + 1;
                   row["MachinID"] = gridViewAdditional.GetRowCellValue(i, "MachinID");
                   row["MachineName"] = gridViewAdditional.GetRowCellValue(i, "MachineName");
                   row["QTY"] = gridViewAdditional.GetRowCellValue(i, "Debit");

                   row["StoreName"] = gridViewAdditional.GetRowCellValue(i, "StoreName");

                   row["ItemID"] = gridViewAdditional.GetRowCellValue(i, "ItemID");
                   row["ItemName"] = gridViewAdditional.GetRowCellValue(i, ItemName);
                   row["SizeName"] = gridViewAdditional.GetRowCellValue(i, SizeName);
                   row["Date"] = gridViewAdditional.GetRowCellValue(i, "DebitDate");
                   row["Time"] = gridViewAdditional.GetRowCellValue(i, "DebitTime");
                   row["EmpName"] = gridViewAdditional.GetRowCellValue(i, "EmpName");
                   dataTable.Rows.Add(row);
               }
               rptFactoryFactor.DataSource = dataTable;
               rptFactoryFactor.DataMember = "rptManu_FactoryAddtionalCommend";
               return rptFactoryFactor;
           }

           public XtraReport Manu_FactoryCompoundBefor()
           {
               string rptrptManu_FactoryFactorCommendName = "rptManu_FactoryCompoundCommendBefore";
               string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\Reports\";
               //rptCompanyHeaderName += (UserInfo.Language == iLanguage.English ? "Eng" : "Arb");
               rptrptManu_FactoryFactorCommendName += "Arb";
               XtraReport rptFactoryFactor = XtraReport.FromFile(Path + rptrptManu_FactoryFactorCommendName + ".repx", true);


               var dataTable = new dsReports.rptManu_FactoryCompoundCommendDataTable();
               for (int i = 0; i <= gridViewBeforCompond.DataRowCount - 1; i++)
               {
                   var row = dataTable.NewRow();
                   row["#"] = i + 1;
                   row["BarCode"] = gridViewBeforCompond.GetRowCellValue(i, "BarcodCompond");

                   row["TypeStone"] = gridViewBeforCompond.GetRowCellValue(i, "TypeSton");

                   row["ItemName"] = gridViewBeforCompond.GetRowCellValue(i, ItemName);
                   row["CostPrice"] = gridViewBeforCompond.GetRowCellValue(i, "CostPrice");
                   row["QTY"] = gridViewBeforCompond.GetRowCellValue(i, "GoldDebit");
                   row["QTYStone"] = gridViewBeforCompond.GetRowCellValue(i, "ComWeightStonin");
                   row["AccountName"] = gridViewBeforCompond.GetRowCellValue(i, "FromAccountName");
                   row["EmpCompundName"] = gridViewBeforCompond.GetRowCellValue(i, "EmpCompundName");
                   dataTable.Rows.Add(row);
               }
               rptFactoryFactor.DataSource = dataTable;
               rptFactoryFactor.DataMember = "rptManu_FactoryCompoundCommendBefore";
               return rptFactoryFactor;
           }

           public XtraReport Manu_FactoryCompoundAfter()
           {
               string rptrptManu_FactoryFactorCommendName = "rptManu_FactoryCompoundCommendAfter";
               string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\Reports\";
               //rptCompanyHeaderName += (UserInfo.Language == iLanguage.English ? "Eng" : "Arb");
               rptrptManu_FactoryFactorCommendName += "Arb";
               XtraReport rptFactoryFactor = XtraReport.FromFile(Path + rptrptManu_FactoryFactorCommendName + ".repx", true);
               var dataTable = new dsReports.rptManu_FactoryCompoundCommendDataTable();
               for (int i = 0; i <= gridViewAfterCompond.DataRowCount - 1; i++)
               {
                   var row = dataTable.NewRow();
                   row["#"] = i + 1;
                   row["BarCode"] = gridViewAfterCompond.GetRowCellValue(i, "BarcodCompond");
                   row["TypeStone"] = gridViewAfterCompond.GetRowCellValue(i, "TypeSton");
                   row["ItemName"] = gridViewAfterCompond.GetRowCellValue(i, ItemName);
                   row["CostPrice"] = gridViewAfterCompond.GetRowCellValue(i, "CostPrice");
                   row["QTY"] = gridViewAfterCompond.GetRowCellValue(i, "GoldCredit");
                   row["QTYStone"] = gridViewAfterCompond.GetRowCellValue(i, "ComWeightSton");
                   row["AccountName"] = gridViewAfterCompond.GetRowCellValue(i, "FromAccountName");
                   row["EmpCompundName"] = gridViewAfterCompond.GetRowCellValue(i, "EmpCompundName");
                   
                   dataTable.Rows.Add(row);
               }
               rptFactoryFactor.DataSource = dataTable;
               rptFactoryFactor.DataMember = "rptManu_FactoryCompoundCommendAfter";
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
                 ReportName = "rptManu_FactoryCommend";
                 bool IncludeHeader = true;
                 string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                 XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

                 /********************** Master *****************************/
                 rptForm.RequestParameters = false;
              
                 for (int i = 0; i < rptForm.Parameters.Count; i++)
                     rptForm.Parameters[i].Visible = false;

                 rptForm.Parameters["CommendID"].Value = txtCommandID.Text;
                 rptForm.Parameters["ReferanceID"].Value =txtReferanceID.Text;
                 rptForm.Parameters["CustomerName"].Value =lblCustomerName.Text;
                 rptForm.Parameters["DelegetName"].Value =lblDelegateName.Text;
                 rptForm.Parameters["ItemID"].Value =txtItemID.Text;
                 rptForm.Parameters["ItemName"].Value =txtItemName.Text;
                 rptForm.Parameters["TypeID"].Value =txtTypeID.Text;
                 rptForm.Parameters["TypeName"].Value =txtTypeName.Text;
                 rptForm.Parameters["GroupID"].Value =txtGroupID.Text;
                 rptForm.Parameters["GroupName"].Value =txtGroupName.Text;
                 rptForm.Parameters["BrandID"].Value = txtBrandID.Text;
                 rptForm.Parameters["BrandName"].Value = txtBrandName.Text;
                 rptForm.Parameters["BarCode"].Value =txtBarCode.Text;
                 GridViewCodeing.AddNewRow();
                 rptForm.Parameters["Gold"].Value = Comon.cDec(GridViewCodeing.GetRowCellValue(0, "MinLimitQty")).ToString();
                 rptForm.Parameters["Daimond"].Value = Comon.cDec(GridViewCodeing.GetRowCellValue(0, "DIAMOND_W"));
                 rptForm.Parameters["Stone"].Value = Comon.cDec(GridViewCodeing.GetRowCellValue(0, "STONE_W"));
                 rptForm.Parameters["Zircone"].Value = Comon.cDec(GridViewCodeing.GetRowCellValue(0, "ZIRCON_W"));
                 rptForm.Parameters["BAGET"].Value = Comon.cDec(GridViewCodeing.GetRowCellValue(0, "BAGET_W"));
                 /********************** Details ****************************/
              
                 rptForm.DataMember = ReportName;
                 /******************** Report Binding ************************/
                 XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                 subreport.Visible = IncludeHeader;
                 subreport.ReportSource = ReportComponent.CompanyHeader();
                 


                 /******************** Report Factory ************************/
                 XRSubreport subreportFactor = (XRSubreport)rptForm.FindControl("subRptrptManu_FactoryFactorCommendBefore", true);
                 subreportFactor.Visible = IncludeHeader;
                 subreportFactor.ReportSource = Manu_FactoryFactorBefor();


                 XRSubreport subreportFactorAfter = (XRSubreport)rptForm.FindControl("subRptrptManu_FactoryFactorCommendAfter", true);
                 subreportFactorAfter.Visible = IncludeHeader;
                 subreportFactorAfter.ReportSource = Manu_FactoryFactorAfter();

                 /******************** Report Brntag ************************/
                 XRSubreport subreportBrntage = (XRSubreport)rptForm.FindControl("subRptrptManu_FactoryBrntageCommendBefor", true);
                 subreportBrntage.Visible = IncludeHeader;
                 subreportBrntage.ReportSource = Manu_FactoryBrntageBefor();


                 XRSubreport subreportBrntagAfter = (XRSubreport)rptForm.FindControl("subRptrptManu_FactoryBrntageCommendAfter", true);
                 subreportBrntagAfter.Visible = IncludeHeader;
                 subreportBrntagAfter.ReportSource = Manu_FactoryBrntageAfter();

                 /******************** Report Compound ************************/
                 XRSubreport subreportCompound = (XRSubreport)rptForm.FindControl("subRptrptManu_FactoryCompoundCommendBefor", true);
                 subreportCompound.Visible = IncludeHeader;
                 subreportCompound.ReportSource = Manu_FactoryCompoundBefor();


                 XRSubreport subreportCompoundAfter = (XRSubreport)rptForm.FindControl("subRptrptManu_FactoryCompoundCommendAfter", true);
                 subreportCompoundAfter.Visible = IncludeHeader;
                 subreportCompoundAfter.ReportSource = Manu_FactoryCompoundAfter();

                 /******************** Report Addtional ************************/
                 XRSubreport subreportAddtional = (XRSubreport)rptForm.FindControl("subRptrptManu_FactoryAddtionalCommendAfter", true);
                 subreportAddtional.Visible = IncludeHeader;
                 subreportAddtional.ReportSource = Manu_FactoryAddtional();



                 /******************** Report talmee ************************/
                 XRSubreport subreportTalmee = (XRSubreport)rptForm.FindControl("subRptrptManu_FactoryTalmeeCommendBefor", true);
                 subreportTalmee.Visible = IncludeHeader;
                 subreportTalmee.ReportSource = Manu_FactoryTalmeeBefor();


                 XRSubreport subreportTalmeeAfter = (XRSubreport)rptForm.FindControl("subRptrptManu_FactoryTalmeeCommendAfter", true);
                 subreportTalmeeAfter.Visible = IncludeHeader;
                 subreportTalmeeAfter.ReportSource = Manu_FactoryTalmeeAfter();



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
         bool IsValidGridPulish()
         {
             double num;

             //if (HasColumnErrors)
             //{
             //    Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
             //    return !HasColumnErrors;
             //}

             GridViewBeforPrentag.MoveLast();

             int length = GridViewBeforPrentag.RowCount - 1;
             if (length <= 0)
             {
                 Messages.MsgError(Messages.TitleError, Messages.msgThereIsNoRecordInput);
                 return false;
             }
             for (int i = 0; i < length; i++)
             {
                 foreach (GridColumn col in GridViewBeforPrentag.Columns)
                 {
                     if (col.FieldName == "MachinID" || col.FieldName == "PrentagCredit" || col.FieldName == "PrentagDebit")
                     {

                         var cellValue = GridViewBeforPrentag.GetRowCellValue(i, col); 

                         if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                         {
                             GridViewBeforPrentag.SetColumnError(col, Messages.msgInputIsRequired);
                             Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                             return false;
                         }
                        

                         else if (!(double.TryParse(cellValue.ToString(), out num)))
                         {
                             GridViewBeforPrentag.SetColumnError(col, Messages.msgInputShouldBeNumber);
                             Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                             return false;
                         }
                         else if (Comon.cDbl(cellValue.ToString()) <= 0)
                         {
                             GridViewBeforPrentag.SetColumnError(col, Messages.msgInputIsGreaterThanZero);
                             Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                             return false;
                         }
                     }
                 }
             }
             return true;
         }
         private void CreateCoding()
         {
             GridViewCodeing.AddNewRow();         
             GridViewCodeing.SetRowCellValue(GridViewCodeing.FocusedRowHandle, "ArbSizeName", txtGroupName.Text);
             GridViewCodeing.SetRowCellValue(GridViewCodeing.FocusedRowHandle, "ArbName", txtItemName.Text);
             GridViewCodeing.SetRowCellValue(GridViewCodeing.FocusedRowHandle, "BarCode", txtBarCode.Text);
         
             GridViewCodeing.SetRowCellValue(GridViewCodeing.FocusedRowHandle, "CostPrice", lblToatalCostOrder.Text.ToString());
             GridViewCodeing.SetRowCellValue(GridViewCodeing.FocusedRowHandle, "SalePrice", lblTotalSale.Text.ToString());
             decimal ZIRCON_W = 0; decimal DIAMOND_W = 0; decimal STONE_W = 0; decimal BAGET_W = 0;
               for (int i = 0; i < gridViewAfterCompond.DataRowCount; i++)
                   {
                   
                      if (Comon.cInt(gridViewAfterCompond.GetRowCellValue(i, "TypeID")) == 5)
                          DIAMOND_W += Comon.cDec(gridViewAfterCompond.GetRowCellValue(i, "ComWeightStonAfter"));
                       else if (Comon.cInt(gridViewAfterCompond.GetRowCellValue(i, "TypeID")) == 10)
                          STONE_W += Comon.cDec(gridViewAfterCompond.GetRowCellValue(i, "ComWeightStonAfter"));
                       else if (Comon.cInt(gridViewAfterCompond.GetRowCellValue(i, "TypeID")) == 6)
                          ZIRCON_W += Comon.cDec(gridViewAfterCompond.GetRowCellValue(i, "ComWeightStonAfter"));
                      else if (Comon.cInt(gridViewAfterCompond.GetRowCellValue(i, "TypeID")) == 12)
                          BAGET_W += Comon.cDec(gridViewAfterCompond.GetRowCellValue(i, "ComWeightStonAfter"));
                 }


               GridViewCodeing.SetRowCellValue(GridViewCodeing.FocusedRowHandle, "DIAMOND_W", DIAMOND_W.ToString());
               GridViewCodeing.SetRowCellValue(GridViewCodeing.FocusedRowHandle, "STONE_W", STONE_W.ToString());
               GridViewCodeing.SetRowCellValue(GridViewCodeing.FocusedRowHandle, "ZIRCON_W", ZIRCON_W.ToString());
               GridViewCodeing.SetRowCellValue(GridViewCodeing.FocusedRowHandle, "BAGET_W", BAGET_W.ToString());

               decimal TotalQtyGold = 0;
               for (int i = 0; i < GridViewAfterPolish.DataRowCount; i++)
               {
                   TotalQtyGold +=Comon.cDec( GridViewAfterPolish.GetRowCellValue(i, "Credit"));
               }
               decimal TotalStone = 0;
               TotalStone = Comon.cDec(Comon.cDec(DIAMOND_W) + Comon.cDec(STONE_W) + Comon.cDec(ZIRCON_W) + Comon.cDec(BAGET_W));
               GridViewCodeing.SetRowCellValue(GridViewCodeing.FocusedRowHandle, "MinLimitQty",Comon.cDec(Comon.cDec(TotalQtyGold)-Comon.cDec(TotalStone)) .ToString());
               GridViewCodeing.SetRowCellValue(GridViewCodeing.FocusedRowHandle, "Caliber", 18);
         }
         private bool AddItems(string BarCode, decimal STONE_W, decimal DIAMOND_W, decimal ZIRCON_W, decimal BAGET_W)
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
                 long ItemID = Comon.cInt(Lip.GetValue(" Select ItemID from Stc_ItemUnits  where BarCode='" + BarCode.Trim() + "'"));

                 double GroupID = Comon.cDbl(txtGroupID.Text.ToString());
                 double ItemG = 0;
                 if (ItemID == 0)
                 {
                     ItemID = Item.GetNewID();
                     ItemG = Lip.GetNewID(GroupID);
                     IsNewItem = true;
                 }
                 else
                     ItemG = Comon.cInt(Lip.GetValue(" Select ItemGroupID from Stc_Items Where ItemID=" + ItemID).ToString());

                 Lip.AddNumericField("ItemID", ItemID.ToString());
                 Lip.AddStringField("ArbName", txtItemName.Text.ToString());
                 Lip.AddStringField("EngName", txtItemName.Text.ToString());
                 Lip.AddNumericField("GroupID", GroupID.ToString());
                 Lip.AddNumericField("ItemGroupID", ItemG.ToString());
                 Lip.AddStringField("Notes", "");
                 Lip.AddNumericField("TypeID", 1);
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
                 Lip.AddNumericField("BaseID", 0);
                 Lip.AddNumericField("BranchID", 0);
                 Lip.AddNumericField("STONE_W", STONE_W.ToString());
                 Lip.AddNumericField("DIAMOND_W", DIAMOND_W.ToString());
                 Lip.AddNumericField("ZIRCON_W", ZIRCON_W.ToString());
                 Lip.AddNumericField("BAGET_W", BAGET_W.ToString());
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
                 //long SizeID = Comon.cLong(Lip.GetValue("Select Top 1 SizeID From Stc_SizingUnits Where  LOWER (" + PrimaryName + ")=LOWER ('" +   txtGroupName.Text.ToString() + "'"));
                 Lip.AddNumericField("ItemID", ItemID.ToString());

                 strSQL = "Select Notes From Stc_ItemsGroups where GroupID=" + GroupID;
                 DataTable dtGroup = Lip.SelectRecord(strSQL);
                 string GroupName = dtGroup.Rows[0]["Notes"].ToString();

                 if (BarCode == string.Empty)
                     BarCode = GroupName + ItemG.ToString().PadLeft(4, '0');


                 Lip.AddNumericField("SizeID", "1");
                 Lip.AddStringField("BarCode", BarCode);
                 Lip.AddNumericField("PackingQty", 1);
                 decimal CostPrice = Comon.cDec(lblToatalCostOrder.Text.ToString());
                 //سعر تكلفة مع مصاريف
                 decimal SpendPrice = Comon.ConvertToDecimalPrice(lblTotalSale.Text);
                 //سعر الكارت وهو البيع
                 decimal SalePrice = Comon.ConvertToDecimalPrice(lblTotalSale.Text);
                 Lip.AddNumericField("SalePrice", SalePrice.ToString());
                 Lip.AddNumericField("CostPrice", lblToatalCostOrder.Text.ToString());
                 Lip.AddNumericField("STONE_W", STONE_W.ToString());
                 Lip.AddNumericField("DIAMOND_W", DIAMOND_W.ToString());
                 Lip.AddNumericField("ZIRCON_W", ZIRCON_W.ToString());
                 Lip.AddNumericField("BAGET_W", BAGET_W.ToString());
                 decimal TotalQtyGold = 0;
                 for (int i = 0; i < GridViewAfterPolish.DataRowCount; i++)
                 {
                     TotalQtyGold += Comon.cDec(GridViewAfterPolish.GetRowCellValue(i, "Credit"));
                 }
                 decimal TotalStone = 0;
                 TotalStone = Comon.cDec(Comon.cDec(DIAMOND_W) + Comon.cDec(STONE_W) + Comon.cDec(ZIRCON_W) + Comon.cDec(BAGET_W));
                
                 Lip.AddNumericField("MinLimitQty",Comon.cDec(Comon.cDec(TotalQtyGold) - Comon.cDec(TotalStone)).ToString());
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
                 strSQL = "delete from Sales_PurchaseInvoiceDetails where  InvoiceID= -1 And  BarCode='" + BarCode.Trim() + "'";
                 Lip.ExecututeSQL(strSQL);
                 {
                     Application.DoEvents();
                     Lip.NewFields();
                     Lip.Table = "Sales_PurchaseInvoiceDetails";
                     Lip.AddNumericField("InvoiceID", -1);
                     Lip.AddNumericField("BranchID", 0);
                     Lip.AddNumericField("FacilityID", UserInfo.FacilityID.ToString());
                     Lip.AddNumericField("ItemID", ItemID.ToString());
                     Lip.AddNumericField("SizeID", "1");
                     Lip.AddNumericField("QTY", Comon.cDec(Comon.cDec(TotalQtyGold) - Comon.cDec(TotalStone)).ToString());
                     Lip.AddNumericField("CostPrice", lblToatalCostOrder.Text.ToString());
                     Lip.AddNumericField("Bones", 0);
                     Lip.AddNumericField("StoreID", 0);
                     Lip.AddNumericField("Discount", 0);
                     Lip.AddNumericField("ExpiryDate", 20201101);
                     Lip.AddNumericField("SalePrice", lblTotalSale.Text);
                     Lip.AddStringField("BarCode", BarCode);
                     Lip.AddStringField("Serials", "");
                     Lip.AddNumericField("Cancel", 0);
                     Lip.AddNumericField("ItemStatus", -1);
                     Lip.AddNumericField("AdditionalValue", 0);
                     Lip.AddNumericField("Caliber", 18);

                     Lip.AddNumericField("STONE_W", STONE_W.ToString());
                     Lip.AddNumericField("DIAMOND_W", DIAMOND_W.ToString());
                     Lip.AddNumericField("ZIRCON_W", ZIRCON_W.ToString());
                     Lip.AddNumericField("BAGET_W", BAGET_W.ToString());
                     Lip.AddStringField("Description", txtNotes.Text);

                     Lip.AddStringField("CLARITY", "SI");
                     Lip.AddStringField("Color", "FG");

                     Lip.ExecuteInsert();
                     CreateCoding();
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
        private void Save()
        {
            
            if (radioButton1.Checked == false && radioButton2.Checked == false && radioButton3.Checked == false)
            {
                Messages.MsgError(Messages.msgErrorSave, "الرجاء إختيار نوع القطعة ");
                TabPageOpretoinOrder.SelectedTabPage = xtraTabPage2;
            }
            else
                if (txtEmplooyID.Text.Trim() == "" || String.IsNullOrWhiteSpace(txtEmplooyID.Text))
            {
                Messages.MsgError(Messages.msgErrorSave, "إدخل بيانات عامل البرنتاج "); txtEmplooyID.Focus(); return;
                TabPageOpretoinOrder.SelectedTabPage = xtraTabPage1;
            }
            else
            {
                Menu_FactoryRunCommandTalmee returnedTalmee;
                List<Menu_FactoryRunCommandTalmee> listreturnedTalmee = new List<Menu_FactoryRunCommandTalmee>();

                Menu_FactoryRunCommandMaster objRecord = new Menu_FactoryRunCommandMaster();
                objRecord.Barcode = txtBarCode.Text.ToString();
                objRecord.BranchID = UserInfo.BRANCHID;
                objRecord.BrandID = Comon.cInt(txtBrandID.Text);
                objRecord.Cancel = 0;
                objRecord.PeiceName = txtItemName.Text + "";
                objRecord.ComandID = Comon.cInt(txtCommandID.Text);
                objRecord.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                objRecord.CustomerID = Comon.cDbl(txtCustomerID.Text);
                objRecord.DocumentID = Comon.cInt(txtReferanceID.Text);
                objRecord.EmpFactorID = Comon.cDbl(txtEmpIDFactor.Text);
                objRecord.EmployeeID = Comon.cDbl(txtEmployeeStokIDFactory.Text);
                objRecord.EmployeeStokID = Comon.cDbl(txtEmployeeStokIDFactory.Text);
                objRecord.EmpPolishnID = Comon.cDbl(txtEmplooyIDPolishing.Text);
                objRecord.EmpPrentagID = Comon.cDbl(txtEmplooyID.Text);
                objRecord.FacilityID = UserInfo.FacilityID;
                objRecord.GivenDate = Comon.ConvertDateToSerial(txtCommandDate.EditValue.ToString());
                objRecord.GoldCompundNet = Comon.cDbl(txtCompondGoldDebit.Text);
                objRecord.GroupID = Comon.cInt(txtGroupID.Text);
                objRecord.ItemID = Comon.cInt(txtItemID.Text);
                objRecord.DelegateID = Comon.cDbl(txtDelegateID.Text);
                //الحسابات

                objRecord.AccountIDFactory = Comon.cDbl(txtAccountIDFactory.Text);
                objRecord.StoreIDFactory = Comon.cDbl(txtStoreIDFactory.Text);
                objRecord.EmployeeStokIDFactory = Comon.cDbl(txtEmployeeStokIDFactory.Text);
                objRecord.EmpIDFactor = Comon.cDbl(txtEmpIDFactor.Text);

                objRecord.AccountIDPrentage = Comon.cDbl(txtAccountIDPrentage.Text);
                objRecord.StoreIDPrentage = Comon.cDbl(txtStoreIDPrentage.Text);
                objRecord.EmployeeStokIDPrentage = Comon.cDbl(txtEmployeeStokIDPrentage.Text);
                objRecord.EmpIDPrentage = Comon.cDbl(txtEmpIDPrentage.Text);

                objRecord.AccountIDBeforCompond = Comon.cDbl(txtAccountIDBeforCompond.Text);
                objRecord.StoreIDBeforComond = Comon.cDbl(txtStoreIDBeforComond.Text);
                objRecord.EmployeeStokIDBeforCompond = Comon.cDbl(txtEmployeeStokIDBeforCompond.Text);
                objRecord.EmpIDBeforCompond = Comon.cDbl(txtEmpIDBeforCompond.Text);
                objRecord.AccountIDAdditions = Comon.cDbl(txtAccountIDAdditions.Text);
                objRecord.StoreIDAdditions = Comon.cDbl(txtStoreIDAdditions.Text);
                objRecord.EmployeeStokIDAdditions = Comon.cDbl(txtEmployeeStokIDAdditions.Text);
                objRecord.EmpIDAdditions = Comon.cDbl(txtEmpIDAdditions.Text);
                objRecord.AccountIDPolishing = Comon.cDbl(txtAccountIDPolishing.Text);
                objRecord.StoreIDPolishing = Comon.cDbl(txtStoreIDPolishing.Text);
                objRecord.EmployeeStokIDPolishing = Comon.cDbl(txtEmployeeStokIDPolishing.Text);
                objRecord.EmplooyIDPolishing = Comon.cDbl(txtEmplooyIDPolishing.Text);
                objRecord.AccountIDBarcodeItem = Comon.cDbl(txtAccountIDBarcodeItem.Text);
                objRecord.StoreIDBarcod = Comon.cDbl(txtStoreIDBarcod.Text);
                objRecord.EmployeeStokIDBarcode = Comon.cDbl(txtEmployeeStokIDBarcode.Text);

                objRecord.Notes = txtNotes.Text;
                objRecord.SpendAmount = Comon.cDbl(txtSpendAmount.Text);



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
                objRecord.netGoldWeight = Comon.cDbl(txtNetGold.Text);
                objRecord.OpretionID = Comon.cInt(txtOpretionID.Text);
                objRecord.TypeID = Comon.cInt(txtTypeID.Text);
                objRecord.ThefactoriID = Comon.cInt(txtThefactoriID.Text);
                objRecord.TotalLost = Comon.cDbl(txtTotallosed.Text);

                if (radioButton1.Checked)
                    objRecord.piece = 1;
                else if (radioButton2.Checked)
                    objRecord.piece = 2;
                else if (radioButton3.Checked)
                    objRecord.piece = 3;
        
                objRecord.GivenDate = Comon.ConvertDateToSerial(txtGivenDate.EditValue.ToString());
        
                objRecord.GivenTime = Comon.cDbl(txtGivenTime.Text);

            
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
                        ////returnedGold.MachinID = Comon.cInt(GridViewBeforfactory.GetRowCellValue(i, "MachinID").ToString());
                        ////returnedGold.MachineName = GridViewBeforfactory.GetRowCellValue(i, "MachineName").ToString();
                        //====حقول مضافة

                        returnedGold.StoreID = Comon.cInt(txtStoreIDFactory.Text.ToString());
                        returnedGold.StoreName = lblStoreNameFactory.Text.ToString();

                        returnedGold.EmpID = txtEmpIDFactor.Text.ToString();
                        returnedGold.EmpName = lblEmpNameFactor.Text.ToString();
                        returnedGold.ItemID = Comon.cInt(GridViewBeforfactory.GetRowCellValue(i, "ItemID").ToString());
                        returnedGold.ArbItemName = GridViewBeforfactory.GetRowCellValue(i, SizeName).ToString();
                      
                        returnedGold.SizeID = Comon.cInt(GridViewBeforfactory.GetRowCellValue(i, "SizeID").ToString());
                        returnedGold.ArbSizeName = GridViewBeforfactory.GetRowCellValue(i, SizeName).ToString();
                      
                        returnedGold.DebitTime = GridViewBeforfactory.GetRowCellValue(i, "DebitTime").ToString();
                        returnedGold.DebitDate = Comon.cDate(GridViewBeforfactory.GetRowCellValue(i, "DebitDate").ToString());
                        //====
                        returnedGold.Debit = Comon.cDbl(GridViewBeforfactory.GetRowCellValue(i, "Debit").ToString());
                        returnedGold.TypeOpration = 1;
                        //returnedGold.Signature = GridViewBeforfactory.GetRowCellValue(i, "Signature").ToString();
                        returnedGold.EmpPrentagID = Comon.cDbl(txtEmplooyID.Text);
                        returnedGold.BranchID = UserInfo.BRANCHID;
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
                           
                            //returnedGold.MachinID = Comon.cInt(GridViewAfterfactory.GetRowCellValue(i, "MachinID").ToString());
                            //returnedGold.MachineName = GridViewAfterfactory.GetRowCellValue(i, "MachineName").ToString();
                            //====حقول مضافة

                            returnedGold.StoreID = Comon.cInt(txtStoreIDFactory.Text.ToString());
                            returnedGold.StoreName = lblStoreNameFactory.Text.ToString();

                            returnedGold.EmpID = txtEmpIDFactor.Text.ToString();
                            returnedGold.EmpName = lblEmpNameFactor.Text.ToString();
                            returnedGold.ItemID = Comon.cInt(GridViewAfterfactory.GetRowCellValue(i, "ItemID").ToString());
                            returnedGold.ArbItemName = GridViewAfterfactory.GetRowCellValue(i, SizeName).ToString();
                            returnedGold.SizeID = Comon.cInt(GridViewAfterfactory.GetRowCellValue(i, "SizeID").ToString());
                            returnedGold.ArbSizeName = GridViewAfterfactory.GetRowCellValue(i, SizeName).ToString();

                            returnedGold.DebitTime = GridViewAfterfactory.GetRowCellValue(i, "DebitTime").ToString();
                            returnedGold.DebitDate = Comon.cDate(GridViewAfterfactory.GetRowCellValue(i, "DebitDate").ToString());
                            //====
                            returnedGold.Credit = Comon.cDbl(GridViewAfterfactory.GetRowCellValue(i, "Credit").ToString());
                            returnedGold.TypeOpration = 2;
                            //returnedGold.Signature = GridViewBeforfactory.GetRowCellValue(i, "Signature").ToString();
                            returnedGold.EmpPrentagID = Comon.cDbl(txtEmplooyID.Text);
                            returnedGold.BranchID = UserInfo.BRANCHID;
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
              
           
                //برنتاج 
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

                        //====حقول مضافة

                        returned.StoreID = Comon.cInt(txtStoreIDPrentage.Text.ToString());
                        returned.StoreName = lblStoreNamePrentage.Text.ToString();

                        returned.EmpID = txtEmpIDPrentage.Text.ToString();
                        returned.EmpName = lblEmpNamePrentage.Text.ToString();

                        returned.ItemID = Comon.cInt(GridViewBeforPrentag.GetRowCellValue(i, "ItemID").ToString());
                        returned.ArbItemName = GridViewBeforPrentag.GetRowCellValue(i, ItemName).ToString();
                        returned.EngItemName = GridViewBeforPrentag.GetRowCellValue(i, ItemName).ToString();

                        returned.SizeID = Comon.cInt(GridViewBeforPrentag.GetRowCellValue(i, "SizeID").ToString());
                        returned.ArbSizeName = GridViewBeforPrentag.GetRowCellValue(i, SizeName).ToString();
                        returned.EngSizeName = GridViewBeforPrentag.GetRowCellValue(i, SizeName).ToString();
                        returned.PrentagDebitTime = GridViewBeforPrentag.GetRowCellValue(i, "PrentagDebitTime").ToString();
                        returned.PrentagDebitDate = Comon.cDate(GridViewBeforPrentag.GetRowCellValue(i, "PrentagDebitDate").ToString());
                        //====


                        returned.PrentagDebit = Comon.cDbl(GridViewBeforPrentag.GetRowCellValue(i, "PrentagDebit").ToString());
                        returned.TypeOpration = 1;
                        returned.PrSignature = "";
                        returned.EmpPrentagID = Comon.cDbl(txtEmplooyID.Text);
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
                    if(lengthAfterPrentage>0)
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

                            returned.StoreID = Comon.cInt(txtStoreIDPrentage.Text.ToString());
                            returned.StoreName = lblStoreNamePrentage.Text.ToString();

                            returned.EmpID = txtEmpIDPrentage.Text.ToString();
                            returned.EmpName = lblEmpNamePrentage.Text.ToString();

                            returned.ItemID = Comon.cInt(GridViewAfterPrentag.GetRowCellValue(i, "ItemID").ToString());
                            returned.ArbItemName = GridViewAfterPrentag.GetRowCellValue(i, ItemName).ToString();
                            returned.EngItemName = GridViewAfterPrentag.GetRowCellValue(i, ItemName).ToString();

                            returned.SizeID = Comon.cInt(GridViewAfterPrentag.GetRowCellValue(i, "SizeID").ToString());
                            returned.ArbSizeName = GridViewAfterPrentag.GetRowCellValue(i, SizeName).ToString();
                            returned.EngSizeName = GridViewAfterPrentag.GetRowCellValue(i, SizeName).ToString();
                            returned.PrentagDebitTime = GridViewAfterPrentag.GetRowCellValue(i, "PrentagDebitTime").ToString();
                            returned.PrentagDebitDate = Comon.cDate(GridViewAfterPrentag.GetRowCellValue(i, "PrentagDebitDate").ToString());
                            //====
                            returned.PrentagCredit = Comon.cDbl(GridViewAfterPrentag.GetRowCellValue(i, "PrentagCredit").ToString());
                            returned.TypeOpration = 2;
                            returned.PrSignature = "";
                            returned.EmpPrentagID = Comon.cDbl(txtEmplooyID.Text);
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
                //تركيب 
                #region Save Compund
                Menu_FactoryRunCommandCompund returnedCompund;
                List<Menu_FactoryRunCommandCompund> listreturnedCompund = new List<Menu_FactoryRunCommandCompund>();
                int lengthCompund = gridViewBeforCompond.DataRowCount;
                int lengthAfterCompund = gridViewAfterCompond.DataRowCount;
                if (lengthCompund > 0)
                {

                    for (int i = 0; i < lengthCompund; i++)
                    {
                        if (gridViewBeforCompond.GetRowCellValue(i, "BarcodCompond") == null)
                        {
                            Messages.MsgError(Messages.msgErrorSave, "الرجاء إدخال باركود القطعة للتركيب ");
                            TabPageOpretoinOrder.SelectedTabPage = xtraTabPage2;

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
                            returnedCompund.SizeID =Comon.cInt( gridViewBeforCompond.GetRowCellValue(i, "SizeID").ToString());
                            returnedCompund.ItemID = Comon.cInt(gridViewBeforCompond.GetRowCellValue(i, "ItemID").ToString());
                            returnedCompund.DebitTime = gridViewBeforCompond.GetRowCellValue(i, "DebitTime").ToString();
                            returnedCompund.DebitDate = Comon.cDate(gridViewBeforCompond.GetRowCellValue(i, "DebitDate").ToString());
                            //returnedCompund.TypeSton = gridViewBeforCompond.GetRowCellValue(i, "TypeSton").ToString();
                            returnedCompund.CostPrice =Comon.cDec(gridViewBeforCompond.GetRowCellValue(i, "CostPrice").ToString());
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

                        for (int i = 0; i < lengthAfterCompund; i++)
                        {
                            if (gridViewAfterCompond.GetRowCellValue(i, "BarcodCompond") == null)
                            {
                                Messages.MsgError(Messages.msgErrorSave, "الرجاء إدخال باركود القطعة للتركيب ");
                                TabPageOpretoinOrder.SelectedTabPage = xtraTabPage2;

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

                #region Save Talmee
                //تلميع 
                int lengthTalmee = GridViewBeforPolish.DataRowCount;
                int lengthAfterTalmee = GridViewAfterPolish.DataRowCount;
                if (lengthTalmee > 0)
                {
                    if (txtEmplooyIDPolishing.Text.Trim() == "" || String.IsNullOrWhiteSpace(txtEmplooyIDPolishing.Text))
                    {
                        Messages.MsgError(Messages.msgErrorSave, "إدخل بيانات عامل التلميع "); txtEmplooyIDPolishing.Focus(); return;
                        TabPageOpretoinOrder.SelectedTabPage = xtraTabPage4;
                    }
                    else
                    {
                        for (int i = 0; i < lengthTalmee; i++)
                        {
                            returnedTalmee = new Menu_FactoryRunCommandTalmee();
                            returnedTalmee.ID = i + 1;
                            returnedTalmee.ComandID = Comon.cInt(txtCommandID.Text.ToString());
                            
                            returnedTalmee.MachinID = Comon.cInt(GridViewBeforPolish.GetRowCellValue(i, "MachinID").ToString());
                            returnedTalmee.Debit = Comon.cDbl(GridViewBeforPolish.GetRowCellValue(i, "Debit").ToString());
                            returnedTalmee.TypeOpration = 1; 
                            returnedTalmee.StoreID = Comon.cInt(txtStoreIDPolishing.Text.ToString());
                            returnedTalmee.StoreName = lblStoreNamePolishin.Text.ToString();

                            returnedTalmee.EmpID = txtEmplooyIDPolishing.Text.ToString();
                            returnedTalmee.EmpName = lblEmpolyeePolishingName.Text.ToString();
                           
                            returnedTalmee.SizeID = Comon.cInt(GridViewBeforPolish.GetRowCellValue(i, "SizeID").ToString());
                            returnedTalmee.ItemID = Comon.cInt(GridViewBeforPolish.GetRowCellValue(i, "ItemID").ToString());
                            returnedTalmee.DebitDate =Comon.cDate( GridViewBeforPolish.GetRowCellValue(i, "DebitDate").ToString());
                            returnedTalmee.DebitTime = GridViewBeforPolish.GetRowCellValue(i, "DebitTime").ToString();
                            returnedTalmee.ArbItemName = GridViewBeforPolish.GetRowCellValue(i, ItemName).ToString();
                            returnedTalmee.EngItemName = GridViewBeforPolish.GetRowCellValue(i, ItemName).ToString(); 
                            returnedTalmee.ArbSizeName = GridViewBeforPolish.GetRowCellValue(i,SizeName).ToString();
                            returnedTalmee.EngSizeName = GridViewBeforPolish.GetRowCellValue(i, SizeName).ToString();  
                            returnedTalmee.MachineName = GridViewBeforPolish.GetRowCellValue(i, "MachineName").ToString();
                            returnedTalmee.BranchID = UserInfo.BRANCHID;
                            returnedTalmee.EmpPolishnID = Comon.cDbl(txtEmplooyIDPolishing.Text);
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

                                returnedTalmee.StoreID = Comon.cInt(txtStoreIDPolishing.Text.ToString());
                                returnedTalmee.StoreName = lblStoreNamePolishin.Text.ToString();

                                returnedTalmee.EmpID = txtEmplooyIDPolishing.Text.ToString();
                                returnedTalmee.EmpName = lblEmpolyeePolishingName.Text.ToString();

                                returnedTalmee.SizeID = Comon.cInt(GridViewAfterPolish.GetRowCellValue(i, "SizeID").ToString());
                                returnedTalmee.ItemID = Comon.cInt(GridViewAfterPolish.GetRowCellValue(i, "ItemID").ToString());
                                returnedTalmee.DebitDate = Comon.cDate(GridViewAfterPolish.GetRowCellValue(i, "DebitDate").ToString());
                                returnedTalmee.DebitTime = GridViewAfterPolish.GetRowCellValue(i, "DebitTime").ToString();
                                returnedTalmee.ArbItemName = GridViewAfterPolish.GetRowCellValue(i, ItemName).ToString();
                                returnedTalmee.EngItemName = GridViewAfterPolish.GetRowCellValue(i, ItemName).ToString();
                                returnedTalmee.ArbSizeName = GridViewAfterPolish.GetRowCellValue(i, SizeName).ToString();
                                returnedTalmee.EngSizeName = GridViewAfterPolish.GetRowCellValue(i, SizeName).ToString(); 
                                returnedTalmee.MachineName = GridViewAfterPolish.GetRowCellValue(i, "MachineName").ToString();
                                returnedTalmee.BranchID = UserInfo.BRANCHID;
                                returnedTalmee.EmpPolishnID = Comon.cDbl(txtEmplooyIDPolishing.Text);
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

                //الإضافات                  
                #region Save Addtional
                Menu_FactoryRunCommandSelver returnedSelver;
                List<Menu_FactoryRunCommandSelver> listreturnedSelver = new List<Menu_FactoryRunCommandSelver>();
                int lengthSelver = gridViewAdditional.DataRowCount;
                    if (lengthSelver > 0)
                {
                    if (txtEmplooyIDPolishing.Text.Trim() == "" || String.IsNullOrWhiteSpace(txtEmplooyIDPolishing.Text))
                    {
                        Messages.MsgError(Messages.msgErrorSave, "إدخل بيانات عامل التلميع "); txtEmplooyIDPolishing.Focus(); return;
                        TabPageOpretoinOrder.SelectedTabPage = xtraTabPage4;
                    }
                    else
                    {
                         

                        for (int i = 0; i < lengthSelver; i++)
                        {
                            returnedSelver = new Menu_FactoryRunCommandSelver();
                            returnedSelver.ID = i + 1;
                            returnedSelver.ComandID = Comon.cInt(txtCommandID.Text.ToString());

                            returnedSelver.MachinID = Comon.cInt(gridViewAdditional.GetRowCellValue(i, "MachinID").ToString());
                            returnedSelver.Debit = Comon.cDbl(gridViewAdditional.GetRowCellValue(i, "Debit").ToString());
                            returnedSelver.EmpID = gridViewAdditional.GetRowCellValue(i, "EmpID").ToString();
                           returnedSelver.StoreID =Comon.cInt( gridViewAdditional.GetRowCellValue(i, "StoreID").ToString());
                           returnedSelver.StoreName = gridViewAdditional.GetRowCellValue(i, "StoreName").ToString();
                            returnedSelver.SizeID = Comon.cInt(gridViewAdditional.GetRowCellValue(i, "SizeID").ToString());
                            returnedSelver.ItemID = Comon.cInt(gridViewAdditional.GetRowCellValue(i, "ItemID").ToString());
                            returnedSelver.DebitDate = Comon.cDate(gridViewAdditional.GetRowCellValue(i, "DebitDate").ToString());
                            returnedSelver.DebitTime = gridViewAdditional.GetRowCellValue(i, "DebitTime").ToString();
                            returnedSelver.ArbItemName = gridViewAdditional.GetRowCellValue(i, ItemName).ToString();
                            returnedSelver.EngItemName = gridViewAdditional.GetRowCellValue(i, ItemName).ToString();
                            returnedSelver.ArbSizeName = gridViewAdditional.GetRowCellValue(i, SizeName).ToString();
                            returnedSelver.EngSizeName = gridViewAdditional.GetRowCellValue(i, SizeName).ToString();
                            returnedSelver.EmpName = gridViewAdditional.GetRowCellValue(i, "EmpName").ToString();
                            returnedSelver.MachineName = gridViewAdditional.GetRowCellValue(i, "MachineName").ToString();
                            returnedSelver.BranchID = UserInfo.BRANCHID;                          
                            returnedSelver.Cancel = 0;
                            returnedSelver.UserID = UserInfo.ID;
                            returnedSelver.RegDate = Comon.ConvertDateToSerial(Lip.GetServerDate());
                            returnedSelver.RegTime = Comon.cDbl(Lip.GetServerTimeSerial());
                            returnedSelver.ComputerInfo = UserInfo.ComputerInfo;
                            returnedSelver.FacilityID = UserInfo.FacilityID;
                            if (IsNewRecord == false)
                            {
                                returnedSelver.EditUserID = UserInfo.ID;
                                returnedSelver.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                                returnedSelver.EditDate = Comon.ConvertDateToSerial(Lip.GetServerDate());
                                returnedSelver.EditComputerInfo = UserInfo.ComputerInfo;
                            }
                            listreturnedSelver.Add(returnedSelver);
                        }

                    }

                }
                      #endregion
                    //مصروفات الانتاج
                #region Save ProductionExpenses
                    Manu_ProductionExpensesDetails returnedProductionExpenses;
                    List<Manu_ProductionExpensesDetails> listreturnedProductionExpenses = new List<Manu_ProductionExpensesDetails>();
                    int lengthProductionExpenses = GridProductionExpenses.DataRowCount;

                    if (lengthProductionExpenses > 0)
                    {
                        for (int i = 0; i < lengthProductionExpenses; i++)
                        {
                            returnedProductionExpenses = new Manu_ProductionExpensesDetails();                            
                            returnedProductionExpenses.ComandID = Comon.cInt(txtCommandID.Text.ToString());
                             
                            //====حقول مضافة
                           returnedProductionExpenses.AccountID = Comon.cDbl(GridProductionExpenses.GetRowCellValue(i, "AccountID").ToString());
                            returnedProductionExpenses.AccountName = GridProductionExpenses.GetRowCellValue(i, "AccountName").ToString();
                            //returnedProductionExpenses.AverageHoursPerDay = Comon.cDec(GridProductionExpenses.GetRowCellValue(i, "AverageHoursPerDay").ToString());
                            //returnedProductionExpenses.DepreciationPercentage =Comon.cDec( GridProductionExpenses.GetRowCellValue(i, "DepreciationPercentage").ToString());
                            //returnedProductionExpenses.Installment = Comon.cDec(GridProductionExpenses.GetRowCellValue(i, "Installment").ToString());
                            //returnedProductionExpenses.MainValue = Comon.cDec(GridProductionExpenses.GetRowCellValue(i, "MainValue").ToString());
                            //returnedProductionExpenses.OrderCostPercentage = Comon.cDec(GridProductionExpenses.GetRowCellValue(i, "OrderCostPercentage").ToString());
                            //returnedProductionExpenses.PeriodInDays = Comon.cInt(GridProductionExpenses.GetRowCellValue(i, "PeriodInDays").ToString());
                        
                            returnedProductionExpenses.BranchID = UserInfo.BRANCHID;
                            returnedProductionExpenses.Cancel = 0;
                            returnedProductionExpenses.UserID = UserInfo.ID;
                            returnedProductionExpenses.RegDate = Comon.ConvertDateToSerial(Lip.GetServerDate());
                            returnedProductionExpenses.RegTime = Comon.cDbl(Lip.GetServerTimeSerial());

                            returnedProductionExpenses.ComputerInfo = UserInfo.ComputerInfo;
                            if (IsNewRecord == false)
                            {

                                returnedProductionExpenses.EditUserID = UserInfo.ID;
                                returnedProductionExpenses.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                                returnedProductionExpenses.EditDate = Comon.ConvertDateToSerial(Lip.GetServerDate());
                                returnedProductionExpenses.EditComputerInfo = UserInfo.ComputerInfo;
                            }
                            listreturnedProductionExpenses.Add(returnedProductionExpenses);
                        }
                    }                   
                    #endregion 
                     
                    //save
                    if (listreturned.Count > 0)
                        objRecord.Menu_F_Prentag = listreturned;

                    if (listreturnedCompund.Count > 0)
                        objRecord.Menu_F_Compund = listreturnedCompund;

                    if (listreturnedTalmee.Count > 0)
                        objRecord.Menu_F_Talmee = listreturnedTalmee;

                    if (listreturnedSelver.Count > 0)
                        objRecord.Menu_F_Selver = listreturnedSelver;

                    if (listreturnedFactory.Count > 0)
                        objRecord.Menu_F_Factory = listreturnedFactory;

                    if (listreturnedProductionExpenses.Count > 0)
                        objRecord.Menu_F_ProductionExpenses = listreturnedProductionExpenses;

                    string Result = Menu_FactoryRunCommandMasterDAL.InsertUsingXML(objRecord, IsNewRecord).ToString();
                    if (Comon.cInt(Result) > 0)
                    {
                        //أوامر الصرف والتوريد الخاص بالتصنيع
                        if (lengthfactry > 0)
                        {
                            SaveOutOn(); //حفظ   الصرف المخزني
                            // حفظ الحركة المخزنية 
                            if (Comon.cInt(Result) > 0)
                            {
                                int MoveID = SaveStockMoveingOut(Comon.cInt(txtCommandID.Text));
                                if (MoveID == 0)
                                    Messages.MsgError(Messages.TitleInfo, "  خطا في حفظ الحركة المخزنية تصنيع- قبل");
                            }
                        }
                        if (lengthAfterfactry > 0)
                        {
                            SaveInOn(); //حفظ   التوريد المخزني
                            // حفظ الحركة المخزنية 
                            if (Comon.cInt(Result) > 0)
                            {
                                int MoveID = SaveStockMoveingIn(Comon.cInt(txtCommandID.Text));
                                if (MoveID == 0)
                                    Messages.MsgError(Messages.TitleInfo, "خطا في حفظ الحركة المخزنية تصنيع - بعد ");
                            }
                        }
                        if (lengthPrentage > 0)
                        {
                            //أوامر الصرف والتوريد الخاص بالبرنتاج
                            SaveOutOnBrntage(); //حفظ   الصرف المخزني
                            // حفظ الحركة المخزنية 
                            if (Comon.cInt(Result) > 0)
                            {
                                int MoveID = SaveStockMoveingBrntageOut(Comon.cInt(txtCommandID.Text));
                                if (MoveID == 0)
                                    Messages.MsgError(Messages.TitleInfo, "خطا في حفظ الحركة المخزنية برنتاج - قبل ");
                            }
                        }
                        if (lengthAfterPrentage > 0)
                        {
                            SaveInOnBrntage(); //حفظ   التوريد المخزني
                            // حفظ الحركة المخزنية 
                            if (Comon.cInt(Result) > 0)
                            {
                                int MoveID = SaveStockMoveingBrntageIn(Comon.cInt(txtCommandID.Text));
                                if (MoveID == 0)
                                    Messages.MsgError(Messages.TitleInfo, "خطا في حفظ الحركة المخزنية برنتاج - بعد");
                            }
                        }

                        //أوامر الصرف والتوريد الخاص بالتركيب 
                        if (lengthCompund > 0)
                        {
                            SaveOutOnCompound(); //حفظ   الصرف المخزني
                            // حفظ الحركة المخزنية 
                            if (Comon.cInt(Result) > 0)
                            {
                                int MoveID = SaveStockMoveingCommpoundOut(Comon.cInt(txtCommandID.Text));
                                if (MoveID == 0)
                                    Messages.MsgError(Messages.TitleInfo, "خطا في حفظ الحركة المخزنية تركيب - قبل ");
                            }
                        }
                        if (lengthAfterCompund > 0)
                        {
                            SaveInOnCompound(); //حفظ   التوريد المخزني
                            // حفظ الحركة المخزنية 
                            if (Comon.cInt(Result) > 0)
                            {
                                int MoveID = SaveStockMoveingCommpoundIn(Comon.cInt(txtCommandID.Text));
                                if (MoveID == 0)
                                    Messages.MsgError(Messages.TitleInfo, "خطا في حفظ الحركة المخزنية تركيب - بعد ");
                            }
                        }
                       
                        //أوامر الصرف والتوريد الخاص بالتلميع
                        if (lengthTalmee > 0)
                        {
                            SaveOutOnPolshin(); //حفظ   الصرف المخزني
                            // حفظ الحركة المخزنية 
                            if (Comon.cInt(Result) > 0)
                            {
                                int MoveID = SaveStockMoveingPolshinOut(Comon.cInt(txtCommandID.Text));
                                if (MoveID == 0)
                                    Messages.MsgError(Messages.TitleInfo, "  خطا في حفظ الحركة المخزنية تلميع- قبل");
                            }
                        }
                            if (lengthAfterTalmee > 0)
                        {
                            SaveInOnPolshin(); //حفظ   التوريد المخزني
                            // حفظ الحركة المخزنية 
                            if (Comon.cInt(Result) > 0)
                            {
                                int MoveID = SaveStockMoveingPolshinIn(Comon.cInt(txtCommandID.Text));
                                if (MoveID == 0)
                                    Messages.MsgError(Messages.TitleInfo, "  خطا في حفظ الحركة المخزنية تلميع- قبل");
                            }
                        }
                    }
                    if (Comon.cInt(Result) > 0)
                    {
                        if (listreturnedProductionExpenses.Count > 0)
                        {
                            decimal ZIRCON_W = 0; decimal DIAMOND_W = 0; decimal STONE_W = 0; decimal BAGET_W = 0;
                            for (int i = 0; i < gridViewAfterCompond.DataRowCount; i++)
                            {
                                {
                                    if (Comon.cInt(gridViewAfterCompond.GetRowCellValue(i, "TypeID")) == 5)
                                        DIAMOND_W += Comon.cDec(gridViewAfterCompond.GetRowCellValue(i, "ComWeightStonAfter"));
                                    else if (Comon.cInt(gridViewAfterCompond.GetRowCellValue(i, "TypeID")) == 10)
                                        STONE_W += Comon.cDec(gridViewAfterCompond.GetRowCellValue(i, "ComWeightStonAfter"));
                                    else if (Comon.cInt(gridViewAfterCompond.GetRowCellValue(i, "TypeID")) == 6)
                                        ZIRCON_W += Comon.cDec(gridViewAfterCompond.GetRowCellValue(i, "ComWeightStonAfter"));
                                    else if (Comon.cInt(gridViewAfterCompond.GetRowCellValue(i, "TypeID")) ==12)
                                        BAGET_W += Comon.cDec(gridViewAfterCompond.GetRowCellValue(i, "ComWeightStonAfter"));
                                }
                                AddItems(txtBarCode.Text, STONE_W, DIAMOND_W, ZIRCON_W, BAGET_W);
                            }
                        }
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                        ClearFields();
                    }
                    else
                    {
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgErrorSave + " " + Result);
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
            //Date
            objRecord.VoucherDate = Comon.ConvertDateToSerial(txtCommandDate.Text).ToString();
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
            objRecord.Posted = 1;

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
            returned.DebitMatirial = Comon.cDbl(txtTotalQty_FactoryBefore);
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            listreturned.Add(returned);

            //Credit Matirial      
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 1;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cLong(txtStoreIDFactory.Text);
            returned.VoucherID = VoucherID;
            returned.CreditMatirial = Comon.cDbl(txtTotalQty_FactoryBefore);
            returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
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
            objRecord.VoucherDate = Comon.ConvertDateToSerial(txtCommandDate.Text).ToString();
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
            objRecord.Posted = 1;

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
            
            returned.DebitMatirial = Comon.cDbl(txtTotalQty_FactorAfter);
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            listreturned.Add(returned);

            //Credit Matirial      
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 1;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(txtAccountIDFactory.Text);
            returned.VoucherID = VoucherID;
            returned.CreditMatirial = Comon.cDbl(txtTotalQty_FactorAfter);
            returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            listreturned.Add(returned);

            //=
            if (listreturned.Count > 0)
            {
                objRecord.VariousVoucherDetails = listreturned;
                Result = VariousVoucherMachinDAL.InsertUsingXML(objRecord, isNew);
            }
            return Result;
        }
        private void SaveOutOn()
        {
            #region Save Out On
            //Save Out On
            bool isNew = IsNewRecord;
            Stc_ManuFactoryCommendOutOnBail_Master objRecordOutOnMaster = new Stc_ManuFactoryCommendOutOnBail_Master();
            if (IsNewRecord)
                objRecordOutOnMaster.InvoiceID = Comon.cInt(Stc_ManuFactoryCommendOutOnBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID, 1));
            else
            {
                DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeFactoryBefore);
                if (dtInvoiceID.Rows.Count > 0)
                    objRecordOutOnMaster.InvoiceID = Comon.cInt(dtInvoiceID.Rows[0][0]);
                else
                {
                    objRecordOutOnMaster.InvoiceID = Comon.cInt(Stc_ManuFactoryCommendOutOnBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID, 1));
                    isNew = true;

                }
            } 
            objRecordOutOnMaster.CommandID = Comon.cInt(txtCommandID.Text);
            objRecordOutOnMaster.InvoiceDate = Comon.ConvertDateToSerial(txtCommandDate.Text);
            objRecordOutOnMaster.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecordOutOnMaster.FacilityID = UserInfo.FacilityID;
            objRecordOutOnMaster.CommandID = Comon.cInt(txtCommandID.Text);
            objRecordOutOnMaster.CurrencyID = Comon.cInt(MySession.GlobalDefaultCurencyID);
            objRecordOutOnMaster.ReferanceID = Comon.cInt(txtReferanceID.Text);
            objRecordOutOnMaster.TypeCommand = 1;
            objRecordOutOnMaster.DocumentType = DocumentTypeFactoryBefore;
            objRecordOutOnMaster.Cancel = 0;
            objRecordOutOnMaster.DebitAccount = Comon.cDbl(txtAccountIDFactory.Text);
            objRecordOutOnMaster.StoreID = Comon.cDbl(txtStoreIDFactory.Text);
            objRecordOutOnMaster.Notes = txtNotes.Text;
            objRecordOutOnMaster.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            //user Info
            objRecordOutOnMaster.UserID = UserInfo.ID;
            objRecordOutOnMaster.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecordOutOnMaster.RegTime = Comon.cDbl(Lip.GetServerTimeSerial());
            objRecordOutOnMaster.ComputerInfo = UserInfo.ComputerInfo;
            objRecordOutOnMaster.EditUserID = 0;
            objRecordOutOnMaster.EditTime = 0;
            objRecordOutOnMaster.EditDate = 0;
            objRecordOutOnMaster.EditComputerInfo = "";
            Stc_ManuFactoryCommendOutOnBail_Details returnedOutOn;
            List<Stc_ManuFactoryCommendOutOnBail_Details> listreturnedOutOn = new List<Stc_ManuFactoryCommendOutOnBail_Details>();
            for (int i = 0; i <= GridViewBeforfactory.DataRowCount - 1; i++)
            {
                returnedOutOn = new Stc_ManuFactoryCommendOutOnBail_Details();
                returnedOutOn.InvoiceID = objRecordOutOnMaster.InvoiceID;
                returnedOutOn.CommandID = Comon.cInt(txtCommandID.Text);
                returnedOutOn.FacilityID = UserInfo.FacilityID;
                returnedOutOn.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returnedOutOn.CommandDate = Comon.cDate(GridViewBeforfactory.GetRowCellValue(i, "DebitDate").ToString());
                returnedOutOn.CommandTime = (Comon.cDateTime(GridViewBeforfactory.GetRowCellValue(i, "DebitTime")).ToShortTimeString());
                returnedOutOn.BarCode = GridViewBeforfactory.GetRowCellValue(i, "BarCode").ToString();
                returnedOutOn.ItemID = Comon.cInt(GridViewBeforfactory.GetRowCellValue(i, "ItemID").ToString());
                returnedOutOn.SizeID = Comon.cInt(GridViewBeforfactory.GetRowCellValue(i, "SizeID").ToString());
                returnedOutOn.QTY = Comon.cDbl(GridViewBeforfactory.GetRowCellValue(i, "Debit").ToString());
                returnedOutOn.CostPrice = Comon.cDbl(0.0001.ToString());
                listreturnedOutOn.Add(returnedOutOn);
            }
            if (listreturnedOutOn.Count > 0)
            {
                objRecordOutOnMaster.CommandOutOnBailDatails = listreturnedOutOn;
                int Result = Stc_ManuFactoryCommendOutOnBailDAL.InsertUsingXML(objRecordOutOnMaster, isNew);
                if (Result > 0)
                {
                    //حفظ القيد الالي
                    long VoucherID = SaveVariousVoucherMachin(Comon.cInt(objRecordOutOnMaster.InvoiceID), isNew);
                    if (VoucherID == 0)
                        Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                    else
                        Lip.ExecututeSQL("Update " + Menu_FactoryRunCommandfactoryDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Menu_FactoryRunCommandfactoryDAL.PremaryKey + " = " + txtCommandID.Text);
                }
            }
            #endregion
        }
        private void SaveInOn()
        {
            #region Save Out On
            //Save Out On
            bool isNew = IsNewRecord;
            Stc_ManuFactoryCommendOutOnBail_Master objRecordInOnMaster = new Stc_ManuFactoryCommendOutOnBail_Master();
            if (IsNewRecord)
                objRecordInOnMaster.InvoiceID = Comon.cInt(Stc_ManuFactoryCommendOutOnBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID, 2));
            else
            {
                DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeFactoryAfter);
                if (dtInvoiceID.Rows.Count > 0)
                    objRecordInOnMaster.InvoiceID = Comon.cInt(dtInvoiceID.Rows[0][0]);
                else
                {
                    objRecordInOnMaster.InvoiceID = Comon.cInt(Stc_ManuFactoryCommendOutOnBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID, 2));
                    isNew = true;
                }
            }
            objRecordInOnMaster.CommandID = Comon.cInt(txtCommandID.Text);
            objRecordInOnMaster.InvoiceDate = Comon.ConvertDateToSerial(txtCommandDate.Text);
            objRecordInOnMaster.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecordInOnMaster.FacilityID = UserInfo.FacilityID;

            objRecordInOnMaster.CurrencyID = Comon.cInt(MySession.GlobalDefaultCurencyID);
            objRecordInOnMaster.ReferanceID = Comon.cInt(txtReferanceID.Text);
            objRecordInOnMaster.TypeCommand = 2;
            objRecordInOnMaster.DocumentType = DocumentTypeFactoryAfter;
            objRecordInOnMaster.Cancel = 0;
            objRecordInOnMaster.DebitAccount = Comon.cDbl(txtAccountIDFactory.Text);
            objRecordInOnMaster.StoreID = Comon.cDbl(txtStoreIDFactory.Text);
            objRecordInOnMaster.Notes = txtNotes.Text;
            objRecordInOnMaster.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            //user Info
            objRecordInOnMaster.UserID = UserInfo.ID;
            objRecordInOnMaster.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecordInOnMaster.RegTime = Comon.cDbl(Lip.GetServerTimeSerial());
            objRecordInOnMaster.ComputerInfo = UserInfo.ComputerInfo;
            objRecordInOnMaster.EditUserID = 0;
            objRecordInOnMaster.EditTime = 0;
            objRecordInOnMaster.EditDate = 0;
            objRecordInOnMaster.EditComputerInfo = "";
            Stc_ManuFactoryCommendOutOnBail_Details returnedOutOn;
            List<Stc_ManuFactoryCommendOutOnBail_Details> listreturnedOutOn = new List<Stc_ManuFactoryCommendOutOnBail_Details>();
            for (int i = 0; i <= GridViewAfterfactory.DataRowCount - 1; i++)
            {
                returnedOutOn = new Stc_ManuFactoryCommendOutOnBail_Details();
                returnedOutOn.InvoiceID = objRecordInOnMaster.InvoiceID;
                returnedOutOn.CommandID = Comon.cInt(txtCommandID.Text);
                returnedOutOn.FacilityID = UserInfo.FacilityID;
                returnedOutOn.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returnedOutOn.CommandDate = Comon.cDate(GridViewAfterfactory.GetRowCellValue(i, "DebitDate").ToString());
                returnedOutOn.CommandTime = (Comon.cDateTime(GridViewAfterfactory.GetRowCellValue(i, "DebitTime")).ToShortTimeString());
                returnedOutOn.BarCode = GridViewAfterfactory.GetRowCellValue(i, "BarCode").ToString();
                returnedOutOn.ItemID = Comon.cInt(GridViewAfterfactory.GetRowCellValue(i, "ItemID").ToString());
                returnedOutOn.SizeID = Comon.cInt(GridViewAfterfactory.GetRowCellValue(i, "SizeID").ToString());
                returnedOutOn.QTY = Comon.cDbl(GridViewAfterfactory.GetRowCellValue(i, "Credit").ToString());
                returnedOutOn.CostPrice = Comon.cDbl(0.0001.ToString());
                listreturnedOutOn.Add(returnedOutOn);
            }
            if (listreturnedOutOn.Count > 0)
            {
                objRecordInOnMaster.CommandOutOnBailDatails = listreturnedOutOn;
                int Result = Stc_ManuFactoryCommendOutOnBailDAL.InsertUsingXML(objRecordInOnMaster, isNew);
                if (Result > 0)
                {
                    //حفظ القيد الالي
                    long VoucherID = SaveVariousVoucherMachinInOn(Comon.cInt(objRecordInOnMaster.InvoiceID),isNew);
                    if (VoucherID == 0)
                        Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                    else
                        Lip.ExecututeSQL("Update " + Menu_FactoryRunCommandfactoryDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Menu_FactoryRunCommandfactoryDAL.PremaryKey + " = " + txtCommandID.Text);
                }
            }
            #endregion
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
            Stc_ItemsMoviing returned;
            List<Stc_ItemsMoviing> listreturned = new List<Stc_ItemsMoviing>();
            for (int i = 0; i <= GridViewBeforfactory.DataRowCount - 1; i++)
            {
                returned = new Stc_ItemsMoviing();
                returned.ID = i + 1;
                returned.MoveDate = Comon.ConvertDateToSerial(txtCommandDate.Text).ToString();
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
                returned.OutPrice = 0;
                //returned.Bones = Comon.cDbl(GridViewBeforfactory.GetRowCellValue(i, "Bones").ToString());
                //returned.OutPrice = Comon.cDbl(GridViewBeforfactory.GetRowCellValue(i, "TotalCost").ToString());
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
        private int SaveStockMoveingIn(int DocumentID)
        {
            Stc_ItemsMoviing objRecord = new Stc_ItemsMoviing();
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.DocumentTypeID = DocumentTypeFactoryAfter;
            objRecord.MoveType = 1;
            objRecord.MoveID = 0;
            objRecord.TranseID = DocumentID;
            Stc_ItemsMoviing returned;
            List<Stc_ItemsMoviing> listreturned = new List<Stc_ItemsMoviing>();
            for (int i = 0; i <= GridViewAfterfactory.DataRowCount - 1; i++)
            {
                returned = new Stc_ItemsMoviing();
                returned.ID = i + 1;
                returned.MoveDate = Comon.ConvertDateToSerial(txtCommandDate.Text).ToString();
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
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID));
                returned.QTY = Comon.cDbl(GridViewAfterfactory.GetRowCellValue(i, "Credit").ToString());
                //returned.InPrice = Comon.cDbl(GridViewAfterfactory.GetRowCellValue(i, "TotalCost").ToString());
                //returned.Bones = Comon.cDbl(GridCastingBefore.GetRowCellValue(i, "Bones").ToString());
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
        #endregion
         
        #region Save In ,Out Brntage
        long SaveVariousVoucherMachinBrntage(int DocumentID,bool isNew)
        {
            int VoucherID = 0;
            long Result = 0;
            Acc_VariousVoucherMachinMaster objRecord = new Acc_VariousVoucherMachinMaster();
            objRecord.DocumentType = DocumentTypeBrntageBefore;
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " And DocumentType=" + objRecord.DocumentType + " And BranchID=" + Comon.cInt(cmbBranchesID.EditValue)));

            objRecord.VoucherID = VoucherID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = MySession.GlobalFacilityID;
            //Date
            objRecord.VoucherDate = Comon.ConvertDateToSerial(txtCommandDate.Text).ToString();
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
            objRecord.Posted = 1;

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
            returned.AccountID = Comon.cDbl(txtAccountIDPrentage.Text);
            returned.VoucherID = VoucherID;
            double txtTotalQty_BrntageBefore = 0;
            for (int i = 0; i < GridViewBeforPrentag.DataRowCount; i++)
            {
                txtTotalQty_BrntageBefore += Comon.cDbl(GridViewBeforPrentag.GetRowCellValue(i, "PrentagDebit").ToString());
            }
            returned.DebitMatirial = Comon.cDbl(txtTotalQty_BrntageBefore);
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            listreturned.Add(returned);

            //Credit Matirial      
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 1;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cLong(txtStoreIDPrentage.Text);
            returned.VoucherID = VoucherID;
            returned.CreditMatirial = Comon.cDbl(txtTotalQty_BrntageBefore);
            returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            listreturned.Add(returned);

            //=
            if (listreturned.Count > 0)
            {
                objRecord.VariousVoucherDetails = listreturned;
                Result = VariousVoucherMachinDAL.InsertUsingXML(objRecord, isNew);
            }
            return Result;
        }
        long SaveVariousVoucherMachinInOnBrntage(int DocumentID,bool isNew)
        {
            int VoucherID = 0;
            long Result = 0;
            Acc_VariousVoucherMachinMaster objRecord = new Acc_VariousVoucherMachinMaster();
            objRecord.DocumentType = DocumentTypeBrntageAfter;
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " And DocumentType=" + objRecord.DocumentType + " And BranchID=" + Comon.cInt(cmbBranchesID.EditValue)));

            objRecord.VoucherID = VoucherID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = MySession.GlobalFacilityID;
            //Date
            objRecord.VoucherDate = Comon.ConvertDateToSerial(txtCommandDate.Text).ToString();
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
            objRecord.Posted = 1;

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
            returned.AccountID = Comon.cLong(txtStoreIDPrentage.Text);
            returned.VoucherID = VoucherID;
            double txtTotalQty_BrntageAfter = 0;
            for (int i = 0; i < GridViewAfterPrentag.DataRowCount; i++)
            {
                txtTotalQty_BrntageAfter += Comon.cDbl(GridViewAfterPrentag.GetRowCellValue(i, "PrentagCredit").ToString());
            }

            returned.DebitMatirial = Comon.cDbl(txtTotalQty_BrntageAfter);
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            listreturned.Add(returned);

            //Credit Matirial      
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 1;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(txtAccountIDPrentage.Text);
            returned.VoucherID = VoucherID;
            returned.CreditMatirial = Comon.cDbl(txtTotalQty_BrntageAfter);
            returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            listreturned.Add(returned);

            //=
            if (listreturned.Count > 0)
            {
                objRecord.VariousVoucherDetails = listreturned;
                Result = VariousVoucherMachinDAL.InsertUsingXML(objRecord, isNew);
            }
            return Result;
        }
        private void SaveOutOnBrntage()
        {
            #region Save Out On
            //Save Out On
            bool isNew = IsNewRecord;
            Stc_ManuFactoryCommendOutOnBail_Master objRecordOutOnMaster = new Stc_ManuFactoryCommendOutOnBail_Master();

            if (IsNewRecord)
                objRecordOutOnMaster.InvoiceID = Comon.cInt(Stc_ManuFactoryCommendOutOnBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID, 1));
            else
            {
                DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeBrntageBefore);
                if (dtInvoiceID.Rows.Count > 0)
                    objRecordOutOnMaster.InvoiceID = Comon.cInt(dtInvoiceID.Rows[0][0]);
                else
                {
                    objRecordOutOnMaster.InvoiceID = Comon.cInt(Stc_ManuFactoryCommendOutOnBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID, 1));
                    isNew = true;
                }
            }
            objRecordOutOnMaster.CommandID = Comon.cInt(txtCommandID.Text);
            objRecordOutOnMaster.InvoiceDate = Comon.ConvertDateToSerial(txtCommandDate.Text);
            objRecordOutOnMaster.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecordOutOnMaster.FacilityID = UserInfo.FacilityID;
            objRecordOutOnMaster.CommandID = Comon.cInt(txtCommandID.Text);
            objRecordOutOnMaster.CurrencyID = Comon.cInt(MySession.GlobalDefaultCurencyID);
            objRecordOutOnMaster.ReferanceID = Comon.cInt(txtReferanceID.Text);
            objRecordOutOnMaster.TypeCommand = 1;
            objRecordOutOnMaster.DocumentType = DocumentTypeBrntageBefore;
            objRecordOutOnMaster.Cancel = 0;
            objRecordOutOnMaster.DebitAccount = Comon.cDbl(txtAccountIDPrentage.Text);
            objRecordOutOnMaster.StoreID = Comon.cDbl(txtStoreIDFactory.Text);
            objRecordOutOnMaster.Notes = txtNotes.Text;
            objRecordOutOnMaster.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            //user Info
            objRecordOutOnMaster.UserID = UserInfo.ID;
            objRecordOutOnMaster.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecordOutOnMaster.RegTime = Comon.cDbl(Lip.GetServerTimeSerial());
            objRecordOutOnMaster.ComputerInfo = UserInfo.ComputerInfo;
            objRecordOutOnMaster.EditUserID = 0;
            objRecordOutOnMaster.EditTime = 0;
            objRecordOutOnMaster.EditDate = 0;
            objRecordOutOnMaster.EditComputerInfo = "";
            Stc_ManuFactoryCommendOutOnBail_Details returnedOutOn;
            List<Stc_ManuFactoryCommendOutOnBail_Details> listreturnedOutOn = new List<Stc_ManuFactoryCommendOutOnBail_Details>();
            for (int i = 0; i <= GridViewBeforPrentag.DataRowCount - 1; i++)
            {
                returnedOutOn = new Stc_ManuFactoryCommendOutOnBail_Details();
                returnedOutOn.InvoiceID = objRecordOutOnMaster.InvoiceID;
                returnedOutOn.CommandID = Comon.cInt(txtCommandID.Text);
                returnedOutOn.FacilityID = UserInfo.FacilityID;
                returnedOutOn.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returnedOutOn.CommandDate = Comon.cDate(GridViewBeforPrentag.GetRowCellValue(i, "PrentagDebitDate").ToString());
                returnedOutOn.CommandTime = (Comon.cDateTime(GridViewBeforPrentag.GetRowCellValue(i, "PrentagDebitTime")).ToShortTimeString());
                returnedOutOn.BarCode = GridViewBeforPrentag.GetRowCellValue(i, "BarcodePrentag").ToString();
                returnedOutOn.ItemID = Comon.cInt(GridViewBeforPrentag.GetRowCellValue(i, "ItemID").ToString());
                returnedOutOn.SizeID = Comon.cInt(GridViewBeforPrentag.GetRowCellValue(i, "SizeID").ToString());
                returnedOutOn.QTY = Comon.cDbl(GridViewBeforPrentag.GetRowCellValue(i, "PrentagDebit").ToString());
                returnedOutOn.CostPrice = Comon.cDbl(0.0001.ToString());
                listreturnedOutOn.Add(returnedOutOn);
            }
            if (listreturnedOutOn.Count > 0)
            {
                objRecordOutOnMaster.CommandOutOnBailDatails = listreturnedOutOn;
                int Result = Stc_ManuFactoryCommendOutOnBailDAL.InsertUsingXML(objRecordOutOnMaster, isNew);
                if (Result > 0)
                {
                    //حفظ القيد الالي
                    long VoucherID = SaveVariousVoucherMachinBrntage(Comon.cInt(objRecordOutOnMaster.InvoiceID),isNew);
                    if (VoucherID == 0)
                        Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                    else
                        Lip.ExecututeSQL("Update " + Menu_FactoryRunCommandPrentagAndPulishnDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Menu_FactoryRunCommandPrentagAndPulishnDAL.PremaryKey + " = " + txtCommandID.Text);
                }
            }
            #endregion
        }
        private void SaveInOnBrntage()
        {
            #region Save Out On
            //Save Out On
            bool isNew = IsNewRecord;
            Stc_ManuFactoryCommendOutOnBail_Master objRecordInOnMaster = new Stc_ManuFactoryCommendOutOnBail_Master();
            if (IsNewRecord)
                objRecordInOnMaster.InvoiceID = Comon.cInt(Stc_ManuFactoryCommendOutOnBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID, 2));
            else
            {
                DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeBrntageAfter);
                if (dtInvoiceID.Rows.Count > 0)
                    objRecordInOnMaster.InvoiceID = Comon.cInt(dtInvoiceID.Rows[0][0]);
                else
                {
                    objRecordInOnMaster.InvoiceID = Comon.cInt(Stc_ManuFactoryCommendOutOnBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID, 2));
                    isNew = true;
                }
            }
            objRecordInOnMaster.CommandID = Comon.cInt(txtCommandID.Text);
            objRecordInOnMaster.InvoiceDate = Comon.ConvertDateToSerial(txtCommandDate.Text);
            objRecordInOnMaster.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecordInOnMaster.FacilityID = UserInfo.FacilityID;

            objRecordInOnMaster.CurrencyID = Comon.cInt(MySession.GlobalDefaultCurencyID);
            objRecordInOnMaster.ReferanceID = Comon.cInt(txtReferanceID.Text);
            objRecordInOnMaster.TypeCommand = 2;
            objRecordInOnMaster.DocumentType = DocumentTypeBrntageAfter;
            objRecordInOnMaster.Cancel = 0;
            objRecordInOnMaster.DebitAccount = Comon.cDbl(txtAccountIDPrentage.Text);
            objRecordInOnMaster.StoreID = Comon.cDbl(txtStoreIDFactory.Text);
            objRecordInOnMaster.Notes = txtNotes.Text;
            objRecordInOnMaster.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            //user Info
            objRecordInOnMaster.UserID = UserInfo.ID;
            objRecordInOnMaster.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecordInOnMaster.RegTime = Comon.cDbl(Lip.GetServerTimeSerial());
            objRecordInOnMaster.ComputerInfo = UserInfo.ComputerInfo;
            objRecordInOnMaster.EditUserID = 0;
            objRecordInOnMaster.EditTime = 0;
            objRecordInOnMaster.EditDate = 0;
            objRecordInOnMaster.EditComputerInfo = "";
            Stc_ManuFactoryCommendOutOnBail_Details returnedOutOn;
            List<Stc_ManuFactoryCommendOutOnBail_Details> listreturnedOutOn = new List<Stc_ManuFactoryCommendOutOnBail_Details>();
            for (int i = 0; i <= GridViewAfterPrentag.DataRowCount - 1; i++)
            {
                returnedOutOn = new Stc_ManuFactoryCommendOutOnBail_Details();
                returnedOutOn.InvoiceID = objRecordInOnMaster.InvoiceID;
                returnedOutOn.CommandID = Comon.cInt(txtCommandID.Text);
                returnedOutOn.FacilityID = UserInfo.FacilityID;
                returnedOutOn.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returnedOutOn.CommandDate = Comon.cDate(GridViewAfterPrentag.GetRowCellValue(i, "PrentagDebitDate").ToString());
                returnedOutOn.CommandTime = (Comon.cDateTime(GridViewAfterPrentag.GetRowCellValue(i, "PrentagDebitTime")).ToShortTimeString());
                returnedOutOn.BarCode = GridViewAfterPrentag.GetRowCellValue(i, "BarcodePrentag").ToString();
                returnedOutOn.ItemID = Comon.cInt(GridViewAfterPrentag.GetRowCellValue(i, "ItemID").ToString());
                returnedOutOn.SizeID = Comon.cInt(GridViewAfterPrentag.GetRowCellValue(i, "SizeID").ToString());
                returnedOutOn.QTY = Comon.cDbl(GridViewAfterPrentag.GetRowCellValue(i, "PrentagCredit").ToString());
                returnedOutOn.CostPrice = Comon.cDbl(0.0001.ToString());
                listreturnedOutOn.Add(returnedOutOn);
            }
            if (listreturnedOutOn.Count > 0)
            {
                objRecordInOnMaster.CommandOutOnBailDatails = listreturnedOutOn;
                int Result = Stc_ManuFactoryCommendOutOnBailDAL.InsertUsingXML(objRecordInOnMaster, isNew);
                if (Result > 0)
                {
                    //حفظ القيد الالي
                    long VoucherID = SaveVariousVoucherMachinInOnBrntage(Comon.cInt(objRecordInOnMaster.InvoiceID),isNew);
                    if (VoucherID == 0)
                        Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                    else
                        Lip.ExecututeSQL("Update " + Menu_FactoryRunCommandPrentagAndPulishnDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Menu_FactoryRunCommandPrentagAndPulishnDAL.PremaryKey + " = " + txtCommandID.Text);
                }
            }
            #endregion
        }
        private int SaveStockMoveingBrntageOut(int DocumentID)
        {
            Stc_ItemsMoviing objRecord = new Stc_ItemsMoviing();
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.DocumentTypeID = DocumentTypeBrntageBefore;
            objRecord.MoveType = 2;
            objRecord.MoveID = 0;
            objRecord.TranseID = DocumentID;
            Stc_ItemsMoviing returned;
            List<Stc_ItemsMoviing> listreturned = new List<Stc_ItemsMoviing>();
            for (int i = 0; i <= GridViewBeforPrentag.DataRowCount - 1; i++)
            {
                returned = new Stc_ItemsMoviing();
                returned.ID = i + 1;
                returned.MoveDate = Comon.ConvertDateToSerial(txtCommandDate.Text).ToString();
                returned.MoveID = 0;
                returned.TranseID = DocumentID;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.DocumentTypeID = DocumentTypeBrntageBefore;
                returned.MoveType = 2;
                returned.StoreID = Comon.cDbl(txtStoreIDPrentage.Text.ToString());
                returned.AccountID = Comon.cDbl(txtAccountIDPrentage.Text);
                returned.BarCode = GridViewBeforPrentag.GetRowCellValue(i, "BarcodePrentag").ToString();
                returned.ItemID = Comon.cInt(GridViewBeforPrentag.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(GridViewBeforPrentag.GetRowCellValue(i, "SizeID").ToString());
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID));
                returned.QTY = Comon.cDbl(GridViewBeforPrentag.GetRowCellValue(i, "PrentagDebit").ToString());
                returned.InPrice = 0;
                returned.OutPrice = 0;
                //returned.Bones = Comon.cDbl(GridViewBeforfactory.GetRowCellValue(i, "Bones").ToString());
                //returned.OutPrice = Comon.cDbl(GridViewBeforfactory.GetRowCellValue(i, "TotalCost").ToString());
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
        private int SaveStockMoveingBrntageIn(int DocumentID)
        {
            Stc_ItemsMoviing objRecord = new Stc_ItemsMoviing();
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.DocumentTypeID = DocumentTypeBrntageAfter;
            objRecord.MoveType = 1;
            objRecord.MoveID = 0;
            objRecord.TranseID = DocumentID;
            Stc_ItemsMoviing returned;
            List<Stc_ItemsMoviing> listreturned = new List<Stc_ItemsMoviing>();
            for (int i = 0; i <= GridViewAfterPrentag.DataRowCount - 1; i++)
            {
                returned = new Stc_ItemsMoviing();
                returned.ID = i + 1;
                returned.MoveDate = Comon.ConvertDateToSerial(txtCommandDate.Text).ToString();
                returned.MoveID = 0;
                returned.TranseID = DocumentID;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.DocumentTypeID = DocumentTypeBrntageAfter;
                returned.MoveType = 1;
                returned.StoreID = Comon.cDbl(txtStoreIDPrentage.Text.ToString());
                returned.AccountID = Comon.cDbl(txtAccountIDPrentage.Text);
                returned.BarCode = GridViewAfterPrentag.GetRowCellValue(i, "BarcodePrentag").ToString();
                returned.ItemID = Comon.cInt(GridViewAfterPrentag.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(GridViewAfterPrentag.GetRowCellValue(i, "SizeID").ToString());
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID));
                returned.QTY = Comon.cDbl(GridViewAfterPrentag.GetRowCellValue(i, "PrentagCredit").ToString());
                //returned.InPrice = Comon.cDbl(GridViewAfterfactory.GetRowCellValue(i, "TotalCost").ToString());
                //returned.Bones = Comon.cDbl(GridCastingBefore.GetRowCellValue(i, "Bones").ToString());
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

        #endregion

        #region Save In ,Out Compound
        long SaveVariousVoucherMachinCompound(int DocumentID,bool isNew)
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
            objRecord.VoucherDate = Comon.ConvertDateToSerial(txtCommandDate.Text).ToString();
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
            objRecord.Posted = 1;

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
            returned.AccountID = Comon.cDbl(txtAccountIDBeforCompond.Text);
            returned.VoucherID = VoucherID;
            returned.DebitMatirial = Comon.cDbl(txtCompondGoldDebit.Text);
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            listreturned.Add(returned);
            //Credit Matirial      
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 1;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cLong(txtStoreIDBeforComond.Text);
            returned.VoucherID = VoucherID;
            returned.CreditMatirial = Comon.cDbl(txtCompondGoldDebit.Text);
            returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            listreturned.Add(returned);
            //=
            if (listreturned.Count > 0)
            {
                objRecord.VariousVoucherDetails = listreturned;
                Result = VariousVoucherMachinDAL.InsertUsingXML(objRecord, isNew);
            }
            return Result;
        }
        long SaveVariousVoucherMachinInOnCompound(int DocumentID,bool isNew)
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
            objRecord.VoucherDate = Comon.ConvertDateToSerial(txtCommandDate.Text).ToString();
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
            objRecord.Posted = 1;

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
            returned.AccountID = Comon.cLong(txtStoreIDBeforComond.Text);
            returned.VoucherID = VoucherID;
            returned.DebitMatirial = Comon.cDbl(txtCompoundGoldCredit.Text);
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            listreturned.Add(returned);

            //Credit Matirial      
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 1;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(txtAccountIDBeforCompond.Text);
            returned.VoucherID = VoucherID;
            returned.CreditMatirial = Comon.cDbl(txtCompoundGoldCredit.Text);
            returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            listreturned.Add(returned);

            //=
            if (listreturned.Count > 0)
            {
                objRecord.VariousVoucherDetails = listreturned;
                Result = VariousVoucherMachinDAL.InsertUsingXML(objRecord, isNew);
            }
            return Result;
        }
        private void SaveOutOnCompound()
        {
            #region Save Out On
            //Save Out On
            bool isNew = IsNewRecord;
            Stc_ManuFactoryCommendOutOnBail_Master objRecordOutOnMaster = new Stc_ManuFactoryCommendOutOnBail_Master();
            if (IsNewRecord)
                objRecordOutOnMaster.InvoiceID = Comon.cInt(Stc_ManuFactoryCommendOutOnBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID, 1));
            else
            {
                DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeCommpoundBefore);
                if (dtInvoiceID.Rows.Count > 0)
                    objRecordOutOnMaster.InvoiceID = Comon.cInt(dtInvoiceID.Rows[0][0]);
                else
                {
                    objRecordOutOnMaster.InvoiceID = Comon.cInt(Stc_ManuFactoryCommendOutOnBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID, 1));
                    isNew = true;

                }

            }
            objRecordOutOnMaster.CommandID = Comon.cInt(txtCommandID.Text);
            objRecordOutOnMaster.InvoiceDate = Comon.ConvertDateToSerial(txtCommandDate.Text);
            objRecordOutOnMaster.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecordOutOnMaster.FacilityID = UserInfo.FacilityID;
            objRecordOutOnMaster.CommandID = Comon.cInt(txtCommandID.Text);
            objRecordOutOnMaster.CurrencyID = Comon.cInt(MySession.GlobalDefaultCurencyID);
            objRecordOutOnMaster.ReferanceID = Comon.cInt(txtReferanceID.Text);
            objRecordOutOnMaster.TypeCommand = 1;
            objRecordOutOnMaster.DocumentType = DocumentTypeCommpoundBefore;
            objRecordOutOnMaster.Cancel = 0;
            objRecordOutOnMaster.DebitAccount = Comon.cDbl(txtAccountIDBeforCompond.Text);
            objRecordOutOnMaster.StoreID = Comon.cDbl(txtStoreIDBeforComond.Text);
            objRecordOutOnMaster.Notes = txtNotes.Text;
            objRecordOutOnMaster.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            //user Info
            objRecordOutOnMaster.UserID = UserInfo.ID;
            objRecordOutOnMaster.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecordOutOnMaster.RegTime = Comon.cDbl(Lip.GetServerTimeSerial());
            objRecordOutOnMaster.ComputerInfo = UserInfo.ComputerInfo;
            objRecordOutOnMaster.EditUserID = 0;
            objRecordOutOnMaster.EditTime = 0;
            objRecordOutOnMaster.EditDate = 0;
            objRecordOutOnMaster.EditComputerInfo = "";
            Stc_ManuFactoryCommendOutOnBail_Details returnedOutOn;
            List<Stc_ManuFactoryCommendOutOnBail_Details> listreturnedOutOn = new List<Stc_ManuFactoryCommendOutOnBail_Details>();
            for (int i = 0; i <= gridViewBeforCompond.DataRowCount - 1; i++)
            {
                returnedOutOn = new Stc_ManuFactoryCommendOutOnBail_Details();
                returnedOutOn.InvoiceID = objRecordOutOnMaster.InvoiceID;
                returnedOutOn.CommandID = Comon.cInt(txtCommandID.Text);
                returnedOutOn.FacilityID = UserInfo.FacilityID;
                returnedOutOn.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returnedOutOn.CommandDate = Comon.cDate(txtGivenDate.Text.ToString());
                returnedOutOn.CommandTime = (Comon.cDateTime(txtGivenTime.Text)).ToString();
                returnedOutOn.BarCode = gridViewBeforCompond.GetRowCellValue(i, "BarcodCompond").ToString();
                //returnedOutOn.ItemID = Comon.cInt(gridViewBeforCompond.GetRowCellValue(i, "ItemID").ToString());
                //returnedOutOn.SizeID = Comon.cInt(gridViewBeforCompond.GetRowCellValue(i, "SizeID").ToString());
                returnedOutOn.QTY = Comon.cDbl(gridViewBeforCompond.GetRowCellValue(i, "GoldDebit").ToString());
                returnedOutOn.CostPrice = Comon.cDbl(0.0001.ToString());
                listreturnedOutOn.Add(returnedOutOn);
            }
            if (listreturnedOutOn.Count > 0)
            {
                objRecordOutOnMaster.CommandOutOnBailDatails = listreturnedOutOn;
                int Result = Stc_ManuFactoryCommendOutOnBailDAL.InsertUsingXML(objRecordOutOnMaster, isNew);
                if (Result > 0)
                {
                    //حفظ القيد الالي
                    long VoucherID = SaveVariousVoucherMachinCompound(Comon.cInt(objRecordOutOnMaster.InvoiceID),isNew);
                    if (VoucherID == 0)
                        Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                    else
                        Lip.ExecututeSQL("Update " + Menu_FactoryRunCommandCompundDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Menu_FactoryRunCommandCompundDAL.PremaryKey + " = " + txtCommandID.Text);
                }
            }
            #endregion
        }
        private void SaveInOnCompound()
        {
            #region Save In On
            //Save Out On
            bool isNew = IsNewRecord;
            Stc_ManuFactoryCommendOutOnBail_Master objRecordInOnMaster = new Stc_ManuFactoryCommendOutOnBail_Master();
            if (IsNewRecord)
                objRecordInOnMaster.InvoiceID = Comon.cInt(Stc_ManuFactoryCommendOutOnBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID, 2));
            else
            {
                DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeCommpoundAfter);
                if (dtInvoiceID.Rows.Count > 0)
                    objRecordInOnMaster.InvoiceID = Comon.cInt(dtInvoiceID.Rows[0][0]);
                else
                {
                    objRecordInOnMaster.InvoiceID = Comon.cInt(Stc_ManuFactoryCommendOutOnBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID, 2));
                    isNew = true;
                }
            }
            objRecordInOnMaster.CommandID = Comon.cInt(txtCommandID.Text);
            objRecordInOnMaster.InvoiceDate = Comon.ConvertDateToSerial(txtCommandDate.Text);
            objRecordInOnMaster.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecordInOnMaster.FacilityID = UserInfo.FacilityID;
            
            objRecordInOnMaster.CurrencyID = Comon.cInt(MySession.GlobalDefaultCurencyID);
            objRecordInOnMaster.ReferanceID = Comon.cInt(txtReferanceID.Text);
            objRecordInOnMaster.TypeCommand = 2;
            objRecordInOnMaster.DocumentType = DocumentTypeCommpoundAfter;
            objRecordInOnMaster.Cancel = 0;
            objRecordInOnMaster.DebitAccount = Comon.cDbl(txtAccountIDBeforCompond.Text);
            objRecordInOnMaster.StoreID = Comon.cDbl(txtStoreIDBeforComond.Text);
            objRecordInOnMaster.Notes = txtNotes.Text;
            objRecordInOnMaster.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            //user Info
            objRecordInOnMaster.UserID = UserInfo.ID;
            objRecordInOnMaster.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecordInOnMaster.RegTime = Comon.cDbl(Lip.GetServerTimeSerial());
            objRecordInOnMaster.ComputerInfo = UserInfo.ComputerInfo;
            objRecordInOnMaster.EditUserID = 0;
            objRecordInOnMaster.EditTime = 0;
            objRecordInOnMaster.EditDate = 0;
            objRecordInOnMaster.EditComputerInfo = "";
            Stc_ManuFactoryCommendOutOnBail_Details returnedOutOn;
            List<Stc_ManuFactoryCommendOutOnBail_Details> listreturnedOutOn = new List<Stc_ManuFactoryCommendOutOnBail_Details>();
            for (int i = 0; i <= gridViewAfterCompond.DataRowCount - 1; i++)
            {
                returnedOutOn = new Stc_ManuFactoryCommendOutOnBail_Details();
                returnedOutOn.InvoiceID = objRecordInOnMaster.InvoiceID;
                returnedOutOn.CommandID = Comon.cInt(txtCommandID.Text);
                returnedOutOn.FacilityID = UserInfo.FacilityID;
                returnedOutOn.BranchID = Comon.cInt(cmbBranchesID.EditValue); 
                returnedOutOn.CommandDate = Comon.cDate(txtGivenDate.Text.ToString());
                returnedOutOn.CommandTime = (Comon.cDateTime(txtGivenTime.Text)).ToString();
                returnedOutOn.BarCode = gridViewAfterCompond.GetRowCellValue(i, "BarcodCompond").ToString();
                //returnedOutOn.ItemID = Comon.cInt(gridViewAfterCompond.GetRowCellValue(i, "ItemID").ToString());
                //returnedOutOn.SizeID = Comon.cInt(gridViewAfterCompond.GetRowCellValue(i, "SizeID").ToString());
                returnedOutOn.QTY = Comon.cDbl(gridViewAfterCompond.GetRowCellValue(i, "GoldCredit").ToString());
                returnedOutOn.CostPrice = Comon.cDbl(0.0001.ToString());
                listreturnedOutOn.Add(returnedOutOn);
            }
            if (listreturnedOutOn.Count > 0)
            {
                objRecordInOnMaster.CommandOutOnBailDatails = listreturnedOutOn;
                int Result = Stc_ManuFactoryCommendOutOnBailDAL.InsertUsingXML(objRecordInOnMaster, isNew);
                if (Result > 0)
                {
                    //حفظ القيد الالي
                    long VoucherID = SaveVariousVoucherMachinInOnCompound(Comon.cInt(objRecordInOnMaster.InvoiceID),isNew);
                    if (VoucherID == 0)
                        Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                    else
                        Lip.ExecututeSQL("Update " + Menu_FactoryRunCommandCompundDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Menu_FactoryRunCommandCompundDAL.PremaryKey + " = " + txtCommandID.Text);
                }
            }
            #endregion
        }
        private int SaveStockMoveingCommpoundOut(int DocumentID)
        {
            Stc_ItemsMoviing objRecord = new Stc_ItemsMoviing();
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.DocumentTypeID = DocumentTypeCommpoundBefore;
            objRecord.MoveType = 2;
            objRecord.MoveID = 0;
            objRecord.TranseID = DocumentID;
            Stc_ItemsMoviing returned;
            List<Stc_ItemsMoviing> listreturned = new List<Stc_ItemsMoviing>();
            for (int i = 0; i <= gridViewBeforCompond.DataRowCount - 1; i++)
            {
                returned = new Stc_ItemsMoviing();
                returned.ID = i + 1;
                returned.MoveDate = Comon.ConvertDateToSerial(txtCommandDate.Text).ToString();
                returned.MoveID = 0;
                returned.TranseID = DocumentID;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.DocumentTypeID = DocumentTypeCommpoundBefore;
                returned.MoveType = 2;
                returned.StoreID = Comon.cDbl(txtStoreIDBeforComond.Text.ToString());
                returned.AccountID = Comon.cDbl(txtAccountIDBeforCompond.Text);
                returned.BarCode = gridViewBeforCompond.GetRowCellValue(i, "BarcodCompond").ToString();
                returned.ItemID = Comon.cInt(gridViewBeforCompond.GetRowCellValue(i, "ItemID").ToString());
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID));
                returned.SizeID = Comon.cInt(gridViewBeforCompond.GetRowCellValue(i, "SizeID").ToString());
                returned.QTY = Comon.cDbl(gridViewBeforCompond.GetRowCellValue(i, "GoldDebit").ToString());
                returned.InPrice = 0;
                returned.OutPrice = 0;
                //returned.Bones = Comon.cDbl(GridViewBeforfactory.GetRowCellValue(i, "Bones").ToString());
                //returned.OutPrice = Comon.cDbl(GridViewBeforfactory.GetRowCellValue(i, "TotalCost").ToString());
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
        private int SaveStockMoveingCommpoundIn(int DocumentID)
        {
            Stc_ItemsMoviing objRecord = new Stc_ItemsMoviing();
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.DocumentTypeID = DocumentTypeCommpoundAfter;
            objRecord.MoveType = 1;
            objRecord.MoveID = 0;
            objRecord.TranseID = DocumentID;
            Stc_ItemsMoviing returned;
            List<Stc_ItemsMoviing> listreturned = new List<Stc_ItemsMoviing>();
            for (int i = 0; i <= gridViewAfterCompond.DataRowCount - 1; i++)
            {
                returned = new Stc_ItemsMoviing();
                returned.ID = i + 1;
                returned.MoveDate = Comon.ConvertDateToSerial(txtCommandDate.Text).ToString();
                returned.MoveID = 0;
                returned.TranseID = DocumentID;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.DocumentTypeID = DocumentTypeCommpoundAfter;
                returned.MoveType = 1;
                returned.StoreID = Comon.cDbl(txtStoreIDBeforComond.Text.ToString());
                returned.AccountID = Comon.cDbl(txtAccountIDBeforCompond.Text);
                returned.BarCode = gridViewAfterCompond.GetRowCellValue(i, "BarcodCompond").ToString();
                returned.ItemID = Comon.cInt(gridViewAfterCompond.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(gridViewAfterCompond.GetRowCellValue(i, "SizeID").ToString());
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID));
                returned.QTY = Comon.cDbl(gridViewAfterCompond.GetRowCellValue(i, "GoldCredit").ToString());
                //returned.InPrice = Comon.cDbl(GridViewAfterfactory.GetRowCellValue(i, "TotalCost").ToString());
                //returned.Bones = Comon.cDbl(GridCastingBefore.GetRowCellValue(i, "Bones").ToString());
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
        #endregion

        #region Save In,Out Polishn
        long SaveVariousVoucherMachinPolshin(int DocumentID,bool isNew)
        {
            int VoucherID = 0;
            long Result = 0;
            Acc_VariousVoucherMachinMaster objRecord = new Acc_VariousVoucherMachinMaster();
            objRecord.DocumentType = DocumentTypePloshinBefore;
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " And DocumentType=" + objRecord.DocumentType + " And BranchID=" + Comon.cInt(cmbBranchesID.EditValue)));

            objRecord.VoucherID = VoucherID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = MySession.GlobalFacilityID;
            //Date
            objRecord.VoucherDate = Comon.ConvertDateToSerial(txtCommandDate.Text).ToString();
            objRecord.CurrencyID =   Comon.cInt(MySession.GlobalDefaultCurencyID);
            //objRecord.CurrencyName = cmbCurency.Text.ToString();
            //objRecord.CurrencyPrice = Comon.cDec(txtCurrncyPrice.Text);
            //objRecord.CurrencyEquivalent = Comon.cDec(lblcurrncyEquvilant.Text);

            //objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);
            // objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
            // objRecord.DelegateID = Comon.cInt(txtDelegateID.Text);
            objRecord.Notes = txtNotes.Text == "" ? this.Text : txtNotes.Text;
            objRecord.DocumentID = DocumentID;
            objRecord.Cancel = 0;
            objRecord.Posted = 1;

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
            returned.AccountID = Comon.cDbl(txtAccountIDPolishing.Text);
            returned.VoucherID = VoucherID;
            double txtTotalQty_PolshinBefore = 0;
            for (int i = 0; i < GridViewBeforPolish.DataRowCount; i++)
            {
                txtTotalQty_PolshinBefore += Comon.cDbl(GridViewBeforPolish.GetRowCellValue(i, "Debit").ToString());
            }
            returned.DebitMatirial = Comon.cDbl(txtTotalQty_PolshinBefore);
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            listreturned.Add(returned);

            //Credit Matirial      
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 1;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cLong(txtStoreIDPolishing.Text);
            returned.VoucherID = VoucherID;
            returned.CreditMatirial = Comon.cDbl(txtTotalQty_PolshinBefore);
            returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            listreturned.Add(returned);

            //=
            if (listreturned.Count > 0)
            {
                objRecord.VariousVoucherDetails = listreturned;
                Result = VariousVoucherMachinDAL.InsertUsingXML(objRecord, isNew);
            }
            return Result;
        }
        long SaveVariousVoucherMachinInOnPolshin(int DocumentID,bool isNew)
        {
            int VoucherID = 0;
            long Result = 0;
            Acc_VariousVoucherMachinMaster objRecord = new Acc_VariousVoucherMachinMaster();
            objRecord.DocumentType = DocumentTypePolshinAfter;
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " And DocumentType=" + objRecord.DocumentType + " And BranchID=" + Comon.cInt(cmbBranchesID.EditValue)));

            objRecord.VoucherID = VoucherID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = MySession.GlobalFacilityID;
            //Date
            objRecord.VoucherDate = Comon.ConvertDateToSerial(txtCommandDate.Text).ToString();
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
            objRecord.Posted = 1;

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
            returned.AccountID = Comon.cLong(txtStoreIDPolishing.Text);
            returned.VoucherID = VoucherID;
            double txtTotalQty_PolshinAfter = 0;
            for (int i = 0; i < GridViewAfterPolish.DataRowCount; i++)
            {
                txtTotalQty_PolshinAfter += Comon.cDbl(GridViewAfterPolish.GetRowCellValue(i, "Credit").ToString());
            }
            returned.DebitMatirial = Comon.cDbl(txtTotalQty_PolshinAfter);
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            listreturned.Add(returned);

            //Credit Matirial      
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 1;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(txtAccountIDPolishing.Text);
            returned.VoucherID = VoucherID;
            returned.CreditMatirial = Comon.cDbl(txtTotalQty_PolshinAfter);
            returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            listreturned.Add(returned);

            //=
            if (listreturned.Count > 0)
            {
                objRecord.VariousVoucherDetails = listreturned;
                Result = VariousVoucherMachinDAL.InsertUsingXML(objRecord, isNew);
            }
            return Result;
        }
        private void SaveOutOnPolshin()
        {
            #region Save Out On
            //Save Out On
            bool isNew = IsNewRecord;
            Stc_ManuFactoryCommendOutOnBail_Master objRecordOutOnMaster = new Stc_ManuFactoryCommendOutOnBail_Master();
            if (IsNewRecord)
                objRecordOutOnMaster.InvoiceID = Comon.cInt(Stc_ManuFactoryCommendOutOnBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID, 1));
            else
            {
                DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypePloshinBefore);
                if (dtInvoiceID.Rows.Count > 0)
                    objRecordOutOnMaster.InvoiceID = Comon.cInt(dtInvoiceID.Rows[0][0]);
                else
                {
                    objRecordOutOnMaster.InvoiceID = Comon.cInt(Stc_ManuFactoryCommendOutOnBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID, 1));
                    isNew = true;

                }

            }
            objRecordOutOnMaster.CommandID = Comon.cInt(txtCommandID.Text);
            objRecordOutOnMaster.InvoiceDate = Comon.ConvertDateToSerial(txtCommandDate.Text);
            objRecordOutOnMaster.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecordOutOnMaster.FacilityID = UserInfo.FacilityID;
            objRecordOutOnMaster.CommandID = Comon.cInt(txtCommandID.Text);
            objRecordOutOnMaster.CurrencyID =   Comon.cInt(MySession.GlobalDefaultCurencyID);
            objRecordOutOnMaster.ReferanceID = Comon.cInt(txtReferanceID.Text);
            objRecordOutOnMaster.TypeCommand = 1;
            objRecordOutOnMaster.DocumentType = DocumentTypePloshinBefore;
            objRecordOutOnMaster.Cancel = 0;
            objRecordOutOnMaster.DebitAccount = Comon.cDbl(txtAccountIDPolishing.Text);
            objRecordOutOnMaster.StoreID = Comon.cDbl(txtStoreIDPolishing.Text);
            objRecordOutOnMaster.Notes = txtNotes.Text;
            objRecordOutOnMaster.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            //user Info
            objRecordOutOnMaster.UserID = UserInfo.ID;
            objRecordOutOnMaster.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecordOutOnMaster.RegTime = Comon.cDbl(Lip.GetServerTimeSerial());
            objRecordOutOnMaster.ComputerInfo = UserInfo.ComputerInfo;
            objRecordOutOnMaster.EditUserID = 0;
            objRecordOutOnMaster.EditTime = 0;
            objRecordOutOnMaster.EditDate = 0;
            objRecordOutOnMaster.EditComputerInfo = "";
            Stc_ManuFactoryCommendOutOnBail_Details returnedOutOn;
            List<Stc_ManuFactoryCommendOutOnBail_Details> listreturnedOutOn = new List<Stc_ManuFactoryCommendOutOnBail_Details>();
            for (int i = 0; i <= GridViewBeforPolish.DataRowCount - 1; i++)
            {
                returnedOutOn = new Stc_ManuFactoryCommendOutOnBail_Details();
                returnedOutOn.InvoiceID = objRecordOutOnMaster.InvoiceID;
                returnedOutOn.CommandID = Comon.cInt(txtCommandID.Text);
                returnedOutOn.FacilityID = UserInfo.FacilityID;
                returnedOutOn.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returnedOutOn.CommandDate = Comon.cDate(GridViewBeforPolish.GetRowCellValue(i, "DebitDate").ToString());
                returnedOutOn.CommandTime = (Comon.cDateTime(GridViewBeforPolish.GetRowCellValue(i, "DebitTime")).ToShortTimeString());
                returnedOutOn.BarCode = GridViewBeforPolish.GetRowCellValue(i, "BarcodeTalmee").ToString();
                returnedOutOn.ItemID = Comon.cInt(GridViewBeforPolish.GetRowCellValue(i, "ItemID").ToString());
                returnedOutOn.SizeID = Comon.cInt(GridViewBeforPolish.GetRowCellValue(i, "SizeID").ToString());
                returnedOutOn.QTY = Comon.cDbl(GridViewBeforPolish.GetRowCellValue(i, "Debit").ToString());
                returnedOutOn.CostPrice = Comon.cDbl(0.0001.ToString());
                listreturnedOutOn.Add(returnedOutOn);
            }
            if (listreturnedOutOn.Count > 0)
            {
                objRecordOutOnMaster.CommandOutOnBailDatails = listreturnedOutOn;
                int Result = Stc_ManuFactoryCommendOutOnBailDAL.InsertUsingXML(objRecordOutOnMaster, isNew);
                if (Result > 0)
                {
                    //حفظ القيد الالي
                    long VoucherID = SaveVariousVoucherMachinPolshin(Comon.cInt(objRecordOutOnMaster.InvoiceID),isNew);
                    if (VoucherID == 0)
                        Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                    else
                        Lip.ExecututeSQL("Update " + Menu_FactoryRunCommandTalmeeDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Menu_FactoryRunCommandTalmeeDAL.PremaryKey + " = " + txtCommandID.Text);
                }
            }
            #endregion
        }
        private void SaveInOnPolshin()
        {
            #region Save Out On
            //Save Out On
            bool isNew = IsNewRecord;
            Stc_ManuFactoryCommendOutOnBail_Master objRecordInOnMaster = new Stc_ManuFactoryCommendOutOnBail_Master();
            if (IsNewRecord)
                objRecordInOnMaster.InvoiceID = Comon.cInt(Stc_ManuFactoryCommendOutOnBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID, 2));
            else
            {
                DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypePolshinAfter);
                if (dtInvoiceID.Rows.Count > 0)
                    objRecordInOnMaster.InvoiceID = Comon.cInt(dtInvoiceID.Rows[0][0]);
                else
                {
                    objRecordInOnMaster.InvoiceID = Comon.cInt(Stc_ManuFactoryCommendOutOnBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID, 2));
                    isNew = true;
                }
            }
            objRecordInOnMaster.CommandID = Comon.cInt(txtCommandID.Text);
            objRecordInOnMaster.InvoiceDate = Comon.ConvertDateToSerial(txtCommandDate.Text);
            objRecordInOnMaster.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecordInOnMaster.FacilityID = UserInfo.FacilityID;

            objRecordInOnMaster.CurrencyID =   Comon.cInt(MySession.GlobalDefaultCurencyID);
            objRecordInOnMaster.ReferanceID = Comon.cInt(txtReferanceID.Text);
            objRecordInOnMaster.TypeCommand = 2;
            objRecordInOnMaster.DocumentType = DocumentTypePolshinAfter;
            objRecordInOnMaster.Cancel = 0;
            objRecordInOnMaster.DebitAccount = Comon.cDbl(txtAccountIDPolishing.Text);
            objRecordInOnMaster.StoreID = Comon.cDbl(txtStoreIDPolishing.Text);
            objRecordInOnMaster.Notes = txtNotes.Text;
            objRecordInOnMaster.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            //user Info
            objRecordInOnMaster.UserID = UserInfo.ID;
            objRecordInOnMaster.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecordInOnMaster.RegTime = Comon.cDbl(Lip.GetServerTimeSerial());
            objRecordInOnMaster.ComputerInfo = UserInfo.ComputerInfo;
            objRecordInOnMaster.EditUserID = 0;
            objRecordInOnMaster.EditTime = 0;
            objRecordInOnMaster.EditDate = 0;
            objRecordInOnMaster.EditComputerInfo = "";
            Stc_ManuFactoryCommendOutOnBail_Details returnedOutOn;
            List<Stc_ManuFactoryCommendOutOnBail_Details> listreturnedOutOn = new List<Stc_ManuFactoryCommendOutOnBail_Details>();
            for (int i = 0; i <= GridViewAfterPolish.DataRowCount - 1; i++)
            {
                returnedOutOn = new Stc_ManuFactoryCommendOutOnBail_Details();
                returnedOutOn.InvoiceID = objRecordInOnMaster.InvoiceID;
                returnedOutOn.CommandID = Comon.cInt(txtCommandID.Text);
                returnedOutOn.FacilityID = UserInfo.FacilityID;
                returnedOutOn.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returnedOutOn.CommandDate = Comon.cDate(GridViewAfterPolish.GetRowCellValue(i, "DebitDate").ToString());
                returnedOutOn.CommandTime = (Comon.cDateTime(GridViewAfterPolish.GetRowCellValue(i, "DebitTime")).ToShortTimeString());
                returnedOutOn.BarCode = GridViewAfterPolish.GetRowCellValue(i, "BarcodeTalmee").ToString();
                returnedOutOn.ItemID = Comon.cInt(GridViewAfterPolish.GetRowCellValue(i, "ItemID").ToString());
                returnedOutOn.SizeID = Comon.cInt(GridViewAfterPolish.GetRowCellValue(i, "SizeID").ToString());
                returnedOutOn.QTY = Comon.cDbl(GridViewAfterPolish.GetRowCellValue(i, "Credit").ToString());
                returnedOutOn.CostPrice = Comon.cDbl(0.0001.ToString());
                listreturnedOutOn.Add(returnedOutOn);
            }
            if (listreturnedOutOn.Count > 0)
            {
                objRecordInOnMaster.CommandOutOnBailDatails = listreturnedOutOn;
                int Result = Stc_ManuFactoryCommendOutOnBailDAL.InsertUsingXML(objRecordInOnMaster, isNew);
                if (Result > 0)
                {
                    //حفظ القيد الالي
                    long VoucherID = SaveVariousVoucherMachinInOnPolshin(Comon.cInt(objRecordInOnMaster.InvoiceID),isNew);
                    if (VoucherID == 0)
                        Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                    else
                        Lip.ExecututeSQL("Update " + Menu_FactoryRunCommandTalmeeDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Menu_FactoryRunCommandTalmeeDAL.PremaryKey + " = " + txtCommandID.Text);
                }
            }
            #endregion
        }

        private int SaveStockMoveingPolshinOut(int DocumentID)
        {
            Stc_ItemsMoviing objRecord = new Stc_ItemsMoviing();
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.DocumentTypeID = DocumentTypePloshinBefore;
            objRecord.MoveType = 2;
            objRecord.MoveID = 0;
            objRecord.TranseID = DocumentID;
            Stc_ItemsMoviing returned;
            List<Stc_ItemsMoviing> listreturned = new List<Stc_ItemsMoviing>();
            for (int i = 0; i <= GridViewBeforPolish.DataRowCount - 1; i++)
            {
                returned = new Stc_ItemsMoviing();
                returned.ID = i + 1;
                returned.MoveDate = Comon.ConvertDateToSerial(txtCommandDate.Text).ToString();
                returned.MoveID = 0;
                returned.TranseID = DocumentID;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.DocumentTypeID = DocumentTypePloshinBefore;
                returned.MoveType = 2;
                returned.StoreID = Comon.cDbl(txtStoreIDPolishing.Text.ToString());
                returned.AccountID = Comon.cDbl(txtAccountIDPolishing.Text);
                returned.BarCode = GridViewBeforPolish.GetRowCellValue(i, "BarcodeTalmee").ToString();
                returned.ItemID = Comon.cInt(GridViewBeforPolish.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(GridViewBeforPolish.GetRowCellValue(i, "SizeID").ToString());
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID));
                returned.QTY = Comon.cDbl(GridViewBeforPolish.GetRowCellValue(i, "Debit").ToString());
                returned.InPrice = 0;
                returned.OutPrice = 0;
                //returned.Bones = Comon.cDbl(GridViewBeforfactory.GetRowCellValue(i, "Bones").ToString());
                //returned.OutPrice = Comon.cDbl(GridViewBeforfactory.GetRowCellValue(i, "TotalCost").ToString());
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
        private int SaveStockMoveingPolshinIn(int DocumentID)
        {
            Stc_ItemsMoviing objRecord = new Stc_ItemsMoviing();
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.DocumentTypeID = DocumentTypePolshinAfter;
            objRecord.MoveType = 1;
            objRecord.MoveID = 0;
            objRecord.TranseID = DocumentID;
            Stc_ItemsMoviing returned;
            List<Stc_ItemsMoviing> listreturned = new List<Stc_ItemsMoviing>();
            for (int i = 0; i <= GridViewAfterPolish.DataRowCount - 1; i++)
            {
                returned = new Stc_ItemsMoviing();
                returned.ID = i + 1;
                returned.MoveDate = Comon.ConvertDateToSerial(txtCommandDate.Text).ToString();
                returned.MoveID = 0;
                returned.TranseID = DocumentID;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.DocumentTypeID = DocumentTypePolshinAfter;
                returned.MoveType = 1;
                returned.StoreID = Comon.cDbl(txtStoreIDPolishing.Text.ToString());
                returned.AccountID = Comon.cDbl(txtAccountIDPolishing.Text);
                returned.BarCode = GridViewAfterPolish.GetRowCellValue(i, "BarcodeTalmee").ToString();
                returned.ItemID = Comon.cInt(GridViewAfterPolish.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(GridViewAfterPolish.GetRowCellValue(i, "SizeID").ToString());
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID));
                returned.QTY = Comon.cDbl(GridViewAfterPolish.GetRowCellValue(i, "Credit").ToString());
                //returned.InPrice = Comon.cDbl(GridViewAfterfactory.GetRowCellValue(i, "TotalCost").ToString());
                //returned.Bones = Comon.cDbl(GridCastingBefore.GetRowCellValue(i, "Bones").ToString());
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
        #endregion 

        int DeleteStockMoving(int DocumentID, int DocumentType)
        {
            int Result = -1;
            Stc_ItemsMoviing objRecord = new Stc_ItemsMoviing();
            objRecord.DocumentTypeID = DocumentType;
            objRecord.TranseID = DocumentID;
            objRecord.BranchID = UserInfo.BRANCHID;
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
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " And DocumentType=" + objRecord.DocumentType + " And BranchID=" + Comon.cInt(UserInfo.BRANCHID)));
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
        //هذه الدالة لحذف امر الصرف او التوريد من الارشيف الخاص باوامر الصرف والتوريد الخاصة بالتصنيع
        int DeleteInOnOROutOnBil(int DocumentID, int DocumentType)
        {
            int Result = 0;
            Stc_ManuFactoryCommendOutOnBail_Master objRecord = new Stc_ManuFactoryCommendOutOnBail_Master();
            objRecord.InvoiceID = DocumentID;
            objRecord.DocumentType = DocumentType;
            //objRecord.TypeCommand = TypeCommand;
            objRecord.BranchID = UserInfo.BRANCHID;
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
                 model.BranchID = UserInfo.BRANCHID;
                 model.FacilityID = UserInfo.FacilityID;

                 string Result = Menu_FactoryRunCommandMasterDAL.Delete(model).ToString();
                 //حذف الحركة المخزنية 
                 if (Comon.cInt(Result) > 0)
                 {
                     int MoveID = 0;
                       MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text), DocumentTypeFactoryBefore);                    
                       MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text), DocumentTypeFactoryAfter);
                       MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text),DocumentTypeBrntageBefore);
                       MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text),DocumentTypeBrntageAfter);
                       MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text), DocumentTypeCommpoundBefore);
                       MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text),DocumentTypeCommpoundAfter);
                       MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text), DocumentTypePloshinBefore);
                       MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text), DocumentTypePolshinAfter);
                     if (MoveID <0)
                         Messages.MsgError(Messages.TitleInfo, "خطا في حذف الحركة  المخزنية");
                 }

                 #region Delete Voucher Machin
                 //حذف القيد الالي
                 if (Comon.cInt(Result) > 0)
                 {
                     int VoucherID = 0;
                     

                     DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeFactoryBefore);
                     if (dtInvoiceID.Rows.Count > 0)
                     {
                         VoucherID = DeleteVariousVoucherMachin(Comon.cInt(dtInvoiceID.Rows[0][0]), DocumentTypeFactoryBefore);
                         if (VoucherID == 0)
                             Messages.MsgError(Messages.TitleInfo, "خطا في حذف قيد العملية تصنيع - قبل ");
                     }
                     int VoucherIDAfter = 0;
                     DataTable dtInvoiceIDAfter = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeFactoryAfter);
                     if (dtInvoiceIDAfter.Rows.Count > 0)
                     {
                         VoucherIDAfter = DeleteVariousVoucherMachin(Comon.cInt(dtInvoiceIDAfter.Rows[0][0]), DocumentTypeFactoryAfter);
                         if (VoucherIDAfter == 0)
                             Messages.MsgError(Messages.TitleInfo, "خطا في حذف قيد العملية تصنيع -بعد");
                     }

                     int VoucherIDBrntageBrfore = 0;
                     DataTable dtInvoiceIDBrntage = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeBrntageBefore);
                     if (dtInvoiceIDBrntage.Rows.Count > 0)
                     {
                         VoucherIDBrntageBrfore = DeleteVariousVoucherMachin(Comon.cInt(dtInvoiceIDBrntage.Rows[0][0]), DocumentTypeBrntageBefore);
                         if (VoucherIDBrntageBrfore == 0)
                             Messages.MsgError(Messages.TitleInfo, "خطا في حذف قيد العملية برنتاج-قبل");
                     }
                     int VoucherIDBrntageAfter = 0;
                     DataTable dtInvoiceIDBrntageAfter = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeBrntageAfter);
                     if (dtInvoiceIDBrntageAfter.Rows.Count > 0)
                     {
                         VoucherIDBrntageAfter = DeleteVariousVoucherMachin(Comon.cInt(dtInvoiceIDBrntageAfter.Rows[0][0]), DocumentTypeBrntageAfter);
                         if (VoucherIDBrntageAfter == 0)
                             Messages.MsgError(Messages.TitleInfo, "خطا في حذف قيد العملية برنتاج-بعد");
                     }
                     int VoucherIDCompoundBefore = 0;
                     DataTable dtInvoiceIDCompoundBefore = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeCommpoundBefore);
                     if (dtInvoiceIDCompoundBefore.Rows.Count > 0)
                     {
                         VoucherIDCompoundBefore = DeleteVariousVoucherMachin(Comon.cInt(dtInvoiceIDCompoundBefore.Rows[0][0]), DocumentTypeCommpoundBefore);
                         if (VoucherIDCompoundBefore == 0)
                             Messages.MsgError(Messages.TitleInfo, "خطا في حذف قيد العملية تركيب-قبل");
                     }
                     int VoucherIDCompoundAfter = 0;
                     DataTable dtInvoiceIDCompoundAfter = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeCommpoundAfter);
                     if (dtInvoiceIDCompoundAfter.Rows.Count > 0)
                     {
                         VoucherIDCompoundAfter = DeleteVariousVoucherMachin(Comon.cInt(dtInvoiceIDCompoundAfter.Rows[0][0]), DocumentTypeCommpoundAfter);
                         if (VoucherIDCompoundAfter == 0)
                             Messages.MsgError(Messages.TitleInfo, "خطا في حذف قيد العملية تركيب-بعد");
                     }
                     int VoucherIDPolishnBefore = 0;
                     DataTable dtInvoiceIDPolishBefore = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypePloshinBefore);
                     if (dtInvoiceIDPolishBefore.Rows.Count > 0)
                     {
                         VoucherIDPolishnBefore = DeleteVariousVoucherMachin(Comon.cInt(dtInvoiceIDPolishBefore.Rows[0][0]), DocumentTypePloshinBefore);
                         if (VoucherIDPolishnBefore == 0)
                             Messages.MsgError(Messages.TitleInfo, "خطا في حذف قيد العملية تلميع-قبل");
                     }
                     int VoucherIDPolishbAfter = 0;
                     DataTable dtInvoiceIDPolishnAfter = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypePolshinAfter);
                     if (dtInvoiceIDPolishnAfter.Rows.Count > 0)
                     {
                         VoucherIDPolishbAfter = DeleteVariousVoucherMachin(Comon.cInt(dtInvoiceIDPolishnAfter.Rows[0][0]), DocumentTypePolshinAfter);
                         if (VoucherIDPolishbAfter == 0)
                             Messages.MsgError(Messages.TitleInfo, "خطا في حذف قيد العملية تلميع-بعد");
                     }
                 }
                 #endregion

                 #region Delete Stock IN Or Out From archive
                 //حذف التوريد والصرف من الارشيف
                 if (Comon.cInt(Result) > 0)
                 {
                     int OutFactoryID = 0;                    
                     DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeFactoryBefore);
                     if (dtInvoiceID.Rows.Count > 0)
                     {
                         OutFactoryID = DeleteInOnOROutOnBil(Comon.cInt(dtInvoiceID.Rows[0][0]), DocumentTypeFactoryBefore);
                         if (OutFactoryID == 0)
                             Messages.MsgError(Messages.TitleInfo, "خطا في حذف الصرف من الارشيف للعلية تصنيع- قبل  ");
                     }
                     int InFactoryID = 0;
                     DataTable dtInvoiceIDAfter = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeFactoryAfter);
                     if (dtInvoiceIDAfter.Rows.Count > 0)
                     {
                         InFactoryID = DeleteInOnOROutOnBil(Comon.cInt(dtInvoiceIDAfter.Rows[0][0]), DocumentTypeFactoryAfter);
                         if (InFactoryID == 0)
                             Messages.MsgError(Messages.TitleInfo, "خطا في حذف التوريد من الارشيف للعملية تصنيع- بعد ");
                     }

                     int OutBrntageID = 0;
                     DataTable dtInvoiceIDBrntageBefor = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeBrntageBefore);
                     if (dtInvoiceIDBrntageBefor.Rows.Count > 0)
                     {
                         OutBrntageID = DeleteInOnOROutOnBil(Comon.cInt(dtInvoiceIDBrntageBefor.Rows[0][0]), DocumentTypeBrntageBefore);
                         if (OutBrntageID == 0)
                             Messages.MsgError(Messages.TitleInfo, "خطا في حذف الصرف من الارشيف للعلية برنتاج- قبل  ");
                     }
                     int InBrntageID = 0;
                     DataTable dtInvoiceIDBrntageAfter = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeBrntageAfter);
                     if (dtInvoiceIDBrntageAfter.Rows.Count > 0)
                     {
                         InBrntageID = DeleteInOnOROutOnBil(Comon.cInt(dtInvoiceIDBrntageAfter.Rows[0][0]), DocumentTypeBrntageAfter);
                         if (InBrntageID == 0)
                             Messages.MsgError(Messages.TitleInfo, "خطا في حذف التوريد من الارشيف للعملية برنتاج- بعد ");
                     }

                     int OutCompundID = 0;
                     DataTable dtInvoiceIDCompundBefor = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeCommpoundBefore);
                     if (dtInvoiceIDCompundBefor.Rows.Count > 0)
                     {
                         OutCompundID = DeleteInOnOROutOnBil(Comon.cInt(dtInvoiceIDCompundBefor.Rows[0][0]), DocumentTypeCommpoundBefore);
                         if (OutCompundID == 0)
                             Messages.MsgError(Messages.TitleInfo, "خطا في حذف الصرف من الارشيف للعلية تركيب- قبل  ");
                     }
                     int InCompundID = 0;
                     DataTable dtInvoiceIDCompundAfter = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeCommpoundAfter);
                     if (dtInvoiceIDCompundAfter.Rows.Count > 0)
                     {
                         InCompundID = DeleteInOnOROutOnBil(Comon.cInt(dtInvoiceIDCompundAfter.Rows[0][0]), DocumentTypeCommpoundAfter);
                         if (InCompundID == 0)
                             Messages.MsgError(Messages.TitleInfo, "خطا في حذف التوريد من الارشيف للعملية تركيب- بعد ");
                     }

                     int OutPolishnID = 0;
                     DataTable dtInvoiceIDPolishnBefor = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypePloshinBefore);
                     if (dtInvoiceIDPolishnBefor.Rows.Count > 0)
                     {
                         OutPolishnID = DeleteInOnOROutOnBil(Comon.cInt(dtInvoiceIDPolishnBefor.Rows[0][0]), DocumentTypePloshinBefore);
                         if (OutPolishnID == 0)
                             Messages.MsgError(Messages.TitleInfo, "خطا في حذف الصرف من الارشيف للعلية تلميع- قبل  ");
                     }
                     int InPolishnID = 0;
                     DataTable dtInvoiceIDPolishnAfter = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypePolshinAfter);
                     if (dtInvoiceIDPolishnAfter.Rows.Count > 0)
                     {
                         InPolishnID = DeleteInOnOROutOnBil(Comon.cInt(dtInvoiceIDPolishnAfter.Rows[0][0]), DocumentTypePolshinAfter);
                         if (InPolishnID == 0)
                             Messages.MsgError(Messages.TitleInfo, "خطا في حذف التوريد من الارشيف للعملية تلميع- بعد ");
                     }
                 }
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
                lblCustomerName.Text = "";
                txtTypeName.Text = "";

                txtBarCode.Text = "";
                txtBrandID.Text = "";
                txtSpendAmount.Text = "0";
                txtCountStonesNumin.Text = "";
                txtCompondGoldDebit.Text = "";
                txtCompoundGoldCredit.Text = "";
                txtCountStonesNumLos.Text = "";
                txtCountStonesNumOut.Text = "";
                txtCountStonesNumCom.Text = "";
                txtCustomerID.Text = "";
                txtEmpIDFactor.Text = "";
                txtEmplooyID.Text = "";
                txtEmplooyIDPolishing.Text = "";
                txtEmployeeStokIDFactory.Text = "";
                txtGoldWeight.Text = "";
                txtGrossWeight.Text = "";
                txtGroupID.Text = "";

                txtNetGold.Text = "";
                txtNotes.Text = "";
                txtOpretionID.Text = "";
                txtItemName.Text = "";

                txtThefactoriID.Text = "";
                txtTotallosed.Text = "";
                txtTypeID.Text = "";
                txtWeightStonesin.Text = "";
                txtWeightStonesLos.Text = "";
                txtWeightStonesOut.Text = "";
                txtWeightStonesCom.Text = "";
                lblEmpNameFactor.Text = "";
                lblEmplooyName.Text = "";
                txtEmployeeStokNameFactory.Text = "";
                txtGroupName.Text = "";
                lblEmpolyeePolishingName.Text = "";
                txtBrandName.Text = "";

                txtGoldWeight.Text = "0";
                txtNetGold.Text = "0";
                txtTotallosed.Text = "0";
                txtGrossWeight.Text = "0";

                txtCompondGoldDebit.Text = "0";
                txtCompoundGoldCredit.Text = "0";
                txtCountStonesNumin.Text = "0";
                txtWeightStonesin.Text = "0";
                txtCountStonesNumOut.Text = "0";
                txtWeightStonesOut.Text = "0";
                txtCountStonesNumLos.Text = "0";
                txtWeightStonesLos.Text = "0";
                txtCountStonesNumCom.Text = "0";
                txtWeightStonesCom.Text = "0";

                //الحسابات
                txtAccountIDFactory.Text = "";
                txtStoreIDFactory.Text = "";
                txtEmployeeStokIDFactory.Text = "";
                txtEmpIDFactor.Text = "";


                txtAccountIDPrentage.Text = "";
                txtStoreIDPrentage.Text = "";
                txtEmployeeStokIDPrentage.Text = "";
                txtEmpIDPrentage.Text = "";

                txtAccountIDBeforCompond.Text = "";
                txtStoreIDBeforComond.Text = "";
                txtEmployeeStokIDBeforCompond.Text = "";
                txtEmpIDBeforCompond.Text = "";
                txtDelegateID.Text = "";
                lblDelegateName.Text = "";

                txtAccountIDAdditions.Text = "";
                txtStoreIDAdditions.Text = "";
                txtEmployeeStokIDAdditions.Text = "";
                txtEmpIDAdditions.Text = "";

                txtAccountIDPolishing.Text = "";
                txtStoreIDPolishing.Text = "";
                txtEmployeeStokIDPolishing.Text = "";
                txtEmplooyIDPolishing.Text = "";

                txtAccountIDBarcodeItem.Text =  "";
                txtStoreIDBarcod.Text = "";
                txtEmployeeStokIDBarcode.Text =  "";

                lblAccountNameFactory.Text = "";
                lblEmployeeStokName.Text = "";
                lblStoreNameFactory.Text = "";
                lblEmpNameFactor.Text = "";


                lblAccountNamePrentage.Text = "";
                lblStoreNamePrentage.Text = "";
                lblEmployeeStokName.Text = "";
                lblEmpNamePrentage.Text = "";
 
                lblAccountNameBeforCompond.Text = "";
                lblStoreNameBeforCompond.Text = "";
                lblEmployeeStokBeforCompond.Text = "";
                lblEmpNameFactor.Text = "";

                lblEmpNameBeforCompond.Text = ""; 
                lblAccountNameAdditions.Text = "";
                lblStoreNameAdditions.Text = "";
                lblEmployeeNameStokAdditions.Text = "";
                lblEmpNameAdditions.Text = "";
                lblAccountNamePolishin.Text = "";
                lblStoreNamePolishin.Text = "";
                lblEmployeeNameStokPolishin.Text = "";
                lblEmpolyeePolishingName.Text = "";
                lblAccountNameBarcodeItem.Text = "";
                lblStoreNameProducts.Text = "";
                lblEmployeeNameStokBarcodeItem.Text = "";
                //

                //جريد فيو
                initGridBeforPrentage();
                initGridAfterPrentage();
                initGridBeforCompent();
                initGridAfterCompent();
                initGridBeforTalmee();
                initGridAfterTalmee();
                initGridSelver();
                initGridFactory();
                initGridAfterFactory();
                initGridProductionExpenses();
                initGridAlcadZircone();
                initGridCostDaimond();
                initGridCodeing();
                //pictureEdit1.Image = null;
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

        private void GridViewBeforPrentag_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {

            if (e.Column.FieldName != "PrSignature")
            {
                e.Graphics.FillRectangle(e.Cache.GetSolidBrush(Color.Beige), e.Bounds);
                e.Graphics.DrawRectangle(e.Cache.GetPen(Color.Black, 1), e.Bounds);
                e.Appearance.DrawString(e.Cache, e.DisplayText, e.Bounds);
                e.Handled = true;
                GridViewBeforPrentag.Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;
                GridViewAfterPrentag.Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;
                 
            }
        }

        GridHitInfo info = null;

        int pressedRowHandle = GridControl.InvalidRowHandle;
        int highlightedRowHandle = GridControl.InvalidRowHandle;

        public int PressedRowHandle
        {
            get { return pressedRowHandle; }
            set
            {
                if (pressedRowHandle != GridControl.InvalidRowHandle)
                {
                    int rowHandle = pressedRowHandle;
                    pressedRowHandle = GridControl.InvalidRowHandle;
                    GridViewBeforPrentag.InvalidateRowCell(rowHandle, GridViewBeforPrentag.Columns["CompanyName"]);
                }
                pressedRowHandle = value;
                GridViewBeforPrentag.InvalidateRowCell(pressedRowHandle, GridViewBeforPrentag.Columns["CompanyName"]);
            }
        }

        public int HighlightedRowHandle
        {
            get { return highlightedRowHandle; }
            set
            {
                if (highlightedRowHandle == value)
                    return;
                if (highlightedRowHandle != GridControl.InvalidRowHandle)
                {
                    int rowHandle = highlightedRowHandle;
                    highlightedRowHandle = GridControl.InvalidRowHandle;
                    GridViewBeforPrentag.InvalidateRowCell(rowHandle, GridViewBeforPrentag.Columns["PrSignature"]);
                }
                else
                {
                    highlightedRowHandle = value;
                    PressedRowHandle = GridControl.InvalidRowHandle;
                }
                GridViewBeforPrentag.InvalidateRowCell(highlightedRowHandle, GridViewBeforPrentag.Columns["PrSignature"]);
            }
        }
        protected ObjectState GetObjectState(int rowHandle)
        {
            if (rowHandle == pressedRowHandle)
                return ObjectState.Pressed;
            else
               if (rowHandle == HighlightedRowHandle)
                return ObjectState.Hot;
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
        private bool IsMouseOverButton(int rowHandle, Point point)
        {
            GridViewInfo info = GridViewBeforPrentag.GetViewInfo() as GridViewInfo;
            GridCellInfo cellInfo = info.GetGridCellInfo(rowHandle, GridViewBeforPrentag.Columns["PrSignature"]);
            Rectangle cellBounds = cellInfo.Bounds;
            return cellBounds.Contains(point);
        }
        protected virtual void OnButtonClick(int RowHandle)
        {
            if(RowHandle>=0)
            Text += GridViewBeforPrentag.GetRowCellValue(RowHandle, "MachinID").ToString();

        }

        private void GridViewBeforPrentag_MouseUp(object sender, MouseEventArgs e)
        { 
            if (PressedRowHandle != GridControl.InvalidRowHandle)
            {
                
            }
        }

        private void gridControl1_MouseDown(object sender, MouseEventArgs e)
        {

             
            if (HighlightedRowHandle != GridControl.InvalidRowHandle)
            {
                PressedRowHandle = HighlightedRowHandle;
                OnButtonClick(PressedRowHandle);
                PressedRowHandle = GridControl.InvalidRowHandle;
            }
            
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
            if (info.InRowCell && info.Column.FieldName == "PrSignature" && IsMouseOverButton(info.RowHandle, new Point(e.X, e.Y)))
                HighlightedRowHandle = info.RowHandle;
            else
                HighlightedRowHandle = GridControl.InvalidRowHandle;
        }

        private void label61_Click(object sender, EventArgs e)
        {

        }

        private void txtItemID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as  Name FROM Stc_Items WHERE ItemID =" + txtItemID.Text + " And Cancel =0 ";
                CSearch.ControlValidating(txtItemID, txtItemName, strSQL);
                
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void txtDelegateID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as DelegateName FROM Sales_SalesDelegate WHERE DelegateID =" + txtDelegateID.Text + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
                CSearch.ControlValidating(txtDelegateID, lblDelegateName, strSQL);
                 
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void txtEmplooyID_Validating(object sender, CancelEventArgs e)
        {
            try
            { 
                strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtEmplooyID.Text) + " And Cancel =0 ";
                CSearch.ControlValidating(txtEmplooyID, lblEmplooyName, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID

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
                  
                strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtEmpIDFactor.Text) + " And Cancel =0 ";
                CSearch.ControlValidating(txtEmpIDFactor, lblEmpNameFactor, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void txtEmpIDPrentage_Validating(object sender, CancelEventArgs e)
        {
            try
            { 
                strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtEmpIDPrentage.Text) + " And Cancel =0 ";
                CSearch.ControlValidating(txtEmpIDPrentage, lblEmpNamePrentage, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void txtEmpIDBeforCompond_Validating(object sender, CancelEventArgs e)
        {
            try
            { 

                strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtEmpIDBeforCompond.Text) + " And Cancel =0 ";
                CSearch.ControlValidating(txtEmpIDBeforCompond, lblEmpNameBeforCompond, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

      

        private void txtEmpIDAdditions_Validating(object sender, CancelEventArgs e)
        {
            try
            { 
                strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtEmpIDAdditions.Text) + " And Cancel =0 ";
                CSearch.ControlValidating(txtEmpIDAdditions, lblEmpNameAdditions, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void txtEmplooyIDPolishing_Validating(object sender, CancelEventArgs e)
        {
            try
            { 
                strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtEmplooyIDPolishing.Text) + " And Cancel =0 ";
                CSearch.ControlValidating(txtEmplooyIDPolishing, lblEmpolyeePolishingName, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void txtAccountIDPrentage_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT ArbName as AccountName FROM Acc_Accounts WHERE AccountID =" + Comon.cDbl(txtAccountIDPrentage.Text) + " And Cancel =0 ";
                CSearch.ControlValidating(txtAccountIDPrentage, lblAccountNamePrentage, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID

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
                strSQL = "SELECT ArbName as AccountName FROM Acc_Accounts WHERE AccountID =" + Comon.cDbl(txtAccountIDFactory.Text) + " And Cancel =0 ";
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
                strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE AccountID =" + Comon.cLong(txtStoreIDFactory.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(UserInfo.BRANCHID);
                CSearch.ControlValidating(txtStoreIDFactory, lblStoreNameFactory, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void txtStoreIDPrentage_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE AccountID =" + Comon.cLong(txtStoreIDPrentage.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(UserInfo.BRANCHID);
                CSearch.ControlValidating(txtStoreIDPrentage, lblStoreNamePrentage, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void txtStoreIDBeforComond_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE AccountID =" + Comon.cDbl(txtStoreIDBeforComond.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(UserInfo.BRANCHID);
                CSearch.ControlValidating(txtStoreIDBeforComond, lblStoreNameBeforCompond, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        //private void txtStoreIDAfterComond_Validating(object sender, CancelEventArgs e)
        //{
        //    try
        //    {
        //        strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(txtStoreIDAfterComond.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(UserInfo.BRANCHID);
        //        CSearch.ControlValidating(txtStoreIDAfterComond, lblStoreNameAfterCompond, strSQL);
        //    }
        //    catch (Exception ex)
        //    {
        //        Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
        //    }
        //}

        private void txtStoreIDAdditions_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(txtStoreIDAdditions.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(UserInfo.BRANCHID);
                CSearch.ControlValidating(txtStoreIDAdditions, lblStoreNameAdditions, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void txtStoreIDPolishing_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE AccountID =" + Comon.cLong(txtStoreIDPolishing.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(UserInfo.BRANCHID);
                CSearch.ControlValidating(txtStoreIDPolishing, lblStoreNamePolishin, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void txtStoreIDProducts_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(txtStoreIDBarcod.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(UserInfo.BRANCHID);
                CSearch.ControlValidating(txtStoreIDBarcod, lblStoreNameProducts, strSQL);
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

                strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtEmployeeStokIDFactory.Text) + " And Cancel =0 ";
                CSearch.ControlValidating(txtEmployeeStokIDFactory, txtEmployeeStokNameFactory, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void txtEmployeeStokIDPrentage_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtEmployeeStokIDPrentage.Text) + " And Cancel =0 ";
                CSearch.ControlValidating(txtEmployeeStokIDPrentage, lblEmployeeStokName, strSQL); 

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void txtEmployeeStokIDBeforCompond_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtEmployeeStokIDBeforCompond.Text) + " And Cancel =0 ";
                CSearch.ControlValidating(txtEmployeeStokIDBeforCompond, lblEmployeeStokBeforCompond, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

         

        private void txtEmployeeStokIDAdditions_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtEmployeeStokIDAdditions.Text) + " And Cancel =0 ";
                CSearch.ControlValidating(txtEmployeeStokIDAdditions, lblEmployeeNameStokAdditions, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void txtEmployeeStokIDPolishing_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtEmployeeStokIDPolishing.Text) + " And Cancel =0 ";
                CSearch.ControlValidating(txtEmployeeStokIDPolishing, lblEmployeeNameStokPolishin, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void txtAccountIDBeforCompond_Validating(object sender, CancelEventArgs e)
        {
            try
            {                 
                strSQL = "SELECT ArbName as AccountName FROM Acc_Accounts WHERE AccountID =" + Comon.cDbl(txtAccountIDBeforCompond.Text) + " And Cancel =0 ";
                CSearch.ControlValidating(txtAccountIDBeforCompond, lblAccountNameBeforCompond, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

         

        private void txtAccountIDAdditions_Validating(object sender, CancelEventArgs e)
        {
            try
            { 
                strSQL = "SELECT ArbName as AccountName FROM Acc_Accounts WHERE AccountID =" + Comon.cDbl(txtAccountIDAdditions.Text) + " And Cancel =0 ";
                CSearch.ControlValidating(txtAccountIDAdditions, lblAccountNameAdditions, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID


            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void txtAccountIDPolishing_Validating(object sender, CancelEventArgs e)
        {
            try
            { 
                strSQL = "SELECT ArbName as AccountName FROM Acc_Accounts WHERE AccountID =" + Comon.cDbl(txtAccountIDPolishing.Text) + " And Cancel =0 ";
                CSearch.ControlValidating(txtAccountIDPolishing, lblAccountNamePolishin, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void txtAccountIDBarcodeItem_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT ArbName as AccountName FROM Acc_Accounts WHERE AccountID =" + Comon.cDbl(txtAccountIDBarcodeItem.Text) + " And Cancel =0 ";
                CSearch.ControlValidating(txtAccountIDBarcodeItem, lblAccountNameBarcodeItem, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void txtEmployeeStokIDBarcode_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtEmployeeStokIDBarcode.Text) + " And Cancel =0 ";
                CSearch.ControlValidating(txtEmployeeStokIDBarcode, lblEmployeeNameStokPolishin, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void GridViewBeforfactory_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
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

        private void gridViewBeforCompond_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName != "ComSignature")
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
 

        private void btnMachinResractionFactoryAfter_Click(object sender, EventArgs e)
        {
            if (IsNewRecord == true) return;
            DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeFactoryAfter);
            if (dtInvoiceID.Rows.Count > 0)
            {
                int ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + dtInvoiceID.Rows[0][0] + " And DocumentType=" + DocumentTypeFactoryAfter).ToString());
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
        }

        private void btnMachinResractionFactoryBefore_Click(object sender, EventArgs e)
        {
            if (IsNewRecord == true) return;
            DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeFactoryBefore);
            if (dtInvoiceID.Rows.Count > 0)
            {
                int ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + dtInvoiceID.Rows[0][0] + " And DocumentType=" + DocumentTypeFactoryBefore).ToString());
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
        }

        private void btnMachinResractionBrntageBefore_Click(object sender, EventArgs e)
        {
            if (IsNewRecord == true) return;
            DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeBrntageBefore);
            if (dtInvoiceID.Rows.Count > 0)
            {
                int ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + dtInvoiceID.Rows[0][0].ToString() + " And DocumentType=" + DocumentTypeBrntageBefore).ToString());
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
        }

        private void btnMachinResractionBrntageAfter_Click(object sender, EventArgs e)
        {
            if (IsNewRecord == true) return;
            DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeBrntageAfter);
            if (dtInvoiceID.Rows.Count > 0)
            {
                int ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + dtInvoiceID.Rows[0][0] + " And DocumentType=" + DocumentTypeBrntageAfter).ToString());
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
        }

        private void btnMachinResractionPolishnBefore_Click(object sender, EventArgs e)
        {
            if (IsNewRecord == true) return;
            DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypePloshinBefore);
            if (dtInvoiceID.Rows.Count > 0)
            {
                int ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + dtInvoiceID.Rows[0][0] + " And DocumentType=" + DocumentTypePloshinBefore).ToString());
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
        }

        private void btnMachinResractionPolishnAfter_Click(object sender, EventArgs e)
        {
            if (IsNewRecord == true) return;
            DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypePolshinAfter);
            if (dtInvoiceID.Rows.Count > 0)
            {
                int ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + dtInvoiceID.Rows[0][0] + " And DocumentType=" + DocumentTypePolshinAfter).ToString());
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
        }

        private void btnMachinResractionCommpondBefore_Click(object sender, EventArgs e)
        {
            if (IsNewRecord == true) return;
            DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeCommpoundBefore);
            if (dtInvoiceID.Rows.Count > 0)
            {
                int ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + dtInvoiceID.Rows[0][0] + " And DocumentType=" + DocumentTypeCommpoundBefore).ToString());
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
        }

        private void btnMachinResractionCompondAfter_Click(object sender, EventArgs e)
        {
            if (IsNewRecord == true) return;
            DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeCommpoundAfter);
            if (dtInvoiceID.Rows.Count > 0)
            {
                int ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + dtInvoiceID.Rows[0][0] + " And DocumentType=" + DocumentTypeCommpoundAfter).ToString());
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
        }

         
    }


}
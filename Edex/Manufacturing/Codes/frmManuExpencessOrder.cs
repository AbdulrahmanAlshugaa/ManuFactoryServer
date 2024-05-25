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
using DevExpress.XtraEditors.Repository;
using Edex.Model;
using Edex.Model.Language;
using Edex.ModelSystem;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using Edex.GeneralObjects.GeneralClasses;
using  DevExpress.Utils;
using Edex.DAL; 
using DevExpress.XtraSplashScreen;
using Edex.DAL.ManufacturingDAL;
using Edex.DAL.SalseSystem.Stc_itemDAL;
using System.IO;
using DevExpress.CodeParser;
using Microsoft.Office.Interop.Excel;
using DevExpress.XtraGrid;
using System.Globalization;
using DataTable = System.Data.DataTable;
using Edex.DAL.Accounting;
using Edex.AccountsObjects.Transactions;
using Edex.AccountsObjects.Codes;

namespace Edex.Manufacturing.Codes
{
   
    public partial class frmManuExpencessOrder : BaseForm
    {
        #region 
        string FocusedControl = "";

        public int DocumentTypeCommandCost = 48;
        private string PrimaryName;
        BindingList<Manu_ProductionExpensesDetails> lstDetailProductionExpenses = new BindingList<Manu_ProductionExpensesDetails>();
        BindingList<Manu_ProductionExpensesDetails> lstDetailFixedExpenses = new BindingList<Manu_ProductionExpensesDetails>();
        BindingList<Manu_ProductionExpensesDetails> lstDetailEstimatedExpenses = new BindingList<Manu_ProductionExpensesDetails>();

        BindingList<Menu_ProductionExpensesAcconts> lstDetailAccountsParent = new BindingList<Menu_ProductionExpensesAcconts>();


        BindingList<Manu_OrderRestriction> lstDetailOrders = new BindingList<Manu_OrderRestriction>();
        public System.Data.DataTable _sampleData = new System.Data.DataTable();
        public System.Data.DataTable _sampleDataCustomer = new System.Data.DataTable();
        private Menu_ProductionExpensesMasterDAL cClass;
        private string strSQL = "";
        private bool IsNewRecord;
        private System.Data.DataTable dt;
        private System.Data.DataTable dt1;
        private System.Data.DataTable dt2;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        public CultureInfo culture = new CultureInfo("en-US");
        public bool HasColumnErrors = false;

        #endregion
        public frmManuExpencessOrder()
        {
            InitializeComponent();

            PrimaryName = "ArbName";
            if (UserInfo.Language == iLanguage.English)
            {
                PrimaryName = "EngName";
            }
     
            this.GridProductionExpenses.CustomDrawCell += GridProductionExpenses_CustomDrawCell;
            this.gridView1.CustomDrawCell += GridProductionExpenses_CustomDrawCell;
            this.gridView6.CustomDrawCell += GridProductionExpenses_CustomDrawCell;
            this.gridView3.CustomDrawCell += GridProductionExpenses_CustomDrawCell;
            this.gridControlProductionExpenses.ProcessGridKey += GridControlProductionExpenses_ProcessGridKey;
            this.gridControl1.ProcessGridKey += GridControl1_ProcessGridKey;
            this.gridControl2.ProcessGridKey += GridControl2_ProcessGridKey;
            this.gridControl4.ProcessGridKey += gridControl4_ProcessGridKey;
            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
            FillCombo.FillComboBoxLookUpEdit(cmbCategoryOrders, "Manu_CatogiryExpenss", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
            cmbCategoryOrders.EditValue = 1;
            cmbBranchesID.EditValue = MySession.GlobalBranchID;
            cmbBranchesID.ReadOnly = !MySession.GlobalAllowBranchModificationAllScreens;
            FillCombo.FillComboBox(cmbTypeOrders, "Manu_TypeOrders", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
            FillCombo.FillComboBox(cmbCurency, "Acc_Currency", "ID", PrimaryName, "", "BranchID="+MySession.GlobalBranchID, (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
            FillCombo.FillComboBox(cmbStatus, "Manu_TypeStatus", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
            cmbStatus.EditValue = MySession.GlobalDefaultProcessPostedStatus;

            _sampleData.Columns.Add(new DataColumn("Debit", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("Credit", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("AccountID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("AccountName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Evaluation", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("Notes", typeof(string)));
            _sampleDataCustomer.Columns.Add(new DataColumn("Debit", typeof(decimal)));
            _sampleDataCustomer.Columns.Add(new DataColumn("Credit", typeof(decimal)));
            _sampleDataCustomer.Columns.Add(new DataColumn("AccountID", typeof(string)));
            _sampleDataCustomer.Columns.Add(new DataColumn("AccountName", typeof(string)));
            _sampleDataCustomer.Columns.Add(new DataColumn("Evaluation", typeof(decimal)));
            _sampleDataCustomer.Columns.Add(new DataColumn("Notes", typeof(string)));

            txtCastingID.Validating += TxtCastingID_Validating;
            txtCommandID.Validating += TxtCommandID_Validating;
            GridProductionExpenses.RowUpdated += GridProductionExpenses_RowUpdated;
            gridView3.RowUpdated += GridProductionExpenses_RowUpdated;
            gridView1.RowUpdated += GridProductionExpenses_RowUpdated;
            txtQTYOrders.Validating += TxtQTYOrders_Validating;
            txtToDate.Validating += TxtToDate_Validating;
            txtFromDate.Validating += TxtToDate_Validating;
            txtOrderID.Validating+=txtOrderID_Validating;
            txtCostCenterID.Validating+=txtCostCenterID_Validating;
            txtCreditAccountID.Validating+=txtCreditAccountID_Validating;
            txtDebitAccountID.Validating+=txtDebitAccountID_Validating;
            
            this.gridView1.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView1_ValidatingEditor);
            this.gridView3.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView1_ValidatingEditor);
            this.gridView8.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView8_ValidatingEditor);
        }

        void gridControl4_ProcessGridKey(object sender, KeyEventArgs e)
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
                    if (this.gridView8.ActiveEditor is CheckEdit)
                    {
                        view.SetFocusedValue(!Comon.cbool(view.GetFocusedValue()));
                        //  CalculateRow(GridCastingAfter.FocusedRowHandle, Comon.cbool(view.GetFocusedValue()));
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

                    }
                }
                //else if (e.KeyData == Keys.Delete)
                //{
                //    if (!IsNewRecord)
                //    {
                //        bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, Messages.msgConfirmDelete);
                //        if (!Yes)
                //            return;
                //    }
                //    int index = view.FocusedRowHandle;
                //    string AccountParent = "";
                //    for (int i = 0; i < GridProductionExpenses.DataRowCount; i++)
                //    {
                //        AccountParent=Lip.GetValue(" SELECT ParentAccountID   FROM Acc_Accounts WHERE Cancel=0  And BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + " AND AccountLevel=" + MySession.GlobalNoOfLevels + " and AccountID=" + Comon.cDbl(GridProductionExpenses.GetRowCellValue(i,"AccountID")));
                //        if (AccountParent == view.GetRowCellValue(index, "AccountID").ToString())
                //        {
                //            GridProductionExpenses.DeleteRow(i);

                //        }
                        
                //    }
                //    view.DeleteSelectedRows();
                //    e.Handled = true;
                //    if (index > 0)
                //    {
                //        if (index > 0)
                //            index = index - 1;
                //        else if (index < 0)
                //        {
                //            index = view.DataRowCount;
                //            index = index - 1;
                //        }
                //        view.SelectRow(index);
                //        view.FocusedRowHandle = index;
                //    }
                //    //CalculateRow();
                //}
                else if (e.KeyData == Keys.Delete)
                {
                    if (!IsNewRecord)
                    {
                        bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, Messages.msgConfirmDelete);
                        if (!Yes)
                            return;
                    }
                    int index = view.FocusedRowHandle;
                    string AccountIDToDelete = view.GetRowCellValue(index, "AccountID").ToString();

                    // حذف جميع الأبناء المتصلة بالحساب المحدد
                    for (int i = GridProductionExpenses.DataRowCount - 1; i >= 0; i--)
                    {
                        string parentAccountID = Lip.GetValue("SELECT ParentAccountID FROM Acc_Accounts WHERE Cancel=0 And BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + " AND AccountLevel=" + MySession.GlobalNoOfLevels + " and AccountID=" + Comon.cDbl(GridProductionExpenses.GetRowCellValue(i, "AccountID")));

                        if (parentAccountID == AccountIDToDelete)
                        {
                            GridProductionExpenses.DeleteRow(i);
                        }
                    }
                    GridProductionExpenses_RowUpdated(null, null);

                    view.DeleteSelectedRows();
                    e.Handled = true;
                    if (index > 0)
                    {
                        index = index - 1;
                    }
                    else if (index < 0)
                    {
                        index = view.DataRowCount - 1;
                    }
                    view.SelectRow(index);
                    view.FocusedRowHandle = index;
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
        bool ChekOrderIsFoundInGrid(string OrderID)
        {
            for (int i = 0; i <= gridView8.DataRowCount - 1; i++)
            {
                if (i != gridView8.FocusedRowHandle)
                    if (gridView8.GetRowCellValue(i, "AccountID").ToString() == OrderID)
                        return true;
            }
            return false;
        }
        private void gridView8_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            double num;
            GridView view = sender as GridView;
            view.ClearColumnErrors();
            HasColumnErrors = false;
            string ColName = view.FocusedColumn.FieldName;



            if (ColName == "AccountID")
            {
                if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
                {
                    e.Valid = false;
                    HasColumnErrors = true;
                    e.ErrorText = Messages.msgInputIsRequired;
                }
                else if (!(double.TryParse(e.Value.ToString(), out num)))
                {
                    e.Valid = false;
                    HasColumnErrors = true;
                    e.ErrorText = Messages.msgInputShouldBeNumber;
                }
                else if (Comon.ConvertToDecimalPrice(e.Value.ToString()) <= 0)
                {
                    e.Valid = false;
                    HasColumnErrors = true;
                    e.ErrorText = Messages.msgInputIsGreaterThanZero;
                }

                /****************************************/

            }

            else if (ColName == "AccountName")
            {
                DataTable dtAccountName = Lip.SelectRecord("Select AccountID, " + PrimaryName + " AS AccountName from Acc_Accounts Where Cancel=0 and BranchID="+Comon.cInt(cmbBranchesID.EditValue)+" And LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') And FacilityID=" + UserInfo.FacilityID + "   AND AccountLevel=" +Comon.cInt( MySession.GlobalNoOfLevels-1));
                if (dtAccountName == null && dtAccountName.Rows.Count == 0)
                {
                    e.Valid = false;
                    HasColumnErrors = true;
                    e.ErrorText = Messages.msgNoFoundThisAccountID;
                }
                else
                {
                    if (Lip.CheckTheAccountIsStope(Comon.cDbl(dtAccountName.Rows[0]["AccountID"]), Comon.cInt(cmbBranchesID.EditValue)))
                    {
                        Messages.MsgWarning(Messages.TitleWorning, Messages.msgAccountIsStope);
                        e.Value = "";
                        return;
                    }

                    if (ChekOrderIsFoundInGrid(e.Value.ToString()))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = UserInfo.Language == iLanguage.Arabic ? "الحساب موجود  لذلك لا يمكن انزاله اكثر من مرة " : "This Account is Found Table";
                        return;
                    }
                    view.SetRowCellValue(view.FocusedRowHandle, view.Columns["AccountID"], dtAccountName.Rows[0]["AccountID"]);
                    view.SetRowCellValue(view.FocusedRowHandle, view.Columns["AccountName"], dtAccountName.Rows[0]["AccountName"]);

                }

            }


        }

        private void gridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            double num;
            GridView view = sender as GridView;
            view.ClearColumnErrors();
            HasColumnErrors = false;
            string ColName = view.FocusedColumn.FieldName;
            
 

            if (ColName == "AccountID" )
            {
                if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
                {
                    e.Valid = false;
                    HasColumnErrors = true;
                    e.ErrorText = Messages.msgInputIsRequired;
                }
                else if (!(double.TryParse(e.Value.ToString(), out num)))
                {
                    e.Valid = false;
                    HasColumnErrors = true;
                    e.ErrorText = Messages.msgInputShouldBeNumber;
                }
                else if (Comon.ConvertToDecimalPrice(e.Value.ToString()) <= 0)
                {
                    e.Valid = false;
                    HasColumnErrors = true;
                    e.ErrorText = Messages.msgInputIsGreaterThanZero;
                }

                /****************************************/

            }
          
            else if (ColName == "AccountName")
            {
                DataTable dtAccountName = Lip.SelectRecord("Select AccountID, " + PrimaryName + " AS AccountName from Acc_Accounts Where Cancel=0 and BranchID="+Comon.cInt(cmbBranchesID.EditValue)+" And LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') And FacilityID=" + UserInfo.FacilityID + "   AND AccountLevel=" + MySession.GlobalNoOfLevels);
                if (dtAccountName == null && dtAccountName.Rows.Count == 0)
                {
                    e.Valid = false;
                    HasColumnErrors = true;
                    e.ErrorText = Messages.msgNoFoundThisAccountID;
                }
                else
                { 
                        if (Lip.CheckTheAccountIsStope(Comon.cDbl(dtAccountName.Rows[0]["AccountID"]), Comon.cInt(cmbBranchesID.EditValue)))
                        {
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgAccountIsStope);
                            e.Value = "";
                            return;
                        }
                        view.SetRowCellValue(view.FocusedRowHandle, view.Columns["AccountID"], dtAccountName.Rows[0]["AccountID"]);
                        view.SetRowCellValue(view.FocusedRowHandle, view.Columns["AccountName"], dtAccountName.Rows[0]["AccountName"]);
                    
                }

            }
            
          
        }

        public void ClearFieldsTop()
        {
            try
            {
                txtCustomerID.ReadOnly = true;
                txtDelegateID.ReadOnly = true;
                txtOrderDate.ReadOnly = true;
                txtGuidanceID.ReadOnly = true;
                cmbTypeOrders.ReadOnly = true;
                txtDelegateID.Text = "";
                txtDelegateID_Validating(null, null);
                txtCustomerID.Text = "";
                txtCustomerID_Validating(null, null);
                txtGuidanceID.Text = "";
                txtGuidanceID_Validating(null, null);
               
            }
            catch
            {

            }
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
        private void txtGuidanceID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as UserName  FROM  [Users] where [Cancel]=0 and BranchID="+Comon.cInt(cmbBranchesID.EditValue)+" and [UserID]=" + txtGuidanceID.Text.ToString();
                CSearch.ControlValidating(txtGuidanceID, lblGuidanceName, strSQL);
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
                    strSQL = "SELECT ArbName as CustomerName  FROM Sales_CustomerAnSublierListArb Where    AcountID =" + txtCustomerID.Text + " and BranchID=" + Comon.cInt(cmbBranchesID.EditValue);
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
        public void ReadTopInfo(string OrderID, bool flag = false)
        {
            try
            {
                ClearFieldsTop();
                {
                    dt = Manu_OrderRestrictionDAL.frmGetDataDetalByID(OrderID, Comon.cInt(MySession.GlobalBranchID), UserInfo.FacilityID);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        txtOrderID.Text = dt.Rows[0]["OrderID"].ToString();
                        cmbTypeOrders.EditValue = Comon.cInt(dt.Rows[0]["TypeOrdersID"].ToString());
                        txtOrderDate.DateTime = DateTime.ParseExact(Comon.ConvertSerialDateTo(dt.Rows[0]["OrderDate"].ToString()), "dd/MM/yyyy", culture);
                      
                        //Validate
                        txtCustomerID.Text = dt.Rows[0]["CustomerID"].ToString();
                        txtCustomerID_Validating(null, null);
                        txtGuidanceID.Text = dt.Rows[0]["GuidanceID"].ToString();
                        txtGuidanceID_Validating(null, null);
                        txtDelegateID.Text = dt.Rows[0]["DelegateID"].ToString();
                        txtDelegateID_Validating(null, null);
                    }
                    else
                    {
                        Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "لا يوجد طلبية تمتلك هذا الرقم .. الرجاء ادخال رقم الطلبية الصحيح" : "There is no order that has this number. Please enter the correct order number");
                        txtOrderID.Text = "";
                    }
                }
            }
            catch
            { }
        }
        public void txtOrderID_Validating(object sender, CancelEventArgs e)
        {
            if (FormView == true)
            {
                int commandCastingID = Comon.cInt(Lip.GetValue(" select [CommandID] FROM  [Manu_CastingOrders] where [BranchID]=" + Comon.cInt(cmbBranchesID.EditValue) + "  and [Cancel]=0 and [OrderID]='" + txtOrderID.Text + "'"));
                if (commandCastingID > 0)
                {
                    txtCastingID.Text = commandCastingID.ToString();
                    TxtCastingID_Validating(null, null);
                }
                else
                {
                    Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "رقم الطلبية المدخلة لم يتم صبها ولا يوجد أمر صب للطلبية المحددة" : "The order number entered has not been decanted and there is no decanting order for the specified order");
                    txtOrderID.Text = "";
                    txtCastingID.Text = "";
                    txtOrderID.Focus();
                }
                if (String.IsNullOrEmpty(txtOrderID.Text) == false)
                {

                    if (String.IsNullOrEmpty(txtCastingID.Text) == true || Comon.cInt(txtCastingID.Text) <= 0)
                    {
                        Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "الرجاء ادخال رقم امر الصب الذي تنتمي له الطلبية" : "Enter The Number Command Casting");
                        txtOrderID.Text = "";

                        txtCastingID.Focus();
                        return;
                        
                    }
                    else
                    {
                          commandCastingID = Comon.cInt(Lip.GetValue(" select [CommandID] FROM  [Manu_CastingOrders] where [BranchID]=" + Comon.cInt(cmbBranchesID.EditValue) + "  and [Cancel]=0 and [OrderID]='" + txtOrderID.Text + "'"));
                        if(commandCastingID!=Comon.cInt(txtCastingID.Text))
                        {  
                            Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "الرجاء اختيار طلبية تنمتي الى امر الصب الذي تم اختيارة  " : "Enter The Number Command Casting");
                            txtOrderID.Text = "";
                            return;
                        }
                    }
                    string txtOrder = txtOrderID.Text;                     
                    int CommandIDTemp = 0;
                    CommandIDTemp = Comon.cInt(Lip.GetValue("select ComandID from Menu_ProductionExpensesMaster where Cancel=0 and BranchID="+Comon.cInt(cmbBranchesID.EditValue)+" and  ComandID<>" + Comon.cInt(txtCommandID.Text) + " and OrderID='" + txtOrderID.Text + "'"));
                    int CommandIDThis = Comon.cInt(Lip.GetValue("select ComandID from Menu_ProductionExpensesMaster where Cancel=0 and BranchID="+Comon.cInt(cmbBranchesID.EditValue)+" and  ComandID=" + Comon.cInt(txtCommandID.Text) + " and OrderID='" + txtOrderID.Text + "'"));
                    if (MySession.GlobalDefaultCanRepetUseOrderOneOureMoreBeforeCasting == true && CommandIDTemp > 0)
                    {
                        if (CommandIDTemp > 0)
                        {
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgDontRepetTheOrderinMoreCommend);
                            txtCommandID.Text = CommandIDTemp.ToString();                            
                            TxtCommandID_Validating(null, null);
                            return;
                        }
                    }
                    else if (IsNewRecord == false && CommandIDTemp <= 0 && CommandIDThis != Comon.cInt(txtCommandID.Text))
                    {
                        txtOrder = txtOrderID.Text;
                        ClearFields();
                        string OrderID = txtOrder;
                        txtOrderID.Text = OrderID;
                        ReadTopInfo(txtOrderID.Text);
                        txtFromDate.DateTime = txtOrderDate.DateTime;
                        txtToDate.DateTime = txtCommandCastingDate.DateTime;
                        //GetOrderDetail(txtOrderID.Text);
                        IsNewRecord = true;
                        Validations.DoNewRipon(this, ribbonControl1);
                    }
                    if ((IsNewRecord && CommandIDTemp <= 0))
                    {
                        string OrderID = txtOrder;
                        strSQL = "SELECT * FROM Manu_OrderRestriction WHERE  OrderID ='" + OrderID.Trim() + "' and BranchID=" + Comon.cInt(cmbBranchesID.EditValue);
                        Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, UserInfo.Language.ToString());
                        System.Data.DataTable dtt = Lip.SelectRecord(strSQL);
                        if (dtt.Rows.Count > 0)
                        {
                            ReadTopInfo(txtOrderID.Text);
                            txtFromDate.DateTime = txtOrderDate.DateTime;
                            txtToDate.DateTime=  txtCommandCastingDate.DateTime;
                            //GetOrderDetail(txtOrderID.Text);
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
                            InitializeFormatDate(txtOrderDate);
                            Messages.MsgError("تنبيه", "   لا يوجد طلبية  بهذا الرقم   ");
                            ClearFields();
                        }
                        return;
                    }


                }
            }
            else
            {
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                return;
            }
        }
   
        private void GridControl2_ProcessGridKey(object sender, KeyEventArgs e)
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
                    if (this.gridView3.ActiveEditor is CheckEdit)
                    {
                        view.SetFocusedValue(!Comon.cbool(view.GetFocusedValue()));
                        //  CalculateRow(GridCastingAfter.FocusedRowHandle, Comon.cbool(view.GetFocusedValue()));
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

                    }
                }
                else if (e.KeyData == Keys.Delete)
                {
                    if (!IsNewRecord)
                    {
                        bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, Messages.msgConfirmDelete);
                        if (!Yes)
                            return;
                    }
                    int index = view.FocusedRowHandle;
                    view.DeleteSelectedRows();
                    GridProductionExpenses_RowUpdated(null, null);
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
                    //CalculateRow();
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
        private void GridControl1_ProcessGridKey(object sender, KeyEventArgs e)
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
                        //  CalculateRow(GridCastingAfter.FocusedRowHandle, Comon.cbool(view.GetFocusedValue()));
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
                    }
                }
                else if (e.KeyData == Keys.Delete)
                {
                    if (!IsNewRecord)
                    {
                        bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, Messages.msgConfirmDelete);
                        if (!Yes)
                            return;
                    }
                    int index = view.FocusedRowHandle;
                    GridProductionExpenses_RowUpdated(null, null);
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
                    //CalculateRow();
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

        private void GridControlProductionExpenses_ProcessGridKey(object sender, KeyEventArgs e)
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
                        if (this.GridProductionExpenses.ActiveEditor is CheckEdit)
                        {
                            view.SetFocusedValue(!Comon.cbool(view.GetFocusedValue()));
                            //  CalculateRow(GridCastingAfter.FocusedRowHandle, Comon.cbool(view.GetFocusedValue()));
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
                          
                        }
                    }
                    else if (e.KeyData == Keys.Delete)
                    {
                        if (!IsNewRecord)
                        {
                            bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, Messages.msgConfirmDelete);
                            if (!Yes)
                                return;
                        }
                        int index = view.FocusedRowHandle;
                        GridProductionExpenses_RowUpdated(null, null);
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
                        //CalculateRow();
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

        private void TxtToDate_Validating(object sender, CancelEventArgs e)
        {
            cmbParentAccountIDVariable_EditValueChanged(null, null);
        }

        private void TxtQTYOrders_Validating(object sender, CancelEventArgs e)
        {
            GridProductionExpenses_RowUpdated(null,null);


        }

        private void GridProductionExpenses_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            decimal TotatEvaluation = 0;
            for (int i=0;i<= GridProductionExpenses.DataRowCount-1; i++)
            {
                TotatEvaluation += Comon.cDec(GridProductionExpenses.GetRowCellValue(i, "Evaluation").ToString());
            }
            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                TotatEvaluation += Comon.cDec(gridView1.GetRowCellValue(i, "Evaluation").ToString());
            }
            for (int i = 0; i <= gridView3.DataRowCount - 1; i++)
            {
                TotatEvaluation += Comon.cDec(gridView3.GetRowCellValue(i, "Evaluation").ToString());
            }

            txtTotalExpences.Text = TotatEvaluation.ToString();
            if(Comon.cDec(txtQTYOrders.Text)!=0)
            txtQTYGram.Text = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(TotatEvaluation) / Comon.cDec(txtQTYOrders.Text)).ToString();
           
            txtTotalCostOrder.Text = Comon.ConvertToDecimalPrice(Comon.cDec(txtQTYGram.Text) * Comon.cDec(txtQtyOrder.Text)).ToString();
            for (int i = 0; i <= gridView6.DataRowCount-1; i++)
            {
                if (gridView6.GetRowCellValue(i, "OrderID").ToString() == txtOrderID.Text.ToString())
                    gridView6.SetRowCellValue(i, "BonesPriceOrder", Comon.ConvertToDecimalPrice(Comon.cDec(txtQtyOrder.Text) * Comon.cDec(txtQTYGram.Text)));
            }
        }

        private void TxtCommandID_Validating(object sender, CancelEventArgs e)
        {
            if (FormView == true)
                ReadRecord(Comon.cInt(txtCommandID.Text));
            else
            {
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                return;
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
        public void SetDataToShow(string Casting,string OrderID)
        {
            txtCastingID.Text = Casting;
            TxtCastingID_Validating(null, null);
            txtOrderID.Text = OrderID;
            txtOrderID_Validating(null, null);
        }
        public void TxtCastingID_Validating(object sender, CancelEventArgs e)
        {

            System.Data.DataTable dt = Menu_ProductionExpensesMasterDAL.frmGetDataOrderDetail(Comon.cInt(txtCastingID.Text), Comon.cInt(cmbBranchesID.EditValue), MySession.GlobalFacilityID);
            if (dt != null && dt.Rows.Count > 0)
            {
                gridControl3.DataSource = dt;
                lstDetailOrders.AllowNew = true;
                lstDetailOrders.AllowEdit = true;
                lstDetailOrders.AllowRemove = true;
                
                txtNumberCups.Text = dt.Rows[0]["NumberCups"].ToString();
                txtNumberOrder.Text = gridView6.DataRowCount.ToString();
                if (Comon.ConvertSerialDateTo(dt.Rows[0]["CommandDate"].ToString()) == "")
                    InitializeFormatDate(txtCommandCastingDate);
                else
                    txtCommandCastingDate.EditValue = DateTime.ParseExact(Comon.ConvertSerialDateTo(dt.Rows[0]["CommandDate"].ToString()), "dd/MM/yyyy", culture);

                decimal TotalQty = 0;
                for (int i = 0; i <= gridView6.DataRowCount - 1; i++)
                {
                    TotalQty += Comon.cDec(gridView6.GetRowCellValue(i, "GoldQTYCloves").ToString());
                }
                txtQTYOrders.Text = TotalQty.ToString();
            }
        }

        private void GridProductionExpenses_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            e.Graphics.FillRectangle(e.Cache.GetSolidBrush(Color.Beige), e.Bounds);
            e.Graphics.DrawRectangle(e.Cache.GetPen(Color.Black, 1), e.Bounds);
            e.Appearance.DrawString(e.Cache, e.DisplayText, e.Bounds);
            e.Handled = true;
            ((GridView)sender).Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;
            ((GridView)sender).Appearance.Row.TextOptions.VAlignment = VertAlignment.Center;
        }

        private void FrmManuExpencessOrder_Load(object sender, EventArgs e)
        {
            initGridAccounts();
            initGridVariableExpenses();
            initGridFixedExpenses();
            initGridEstimatedExpenses();
            initGlstDetailOrders();
            DoNew();
            simpleButton1_Click(null, null);
            simpleButton2_Click(null, null);
            GridProductionExpenses_RowUpdated(null, null);
            //btnShow_Click();
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
    
        //private void btnShow_Click()
        //{
        //    try
        //    {
        //        long AccountID = 0;

        //        long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
        //        long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
        //        int GlobalNoOfLevels = MySession.GlobalNoOfLevels;

        //        System.Data.DataTable dtCustomer = new System.Data.DataTable();
        //        strSQL = "SELECT AccountID,ArbName as AccountName FROM Acc_Accounts WHERE Cancel=0 And BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + " AND AccountLevel=" + GlobalNoOfLevels + " AND AccountID LIKE '3%'";

        //        if (Comon.cDbl(cmbParentAccountIDVariable.EditValue) > 0)
        //            strSQL = "SELECT AccountID,ArbName as AccountName FROM Acc_Accounts WHERE Cancel=0  And BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + " AND AccountLevel=" + GlobalNoOfLevels + " and ParentAccountID=" + Comon.cDbl(cmbParentAccountIDVariable.EditValue);

        //        dtCustomer = Lip.SelectRecord(strSQL);

        //        #region GetBalanceCustomer
        //        for (int i = 0; i <= dtCustomer.Rows.Count - 1; i++)
        //        {
        //            AccountID = Comon.cLong(dtCustomer.Rows[i]["AccountID"].ToString()); 
        //            VariousVoucherMachin(AccountID.ToString(), FromDate, ToDate);
        //        }
                 
        //       _sampleDataCustomer.Clear();
        //        for (int i = 0; i <= _sampleData.Rows.Count-1; i++)
        //        {
        //            decimal debitValue = Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Debit"].ToString());
        //            if (debitValue > 0)
        //            {
        //                DataRow newRow = _sampleDataCustomer.NewRow();
        //                newRow["AccountID"] = _sampleData.Rows[i]["AccountID"].ToString();
        //                newRow["AccountName"] = _sampleData.Rows[i]["AccountName"].ToString();
        //                newRow["Debit"] = _sampleData.Rows[i]["Debit"].ToString();
        //                newRow["Evaluation"] = _sampleData.Rows[i]["Debit"].ToString();
        //                newRow["Credit"] = _sampleData.Rows[i]["Credit"].ToString();
        //                _sampleDataCustomer.Rows.Add(newRow);
        //            }
        //        }

        //        DataTable dt = new DataTable();
        //        dt = _sampleDataCustomer.Copy();
        //        if (GridProductionExpenses.DataRowCount<=0)
        //            gridControlProductionExpenses.DataSource = _sampleDataCustomer;
        //        else
        //            for (int i = 0; i < _sampleDataCustomer.Rows.Count; i++)
        //            {
        //                GridProductionExpenses.AddNewRow();
        //                GridProductionExpenses.SetRowCellValue(GridProductionExpenses.DataRowCount, "AccountID", dt.Rows[i]["AccountID"].ToString());
        //                GridProductionExpenses.SetRowCellValue(GridProductionExpenses.DataRowCount, "AccountName", dt.Rows[i]["AccountName"].ToString());
        //                GridProductionExpenses.SetRowCellValue(GridProductionExpenses.DataRowCount, "Debit", dt.Rows[i]["Debit"].ToString());
        //                GridProductionExpenses.SetRowCellValue(GridProductionExpenses.DataRowCount, "Evaluation", dt.Rows[i]["Evaluation"].ToString());
        //                GridProductionExpenses.SetRowCellValue(GridProductionExpenses.DataRowCount, "Credit", dt.Rows[i]["Credit"].ToString());
        //            }
        //        gridControlProductionExpenses.RefreshDataSource();
        //    }
        //    catch { }
        //}
        private void btnShow_Click()
        {
            try
            {
                long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                int GlobalNoOfLevels = MySession.GlobalNoOfLevels;

                System.Data.DataTable dtCustomer = new System.Data.DataTable();
                strSQL = "SELECT AccountID,ArbName as AccountName FROM Acc_Accounts WHERE Cancel=0 And BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + " AND AccountLevel=" + GlobalNoOfLevels + " AND AccountID LIKE '3%'";

                if (Comon.cDbl(gridView8.DataRowCount) > 0)
                {

                    _sampleDataCustomer.Clear();
                    //_sampleData.Clear();
                    for (int j = 0; j < gridView8.DataRowCount; j++)
                    {
                        _sampleData.Clear();
                        strSQL = "SELECT AccountID,ArbName as AccountName FROM Acc_Accounts WHERE Cancel=0  And BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + " AND AccountLevel=" + GlobalNoOfLevels + " and ParentAccountID=" + Comon.cDbl(gridView8.GetRowCellValue(j,"AccountID") );

                        dtCustomer = Lip.SelectRecord(strSQL);

                        #region GetBalanceCustomer
                        for (int i = 0; i <= dtCustomer.Rows.Count - 1; i++)
                        {
                            long AccountID = Comon.cLong(dtCustomer.Rows[i]["AccountID"].ToString());
                           
                            VariousVoucherMachin(AccountID.ToString(), FromDate, ToDate);
                        }

                        for (int i = 0; i <= _sampleData.Rows.Count - 1; i++)
                        {
                            decimal debitValue = Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Debit"].ToString());
                            if (debitValue > 0)
                            {
                                DataRow newRow = _sampleDataCustomer.NewRow();
                                newRow["AccountID"] = _sampleData.Rows[i]["AccountID"].ToString();
                                newRow["AccountName"] = _sampleData.Rows[i]["AccountName"].ToString();
                                newRow["Debit"] = _sampleData.Rows[i]["Debit"].ToString();
                                newRow["Evaluation"] = _sampleData.Rows[i]["Debit"].ToString();
                                newRow["Credit"] = _sampleData.Rows[i]["Credit"].ToString();
                                newRow["Notes"] = _sampleData.Rows[i]["Notes"].ToString();
                                _sampleDataCustomer.Rows.Add(newRow);
                            }
                        }
                        gridControlProductionExpenses.DataSource = _sampleDataCustomer;
                    }
                    
                }
             
            }
            catch { }
        }



        private void VariousVoucherMachin(string AccountID, long FromDate, long ToDate)
        {
            try
            {
                System.Data.DataTable dtCredit = new System.Data.DataTable();
                string strSQL = null; DataRow row;
                strSQL = "SELECT Acc_VariousVoucherMachinMaster.VOUCHERDATE AS TheDate,Acc_VariousVoucherMachinMaster.VOUCHERID AS ID,"
               + " 'VariousVoucher' AS RecordType, ' ' AS AccountName,Acc_VariousVoucherMachinDetails.ACCOUNTID,Acc_VariousVoucherMachinDetails.DEBIT,"
               + " Acc_VariousVoucherMachinDetails.CREDIT,Acc_VariousVoucherMachinDetails.Declaration as Notes FROM Acc_VariousVoucherMachinMaster INNER JOIN Acc_VariousVoucherMachinDetails"
               + " ON Acc_VariousVoucherMachinMaster.VOUCHERID= Acc_VariousVoucherMachinDetails.VOUCHERID AND Acc_VariousVoucherMachinMaster.BranchID= Acc_VariousVoucherMachinDetails.BranchID"
               + " AND Acc_VariousVoucherMachinMaster.FacilityID  = Acc_VariousVoucherMachinDetails.FacilityID WHERE Acc_VariousVoucherMachinDetails.ACCOUNTID = " + AccountID
               + " AND Acc_VariousVoucherMachinMaster.CANCEL = 0 AND Acc_VariousVoucherMachinMaster.BranchID =" + Comon.cInt(cmbBranchesID.EditValue) + " AND Acc_VariousVoucherMachinMaster.FacilityID =" + UserInfo.FacilityID.ToString();

                if (FromDate != 0)
                {

                    strSQL = strSQL + " AND  Acc_VariousVoucherMachinMaster.VoucherDate >=" + FromDate;
                }
                if (ToDate != 0)
                {

                    strSQL = strSQL + " AND  Acc_VariousVoucherMachinMaster.VoucherDate <=" + ToDate;
                }

                //if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                //{

                //    strSQL = strSQL + " AND  Acc_VariousVoucherMachinDetails.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                //}


                strSQL = strSQL + " ORDER BY Acc_VariousVoucherMachinMaster.VoucherDate";

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());
                dtCredit = Lip.SelectRecord(strSQL);
                if (dtCredit.Rows.Count > 0)
                {
                  
                    for (int i = 0; i <= dtCredit.Rows.Count - 1; i++)
                    {
                        if (Comon.cDec(dtCredit.Rows[i]["Debit"]) > 0)
                        {
                         
                            row = _sampleData.NewRow();
                            row["Credit"] = dtCredit.Rows[i]["Credit"];
                            row["Debit"] = dtCredit.Rows[i]["Debit"];
                            row["AccountID"] = dtCredit.Rows[i]["AccountID"];
                            row["Notes"] = dtCredit.Rows[i]["Notes"];
                            row["AccountName"] = Lip.GetValue("select " + PrimaryName + " as AccountName  from Acc_Accounts where  Cancel=0 and BranchID="+Comon.cInt(cmbBranchesID.EditValue)+" and AccountID=" + dtCredit.Rows[i]["AccountID"]);
                            _sampleData.Rows.Add(row);
                           
                        }
                    }
                }
                dtCredit.Dispose();
                row = null;
            }
            catch { }
        }
        #endregion
        #region 
        void initGlstDetailOrders()
        {
            lstDetailOrders = new BindingList<Manu_OrderRestriction>();
            lstDetailOrders.AllowNew = true;
            lstDetailOrders.AllowEdit = true;
            lstDetailOrders.AllowRemove = true;
            gridControl3.DataSource = lstDetailOrders;

            gridView6.Columns["Cancel"].Visible = false;
            gridView6.Columns["TypeAuxiliaryMatirialID"].Visible = false;
            gridView6.Columns["BranchID"].Visible = false;
            gridView6.Columns["FacilityID"].Visible = false;

            gridView6.Columns["EditUserID"].Visible = false;
            gridView6.Columns["EditDate"].Visible = false;
            gridView6.Columns["EditTime"].Visible = false;
            gridView6.Columns["RegDate"].Visible = false;
            gridView6.Columns["UserID"].Visible = false;

            gridView6.Columns["ComputerInfo"].Visible = false;
            gridView6.Columns["EditComputerInfo"].Visible = false;
            gridView6.Columns["RegTime"].Visible = false;
            gridView6.Columns["TypeID"].Visible = false;
            gridView6.Columns["Notes"].Visible = false;
            gridView6.Columns["CustomerID"].Visible = false;
            gridView6.Columns["TypeOrdersID"].Visible = false;
            gridView6.Columns["DelegateID"].Visible = false;
            gridView6.Columns["TypeOrdersID"].Visible = false;
            gridView6.Columns["DelegateID"].Visible = false;
            gridView6.Columns["GuidanceID"].Visible = false;
            gridView6.Columns["TypeOrdersID"].Visible = false;
            gridView6.Columns["DelegateID"].Visible = false;
            gridView6.Columns["BonesPriceOrder"].Visible = false;
            gridView6.Columns["ImageCode"].Visible = false;
            if (UserInfo.Language == iLanguage.Arabic)
            {
                gridView6.Columns["OrderID"].Caption = "رقم الطلبية  ";
                gridView6.Columns["OrderDate"].Caption = "تاريخ ";
                gridView6.Columns["CustomerName"].Caption = "العميل ";
                gridView6.Columns["GoldQTYCloves"].Caption = "الوزن ";
                gridView6.Columns["BonesPriceOrder"].Caption = "أجور";

            }
            else
            {
                gridView6.Columns["OrderID"].Caption = "Order ID";
                gridView6.Columns["OrderDate"].Caption = "Order Date";
                gridView6.Columns["CustomerName"].Caption = "Customer Name";
                gridView6.Columns["GoldQTYCloves"].Caption = "Qty ";
                gridView6.Columns["BonesPriceOrder"].Caption = "Bones";
            }

        }
        void initGridVariableExpenses()
        {

            lstDetailProductionExpenses = new BindingList<Manu_ProductionExpensesDetails>();
            lstDetailProductionExpenses.AllowNew = true;
            lstDetailProductionExpenses.AllowEdit = true;
            lstDetailProductionExpenses.AllowRemove = true;
            gridControlProductionExpenses.DataSource = lstDetailProductionExpenses;

            //RepositoryItemLookUpEdit rAccountName = Common.LookUpEditAccountName();
            //gridControlProductionExpenses.RepositoryItems.Add(rAccountName);
            //GridProductionExpenses.Columns["AccountName"].ColumnEdit = rAccountName;

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

            GridProductionExpenses.Columns["TypeExpenssID"].Visible = false;
            GridProductionExpenses.Columns["EditComputerInfo"].Visible = false;
            GridProductionExpenses.Columns["RegTime"].Visible = false;
            GridProductionExpenses.Columns["Evaluation"].OptionsColumn.AllowEdit = false;
            GridProductionExpenses.Columns["Evaluation"].OptionsColumn.AllowFocus = false;
            GridProductionExpenses.Columns["Credit"].OptionsColumn.AllowEdit = false;
            GridProductionExpenses.Columns["Credit"].OptionsColumn.AllowFocus = false;
            GridProductionExpenses.Columns["Evaluation"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            GridProductionExpenses.Columns["Evaluation"].SummaryItem.DisplayFormat = "{0:0.00}";


            GridProductionExpenses.Columns["Debit"].OptionsColumn.AllowEdit = false;
            GridProductionExpenses.Columns["Debit"].OptionsColumn.AllowFocus = false;
            GridProductionExpenses.Columns["Debit"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            GridProductionExpenses.Columns["Debit"].SummaryItem.DisplayFormat = "{0:0.00}";
            if (UserInfo.Language == iLanguage.Arabic)
            {
                GridProductionExpenses.Columns["AccountID"].Caption = "رقم الحســـاب ";
                GridProductionExpenses.Columns["Notes"].Caption = "ملاحظـــات";
                GridProductionExpenses.Columns["AccountName"].Caption = "إسم الحســـاب";
                GridProductionExpenses.Columns["Credit"].Caption = "الدائــــن";
                GridProductionExpenses.Columns["Debit"].Caption = "المديـــن ";
                GridProductionExpenses.Columns["Evaluation"].Caption = "التقييــم ";
            }
            else
            {
                GridProductionExpenses.Columns["AccountID"].Caption = "Account ID";
                GridProductionExpenses.Columns["AccountName"].Caption = "Account Name";
                GridProductionExpenses.Columns["Credit"].Caption = "Credit";
                GridProductionExpenses.Columns["Debit"].Caption = "Debit ";

                GridProductionExpenses.Columns["Evaluation"].Caption = "Evaluation";
            }
        }
        void initGridFixedExpenses()
        {

            lstDetailFixedExpenses = new BindingList<Manu_ProductionExpensesDetails>();
            lstDetailFixedExpenses.AllowNew = true;
            lstDetailFixedExpenses.AllowEdit = true;
            lstDetailFixedExpenses.AllowRemove = true;
            gridControl1.DataSource = lstDetailFixedExpenses;

            RepositoryItemLookUpEdit rAccountName = Common.LookUpEditAccountName();
            gridControl1.RepositoryItems.Add(rAccountName);
            gridView1.Columns["AccountName"].ColumnEdit = rAccountName;

            gridView1.Columns["ComandID"].Visible = false;
            gridView1.Columns["Cancel"].Visible = false;
            gridView1.Columns["BranchID"].Visible = false;
            gridView1.Columns["FacilityID"].Visible = false;

            gridView1.Columns["EditUserID"].Visible = false;
            gridView1.Columns["EditDate"].Visible = false;
            gridView1.Columns["EditTime"].Visible = false;
            gridView1.Columns["RegDate"].Visible = false;
            gridView1.Columns["UserID"].Visible = false;

            gridView1.Columns["ComputerInfo"].Visible = false;
            gridView1.Columns["EditComputerInfo"].Visible = false;
            gridView1.Columns["RegTime"].Visible = false;

            gridView1.Columns["TypeExpenssID"].Visible = false;
            //gridView1.Columns["Evaluation"].OptionsColumn.AllowEdit = false;
            //gridView1.Columns["Evaluation"].OptionsColumn.AllowFocus = false;

            gridView1.Columns["Credit"].Visible = false;
            gridView1.Columns["Debit"].Visible = false;

            gridView1.Columns["Evaluation"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView1.Columns["Evaluation"].SummaryItem.DisplayFormat = "{0:0.00}";
            if (UserInfo.Language == iLanguage.Arabic)
            {
                gridView1.Columns["Notes"].Caption = "ملاحظـــات";
                gridView1.Columns["AccountID"].Caption = "رقم الحســـاب ";
                gridView1.Columns["AccountName"].Caption = "إسم الحســـاب";
                gridView1.Columns["Credit"].Caption = "الدائــــن";
                gridView1.Columns["Debit"].Caption = "المديـــن ";
                gridView1.Columns["Evaluation"].Caption = "التقييــم ";

            }
            else
            {
                gridView1.Columns["AccountID"].Caption = "Account ID";
                gridView1.Columns["AccountName"].Caption = "Account Name";
                gridView1.Columns["Credit"].Caption = "Credit";
                gridView1.Columns["Debit"].Caption = "Debit ";

                gridView1.Columns["Evaluation"].Caption = "Evaluation";
            }
        }
        void initGridEstimatedExpenses()
        {

            lstDetailEstimatedExpenses = new BindingList<Manu_ProductionExpensesDetails>();
            lstDetailEstimatedExpenses.AllowNew = true;
            lstDetailEstimatedExpenses.AllowEdit = true;
            lstDetailEstimatedExpenses.AllowRemove = true;
            gridControl2.DataSource = lstDetailEstimatedExpenses;

            RepositoryItemLookUpEdit rAccountName = Common.LookUpEditAccountName();
            gridControl2.RepositoryItems.Add(rAccountName);
            gridView3.Columns["AccountName"].ColumnEdit = rAccountName;

            gridView3.Columns["ComandID"].Visible = false;
            gridView3.Columns["Cancel"].Visible = false;
            gridView3.Columns["BranchID"].Visible = false;
            gridView3.Columns["FacilityID"].Visible = false;

            gridView3.Columns["EditUserID"].Visible = false;
            gridView3.Columns["EditDate"].Visible = false;
            gridView3.Columns["EditTime"].Visible = false;
            gridView3.Columns["RegDate"].Visible = false;
            gridView3.Columns["UserID"].Visible = false;

            gridView3.Columns["TypeExpenssID"].Visible = false;
            gridView3.Columns["ComputerInfo"].Visible = false;
            gridView3.Columns["EditComputerInfo"].Visible = false;
            gridView3.Columns["RegTime"].Visible = false;
            gridView3.Columns["Credit"].Visible = false;
            gridView3.Columns["Debit"].Visible = false;
            //gridView3.Columns["Evaluation"].OptionsColumn.AllowEdit = false;
            //gridView3.Columns["Evaluation"].OptionsColumn.AllowFocus = false;
            gridView3.Columns["Evaluation"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView3.Columns["Evaluation"].SummaryItem.DisplayFormat = "{0:0.00}";
            if (UserInfo.Language == iLanguage.Arabic)
            {
                gridView3.Columns["Notes"].Caption = "ملاحظـــات";
                gridView3.Columns["AccountID"].Caption = "رقم الحســـاب ";
                gridView3.Columns["AccountName"].Caption = "إسم الحســـاب";
                gridView3.Columns["Credit"].Caption = "الدائــــن";
                gridView3.Columns["Debit"].Caption = "المديـــن ";
                gridView3.Columns["Evaluation"].Caption = "التقييــم ";

            }
            else
            {
                gridView3.Columns["AccountID"].Caption = "Account ID";
                gridView3.Columns["AccountName"].Caption = "Account Name";
                gridView3.Columns["Credit"].Caption = "Credit";
                gridView3.Columns["Debit"].Caption = "Debit ";

                gridView3.Columns["Evaluation"].Caption = "Evaluation";
            }
        }

     
        void initGridAccounts()
        {

            lstDetailAccountsParent = new BindingList<Menu_ProductionExpensesAcconts>();
            lstDetailAccountsParent.AllowNew = true;
            lstDetailAccountsParent.AllowEdit = true;
            lstDetailAccountsParent.AllowRemove = true;
            gridControl4.DataSource = lstDetailAccountsParent;

            RepositoryItemLookUpEdit rAccountName = Common.LookUpEditAccountName(MySession.GlobalNoOfLevels-1);
            gridControl4.RepositoryItems.Add(rAccountName);
            gridView8.Columns["AccountName"].ColumnEdit = rAccountName;

            gridView8.Columns["ComandID"].Visible = false;
            gridView8.Columns["Cancel"].Visible = false;
            gridView8.Columns["BranchID"].Visible = false;

            gridView8.Columns["Manu_Detils"].Visible = false; 
            gridView8.Columns["AccountID"].Width = 150;
            gridView8.Columns["AccountName"].Width = 200;
            if (UserInfo.Language == iLanguage.Arabic)
            { 
                gridView8.Columns["AccountID"].Caption = "رقم الحســـاب ";
                gridView8.Columns["AccountName"].Caption = "إسم الحســـاب";

            }
            else
            {
                gridView8.Columns["AccountID"].Caption = "Account ID";
                gridView8.Columns["AccountName"].Caption = "Account Name";
           
            }
        }
        #endregion

        public void ClearFields()
        {
            try
            {

                txtNumberCups.Text = "";
                txtNumberOrder.Text = "";
                txtCostCenterID.Text = "";
                lblCostCenterName.Text = "";
                txtTotalCostOrder.Text = "";
                txtQTYGram.Text = "";
                txtQtyOrder.Text = "";
                txtCastingID.Text = "";
                txtQTYOrders.Text = "";
                txtTotalCostOrder.ReadOnly = true;
                txtQTYGram.ReadOnly = true;
                ClearFieldsTop();
                txtSalesPriceQram.Text = "";

                txtNotes.Text = "";
                txtDebitAccountID.Text = "";
                txtCreditAccountID.Text = "";
                txtCreditAccountID_Validating(null, null);
                txtDebitAccountID_Validating(null, null);
                txtCommandDate.EditValue = DateTime.Now;
                _sampleDataCustomer.Clear();
                lstDetailProductionExpenses = new BindingList<Manu_ProductionExpensesDetails>();
                lstDetailProductionExpenses.AllowNew = true;
                lstDetailProductionExpenses.AllowEdit = true;
                lstDetailProductionExpenses.AllowRemove = true;
                gridControlProductionExpenses.DataSource = lstDetailProductionExpenses;

                lstDetailFixedExpenses = new BindingList<Manu_ProductionExpensesDetails>();
                lstDetailFixedExpenses.AllowNew = true;
                lstDetailFixedExpenses.AllowEdit = true;
                lstDetailFixedExpenses.AllowRemove = true;
                gridControl1.DataSource = lstDetailFixedExpenses;

                lstDetailEstimatedExpenses = new BindingList<Manu_ProductionExpensesDetails>();
                lstDetailEstimatedExpenses.AllowNew = true;
                lstDetailEstimatedExpenses.AllowEdit = true;
                lstDetailEstimatedExpenses.AllowRemove = true;
                gridControl2.DataSource = lstDetailEstimatedExpenses;


                lstDetailAccountsParent = new BindingList<Menu_ProductionExpensesAcconts>();
                lstDetailAccountsParent.AllowNew = true;
                lstDetailAccountsParent.AllowEdit = true;
                lstDetailAccountsParent.AllowRemove = true;
                gridControl4.DataSource = lstDetailAccountsParent;
                dt = new System.Data.DataTable();

                lstDetailOrders = new BindingList<Manu_OrderRestriction>();
                lstDetailOrders.AllowNew = true;
                lstDetailOrders.AllowEdit = true;
                lstDetailOrders.AllowRemove = true;
                gridControl3.DataSource = lstDetailOrders;

              
              

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public void ReadRecord(int CommendID, bool flag = false)
        {
            try
            {
                ClearFields();
                {
                    IsNewRecord = false;
                    dt = Menu_ProductionExpensesMasterDAL.frmGetDataDetalByIDByTypeExpenss(CommendID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID,1);
                    dt1 = Menu_ProductionExpensesMasterDAL.frmGetDataDetalByIDByTypeExpenss(CommendID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID, 2);
                    dt2 = Menu_ProductionExpensesMasterDAL.frmGetDataDetalByIDByTypeExpenss(CommendID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID, 3);
                    DataTable dt3 = Menu_ProductionExpensesMasterDAL.frmGetDataAccount(CommendID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                   
                        IsNewRecord = false;

                        txtCommandID.Text = dt.Rows[0]["ComandID"].ToString();

                        txtNumberOrder.Text = dt.Rows[0]["NumberOrder"].ToString();
                        txtSalesPriceQram.Text = dt.Rows[0]["SalesPriceQram"].ToString();
                        txtNumberCups.Text = dt.Rows[0]["NumberCups"].ToString();
                        //Validate
                        txtQTYGram.Text= dt.Rows[0]["QTYGram"].ToString();
                        txtQTYOrders.Text= dt.Rows[0]["QTYOrders"].ToString();

                        txtQtyOrder.Text = dt.Rows[0]["QTYOrder"].ToString();
                        cmbStatus.EditValue = Comon.cInt(dt.Rows[0]["Posted"].ToString());

                        txtCastingID.Text = dt.Rows[0]["CastingID"].ToString();
                        TxtCastingID_Validating(null, null);
                        cmbCurency.EditValue = Comon.cInt(dt.Rows[0]["CurencyID"].ToString());
                        cmbCurency_EditValueChanged(null, null);
                        txtCreditAccountID.Text = dt.Rows[0]["CreditAccountID"].ToString();
                        txtCreditAccountID_Validating(null, null);

                        txtDebitAccountID.Text = dt.Rows[0]["DebitAccountID"].ToString();
                        txtDebitAccountID_Validating(null, null);

                        txtCostCenterID.Text = dt.Rows[0]["CostCenterID"].ToString();
                        txtCostCenterID_Validating(null, null);
                        cmbCategoryOrders.EditValue= dt.Rows[0]["CategoryOrders"].ToString();
                        txtNotes.Text = dt.Rows[0]["Notes"].ToString();
                        cmbBranchesID.EditValue = Comon.cInt(dt.Rows[0]["BranchID"]);

                        gridControlProductionExpenses.DataSource = dt;
                        lstDetailProductionExpenses.AllowNew = true;
                        lstDetailProductionExpenses.AllowEdit = true;
                        lstDetailProductionExpenses.AllowRemove = true;

                        gridControl1.DataSource = dt1;

                        lstDetailFixedExpenses.AllowNew = true;
                        lstDetailFixedExpenses.AllowEdit = true;
                        lstDetailFixedExpenses.AllowRemove = true;

                        gridControl2.DataSource = dt2;
                        lstDetailEstimatedExpenses.AllowNew = true;
                        lstDetailEstimatedExpenses.AllowEdit = true;
                        lstDetailEstimatedExpenses.AllowRemove = true;


                        gridControl4.DataSource = dt3;
                        lstDetailAccountsParent.AllowNew = true;
                        lstDetailAccountsParent.AllowEdit = true;
                        lstDetailAccountsParent.AllowRemove = true;
                        txtFromDate.DateTime = DateTime.ParseExact(Comon.ConvertSerialDateTo(dt.Rows[0]["FromDate"].ToString()), "dd/MM/yyyy", culture);
                        txtToDate.DateTime = DateTime.ParseExact(Comon.ConvertSerialDateTo(dt.Rows[0]["ToDate"].ToString()), "dd/MM/yyyy", culture);
                        
                        txtOrderID.Text = dt.Rows[0]["OrderID"].ToString();
                        ReadTopInfo(txtOrderID.Text);
                        GridProductionExpenses_RowUpdated(null, null);
                       
                        Validations.DoReadRipon(this, ribbonControl1);
                        EnabledControl(false);
                        //ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Caption = txtCommandID.Text;
                    }
                }


        }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
        }
    }
        public void MoveRec(long PremaryKeyValue, int Direction)
        {
            try
            {
                System.Windows.Forms.Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                #region If

                if (FormView == true)
                {
                    SplashScreenManager.CloseForm(false);
                    strSQL = "SELECT TOP 1 * FROM " + Menu_ProductionExpensesMasterDAL.TableName + " Where Cancel =0  And BranchID= " + Comon.cInt(cmbBranchesID.EditValue);
                    switch (Direction)
                    {
                        case xMoveFirst:
                            {
                                strSQL = strSQL + " ORDER BY " + Menu_ProductionExpensesMasterDAL.PremaryKey + " ASC";
                                break;
                            }
                        case xMoveNext:
                            {
                                strSQL = strSQL + " And " + Menu_ProductionExpensesMasterDAL.PremaryKey + ">" + PremaryKeyValue + " ORDER BY " + Menu_ProductionExpensesMasterDAL.PremaryKey + " asc";
                                break;
                            }

                        case xMovePrev:
                            {
                                strSQL = strSQL + " And " + Menu_ProductionExpensesMasterDAL.PremaryKey + "<" + PremaryKeyValue + " ORDER BY " + Menu_ProductionExpensesMasterDAL.PremaryKey + " desc";
                                break;
                            }
                        case xMoveLast:
                            {
                                strSQL = strSQL + " ORDER BY " + Menu_ProductionExpensesMasterDAL.PremaryKey + " DESC";
                                break;
                            }
                    }
                    cClass = new Menu_ProductionExpensesMasterDAL();

                    int InvoicIDTemp = Comon.cInt(txtCommandID.Text);
                    InvoicIDTemp = cClass.GetRecordSetBySQL(strSQL);
                    if (cClass.FoundResult == true)
                    {
                        ReadRecord(InvoicIDTemp);
                        EnabledControl(false);
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
        void Find()
        {
            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = " Where 1=1 ";

            FocusedControl = GetIndexFocusedControl();
            if (FocusedControl == null) return;
            else if (FocusedControl.Trim() == txtCostCenterID.Name)
            {

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "رقم مركز التكلفة", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                else
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "Cost Center ID", Comon.cInt(cmbBranchesID.EditValue.ToString()));
            }
            //else if (FocusedControl.Trim() == gridControlProductionExpenses.Name)
            //{
            //    if (GridProductionExpenses.FocusedColumn.Name == "colAccountID")
            //    {
            //        if (GridProductionExpenses.FocusedColumn == null) return;
            //        if (UserInfo.Language == iLanguage.Arabic)
            //            PrepareSearchQuery.Find(ref cls, null, null, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
            //        else
            //            PrepareSearchQuery.Find(ref cls, null, null, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
            //    }
            //}
            else if (FocusedControl.Trim() ==txtDebitAccountID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtDebitAccountID, lblDebitAccountName, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtDebitAccountID, lblDebitAccountName, "AccountID", "Account ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() ==txtCreditAccountID.Name)
            {
 
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCreditAccountID, lblCreditAccountName, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtCreditAccountID, lblCreditAccountName, "AccountID", "Account ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == gridControl1.Name)
            {
                if (gridView1.FocusedColumn.Name == "colAccountID")
                {
                    if (gridView1.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                }
            }
            else if (FocusedControl.Trim() == gridControl4.Name)
            {
                if (gridView8.FocusedColumn.Name == "colAccountID")
                {
                    if (gridView8.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                }
            }

            else if (FocusedControl.Trim() == gridControl2.Name)
            {
                if (gridView3.FocusedColumn.Name == "colAccountID")
                {
                    if (gridView3.FocusedColumn == null) return;
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                }
            }
            GetSelectedSearchValue(cls);
        }

        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {
                if (FocusedControl.Trim() == gridControl4.Name)
                {
                    if (gridView8.FocusedColumn.Name == "colAccountID")
                    {
                        gridView8.AddNewRow();
                        gridView8.SetRowCellValue(gridView8.FocusedRowHandle, "AccountID", cls.PrimaryKeyValue.ToString());
                        string AccountName = Lip.GetValue("SELECT " + PrimaryName + " as AccountName  FROM  [Acc_Accounts] where [BranchID]=" +Comon.cInt( cmbBranchesID.EditValue) + " and Cancel=0 and AccountID=" + cls.PrimaryKeyValue.ToString());
                        if (  AccountName != null && AccountName != "" )
                        {
                            gridView8.SetRowCellValue(gridView8.FocusedRowHandle, "AccountName", AccountName);
                        }
                    }
                 }
                else if (FocusedControl.Trim() == gridControlProductionExpenses.Name)
                {
                    if (GridProductionExpenses.FocusedColumn.Name == "colAccountID")
                    {
                        GridProductionExpenses.AddNewRow();
                        GridProductionExpenses.SetRowCellValue(GridProductionExpenses.FocusedRowHandle, "AccountID", cls.PrimaryKeyValue.ToString());
                        string AccountName = Lip.GetValue("SELECT " + PrimaryName + " as AccountName  FROM  [Acc_Accounts] where [BranchID]=" + Comon.cInt( cmbBranchesID.EditValue) + " and Cancel=0 and AccountID=" + cls.PrimaryKeyValue.ToString());
                        if (AccountName != "" && AccountName != null)
                        {
                            GridProductionExpenses.SetRowCellValue(GridProductionExpenses.FocusedRowHandle, "AccountName", AccountName);
                        }
                    }
                }
                if (FocusedControl.Trim() == gridControl1.Name)
                {
                    if (gridView1.FocusedColumn.Name == "colAccountID")
                    {
                        gridView1.AddNewRow();
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "AccountID", cls.PrimaryKeyValue.ToString());
                        string AccountName = Lip.GetValue("SELECT " + PrimaryName + " as AccountName  FROM  [Acc_Accounts] where [BranchID]=" +Comon.cInt( cmbBranchesID.EditValue) + " and Cancel=0 and AccountID=" + cls.PrimaryKeyValue.ToString());
                        if (AccountName != "" && AccountName != null)
                        {
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "AccountName", AccountName);
                        }
                    }
                }
                if (FocusedControl.Trim() == gridControl2.Name)
                {
                    if (gridView3.FocusedColumn.Name == "colAccountID")
                    {
                        gridView3.AddNewRow();
                        gridView3.SetRowCellValue(gridView3.FocusedRowHandle, "AccountID", cls.PrimaryKeyValue.ToString());
                        string AccountName = Lip.GetValue("SELECT " + PrimaryName + " as AccountName  FROM  [Acc_Accounts] where [BranchID]=" +Comon.cInt( cmbBranchesID.EditValue) + " and Cancel=0 and AccountID=" + cls.PrimaryKeyValue.ToString());
                        if (AccountName != "" && AccountName != null)
                        {
                            gridView3.SetRowCellValue(gridView3.FocusedRowHandle, "AccountName", AccountName);
                        }
                    }
                }
                else if (FocusedControl == txtCostCenterID.Name)
                {
                    txtCostCenterID.Text = cls.PrimaryKeyValue.ToString();
                    txtCostCenterID_Validating(null, null);
                }
                else if (FocusedControl == txtCreditAccountID.Name)
                {
                    txtCreditAccountID.Text = cls.PrimaryKeyValue.ToString();
                    txtCreditAccountID_Validating(null, null);
                }
                else if (FocusedControl == txtDebitAccountID.Name)
                {
                    txtDebitAccountID.Text = cls.PrimaryKeyValue.ToString();
                    txtDebitAccountID_Validating(null, null);
                }
            }

        }
        private void txtCreditAccountID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (txtCreditAccountID.Text.Trim()==txtDebitAccountID.Text.Trim()&&string.IsNullOrEmpty(txtCreditAccountID.Text.Trim())==false)
                {
                   Messages.MsgWarning(Messages.TitleWorning, Messages.msgCanNotChoseSameAccount + " " +txtDebitAccountID.Text.Trim());
                   txtCreditAccountID.Text = "";
                   return;
                }
                strSQL = "SELECT ArbName AS AccountName FROM Acc_Accounts WHERE (BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + " ) AND " + " (Cancel = 0) AND (AccountID = " + txtCreditAccountID.Text + ") ";
                CSearch.ControlValidating(txtCreditAccountID, lblCreditAccountName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void txtDebitAccountID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (txtCreditAccountID.Text.Trim() == txtDebitAccountID.Text.Trim() && string.IsNullOrEmpty(txtDebitAccountID.Text.Trim()) == false)
                {
                    Messages.MsgWarning(Messages.TitleWorning, Messages.msgCanNotChoseSameAccount + " " + txtCreditAccountID.Text.Trim());
                    txtDebitAccountID.Text = "";
                    return;
                }
                strSQL = "SELECT ArbName AS AccountName FROM Acc_Accounts WHERE (BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + " ) AND " + " (Cancel = 0) AND (AccountID = " + txtDebitAccountID.Text + ") ";
                CSearch.ControlValidating(txtDebitAccountID,lblDebitAccountName , strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
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
        private void cmbParentAccountIDVariable_EditValueChanged(object sender, EventArgs e)
        {
           
        }

        private void frmManuExpencessOrder_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the F3 key is pressed and call the Find() function if it is
            if (e.KeyCode == Keys.F3)
                Find();

            // Check if the F9 key is pressed and call the DoSave() function if it is
            if (e.KeyCode == Keys.F9)
                DoSave();
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
        }
        private void Save()
        {
            {
                Menu_ProductionExpensesMaster objRecord = new Menu_ProductionExpensesMaster();
                Menu_ProductionExpensesAcconts objRecordAccount ;
                List<Menu_ProductionExpensesAcconts> listreturnedAccount= new List<Menu_ProductionExpensesAcconts>();
                objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                objRecord.Cancel = 0;
                objRecord.ComandID = Comon.cInt(txtCommandID.Text);
                objRecord.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                objRecord.SalesPriceQram = Comon.cDec(txtSalesPriceQram.Text);
                objRecord.FacilityID = UserInfo.FacilityID;
                objRecord.CommandDate = Comon.ConvertDateToSerial(txtCommandDate.EditValue.ToString());
                objRecord.ToDate = Comon.ConvertDateToSerial(txtToDate.Text.ToString());
                objRecord.FromDate = Comon.ConvertDateToSerial(txtFromDate.Text.ToString());
                objRecord.NumberCups = Comon.cInt(txtNumberCups.Text);
                objRecord.NumberOrder = Comon.cInt(txtNumberOrder.Text);
                objRecord.QTYOrders = Comon.cInt(txtQTYOrders.Text);
                objRecord.QTYGram = Comon.cDec(txtQTYGram.Text);
                objRecord.CategoryOrders = Comon.cInt(cmbCategoryOrders.EditValue);
                objRecord.OrderID = txtOrderID.Text.ToString();
                objRecord.CastingID = Comon.cInt(txtCastingID.Text);
                objRecord.QTYOrder = Comon.cDec(txtQtyOrder.Text);
                objRecord.DebitAccountID = Comon.cDbl(txtDebitAccountID.Text);
                objRecord.CreditAccountID = Comon.cDbl(txtCreditAccountID.Text);               
                objRecord.CurencyID = Comon.cInt(cmbCurency.EditValue);
                objRecord.Posted = Comon.cInt(cmbStatus.EditValue);
                for (int i = 0; i < gridView8.DataRowCount; i++)
                {
                    objRecordAccount = new Menu_ProductionExpensesAcconts();
                    objRecordAccount.AccountID = Comon.cDbl(gridView8.GetRowCellValue(i, "AccountID"));
                    objRecordAccount.AccountName = gridView8.GetRowCellValue(i, "AccountName").ToString();
                    objRecordAccount.ComandID = Comon.cInt(txtCommandID.Text);
                    objRecordAccount.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                    objRecordAccount.Cancel = 0;
                    listreturnedAccount.Add(objRecordAccount);
                }
                //الحسابات
                objRecord.Notes = txtNotes.Text;
                #region Save Compund
                Manu_ProductionExpensesDetails returnedCompund;
                List<Manu_ProductionExpensesDetails> listreturnedCompund = new List<Manu_ProductionExpensesDetails>();
                int lengthExpenss = GridProductionExpenses.DataRowCount;
                int lengthExpenssFixid = gridView1.DataRowCount;
                int lengthExpenssInsstlma = gridView3.DataRowCount;

                for (int i = 0; i <= lengthExpenss - 1; i++)
                {

                    returnedCompund = new Manu_ProductionExpensesDetails();
                    returnedCompund.ComandID = Comon.cInt(txtCommandID.Text.ToString());
                    returnedCompund.AccountID = Comon.cDbl(GridProductionExpenses.GetRowCellValue(i, "AccountID").ToString());

                    returnedCompund.AccountName = GridProductionExpenses.GetRowCellValue(i, "AccountName").ToString();
                    returnedCompund.Debit = Comon.ConvertToDecimalPrice(GridProductionExpenses.GetRowCellValue(i, "Debit").ToString());
                    returnedCompund.Credit = Comon.ConvertToDecimalPrice(GridProductionExpenses.GetRowCellValue(i, "Credit").ToString());
                    returnedCompund.Evaluation = Comon.ConvertToDecimalPrice(GridProductionExpenses.GetRowCellValue(i, "Evaluation").ToString());
                    returnedCompund.TypeExpenssID = 1;
                    returnedCompund.FacilityID = UserInfo.FacilityID;
                    returnedCompund.BranchID = UserInfo.BRANCHID;
                    returnedCompund.Cancel = 0;
                    //  returnedCompund.ComSignature = GridProductionExpenses.GetRowCellValue(i, "ComSignature").ToString();
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
                for (int i = 0; i <= lengthExpenssFixid - 1; i++)
                {
                    returnedCompund = new Manu_ProductionExpensesDetails();
                    returnedCompund.ComandID = Comon.cInt(txtCommandID.Text.ToString());
                    returnedCompund.AccountID = Comon.cDbl(gridView1.GetRowCellValue(i, "AccountID").ToString());

                    returnedCompund.AccountName = gridView1.GetRowCellValue(i, "AccountName").ToString();
                    //returnedCompund.Debit = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Debit").ToString());
                    //returnedCompund.Credit = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Credit").ToString());
                    returnedCompund.Evaluation = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Evaluation").ToString());
                    returnedCompund.TypeExpenssID = 2;
                    returnedCompund.FacilityID = UserInfo.FacilityID;
                    returnedCompund.BranchID = UserInfo.BRANCHID;
                    returnedCompund.Cancel = 0;
                    //  returnedCompund.ComSignature = gridView1.GetRowCellValue(i, "ComSignature").ToString();
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
                for (int i = 0; i <= lengthExpenssInsstlma - 1; i++)
                {
                    returnedCompund = new Manu_ProductionExpensesDetails();
                    returnedCompund.ComandID = Comon.cInt(txtCommandID.Text.ToString());
                    returnedCompund.AccountID = Comon.cDbl(gridView3.GetRowCellValue(i, "AccountID").ToString());
                    returnedCompund.AccountName = gridView3.GetRowCellValue(i, "AccountName").ToString();
                    //returnedCompund.Debit = Comon.ConvertToDecimalPrice(gridView3.GetRowCellValue(i, "Debit").ToString());
                    //returnedCompund.Credit = Comon.ConvertToDecimalPrice(gridView3.GetRowCellValue(i, "Credit").ToString());
                    returnedCompund.Evaluation = Comon.ConvertToDecimalPrice(gridView3.GetRowCellValue(i, "Evaluation").ToString());
                    returnedCompund.TypeExpenssID = 3;
                    returnedCompund.FacilityID = UserInfo.FacilityID;
                    returnedCompund.BranchID = UserInfo.BRANCHID;
                    returnedCompund.Cancel = 0;
                    //  returnedCompund.ComSignature = gridView3.GetRowCellValue(i, "ComSignature").ToString();
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
                #endregion
                if (listreturnedCompund.Count > 0)
                {
                    objRecord.Manu_ProductionExpenses = listreturnedCompund;
                    objRecord.Manu_AccountDetils = listreturnedAccount;
                    string Result = Menu_ProductionExpensesMasterDAL.InsertUsingXML(objRecord, IsNewRecord).ToString();
                    //حفظ القيد الالي
                    if (Comon.cInt(Result) > 0 && Comon.cInt(cmbStatus.EditValue)>1)
                    {
                    //حفظ القيد الالي
                        long VoucherID = SaveVariousVoucherMachin(Comon.cInt(Result), IsNewRecord);
                        if (VoucherID == 0)
                            Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
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

        long SaveVariousVoucherMachin(int DocumentID, bool isNew)
        {
            int VoucherID = 0;
            long Result = 0;
            Acc_VariousVoucherMachinMaster objRecord = new Acc_VariousVoucherMachinMaster();
            objRecord.DocumentType = DocumentTypeCommandCost;
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " And DocumentType=" + objRecord.DocumentType + " And BranchID=" + Comon.cInt(cmbBranchesID.EditValue)));

            objRecord.VoucherID = VoucherID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = MySession.GlobalFacilityID;
            //Date
            objRecord.VoucherDate = Comon.ConvertDateToSerial(txtOrderDate.Text).ToString();
            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
            //objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);
            //objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
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

            //Debit Gold        
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 1;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(txtDebitAccountID.Text);
            returned.VoucherID = VoucherID; 
            returned.Debit = Comon.cDbl(txtTotalCostOrder.Text);
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);

            returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
            returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
            returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Debit) * Comon.cDbl(returned.CurrencyPrice)); 
            listreturned.Add(returned);
            //Credit Gold      
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 1;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(txtCreditAccountID.Text);
            returned.VoucherID = VoucherID;
            returned.Credit = Comon.cDbl(txtTotalCostOrder.Text); 
            returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);

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
        #region Do  Function
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
                System.Windows.Forms.Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                int TempID = Comon.cInt(txtCommandID.Text);
                Menu_ProductionExpensesMaster model = new Menu_ProductionExpensesMaster();
                model.ComandID = Comon.cInt(txtCommandID.Text);
                model.EditUserID = UserInfo.ID;
                model.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                model.FacilityID = UserInfo.FacilityID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());

                string Result = Menu_ProductionExpensesMasterDAL.Delete(model);
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

            EnabledControl(true);

            Validations.DoEditRipon(this, ribbonControl1);
        }
        protected override void DoSave()
        {
            try
            {
                if (!Validations.IsValidForm(this))
                    return;
                if (!Validations.IsValidForm(groupBox2))
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

                if (!Lip.CheckTheProcessesIsPosted("Menu_ProductionExpensesMaster", Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(cmbStatus.EditValue), Comon.cLong(txtCommandID.Text), PrimeryColName: "CommandID"))
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
        protected override void DoNew()
        {
            try
            {
                IsNewRecord = true;
                txtCommandID.Text = Menu_ProductionExpensesMasterDAL.GetNewID(MySession.GlobalFacilityID,Comon.cInt(cmbBranchesID.EditValue)).ToString();
                txtCommandID.ReadOnly= false;
                InitializeFormatDate(txtFromDate);
                InitializeFormatDate(txtToDate);
                InitializeFormatDate(txtCommandDate);
                InitializeFormatDate(txtOrderDate);
                InitializeFormatDate(txtCommandCastingDate);
                ClearFields();
                EnabledControl(true);
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        #endregion

        private void label42_Click(object sender, EventArgs e)
        { }
        private void txtFromDate_EditValueChanged(object sender, EventArgs e)
        {
            if (Lip.CheckDateISAvilable(((DateEdit)sender).Text))
            {
                Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheDateGreaterCurrntDate);
                ((DateEdit)sender).Text = Lip.GetServerDate();
                return;
            }
            if (Comon.cInt(gridView8.DataRowCount) > 0)
              btnShow_Click();                       
        }
        private void btnFactory_Click(object sender, EventArgs e)
        {
            if (Comon.cInt(gridView8.DataRowCount) > 0)
            {
                btnShow_Click();
                GridProductionExpenses_RowUpdated(null, null);
            }
            else
                Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "الرجاء ادخال حسابات الرئيسية المباشرة  ومن ثم انزال المصروفات " : "Select Accounts and try agine ");            
        }
        private void btnMachinResractionAdditionalAfter_Click(object sender, EventArgs e)
        {
            if (IsNewRecord == true) return;
            int ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + txtCommandID.Text + " And DocumentType=" + DocumentTypeCommandCost).ToString());
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
                Messages.MsgError("تنبيه", "لا يوجد قيد - الرجاء اعادة حفظ المستند");
        }

        private void txtQtyOrder_EditValueChanged(object sender, EventArgs e)
        {
            GridProductionExpenses_RowUpdated(null, null);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = Lip.SelectRecord("SELECT   [Acc_DeclaringFixedSpends].*,Acc_Accounts.ArbName as AccountName FROM  [Acc_DeclaringFixedSpends] inner join Acc_Accounts on Acc_DeclaringFixedSpends.AccountID=Acc_Accounts.AccountID and Acc_DeclaringFixedSpends.BranchID=Acc_Accounts.BranchID where Acc_DeclaringFixedSpends.[BranchID]=" + Comon.cInt(cmbBranchesID.EditValue));
                gridControl1.DataSource = dt;
                lstDetailFixedExpenses.AllowNew = true;
                lstDetailFixedExpenses.AllowEdit = true;
                lstDetailFixedExpenses.AllowRemove = true;
                GridProductionExpenses_RowUpdated(null, null);
            }
            catch { }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = Lip.SelectRecord("SELECT   [Acc_DeclaringEstimatedSpends].*,Acc_Accounts.ArbName as AccountName FROM  [Acc_DeclaringEstimatedSpends] inner join Acc_Accounts on Acc_DeclaringEstimatedSpends.AccountID=Acc_Accounts.AccountID and Acc_DeclaringEstimatedSpends.BranchID=Acc_Accounts.BranchID where Acc_DeclaringEstimatedSpends.[BranchID]=" + Comon.cInt(cmbBranchesID.EditValue));
                gridControl2.DataSource = dt;
                lstDetailEstimatedExpenses.AllowNew = true;
                lstDetailEstimatedExpenses.AllowEdit = true;
                lstDetailEstimatedExpenses.AllowRemove = true;
                GridProductionExpenses_RowUpdated(null, null);
            }
            catch (Exception)
            {
 
            }
         
        }

        private void btnCompond_Click(object sender, EventArgs e)
        {
            frmClosingOrders frm = new frmClosingOrders();
            if (Edex.ModelSystem.Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();

            }
            else
                frm.Dispose();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            frmManufacturingDismantOrders frm = new frmManufacturingDismantOrders();
            if (Edex.ModelSystem.Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();

            }
            else
                frm.Dispose();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            frmDeclaringFixedSpends frm = new frmDeclaringFixedSpends();
            if (Edex.ModelSystem.Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();

            }
            else
                frm.Dispose();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            frmDeclaringEstimatedSpends frm = new frmDeclaringEstimatedSpends();
            if (Edex.ModelSystem.Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
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
            int isLocalCurrncy = Comon.cInt(Lip.GetValue("select TypeCurrency from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0  and BranchID=" + Comon.cInt(cmbBranchesID.EditValue)));
            if (isLocalCurrncy > 1)
            {
                decimal CurrncyPrice = Comon.cDec(Lip.GetValue("select ExchangeRate from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0 and BranchID=" + Comon.cInt(cmbBranchesID.EditValue)));
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

        private void txtCommandDate_EditValueChanged(object sender, EventArgs e)
        {
            if (Lip.CheckDateISAvilable(((DateEdit)sender).Text))
            {
                Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheDateGreaterCurrntDate);
                ((DateEdit)sender).Text = Lip.GetServerDate();
                return;
            }
        }
    }
}
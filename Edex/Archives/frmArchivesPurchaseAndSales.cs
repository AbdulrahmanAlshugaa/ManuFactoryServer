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
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralClasses;
using Edex.ModelSystem;
using System.Net;
using HtmlAgilityPack;
using DevExpress.XtraGrid.Views.Grid;
using Edex.AccountsObjects.Transactions;
using Edex.SalesAndPurchaseObjects.Transactions;
using Edex.SalesAndSaleObjects.Transactions;

namespace Edex.Archives
{
    public partial class frmArchivesPurchaseAndSales : BaseForm
    {
        #region Declare
        private string strSQL = "";
        private string where = "";
        private string lang = "";
        private string FocusedControl = "";
        private string PrimaryName;
        DataTable dtFactoryOprationType = new DataTable();
        public DataTable _sampleData = new DataTable();
        DataTable dt = new DataTable();
        #endregion
        public frmArchivesPurchaseAndSales()
        {
            InitializeComponent();
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[9].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[12].Visible = true;
            InitializeFormatDate(txtFromDate);
            InitializeFormatDate(txtToDate);
            gridView1.OptionsBehavior.ReadOnly = true;
            gridView1.OptionsBehavior.Editable = false;
            PrimaryName = "ArbName";
            if(UserInfo.Language==iLanguage.English)
            {
                PrimaryName = "EngName";
            }
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = true;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Caption = (UserInfo.Language == iLanguage.Arabic ? "استعلام جديد" : "New Query");
            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
            FillCombo.FillComboBoxLookUpEdit(cmbFromType, "Arc_ArchivesType", "ID", PrimaryName, "", "ID<5", (UserInfo.Language == iLanguage.English ? "Select Type" : "حدد النوع "));
            FillCombo.FillComboBoxLookUpEdit(cmbToType, "Arc_ArchivesType", "ID", PrimaryName, "", "ID<5", (UserInfo.Language == iLanguage.English ? "Select Type" : "حدد النوع "));
            cmbBranchesID.EditValue = MySession.GlobalBranchID;
            cmbBranchesID.ReadOnly = !MySession.GlobalAllowBranchModificationAllScreens;
            FillCombo.FillComboBoxLookUpEdit(cmbStatus, "Manu_TypeStatus", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select StatUs" : "حدد الحالة "));
            FillCombo.FillComboBox(cmbPaymentMethod, "Sales_PurchaseMethods", "MethodID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد الدفع"));
            FillCombo.FillComboBox(cmbCurency, "Acc_Currency", "ID", PrimaryName, "", "BranchID=" +MySession.GlobalBranchID, (UserInfo.Language == iLanguage.English ? "Select " : "حدد العملة"));

            FillCombo.FillComboBox(cmbDiscountID, "Arc_DiscountType", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد الخصم "));
            FillCombo.FillComboBox(cmbExpensesID, "Arc_ExpensessType", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد النوع "));
            FillCombo.FillComboBox(cmbHaveAddtional, "Arc_AddtionalType", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد النوع "));

            this.KeyDown += frmArchivesPurchaseAndSales_KeyDown;
            this.txtStoreID.Validating+=txtStoreID_Validating;
            this.txtCustomerOrSupplierID.Validating += txtCustomerOrSupplierID_Validating;
            this.txtCostCenterID.Validating+=txtCostCenterID_Validating;
            this.txtDelegeteID.Validating+=txtDelegeteID_Validating;
            this.txtBankID.Validating+=txtBankID_Validating;
            this.txtBoxID.Validating += txtBoxID_Validating;
            this.Load += frmArchivesPurchaseAndSales_Load;

            this.txtUserIDEntry.Validating += txtUserIDEntry_Validating;
            this.txtUserIDUpdated.Validating+=txtUserIDUpdated_Validating;

            this.gridView1.RowCellStyle += gridView1_RowCellStyle;
            this.gridView1.DoubleClick += gridView1_DoubleClick;
        }
        void gridView1_DoubleClick(object sender, EventArgs e)
        {

            try
            {
                GridView view = sender as GridView;

                switch (view.GetFocusedRowCellValue("TypeOpration").ToString())
                {
                    case "1":
                        frmCashierPurchaseMatirial frm = new frmCashierPurchaseMatirial();
                        if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID,  MySession.GlobalBranchID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm);
                            frm.Show();
                            frm.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                        }
                        else
                            frm.Dispose();
                        break;
                    case "2":
                        frmCashierPurchaseReturnMatirial frmReturn = new frmCashierPurchaseReturnMatirial();
                        if (Permissions.UserPermissionsFrom(frmReturn, frmReturn.ribbonControl1, UserInfo.ID,  MySession.GlobalBranchID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frmReturn);
                            frmReturn.Show();
                            frmReturn.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                        }
                        else
                            frmReturn.Dispose();
                        break;
                    case "3":
                        frmCashierSalesAlmas frmSales = new frmCashierSalesAlmas();
                        if (Permissions.UserPermissionsFrom(frmSales, frmSales.ribbonControl1, UserInfo.ID,  MySession.GlobalBranchID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frmSales);
                            frmSales.Show();
                            frmSales.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                        }
                        else
                            frmSales.Dispose();
                        break;
                    case "4":
                        frmSalesInvoiceReturn frmSalesReturn = new frmSalesInvoiceReturn();
                        if (Permissions.UserPermissionsFrom(frmSalesReturn, frmSalesReturn.ribbonControl1, UserInfo.ID,  MySession.GlobalBranchID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frmSalesReturn);
                            frmSalesReturn.Show();
                            frmSalesReturn.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                        }
                        else
                            frmSalesReturn.Dispose();
                        break;
                    case "5":
                        frmCashierPurchaseServicesEqv frmPurchaseEqv = new frmCashierPurchaseServicesEqv();
                        if (Permissions.UserPermissionsFrom(frmPurchaseEqv, frmPurchaseEqv.ribbonControl1, UserInfo.ID,  MySession.GlobalBranchID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frmPurchaseEqv);
                            frmPurchaseEqv.Show();
                            frmPurchaseEqv.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                        }
                        else
                            frmPurchaseEqv.Dispose();
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }
        void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName == "StatUs")   
            {
                if (gridView1.GetRowCellValue(e.RowHandle, "StatUs").ToString() == (UserInfo.Language == iLanguage.Arabic ? "محذوف" : "Deleted"))
                {
                    e.Appearance.BackColor = Color.Red;   
                }
                else
                { 
                    e.Appearance.BackColor = e.Appearance.BackColor;
                }
            }
        }

        void txtUserIDEntry_Validating(object sender, CancelEventArgs e)
        {
          
            try
            {
                strSQL = "SELECT " + PrimaryName + " as UserName FROM Users WHERE UserID =" + Comon.cInt(txtUserIDEntry.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtUserIDEntry,lblUserNameEntry, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
         
        }
        void txtUserIDUpdated_Validating(object sender, CancelEventArgs e)
        {

            try
            {
                strSQL = "SELECT " + PrimaryName + " as UserName FROM Users WHERE UserID =" + Comon.cInt(txtUserIDUpdated.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtUserIDUpdated, lblUserNameUpdated, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }

        void frmArchivesPurchaseAndSales_Load(object sender, EventArgs e)
        {
            _sampleData.Columns.Add(new DataColumn("Sn", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("RecordType", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("ID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("RefranceID", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("CurrncyName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("NetAmmount", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("SupplierOrCustomer", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("BoxAndBank", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("StoreName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("PaymentMethod", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("StatUs", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("UserName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("UserNameUpdate", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("CostCenterName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("TypeOpration", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Notes", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("AdditionaAmountTotal", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("TotalBeforeAddtional", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("TypeInvoice", typeof(int)));
           
       }

        

        void txtBoxID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if(Comon.cInt( cmbBranchesID.EditValue)!=0)
                    strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE  (Cancel = 0) AND (AccountID = " + txtBoxID.Text + ") and BranchID="+cmbBranchesID.EditValue;
                else
                    strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE  (Cancel = 0) AND (AccountID = " + txtBoxID.Text + ")";
                

                CSearch.ControlValidating(txtBoxID, lblBoxeName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        void txtCustomerOrSupplierID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string strSql;
                DataTable dt;
                if (txtCustomerOrSupplierID.Text != string.Empty && txtCustomerOrSupplierID.Text != "0")
                {
                    if (Comon.cInt(cmbBranchesID.EditValue) != 0)
                        strSQL = "SELECT " + PrimaryName + " as CustomerName ,VATID,Mobile FROM Sales_CustomerAnSublierListArb Where    AcountID =" + txtCustomerOrSupplierID.Text + " and BranchID=" + cmbBranchesID.EditValue;
                    else
                        strSQL = "SELECT " + PrimaryName + " as CustomerName ,VATID,Mobile FROM Sales_CustomerAnSublierListArb Where    AcountID =" + txtCustomerOrSupplierID.Text;

                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, UserInfo.Language.ToString());
                    dt = Lip.SelectRecord(strSQL);
                    if (dt.Rows.Count > 0)
                    {
                        lblCustomerOrSupplierName.Text = dt.Rows[0]["CustomerName"].ToString();
                        //txtCustomerMobile.Text = dt.Rows[0]["Mobile"].ToString();
                    }
                    else
                    {
                        if (Comon.cInt(cmbBranchesID.EditValue) != 0)
                           strSql = "SELECT " + PrimaryName + " as CustomerName,SpecialDiscount,VATID, CustomerID,Mobile   FROM Sales_Customers Where  Cancel =0 And  AccountID =" + txtCustomerOrSupplierID.Text + " and BranchID=" + cmbBranchesID.EditValue;
                        else
                            strSql = "SELECT " + PrimaryName + " as CustomerName,SpecialDiscount,VATID, CustomerID,Mobile   FROM Sales_Customers Where  Cancel =0 And  AccountID =" + txtCustomerOrSupplierID.Text ;

                        Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSql, UserInfo.Language.ToString());
                        dt = Lip.SelectRecord(strSql);
                        if (dt.Rows.Count > 0)
                        {
                            lblCustomerOrSupplierName.Text = dt.Rows[0]["CustomerName"].ToString();
                        }
                        else
                        {
                            lblCustomerOrSupplierName.Text = "";
                            txtCustomerOrSupplierID.Text = "";
                        }
                    }
                }
                else
                {
                    lblCustomerOrSupplierName.Text = "";
                    txtCustomerOrSupplierID.Text = "";
                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        
        void frmArchivesPurchaseAndSales_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                Find();
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
        private void txtBankID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if(Comon.cInt(cmbBranchesID.EditValue)!=0)
                    strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE  (Cancel = 0) AND (AccountID = " + txtBankID.Text + ") and BranchID="+MySession.GlobalBranchID;
                else

                    strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE  (Cancel = 0) AND (AccountID = " + txtBankID.Text + ") ";
                CSearch.ControlValidating(txtBankID, lblBankName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public void Find()
        {

            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = "Where 1=1";

            FocusedControl = GetIndexFocusedControl();

            if (FocusedControl == null) return;
            if (FocusedControl.Trim() ==txtCustomerOrSupplierID.Name)
            {
                if (!MySession.GlobalAllowChangefrmPurchaseSupplierID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCustomerOrSupplierID, lblCustomerOrSupplierName, "CustomerIDAndSublierID", "رقم الــعـــمـــيــل", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtCustomerOrSupplierID, lblCustomerOrSupplierName, "CustomerIDAndSublierID", "Customer ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtStoreID.Name)
            {
                if (!MySession.GlobalAllowChangefrmPurchaseStoreID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "Account ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() ==txtUserIDEntry.Name)
            {
                
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtUserIDEntry, lblUserNameEntry, "UserID", "رقم المستخدم", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtUserIDEntry, lblUserNameEntry, "UserID", "User ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() ==txtUserIDUpdated.Name)
            {

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtUserIDUpdated, lblUserNameUpdated, "UserID", "رقم المستخدم", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtUserIDUpdated, lblUserNameUpdated, "UserID", "User ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() ==txtBankID.Name)
            {
                if (!MySession.GlobalAllowChangefrmPurchaseStoreID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtBankID, lblBankName, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtBankID, lblBankName, "AccountID", "Account ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() ==txtBoxID.Name)
            {
                if (!MySession.GlobalAllowChangefrmPurchaseStoreID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtBoxID,lblBoxeName, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtBoxID, lblBoxeName, "AccountID", "Account ID", MySession.GlobalBranchID);
            }
         
            else if (FocusedControl.Trim() ==txtDelegeteID.Name)
            {
                if (!MySession.GlobalAllowChangefrmPurchaseDelegateID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtDelegeteID, lblDelegeteName, "SaleDelegateID", "رقم مندوب المبيعات", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtDelegeteID, lblDelegeteName, "SaleDelegateID", "Delegate ID", MySession.GlobalBranchID);
            }
            
            else if (FocusedControl.Trim() == txtCostCenterID.Name)
            {
                if (!MySession.GlobalAllowChangefrmPurchaseCostCenterID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "رقم مركز التكلفة", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "Cost Center ID", MySession.GlobalBranchID);
            }
            
         GetSelectedSearchValue(cls);
        }
        private void txtStoreID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if(Comon.cInt(cmbBranchesID.EditValue)!=0)
                     strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE AccountID =" + Comon.cLong(txtStoreID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                else  strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE AccountID =" + Comon.cLong(txtStoreID.Text) + " And Cancel =0 ";
                CSearch.ControlValidating(txtStoreID, lblStoreName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }


        private void txtCostCenterID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (Comon.cInt(cmbBranchesID.EditValue) != 0)
                strSQL = "SELECT " + PrimaryName + " as CostCenterName FROM Acc_CostCenters WHERE CostCenterID =" + Comon.cInt(txtCostCenterID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                else 
                    strSQL = "SELECT " + PrimaryName + " as CostCenterName FROM Acc_CostCenters WHERE CostCenterID =" + Comon.cInt(txtCostCenterID.Text) + " And Cancel =0 ";
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
               if (FocusedControl ==txtCustomerOrSupplierID.Name)
                {
                    txtCustomerOrSupplierID.Text = cls.PrimaryKeyValue.ToString();
                    txtCustomerOrSupplierID_Validating(null, null);
                }
               if (FocusedControl ==txtBoxID.Name)
               {
                   txtBoxID.Text = cls.PrimaryKeyValue.ToString();
                   txtBoxID_Validating(null, null);
               }
               if (FocusedControl ==txtBankID.Name)
               {
                   txtBankID.Text = cls.PrimaryKeyValue.ToString();
                   txtBankID_Validating(null, null);
               }
               if (FocusedControl ==txtUserIDEntry.Name)
               {
                   txtUserIDEntry.Text = cls.PrimaryKeyValue.ToString();
                   txtUserIDEntry_Validating(null, null);
               }
               if (FocusedControl ==txtUserIDUpdated.Name)
               {
                   txtUserIDUpdated.Text = cls.PrimaryKeyValue.ToString();
                   txtUserIDUpdated_Validating(null, null);
               }
                else if (FocusedControl == txtStoreID.Name)
                {
                    txtStoreID.Text = cls.PrimaryKeyValue.ToString();
                    txtStoreID_Validating(null, null);
                }
                else if (FocusedControl == txtCostCenterID.Name)
                {
                    txtCostCenterID.Text = cls.PrimaryKeyValue.ToString();
                    txtCostCenterID_Validating(null, null);
                }               
               else if (FocusedControl ==txtDelegeteID.Name)
                {
                    txtDelegeteID.Text = cls.PrimaryKeyValue.ToString();
                    txtDelegeteID_Validating(null, null);
                }
               }

        }
        private void txtDelegeteID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (Comon.cInt(cmbBranchesID.EditValue) != 0)
                strSQL = "SELECT " + PrimaryName + " as DelegateName FROM Sales_SalesDelegate WHERE DelegateID =" +txtDelegeteID.Text + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                else
                    strSQL = "SELECT " + PrimaryName + " as DelegateName FROM Sales_SalesDelegate WHERE DelegateID =" + txtDelegeteID.Text + " And Cancel =0 ";

                CSearch.ControlValidating(txtDelegeteID, lblDelegeteName, strSQL);
            }
            catch (Exception ex)
            {
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
        protected override void DoAddFrom()
        {
            try
            {
                _sampleData.Clear();
                gridControl1.RefreshDataSource();
                txtToTransactionsID.Text = "";
                txtFromTransactionID.Text = "";
                txtDelegeteID.Text = "";
                txtDelegeteID_Validating(null, null);
                txtBankID.Text = "";
                txtBankID_Validating(null, null);
                txtBoxID.Text = "";
                txtBoxID_Validating(null, null);
                txtCostCenterID.Text = "";
                txtCostCenterID_Validating(null, null);
                txtCustomerOrSupplierID.Text = "";
                txtCustomerOrSupplierID_Validating(null, null);
                txtReferanceID.Text = "";
                txtStoreID.Text = "";
                txtStoreID_Validating(null, null);
                txtUserIDEntry.Text = "";
                txtUserIDUpdated.Text = "";

                txtFromDate.Text = "";
                txtToDate.Text = "";
                txtCostCenterID.Text = "";
                lblCostCenterName.Text = "";
                
                txtFromDate.Enabled = true;
                txtToDate.Enabled = true;
                txtCostCenterID.Enabled = true;

                txtStoreID.Enabled = true;
                txtCostCenterID.Enabled = true;
                txtCustomerOrSupplierID.Enabled = true;
                cmbPaymentMethod.Enabled = true;
                txtFromDate.Enabled = true;
                txtToDate.Enabled = true;
                txtToTransactionsID.Enabled = true;
                txtFromTransactionID.Enabled = true;

                gridControl1.DataSource = _sampleData;

            }
            catch (Exception ex)
            {
                //WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        
        string GetStrSQL()
        {

            
            Application.DoEvents();

            string filter = " ";
            strSQL = "";

            if (Comon.cInt(cmbBranchesID.EditValue) > 0)
                filter = "(.Sales_PurchaseInvoiceMaster.BranchID = " + cmbBranchesID.EditValue + ") AND dbo.Sales_PurchaseInvoiceMaster.InvoiceID >0     AND ";
            else
                filter = " dbo.Sales_PurchaseInvoiceMaster.InvoiceID >0    AND ";
            long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
            long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));

            DataTable dt;
            
            // حسب الرقم
            if (Comon.cInt(cmbStatus.EditValue) == 1)
                filter = filter + " dbo.Sales_PurchaseInvoiceMaster.Cancel =1 AND ";

            if (txtFromTransactionID.Text != string.Empty)
                filter = filter + " dbo.Sales_PurchaseInvoiceMaster.InvoiceID >=" + txtFromTransactionID.Text + " AND ";
            if(Comon.cInt(cmbHaveAddtional.EditValue)==1)
                filter = filter + " dbo.Sales_PurchaseInvoiceMaster.AdditionaAmountTotal>0 AND ";
            else if (Comon.cInt(cmbHaveAddtional.EditValue) == 2)
                filter = filter + " dbo.Sales_PurchaseInvoiceMaster.AdditionaAmountTotal<=0 AND ";
            else if (Comon.cInt(cmbCurency.EditValue) >0)
                filter = filter + " dbo.Sales_PurchaseInvoiceMaster.CurrencyID=" + Comon.cInt(cmbCurency.EditValue) + " AND ";
             if (Comon.cInt(txtUserIDEntry.Text) > 0)
                filter = filter + " Sales_PurchaseInvoiceMaster.UserID=" + Comon.cInt(txtUserIDEntry.Text) + " AND ";
             if (Comon.cInt(txtUserIDUpdated.Text) > 0)
                filter = filter + " Sales_PurchaseInvoiceMaster.EditUserID=" + Comon.cInt(txtUserIDUpdated.Text) + " AND ";
            if (txtToTransactionsID.Text != string.Empty)
                filter = filter + " dbo.Sales_PurchaseInvoiceMaster.InvoiceID <=" + txtToTransactionsID.Text + " AND ";
            if (txtReferanceID.Text.Trim() != string.Empty)
                filter = filter + "  Sales_PurchaseInvoiceMaster.DocumentID=" + txtReferanceID.Text + " AND ";

            if(Comon.cDbl(txtBankID.Text)>0)
                filter = filter + "  Sales_PurchaseInvoiceMaster.CreditAccount=" + Comon.cDbl(txtBankID.Text) + " AND ";
            if (Comon.cDbl(txtBoxID.Text) > 0)
                filter = filter + "  Sales_PurchaseInvoiceMaster.CreditAccount=" + Comon.cDbl(txtBoxID.Text) + " AND ";
            // حسب التاريخ
            if (FromDate != 0)
                filter = filter + " dbo.Sales_PurchaseInvoiceMaster.InvoiceDate >=" + FromDate + " AND ";

            if (ToDate != 0)
                filter = filter + " dbo.Sales_PurchaseInvoiceMaster.InvoiceDate <=" + ToDate + " AND ";

            // '''البائع''''العميل''''التكلفة''''المستودع
            if (txtStoreID.Text != string.Empty)
                filter = filter + " dbo.Sales_PurchaseInvoiceMaster.StoreID  =" + Comon.cLong(txtStoreID.Text) + "  AND ";

            if (txtCostCenterID.Text != string.Empty)
                filter = filter + " dbo.Sales_PurchaseInvoiceMaster.CostCenterID  =" + Comon.cInt(txtCostCenterID.Text) + "  AND ";

            if (txtCustomerOrSupplierID.Text != string.Empty)
                filter = filter + " dbo.Sales_PurchaseInvoiceMaster.SupplierID  =" + Comon.cLong(txtCustomerOrSupplierID.Text) + "  AND ";
            if (cmbPaymentMethod.Text != string.Empty&&Comon.cInt(cmbPaymentMethod.EditValue)>0)
                filter = filter + " Sales_PurchaseInvoiceMaster.MethodeID =" + cmbPaymentMethod.EditValue + " AND ";
            // '''''''''''''
            filter = filter.Remove(filter.Length - 4, 4);

            strSQL = "SELECT '1' as TypeOpration,Sales_PurchaseInvoiceMaster.TypeInvoice,dbo.Acc_Currency." + PrimaryName + " as CurrncyName,dbo.Users." + PrimaryName + " as UserName,dbo.Users." + PrimaryName + " as UserNameUpdate,Acc_Accounts." + PrimaryName + " as BoxAndBank, Sales_PurchaseInvoiceMaster.DocumentID, dbo.Sales_PurchaseInvoiceMaster.Cancel,   Sales_PurchaseInvoiceMaster.NetProcessID,dbo.Sales_PurchaseInvoiceMaster.MethodeID, Sales_PurchaseInvoiceMaster.NetAmount,  dbo.Sales_PurchaseInvoiceMaster.AdditionaAmountTotal As SumVat, dbo.Sales_PurchaseInvoiceMaster.InvoiceID, dbo.Sales_PurchaseInvoiceMaster.BranchID, dbo.Sales_PurchaseInvoiceMaster.DiscountOnTotal,"
            + " dbo.Sales_PurchaseInvoiceMaster.InvoiceDate, Sales_PurchaseInvoiceMaster.NetBalance AS total, "
            + "  dbo.Stc_Stores." + PrimaryName + " AS StoreName, dbo.Sales_PurchaseInvoiceMaster.Notes, "
            + " dbo.Sales_PurchasesDelegate." + PrimaryName + " AS DelegateName,  dbo.Sales_Suppliers." + PrimaryName + " AS SupplierName, dbo.Sales_PurchaseMethods.ArbName AS MethodeName,"
            + " dbo.Acc_CostCenters.ArbName AS CostCenter FROM dbo.Sales_PurchaseInvoiceMaster INNER JOIN dbo.Sales_PurchaseInvoiceDetails ON dbo.Sales_PurchaseInvoiceMaster.InvoiceID"
            + " = dbo.Sales_PurchaseInvoiceDetails.InvoiceID AND dbo.Sales_PurchaseInvoiceMaster.BranchID = dbo.Sales_PurchaseInvoiceDetails.BranchID LEFT OUTER JOIN"
            + " dbo.Acc_CostCenters ON dbo.Sales_PurchaseInvoiceMaster.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Sales_PurchaseInvoiceMaster.CostCenterID = "
            + " dbo.Acc_CostCenters.CostCenterID LEFT OUTER JOIN dbo.Sales_Suppliers ON dbo.Sales_PurchaseInvoiceMaster.BranchID = dbo.Sales_Suppliers.BranchID AND "
            + " dbo.Sales_PurchaseInvoiceMaster.SupplierID = dbo.Sales_Suppliers.AccountID LEFT OUTER JOIN dbo.Sales_PurchasesDelegate ON dbo.Sales_PurchaseInvoiceMaster.BranchID"
            + " = dbo.Sales_PurchasesDelegate.BranchID AND dbo.Sales_PurchaseInvoiceMaster.DelegateID = dbo.Sales_PurchasesDelegate.DelegateID and dbo.Sales_PurchaseInvoiceMaster.BranchID = dbo.Sales_PurchasesDelegate.BranchID LEFT OUTER JOIN"
            + " dbo.Stc_Stores ON dbo.Sales_PurchaseInvoiceMaster.BranchID = dbo.Stc_Stores.BranchID AND dbo.Sales_PurchaseInvoiceMaster.StoreID = dbo.Stc_Stores.AccountID LEFT OUTER JOIN"
            + " dbo.Sales_PurchaseMethods ON dbo.Sales_PurchaseInvoiceMaster.MethodeID = dbo.Sales_PurchaseMethods.MethodID "
            + "  LEFT OUTER JOIN Users on Sales_PurchaseInvoiceMaster.UserID=Users.UserID and  Sales_PurchaseInvoiceMaster.BranchID=Users.BranchID or Sales_PurchaseInvoiceMaster.EditUserID=Users.UserID "
            + "  LEFT OUTER JOIN Acc_Accounts on Sales_PurchaseInvoiceMaster.CreditAccount=Acc_Accounts.AccountID and Sales_PurchaseInvoiceMaster.BranchID=Acc_Accounts.BranchID "
            + " LEFT OUTER JOIN Acc_Currency on Sales_PurchaseInvoiceMaster.CurrencyID=Acc_Currency.ID and Sales_PurchaseInvoiceMaster.BranchID=Acc_Currency.BranchID where " + filter + " GROUP BY "
            + "   Sales_PurchaseInvoiceMaster.GoldUsing ,Sales_PurchaseInvoiceMaster.NetBalance , Sales_PurchaseInvoiceMaster.NetProcessID,dbo.Sales_PurchaseInvoiceMaster.MethodeID, Sales_PurchaseInvoiceMaster.NetAmount,  dbo.Sales_PurchaseInvoiceMaster.InvoiceID,dbo.Sales_PurchaseInvoiceMaster.AdditionaAmountTotal ,"
            + " dbo.Sales_PurchaseInvoiceMaster.Cancel,Sales_PurchaseInvoiceMaster.TypeInvoice, dbo.Sales_PurchaseInvoiceMaster.BranchID,dbo.Sales_PurchaseInvoiceMaster.DiscountOnTotal, dbo.Sales_PurchaseInvoiceMaster.InvoiceDate,   "
            + " dbo.Stc_Stores.ArbName, dbo.Sales_PurchaseInvoiceMaster.Notes, dbo.Sales_PurchasesDelegate.ArbName, dbo.Sales_Suppliers.ArbName,Sales_PurchaseInvoiceMaster.DocumentID, "
            + " dbo.Sales_PurchaseMethods." + PrimaryName + ",dbo.Acc_Currency." + PrimaryName + ", dbo.Acc_CostCenters." + PrimaryName + ",dbo.Users." + PrimaryName + ",Acc_Accounts." + PrimaryName;
            Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

            return strSQL;


        }
        string GetStrSQLPurchaseReturn()
        {

        
            Application.DoEvents();
            string filter ="";
             strSQL = "";

            if (Comon.cInt(cmbBranchesID.EditValue) > 0)
                filter = "(.Sales_PurchaseInvoiceReturnMaster.BranchID = " + cmbBranchesID.EditValue + ") AND dbo.Sales_PurchaseInvoiceReturnMaster.InvoiceID >0 AND ";
            else 
                 filter = "  dbo.Sales_PurchaseInvoiceReturnMaster.InvoiceID >0    AND ";
           
            long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
            long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));

            DataTable dt;
            // Dim dtMethodeName As DataTable
            // حسب الرقم
            if (Comon.cInt(txtUserIDEntry.Text) > 0)
                filter = filter + " Sales_PurchaseInvoiceReturnMaster.UserID=" + Comon.cInt(txtUserIDEntry.Text) + " AND ";
            if (Comon.cInt(txtUserIDUpdated.Text) > 0)
                filter = filter + " Sales_PurchaseInvoiceReturnMaster.EditUserID=" + Comon.cInt(txtUserIDUpdated.Text) + " AND ";
            if (Comon.cDbl(txtBankID.Text) > 0)
                filter = filter + "  Sales_PurchaseInvoiceReturnMaster.CreditAccount=" + Comon.cDbl(txtBankID.Text) + " AND ";
            if (Comon.cDbl(txtBoxID.Text) > 0)
                filter = filter + "  Sales_PurchaseInvoiceReturnMaster.CreditAccount=" + Comon.cDbl(txtBoxID.Text) + " AND ";
            if (Comon.cInt(cmbStatus.EditValue) == 1)
                filter = filter + " dbo.Sales_PurchaseInvoiceReturnMaster.Cancel =1 AND ";
            if (Comon.cInt(cmbHaveAddtional.EditValue) == 1)
                filter = filter + " dbo.Sales_PurchaseInvoiceReturnMaster.AdditionaAmountTotal>0 AND ";
            else if (Comon.cInt(cmbHaveAddtional.EditValue) == 2)
                filter = filter + " dbo.Sales_PurchaseInvoiceReturnMaster.AdditionaAmountTotal<=0 AND ";
            if (txtReferanceID.Text.Trim() != string.Empty)
                filter = filter + "  Sales_PurchaseInvoiceReturnMaster.DocumentID=" + txtReferanceID.Text + " AND ";
             if (Comon.cInt(cmbCurency.EditValue) > 0)
                 filter = filter + " dbo.Sales_PurchaseInvoiceReturnMaster.CurencyID=" + Comon.cInt(cmbCurency.EditValue) + " AND ";
            if (txtFromTransactionID.Text != string.Empty)
                filter = filter + " dbo.Sales_PurchaseInvoiceReturnMaster.InvoiceID >=" + txtFromTransactionID.Text + " AND ";

            if (txtToTransactionsID.Text != string.Empty)
                filter = filter + " dbo.Sales_PurchaseInvoiceReturnMaster.InvoiceID <=" + txtToTransactionsID.Text + " AND ";

            // حسب التاريخ
            if (FromDate != 0)
                filter = filter + " dbo.Sales_PurchaseInvoiceReturnMaster.InvoiceDate >=" + FromDate + " AND ";

            if (ToDate != 0)
                filter = filter + " dbo.Sales_PurchaseInvoiceReturnMaster.InvoiceDate <=" + ToDate + " AND ";

            // '''البائع''''العميل''''التكلفة''''المستودع
            if (txtStoreID.Text != string.Empty)
                filter = filter + " dbo.Sales_PurchaseInvoiceReturnMaster.StoreID  =" + Comon.cLong(txtStoreID.Text) + "  AND ";

            if (txtCostCenterID.Text != string.Empty)
                filter = filter + " dbo.Sales_PurchaseInvoiceReturnMaster.CostCenterID  =" + Comon.cInt(txtCostCenterID.Text) + "  AND ";

            if (txtCustomerOrSupplierID.Text != string.Empty)
                filter = filter + " dbo.Sales_PurchaseInvoiceReturnMaster.SupplierID  =" + Comon.cLong(txtCustomerOrSupplierID.Text) + "  AND ";
            if (cmbPaymentMethod.Text != string.Empty && Comon.cInt(cmbPaymentMethod.EditValue) > 0)
                filter = filter + " Sales_PurchaseInvoiceReturnMaster.MethodeID =" + cmbPaymentMethod.EditValue + " AND ";
            // '''''''''''''
            filter = filter.Remove(filter.Length - 4, 4);

            strSQL = "SELECT '2' as TypeOpration, 0 TypeInvoice,dbo.Acc_Currency." + PrimaryName + " as CurrncyName,dbo.Users." + PrimaryName + " as UserName,dbo.Users." + PrimaryName + " as UserNameUpdate,Acc_Accounts." + PrimaryName + " as BoxAndBank, Sales_PurchaseInvoiceReturnMaster.DocumentID, dbo.Sales_PurchaseInvoiceReturnMaster.Cancel,   Sales_PurchaseInvoiceReturnMaster.NetProcessID,dbo.Sales_PurchaseInvoiceReturnMaster.MethodeID, Sales_PurchaseInvoiceReturnMaster.NetAmount,  dbo.Sales_PurchaseInvoiceReturnMaster.AdditionaAmountTotal As SumVat, dbo.Sales_PurchaseInvoiceReturnMaster.InvoiceID, dbo.Sales_PurchaseInvoiceReturnMaster.BranchID, dbo.Sales_PurchaseInvoiceReturnMaster.DiscountOnTotal,"
            + " dbo.Sales_PurchaseInvoiceReturnMaster.InvoiceDate, Sales_PurchaseInvoiceReturnMaster.NetBalance AS total, "
            + "  dbo.Stc_Stores." + PrimaryName + " AS StoreName, dbo.Sales_PurchaseInvoiceReturnMaster.Notes, "
            + " dbo.Sales_PurchasesDelegate." + PrimaryName + " AS DelegateName,  dbo.Sales_Suppliers." + PrimaryName + " AS SupplierName, dbo.Sales_PurchaseMethods.ArbName AS MethodeName,"
            + " dbo.Acc_CostCenters.ArbName AS CostCenter FROM dbo.Sales_PurchaseInvoiceReturnMaster INNER JOIN dbo.Sales_PurchaseInvoiceReturnDetails ON dbo.Sales_PurchaseInvoiceReturnMaster.InvoiceID"
            + " = dbo.Sales_PurchaseInvoiceReturnDetails.InvoiceID AND dbo.Sales_PurchaseInvoiceReturnMaster.BranchID = dbo.Sales_PurchaseInvoiceReturnDetails.BranchID LEFT OUTER JOIN"
            + " dbo.Acc_CostCenters ON dbo.Sales_PurchaseInvoiceReturnMaster.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Sales_PurchaseInvoiceReturnMaster.CostCenterID = "
            + " dbo.Acc_CostCenters.CostCenterID LEFT OUTER JOIN dbo.Sales_Suppliers ON dbo.Sales_PurchaseInvoiceReturnMaster.BranchID = dbo.Sales_Suppliers.BranchID AND "
            + " dbo.Sales_PurchaseInvoiceReturnMaster.SupplierID = dbo.Sales_Suppliers.AccountID LEFT OUTER JOIN dbo.Sales_PurchasesDelegate ON dbo.Sales_PurchaseInvoiceReturnMaster.BranchID"
            + " = dbo.Sales_PurchasesDelegate.BranchID AND dbo.Sales_PurchaseInvoiceReturnMaster.DelegateID = dbo.Sales_PurchasesDelegate.DelegateID LEFT OUTER JOIN"
            + " dbo.Stc_Stores ON dbo.Sales_PurchaseInvoiceReturnMaster.BranchID = dbo.Stc_Stores.BranchID AND dbo.Sales_PurchaseInvoiceReturnMaster.StoreID = dbo.Stc_Stores.AccountID LEFT OUTER JOIN"
            + " dbo.Sales_PurchaseMethods ON dbo.Sales_PurchaseInvoiceReturnMaster.MethodeID = dbo.Sales_PurchaseMethods.MethodID "
            + "  LEFT OUTER JOIN Users on Sales_PurchaseInvoiceReturnMaster.UserID=Users.UserID and Sales_PurchaseInvoiceReturnMaster.BranchID=Users.BranchID or ( Sales_PurchaseInvoiceReturnMaster.EditUserID=Users.UserID and  Sales_PurchaseInvoiceReturnMaster.BranchID=Users.BranchID) "
            + "  LEFT OUTER JOIN Acc_Accounts on Sales_PurchaseInvoiceReturnMaster.CreditAccount=Acc_Accounts.AccountID and Sales_PurchaseInvoiceReturnMaster.BranchID=Acc_Accounts.BranchID"
            + "  LEFT OUTER JOIN Acc_Currency on Sales_PurchaseInvoiceReturnMaster.CurencyID=Acc_Currency.ID and Sales_PurchaseInvoiceReturnMaster.BranchID=Acc_Currency.BranchID where " + filter + " GROUP BY "
            + "   Sales_PurchaseInvoiceReturnMaster.GoldUsing ,Sales_PurchaseInvoiceReturnMaster.NetBalance , Sales_PurchaseInvoiceReturnMaster.NetProcessID,dbo.Sales_PurchaseInvoiceReturnMaster.MethodeID, Sales_PurchaseInvoiceReturnMaster.NetAmount,  dbo.Sales_PurchaseInvoiceReturnMaster.InvoiceID,dbo.Sales_PurchaseInvoiceReturnMaster.AdditionaAmountTotal ,"
            + " dbo.Sales_PurchaseInvoiceReturnMaster.Cancel, dbo.Sales_PurchaseInvoiceReturnMaster.BranchID,dbo.Sales_PurchaseInvoiceReturnMaster.DiscountOnTotal, dbo.Sales_PurchaseInvoiceReturnMaster.InvoiceDate,   "
            + " dbo.Stc_Stores.ArbName, dbo.Sales_PurchaseInvoiceReturnMaster.Notes, dbo.Sales_PurchasesDelegate.ArbName, dbo.Sales_Suppliers.ArbName,Sales_PurchaseInvoiceReturnMaster.DocumentID, "
            + " dbo.Sales_PurchaseMethods." + PrimaryName + ",dbo.Acc_Currency." + PrimaryName + ", dbo.Acc_CostCenters." + PrimaryName + ",dbo.Users." + PrimaryName + ",Acc_Accounts." + PrimaryName;
            Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

            return strSQL;


        }
        string GetStrSQLSalse()
        {

   
            Application.DoEvents();

            string filter = "";
            strSQL = "";

            if (Comon.cInt(cmbBranchesID.EditValue) > 0)
                filter = "(.Sales_SalesInvoiceMaster.BranchID = " + cmbBranchesID.EditValue + ") AND dbo.Sales_SalesInvoiceMaster.InvoiceID >0     AND";
            else
               filter= "  dbo.Sales_SalesInvoiceMaster.InvoiceID >0    AND";
            long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
            long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));

            DataTable dt;
            // Dim dtMethodeName As DataTable
            // حسب الرقم
            if (Comon.cInt(txtUserIDEntry.Text) > 0)
                filter = filter + " Sales_SalesInvoiceMaster.UserID=" + Comon.cInt(txtUserIDEntry.Text) + " AND ";
            if (Comon.cInt(txtUserIDUpdated.Text) > 0)
                filter = filter + " Sales_SalesInvoiceMaster.EditUserID=" + Comon.cInt(txtUserIDUpdated.Text) + " AND ";
            if (Comon.cDbl(txtBankID.Text) > 0)
                filter = filter + "  Sales_SalesInvoiceMaster.CreditAccount=" + Comon.cDbl(txtBankID.Text) + " AND ";
            if (Comon.cDbl(txtBoxID.Text) > 0)
                filter = filter + "  Sales_SalesInvoiceMaster.CreditAccount=" + Comon.cDbl(txtBoxID.Text) + " AND ";
            if (Comon.cInt(cmbStatus.EditValue) == 1)
                filter = filter + " dbo.Sales_SalesInvoiceMaster.Cancel =1 AND ";
             if (Comon.cInt(cmbCurency.EditValue) > 0)
                 filter = filter + " dbo.Sales_SalesInvoiceMaster.CurencyID=" + Comon.cInt(cmbCurency.EditValue) + " AND ";
            if (Comon.cInt(cmbHaveAddtional.EditValue) == 1)
                filter = filter + " dbo.Sales_SalesInvoiceMaster.AdditionaAmountTotal>0 AND ";
            else if (Comon.cInt(cmbHaveAddtional.EditValue) == 2)
                filter = filter + " dbo.Sales_SalesInvoiceMaster.AdditionaAmountTotal<=0 AND ";

            if (txtReferanceID.Text.Trim() != string.Empty)
                filter = filter + "  Sales_SalesInvoiceMaster.DocumentID=" + txtReferanceID.Text + " AND ";
            if (txtFromTransactionID.Text != string.Empty)
                filter = filter + " dbo.Sales_SalesInvoiceMaster.InvoiceID >=" + txtFromTransactionID.Text + " AND ";

            if (txtToTransactionsID.Text != string.Empty)
                filter = filter + " dbo.Sales_SalesInvoiceMaster.InvoiceID <=" + txtToTransactionsID.Text + " AND ";

            // حسب التاريخ
            if (FromDate != 0)
                filter = filter + " dbo.Sales_SalesInvoiceMaster.InvoiceDate >=" + FromDate + " AND ";

            if (ToDate != 0)
                filter = filter + " dbo.Sales_SalesInvoiceMaster.InvoiceDate <=" + ToDate + " AND ";

            // '''البائع''''العميل''''التكلفة''''المستودع
            if (txtStoreID.Text != string.Empty)
                filter = filter + " dbo.Sales_SalesInvoiceMaster.StoreID  =" + Comon.cLong(txtStoreID.Text) + "  AND ";

            if (txtCostCenterID.Text != string.Empty)
                filter = filter + " dbo.Sales_SalesInvoiceMaster.CostCenterID  =" + Comon.cInt(txtCostCenterID.Text) + "  AND ";

            if (txtCustomerOrSupplierID.Text != string.Empty)
                filter = filter + " dbo.Sales_SalesInvoiceMaster.CustomerID  =" + Comon.cLong(txtCustomerOrSupplierID.Text) + "  AND ";
            if (cmbPaymentMethod.Text != string.Empty && Comon.cInt(cmbPaymentMethod.EditValue) > 0)
                filter = filter + " Sales_SalesInvoiceMaster.MethodeID =" + cmbPaymentMethod.EditValue + " AND ";
            // '''''''''''''
            filter = filter.Remove(filter.Length - 4, 4);

            strSQL = "SELECT '3' as TypeOpration, 0 TypeInvoice,dbo.Acc_Currency." + PrimaryName + " as CurrncyName,dbo.Users." + PrimaryName + " as UserName,dbo.Users." + PrimaryName + " as UserNameUpdate,Acc_Accounts." + PrimaryName + " as BoxAndBank, Sales_SalesInvoiceMaster.DocumentID, dbo.Sales_SalesInvoiceMaster.Cancel,   Sales_SalesInvoiceMaster.NetProcessID,dbo.Sales_SalesInvoiceMaster.MethodeID, Sales_SalesInvoiceMaster.NetAmount,  dbo.Sales_SalesInvoiceMaster.AdditionaAmountTotal As SumVat, dbo.Sales_SalesInvoiceMaster.InvoiceID, dbo.Sales_SalesInvoiceMaster.BranchID, dbo.Sales_SalesInvoiceMaster.DiscountOnTotal,"
            + " dbo.Sales_SalesInvoiceMaster.InvoiceDate, Sales_SalesInvoiceMaster.NetBalance AS total, "
            + "  dbo.Stc_Stores." + PrimaryName + " AS StoreName, dbo.Sales_SalesInvoiceMaster.Notes, "
            + " dbo.Sales_SalesDelegate." + PrimaryName + " AS DelegateName,  dbo.Sales_Customers." + PrimaryName + " AS SupplierName, dbo.Sales_PurchaseMethods." + PrimaryName + "  AS MethodeName,"
            + " dbo.Acc_CostCenters." + PrimaryName + "  AS CostCenter FROM dbo.Sales_SalesInvoiceMaster INNER JOIN dbo.Sales_SalesInvoiceDetails ON dbo.Sales_SalesInvoiceMaster.InvoiceID"
            + " = dbo.Sales_SalesInvoiceDetails.InvoiceID AND dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Sales_SalesInvoiceDetails.BranchID LEFT OUTER JOIN"
            + " dbo.Acc_CostCenters ON dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Sales_SalesInvoiceMaster.CostCenterID = "
            + " dbo.Acc_CostCenters.CostCenterID LEFT OUTER JOIN dbo.Sales_Customers ON dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Sales_Customers.BranchID AND "
            + " dbo.Sales_SalesInvoiceMaster.CustomerID = dbo.Sales_Customers.AccountID LEFT OUTER JOIN dbo.Sales_SalesDelegate ON dbo.Sales_SalesInvoiceMaster.BranchID "
            + " = dbo.Sales_SalesDelegate.BranchID AND dbo.Sales_SalesInvoiceMaster.DelegateID = dbo.Sales_SalesDelegate.DelegateID and dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Sales_SalesDelegate.BranchID LEFT OUTER JOIN"
            + " dbo.Stc_Stores ON dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Stc_Stores.BranchID AND dbo.Sales_SalesInvoiceMaster.StoreID = dbo.Stc_Stores.AccountID LEFT OUTER JOIN"
            + " dbo.Sales_PurchaseMethods ON dbo.Sales_SalesInvoiceMaster.MethodeID = dbo.Sales_PurchaseMethods.MethodID "
            + "  LEFT OUTER JOIN Users on Sales_SalesInvoiceMaster.UserID=Users.UserID and Sales_SalesInvoiceMaster.BranchID=Users.BranchID or (Sales_SalesInvoiceMaster.EditUserID=Users.UserID and Sales_SalesInvoiceMaster.BranchID=Users.BranchID) "
            + "  LEFT OUTER JOIN Acc_Accounts on Sales_SalesInvoiceMaster.CreditAccount=Acc_Accounts.AccountID and Sales_SalesInvoiceMaster.BranchID=Acc_Accounts.BranchID "
            + " LEFT OUTER JOIN Acc_Currency on Sales_SalesInvoiceMaster.CurencyID=Acc_Currency.ID and Sales_SalesInvoiceMaster.BranchID=Acc_Currency.BranchID where " + filter + " GROUP BY "
            + "    Sales_SalesInvoiceMaster.NetBalance , Sales_SalesInvoiceMaster.NetProcessID,dbo.Sales_SalesInvoiceMaster.MethodeID, Sales_SalesInvoiceMaster.NetAmount,  dbo.Sales_SalesInvoiceMaster.InvoiceID,dbo.Sales_SalesInvoiceMaster.AdditionaAmountTotal ,"
            + " dbo.Sales_SalesInvoiceMaster.Cancel, dbo.Sales_SalesInvoiceMaster.BranchID,dbo.Sales_SalesInvoiceMaster.DiscountOnTotal, dbo.Sales_SalesInvoiceMaster.InvoiceDate,   "
            + " dbo.Stc_Stores." + PrimaryName + " , dbo.Sales_SalesInvoiceMaster.Notes, dbo.Sales_SalesDelegate." + PrimaryName + " , dbo.Sales_Customers." + PrimaryName + " ,Sales_SalesInvoiceMaster.DocumentID, "
            + " dbo.Sales_PurchaseMethods." + PrimaryName + ",dbo.Acc_Currency." + PrimaryName + ", dbo.Acc_CostCenters." + PrimaryName + ",dbo.Users." + PrimaryName + ",Acc_Accounts." + PrimaryName;
            Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

            return strSQL;


        }
        string GetStrSQLSalseReturn()
        {

        
            Application.DoEvents();

            string filter = " ";
            strSQL = "";

            if (Comon.cInt(cmbBranchesID.EditValue) > 0)
                filter = "(.Sales_SalesInvoiceReturnMaster.BranchID = " + cmbBranchesID.EditValue + ") AND dbo.Sales_SalesInvoiceReturnMaster.InvoiceID >0     AND";
            else
                filter = "  dbo.Sales_SalesInvoiceReturnMaster.InvoiceID >0    AND ";
            long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
            long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));

            DataTable dt;
          
            // حسب الرقم
            if (Comon.cInt(txtUserIDEntry.Text) > 0)
                filter = filter + " Sales_SalesInvoiceReturnMaster.UserID=" + Comon.cInt(txtUserIDEntry.Text) + " AND ";
            if (Comon.cInt(txtUserIDUpdated.Text) > 0)
                filter = filter + " Sales_SalesInvoiceReturnMaster.EditUserID=" + Comon.cInt(txtUserIDUpdated.Text) + " AND ";
            if (Comon.cDbl(txtBankID.Text) > 0)
                filter = filter + "  Sales_SalesInvoiceReturnMaster.CreditAccount=" + Comon.cDbl(txtBankID.Text) + " AND ";
            if (Comon.cDbl(txtBoxID.Text) > 0)
                filter = filter + "  Sales_SalesInvoiceReturnMaster.CreditAccount=" + Comon.cDbl(txtBoxID.Text) + " AND ";
            if (Comon.cInt(cmbStatus.EditValue) == 1)
                filter = filter + " dbo.Sales_SalesInvoiceReturnMaster.Cancel =1 AND ";
            if (Comon.cInt(cmbHaveAddtional.EditValue) == 1)
                filter = filter + " dbo.Sales_SalesInvoiceReturnMaster.AdditionaAmountTotal>0 AND ";
            else if (Comon.cInt(cmbHaveAddtional.EditValue) == 2)
                filter = filter + " dbo.Sales_SalesInvoiceReturnMaster.AdditionaAmountTotal<=0 AND ";
            if (txtFromTransactionID.Text != string.Empty)
                filter = filter + " dbo.Sales_SalesInvoiceReturnMaster.InvoiceID >=" + txtFromTransactionID.Text + " AND ";
            else if (Comon.cInt(cmbCurency.EditValue) > 0)
                filter = filter + " dbo.Sales_SalesInvoiceReturnMaster.CurencyID=" + Comon.cInt(cmbCurency.EditValue) + " AND ";
            if (txtToTransactionsID.Text != string.Empty)
                filter = filter + " dbo.Sales_SalesInvoiceReturnMaster.InvoiceID <=" + txtToTransactionsID.Text + " AND ";
            if(txtReferanceID.Text.Trim()!=string.Empty)
                filter = filter + "  Sales_SalesInvoiceReturnMaster.DocumentID=" + txtReferanceID.Text + " AND ";
            // حسب التاريخ
            if (FromDate != 0)
                filter = filter + " dbo.Sales_SalesInvoiceReturnMaster.InvoiceDate >=" + FromDate + " AND ";

            if (ToDate != 0)
                filter = filter + " dbo.Sales_SalesInvoiceReturnMaster.InvoiceDate <=" + ToDate + " AND ";

            // '''البائع''''العميل''''التكلفة''''المستودع
            if (txtStoreID.Text != string.Empty)
                filter = filter + " dbo.Sales_SalesInvoiceReturnMaster.StoreID  =" + Comon.cLong(txtStoreID.Text) + "  AND ";

            if (txtCostCenterID.Text != string.Empty)
                filter = filter + " dbo.Sales_SalesInvoiceReturnMaster.CostCenterID  =" + Comon.cInt(txtCostCenterID.Text) + "  AND ";

            if (txtCustomerOrSupplierID.Text != string.Empty)
                filter = filter + " dbo.Sales_SalesInvoiceReturnMaster.CustomerID  =" + Comon.cLong(txtCustomerOrSupplierID.Text) + "  AND ";
            if (cmbPaymentMethod.Text != string.Empty && Comon.cInt(cmbPaymentMethod.EditValue) > 0)
                filter = filter + " Sales_SalesInvoiceReturnMaster.MethodeID =" + cmbPaymentMethod.EditValue + " AND ";
            // '''''''''''''
            filter = filter.Remove(filter.Length - 4, 4);

            strSQL = "SELECT '4' as TypeOpration, 0 TypeInvoice,dbo.Acc_Currency." + PrimaryName + " as CurrncyName,dbo.Users." + PrimaryName + " as UserName,dbo.Users." + PrimaryName + " as UserNameUpdate,Acc_Accounts." + PrimaryName + " as BoxAndBank, Sales_SalesInvoiceReturnMaster.DocumentID, dbo.Sales_SalesInvoiceReturnMaster.Cancel,   Sales_SalesInvoiceReturnMaster.NetProcessID,dbo.Sales_SalesInvoiceReturnMaster.MethodeID, Sales_SalesInvoiceReturnMaster.NetAmount,  dbo.Sales_SalesInvoiceReturnMaster.AdditionaAmountTotal As SumVat, dbo.Sales_SalesInvoiceReturnMaster.InvoiceID, dbo.Sales_SalesInvoiceReturnMaster.BranchID, dbo.Sales_SalesInvoiceReturnMaster.DiscountOnTotal,"
            + " dbo.Sales_SalesInvoiceReturnMaster.InvoiceDate, Sales_SalesInvoiceReturnMaster.NetBalance AS total, "
            + "  dbo.Stc_Stores." + PrimaryName + " AS StoreName, dbo.Sales_SalesInvoiceReturnMaster.Notes, "
            + " dbo.Sales_SalesDelegate." + PrimaryName + " AS DelegateName,  dbo.Sales_Customers." + PrimaryName + " AS SupplierName, dbo.Sales_PurchaseMethods." + PrimaryName + "  AS MethodeName,"
            + " dbo.Acc_CostCenters." + PrimaryName + "  AS CostCenter FROM dbo.Sales_SalesInvoiceReturnMaster INNER JOIN dbo.Sales_SalesInvoiceReturnDetails ON dbo.Sales_SalesInvoiceReturnMaster.InvoiceID"
            + " = dbo.Sales_SalesInvoiceReturnDetails.InvoiceID AND dbo.Sales_SalesInvoiceReturnMaster.BranchID = dbo.Sales_SalesInvoiceReturnDetails.BranchID LEFT OUTER JOIN"
            + " dbo.Acc_CostCenters ON dbo.Sales_SalesInvoiceReturnMaster.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Sales_SalesInvoiceReturnMaster.CostCenterID = "
            + " dbo.Acc_CostCenters.CostCenterID LEFT OUTER JOIN dbo.Sales_Customers ON dbo.Sales_SalesInvoiceReturnMaster.BranchID = dbo.Sales_Customers.BranchID AND "
            + " dbo.Sales_SalesInvoiceReturnMaster.CustomerID = dbo.Sales_Customers.AccountID LEFT OUTER JOIN dbo.Sales_SalesDelegate ON dbo.Sales_SalesInvoiceReturnMaster.BranchID"
            + " = dbo.Sales_SalesDelegate.BranchID AND dbo.Sales_SalesInvoiceReturnMaster.DelegateID = dbo.Sales_SalesDelegate.DelegateID LEFT OUTER JOIN"
            + " dbo.Stc_Stores ON dbo.Sales_SalesInvoiceReturnMaster.BranchID = dbo.Stc_Stores.BranchID AND dbo.Sales_SalesInvoiceReturnMaster.StoreID = dbo.Stc_Stores.AccountID LEFT OUTER JOIN"
            + " dbo.Sales_PurchaseMethods ON dbo.Sales_SalesInvoiceReturnMaster.MethodeID = dbo.Sales_PurchaseMethods.MethodID "
            + "  LEFT OUTER JOIN Users on Sales_SalesInvoiceReturnMaster.UserID=Users.UserID and  Sales_SalesInvoiceReturnMaster.BranchID=Users.BranchID or (Sales_SalesInvoiceReturnMaster.EditUserID=Users.UserID and  Sales_SalesInvoiceReturnMaster.BranchID=Users.BranchID) "
            + "  LEFT OUTER JOIN Acc_Accounts on Sales_SalesInvoiceReturnMaster.CreditAccount=Acc_Accounts.AccountID and Sales_SalesInvoiceReturnMaster.BranchID=Acc_Accounts.BranchID"
            + " LEFT OUTER JOIN Acc_Currency on Sales_SalesInvoiceReturnMaster.CurencyID=Acc_Currency.ID and Sales_SalesInvoiceReturnMaster.BranchID=Acc_Currency.BranchID where " + filter + " GROUP BY "
            + "   Sales_SalesInvoiceReturnMaster.NetBalance , Sales_SalesInvoiceReturnMaster.NetProcessID,dbo.Sales_SalesInvoiceReturnMaster.MethodeID, Sales_SalesInvoiceReturnMaster.NetAmount,  dbo.Sales_SalesInvoiceReturnMaster.InvoiceID,dbo.Sales_SalesInvoiceReturnMaster.AdditionaAmountTotal ,"
            + " dbo.Sales_SalesInvoiceReturnMaster.Cancel, dbo.Sales_SalesInvoiceReturnMaster.BranchID,dbo.Sales_SalesInvoiceReturnMaster.DiscountOnTotal, dbo.Sales_SalesInvoiceReturnMaster.InvoiceDate,   "
            + " dbo.Stc_Stores." + PrimaryName + " , dbo.Sales_SalesInvoiceReturnMaster.Notes, dbo.Sales_SalesDelegate." + PrimaryName + " , dbo.Sales_Customers." + PrimaryName + " ,Sales_SalesInvoiceReturnMaster.DocumentID, "
            + " dbo.Sales_PurchaseMethods." + PrimaryName + ",dbo.Acc_Currency." + PrimaryName + ", dbo.Acc_CostCenters." + PrimaryName + ",dbo.Users." + PrimaryName + ",Acc_Accounts." + PrimaryName;
            Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

            return strSQL;


        }
        private void PurchaseInvoice()
        {
            try
            {
                decimal netSum = 0;
                decimal netCashSum = 0;
                decimal caschPaidWithNet = 0;
                decimal cash = 0;
                decimal future = 0;
                decimal check1 = 0;
                decimal total = 0;
                DataRow row;
                dt.Clear();
                _sampleData.Clear();
                 
                int fromType = Comon.cInt(cmbFromType.EditValue);
                int toType = Comon.cInt(cmbToType.EditValue);

                if (fromType == 1 || (toType >= 1 && fromType <= 1)||(toType==fromType && fromType<=0))
                {
                    dt = Lip.SelectRecord(GetStrSQL());
                }

                if ( (toType==fromType && fromType<=0)||
                     ((fromType >= 1 && fromType <=2 && toType >= 2 && (toType != fromType && toType >= 2)) ||
                     (fromType >= 1 && fromType <= 2 && toType <= 0) ||  
                     (toType >=2  && fromType <= 0)) || 
                     (toType == fromType && toType == 2))
                {
                    dt.Merge(Lip.SelectRecord(GetStrSQLPurchaseReturn()));
                }

                 
                if ((toType==fromType && fromType<=0)||
                    ((fromType >= 1 && toType >= 3 && (toType != fromType && toType >= 3)) ||
                    (fromType >= 1 && fromType <= 3 && toType <= 0) || (toType >=3  && fromType <= 0)) || 
                    (toType == fromType && toType == 3))
                {
                    dt.Merge(Lip.SelectRecord(GetStrSQLSalse()));
                }
               
                if ((toType==fromType && fromType<=0)||
                    ((fromType >= 1 && toType >= 4 && (toType != fromType && toType >= 4))  ||
                    (fromType >= 1 && fromType <= 4 && toType <= 0) || (toType >=4  && fromType <= 0)) || 
                    (toType == fromType && toType == 4))
                {
                    dt.Merge(Lip.SelectRecord(GetStrSQLSalseReturn()));
                }
               
                

                if (strSQL != null || strSQL != "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["Sn"] = _sampleData.Rows.Count + 1;
                            row["ID"] = dt.Rows[i]["InvoiceID"].ToString();
                            row["RefranceID"] = dt.Rows[i]["DocumentID"].ToString();
                            //row["RefranceID"] = dt.Rows[i]["RefranceID"].ToString();
                            row["InvoiceDate"] = Comon.ConvertSerialDateTo(dt.Rows[i]["InvoiceDate"].ToString());
                            row["NetAmmount"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["Total"]).ToString("N" + 3);
                            row["CurrncyName"] = (dt.Rows[i]["CurrncyName"].ToString() != string.Empty ? dt.Rows[i]["CurrncyName"] : "");
                            if (Comon.cInt(dt.Rows[i]["Cancel"]) == 1)
                                row["StatUs"] =UserInfo.Language==iLanguage.Arabic? "محذوف":"Deleted";
                            else
                                row["StatUs"] =UserInfo.Language==iLanguage.Arabic? "مرحل ":"Aported";
                            row["TypeOpration"] = dt.Rows[i]["TypeOpration"].ToString();
                            if (Comon.cInt(dt.Rows[i]["TypeOpration"]) == 1)
                            {
                                if (Comon.cInt(dt.Rows[i]["TypeInvoice"]) == 1)
                                   row["RecordType"] = UserInfo.Language == iLanguage.Arabic ? "مشتريات" : "Purchase";
                                else if (Comon.cInt(dt.Rows[i]["TypeInvoice"]) == 2)
                                {
                                    row["RecordType"] = UserInfo.Language == iLanguage.Arabic ? "مقابل خدمات" : "Purchases for services";
                                    row["TypeOpration"] = 5;
                                }
                            }
                            else if (Comon.cInt(dt.Rows[i]["TypeOpration"]) == 2)
                                row["RecordType"] = UserInfo.Language == iLanguage.Arabic ? "مردود مشتريات" : "Purchase Return";
                            else if (Comon.cInt(dt.Rows[i]["TypeOpration"]) == 3)
                                row["RecordType"] = UserInfo.Language == iLanguage.Arabic ? "مبيعات" : "Salse";
                            else if (Comon.cInt(dt.Rows[i]["TypeOpration"]) == 4)
                                row["RecordType"] = UserInfo.Language == iLanguage.Arabic ? "مردود مبيعات" : "Salse Return";
                          
                            row["BoxAndBank"] = (dt.Rows[i]["BoxAndBank"].ToString() != string.Empty ? dt.Rows[i]["BoxAndBank"] : "");
                            row["UserNameUpdate"] = (dt.Rows[i]["UserNameUpdate"].ToString() != string.Empty ? dt.Rows[i]["UserNameUpdate"] : "");
                            row["AdditionaAmountTotal"] = dt.Rows[i]["SumVat"];
                            row["TotalBeforeAddtional"] =Comon.cDec( Comon.cDec(  row["NetAmmount"])-Comon.cDec( row["AdditionaAmountTotal"]));
                            row["PaymentMethod"] = dt.Rows[i]["MethodeName"];
                            row["SupplierOrCustomer"] = (dt.Rows[i]["SupplierName"].ToString() != string.Empty ? dt.Rows[i]["SupplierName"] : "");
                            row["StoreName"] = dt.Rows[i]["StoreName"];
                            row["CostCenterName"] = (dt.Rows[i]["CostCenter"].ToString() != string.Empty ? dt.Rows[i]["CostCenter"] : "");
                            row["Notes"] = (dt.Rows[i]["Notes"].ToString() != string.Empty ? dt.Rows[i]["Notes"] : "");
                            row["UserName"] = dt.Rows[i]["UserName"];                            
                            _sampleData.Rows.Add(row);
                        }
                    }                 
                }
            }
            catch { }
        }
        private void btnShow_Click(object sender, EventArgs e)
        {
            PurchaseInvoice();
            gridControl1.DataSource = _sampleData;
            if (gridView1.RowCount > 0)
            {
                //btnShow.Visible = true;

                //txtStoreID.Enabled = false;
                //txtCostCenterID.Enabled = false;
                //txtCustomerOrSupplierID.Enabled = false;
                //cmbPaymentMethod.Enabled = false;
                //txtFromDate.Enabled = false;
                //txtToDate.Enabled = false;
                //txtToTransactionsID.Enabled = false;
                //txtFromTransactionID.Enabled = false;

            }
            else
            {

                Messages.MsgInfo(Messages.TitleInfo, MySession.GlobalLanguageName == iLanguage.Arabic ? "لايوجد بيانات لعرضها" : "There is no Data to show it");
                //btnShow.Visible = true;
                //  DoNew();
            }
        }
    }
}
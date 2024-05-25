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
using DevExpress.XtraGrid.Views.Grid;
using Edex.StockObjects.Transactions;
namespace Edex.Archives
{
    
    public partial class frmArchivesStores : BaseForm
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
        public frmArchivesStores()
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
            if (UserInfo.Language == iLanguage.English)
            {
                PrimaryName = "EngName";
            }
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = true;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Caption = (UserInfo.Language == iLanguage.Arabic ? "استعلام جديد" : "New Query");
            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
            FillCombo.FillComboBoxLookUpEdit(cmbFromType, "Arc_ArchivesType", "ID", PrimaryName, "", "ID>4 and ID<=10", (UserInfo.Language == iLanguage.English ? "Select Type" : "حدد النوع "));
            FillCombo.FillComboBoxLookUpEdit(cmbToType, "Arc_ArchivesType", "ID", PrimaryName, "", "ID>4 and ID<=10", (UserInfo.Language == iLanguage.English ? "Select Type" : "حدد النوع "));
            cmbBranchesID.EditValue = MySession.GlobalBranchID;
            cmbBranchesID.ReadOnly = !MySession.GlobalAllowBranchModificationAllScreens;
            FillCombo.FillComboBoxLookUpEdit(cmbStatus, "Manu_TypeStatus", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select StatUs" : "حدد الحالة "));
            FillCombo.FillComboBox(cmbCurency, "Acc_Currency", "ID", PrimaryName, "", "BranchID=" +MySession.GlobalBranchID, (UserInfo.Language == iLanguage.English ? "Select " : "حدد العملة"));         
            this.KeyDown += frmArchivesStores_KeyDown;
            this.txtStoreID.Validating += txtStoreID_Validating;
            this.txtCustomerOrSupplierID.Validating += txtCustomerOrSupplierID_Validating;
            this.txtCostCenterID.Validating += txtCostCenterID_Validating;
            this.txtDelegeteID.Validating += txtDelegeteID_Validating;
            this.txtBankID.Validating += txtBankID_Validating;
            this.txtBoxID.Validating += txtBoxID_Validating;
            this.Load += frmArchivesStores_Load;

            this.txtUserIDEntry.Validating += txtUserIDEntry_Validating;
            this.txtUserIDUpdated.Validating += txtUserIDUpdated_Validating;

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
                    case "5":
                        frmGoldInOnBail frm = new frmGoldInOnBail();
                        if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm);
                            frm.Show();
                            frm.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                        }
                        else
                            frm.Dispose();
                        break;
                    case "6":
                        frmGoldOutOnBail frmGoldOut = new frmGoldOutOnBail();
                        if (Permissions.UserPermissionsFrom(frmGoldOut, frmGoldOut.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frmGoldOut);
                            frmGoldOut.Show();
                            frmGoldOut.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                        }
                        else
                            frmGoldOut.Dispose();
                        break;
                    case "7":
                        frmMatirialInOnBail frmMatirialIn = new frmMatirialInOnBail();
                        if (Permissions.UserPermissionsFrom(frmMatirialIn, frmMatirialIn.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frmMatirialIn);
                            frmMatirialIn.Show();
                            frmMatirialIn.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                        }
                        else
                            frmMatirialIn.Dispose();
                        break;
                    case "8":
                        frmMatirialOutOnBail frmMatirialOut = new frmMatirialOutOnBail();
                        if (Permissions.UserPermissionsFrom(frmMatirialOut, frmMatirialOut.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frmMatirialOut);
                            frmMatirialOut.Show();
                            frmMatirialOut.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                        }
                        else
                            frmMatirialOut.Dispose();
                        break;
                    case "9":
                        frmTransferMultipleStoresGold frmMultipleGold = new frmTransferMultipleStoresGold();
                        if (Permissions.UserPermissionsFrom(frmMultipleGold, frmMultipleGold.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frmMultipleGold);
                            frmMultipleGold.Show();
                            frmMultipleGold.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                        }
                        else
                            frmMultipleGold.Dispose();
                        break;
                    case "1":
                        frmTransferMultipleStoreMatirial frmMultipleMatirial = new frmTransferMultipleStoreMatirial();
                        if (Permissions.UserPermissionsFrom(frmMultipleMatirial, frmMultipleMatirial.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frmMultipleMatirial);
                            frmMultipleMatirial.Show();
                            frmMultipleMatirial.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                        }
                        else
                            frmMultipleMatirial.Dispose();
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
                if(Comon.cInt(cmbBranchesID.EditValue)>0)
                   strSQL = "SELECT " + PrimaryName + " as UserName FROM Users WHERE UserID =" + Comon.cInt(txtUserIDEntry.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                else
                    strSQL = "SELECT " + PrimaryName + " as UserName FROM Users WHERE UserID =" + Comon.cInt(txtUserIDEntry.Text) + " And Cancel =0 ";
               
                CSearch.ControlValidating(txtUserIDEntry, lblUserNameEntry, strSQL);
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
                if(Comon.cInt(cmbBranchesID.EditValue)>0)
                    strSQL = "SELECT " + PrimaryName + " as UserName FROM Users WHERE UserID =" + Comon.cInt(txtUserIDUpdated.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                else

                    strSQL = "SELECT " + PrimaryName + " as UserName FROM Users WHERE UserID =" + Comon.cInt(txtUserIDUpdated.Text) + " And Cancel =0 ";
                CSearch.ControlValidating(txtUserIDUpdated, lblUserNameUpdated, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        void frmArchivesStores_Load(object sender, EventArgs e)
        {
            _sampleData.Columns.Add(new DataColumn("Sn", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("RecordType", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("ID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("RefranceID", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("CurrncyName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("NetAmmount", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("SupplierOrCustomer", typeof(string))); 
            _sampleData.Columns.Add(new DataColumn("StoreName", typeof(string))); 
            _sampleData.Columns.Add(new DataColumn("StatUs", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("UserName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("UserNameUpdate", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("CostCenterName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("TypeOpration", typeof(string)) { MaxLength = 100 });
            _sampleData.Columns.Add(new DataColumn("Notes", typeof(string)));
        }



        void txtBoxID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if(Comon.cInt(cmbBranchesID.EditValue)>0)                    
                     strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE  (Cancel = 0) AND (AccountID = " + txtBoxID.Text + ") and BranchID="+cmbBranchesID.EditValue;
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE  (Cancel = 0) AND (AccountID = " + txtBoxID.Text + ") ";
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
                    if(Comon.cInt(cmbBranchesID.EditValue)>0)
                         strSQL = "SELECT " + PrimaryName + " as CustomerName ,VATID,Mobile FROM Sales_CustomerAnSublierListArb Where    AcountID =" + txtCustomerOrSupplierID.Text+" and BranchID="+cmbBranchesID.EditValue;
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
                        if(Comon.cInt(cmbBranchesID.EditValue)>0)
                             strSql = "SELECT " + PrimaryName + " as CustomerName,SpecialDiscount,VATID, CustomerID,Mobile   FROM Sales_Customers Where  Cancel =0 And  AccountID =" + txtCustomerOrSupplierID.Text+" and BranchID="+cmbBranchesID.EditValue;
                        else
                             strSql = "SELECT " + PrimaryName + " as CustomerName,SpecialDiscount,VATID, CustomerID,Mobile   FROM Sales_Customers Where  Cancel =0 And  AccountID =" + txtCustomerOrSupplierID.Text;
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

        void frmArchivesStores_KeyDown(object sender, KeyEventArgs e)
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
                if(Comon.cInt(cmbBranchesID.EditValue)>0)
                     strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE  (Cancel = 0) AND (AccountID = " + txtBankID.Text + ") and BranchID="+cmbBranchesID.EditValue;
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
            if (FocusedControl.Trim() == txtCustomerOrSupplierID.Name)
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
            else if (FocusedControl.Trim() == txtUserIDEntry.Name)
            {

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtUserIDEntry, lblUserNameEntry, "UserID", "رقم المستخدم", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtUserIDEntry, lblUserNameEntry, "UserID", "User ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtUserIDUpdated.Name)
            {

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtUserIDUpdated, lblUserNameUpdated, "UserID", "رقم المستخدم", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtUserIDUpdated, lblUserNameUpdated, "UserID", "User ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtBankID.Name)
            {
                if (!MySession.GlobalAllowChangefrmPurchaseStoreID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtBankID, lblBankName, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtBankID, lblBankName, "AccountID", "Account ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtBoxID.Name)
            {
                if (!MySession.GlobalAllowChangefrmPurchaseStoreID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtBoxID, lblBoxeName, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtBoxID, lblBoxeName, "AccountID", "Account ID", MySession.GlobalBranchID);
            }

            else if (FocusedControl.Trim() == txtDelegeteID.Name)
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
                if(Comon.cInt(cmbBranchesID.EditValue)>0)
                strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE AccountID =" + Comon.cLong(txtStoreID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                else

                    strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE AccountID =" + Comon.cLong(txtStoreID.Text) + " And Cancel =0 " ;
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
                if(Comon.cInt(cmbBranchesID.EditValue)>0)
                    strSQL = "SELECT " + PrimaryName + " as CostCenterName FROM Acc_CostCenters WHERE CostCenterID =" + Comon.cInt(txtCostCenterID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                else
                    strSQL = "SELECT " + PrimaryName + " as CostCenterName FROM Acc_CostCenters WHERE CostCenterID =" + Comon.cInt(txtCostCenterID.Text) + " And Cancel =0";
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
                if (FocusedControl == txtCustomerOrSupplierID.Name)
                {
                    txtCustomerOrSupplierID.Text = cls.PrimaryKeyValue.ToString();
                    txtCustomerOrSupplierID_Validating(null, null);
                }
                if (FocusedControl == txtBoxID.Name)
                {
                    txtBoxID.Text = cls.PrimaryKeyValue.ToString();
                    txtBoxID_Validating(null, null);
                }
                if (FocusedControl == txtBankID.Name)
                {
                    txtBankID.Text = cls.PrimaryKeyValue.ToString();
                    txtBankID_Validating(null, null);
                }
                if (FocusedControl == txtUserIDEntry.Name)
                {
                    txtUserIDEntry.Text = cls.PrimaryKeyValue.ToString();
                    txtUserIDEntry_Validating(null, null);
                }
                if (FocusedControl == txtUserIDUpdated.Name)
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
                else if (FocusedControl == txtDelegeteID.Name)
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
                if(Comon.cInt(cmbBranchesID.EditValue)>0)
                    strSQL = "SELECT " + PrimaryName + " as DelegateName FROM Sales_SalesDelegate WHERE DelegateID =" + txtDelegeteID.Text + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
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

        string GetStrSQLGoldOut()
        {

          
            Application.DoEvents();
            string filter = "";
            
            strSQL = "";

            if (Comon.cInt(cmbBranchesID.EditValue) > 0)
                filter = "(dbo.Stc_GoldOutOnBail_Master.BranchID = " + cmbBranchesID.EditValue + ") AND dbo.Stc_GoldOutOnBail_Master.InvoiceID >0     AND ";
            else
                filter = "  dbo.Stc_GoldOutOnBail_Master.InvoiceID >0    AND ";
            long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
            long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));

            DataTable dt;

            // حسب الرقم
            if (Comon.cInt(cmbStatus.EditValue) == 1)
                filter = filter + " dbo.Stc_GoldOutOnBail_Master.Cancel =1 AND ";

            if (txtFromTransactionID.Text != string.Empty)
                filter = filter + " dbo.Stc_GoldOutOnBail_Master.InvoiceID >=" + txtFromTransactionID.Text + " AND ";
            
            else if (Comon.cInt(cmbCurency.EditValue) > 0)
                filter = filter + " dbo.Stc_GoldOutOnBail_Master.CurrencyID=" + Comon.cInt(cmbCurency.EditValue) + " AND ";
            if (Comon.cInt(txtUserIDEntry.Text) > 0)
                filter = filter + " Stc_GoldOutOnBail_Master.UserID=" + Comon.cInt(txtUserIDEntry.Text) + " AND ";
            if (Comon.cInt(txtUserIDUpdated.Text) > 0)
                filter = filter + " Stc_GoldOutOnBail_Master.EditUserID=" + Comon.cInt(txtUserIDUpdated.Text) + " AND ";
            if (txtToTransactionsID.Text != string.Empty)
                filter = filter + " dbo.Stc_GoldOutOnBail_Master.InvoiceID <=" + txtToTransactionsID.Text + " AND ";
            if (txtReferanceID.Text.Trim() != string.Empty)
                filter = filter + "  Stc_GoldOutOnBail_Master.DocumentID=" + txtReferanceID.Text + " AND ";

            if (Comon.cDbl(txtBankID.Text) > 0)
                filter = filter + "  Stc_GoldOutOnBail_Master.CreditAccount=" + Comon.cDbl(txtBankID.Text) + " AND ";
            if (Comon.cDbl(txtBoxID.Text) > 0)
                filter = filter + "  Stc_GoldOutOnBail_Master.CreditAccount=" + Comon.cDbl(txtBoxID.Text) + " AND ";
            // حسب التاريخ
            if (FromDate != 0)
                filter = filter + " dbo.Stc_GoldOutOnBail_Master.InvoiceDate >=" + FromDate + " AND ";

            if (ToDate != 0)
                filter = filter + " dbo.Stc_GoldOutOnBail_Master.InvoiceDate <=" + ToDate + " AND ";

            // '''البائع''''العميل''''التكلفة''''المستودع
            if (txtStoreID.Text != string.Empty)
                filter = filter + " dbo.Stc_GoldOutOnBail_Master.StoreID  =" + Comon.cLong(txtStoreID.Text) + "  AND ";

            if (txtCostCenterID.Text != string.Empty)
                filter = filter + " dbo.Stc_GoldOutOnBail_Master.CostCenterID  =" + Comon.cInt(txtCostCenterID.Text) + "  AND ";

            if (txtCustomerOrSupplierID.Text != string.Empty)
                filter = filter + " dbo.Stc_GoldOutOnBail_Master.CreditAccount  =" + Comon.cLong(txtCustomerOrSupplierID.Text) + "  AND ";
             
            // '''''''''''''
            filter = filter.Remove(filter.Length - 4, 4);

            strSQL = "SELECT '6' as TypeOpration,dbo.Acc_Currency." + PrimaryName + " as CurrncyName,dbo.Users." + PrimaryName + " as UserName,dbo.Users." + PrimaryName + " as UserNameUpdate, Stc_GoldOutOnBail_Master.DocumentID, dbo.Stc_GoldOutOnBail_Master.Cancel, dbo.Stc_GoldOutOnBail_Master.InvoiceID, dbo.Stc_GoldOutOnBail_Master.BranchID, "
            + " dbo.Stc_GoldOutOnBail_Master.InvoiceDate,sum(Stc_GoldOutOnBail_Details.TotalCost) AS total, "
            + "  dbo.Stc_Stores." + PrimaryName + " AS StoreName, dbo.Stc_GoldOutOnBail_Master.Notes, "
            + "    dbo.Acc_Accounts." + PrimaryName + " AS SupplierName,  "
            + " dbo.Acc_CostCenters.ArbName AS CostCenter FROM dbo.Stc_GoldOutOnBail_Master INNER JOIN dbo.Stc_GoldOutOnBail_Details ON dbo.Stc_GoldOutOnBail_Master.InvoiceID"
            + " = dbo.Stc_GoldOutOnBail_Details.InvoiceID AND dbo.Stc_GoldOutOnBail_Master.BranchID = dbo.Stc_GoldOutOnBail_Details.BranchID LEFT OUTER JOIN"
            + " dbo.Acc_CostCenters ON dbo.Stc_GoldOutOnBail_Master.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Stc_GoldOutOnBail_Master.CostCenterID = "
            + " dbo.Acc_CostCenters.CostCenterID    LEFT OUTER JOIN "
            + " dbo.Stc_Stores ON dbo.Stc_GoldOutOnBail_Master.BranchID = dbo.Stc_Stores.BranchID AND dbo.Stc_GoldOutOnBail_Master.StoreID = dbo.Stc_Stores.AccountID  "
            + "  LEFT OUTER JOIN Users on Stc_GoldOutOnBail_Master.UserID=Users.UserID and Stc_GoldOutOnBail_Master.BranchID=Users.BranchID or (Stc_GoldOutOnBail_Master.EditUserID=Users.UserID and Stc_GoldOutOnBail_Master.BranchID=Users.BranchID) "
            + "  LEFT OUTER JOIN Acc_Accounts on Stc_GoldOutOnBail_Master.CreditAccount=Acc_Accounts.AccountID and Stc_GoldOutOnBail_Master.BranchID=Acc_Accounts.BranchID "
            + "  LEFT OUTER JOIN Acc_Currency on Stc_GoldOutOnBail_Master.CurrencyID=Acc_Currency.ID and Stc_GoldOutOnBail_Master.BranchID=Acc_Currency.BranchID where " + filter + " GROUP BY "
            + "  Stc_GoldOutOnBail_Master.InvoiceEquivalenTotal ,  dbo.Stc_GoldOutOnBail_Master.InvoiceID,"
            + " dbo.Stc_GoldOutOnBail_Master.Cancel, dbo.Stc_GoldOutOnBail_Master.BranchID, dbo.Stc_GoldOutOnBail_Master.InvoiceDate,   "
            + " dbo.Stc_Stores.ArbName, dbo.Stc_GoldOutOnBail_Master.Notes,  Stc_GoldOutOnBail_Master.DocumentID, "
            + " dbo.Acc_Currency." + PrimaryName + ", dbo.Acc_CostCenters." + PrimaryName + ",dbo.Users." + PrimaryName + ",Acc_Accounts." + PrimaryName;
            Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());
            return strSQL;
        }
        string GetStrSQLGoldin()
        {

            
            Application.DoEvents();

            string filter = "";
            strSQL = "";

            if (Comon.cInt(cmbBranchesID.EditValue) > 0)
                filter = "(dbo.Stc_GoldInonBail_Master.BranchID = " + cmbBranchesID.EditValue + ") AND dbo.Stc_GoldInonBail_Master.InvoiceID >0     AND ";
            else 
               filter=" dbo.Stc_GoldInonBail_Master.InvoiceID >0  AND ";
            long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
            long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));

            DataTable dt;

            // حسب الرقم
            if (Comon.cInt(cmbStatus.EditValue) == 1)
                filter = filter + " dbo.Stc_GoldInonBail_Master.Cancel =1 AND ";

            if (txtFromTransactionID.Text != string.Empty)
                filter = filter + " dbo.Stc_GoldInonBail_Master.InvoiceID >=" + txtFromTransactionID.Text + " AND ";

            else if (Comon.cInt(cmbCurency.EditValue) > 0)
                filter = filter + " dbo.Stc_GoldInonBail_Master.CurrencyID=" + Comon.cInt(cmbCurency.EditValue) + " AND ";
            if (Comon.cInt(txtUserIDEntry.Text) > 0)
                filter = filter + " Stc_GoldInonBail_Master.UserID=" + Comon.cInt(txtUserIDEntry.Text) + " AND ";
            if (Comon.cInt(txtUserIDUpdated.Text) > 0)
                filter = filter + " Stc_GoldInonBail_Master.EditUserID=" + Comon.cInt(txtUserIDUpdated.Text) + " AND ";
            if (txtToTransactionsID.Text != string.Empty)
                filter = filter + " dbo.Stc_GoldInonBail_Master.InvoiceID <=" + txtToTransactionsID.Text + " AND ";
            if (txtReferanceID.Text.Trim() != string.Empty)
                filter = filter + "  Stc_GoldInonBail_Master.DocumentID=" + txtReferanceID.Text + " AND ";

            if (Comon.cDbl(txtBankID.Text) > 0)
                filter = filter + "  Stc_GoldInonBail_Master.CreditAccount=" + Comon.cDbl(txtBankID.Text) + " AND ";
            if (Comon.cDbl(txtBoxID.Text) > 0)
                filter = filter + "  Stc_GoldInonBail_Master.CreditAccount=" + Comon.cDbl(txtBoxID.Text) + " AND ";
            // حسب التاريخ
            if (FromDate != 0)
                filter = filter + " dbo.Stc_GoldInonBail_Master.InvoiceDate >=" + FromDate + " AND ";

            if (ToDate != 0)
                filter = filter + " dbo.Stc_GoldInonBail_Master.InvoiceDate <=" + ToDate + " AND ";

            // '''البائع''''العميل''''التكلفة''''المستودع
            if (txtStoreID.Text != string.Empty)
                filter = filter + " dbo.Stc_GoldInonBail_Master.StoreID  =" + Comon.cLong(txtStoreID.Text) + "  AND ";

            if (txtCostCenterID.Text != string.Empty)
                filter = filter + " dbo.Stc_GoldInonBail_Master.CostCenterID  =" + Comon.cInt(txtCostCenterID.Text) + "  AND ";

            if (txtCustomerOrSupplierID.Text != string.Empty)
                filter = filter + " dbo.Stc_GoldInonBail_Master.CreditAccount  =" + Comon.cLong(txtCustomerOrSupplierID.Text) + "  AND ";

            // '''''''''''''
            filter = filter.Remove(filter.Length - 4, 4);

            strSQL = "SELECT '5' as TypeOpration,dbo.Acc_Currency." + PrimaryName + " as CurrncyName,dbo.Users." + PrimaryName + " as UserName,dbo.Users." + PrimaryName + " as UserNameUpdate,  Stc_GoldInonBail_Master.DocumentID, dbo.Stc_GoldInonBail_Master.Cancel, dbo.Stc_GoldInonBail_Master.InvoiceID, dbo.Stc_GoldInonBail_Master.BranchID, "
            + " dbo.Stc_GoldInonBail_Master.InvoiceDate,sum( Stc_GoldInonBail_Details.TotalCost) AS total, "
            + "  dbo.Stc_Stores." + PrimaryName + " AS StoreName, dbo.Stc_GoldInonBail_Master.Notes, "
            + "    dbo.Acc_Accounts." + PrimaryName + " AS SupplierName,  "
            + " dbo.Acc_CostCenters.ArbName AS CostCenter FROM dbo.Stc_GoldInonBail_Master INNER JOIN dbo.Stc_GoldInonBail_Details ON dbo.Stc_GoldInonBail_Master.InvoiceID"
            + " = dbo.Stc_GoldInonBail_Details.InvoiceID AND dbo.Stc_GoldInonBail_Master.BranchID = dbo.Stc_GoldInonBail_Details.BranchID LEFT OUTER JOIN"
            + " dbo.Acc_CostCenters ON dbo.Stc_GoldInonBail_Master.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Stc_GoldInonBail_Master.CostCenterID = "
            + " dbo.Acc_CostCenters.CostCenterID    LEFT OUTER JOIN "
            + " dbo.Stc_Stores ON dbo.Stc_GoldInonBail_Master.BranchID = dbo.Stc_Stores.BranchID AND dbo.Stc_GoldInonBail_Master.StoreID = dbo.Stc_Stores.AccountID  "
            + "  LEFT OUTER JOIN Users on Stc_GoldInonBail_Master.UserID=Users.UserID and Stc_GoldInonBail_Master.BranchID=Users.BranchID or (Stc_GoldInonBail_Master.EditUserID=Users.UserID and Stc_GoldInonBail_Master.BranchID=Users.BranchID) "
            + "  LEFT OUTER JOIN Acc_Accounts on Stc_GoldInonBail_Master.CreditAccount=Acc_Accounts.AccountID and  Stc_GoldInonBail_Master.BranchID=Acc_Accounts.BranchID "
            + "  LEFT OUTER JOIN Acc_Currency on Stc_GoldInonBail_Master.CurrencyID=Acc_Currency.ID  and Stc_GoldInonBail_Master.BranchID=Acc_Currency.BranchID where " + filter + " GROUP BY "
            + "  Stc_GoldInonBail_Master.InvoiceEquivalenTotal ,  dbo.Stc_GoldInonBail_Master.InvoiceID,"
            + " dbo.Stc_GoldInonBail_Master.Cancel, dbo.Stc_GoldInonBail_Master.BranchID, dbo.Stc_GoldInonBail_Master.InvoiceDate,   "
            + " dbo.Stc_Stores.ArbName, dbo.Stc_GoldInonBail_Master.Notes,  Stc_GoldInonBail_Master.DocumentID, "
            + " dbo.Acc_Currency." + PrimaryName + ", dbo.Acc_CostCenters." + PrimaryName + ",dbo.Users." + PrimaryName + ",Acc_Accounts." + PrimaryName;
            Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

            return strSQL;


        }
        string GetStrSQLMatirialin()
        {
             
            Application.DoEvents();

            string filter ="";
            strSQL = "";

            if (Comon.cInt(cmbBranchesID.EditValue) > 0)
                filter = "(dbo.Stc_MatirialInonBail_Master.BranchID = " + cmbBranchesID.EditValue + ") AND dbo.Stc_MatirialInonBail_Master.InvoiceID >0     AND ";
            else
                filter = " dbo.Stc_MatirialInonBail_Master.InvoiceID >0    AND ";
            long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
            long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));

            DataTable dt;

            // حسب الرقم
            if (Comon.cInt(cmbStatus.EditValue) == 1)
                filter = filter + " dbo.Stc_MatirialInonBail_Master.Cancel =1 AND ";

            if (txtFromTransactionID.Text != string.Empty)
                filter = filter + " dbo.Stc_MatirialInonBail_Master.InvoiceID >=" + txtFromTransactionID.Text + " AND ";

            else if (Comon.cInt(cmbCurency.EditValue) > 0)
                filter = filter + " dbo.Stc_MatirialInonBail_Master.CurrencyID=" + Comon.cInt(cmbCurency.EditValue) + " AND ";
            if (Comon.cInt(txtUserIDEntry.Text) > 0)
                filter = filter + " Stc_MatirialInonBail_Master.UserID=" + Comon.cInt(txtUserIDEntry.Text) + " AND ";
            if (Comon.cInt(txtUserIDUpdated.Text) > 0)
                filter = filter + " Stc_MatirialInonBail_Master.EditUserID=" + Comon.cInt(txtUserIDUpdated.Text) + " AND ";
            if (txtToTransactionsID.Text != string.Empty)
                filter = filter + " dbo.Stc_MatirialInonBail_Master.InvoiceID <=" + txtToTransactionsID.Text + " AND ";
            if (txtReferanceID.Text.Trim() != string.Empty)
                filter = filter + "  Stc_MatirialInonBail_Master.DocumentID=" + txtReferanceID.Text + " AND ";

            if (Comon.cDbl(txtBankID.Text) > 0)
                filter = filter + "  Stc_MatirialInonBail_Master.CreditAccount=" + Comon.cDbl(txtBankID.Text) + " AND ";
            if (Comon.cDbl(txtBoxID.Text) > 0)
                filter = filter + "  Stc_MatirialInonBail_Master.CreditAccount=" + Comon.cDbl(txtBoxID.Text) + " AND ";
            // حسب التاريخ
            if (FromDate != 0)
                filter = filter + " dbo.Stc_MatirialInonBail_Master.InvoiceDate >=" + FromDate + " AND ";

            if (ToDate != 0)
                filter = filter + " dbo.Stc_MatirialInonBail_Master.InvoiceDate <=" + ToDate + " AND ";

            // '''البائع''''العميل''''التكلفة''''المستودع
            if (txtStoreID.Text != string.Empty)
                filter = filter + " dbo.Stc_MatirialInonBail_Master.StoreID  =" + Comon.cLong(txtStoreID.Text) + "  AND ";

            if (txtCostCenterID.Text != string.Empty)
                filter = filter + " dbo.Stc_MatirialInonBail_Master.CostCenterID  =" + Comon.cInt(txtCostCenterID.Text) + "  AND ";

            if (txtCustomerOrSupplierID.Text != string.Empty)
                filter = filter + " dbo.Stc_MatirialInonBail_Master.CreditAccount  =" + Comon.cLong(txtCustomerOrSupplierID.Text) + "  AND ";

            // '''''''''''''
            filter = filter.Remove(filter.Length - 4, 4);

            strSQL = "SELECT '7' as TypeOpration,dbo.Acc_Currency." + PrimaryName + " as CurrncyName,dbo.Users." + PrimaryName + " as UserName,dbo.Users." + PrimaryName + " as UserNameUpdate,   Stc_MatirialInonBail_Master.DocumentID, dbo.Stc_MatirialInonBail_Master.Cancel, dbo.Stc_MatirialInonBail_Master.InvoiceID, dbo.Stc_MatirialInonBail_Master.BranchID, "
            + " dbo.Stc_MatirialInonBail_Master.InvoiceDate,sum( Stc_MatirialInonBail_Details.TotalCost) AS total, "
            + "  dbo.Stc_Stores." + PrimaryName + " AS StoreName, dbo.Stc_MatirialInonBail_Master.Notes, "
            + "    dbo.Acc_Accounts." + PrimaryName + " AS SupplierName,  "
            + " dbo.Acc_CostCenters.ArbName AS CostCenter FROM dbo.Stc_MatirialInonBail_Master INNER JOIN dbo.Stc_MatirialInonBail_Details ON dbo.Stc_MatirialInonBail_Master.InvoiceID"
            + " = dbo.Stc_MatirialInonBail_Details.InvoiceID AND dbo.Stc_MatirialInonBail_Master.BranchID = dbo.Stc_MatirialInonBail_Details.BranchID LEFT OUTER JOIN"
            + " dbo.Acc_CostCenters ON dbo.Stc_MatirialInonBail_Master.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Stc_MatirialInonBail_Master.CostCenterID = "
            + " dbo.Acc_CostCenters.CostCenterID    LEFT OUTER JOIN "
            + " dbo.Stc_Stores ON dbo.Stc_MatirialInonBail_Master.BranchID = dbo.Stc_Stores.BranchID AND dbo.Stc_MatirialInonBail_Master.StoreID = dbo.Stc_Stores.AccountID  "
            + "  LEFT OUTER JOIN Users on Stc_MatirialInonBail_Master.UserID=Users.UserID and Stc_MatirialInonBail_Master.BranchID=Users.BranchID or ( Stc_MatirialInonBail_Master.EditUserID=Users.UserID and  Stc_MatirialInonBail_Master.BranchID=Users.BranchID) "
            + "  LEFT OUTER JOIN Acc_Accounts on Stc_MatirialInonBail_Master.CreditAccount=Acc_Accounts.AccountID and Stc_MatirialInonBail_Master.BranchID=Acc_Accounts.BranchID "
            + "  LEFT OUTER JOIN Acc_Currency on Stc_MatirialInonBail_Master.CurrencyID=Acc_Currency.ID and Stc_MatirialInonBail_Master.BranchID=Acc_Currency.BranchID where " + filter + " GROUP BY "
            + "    dbo.Stc_MatirialInonBail_Master.InvoiceID,"
            + " dbo.Stc_MatirialInonBail_Master.Cancel, dbo.Stc_MatirialInonBail_Master.BranchID, dbo.Stc_MatirialInonBail_Master.InvoiceDate,   "
            + " dbo.Stc_Stores.ArbName, dbo.Stc_MatirialInonBail_Master.Notes,  Stc_MatirialInonBail_Master.DocumentID, "
            + " dbo.Acc_Currency." + PrimaryName + ", dbo.Acc_CostCenters." + PrimaryName + ",dbo.Users." + PrimaryName + ",Acc_Accounts." + PrimaryName;
            Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

            return strSQL;


        }
        string GetStrSQLMatirialOut()
        {
             
            Application.DoEvents();

            string filter = " ";
            strSQL = "";

            if (Comon.cInt(cmbBranchesID.EditValue) > 0)
                filter = "(dbo.Stc_MatirialOutOnBail_Master.BranchID = " + cmbBranchesID.EditValue + ") AND dbo.Stc_MatirialOutOnBail_Master.InvoiceID >0     AND ";
            else
                filter = "  dbo.Stc_MatirialOutOnBail_Master.InvoiceID >0    AND ";
            long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
            long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));

            DataTable dt;

            // حسب الرقم
            if (Comon.cInt(cmbStatus.EditValue) == 1)
                filter = filter + " dbo.Stc_MatirialOutOnBail_Master.Cancel =1 AND ";

            if (txtFromTransactionID.Text != string.Empty)
                filter = filter + " dbo.Stc_MatirialOutOnBail_Master.InvoiceID >=" + txtFromTransactionID.Text + " AND ";

            else if (Comon.cInt(cmbCurency.EditValue) > 0)
                filter = filter + " dbo.Stc_MatirialOutOnBail_Master.CurrencyID=" + Comon.cInt(cmbCurency.EditValue) + " AND ";
            if (Comon.cInt(txtUserIDEntry.Text) > 0)
                filter = filter + " Stc_MatirialOutOnBail_Master.UserID=" + Comon.cInt(txtUserIDEntry.Text) + " AND ";
            if (Comon.cInt(txtUserIDUpdated.Text) > 0)
                filter = filter + " Stc_MatirialOutOnBail_Master.EditUserID=" + Comon.cInt(txtUserIDUpdated.Text) + " AND ";
            if (txtToTransactionsID.Text != string.Empty)
                filter = filter + " dbo.Stc_MatirialOutOnBail_Master.InvoiceID <=" + txtToTransactionsID.Text + " AND ";
            if (txtReferanceID.Text.Trim() != string.Empty)
                filter = filter + "  Stc_MatirialOutOnBail_Master.DocumentID=" + txtReferanceID.Text + " AND ";

            if (Comon.cDbl(txtBankID.Text) > 0)
                filter = filter + "  Stc_MatirialOutOnBail_Master.CreditAccount=" + Comon.cDbl(txtBankID.Text) + " AND ";
            if (Comon.cDbl(txtBoxID.Text) > 0)
                filter = filter + "  Stc_MatirialOutOnBail_Master.CreditAccount=" + Comon.cDbl(txtBoxID.Text) + " AND ";
            // حسب التاريخ
            if (FromDate != 0)
                filter = filter + " dbo.Stc_MatirialOutOnBail_Master.InvoiceDate >=" + FromDate + " AND ";

            if (ToDate != 0)
                filter = filter + " dbo.Stc_MatirialOutOnBail_Master.InvoiceDate <=" + ToDate + " AND ";

            // '''البائع''''العميل''''التكلفة''''المستودع
            if (txtStoreID.Text != string.Empty)
                filter = filter + " dbo.Stc_MatirialOutOnBail_Master.StoreID  =" + Comon.cLong(txtStoreID.Text) + "  AND ";

            if (txtCostCenterID.Text != string.Empty)
                filter = filter + " dbo.Stc_MatirialOutOnBail_Master.CostCenterID  =" + Comon.cInt(txtCostCenterID.Text) + "  AND ";

            if (txtCustomerOrSupplierID.Text != string.Empty)
                filter = filter + " dbo.Stc_MatirialOutOnBail_Master.CreditAccount  =" + Comon.cLong(txtCustomerOrSupplierID.Text) + "  AND ";

            // '''''''''''''
            filter = filter.Remove(filter.Length - 4, 4);

            strSQL = "SELECT '8' as TypeOpration,dbo.Acc_Currency." + PrimaryName + " as CurrncyName,dbo.Users." + PrimaryName + " as UserName,dbo.Users." + PrimaryName + " as UserNameUpdate,   Stc_MatirialOutOnBail_Master.DocumentID, dbo.Stc_MatirialOutOnBail_Master.Cancel, dbo.Stc_MatirialOutOnBail_Master.InvoiceID, dbo.Stc_MatirialOutOnBail_Master.BranchID, "
            + " dbo.Stc_MatirialOutOnBail_Master.InvoiceDate,sum( Stc_MatirialOutOnBail_Details.TotalCost) AS total, "
            + "  dbo.Stc_Stores." + PrimaryName + " AS StoreName, dbo.Stc_MatirialOutOnBail_Master.Notes, "
            + "    dbo.Acc_Accounts." + PrimaryName + " AS SupplierName,  "
            + " dbo.Acc_CostCenters.ArbName AS CostCenter FROM dbo.Stc_MatirialOutOnBail_Master INNER JOIN dbo.Stc_MatirialOutOnBail_Details ON dbo.Stc_MatirialOutOnBail_Master.InvoiceID"
            + " = dbo.Stc_MatirialOutOnBail_Details.InvoiceID AND dbo.Stc_MatirialOutOnBail_Master.BranchID = dbo.Stc_MatirialOutOnBail_Details.BranchID LEFT OUTER JOIN"
            + " dbo.Acc_CostCenters ON dbo.Stc_MatirialOutOnBail_Master.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Stc_MatirialOutOnBail_Master.CostCenterID = "
            + " dbo.Acc_CostCenters.CostCenterID    LEFT OUTER JOIN "
            + " dbo.Stc_Stores ON dbo.Stc_MatirialOutOnBail_Master.BranchID = dbo.Stc_Stores.BranchID AND dbo.Stc_MatirialOutOnBail_Master.StoreID = dbo.Stc_Stores.AccountID  "
            + "  LEFT OUTER JOIN Users on Stc_MatirialOutOnBail_Master.UserID=Users.UserID and Stc_MatirialOutOnBail_Master.BranchID=Users.BranchID or ( Stc_MatirialOutOnBail_Master.EditUserID=Users.UserID  and Stc_MatirialOutOnBail_Master.BranchID=Users.BranchID ) "
            + "  LEFT OUTER JOIN Acc_Accounts on Stc_MatirialOutOnBail_Master.CreditAccount=Acc_Accounts.AccountID and Stc_MatirialOutOnBail_Master.BranchID=Acc_Accounts.BranchID "
            + "  LEFT OUTER JOIN Acc_Currency on Stc_MatirialOutOnBail_Master.CurrencyID=Acc_Currency.ID and Stc_MatirialOutOnBail_Master.BranchID=Acc_Currency.BranchID where " + filter + " GROUP BY "
            + "    dbo.Stc_MatirialOutOnBail_Master.InvoiceID,"
            + " dbo.Stc_MatirialOutOnBail_Master.Cancel, dbo.Stc_MatirialOutOnBail_Master.BranchID, dbo.Stc_MatirialOutOnBail_Master.InvoiceDate,   "
            + " dbo.Stc_Stores.ArbName, dbo.Stc_MatirialOutOnBail_Master.Notes,  Stc_MatirialOutOnBail_Master.DocumentID, "
            + " dbo.Acc_Currency." + PrimaryName + ", dbo.Acc_CostCenters." + PrimaryName + ",dbo.Users." + PrimaryName + ",Acc_Accounts." + PrimaryName;
            Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

            return strSQL;


        }
        string GetStrSQLMaltiGold()
        {

       
            Application.DoEvents();

            string filter = "";
            strSQL = "";

            if (Comon.cInt(cmbBranchesID.EditValue) > 0)
                filter = "(dbo.Stc_TransferMultipleStoresGold_Master.BranchID = " + cmbBranchesID.EditValue + ") AND dbo.Stc_TransferMultipleStoresGold_Master.InvoiceID >0     AND ";
            else
                filter = " dbo.Stc_TransferMultipleStoresGold_Master.InvoiceID >0   AND ";
            long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
            long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));

            DataTable dt;

            // حسب الرقم
            if (Comon.cInt(cmbStatus.EditValue) == 1)
                filter = filter + " dbo.Stc_TransferMultipleStoresGold_Master.Cancel =1 AND ";

            if (txtFromTransactionID.Text != string.Empty)
                filter = filter + " dbo.Stc_TransferMultipleStoresGold_Master.InvoiceID >=" + txtFromTransactionID.Text + " AND ";

            else if (Comon.cInt(cmbCurency.EditValue) > 0)
                filter = filter + " dbo.Stc_TransferMultipleStoresGold_Master.CurrencyID=" + Comon.cInt(cmbCurency.EditValue) + " AND ";
            if (Comon.cInt(txtUserIDEntry.Text) > 0)
                filter = filter + " Stc_TransferMultipleStoresGold_Master.UserID=" + Comon.cInt(txtUserIDEntry.Text) + " AND ";
            if (Comon.cInt(txtUserIDUpdated.Text) > 0)
                filter = filter + " Stc_TransferMultipleStoresGold_Master.EditUserID=" + Comon.cInt(txtUserIDUpdated.Text) + " AND ";
            if (txtToTransactionsID.Text != string.Empty)
                filter = filter + " dbo.Stc_TransferMultipleStoresGold_Master.InvoiceID <=" + txtToTransactionsID.Text + " AND ";
            if (txtReferanceID.Text.Trim() != string.Empty)
                filter = filter + "  Stc_TransferMultipleStoresGold_Master.DocumentID=" + txtReferanceID.Text + " AND ";

            if (Comon.cDbl(txtBankID.Text) > 0)
                filter = filter + "  Stc_TransferMultipleStoresGold_Master.CreditAccount=" + Comon.cDbl(txtBankID.Text) + " AND ";
            if (Comon.cDbl(txtBoxID.Text) > 0)
                filter = filter + "  Stc_TransferMultipleStoresGold_Master.CreditAccount=" + Comon.cDbl(txtBoxID.Text) + " AND ";
            // حسب التاريخ
            if (FromDate != 0)
                filter = filter + " dbo.Stc_TransferMultipleStoresGold_Master.InvoiceDate >=" + FromDate + " AND ";

            if (ToDate != 0)
                filter = filter + " dbo.Stc_TransferMultipleStoresGold_Master.InvoiceDate <=" + ToDate + " AND ";

            // '''البائع''''العميل''''التكلفة''''المستودع
            if (txtStoreID.Text != string.Empty)
                filter = filter + " dbo.Stc_TransferMultipleStoresGold_Master.StoreID  =" + Comon.cLong(txtStoreID.Text) + "  AND ";

            if (txtCostCenterID.Text != string.Empty)
                filter = filter + " dbo.Stc_TransferMultipleStoresGold_Master.CostCenterID  =" + Comon.cInt(txtCostCenterID.Text) + "  AND ";

            if (txtCustomerOrSupplierID.Text != string.Empty)
                filter = filter + " dbo.Stc_TransferMultipleStoresGold_Master.CreditAccount  =" + Comon.cLong(txtCustomerOrSupplierID.Text) + "  AND ";

            // '''''''''''''
            filter = filter.Remove(filter.Length - 4, 4);

            strSQL = "SELECT '9' as TypeOpration,dbo.Acc_Currency." + PrimaryName + " as CurrncyName,dbo.Users." + PrimaryName + " as UserName,dbo.Users." + PrimaryName + " as UserNameUpdate,  Stc_TransferMultipleStoresGold_Master.DocumentID, dbo.Stc_TransferMultipleStoresGold_Master.Cancel, dbo.Stc_TransferMultipleStoresGold_Master.InvoiceID, dbo.Stc_TransferMultipleStoresGold_Master.BranchID, "
            + " dbo.Stc_TransferMultipleStoresGold_Master.InvoiceDate,sum( Stc_TransferMultipleStoresGold_Details.TotalCost) AS total, "
            + "  dbo.Stc_Stores." + PrimaryName + " AS StoreName, dbo.Stc_TransferMultipleStoresGold_Master.Notes, "
            + "    dbo.Acc_Accounts." + PrimaryName + " AS SupplierName,  "
            + " dbo.Acc_CostCenters.ArbName AS CostCenter FROM dbo.Stc_TransferMultipleStoresGold_Master INNER JOIN dbo.Stc_TransferMultipleStoresGold_Details ON dbo.Stc_TransferMultipleStoresGold_Master.InvoiceID"
            + " = dbo.Stc_TransferMultipleStoresGold_Details.InvoiceID AND dbo.Stc_TransferMultipleStoresGold_Master.BranchID = dbo.Stc_TransferMultipleStoresGold_Details.BranchID LEFT OUTER JOIN"
            + " dbo.Acc_CostCenters ON dbo.Stc_TransferMultipleStoresGold_Master.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Stc_TransferMultipleStoresGold_Master.CostCenterID = "
            + " dbo.Acc_CostCenters.CostCenterID    LEFT OUTER JOIN "
            + " dbo.Stc_Stores ON dbo.Stc_TransferMultipleStoresGold_Master.BranchID = dbo.Stc_Stores.BranchID AND dbo.Stc_TransferMultipleStoresGold_Master.StoreID = dbo.Stc_Stores.AccountID  "
            + "  LEFT OUTER JOIN Users on Stc_TransferMultipleStoresGold_Master.UserID=Users.UserID  and Stc_TransferMultipleStoresGold_Master.BranchID=Users.BranchID or (Stc_TransferMultipleStoresGold_Master.EditUserID=Users.UserID and Stc_TransferMultipleStoresGold_Master.BranchID=Users.BranchID) "
            + "  LEFT OUTER JOIN Acc_Accounts on Stc_TransferMultipleStoresGold_Master.CreditAccount=Acc_Accounts.AccountID and Stc_TransferMultipleStoresGold_Master.BranchID=Acc_Accounts.BranchID "
            + "  LEFT OUTER JOIN Acc_Currency on Stc_TransferMultipleStoresGold_Master.CurrencyID=Acc_Currency.ID and Stc_TransferMultipleStoresGold_Master.BranchID=Acc_Currency.BranchID where " + filter + " GROUP BY "
            + "    dbo.Stc_TransferMultipleStoresGold_Master.InvoiceID,"
            + " dbo.Stc_TransferMultipleStoresGold_Master.Cancel, dbo.Stc_TransferMultipleStoresGold_Master.BranchID, dbo.Stc_TransferMultipleStoresGold_Master.InvoiceDate,   "
            + " dbo.Stc_Stores.ArbName, dbo.Stc_TransferMultipleStoresGold_Master.Notes,  Stc_TransferMultipleStoresGold_Master.DocumentID, "
            + " dbo.Acc_Currency." + PrimaryName + ", dbo.Acc_CostCenters." + PrimaryName + ",dbo.Users." + PrimaryName + ",Acc_Accounts." + PrimaryName;
            Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

            return strSQL;


        }
        string GetStrSQLMaltiMatirial()
        {

          
            Application.DoEvents();

            string filter = " ";
            strSQL = "";

            if (Comon.cInt(cmbBranchesID.EditValue) > 0)
                filter = "(dbo.Stc_TransferMultipleStoresMatirial_Master.BranchID = " + cmbBranchesID.EditValue + ") AND dbo.Stc_TransferMultipleStoresMatirial_Master.InvoiceID >0     AND ";
            else
                filter = " dbo.Stc_TransferMultipleStoresMatirial_Master.InvoiceID >0    AND ";
            long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
            long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));

            DataTable dt;

            // حسب الرقم
            if (Comon.cInt(cmbStatus.EditValue) == 1)
                filter = filter + " dbo.Stc_TransferMultipleStoresMatirial_Master.Cancel =1 AND ";

            if (txtFromTransactionID.Text != string.Empty)
                filter = filter + " dbo.Stc_TransferMultipleStoresMatirial_Master.InvoiceID >=" + txtFromTransactionID.Text + " AND ";

            else if (Comon.cInt(cmbCurency.EditValue) > 0)
                filter = filter + " dbo.Stc_TransferMultipleStoresMatirial_Master.CurrencyID=" + Comon.cInt(cmbCurency.EditValue) + " AND ";
            if (Comon.cInt(txtUserIDEntry.Text) > 0)
                filter = filter + " Stc_TransferMultipleStoresMatirial_Master.UserID=" + Comon.cInt(txtUserIDEntry.Text) + " AND ";
            if (Comon.cInt(txtUserIDUpdated.Text) > 0)
                filter = filter + " Stc_TransferMultipleStoresMatirial_Master.EditUserID=" + Comon.cInt(txtUserIDUpdated.Text) + " AND ";
            if (txtToTransactionsID.Text != string.Empty)
                filter = filter + " dbo.Stc_TransferMultipleStoresMatirial_Master.InvoiceID <=" + txtToTransactionsID.Text + " AND ";
            if (txtReferanceID.Text.Trim() != string.Empty)
                filter = filter + "  Stc_TransferMultipleStoresMatirial_Master.DocumentID=" + txtReferanceID.Text + " AND ";

            if (Comon.cDbl(txtBankID.Text) > 0)
                filter = filter + "  Stc_TransferMultipleStoresMatirial_Master.CreditAccount=" + Comon.cDbl(txtBankID.Text) + " AND ";
            if (Comon.cDbl(txtBoxID.Text) > 0)
                filter = filter + "  Stc_TransferMultipleStoresMatirial_Master.CreditAccount=" + Comon.cDbl(txtBoxID.Text) + " AND ";
            // حسب التاريخ
            if (FromDate != 0)
                filter = filter + " dbo.Stc_TransferMultipleStoresMatirial_Master.InvoiceDate >=" + FromDate + " AND ";
            if (ToDate != 0)
                filter = filter + " dbo.Stc_TransferMultipleStoresMatirial_Master.InvoiceDate <=" + ToDate + " AND ";
            // '''البائع''''العميل''''التكلفة''''المستودع
            if (txtStoreID.Text != string.Empty)
                filter = filter + " dbo.Stc_TransferMultipleStoresMatirial_Master.StoreID  =" + Comon.cLong(txtStoreID.Text) + "  AND ";
            if (txtCostCenterID.Text != string.Empty)
                filter = filter + " dbo.Stc_TransferMultipleStoresMatirial_Master.CostCenterID  =" + Comon.cInt(txtCostCenterID.Text) + "  AND ";
            if (txtCustomerOrSupplierID.Text != string.Empty)
                filter = filter + " dbo.Stc_TransferMultipleStoresMatirial_Master.CreditAccount  =" + Comon.cLong(txtCustomerOrSupplierID.Text) + "  AND ";
            // '''''''''''''
            filter = filter.Remove(filter.Length - 4, 4);

            strSQL = "SELECT '1' as TypeOpration,dbo.Acc_Currency." + PrimaryName + " as CurrncyName,dbo.Users." + PrimaryName + " as UserName,dbo.Users." + PrimaryName + " as UserNameUpdate, Stc_TransferMultipleStoresMatirial_Master.DocumentID, dbo.Stc_TransferMultipleStoresMatirial_Master.Cancel, dbo.Stc_TransferMultipleStoresMatirial_Master.InvoiceID, dbo.Stc_TransferMultipleStoresMatirial_Master.BranchID, "
            + " dbo.Stc_TransferMultipleStoresMatirial_Master.InvoiceDate,sum( Stc_TransferMultipleStoresMatirial_Details.TotalCost) AS total, "
            + "  dbo.Stc_Stores." + PrimaryName + " AS StoreName, dbo.Stc_TransferMultipleStoresMatirial_Master.Notes, "
            + "  '1' as SupplierName,dbo.Acc_CostCenters.ArbName AS CostCenter FROM dbo.Stc_TransferMultipleStoresMatirial_Master INNER JOIN dbo.Stc_TransferMultipleStoresMatirial_Details ON dbo.Stc_TransferMultipleStoresMatirial_Master.InvoiceID"
            + " = dbo.Stc_TransferMultipleStoresMatirial_Details.InvoiceID AND dbo.Stc_TransferMultipleStoresMatirial_Master.BranchID = dbo.Stc_TransferMultipleStoresMatirial_Details.BranchID LEFT OUTER JOIN"
            + " dbo.Acc_CostCenters ON dbo.Stc_TransferMultipleStoresMatirial_Master.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Stc_TransferMultipleStoresMatirial_Master.CostCenterID = "
            + " dbo.Acc_CostCenters.CostCenterID    LEFT OUTER JOIN "
            + " dbo.Stc_Stores ON dbo.Stc_TransferMultipleStoresMatirial_Master.BranchID = dbo.Stc_Stores.BranchID AND dbo.Stc_TransferMultipleStoresMatirial_Master.StoreID = dbo.Stc_Stores.AccountID  "
            + "  LEFT OUTER JOIN Users on Stc_TransferMultipleStoresMatirial_Master.UserID=Users.UserID and Stc_TransferMultipleStoresMatirial_Master.BranchID=Users.BranchID or (Stc_TransferMultipleStoresMatirial_Master.EditUserID=Users.UserID and Stc_TransferMultipleStoresMatirial_Master.BranchID=Users.BranchID) "
            + "  LEFT OUTER JOIN Acc_Currency on Stc_TransferMultipleStoresMatirial_Master.CurrencyID=Acc_Currency.ID and Stc_TransferMultipleStoresMatirial_Master.BranchID=Acc_Currency.BranchID where " + filter + " GROUP BY "
            + "    dbo.Stc_TransferMultipleStoresMatirial_Master.InvoiceID,"
            + " dbo.Stc_TransferMultipleStoresMatirial_Master.Cancel, dbo.Stc_TransferMultipleStoresMatirial_Master.BranchID, dbo.Stc_TransferMultipleStoresMatirial_Master.InvoiceDate,   "
            + " dbo.Stc_Stores.ArbName, dbo.Stc_TransferMultipleStoresMatirial_Master.Notes,  Stc_TransferMultipleStoresMatirial_Master.DocumentID, "
            + " dbo.Acc_Currency." + PrimaryName + ", dbo.Acc_CostCenters." + PrimaryName + ",dbo.Users." + PrimaryName ;
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

                if (fromType == 5 || (toType >= 5 && fromType <= 5) || (toType == fromType && fromType <= 0))
                {
                    dt = Lip.SelectRecord(GetStrSQLGoldin());
                }

                if ((toType == fromType && fromType <= 0) ||
                     ((fromType >= 5 && fromType <= 6 && toType >= 6 && (toType != fromType && toType >= 6)) ||
                     (fromType >= 5 && fromType <= 6 && toType <= 0) ||
                     (toType >= 6 && fromType <= 0)) ||
                     (toType == fromType && toType == 6))
                {
                    dt.Merge(Lip.SelectRecord(GetStrSQLGoldOut()));
                }
                if ((toType == fromType && fromType <= 0) ||
                     ((fromType >= 5 && toType >= 7 && (toType != fromType && toType >= 7)) ||
                     (fromType >= 5 && fromType <= 7 && toType <= 0) || (toType >= 7 && fromType <= 0)) ||
                     (toType == fromType && toType == 7))
                {
                    dt.Merge(Lip.SelectRecord(GetStrSQLMatirialin()));
                }
                if ((toType == fromType && fromType <= 0) ||
                    ((fromType >= 5 && toType >= 8 && (toType != fromType && toType >= 8)) ||
                    (fromType >= 5 && fromType <= 8 && toType <= 0) || (toType >= 8 && fromType <= 0)) ||
                    (toType == fromType && toType == 8))
                {
                    dt.Merge(Lip.SelectRecord(GetStrSQLMatirialOut()));
                }
                if ((toType == fromType && fromType <= 0) ||
                   ((fromType >= 5 && toType >= 9 && (toType != fromType && toType >= 9)) ||
                   (fromType >= 5 && fromType <= 9 && toType <= 0) || (toType >= 9 && fromType <= 0)) ||
                   (toType == fromType && toType == 9))
                {
                    dt.Merge(Lip.SelectRecord(GetStrSQLMaltiGold()));
                }
                if ((toType == fromType && fromType <= 0) ||
                         ((fromType >= 5 && toType >= 10 && (toType != fromType && toType >= 10)) ||
                         (fromType >= 5 && fromType <= 10 && toType <= 0) || (toType >= 10 && fromType <= 0)) ||
                         (toType == fromType && toType == 10))
                {
                     dt.Merge(Lip.SelectRecord(GetStrSQLMaltiMatirial()));
                    

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
                            row["InvoiceDate"] = Comon.ConvertSerialDateTo(dt.Rows[i]["InvoiceDate"].ToString());
                            row["NetAmmount"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["Total"]).ToString("N" + 3);
                            row["CurrncyName"] = (dt.Rows[i]["CurrncyName"].ToString() != string.Empty ? dt.Rows[i]["CurrncyName"] : "");
                            if (Comon.cInt(dt.Rows[i]["Cancel"]) == 1)
                                row["StatUs"] = UserInfo.Language == iLanguage.Arabic ? "محذوف" : "Deleted";
                            else
                                row["StatUs"] = UserInfo.Language == iLanguage.Arabic ? "مرحل " : "Aported";
                            if (Comon.cInt(dt.Rows[i]["TypeOpration"]) == 5)
                                row["RecordType"] = UserInfo.Language == iLanguage.Arabic ? "توريد ذهب  " : "Purchase";
                            else if (Comon.cInt(dt.Rows[i]["TypeOpration"]) == 6)
                                row["RecordType"] = UserInfo.Language == iLanguage.Arabic ? " صرف ذهب" : "Purchase Return";
                            else if (Comon.cInt(dt.Rows[i]["TypeOpration"]) == 7)
                                row["RecordType"] = UserInfo.Language == iLanguage.Arabic ? "توريد مواد خام" : "Purchase Return";
                            else if (Comon.cInt(dt.Rows[i]["TypeOpration"]) == 8)
                                row["RecordType"] = UserInfo.Language == iLanguage.Arabic ? "صرف مواد خام" : "Purchase Return";
                            else if (Comon.cInt(dt.Rows[i]["TypeOpration"]) == 9)
                                row["RecordType"] = UserInfo.Language == iLanguage.Arabic ? "تحويل مخزني متعدد ذهب " : "Maltie Store Gold Transfer  ";
                            else if (Comon.cInt(dt.Rows[i]["TypeOpration"]) == 1)
                                row["RecordType"] = UserInfo.Language == iLanguage.Arabic ? "تحويل مخزني متعدد خام " : "Maltie Store Matirial Transfer  ";
                            row["TypeOpration"] = dt.Rows[i]["TypeOpration"].ToString(); 
                            row["UserNameUpdate"] = (dt.Rows[i]["UserNameUpdate"].ToString() != string.Empty ? dt.Rows[i]["UserNameUpdate"] : "");
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
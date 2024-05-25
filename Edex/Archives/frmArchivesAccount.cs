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
using Edex.AccountsObjects.Transactions;

namespace Edex.Archives
{
  
    public partial class frmArchivesAccount : BaseForm
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
        public frmArchivesAccount()
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
            FillCombo.FillComboBoxLookUpEdit(cmbFromType, "Arc_ArchivesType", "ID", PrimaryName, "", "ID>10 and ID<=14", (UserInfo.Language == iLanguage.English ? "Select Type" : "حدد النوع "));
            FillCombo.FillComboBoxLookUpEdit(cmbToType, "Arc_ArchivesType", "ID", PrimaryName, "", "ID>10 and ID<=14", (UserInfo.Language == iLanguage.English ? "Select Type" : "حدد النوع "));
            cmbBranchesID.EditValue = MySession.GlobalBranchID;
            cmbBranchesID.ReadOnly = !MySession.GlobalAllowBranchModificationAllScreens;
            FillCombo.FillComboBoxLookUpEdit(cmbStatus, "Manu_TypeStatus", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select StatUs" : "حدد الحالة "));
            FillCombo.FillComboBox(cmbCurency, "Acc_Currency", "ID", PrimaryName, "", "BranchID="+MySession.GlobalBranchID, (UserInfo.Language == iLanguage.English ? "Select " : "حدد العملة"));
            this.KeyDown += frmArchivesAccount_KeyDown; 
            this.txtCostCenterID.Validating += txtCostCenterID_Validating;
            this.txtDelegeteID.Validating += txtDelegeteID_Validating;
            this.txtBankID.Validating += txtBankID_Validating;
            this.txtBoxID.Validating += txtBoxID_Validating;
            this.Load += frmArchivesAccount_Load;

            this.txtUserIDEntry.Validating += txtUserIDEntry_Validating;
            this.txtUserIDUpdated.Validating += txtUserIDUpdated_Validating;

            this.gridView1.RowCellStyle += gridView1_RowCellStyle;
            this.gridView1.DoubleClick += gridView1_DoubleClick;
        }

        void gridView1_DoubleClick(object sender, EventArgs e)
        {
          
            try{
            GridView view = sender as GridView;

            switch (view.GetFocusedRowCellValue("TypeOpration").ToString())
            {
                case "11":
                    frmReceiptVoucher frm = new frmReceiptVoucher();
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
                case "12":
                    frmSpendVoucher frmSpend = new frmSpendVoucher();
                    if (Permissions.UserPermissionsFrom(frmSpend, frmSpend.ribbonControl1, UserInfo.ID,  MySession.GlobalBranchID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frmSpend);
                        frmSpend.Show();
                        frmSpend.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frmSpend.Dispose();
                    break;
                case "13":
                    frmVariousVoucher frmVarois = new frmVariousVoucher();
                    if (Permissions.UserPermissionsFrom(frmVarois, frmVarois.ribbonControl1, UserInfo.ID,  MySession.GlobalBranchID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frmVarois);
                        frmVarois.Show();
                        frmVarois.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frmVarois.Dispose();
                    break;
                case "14":
                    frmOpeningVoucher frmOpening = new frmOpeningVoucher();
                    if (Permissions.UserPermissionsFrom(frmOpening, frmOpening.ribbonControl1, UserInfo.ID,  MySession.GlobalBranchID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frmOpening);
                        frmOpening.Show();
                        frmOpening.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frmOpening.Dispose();
                    break;
            }
            }
            catch(Exception ex){

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
                strSQL = "SELECT " + PrimaryName + " as UserName FROM Users WHERE UserID =" + Comon.cInt(txtUserIDUpdated.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtUserIDUpdated, lblUserNameUpdated, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        void frmArchivesAccount_Load(object sender, EventArgs e)
        {
            _sampleData.Columns.Add(new DataColumn("Sn", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("RecordType", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("ID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("RefranceID", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("CurrncyName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("NetAmmount", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("AccountIDDibet", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("AccountIDCredit", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("StatUs", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("UserName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("UserNameUpdate", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("CostCenterName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("TypeOpration", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Notes", typeof(string)));
        }



        void txtBoxID_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE  (Cancel = 0) AND (AccountID = " + txtBoxID.Text + ") And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtBoxID, lblBoxeName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

     
        void frmArchivesAccount_KeyDown(object sender, KeyEventArgs e)
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

                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE  (Cancel = 0) AND (AccountID = " + txtBankID.Text + ") And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
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
                strSQL = "SELECT " + PrimaryName + " as DelegateName FROM Sales_SalesDelegate WHERE DelegateID =" + txtDelegeteID.Text + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
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
                txtReferanceID.Text = "";  
                txtUserIDEntry.Text = "";
                txtUserIDUpdated.Text = "";

                txtFromDate.Text = "";
                txtToDate.Text = "";
                txtCostCenterID.Text = "";
                lblCostCenterName.Text = "";

                txtFromDate.Enabled = true;
                txtToDate.Enabled = true;
                txtCostCenterID.Enabled = true;
                 
                txtCostCenterID.Enabled = true; 
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

        string GetStrSQLReceiptVoucher()
        {

            //btnShow.Visible = false;
            Application.DoEvents();

            string filter = "(dbo.Acc_ReceiptVoucherMaster.BranchID = " +MySession.GlobalBranchID + ") AND dbo.Acc_ReceiptVoucherMaster.ReceiptVoucherID >0    AND ";
            strSQL = "";
            if (Comon.cInt(cmbBranchesID.EditValue) > 0)
                filter = "(dbo.Acc_ReceiptVoucherMaster.BranchID = " + cmbBranchesID.EditValue + ") AND dbo.Acc_ReceiptVoucherMaster.ReceiptVoucherID >0     AND ";

            long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
            long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));

            DataTable dt;
            // حسب الرقم
            if (Comon.cInt(cmbStatus.EditValue) == 1)
                filter = filter + " dbo.Acc_ReceiptVoucherMaster.Cancel =1 AND ";

            if (txtFromTransactionID.Text != string.Empty)
                filter = filter + " dbo.Acc_ReceiptVoucherMaster.InvoiceID >=" + txtFromTransactionID.Text + " AND ";

            else if (Comon.cInt(cmbCurency.EditValue) > 0)
                filter = filter + " dbo.Acc_ReceiptVoucherMaster.CurrencyID=" + Comon.cInt(cmbCurency.EditValue) + " AND ";
            if (Comon.cInt(txtUserIDEntry.Text) > 0)
                filter = filter + " Acc_ReceiptVoucherMaster.UserID=" + Comon.cInt(txtUserIDEntry.Text) + " AND ";
            if (Comon.cInt(txtUserIDUpdated.Text) > 0)
                filter = filter + " Acc_ReceiptVoucherMaster.EditUserID=" + Comon.cInt(txtUserIDUpdated.Text) + " AND ";
            if (txtToTransactionsID.Text != string.Empty)
                filter = filter + " dbo.Acc_ReceiptVoucherMaster.InvoiceID <=" + txtToTransactionsID.Text + " AND ";
            if (txtReferanceID.Text.Trim() != string.Empty)
                filter = filter + "  Acc_ReceiptVoucherMaster.DocumentID=" + txtReferanceID.Text + " AND ";

            if (Comon.cDbl(txtBankID.Text) > 0)
                filter = filter + "  Acc_ReceiptVoucherMaster.DebitAccountID=" + Comon.cDbl(txtBankID.Text) + " AND ";
            if (Comon.cDbl(txtBoxID.Text) > 0)
                filter = filter + "  Acc_ReceiptVoucherDetails.AccountID=" + Comon.cDbl(txtBoxID.Text) + " AND ";
            // حسب التاريخ
            if (FromDate != 0)
                filter = filter + " dbo.Acc_ReceiptVoucherMaster.ReceiptVoucherDate >=" + FromDate + " AND ";
            if (ToDate != 0)
                filter = filter + " dbo.Acc_ReceiptVoucherMaster.ReceiptVoucherDate <=" + ToDate + " AND ";
       
            if (txtCostCenterID.Text != string.Empty)
                filter = filter + " dbo.Acc_ReceiptVoucherMaster.CostCenterID  =" + Comon.cInt(txtCostCenterID.Text) + "  AND ";
            // '''''''''''''
            string ToAccount = (UserInfo.Language == iLanguage.Arabic ? "الى مذكورين" : "To those mentioned");
            filter = filter.Remove(filter.Length - 4, 4);
            strSQL = "SELECT  '11' as TypeOpration,(Case when Count(Acc_ReceiptVoucherDetails.ReceiptVoucherID)=1 then Max( Acc_Accounts2." + PrimaryName + " ) else  '" + ToAccount + "' end)as AccountIDCredit,dbo.Acc_Currency." + PrimaryName + " as CurrncyName,dbo.Acc_Accounts." + PrimaryName + " as AccountIDDibet,dbo.Users." + PrimaryName + " as UserName,Users2." + PrimaryName + " as UserNameUpdate, dbo.Acc_CostCenters.ArbName AS CostCenter ,dbo.Acc_ReceiptVoucherMaster.Cancel, Acc_ReceiptVoucherMaster.DocumentID,"
                + " (dbo.Acc_ReceiptVoucherMaster.DebitAmount) AS total, dbo.Acc_ReceiptVoucherMaster.ReceiptVoucherID AS InvoiceID,"
                + " dbo.Acc_ReceiptVoucherMaster.ReceiptVoucherDate AS InvoiceDate, dbo.Acc_ReceiptVoucherMaster.Notes  ,"
                + " dbo.Acc_ReceiptVoucherMaster.DocumentID FROM dbo.Users INNER JOIN dbo.Acc_ReceiptVoucherMaster "
                + "  ON dbo.Users.UserID = dbo.Acc_ReceiptVoucherMaster.UserID  INNER JOIN dbo.Acc_ReceiptVoucherDetails ON dbo.Acc_ReceiptVoucherMaster.ReceiptVoucherID"
                + " = dbo.Acc_ReceiptVoucherDetails.ReceiptVoucherID AND dbo.Acc_ReceiptVoucherMaster.BranchID = dbo.Acc_ReceiptVoucherDetails.BranchID LEFT OUTER JOIN "
                + " dbo.Acc_CostCenters ON dbo.Acc_ReceiptVoucherDetails.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Acc_ReceiptVoucherDetails.CostCenterID =dbo.Acc_CostCenters.CostCenterID "
                + "  LEFT OUTER JOIN Acc_Accounts on Acc_ReceiptVoucherMaster.DebitAccountID=Acc_Accounts.AccountID AND Acc_ReceiptVoucherMaster.BranchID=Acc_Accounts.BranchID "
                + "  LEFT OUTER JOIN Acc_Accounts AS Acc_Accounts2 ON Acc_ReceiptVoucherDetails.AccountID = Acc_Accounts2.AccountID and Acc_ReceiptVoucherDetails.BranchID = Acc_Accounts2.BranchID "
                + "  LEFT OUTER JOIN Users AS Users2 ON Acc_ReceiptVoucherMaster.EditUserID = Users2.UserID and Acc_ReceiptVoucherMaster.BranchID = Users2.BranchID "
                + "  LEFT OUTER JOIN Acc_Currency on Acc_ReceiptVoucherMaster.CurrencyID=Acc_Currency.ID  and Acc_ReceiptVoucherMaster.BranchID=Acc_Currency.BranchID Where" + filter
                + " Group by Acc_ReceiptVoucherDetails.ReceiptVoucherID,dbo.Acc_Accounts." + PrimaryName + ",dbo.Acc_Currency." + PrimaryName + " ,dbo.Users." + PrimaryName + ",Users2." + PrimaryName + ",dbo.Acc_CostCenters." + PrimaryName + ",dbo.Acc_ReceiptVoucherMaster.Cancel,dbo.Acc_ReceiptVoucherMaster.ReceiptVoucherID,Acc_ReceiptVoucherMaster.DocumentID,dbo.Acc_ReceiptVoucherMaster.DebitAmount,dbo.Acc_ReceiptVoucherMaster.Notes ,dbo.Acc_ReceiptVoucherMaster.ReceiptVoucherDate ";
            Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());
            return strSQL;


        }
        string GetStrSQLSpendVoucher()
        {

            //btnShow.Visible = false;
            Application.DoEvents();

            string filter = "(dbo.Acc_SpendVoucherMaster.BranchID = " +  MySession.GlobalBranchID + ") AND dbo.Acc_SpendVoucherMaster.SpendVoucherID >0    AND ";
            strSQL = "";
            if (Comon.cInt(cmbBranchesID.EditValue) > 0)
                filter = "(dbo.Acc_SpendVoucherMaster.BranchID = " + cmbBranchesID.EditValue + ") AND dbo.Acc_SpendVoucherMaster.SpendVoucherID >0     AND ";
           
            long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
            long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));

            DataTable dt;
            // حسب الرقم
            if (Comon.cInt(cmbStatus.EditValue) == 1)
                filter = filter + " dbo.Acc_SpendVoucherMaster.Cancel =1 AND ";

            if (txtFromTransactionID.Text != string.Empty)
                filter = filter + " dbo.Acc_SpendVoucherMaster.InvoiceID >=" + txtFromTransactionID.Text + " AND ";

            else if (Comon.cInt(cmbCurency.EditValue) > 0)
                filter = filter + " dbo.Acc_SpendVoucherMaster.CurrencyID=" + Comon.cInt(cmbCurency.EditValue) + " AND ";
            if (Comon.cInt(txtUserIDEntry.Text) > 0)
                filter = filter + " Acc_SpendVoucherMaster.UserID=" + Comon.cInt(txtUserIDEntry.Text) + " AND ";
            if (Comon.cInt(txtUserIDUpdated.Text) > 0)
                filter = filter + " Acc_SpendVoucherMaster.EditUserID=" + Comon.cInt(txtUserIDUpdated.Text) + " AND ";
            if (txtToTransactionsID.Text != string.Empty)
                filter = filter + " dbo.Acc_SpendVoucherMaster.InvoiceID <=" + txtToTransactionsID.Text + " AND ";
            if (txtReferanceID.Text.Trim() != string.Empty)
                filter = filter + "  Acc_SpendVoucherMaster.DocumentID=" + txtReferanceID.Text + " AND ";

            if (Comon.cDbl(txtBankID.Text) > 0)
                filter = filter + "  Acc_SpendVoucherMaster.CreditAccountID=" + Comon.cDbl(txtBankID.Text) + " AND ";
            if (Comon.cDbl(txtBoxID.Text) > 0)
                filter = filter + "  Acc_SpendVoucherDetails.AccountID=" + Comon.cDbl(txtBoxID.Text) + " AND ";
            // حسب التاريخ
            if (FromDate != 0)
                filter = filter + " dbo.Acc_SpendVoucherMaster.SpendVoucherDate >=" + FromDate + " AND ";
            if (ToDate != 0)
                filter = filter + " dbo.Acc_SpendVoucherMaster.SpendVoucherDate <=" + ToDate + " AND ";

            if (txtCostCenterID.Text != string.Empty)
                filter = filter + " dbo.Acc_SpendVoucherMaster.CostCenterID  =" + Comon.cInt(txtCostCenterID.Text) + "  AND ";
            // '''''''''''''
            string ToAccount = (UserInfo.Language == iLanguage.Arabic ? "الى مذكورين" : "To those mentioned");
            filter = filter.Remove(filter.Length - 4, 4);
            strSQL = "SELECT  '12' as TypeOpration,(Case when Count(Acc_SpendVoucherDetails.SpendVoucherID)=1 then Max(Acc_Accounts2." + PrimaryName + ")  else '" + ToAccount + "' end)as AccountIDCredit,dbo.Acc_Currency." + PrimaryName + " as CurrncyName,dbo.Acc_Accounts." + PrimaryName + " as AccountIDDibet,dbo.Users." + PrimaryName + " as UserName,Users2." + PrimaryName + " as UserNameUpdate, dbo.Acc_CostCenters.ArbName AS CostCenter ,dbo.Acc_SpendVoucherMaster.Cancel, Acc_SpendVoucherMaster.DocumentID,"
                + " (dbo.Acc_SpendVoucherMaster.CreditAmount) AS total, dbo.Acc_SpendVoucherMaster.SpendVoucherID AS InvoiceID,"
                + " dbo.Acc_SpendVoucherMaster.SpendVoucherDate AS InvoiceDate, dbo.Acc_SpendVoucherMaster.Notes  ,"
                + " dbo.Acc_SpendVoucherMaster.DocumentID FROM dbo.Users INNER JOIN dbo.Acc_SpendVoucherMaster "
                + "  ON dbo.Users.UserID = dbo.Acc_SpendVoucherMaster.UserID  INNER JOIN dbo.Acc_SpendVoucherDetails ON dbo.Acc_SpendVoucherMaster.SpendVoucherID"
                + " = dbo.Acc_SpendVoucherDetails.SpendVoucherID AND dbo.Acc_SpendVoucherMaster.BranchID = dbo.Acc_SpendVoucherDetails.BranchID LEFT OUTER JOIN "
                + " dbo.Acc_CostCenters ON dbo.Acc_SpendVoucherDetails.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Acc_SpendVoucherDetails.CostCenterID =dbo.Acc_CostCenters.CostCenterID "
                + "  LEFT OUTER JOIN Acc_Accounts on Acc_SpendVoucherMaster.CreditAccountID=Acc_Accounts.AccountID  and Acc_SpendVoucherMaster.BranchID=Acc_Accounts.BranchID "
                + "  LEFT OUTER JOIN Acc_Accounts AS Acc_Accounts2 ON Acc_SpendVoucherDetails.AccountID = Acc_Accounts2.AccountID and Acc_SpendVoucherDetails.BranchID = Acc_Accounts2.BranchID "
                + "  LEFT OUTER JOIN Users AS Users2 ON Acc_SpendVoucherMaster.EditUserID = Users2.UserID and Acc_SpendVoucherMaster.BranchID = Users2.BranchID "
                + "  LEFT OUTER JOIN Acc_Currency on Acc_SpendVoucherMaster.CurrencyID=Acc_Currency.ID and Acc_SpendVoucherMaster.BranchID=Acc_Currency.BranchID Where" + filter
                + " Group by Acc_SpendVoucherDetails.SpendVoucherID,dbo.Acc_Accounts." + PrimaryName + ",dbo.Acc_Currency." + PrimaryName + ",dbo.Users." + PrimaryName + ",Users2." + PrimaryName + ",dbo.Acc_CostCenters." + PrimaryName + ",dbo.Acc_SpendVoucherMaster.Cancel,dbo.Acc_SpendVoucherMaster.SpendVoucherID,Acc_SpendVoucherMaster.DocumentID,dbo.Acc_SpendVoucherMaster.CreditAmount,dbo.Acc_SpendVoucherMaster.Notes ,dbo.Acc_SpendVoucherMaster.SpendVoucherDate ";
            Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());
            return strSQL;


        }
        string GetStrSQLVariousVoucher()
        {

            
            Application.DoEvents();

            string filter = "(dbo.Acc_VariousVoucherMaster.BranchID = " +  MySession.GlobalBranchID + ") AND dbo.Acc_VariousVoucherMaster.VoucherID >0    AND ";
            strSQL = "";
            if (Comon.cInt(cmbBranchesID.EditValue) > 0)
                filter = "(dbo.Acc_VariousVoucherMaster.BranchID = " + cmbBranchesID.EditValue + ") AND dbo.Acc_VariousVoucherMaster.VoucherID >0     AND ";

            long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
            long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));

            DataTable dt;
            // حسب الرقم
            if (Comon.cInt(cmbStatus.EditValue) == 1)
                filter = filter + " dbo.Acc_VariousVoucherMaster.Cancel =1 AND ";

            if (txtFromTransactionID.Text != string.Empty)
                filter = filter + " dbo.Acc_VariousVoucherMaster.InvoiceID >=" + txtFromTransactionID.Text + " AND ";

            else if (Comon.cInt(cmbCurency.EditValue) > 0)
                filter = filter + " dbo.Acc_VariousVoucherMaster.CurrencyID=" + Comon.cInt(cmbCurency.EditValue) + " AND ";
            if (Comon.cInt(txtUserIDEntry.Text) > 0)
                filter = filter + " Acc_VariousVoucherMaster.UserID=" + Comon.cInt(txtUserIDEntry.Text) + " AND ";
            if (Comon.cInt(txtUserIDUpdated.Text) > 0)
                filter = filter + " Acc_VariousVoucherMaster.EditUserID=" + Comon.cInt(txtUserIDUpdated.Text) + " AND ";
            if (txtToTransactionsID.Text != string.Empty)
                filter = filter + " dbo.Acc_VariousVoucherMaster.InvoiceID <=" + txtToTransactionsID.Text + " AND ";
            if (txtReferanceID.Text.Trim() != string.Empty)
                filter = filter + "  Acc_VariousVoucherMaster.DocumentID=" + txtReferanceID.Text + " AND ";

            if (Comon.cDbl(txtBankID.Text) > 0)
                filter = filter + "  Acc_VariousVoucherDetails.AccountID=" + Comon.cDbl(txtBankID.Text) + " AND ";
            if (Comon.cDbl(txtBoxID.Text) > 0)
                filter = filter + "  Acc_VariousVoucherDetails.AccountID=" + Comon.cDbl(txtBoxID.Text) + " AND ";
            // حسب التاريخ
            if (FromDate != 0)
                filter = filter + " dbo.Acc_VariousVoucherMaster.VoucherDate >=" + FromDate + " AND ";
            if (ToDate != 0)
                filter = filter + " dbo.Acc_VariousVoucherMaster.VoucherDate <=" + ToDate + " AND ";

            if (txtCostCenterID.Text != string.Empty)
                filter = filter + " dbo.Acc_VariousVoucherMaster.CostCenterID  =" + Comon.cInt(txtCostCenterID.Text) + "  AND ";
            // '''''''''''''
            string ToAccount = (UserInfo.Language == iLanguage.Arabic ? "الى مذكورين" : "To those mentioned");
            string FromAccount = (UserInfo.Language == iLanguage.Arabic ? "من مذكورين" : "From those mentioned");
            filter = filter.Remove(filter.Length - 4, 4);
            strSQL = "SELECT  '13' as TypeOpration,(CASE WHEN (SUM(CASE WHEN Acc_VariousVoucherDetails.Credit > 0 THEN 1 ELSE 0 END) = 1) THEN Max(Acc_Accounts." + PrimaryName + ") else '" + ToAccount + "' end)as AccountIDCredit,dbo.Acc_Currency." + PrimaryName + " as CurrncyName,(CASE WHEN (SUM(CASE WHEN Acc_VariousVoucherDetails.Debit > 0 THEN 1 ELSE 0 END) = 1) THEN Max( Acc_Accounts2." + PrimaryName + ") else  '" + FromAccount + "' end) as AccountIDDibet,dbo.Users." + PrimaryName + " as UserName,Users2." + PrimaryName + " as UserNameUpdate, dbo.Acc_CostCenters.ArbName AS CostCenter ,dbo.Acc_VariousVoucherMaster.Cancel, Acc_VariousVoucherMaster.DocumentID,"
                + " Sum(dbo.Acc_VariousVoucherDetails.Debit) AS total, dbo.Acc_VariousVoucherMaster.VoucherID AS InvoiceID,"
                + " dbo.Acc_VariousVoucherMaster.VoucherDate AS InvoiceDate, dbo.Acc_VariousVoucherMaster.Notes  ,"
                + " dbo.Acc_VariousVoucherMaster.DocumentID FROM dbo.Users INNER JOIN dbo.Acc_VariousVoucherMaster "
                + "  ON dbo.Users.UserID = dbo.Acc_VariousVoucherMaster.UserID  INNER JOIN dbo.Acc_VariousVoucherDetails ON dbo.Acc_VariousVoucherMaster.VoucherID"
                + " = dbo.Acc_VariousVoucherDetails.VoucherID AND dbo.Acc_VariousVoucherMaster.BranchID = dbo.Acc_VariousVoucherDetails.BranchID LEFT OUTER JOIN "
                + " dbo.Acc_CostCenters ON dbo.Acc_VariousVoucherDetails.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Acc_VariousVoucherDetails.CostCenterID =dbo.Acc_CostCenters.CostCenterID "
                + "  LEFT OUTER JOIN Acc_Accounts on Acc_VariousVoucherDetails.AccountID=Acc_Accounts.AccountID and  Acc_VariousVoucherDetails.BranchID=Acc_Accounts.BranchID and Acc_VariousVoucherDetails.Credit>0 "
                + "  LEFT OUTER JOIN Acc_Accounts AS Acc_Accounts2 ON Acc_VariousVoucherDetails.AccountID = Acc_Accounts2.AccountID and Acc_VariousVoucherDetails.BranchID = Acc_Accounts2.BranchID and Acc_VariousVoucherDetails.Debit>0 "
                + "  LEFT OUTER JOIN Users AS Users2 ON Acc_VariousVoucherMaster.EditUserID = Users2.UserID  and Acc_VariousVoucherMaster.BranchID = Users2.BranchID "
                + "  LEFT OUTER JOIN Acc_Currency on Acc_VariousVoucherDetails.CurrencyID=Acc_Currency.ID and Acc_VariousVoucherDetails.BranchID=Acc_Currency.BranchID  Where" + filter
                + " Group by Acc_VariousVoucherDetails.VoucherID,dbo.Acc_Currency." + PrimaryName + " ,dbo.Users." + PrimaryName + ",Users2." + PrimaryName + ",dbo.Acc_CostCenters." + PrimaryName + ",dbo.Acc_VariousVoucherMaster.Cancel,dbo.Acc_VariousVoucherMaster.VoucherID,Acc_VariousVoucherMaster.DocumentID,dbo.Acc_VariousVoucherMaster.Notes ,dbo.Acc_VariousVoucherMaster.VoucherDate ";
            Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());
            return strSQL;


        }
        string GetStrSQLOpeningVoucher()
        {

            
            Application.DoEvents();

            string filter = "(dbo.Acc_VariousVoucherMaster.BranchID = " +  MySession.GlobalBranchID + ") AND ";
            strSQL = "";
            if (Comon.cInt(cmbBranchesID.EditValue) > 0)
                filter = "(dbo.Acc_VariousVoucherMaster.BranchID = " + cmbBranchesID.EditValue + ")  AND ";

            long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
            long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));

            DataTable dt;
            // حسب الرقم
            if (Comon.cInt(cmbStatus.EditValue) == 1)
                filter = filter + " dbo.Acc_VariousVoucherMaster.Cancel =1 AND ";

            if (txtFromTransactionID.Text != string.Empty)
                filter = filter + " dbo.Acc_VariousVoucherMaster.InvoiceID >=" + txtFromTransactionID.Text + " AND ";

            else if (Comon.cInt(cmbCurency.EditValue) > 0)
                filter = filter + " dbo.Acc_VariousVoucherMaster.CurrencyID=" + Comon.cInt(cmbCurency.EditValue) + " AND ";
            if (Comon.cInt(txtUserIDEntry.Text) > 0)
                filter = filter + " Acc_VariousVoucherMaster.UserID=" + Comon.cInt(txtUserIDEntry.Text) + " AND ";
            if (Comon.cInt(txtUserIDUpdated.Text) > 0)
                filter = filter + " Acc_VariousVoucherMaster.EditUserID=" + Comon.cInt(txtUserIDUpdated.Text) + " AND ";
            if (txtToTransactionsID.Text != string.Empty)
                filter = filter + " dbo.Acc_VariousVoucherMaster.InvoiceID <=" + txtToTransactionsID.Text + " AND ";
            if (txtReferanceID.Text.Trim() != string.Empty)
                filter = filter + "  Acc_VariousVoucherMaster.DocumentID=" + txtReferanceID.Text + " AND ";

            if (Comon.cDbl(txtBankID.Text) > 0)
                filter = filter + "  Acc_VariousVoucherDetails.AccountID=" + Comon.cDbl(txtBankID.Text) + " AND ";
            if (Comon.cDbl(txtBoxID.Text) > 0)
                filter = filter + "  Acc_VariousVoucherDetails.AccountID=" + Comon.cDbl(txtBoxID.Text) + " AND ";
            // حسب التاريخ
            if (FromDate != 0)
                filter = filter + " dbo.Acc_VariousVoucherMaster.VoucherDate >=" + FromDate + " AND ";
            if (ToDate != 0)
                filter = filter + " dbo.Acc_VariousVoucherMaster.VoucherDate <=" + ToDate + " AND ";

            if (txtCostCenterID.Text != string.Empty)
                filter = filter + " dbo.Acc_VariousVoucherMaster.CostCenterID  =" + Comon.cInt(txtCostCenterID.Text) + "  AND ";
            // '''''''''''''
            string ToAccount = (UserInfo.Language == iLanguage.Arabic ? "الى مذكورين" : "To those mentioned");
            string FromAccount = (UserInfo.Language == iLanguage.Arabic ? "من مذكورين" : "From those mentioned");
            filter = filter.Remove(filter.Length - 4, 4);
            strSQL = "SELECT  '14' as TypeOpration,(CASE WHEN (SUM(CASE WHEN Acc_VariousVoucherDetails.Credit > 0 THEN 1 ELSE 0 END) = 1) THEN Max(Acc_Accounts." + PrimaryName + ") else '" + ToAccount + "' end)as AccountIDCredit,dbo.Acc_Currency." + PrimaryName + " as CurrncyName,(CASE WHEN (SUM(CASE WHEN Acc_VariousVoucherDetails.Debit > 0 THEN 1 ELSE 0 END) = 1) THEN Max( Acc_Accounts2." + PrimaryName + ") else  '" + FromAccount + "' end) as AccountIDDibet,dbo.Users." + PrimaryName + " as UserName,Users2." + PrimaryName + " as UserNameUpdate, dbo.Acc_CostCenters.ArbName AS CostCenter ,dbo.Acc_VariousVoucherMaster.Cancel, Acc_VariousVoucherMaster.DocumentID,"
                + " Sum(dbo.Acc_VariousVoucherDetails.Debit) AS total, dbo.Acc_VariousVoucherMaster.VoucherID AS InvoiceID,"
                + " dbo.Acc_VariousVoucherMaster.VoucherDate AS InvoiceDate, dbo.Acc_VariousVoucherMaster.Notes  ,"
                + " dbo.Acc_VariousVoucherMaster.DocumentID FROM dbo.Users INNER JOIN dbo.Acc_VariousVoucherMaster "
                + "  ON dbo.Users.UserID = dbo.Acc_VariousVoucherMaster.UserID  INNER JOIN dbo.Acc_VariousVoucherDetails ON dbo.Acc_VariousVoucherMaster.VoucherID"
                + " = dbo.Acc_VariousVoucherDetails.VoucherID AND dbo.Acc_VariousVoucherMaster.BranchID = dbo.Acc_VariousVoucherDetails.BranchID LEFT OUTER JOIN "
                + " dbo.Acc_CostCenters ON dbo.Acc_VariousVoucherDetails.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Acc_VariousVoucherDetails.CostCenterID =dbo.Acc_CostCenters.CostCenterID "
                + "  LEFT OUTER JOIN Acc_Accounts on Acc_VariousVoucherDetails.AccountID=Acc_Accounts.AccountID  and  Acc_VariousVoucherDetails.BranchID=Acc_Accounts.BranchID and Acc_VariousVoucherDetails.Credit>0 "
                + "  LEFT OUTER JOIN Acc_Accounts AS Acc_Accounts2 ON Acc_VariousVoucherDetails.AccountID = Acc_Accounts2.AccountID and Acc_VariousVoucherDetails.BranchID = Acc_Accounts2.BranchID and Acc_VariousVoucherDetails.BranchID = Acc_Accounts2.BranchID and Acc_VariousVoucherDetails.Debit>0 "
                + "  LEFT OUTER JOIN Users AS Users2 ON Acc_VariousVoucherMaster.EditUserID = Users2.UserID and Acc_VariousVoucherMaster.BranchID = Users2.BranchID "
                + "  LEFT OUTER JOIN Acc_Currency on Acc_VariousVoucherDetails.CurrencyID=Acc_Currency.ID and Acc_VariousVoucherDetails.BranchID=Acc_Currency.BranchID  Where" + filter
                + " Group by Acc_VariousVoucherDetails.VoucherID,dbo.Acc_Currency." + PrimaryName + " ,dbo.Users." + PrimaryName + ",Users2." + PrimaryName + ",dbo.Acc_CostCenters." + PrimaryName + ",dbo.Acc_VariousVoucherMaster.Cancel,dbo.Acc_VariousVoucherMaster.VoucherID,Acc_VariousVoucherMaster.DocumentID,dbo.Acc_VariousVoucherMaster.Notes ,dbo.Acc_VariousVoucherMaster.VoucherDate ";
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

                if (fromType == 11 || (toType >= 11 && fromType <= 11) || (toType == fromType && fromType <= 0))
                {
                    dt = Lip.SelectRecord(GetStrSQLReceiptVoucher());
                }

                if ((toType == fromType && fromType <= 0) ||
                     ((fromType >= 11 && fromType <= 12 && toType >= 12 && (toType != fromType && toType >= 12)) ||
                     (fromType >= 11 && fromType <= 12 && toType <= 0) ||
                     (toType >= 12 && fromType <= 0)) ||
                     (toType == fromType && toType == 12))
                {
                    dt.Merge(Lip.SelectRecord(GetStrSQLSpendVoucher()));
                }


                if ((toType == fromType && fromType <= 0) ||
                     ((fromType >= 11 && toType >= 13 && (toType != fromType && toType >= 13)) ||
                     (fromType >= 11 && fromType <= 13 && toType <= 0) || (toType >= 13 && fromType <= 0)) ||
                     (toType == fromType && toType == 13))
                {
                    dt.Merge(Lip.SelectRecord(GetStrSQLVariousVoucher()));
                }

                if ((toType == fromType && fromType <= 0) ||
                    ((fromType >= 11 && toType >= 14 && (toType != fromType && toType >= 14)) ||
                    (fromType >= 11 && fromType <= 14 && toType <= 0) || (toType >= 14 && fromType <= 0)) ||
                    (toType == fromType && toType == 14))
                {
                    dt.Merge(Lip.SelectRecord(GetStrSQLOpeningVoucher()));
                }
                 
                //if ((toType == fromType && fromType <= 0) ||
                //   ((fromType >= 11 && toType >= 9 && (toType != fromType && toType >= 9)) ||
                //   (fromType >= 11 && fromType <= 9 && toType <= 0) || (toType >= 9 && fromType <= 0)) ||
                //   (toType == fromType && toType == 9))
                //{
                //    dt.Merge(Lip.SelectRecord(GetStrSQLMaltiGold()));
                //}
                
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
                                row["StatUs"] = UserInfo.Language == iLanguage.Arabic ? "محذوف" : "Deleted";
                            else
                                row["StatUs"] = UserInfo.Language == iLanguage.Arabic ? "مرحل " : "Aported";
                            if (Comon.cInt(dt.Rows[i]["TypeOpration"]) == 11)
                                row["RecordType"] = UserInfo.Language == iLanguage.Arabic ? "سند قبض " : "Recipt Vovchare";
                            else if (Comon.cInt(dt.Rows[i]["TypeOpration"]) == 12)
                                row["RecordType"] = UserInfo.Language == iLanguage.Arabic ? "سند صرف " : "Spend Voucher";
                            else if (Comon.cInt(dt.Rows[i]["TypeOpration"]) == 13)
                                row["RecordType"] = UserInfo.Language == iLanguage.Arabic ? "قيد يومي " : "Daily Voucher";
                            else if (Comon.cInt(dt.Rows[i]["TypeOpration"]) == 14)
                                row["RecordType"] = UserInfo.Language == iLanguage.Arabic ? "قيد افتتاحي " : "Opening Voucher";
                            row["TypeOpration"] = Comon.cInt(dt.Rows[i]["TypeOpration"]);
                            row["UserNameUpdate"] = (dt.Rows[i]["UserNameUpdate"].ToString() != string.Empty ? dt.Rows[i]["UserNameUpdate"] : "");
                            row["AccountIDDibet"] = (dt.Rows[i]["AccountIDDibet"].ToString() != string.Empty ? dt.Rows[i]["AccountIDDibet"] : "");
                            row["AccountIDCredit"] = dt.Rows[i]["AccountIDCredit"];
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
            //    btnShow.Visible = true;
                 
            //    txtCostCenterID.Enabled = false; 
            //    txtFromDate.Enabled = false;
            //    txtToDate.Enabled = false;
            //    txtToTransactionsID.Enabled = false;
            //    txtFromTransactionID.Enabled = false;

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
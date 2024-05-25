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
using Edex.GeneralObjects.GeneralClasses;
using Edex.ModelSystem;
using Edex.Model;
using Edex.Model.Language;
using DevExpress.XtraSplashScreen;

namespace Edex.AccountsObjects.Transactions
{
    public partial class frmTransferAccountTransactions : BaseForm
    {
        private string PrimaryName;
        private string strSQL = "";
        private string FocusedControl = "";
         
        
        public frmTransferAccountTransactions()
        {
            InitializeComponent();
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = false;
            //ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            //ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[9].Visible = false;
            //ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = true;
            //ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Caption = (UserInfo.Language == iLanguage.Arabic ? "استعلام جديد" : "New Query");
            PrimaryName = "ArbName";
            if (UserInfo.Language == iLanguage.English)
            {
                PrimaryName = "EngName";
            }
            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
            FillCombo.FillComboBox(cmbCurency, "Acc_Currency", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد العملة "));
            FillCombo.FillComboBoxLookUpEdit(cmbFromType, "Acc_TypeTransctionToMove", "ID", PrimaryName, "", "ID>=1", (UserInfo.Language == iLanguage.English ? "Select Type" : "حدد النوع "));
            
            cmbBranchesID.EditValue = MySession.GlobalBranchID;
            this.txtFromAccountID.Validating+=txtFromAccountID_Validating;
            this.txtToAccountID.Validating+=txtToAccountID_Validating;
            this.KeyDown+=frmTransferAccountTransactions_KeyDown;
            InitializeFormatDate(txtFromDate);
            InitializeFormatDate(txtToDate);
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
        protected override void DoNew()
        {
            try
            {
               ClearFields();
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public void ClearFields()
        {
            try
            {
                txtFromAccountID.Text = "";
                txtFromAccountID_Validating(null, null);
                txtToAccountID.Text = "";
                txtToAccountID_Validating(null, null);
                txtTransID.Text = "";
                txtNotes.Text = "";
                txtFromDate.Text = "";
                txtToDate.Text = "";

               
                cmbCurency.EditValue = MySession.GlobalDefaultSaleCurencyID;
                 

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void frmTransferAccountTransactions_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                Find();
        }
        public void Find()
        {
            try
            {
                CSearch cls = new CSearch();
                int[] ColumnWidth = new int[] { 100, 300 };
                string SearchSql = "";
                string Condition = "Where 1=1";
                FocusedControl = GetIndexFocusedControl();
                if (FocusedControl == null) return;
                if (FocusedControl.Trim() ==txtFromAccountID.Name)
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, txtFromAccountID,lblFromAccountID, "AccountID", "رقم الحساب", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, txtFromAccountID, lblFromAccountID, "AccountID", "Account ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                if (FocusedControl.Trim() == txtToAccountID.Name)
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, txtToAccountID,lblToAccountID, "AccountID", "رقم الحساب", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, txtToAccountID, lblToAccountID, "AccountID", "Account ID", Comon.cInt(cmbBranchesID.EditValue));
                }
                GetSelectedSearchValue(cls);
            }
            catch { }
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
        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {

                if (FocusedControl == txtToAccountID.Name)
                {
                    txtToAccountID.Text = cls.PrimaryKeyValue.ToString();
                    txtToAccountID_Validating(null, null);
                }

                if (FocusedControl ==txtFromAccountID.Name)
                {
                    txtFromAccountID.Text = cls.PrimaryKeyValue.ToString();
                    txtFromAccountID_Validating(null, null);
                }
            }
        }
        private void txtFromAccountID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (Comon.cInt(cmbBranchesID.EditValue) == 1)
                    strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE     (Cancel = 0) AND (AccountID = " + Comon.cDbl(txtFromAccountID.Text) + ") ";
                else
                    strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE (BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + " ) AND " + " (Cancel = 0) AND (AccountID = " + Comon.cDbl(txtFromAccountID.Text) + ") ";
                CSearch.ControlValidating(txtFromAccountID, lblFromAccountID, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void txtToAccountID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (Comon.cInt(cmbBranchesID.EditValue) == 1)
                    strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE (Cancel = 0) AND (AccountID = " + Comon.cDbl(txtToAccountID.Text) + ") ";
                else
                    strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE (BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + " ) AND " + " (Cancel = 0) AND (AccountID = " + Comon.cDbl(txtToAccountID.Text) + ") ";
                CSearch.ControlValidating(txtToAccountID, lblToAccountID, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void btnFromAcountID_Click(object sender, EventArgs e)
        {
            try
            {
                PrepareSearchQuery.SearchForAccounts(txtFromAccountID, lblFromAccountID);
                txtFromAccountID_Validating(null, null);
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        private void btnToAcountID_Click(object sender, EventArgs e)
        {
            try
            {
                PrepareSearchQuery.SearchForAccounts(txtToAccountID, lblToAccountID);
                txtToAccountID_Validating(null, null);
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        void UpdateAccountIDToOnther(string TabelName, string AccountName, string FromAccountValue, string ToAccountValue, string InvoiceIDColName, long InvoiceID, int DocumentType, string Condion = "", string ColDateName = "InvoiceDate")
        {
            string filter = " " + AccountName + "=" + FromAccountValue + " and ";
            if (Comon.cInt(InvoiceID) > 0)
                filter = filter + " And  " + InvoiceIDColName + " =" + InvoiceID + " and ";
            long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
            long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));

            if (FromDate != 0 && ColDateName != "")
                filter = filter + ColDateName + ">=" + FromDate + " AND ";
            if (ToDate != 0 && ColDateName != "")
                filter = filter + ColDateName + "<=" + ToDate + " AND ";

            filter = filter.Remove(filter.Length - 4, 4);
            Lip.NewFields();
            Lip.Table = TabelName;
            Lip.AddNumericField(AccountName, ToAccountValue);
            Lip.sCondition = filter + Condion;
            Lip.ExecuteUpdate();


            long VoucherID = 0;
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + InvoiceID + " And DocumentType=" + DocumentType + " And BranchID=" + Comon.cInt(cmbBranchesID.EditValue)));

            Lip.NewFields();
            Lip.Table = "Acc_VariousVoucherMachinDetails";
            Lip.AddNumericField("AccountID", ToAccountValue);

            DataTable dtVoucherID;
            filter = " Cancel=0 and DocumentType=" + DocumentType + " and ";
            if (FromDate != 0 && ColDateName != "")
                filter = filter + " VoucherDate>=" + FromDate + " AND ";
            if (ToDate != 0 && ColDateName != "")
                filter = filter + " VoucherDate<=" + ToDate + " AND ";
            if (InvoiceID > 0)
                filter = filter + " VoucherID=" + VoucherID + " AND ";
            filter = filter.Remove(filter.Length - 4, 4);
            dtVoucherID = Lip.SelectRecord("select VoucherID  from Acc_VariousVoucherMachinMaster Where " + filter);

            string filterMachinR = "   AccountID=" + FromAccountValue + " and ";
            if (InvoiceID > 0)
                filterMachinR = filterMachinR + "    VoucherID in (" + string.Join(",", dtVoucherID.AsEnumerable().Select(row => row["VoucherID"].ToString())) + ") and ";
            else
                filterMachinR = filterMachinR + "   DocumentType=" + DocumentType + " and ";
            filterMachinR = filterMachinR.Remove(filterMachinR.Length - 4, 4);
            Lip.sCondition = filterMachinR;
            Lip.ExecuteUpdate();


            if (radioAll.Checked || radioDebit.Checked)
            {
                string fillter = " Cancel=0 and ";

                if (FromDate != 0 && ColDateName != "")
                    fillter = fillter + " MoveDate>=" + FromDate + " AND ";
                if (ToDate != 0 && ColDateName != "")
                    fillter = fillter + " MoveDate<=" + ToDate + " AND ";
                if (InvoiceID > 0)
                    fillter = "  TranseID=" + InvoiceID + " and ";
                fillter = fillter.Remove(fillter.Length - 4, 4);

                Lip.NewFields();
                Lip.Table = "Stc_ItemsMoviing";
                Lip.AddNumericField("StoreID", ToAccountValue);
                Lip.sCondition =fillter+ " and MoveType=1  and StoreID="+FromAccountValue+"  and DocumentTypeID=" + DocumentType ;
                Lip.ExecuteUpdate();

                Lip.NewFields();
                Lip.Table = "Stc_ItemsMoviing";
                Lip.AddNumericField("AccountID", ToAccountValue);
                Lip.sCondition = fillter+" and MoveType=2  and AccountID=" + FromAccountValue + "  and DocumentTypeID=" + DocumentType;
                Lip.ExecuteUpdate();

            }
            if (radioAll.Checked || radioCredit.Checked)
            {
                string fillter = " Cancel=0 and ";

                if (FromDate != 0 && ColDateName != "")
                    fillter = fillter + " MoveDate>=" + FromDate + " AND ";
                if (ToDate != 0 && ColDateName != "")
                    fillter = fillter + " MoveDate<=" + ToDate + " AND ";

                if (InvoiceID > 0)
                    fillter = "  TranseID=" + InvoiceID + " and ";
                fillter = fillter.Remove(fillter.Length - 4, 4);

                Lip.NewFields();
                Lip.Table = "Stc_ItemsMoviing";
                Lip.AddNumericField("StoreID", ToAccountValue);
                Lip.sCondition =fillter+ " and MoveType=2 and  StoreID=" + FromAccountValue + "  and DocumentTypeID=" + DocumentType ;
                Lip.ExecuteUpdate();

                Lip.NewFields();
                Lip.Table = "Stc_ItemsMoviing";
                Lip.AddNumericField("AccountID", ToAccountValue);
                Lip.sCondition =fillter+ " and MoveType=1 and   AccountID=" + FromAccountValue + "  and DocumentTypeID=" + DocumentType ;
                Lip.ExecuteUpdate();
               
            }

        }
        private void btnMoveItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Validations.IsValidForm(this))
                    return;
                DataTable dtFromAcc = Lip.SelectRecord("SELECT  AccountLevel, [EndType]  FROM  [Acc_Accounts] where [AccountID]=" + txtFromAccountID.Text + " and Cancel=0  ");
                 DataTable dtToAcc = Lip.SelectRecord("SELECT  AccountLevel, [EndType]  FROM  [Acc_Accounts] where [AccountID]=" +txtToAccountID.Text + " and Cancel=0  ");
                if(Comon.cInt( dtFromAcc.Rows[0]["AccountLevel"])!=Comon.cInt( dtToAcc.Rows[0]["AccountLevel"]))
                {
                    Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "لا يمكن النقل .. لان الحسابين ليس من نفس المستوى " : "It is not possible to transfer... because the two accounts are not of the same level.");
                    return;
                }
                 if (Comon.cInt(dtFromAcc.Rows[0]["EndType"]) != Comon.cInt(dtToAcc.Rows[0]["EndType"]))
                {
                    Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "لا يمكن النقل .. لان الحسابين لا ينتميان الى  نفس الفئة  " : "It is not possible to transfer... because the two accounts do not belong to the same category.");
                    return;
                }
                 Application.DoEvents();
                 SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                 if (Comon.cInt(cmbFromType.EditValue) == -1 || Comon.cInt(cmbFromType.EditValue)==1)
                 {
                     if (radioAll.Checked || radioDebit.Checked)
                     {
                         UpdateAccountIDToOnther("Sales_PurchaseInvoiceMaster", "DebitAccount", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "InvoiceID", Comon.cLong(txtTransID.Text), 23);
                         UpdateAccountIDToOnther("Sales_PurchaseInvoiceMaster", "StoreID", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "InvoiceID", Comon.cLong(txtTransID.Text), 23);
                         UpdateAccountIDToOnther("Sales_PurchaseInvoiceMaster", "AdditionalAccount", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "InvoiceID", Comon.cLong(txtTransID.Text), 23);
                         UpdateAccountIDToOnther("Sales_PurchaseInvoiceMaster", "TransportDebitAccount", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "InvoiceID", Comon.cLong(txtTransID.Text), 23);
                     }
                     if (radioAll.Checked || radioCredit.Checked)
                     {
                         UpdateAccountIDToOnther("Sales_PurchaseInvoiceMaster", "CreditAccount", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "InvoiceID", Comon.cLong(txtTransID.Text), 23);
                         UpdateAccountIDToOnther("Sales_PurchaseInvoiceMaster", "DiscountCreditAccount", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "InvoiceID", Comon.cLong(txtTransID.Text), 23);
                         UpdateAccountIDToOnther("Sales_PurchaseInvoiceMaster", "SupplierID", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "InvoiceID", Comon.cLong(txtTransID.Text), 23);                       
                     }                        
                  }

                 if (Comon.cInt(cmbFromType.EditValue) == -1 || Comon.cInt(cmbFromType.EditValue) == 2)
                 {
                     if (radioAll.Checked || radioDebit.Checked)
                     {
                         UpdateAccountIDToOnther("Sales_PurchaseInvoiceReturnMaster", "SupplierID", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "InvoiceID", Comon.cLong(txtTransID.Text), 24);
                         UpdateAccountIDToOnther("Sales_PurchaseInvoiceReturnMaster", "DebitAccount", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "InvoiceID", Comon.cLong(txtTransID.Text), 24);
                         UpdateAccountIDToOnther("Sales_PurchaseInvoiceReturnMaster", "DiscountDebitAccount", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "InvoiceID", Comon.cLong(txtTransID.Text), 24);                         
                     }
                     if (radioAll.Checked || radioCredit.Checked)
                     {
                         UpdateAccountIDToOnther("Sales_PurchaseInvoiceReturnMaster", "StoreID", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "InvoiceID", Comon.cLong(txtTransID.Text), 24);
                         UpdateAccountIDToOnther("Sales_PurchaseInvoiceReturnMaster", "CreditAccount", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "InvoiceID", Comon.cLong(txtTransID.Text), 24);
                         UpdateAccountIDToOnther("Sales_PurchaseInvoiceReturnMaster", "AdditionalAccount", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "InvoiceID", Comon.cLong(txtTransID.Text), 24);
                     }
                 }
                 if (Comon.cInt(cmbFromType.EditValue) == -1 || Comon.cInt(cmbFromType.EditValue) == 5)
                 {
                     if (radioAll.Checked || radioDebit.Checked)
                        UpdateAccountIDToOnther("Stc_GoldInonBail_Master", "StoreID", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "InvoiceID", Comon.cLong(txtTransID.Text), 14);
                     if (radioAll.Checked || radioCredit.Checked)
                        UpdateAccountIDToOnther("Stc_GoldInonBail_Master", "CreditAccount", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "InvoiceID", Comon.cLong(txtTransID.Text), 14);
                       
                 }

                 if (Comon.cInt(cmbFromType.EditValue) == -1 || Comon.cInt(cmbFromType.EditValue) == 6)
                 {
                     if (radioAll.Checked || radioDebit.Checked)
                        UpdateAccountIDToOnther("Stc_GoldOutOnBail_Master", "CreditAccount", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "InvoiceID", Comon.cLong(txtTransID.Text), 16);
                     if (radioAll.Checked || radioCredit.Checked)
                        UpdateAccountIDToOnther("Stc_GoldOutOnBail_Master", "StoreID", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "InvoiceID", Comon.cLong(txtTransID.Text), 16);
                      
                 }
                 if (Comon.cInt(cmbFromType.EditValue) == -1 || Comon.cInt(cmbFromType.EditValue) == 7)
                 {
                     if (radioAll.Checked || radioDebit.Checked)
                        UpdateAccountIDToOnther("Stc_MatirialInonBail_Master", "StoreID", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "InvoiceID", Comon.cLong(txtTransID.Text), 17);
                     if (radioAll.Checked || radioCredit.Checked)
                       UpdateAccountIDToOnther("Stc_MatirialInonBail_Master", "CreditAccount", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "InvoiceID", Comon.cLong(txtTransID.Text), 17);
                      
                 }
                 if (Comon.cInt(cmbFromType.EditValue) == -1 || Comon.cInt(cmbFromType.EditValue) == 8)
                 {
                     if (radioAll.Checked || radioDebit.Checked)
                        UpdateAccountIDToOnther("Stc_MatirialOutOnBail_Master", "CreditAccount", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "InvoiceID", Comon.cLong(txtTransID.Text), 18);
                     if (radioAll.Checked || radioCredit.Checked)
                       UpdateAccountIDToOnther("Stc_MatirialOutOnBail_Master", "StoreID", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "InvoiceID", Comon.cLong(txtTransID.Text), 18);
                      
                 }

                 if (Comon.cInt(cmbFromType.EditValue) == -1 || Comon.cInt(cmbFromType.EditValue) == 9)
                 {
                     if (radioAll.Checked || radioDebit.Checked)
                         UpdateAccountIDToOnther("Stc_TransferMultipleStoresGold_Details", "StoreAccountID", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "InvoiceID", Comon.cLong(txtTransID.Text), 19,ColDateName:"");
                     if (radioAll.Checked || radioCredit.Checked)
                         UpdateAccountIDToOnther("Stc_TransferMultipleStoresGold_Master", "StoreID", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "InvoiceID", Comon.cLong(txtTransID.Text), 19);
                 }

                 if (Comon.cInt(cmbFromType.EditValue) == -1 || Comon.cInt(cmbFromType.EditValue) == 10)
                 {
                     if (radioAll.Checked || radioDebit.Checked)
                         UpdateAccountIDToOnther("Stc_TransferMultipleStoresMatirial_Details", "StoreAccountID", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "InvoiceID", Comon.cLong(txtTransID.Text), 20,ColDateName:"");
                     if (radioAll.Checked || radioCredit.Checked)
                         UpdateAccountIDToOnther("Stc_TransferMultipleStoresMatirial_Master", "StoreID", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "InvoiceID", Comon.cLong(txtTransID.Text), 20);
                 }

                 if (Comon.cInt(cmbFromType.EditValue) == -1 || Comon.cInt(cmbFromType.EditValue) == 11)
                 {
                     if (radioAll.Checked || radioDebit.Checked)
                         UpdateAccountIDToOnther("Acc_ReceiptVoucherMaster", "DebitAccountID", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ReceiptVoucherID", Comon.cLong(txtTransID.Text), 3,ColDateName:"ReceiptVoucherDate");
                     if (radioAll.Checked || radioCredit.Checked)
                         UpdateAccountIDToOnther("Acc_ReceiptVoucherDetails", "AccountID", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ReceiptVoucherID", Comon.cLong(txtTransID.Text), 3,ColDateName:"");
                 }

                 if (Comon.cInt(cmbFromType.EditValue) == -1 || Comon.cInt(cmbFromType.EditValue) == 12)
                 {
                     if (radioAll.Checked || radioDebit.Checked)
                         UpdateAccountIDToOnther("Acc_SpendVoucherDetails", "AccountID", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "SpendVoucherID", Comon.cLong(txtTransID.Text), 2,ColDateName:"");
                     if (radioAll.Checked || radioCredit.Checked)
                         UpdateAccountIDToOnther("Acc_SpendVoucherMaster", "CreditAccountID", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "SpendVoucherID", Comon.cLong(txtTransID.Text), 2,ColDateName:"SpendVoucherDate");
                 }

                 if (Comon.cInt(cmbFromType.EditValue) == -1 || Comon.cInt(cmbFromType.EditValue) == 13)
                 {
                     if (radioAll.Checked || radioDebit.Checked)
                         UpdateAccountIDToOnther("Acc_VariousVoucherDetails", "AccountID", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "VoucherID", Comon.cLong(txtTransID.Text), 1," and Debit>0","");
                     if (radioAll.Checked || radioCredit.Checked)
                         UpdateAccountIDToOnther("Acc_VariousVoucherDetails", "AccountID", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "VoucherID", Comon.cLong(txtTransID.Text), 1," and Credit>0","");
                 }
                 if (Comon.cInt(cmbFromType.EditValue) == -1 || Comon.cInt(cmbFromType.EditValue) == 14)
                 {
                     if (radioAll.Checked || radioDebit.Checked)
                         UpdateAccountIDToOnther("Acc_VariousVoucherDetails", "AccountID", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "VoucherID", Comon.cLong(txtTransID.Text), 0, " and Debit>0","");
                     if (radioAll.Checked || radioCredit.Checked)
                         UpdateAccountIDToOnther("Acc_VariousVoucherDetails", "AccountID", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "VoucherID", Comon.cLong(txtTransID.Text), 0, " and Credit>0","");
                 }
                 if (Comon.cInt(cmbFromType.EditValue) == -1 || Comon.cInt(cmbFromType.EditValue) == 15)
                 {
                     if (radioAll.Checked || radioDebit.Checked)
                     {
                         UpdateAccountIDToOnther("Manu_CadWaxFactoryMaster", "StoreIDAfter", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "CommandID", Comon.cLong(txtTransID.Text), 26, " and TypeStageID=2", "");
                     }
                     if (radioAll.Checked || radioCredit.Checked)
                     {
                         UpdateAccountIDToOnther("Manu_CadWaxFactoryMaster", "StoreIDBefore", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "CommandID", Comon.cLong(txtTransID.Text), 26, " and TypeStageID=2", "");

                     }
                 }
                 if (Comon.cInt(cmbFromType.EditValue) == -1 || Comon.cInt(cmbFromType.EditValue) == 16)
                 {
                     if (radioAll.Checked || radioDebit.Checked)
                     {
                         UpdateAccountIDToOnther("Manu_CadWaxFactoryMaster", "StoreIDAfter", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "CommandID", Comon.cLong(txtTransID.Text), 25, " and [TypeStageID]=1", "");
                     }
                     if (radioAll.Checked || radioCredit.Checked)
                     {
                         UpdateAccountIDToOnther("Manu_CadWaxFactoryMaster", "StoreIDBefore", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "CommandID", Comon.cLong(txtTransID.Text), 25, " and [TypeStageID]=1", "");

                     }
                 }
                 if (Comon.cInt(cmbFromType.EditValue) == -1 || Comon.cInt(cmbFromType.EditValue) == 17)
                 {
                     if (radioAll.Checked || radioDebit.Checked)
                     {
                         UpdateAccountIDToOnther("Manu_ZirconDiamondFactoryMaster", "StoreIDAfter", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "CommandID", Comon.cLong(txtTransID.Text), 28, " and [TypeStageID]=4", "");
                     }
                     if (radioAll.Checked || radioCredit.Checked)
                     {
                         UpdateAccountIDToOnther("Manu_ZirconDiamondFactoryMaster", "StoreIDBefore", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "CommandID", Comon.cLong(txtTransID.Text), 28, " and [TypeStageID]=4", "");

                     }
                 }
                 if (Comon.cInt(cmbFromType.EditValue) == -1 || Comon.cInt(cmbFromType.EditValue) == 18)
                 {
                     if (radioAll.Checked || radioDebit.Checked)
                     {
                         UpdateAccountIDToOnther("Manu_ZirconDiamondFactoryMaster", "StoreIDAfter", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "CommandID", Comon.cLong(txtTransID.Text),27, " and [TypeStageID]=3", "");
                     }
                     if (radioAll.Checked || radioCredit.Checked)
                     {
                         UpdateAccountIDToOnther("Manu_ZirconDiamondFactoryMaster", "StoreIDBefore", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "CommandID", Comon.cLong(txtTransID.Text), 27, " and [TypeStageID]=3", "");

                     }
                 }
                 if (Comon.cInt(cmbFromType.EditValue) == -1 || Comon.cInt(cmbFromType.EditValue) == 19)
                 {
                     if (radioAll.Checked || radioDebit.Checked)
                       UpdateAccountIDToOnther("Manu_AfforestationFactoryMaster", "AfterAccountID", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "CommandID", Comon.cLong(txtTransID.Text), 29, " and [TypeStageID]=5", "");
                      
                     if (radioAll.Checked || radioCredit.Checked)
                       UpdateAccountIDToOnther("Manu_AfforestationFactoryMaster", "StoreIDBefore", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "CommandID", Comon.cLong(txtTransID.Text),29, " and [TypeStageID]=5", "");
                 }
                 if (Comon.cInt(cmbFromType.EditValue) == -1 || Comon.cInt(cmbFromType.EditValue) == 20)
                 {
                     UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "AccountIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 32, " and [TypeStageID]=6", "");
                     UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "StoreIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 32, " and [TypeStageID]=6", "");
                     UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "AccountIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 33, " and [TypeStageID]=6", "");
                     UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "StoreIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 33, " and [TypeStageID]=6", "");
                
                 }
                 if (Comon.cInt(cmbFromType.EditValue) == -1 || Comon.cInt(cmbFromType.EditValue) == 21)
                 {
                      UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "AccountIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 38, " and [TypeStageID]=9", "");
                       UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "StoreIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 38, " and [TypeStageID]=9", "");
                       UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "AccountIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 39, " and [TypeStageID]=9", "");
                       UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "StoreIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 39, " and [TypeStageID]=9", "");
                
                 }

                 if (Comon.cInt(cmbFromType.EditValue) == -1 || Comon.cInt(cmbFromType.EditValue) == 22)
                 {
                    UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "AccountIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 40, " and [TypeStageID]=8", "");
                    UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "StoreIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 40, " and [TypeStageID]=8", "");
                    UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "AccountIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 41, " and [TypeStageID]=8", "");
                    UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "StoreIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 41, " and [TypeStageID]=8", ""); 
                 }
                 if (Comon.cInt(cmbFromType.EditValue) == -1 || Comon.cInt(cmbFromType.EditValue) == 23)
                 {
                     UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "AccountIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 42, " and [TypeStageID]=13", "");
                     UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "StoreIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 42, " and [TypeStageID]=13", "");
                     UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "AccountIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 43, " and [TypeStageID]=13", "");
                     UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "StoreIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 43, " and [TypeStageID]=13", "");
                 }
                 if (Comon.cInt(cmbFromType.EditValue) == -1 || Comon.cInt(cmbFromType.EditValue) == 24)
                 {
                     UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "AccountIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 50, " and [TypeStageID]=14", "");
                     UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "StoreIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 50, " and [TypeStageID]=14", "");
                     UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "AccountIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 51, " and [TypeStageID]=14", "");
                     UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "StoreIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text),51, " and [TypeStageID]=14", "");
                 }
                 if (Comon.cInt(cmbFromType.EditValue) == -1 || Comon.cInt(cmbFromType.EditValue) == 25)
                 {
                     UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "AccountIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 34, " and [TypeStageID]=7", "");
                     UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "StoreIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 34, " and [TypeStageID]=7", "");
                     UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "AccountIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 35, " and [TypeStageID]=7", "");
                     UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "StoreIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 35, " and [TypeStageID]=7", "");
                 }
                 if (Comon.cInt(cmbFromType.EditValue) == -1 || Comon.cInt(cmbFromType.EditValue) == 26)
                 {
                     UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "AccountIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 36, " and [TypeStageID]=12", "");
                     UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "StoreIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 36, " and [TypeStageID]=12", "");
                     UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "AccountIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 37, " and [TypeStageID]=12", "");
                     UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "StoreIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 37, " and [TypeStageID]=12", "");
                 }
                 if (Comon.cInt(cmbFromType.EditValue) == -1 || Comon.cInt(cmbFromType.EditValue) == 27)
                 {
                     UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "AccountIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 44, " and [TypeStageID]=11", "");
                     UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "StoreIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 44, " and [TypeStageID]=11", "");
                     UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "AccountIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 45, " and [TypeStageID]=11", "");
                     UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "StoreIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 45, " and [TypeStageID]=11", "");
                 }
                 if (Comon.cInt(cmbFromType.EditValue) == -1 || Comon.cInt(cmbFromType.EditValue) == 28)
                 {
                     UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "AccountIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 46, " and [TypeStageID]=10", "");
                     UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "StoreIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 46, " and [TypeStageID]=10", "");
                     UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "AccountIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 47, " and [TypeStageID]=10", "");
                     UpdateAccountIDToOnther("Menu_FactoryRunCommandMaster", "StoreIDFactory", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 47, " and [TypeStageID]=10", "");
                 }
                 if (Comon.cInt(cmbFromType.EditValue) == -1 || Comon.cInt(cmbFromType.EditValue) == 29)
                 {
                     if (radioAll.Checked || radioDebit.Checked)
                         UpdateAccountIDToOnther("Menu_ProductionExpensesMaster", "DebitAccountID", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 48, " ", "");
                     if (radioAll.Checked || radioCredit.Checked)
                         UpdateAccountIDToOnther("Menu_ProductionExpensesMaster", "CreditAccountID", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "ComandID", Comon.cLong(txtTransID.Text), 48, " ", "");
                 }
                 if (Comon.cInt(cmbFromType.EditValue) == -1 || Comon.cInt(cmbFromType.EditValue) == 30)
                 {
                     if (radioAll.Checked || radioDebit.Checked)
                         UpdateAccountIDToOnther("Manu_CloseOrdersMaster", "AfterStoreID", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "CommandID", Comon.cLong(txtTransID.Text), 52, "  ", "");

                     if (radioAll.Checked || radioCredit.Checked)
                         UpdateAccountIDToOnther("Manu_CloseOrdersMaster", "BeforeStoreID", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "CommandID", Comon.cLong(txtTransID.Text), 52, "  ", "");
                 }
                 if (Comon.cInt(cmbFromType.EditValue) == -1 || Comon.cInt(cmbFromType.EditValue) == 31)
                 {
                     if (radioAll.Checked || radioDebit.Checked)
                         UpdateAccountIDToOnther("Stc_GoodOpeningMaster", "DebitAccount", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "InvoiceID", Comon.cLong(txtTransID.Text),15, "  ", "");

                     if (radioAll.Checked || radioCredit.Checked)
                         UpdateAccountIDToOnther("Stc_GoodOpeningMaster", "CreditAccount", txtFromAccountID.Text.ToString(), txtToAccountID.Text.ToString(), "InvoiceID", Comon.cLong(txtTransID.Text), 15, "  ", "");
                 }

                 //Save the process
                 Lip.NewFields();
                 Lip.Table = "Acc_TransferTransactionAccounts";
                 Lip.AddNumericField("FromAccountID",txtFromAccountID.Text.ToString());
                 Lip.AddNumericField("ToAccountID",txtToAccountID.Text.ToString());
                 Lip.AddStringField(PrimaryName,lblFromAccountID.Text.ToString());
                 Lip.AddNumericField("UserID", UserInfo.ID.ToString());
                 Lip.AddNumericField("RegDate", Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate())).ToString());
                 Lip.AddNumericField("RegTime", Comon.cDbl(Lip.GetServerTimeSerial()).ToString());
                 Lip.AddNumericField("EditUserID", UserInfo.ID);
                 Lip.AddNumericField("EditDate", Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate())).ToString());
                 Lip.AddNumericField("EditTime", Comon.cDbl(Lip.GetServerTimeSerial()).ToString());
                 Lip.AddStringField("ComputerInfo", UserInfo.ComputerInfo.ToString());
                 Lip.AddStringField("EditComputerInfo", UserInfo.ComputerInfo.ToString());
                 Lip.AddStringField("Reasons", txtNotes.Text.ToString());
                 Lip.AddStringField("TypeOprationID",Comon.cInt(cmbFromType.EditValue).ToString());
                 Lip.AddStringField("NatureTransportationID",radioAll.Checked?"1":radioCredit.Checked?"2":radioDebit.Checked?"3":"0");
                 Lip.AddNumericField("Cancel", 0);
                 Lip.ExecuteInsert();
                 SplashScreenManager.CloseForm(false);
                 Messages.MsgInfo(Messages.TitleInfo, UserInfo.Language == iLanguage.Arabic ? "تم نقل حركة الحساب  بنجاح" : "The Account movement was transferred successfully");
                        
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
    }
}
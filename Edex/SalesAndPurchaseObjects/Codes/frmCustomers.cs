using DevExpress.XtraEditors;
using Edex.DAL;
using Edex.Model;
using Edex.GeneralObjects.GeneralClasses;
using Edex.GeneralObjects.GeneralForms;
using Edex.ModelSystem;
using Edex.StockObjects.StoresClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Edex.Model.Language;
using Edex.DAL.Stc_itemDAL;
using DevExpress.XtraReports.UI;
using Edex.Reports;
using Edex.SalesAndPurchaseObjects.SalesClasses;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.Utils;
using System.Globalization;
namespace Edex.SalesAndPurchaseObjects.Codes
{
    public partial class frmCustomers : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        /// <summary>
        /// this is Constractor
        /// </summary>
        public frmCustomers()
        {
            InitializeComponent();
            PrimaryName = "ArbName";
            if (UserInfo.Language == iLanguage.English)
                PrimaryName = "EngName";

            /***************************Edit & Print & Export ****************************/
           // ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;
            /*****************************************************************************/
            InitializeFormatDate(txtRegDate);
            InitializeFormatDate(txtTransactionDate);
            /***************************Initialize Events********************************/
            this.txtEmail.EditValueChanged += new System.EventHandler(this.txtEmail_EditValueChanged);
            this.txtEmail.Validating += new System.ComponentModel.CancelEventHandler(this.txtEmail_Validating);
            this.txtCustomerID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCustomerID_Validating);
            this.txtCustomerID.EditValueChanged += new System.EventHandler(this.txtCustomerID_EditValueChanged);
            this.txtArbName.EditValueChanged += new System.EventHandler(this.txtArbName_EditValueChanged);
            this.txtArbName.Validating += new System.ComponentModel.CancelEventHandler(this.txtArbName_Validating);
            this.txtEngName.Validating += new System.ComponentModel.CancelEventHandler(this.txtEngName_Validating);

            this.txtDelegateID.Validating+=txtDelegateID_Validating;
            this.txtConductorID.Validating += txtConductorID_Validating;

            FillCombo.FillComboBoxLookUpEdit(cmbParent, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0   AND AccountLevel=" + ((MySession.GlobalNoOfLevels > 4) ? (Comon.cInt(MySession.GlobalNoOfLevels) - 2) : (Comon.cInt(MySession.GlobalNoOfLevels) - 1)) + " and BranchID=" + MySession.GlobalBranchID, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
            FillCombo.FillComboBoxLookUpEdit(cmbCity, "Cities", "ID", PrimaryName, "", "Cancel =0 ", (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
            
            cmbParent.EditValue = Comon.cDbl(MySession.GlobalDefaultParentCustomerAccountID);
            FillCombo.FillComboBoxLookUpEdit(cmbParentAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0 and BranchID= " + MySession.GlobalBranchID + "  AND AccountLevel=" + (Comon.cInt(MySession.GlobalNoOfLevels) - 1), (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
            FillCombo.FillComboBoxLookUpEdit(cmbTypeCustomer, "Sales_CustomerCategory", "CategoryID", PrimaryName, "", "Cancel =0   ", (UserInfo.Language == iLanguage.English ? "Select Type" : "حدد نوع العميل"));
            
            List<DataColumn> items = new List<DataColumn>();
            items.Add(new DataColumn("ID", typeof(int)));
            items.Add(new DataColumn("Name", typeof(string)));

            DataTable dataTable = new DataTable();
            dataTable.Columns.AddRange(items.ToArray());

            DataRow row1 = dataTable.NewRow();
            row1["ID"] = 1;
            row1["Name"] = UserInfo.Language == iLanguage.Arabic ? "مقبول" : "Acceptable";
            dataTable.Rows.Add(row1);

            DataRow row2 = dataTable.NewRow();
            row2["ID"] = 2;
            row2["Name"] = UserInfo.Language == iLanguage.Arabic ? "جيد" : "Good";
            dataTable.Rows.Add(row2);

            DataRow row3 = dataTable.NewRow();
            row3["ID"] = 3;
            row3["Name"] = UserInfo.Language == iLanguage.Arabic ? "ممتاز" : "Excellent";
            dataTable.Rows.Add(row3);

            cmbCategory.Properties.DataSource = dataTable;
            cmbCategory.Properties.DisplayMember = "Name";
            cmbCategory.Properties.ValueMember = "ID";

   
        }
        #region Declare
        private cCustomers cClass = new cCustomers();

        public string GetNewID;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        public long rowCount = 0;
        public CultureInfo culture = new CultureInfo("en-US");
        private string strSQL;
        private bool IsNewRecord;
        public string ParentAccountID;
        public int AccountLevel;
        string FocusedControl = "";
        private string PrimaryName;
        public string ParentID
        {
            get { return ParentAccountID; }
            set { ParentAccountID = value; }
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
        public string ArbName;
        public string EngName;
        public long AccountID;
        public bool IsNew = false;
        #endregion
        #region Form Event

        #endregion
        #region Function
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
       /// <summary>
       /// This Function to Get New Account Id
       /// </summary>
       /// <returns></returns>
        public long GetNewAccountID()
        {
            if (Comon.cDbl(cmbParentAccountID.EditValue) > 0)
            {
                int SumDigitsCountBeforeSelectedLevel = 1;
                try
                {
                    int code;

                    int sNode;

                    int DigitsCountForSelectedLevel;
                    long MaxID;
                    string str;
                    string strDigits = "";
                    //ParentAccountID = Lip.GetValue("SELECT AccountID FROM Acc_DeclaringMainAccounts WHERE DeclareAccountName='CustomerAccount'");
                    ParentAccountID = cmbParentAccountID.EditValue + "";
                    AccountLevel = int.Parse(Lip.GetValue("SELECT AccountLevel FROM  Acc_Accounts WHERE AccountID = " + ParentAccountID+" and BranchID= " + MySession.GlobalBranchID)) + 1;
                    str = Lip.GetValue("SELECT  MAX(AccountID) FROM  Acc_Accounts Where ParentAccountID=" + ParentAccountID+" and BranchID= " + MySession.GlobalBranchID);
                    strSQL = "SELECT Sum(DigitsNumber) FROM  Acc_AccountsLevels WHERE  BranchID = " + MySession.GlobalBranchID + " And LevelNumber <" + AccountLevel;
                    SumDigitsCountBeforeSelectedLevel = int.Parse(Lip.GetValue(strSQL));
                    strSQL = "SELECT  DigitsNumber FROM  Acc_AccountsLevels WHERE  BranchID = " + MySession.GlobalBranchID + " And LevelNumber =" + AccountLevel;
                    DigitsCountForSelectedLevel = int.Parse(Lip.GetValue(strSQL));
                    if (str == "")
                        code = 0;
                    else
                        code = int.Parse(str.Substring(SumDigitsCountBeforeSelectedLevel, DigitsCountForSelectedLevel));
                    MaxID = 1;
                    for (int i = 1; i <= DigitsCountForSelectedLevel; ++i)
                    {
                        MaxID = MaxID * 10;
                        strDigits = strDigits + "0";

                    }

                    if (code < MaxID)
                    {

                        code = code + 1;
                        GetNewID = ParentAccountID.Substring(0, SumDigitsCountBeforeSelectedLevel) + code.ToString(strDigits);
                        // GetNewID +=code.ToString(strDigits);

                    }
                    else
                    {
                        if (UserInfo.Language == iLanguage.English)
                            XtraMessageBox.Show("You Cannot Add More Than " + MaxID + " Accounts in This Level");
                        else
                            XtraMessageBox.Show("لا يمكن إضافة اكثر من " + MaxID + " حسابات في هذا المستوى");
                    }
                    return long.Parse(GetNewID.PadRight(MySession.GlobalAccountsLevelDigits, '0'));
                }
                catch (Exception ex)
                {
                    Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                }

                
            }
            return Comon.cLong(0);

        }
        /// <summary>
        /// This function to Query to retrieve customer data from the database
        /// </summary>
        public void FillGrid()
        {
            // Query to retrieve customer data from the customers table
           // strSQL = "SELECT  " + cClass.PremaryKey + " as ID, EngName as [customer Name] FROM " + cClass.TableName + " WHERE Cancel =0  ";
            strSQL = "SELECT  " + cClass.PremaryKey + " as ID, "+PrimaryName +" as [Customer Name] FROM " + cClass.TableName + " WHERE Cancel =0 and BranchID= " + MySession.GlobalBranchID;

            if (UserInfo.Language == iLanguage.Arabic)
                // Select the table and fields required from it in English
                strSQL = "SELECT " + cClass.PremaryKey + " as ID," + PrimaryName + "  as [الاسم] FROM " + cClass.TableName + " WHERE Cancel =0 and BranchID= " + MySession.GlobalBranchID; 

            // Execute the query and save the results in a DataTable
            DataTable dt = new DataTable();
            dt = Lip.SelectRecord(strSQL);

            // Display the query results in a GridView
            if (dt.Rows.Count > 0)
            {
                GridView.GridControl.DataSource = dt;
                GridView.Columns[0].Width = 50;
                GridView.Columns[1].Width = 100;
            }
        }
        /// <summary>
        /// this function to select id and name customer
        /// </summary>
        public void Find()
        {

            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = "Where 1=1";

            FocusedControl = GetIndexFocusedControl();

            if (FocusedControl == null) return;
            else if (FocusedControl.Trim() == txtCustomerID.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCustomerID, null, "CustomerID", "رقم الــعـــمـــيــل", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtCustomerID, null, "CustomerID", "Customer ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtDelegateID.Name)
            {
                //if (!MySession.GlobalAllowChangefrmPurchaseDelegateID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtDelegateID, lblDelegateName, "SaleDelegateID", "رقم مندوب المبيعات", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtDelegateID, lblDelegateName, "SaleDelegateID", "Delegate ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtConductorID.Name)
            {
                //if (!MySession.GlobalAllowChangefrmPurchaseDelegateID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtConductorID, lblConductorName, "SaleDelegateID", "رقم مندوب المبيعات", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtConductorID, lblConductorName, "SaleDelegateID", "Delegate ID", MySession.GlobalBranchID);
            }
            GetSelectedSearchValue(cls);
        }
        /// <summary>
        /// This function to Get Selected Search Value
        /// </summary>
        /// <param name="cls"></param>
        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {
                if (FocusedControl == txtCustomerID.Name)
                {
                    txtCustomerID.Text = cls.PrimaryKeyValue.ToString();
                    txtCustomerID_Validating(null, null);
                }
                else if (FocusedControl == txtDelegateID.Name)
                {
                    txtDelegateID.Text = cls.PrimaryKeyValue.ToString();
                    txtDelegateID_Validating(null, null);
                }
                else if (FocusedControl == txtConductorID.Name)
                {
                    txtConductorID.Text = cls.PrimaryKeyValue.ToString();
                    txtConductorID_Validating(null, null);
                }
            }
         }
        private void txtConductorID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as DelegateName FROM Sales_SalesDelegate WHERE DelegateID =" + txtConductorID.Text + " And Cancel =0 And  BranchID =" + Comon.cInt(MySession.GlobalBranchID);
                CSearch.ControlValidating(txtConductorID, lblConductorName, strSQL);
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
                strSQL = "SELECT " + PrimaryName + " as DelegateName FROM Sales_SalesDelegate WHERE DelegateID =" + txtDelegateID.Text + " And Cancel =0 And  BranchID =" + Comon.cInt(MySession.GlobalBranchID);
                CSearch.ControlValidating(txtDelegateID, lblDelegateName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        /// <summary>
        /// This function to read record from cCustomers class to field
        /// </summary>
        public void ReadRecord()
        {
            try
            {
                IsNewRecord = false;
               ClearFields();
                {
                    //set values to field
                    txtCustomerID.Text = cClass.CustomerID.ToString();
                    txtArbName.Text = cClass.ArbName;
                    txtEngName.Text = cClass.EngName;
                    txtMobile.Text = cClass.Mobile;
                    txtTel.Text = cClass.Tel;
                    txtAddress.Text = cClass.Address;
                    txtFax.Text = cClass.Fax;
                    txtNotes.Text = cClass.Notes;
                    txtEmail.Text = cClass.Email;
                    txtVAT.Text = cClass.VATID;
                    txtMaxLimit.Text = cClass.MaxLimit;
                    txtAgeDebt.Text = cClass.MaxAgeDebt;
                    chkAllowMaxLimit.Checked = (cClass.AllowMaxLimit==1)?true:false;
                    chkAllowMaxAgeDebt.Checked=(cClass.AllowMaxAgeDebt==1)?true:false;
                     txtAccountID.Text = cClass.AccountID.ToString();
                     cmbParentAccountID.EditValue = Comon.cDbl(cClass.ParentAccountID.ToString());
                     chkStopAccount.Checked = Comon.cInt(cClass.StopAccount) == 1 ? true : false;
                    txtSpecialDiscount.Text = cClass.SpecialDiscount.ToString();
                    txtRegDate.DateTime = DateTime.ParseExact(Comon.ConvertSerialDateTo(cClass.RegDate), "dd/MM/yyyy", culture);
                    txtTransactionDate.DateTime = DateTime.ParseExact(Comon.ConvertSerialDateTo(cClass.TransactionDate), "dd/MM/yyyy", culture);
                    txtRegion.Text = cClass.Region.ToString();

                    txtBankAccountNo.Text = cClass.BankAccountNo.ToString();

                    txtBankName.Text = cClass.BankName.ToString();
                    cmbCategory.EditValue = cClass.Category;
                    cmbCity.EditValue = cClass.City;

                    cmbCollectionDay.Text = cClass.CollectionDay.ToString();
                    txtConductorID.Text = cClass.ConductorID.ToString();
                    txtDelegateID.Text = cClass.DelegateID.ToString();
                    cmbTypeCustomer.EditValue = cClass.TypeCustomer.ToString();
                    Validations.DoReadRipon(this, ribbonControl1);
                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        /// <summary>
        /// This Function For Clear All TextBox
        /// </summary>
        public void ClearFields()
        {
            try
            {
                txtCustomerID.Text = cClass.GetNewID().ToString();
                txtArbName.Text = " ";
                txtEngName.Text = " ";
                txtMobile.Text = " ";
                txtTel.Text = " ";
                txtAddress.Text = " ";
                txtFax.Text = " ";
                txtNotes.Text = " ";
                txtEmail.Text = "";
                txtVAT.Text = "";
                txtAgeDebt.Text = "";
                txtMaxLimit.Text = "";
                chkAllowMaxAgeDebt.Checked = false;
                chkAllowMaxLimit.Checked = false;
                txtSpecialDiscount.Text = " "; 
                txtAccountID.Text = GetNewAccountID().ToString();
                chkStopAccount.Checked = false;
                txtRegDate.Text = Lip.GetServerDate();
                txtTransactionDate.EditValue = "";
                cmbTypeCustomer.EditValue = "";
                txtBankAccountNo.Text = "";
                txtBankName.Text = "";

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        /******************** MoveRec ************************/
        public void MoveRec(long PremaryKeyValue, int Direction)
        {
            try
            {
                #region If
                if (FormView == true)
                {
                    strSQL = "SELECT TOP 1 * FROM " + cClass.TableName + " Where Cancel =0    and BranchID= " + MySession.GlobalBranchID;
                    switch (Direction)
                    {
                        case xMoveFirst:
                            {
                                strSQL = strSQL + " ORDER BY " + cClass.PremaryKey + " ASC";
                                break;
                            }

                        case xMoveNext:
                            {
                                strSQL = strSQL + " And " + cClass.PremaryKey + ">" + PremaryKeyValue + " ORDER BY " + cClass.PremaryKey + " asc";
                                break;
                            }

                        case xMovePrev:
                            {
                                strSQL = strSQL + " And " + cClass.PremaryKey + "<" + PremaryKeyValue + " ORDER BY " + cClass.PremaryKey + " desc";
                                break;
                            }

                        case xMoveLast:
                            {
                                strSQL = strSQL + " ORDER BY " + cClass.PremaryKey + " DESC";
                                break;
                            }
                    }
                    cClass.GetRecordSetBySQL(strSQL);
                    if (cClass.FoundResult == true)
                        ReadRecord();
                }

                #endregion

                else
                {
                    Messages.MsgStop(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                    return;
                }


            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        
        /*******************Do Functions *************************/
        protected override void DoEdit()
        {
            EnabledControl(true);
            Validations.DoEditRipon(this, ribbonControl1);
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
        protected override void DoNew()
        {
            try
            {
                IsNewRecord = true;
                ClearFields();
                txtArbName.Focus();

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
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
                MoveRec(Comon.cInt(txtCustomerID.Text), xMoveNext);


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
                MoveRec(Comon.cInt(txtCustomerID.Text), xMovePrev);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        /// <summary>
        /// This Function For Show Interface To Search
        /// </summary>
        protected override void DoSearch()
        {
            try
            {
                Find();
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }     
        protected override void DoSave()
        {
            try
            {
                IsNew = false;
                if (IsNewRecord && !FormAdd)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToAddNewRecord);
                    return;
                }
                if (!IsNewRecord)
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
                if (!Validations.IsValidForm(this))
                    return;

                Sales_Customers model = new Sales_Customers();
                model.CustomerID = Comon.cInt(txtCustomerID.Text);

                model.AccountID = cClass.AccountID;
                //Comon.cLong(txtAccountID.Text);
                if (IsNewRecord == true)
                {
                    model.CustomerID = 0;
                    IsNew = true;
                    model.AccountID = GetNewAccountID();
                }

                model.StopAccount = chkStopAccount.Checked == true ? 1 : 0;
                model.ArbName = txtArbName.Text;
                ArbName = txtArbName.Text;
                EngName = txtEngName.Text;
                model.EngName = txtEngName.Text;
                model.SpecialDiscount = Comon.cLong(txtSpecialDiscount.Text);
                model.UserID = UserInfo.ID;
                model.EditUserID = UserInfo.ID;
                model.ComputerInfo = UserInfo.ComputerInfo;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.BranchID = UserInfo.BRANCHID;
                model.FacilityID = UserInfo.FacilityID;
                model.Tel = txtTel.Text.Trim();
                model.Mobile = txtMobile.Text.Trim();
                model.Fax = txtFax.Text.Trim();
                model.Address = txtAddress.Text.Trim();
                model.VATID = txtVAT.Text.Trim();
                model.Notes = txtNotes.Text.Trim();
                model.Email = txtEmail.Text.Trim();

                model.MaxLimit =Comon.cDec(txtMaxLimit.Text.Trim());

                model.AllowMaxAgeDebt = (chkAllowMaxAgeDebt.Checked) ? 1 : 0;
                model.AllowMaxLimit =(chkAllowMaxLimit.Checked)?1:0;
                 
                model.MaxAgeDebt =Comon.cDec(txtAgeDebt.Text.Trim());
                
                model.BankAccountNo = Comon.cLong(txtBankAccountNo.Text);
                model.BankName = txtBankName.Text.ToString();
                model.Category = Comon.cInt(cmbCategory.EditValue);
                model.City = Comon.cInt(cmbCity.EditValue);
                model.CollectionDay = cmbCollectionDay.Text;
                model.ConductorID = Comon.cLong(txtConductorID.Text);
                model.TypeCustomer = Comon.cInt(cmbTypeCustomer.EditValue);
                
                model.RegDate = Comon.cLong(Lip.GetServerDateSerial());
                if (txtTransactionDate.Text.Trim() != "")
                    model.TransactionDate = Comon.ConvertDateToSerial(txtTransactionDate.Text);
                else
                    model.TransactionDate = Comon.ConvertDateToSerial(txtRegDate.Text);
                model.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerDateSerial());
                model.Cancel = 0;
                model.ContactPerson = "";
                model.IdentityNumber = "";
                model.CustomerType = "";
                model.BlockingReason = "";
                model.IsInBlackList = 0;
                model.Gender = 0;
                model.NationalityID = 0;
                AccountID = long.Parse(model.AccountID.ToString());
                model.ParentAccountID = Comon.cDbl(cmbParentAccountID.EditValue);
                int StoreID;
                int UpdateID;
                if (IsNewRecord == true)
                    StoreID = Sales_CustomersDAL.InsertSales_Customers(model);
                else
                    UpdateID = Sales_CustomersDAL.UpdateSales_Customers(model);

                addAccountID();
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                if (IsNewRecord == true)
                    DoNew();
                FillGrid();
                if (Comon.cDbl(this.Text) == 99)
                    this.Close();

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        protected override void DoDelete()
        {
            if (Lip.CheckAccountingTransactions(Comon.cLong(txtAccountID.Text)))
            {
                try
                {

                    if (!FormDelete)
                    {
                        Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToUpdateRecord);
                        return;
                    }
                    else
                    {
                        bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, Messages.msgConfirmDelete);
                        if (!Yes)
                            return;
                    }

                    int TempID = Comon.cInt(txtCustomerID.Text);

                    Sales_Customers model = new Sales_Customers();
                    model.CustomerID = Comon.cInt(txtCustomerID.Text);
                    model.EditUserID = UserInfo.ID;
                    model.BranchID = UserInfo.BRANCHID;
                    model.FacilityID = UserInfo.FacilityID;
                    model.EditComputerInfo = UserInfo.ComputerInfo;
                    model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                    model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());
                    if (cClass.CheckAccountHasTransactions(Comon.cLong(cClass.AccountID)) == true)
                    {
                        XtraMessageBox.Show("الحساب لديه حركة شراء وبيع لايمكن حذفه  ");
                    }
                    else
                    {
                        bool Result = Sales_CustomersDAL.DeleteSales_Customers(model);
                        bool Result1 = DelAccountID();
                        if (Result == true && Result1 == true)
                            Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);
                        MoveRec(model.CustomerID, xMovePrev);
                        FillGrid();



                    }


                }
                catch (Exception ex)
                {
                    Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                }
            }
            else
            {
                Messages.MsgAsterisk("لا يمكن الحذف", "لا يمكن حذف حساب العميل  بسبب وجود حركات محاسبية علية");

            }
        }
        protected override void DoPrint()
        {

            try
            {
                /******************** Report Header *************************/
                GridView.ShowRibbonPrintPreview();

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        /************************************ **********************************************/
        //This Function for Add Acc_AccountID 
        public void addAccountID()
        {
            long testID = GetNewAccountID();
            Acc_Accounts model = new Acc_Accounts();
            model.AccountID = AccountID;
            model.AccountLevel = AccountLevel;
            model.AccountTypeID = 1;
            model.BranchID = UserInfo.BRANCHID;
            model.FacilityID = UserInfo.FacilityID;
            model.StopAccount = chkStopAccount.Checked == true ? 1 : 0;

            model.ParentAccountID = long.Parse(cmbParentAccountID.EditValue.ToString());
            model.MaxLimit = Comon.cDbl(txtMaxLimit.Text);

            model.AllowMaxLimit = (chkAllowMaxLimit.Checked) ? 1 : 0;
            model.MinLimit = 0;
            model.RegDate = Comon.cLong(Lip.GetServerDateSerial());
            model.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
            model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
            model.EditTime = Comon.cLong(Lip.GetServerDateSerial());
            model.Cancel = 0;
            model.ArbName = ArbName;
            model.EngName = EngName;
            model.UserID = UserInfo.ID;
            model.EditUserID = UserInfo.ID;
            model.ComputerInfo = UserInfo.ComputerInfo;
            model.EditComputerInfo = UserInfo.ComputerInfo;
            int StoreID;

            strSQL = "Select * from Acc_Accounts where  BRANCHID= " + model.BranchID + " and AccountID=" + txtAccountID.Text;
            DataTable dtAcco = new DataTable();
            dtAcco = Lip.SelectRecord(strSQL);
            if (dtAcco.Rows.Count > 0)
                Acc_AccountsDAL.UpdateAcc_Accounts(model);
            else
                Acc_AccountsDAL.InsertAcc_Accounts(model);

            //strSQL = "SELECT  *   FROM  Branches";
            //DataTable dtcustomer = Lip.SelectRecord(strSQL);
            //if (dtcustomer.Rows.Count > 0)
            //{
            //    for (int i = 0; i <= dtcustomer.Rows.Count - 1; i++)
            //    {
            //        model.BranchID = Comon.cInt(dtcustomer.Rows[i]["BRANCHID"].ToString());

            //        strSQL = "Select * from Acc_Accounts where  BRANCHID= " + model.BranchID + " and AccountID=" + txtAccountID.Text;
            //        DataTable dtAcco = new DataTable();
            //        dtAcco = Lip.SelectRecord(strSQL);
            //        if (dtAcco.Rows.Count > 0)
            //            Acc_AccountsDAL.UpdateAcc_Accounts(model);
            //        else
            //            Acc_AccountsDAL.InsertAcc_Accounts(model);

            //    }
            //}

        }

        //This Function For Delete The Acc_AccountID 
        public bool DelAccountID()
        {
            Acc_Accounts model = new Acc_Accounts();
            model.AccountID = Comon.cLong(cClass.AccountID);
            model.BranchID = MySession.GlobalBranchID;
            model.FacilityID = UserInfo.FacilityID;
            model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
            model.EditTime = Comon.cLong(Lip.GetServerDateSerial());
            model.EditUserID = UserInfo.ID;
            model.EditComputerInfo = UserInfo.ComputerInfo;

            bool Result;
            Result = Acc_AccountsDAL.DeleteAcc_Accounts(model);
            return Result;
        }
        //This Function For Exception the Email 
        private bool EmailAddressChecker(string emailAddress)
        {

            string regExPattern = "^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$";
            bool emailAddressMatch = Match.Equals(emailAddress, regExPattern);

            return emailAddressMatch;
        }
        #endregion


        #region Event
        private void frmCustomers_Load(object sender, EventArgs e)
        {


            FillGrid();
            DoNew();
           
            if (Comon.cDbl(this.Text) > 0)
            {
                txtMobile.Text = this.Text;
                this.Text = "99";
            }
        }
        private void txtCustomerID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string TempUserID;
                if (int.Parse(txtCustomerID.Text.Trim()) > 0)
                {
                    cClass.GetRecordSet(Comon.cInt(txtCustomerID.Text));
                    TempUserID = txtCustomerID.Text;
                    ClearFields();//clear all field
                    txtCustomerID.Text = TempUserID;
                    if (cClass.FoundResult == true)
                    {
                        if (FormView == true)
                            ReadRecord();
                        else
                        {
                            Messages.MsgStop(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);

                            //return;
                        }
                    }
                    else if (FormAdd == true)
                        IsNewRecord = true;
                    else
                        return;
                }

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void txtCustomerID_EditValueChanged(object sender, EventArgs e)
        {
            ((TextEdit)sender).Properties.Appearance.BorderColor = Color.Black;

        }
        private void txtArbName_EditValueChanged(object sender, EventArgs e)
        {
            ((TextEdit)sender).Properties.Appearance.BorderColor = Color.Black;

        }

        private void GridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                int rowIndex = e.FocusedRowHandle;

                txtCustomerID.Text = GridView.GetRowCellValue(rowIndex, GridView.Columns[0].FieldName).ToString();
                txtCustomerID_Validating(null, null);

            }
            catch (Exception)
            {
                return;
            }

        }
        private void txtArbName_EditValueChanged_1(object sender, EventArgs e)
        {
            ((TextEdit)sender).Properties.Appearance.BorderColor = Color.Black;

        }
        private void GridView_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            int rowIndex = e.RowHandle;
            txtCustomerID.Text = GridView.GetRowCellValue(rowIndex, GridView.Columns[0].FieldName).ToString();
            txtCustomerID_Validating(null, null);
        }
    
        private void txtArbName_Validating(object sender, CancelEventArgs e)
        {
             
            int CustomerID = Comon.cInt(Lip.GetValue("SELECT   CustomerID FROM  [Sales_Customers] where " + PrimaryName + "='" + txtArbName.Text + "' and Cancel=0 and BranchID= " + MySession.GlobalBranchID));

            if (CustomerID > 0 && CustomerID != Comon.cInt(txtCustomerID.Text))
            {
                bool yes = Messages.MsgQuestionYesNo(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "اسم العميل موجود من قبل... هل تريد المتابعة ؟" : "Customer Name is Find . are you following");
                if (!yes)
                    return;
            }
            TextEdit obj = (TextEdit)sender;
            if (UserInfo.Language == iLanguage.Arabic)
            {

                txtEngName.Text = Translator.ConvertNameToOtherLanguage(obj.Text.Trim().ToString(), UserInfo.Language);

            }
        }
        private void txtEngName_Validating(object sender, CancelEventArgs e)
        {
            TextEdit obj = (TextEdit)sender;

            if (UserInfo.Language == iLanguage.English)
                txtArbName.Text = Translator.ConvertNameToOtherLanguage(obj.Text.Trim().ToString(), UserInfo.Language);
        }
        // This  Event Validating TextEdit For Email
        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                //if (!string.IsNullOrEmpty(txtEmail.Text.Trim()))
                //{

                //    if (EmailAddressChecker(txtEmail.Text) == false)
                //    {
                //        txtEmail.Focus();
                //        ToolTipController toolTip = new ToolTipController();
                //        txtEmail.ToolTipController = toolTip;
                //        toolTip.Appearance.BackColor = Color.AntiqueWhite;
                //        toolTip.ShowBeak = true;
                //        toolTip.CloseOnClick = DefaultBoolean.True;
                //        toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                //        toolTip.InitialDelay = 500;
                //        toolTip.ShowBeak = true;
                //        toolTip.Rounded = true;
                //        toolTip.ShowShadow = true;
                //        toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                //        toolTip.SetToolTipIconType(txtEmail, ToolTipIconType.Error);
                //        toolTip.ToolTipType = ToolTipType.Standard;
                //        toolTip.SetTitle(txtEmail, "Error");
                //        toolTip.ShowHint(Messages.msgInputEmil, ToolTipLocation.TopLeft, txtEmail.PointToScreen(new Point(0, txtEmail.Height)));
                //        txtEmail.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);

                //    }
                //} 

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }


        private void txtEmail_EditValueChanged(object sender, EventArgs e)
        {
            ((TextEdit)sender).Properties.Appearance.BorderColor = Color.Black;

        }
        //This Event To Save The Customer By F9 
        private void frmCustomers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F9)
            {
                falgPrint = true;
                DoSave();
            }
            if (e.KeyCode == Keys.F6)
            {
                DoSave();
            }
            if (e.KeyCode == Keys.F3)
                Find();
        }

        #endregion

        private void cmbParentAccountID_EditValueChanged(object sender, EventArgs e)
        {
            txtAccountID.Text = GetNewAccountID().ToString();
        }

        private void chkAllowMaxAgeDebt_CheckedChanged(object sender, EventArgs e)
        {
            if(chkAllowMaxAgeDebt.Checked&&Comon.cInt( txtAgeDebt.Text)<=0)
            {
                chkAllowMaxAgeDebt.Checked = false;
                Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "الرجاء ادخال عمر الديون في الخانة المخصصة له ومن ثم تحديد عدم التجاوز" : "Please enter the age of the debt in the box designated for it and then select not to exceed");
                txtAgeDebt.Focus();
            }
        }

        private void chkAllowMaxLimit_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllowMaxLimit.Checked && Comon.cInt(txtMaxLimit.Text) <= 0)
            {
                chkAllowMaxLimit.Checked = false;
                Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "الرجاء ادخال سقف الديون في الخانة المخصصة له ومن ثم تحديد عدم التجاوز" : "Please enter the age of the debt in the box designated for it and then select not to exceed");
                txtMaxLimit.Focus();
                
            }
        }




        //////////////////
    }
}

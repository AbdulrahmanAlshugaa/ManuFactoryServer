using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Edex.Model;
using Edex.ModelSystem;
using Edex.GeneralObjects.GeneralClasses;
using Edex.Model.Language;
using Edex.SalesAndPurchaseObjects.SalesClasses;
using DevExpress.XtraEditors;
using Edex.DAL;

namespace Edex.RestaurantSystem.Transactions
{
    public partial class ctAddDelivery : UserControl
    {
        //string TableName = "SalesCashierClose";
        //string PremaryKey = "CloseCashierID";
        private cDrivers cClass = new cDrivers();

        public string GetNewID;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        public long rowCount = 0;
       public  int CustomerNo = 0;
        private string strSQL;
        private bool IsNewRecord;
        public string ParentAccountID;
        public int AccountLevel;
        public string ParentID
        {
            get { return ParentAccountID; }
            set { ParentAccountID = value; }
        }
        public string ArbName;
        public string EngName;
        public long AccountID;
        public bool IsNew = false;
        public ctAddDelivery()
        {
            InitializeComponent();
            var strSQL = "";
            if (UserInfo.Language == iLanguage.English)
                strSQL = "EngName";
            else
                strSQL = "ArbName";
          //  Common.filllookupEDit(ref repositoryItemLookUpEdit2, "ID", "HR_District", "ArbName", "Cancel=0");
            FillCombo.FillComboBox(cmbDestrict, "HR_District", "ID", strSQL, "", "Cancel=0", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
            FillCombo.FillComboBox(cmbStreet, "HR_Street", "ID", strSQL, "", "Cancel=0", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
            DoNew();
            txtAddress.Visible = true;
            lblAddress.Visible = true;
        }
        private void DoNew()
        {
            try
            { 
                ClearFields();
                txtArbName.Focus();
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        } 
        public long GetNewAccountID()
        {
            try
            {
                int code;
                int sNode;
                int SumDigitsCountBeforeSelectedLevel;
                int DigitsCountForSelectedLevel;
                long MaxID;
                string str;
                string strDigits = "";
                ParentAccountID = Lip.GetValue("SELECT AccountID FROM Acc_DeclaringMainAccounts WHERE DeclareAccountName='EmpAccountID' And BranchID=" + UserInfo.BRANCHID);
                AccountLevel = int.Parse(Lip.GetValue("SELECT AccountLevel FROM  Acc_Accounts WHERE AccountID = " + ParentAccountID + " AND BranchID = " + UserInfo.BRANCHID)) + 1;
                str = Lip.GetValue("SELECT  MAX(AccountID) FROM  Acc_Accounts Where ParentAccountID=" + ParentAccountID + "  And BranchID =" + UserInfo.BRANCHID);
                strSQL = "SELECT Sum(DigitsNumber) FROM  Acc_AccountsLevels WHERE  BranchID = " + UserInfo.BRANCHID + " And LevelNumber <" + AccountLevel;
                SumDigitsCountBeforeSelectedLevel = int.Parse(Lip.GetValue(strSQL));
                strSQL = "SELECT  DigitsNumber FROM  Acc_AccountsLevels WHERE  BranchID = " + UserInfo.BRANCHID + " And LevelNumber =" + AccountLevel;
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
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

            return long.Parse(GetNewID.PadRight(MySession.GlobalAccountsLevelDigits, '0'));




        }
        
      


        private long GetNewAccountID(int FACILITYID, int BranchID, long ParentAccountID)
        {
            long functionReturnValue = 0;
            string where = "FACILITYID=" + FACILITYID + " AND BRANCHID=" + BranchID;
            string strSQL = "";
            int GlobalAccountsLevelDigits = int.Parse(Lip.GetValue("SELECT  Sum(DigitsNumber) FROM  Acc_AccountsLevels where " + where));
            try
            {
                int code = 0;
                int AccountLevel = int.Parse(Lip.GetValue("SELECT AccountLevel FROM  Acc_Accounts WHERE AccountID = " + ParentAccountID + " AND  " + where));
                int sNode = AccountLevel + 1;
                int SumDigitsCountBeforeSelectedLevel = 0;
                int DigitsCountForSelectedLevel = 0;
                long MaxID = 0;
                string str = Lip.GetValue("SELECT  MAX(AccountID) FROM  Acc_Accounts Where ParentAccountID=" + ParentAccountID + "  And " + where);
                strSQL = "SELECT Sum(DigitsNumber) FROM  Acc_AccountsLevels WHERE  " + where + " And LevelNumber <" + sNode;
                SumDigitsCountBeforeSelectedLevel = int.Parse(Lip.GetValue(strSQL));
                strSQL = "SELECT  DigitsNumber FROM  Acc_AccountsLevels WHERE " + where + " And LevelNumber=" + sNode;
                DigitsCountForSelectedLevel = int.Parse(Lip.GetValue(strSQL));
                if (string.IsNullOrEmpty(str))
                {
                    code = 0;
                }
                else
                {
                    code = int.Parse(str.Substring(SumDigitsCountBeforeSelectedLevel, DigitsCountForSelectedLevel));
                }
                string strDigits = null;
                MaxID = 1;
                for (int i = 1; i <= DigitsCountForSelectedLevel; i++)
                {

                    MaxID = MaxID * 10;
                    if (i == 1)
                        strDigits = strDigits + UserInfo.BRANCHID.ToString();
                    else
                        strDigits = strDigits + "0";
                   // strDigits = strDigits + "0";
                }

                // لكل مستوى عدد محدد من الحسابات
                if (code < MaxID)
                {

                    code = code + 1;
                    string strRet = ParentAccountID.ToString().Substring(0, SumDigitsCountBeforeSelectedLevel) + code.ToString(strDigits);
                    strRet = strRet.PadRight(GlobalAccountsLevelDigits, '0');
                    functionReturnValue = long.Parse(strRet);

                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                return 0;
            }
            return functionReturnValue;
        }


        public void ClearFields()
        {
            try
            {
                txtCustomerID.Text = cClass.GetNewID().ToString();
               // ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Item.Caption = (int.Parse(txtCustomerID.Text)).ToString();
                txtArbName.Text = " ";
                txtEngName.Text = " ";
                txtMobile.Text = " ";
                txtTelefone.Text = " ";
                txtAddress.Text = " ";
                txtApartment.Text = " ";
                txtFloor.Text = " ";
                txtBuilding.Text = " ";
                txtAddress.Text = " ";
                cmbDestrict.EditValue = 0;
                cmbStreet.EditValue = 0;
                //txtFax.Text = " ";
               // /.Text = " ";
               // txtEmail.Text = "";
               // txtVAT.Text = "";
               // txtSpecialDiscount.Text = " ";
               // txtAccountID.Text = GetNewAccountID().ToString();
              //  _sampleData.Clear();

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {

            if (txtArbName.Text != "" || txtMobile.Text!="")
                DoSave();
            else 
            {
                Messages.MsgError("تعذر الحفظ", "يجب ادخال اسم العميل او رقم الهاتف");

            }
        }
        public  void DoSave()
        {
            try
            {
                IsNew = false;
                Sales_Customers model = new Sales_Customers();
                model.CustomerID = Comon.cInt(txtCustomerID.Text);
                model.AccountID = Comon.cLong(txtAccountID.Text);
                model.CustomerID = 0;
                IsNew = true;
                model.ArbName = txtArbName.Text;
                ArbName = txtArbName.Text;
                EngName = txtEngName.Text;
                model.EngName = txtEngName.Text;
                model.SpecialDiscount = Comon.cLong(0);
                model.UserID = UserInfo.ID;
                model.EditUserID = UserInfo.ID;
                model.ComputerInfo = UserInfo.ComputerInfo;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.BranchID = UserInfo.BRANCHID;
                model.FacilityID = UserInfo.FacilityID;
                model.Tel = txtTelefone.Text;
                model.Mobile = txtMobile.Text;
                model.Fax = "";
                model.Address = txtAddress.Text;
                model.VATID = "";
                model.Notes = "";
                model.Email = txtFloor.Text;
                model.RegDate = Comon.cLong(Lip.GetServerDateSerial());
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
                int StoreID;
                bool UpdateID;
                StoreID = Sales_CustomersDAL.InsertSales_Drivers(model);
               // addAccountID();
               // addLocationCustomers(StoreID);
                //if (sendFromExel=false )
                
              
               
                SendKeys.Send("{ESC}");
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
         public void addAccountID()
        {
            try
            {
                long testID = GetNewAccountID();
                Acc_Accounts model = new Acc_Accounts();
                model.AccountID = AccountID;
                model.AccountLevel = AccountLevel;
                model.AccountTypeID = 1;
                model.BranchID = UserInfo.BRANCHID;
                model.FacilityID = UserInfo.FacilityID;
                model.StopAccount = 0;
                model.ParentAccountID = long.Parse(ParentAccountID);
                model.MaxLimit = 0;
                model.MinLimit = 0;
                model.RegDate = Comon.cLong(Lip.GetServerDateSerial());
                model.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerDateSerial());
                model.Cancel = 0;
                model.EndType = 1;
                model.ArbName = ArbName;
                model.EngName = EngName;
                model.UserID = UserInfo.ID;
                model.EditUserID = UserInfo.ID;
                model.ComputerInfo = UserInfo.ComputerInfo;
                model.EditComputerInfo = UserInfo.ComputerInfo;

                int StoreID;
                if (IsNew == true)
                    StoreID = Acc_AccountsDAL.InsertAcc_Accounts(model);
                else
                    Acc_AccountsDAL.UpdateAcc_Accounts(model);
            }
            catch { }
        }
        public bool DelAccountID()
        {
            bool Result = false;
            try
            {
                Acc_Accounts model = new Acc_Accounts();
                model.AccountID = Comon.cLong(cClass.AccountID);
                model.BranchID = UserInfo.BRANCHID;
                model.FacilityID = UserInfo.FacilityID;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerDateSerial());
                model.EditUserID = UserInfo.ID;
                model.EditComputerInfo = UserInfo.ComputerInfo;


                Result = Acc_AccountsDAL.DeleteAcc_Accounts(model);
                return Result;
            }
            catch { }
            return Result;
        }
        private void addLocationCustomers(int CustomerID)
        {

            try
            { 
                Sales_CustomersAddress model = new Sales_CustomersAddress();
                model.ArbName = txtAddress.Text;
                model.EngName = txtAddress.Text;
                model.Location = Comon.cInt(cmbDestrict.EditValue);
                model.Street = Comon.cInt(cmbStreet.EditValue);
                model.Building = txtBuilding.Text;
                model.Floor = txtFloor.Text;
                model.Apartment = txtApartment.Text;
                model.CustomerID = CustomerID;
                model.Cancel = 0;
                int StoreID=0;
               // StoreID = Sales_CustomersDAL.InsertSales_Customers(model);
                CustomerNo = StoreID;
                
            }
            catch (Exception ex)
            {
                // Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
            }
        }

        private void ctAddCustomers_Load(object sender, EventArgs e)
        {

        }

        public void btnClose_Click(object sender, EventArgs e)
        {
            //this.Dispose();
        }
    }
}

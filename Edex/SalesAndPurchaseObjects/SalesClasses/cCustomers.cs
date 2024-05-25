using Edex.Model;
using Edex.ModelSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.SalesAndPurchaseObjects.SalesClasses
{
   public class cCustomers
   {
       
       #region Declare
       public readonly string TableName = "Sales_Customers";
        public readonly string PremaryKey = "CustomerID";
        public int CustomerID;
        public string ArbName;
        public string EngName;
        public string Tel;
        public string Mobile;
        public string Fax;
        public string Email;
        public string Address;
        public string Notes;
        public Nullable<double> AccountID;
        public Nullable<double> ParentAccountID;
        public Nullable<double> SpecialDiscount;
        public string MaxLimit { get; set; }
        public string MaxAgeDebt { get; set; }
        public int AllowMaxLimit { get; set; }
        public int AllowMaxAgeDebt { get; set; }
        public int StopAccount { get; set; }
        public string RegDate { get; set; }
        public string TransactionDate { get; set; }
        public int Category { get; set; }
        public int TypeCustomer { get; set; }
        public float BankAccountNo { get; set; }
        public string BankName { get; set; }
        public int City { get; set; }
        public string Region { get; set; }
        public string CollectionDay { get; set; }
        public double ConductorID { get; set; }
        public double DelegateID { get; set; }
        public string VATID;
        public bool FoundResult;
        public bool NeedSaving;
        public bool IsNewRecord;

        private DataTable dt;
        private string strSQL;
        private object Result;
       #endregion
       /// <summary>
       /// This Function To read data from data table to Proprities and variable 
       /// </summary>
        private void ReadRecord()
        {
            try
            {
                {
                    //set Values to proprties and variable 
                    var withBlock = dt;
                    CustomerID = int.Parse(dt.Rows[0]["CustomerID"].ToString());
                    ArbName = dt.Rows[0]["ArbName"].ToString();
                    EngName = dt.Rows[0]["EngName"].ToString();
                    Notes = dt.Rows[0]["Notes"].ToString();
                    Address = dt.Rows[0]["Address"].ToString();
                    Tel = dt.Rows[0]["Tel"].ToString();
                    Fax = dt.Rows[0]["Fax"].ToString();
                    Mobile = dt.Rows[0]["Mobile"].ToString();
                    SpecialDiscount = long.Parse(dt.Rows[0]["SpecialDiscount"].ToString());
                    VATID = dt.Rows[0]["VATID"].ToString();
                    Email = dt.Rows[0]["Email"].ToString();
                    AccountID = double.Parse(dt.Rows[0]["AccountID"].ToString());
                   ParentAccountID =  Comon.cDbl(dt.Rows[0]["ParentAccountID"].ToString())  ;
                   StopAccount = Comon.cInt(dt.Rows[0]["StopAccount"].ToString());
                   MaxAgeDebt =  dt.Rows[0]["MaxAgeDebt"].ToString();
                   MaxLimit = dt.Rows[0]["MaxLimit"].ToString();
                   AllowMaxAgeDebt = Comon.cInt(dt.Rows[0]["AllowMaxAgeDebt"].ToString());
                   AllowMaxLimit = Comon.cInt(dt.Rows[0]["AllowMaxLimit"].ToString());
                   RegDate =  dt.Rows[0]["RegDate"].ToString();
                   TransactionDate = dt.Rows[0]["TransactionDate"].ToString();
                   Category = Comon.cInt(dt.Rows[0]["Category"].ToString());
                   TypeCustomer = Comon.cInt(dt.Rows[0]["TypeCustomer"].ToString());
                   BankAccountNo = Comon.cLong(dt.Rows[0]["BankAccountNo"].ToString());
                   BankName = dt.Rows[0]["BankName"].ToString();
                   City = Comon.cInt(dt.Rows[0]["City"].ToString());
                   Region = dt.Rows[0]["Region"].ToString();
                   CollectionDay = dt.Rows[0]["CollectionDay"].ToString();
                   ConductorID = Comon.cLong(dt.Rows[0]["ConductorID"].ToString());
                   DelegateID = Comon.cLong(dt.Rows[0]["DelegateID"].ToString());
                }
                FoundResult = true;
                IsNewRecord = false;
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
       /// <summary>
       /// This Function to Get data Customer by CustomerID
       /// </summary>
       /// <param name="PremaryKeyValue"></param>
        public void GetRecordSet(long PremaryKeyValue)
        {
            try
            {
                FoundResult = false;
                strSQL = "SELECT Top 1 * FROM " + TableName
                    + " WHERE Cancel =0 AND " + PremaryKey + "=" + PremaryKeyValue +" and BranchID= " + MySession.GlobalBranchID;
                dt = Lip.SelectRecord(strSQL);//execute the sql select
                if (dt.Rows.Count > 0)
                {
                    ReadRecord();
                    FoundResult = true;
                }
                dt = null;
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

       /// <summary>
       /// This function to get record which  set by sql
       /// </summary>
       /// <param name="strSQL"></param>
        public void GetRecordSetBySQL(string strSQL)
        {
            try
            {
                FoundResult = false;
                dt = Lip.SelectRecord(strSQL);//execute sql select
                if (dt.Rows.Count > 0)
                {
                    ReadRecord();
                    FoundResult = true;
                }
                dt = null;
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
       /// <summary>
       /// This function to Get Max ID +1 for New ID
       /// </summary>
       /// <returns></returns>
        public long GetNewID()
        {
            try
            {
                DataTable dt;//new instance DataTable
                string strSQL;
                strSQL = "SELECT Max(" + PremaryKey + ") + 1 FROM " + TableName+ " where   BranchID= " + MySession.GlobalBranchID;//stetement select Max Customer ID
                dt = Lip.SelectRecord(strSQL);
                string GetNewID = dt.Rows[0][0] == DBNull.Value ? "1" : dt.Rows[0][0].ToString();
                return Convert.ToInt32(GetNewID);

            }
            catch (Exception ex)
            {
                return 0;
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
       
       /// <summary>
        /// This Function To Check if the Account Has any Transactions
       /// </summary>
       /// <param name="AccountID"></param>
       /// <returns>return value Boolen : True if Has Transactions , False if Has not Transactions </returns>
       public bool CheckAccountHasTransactions(long AccountID) {
            try {
                if (CheckReceiptVoucherDetails(AccountID) == true)
                    return true;
                else if (CheckReceiptVoucherMaster(AccountID) == true)
                    return true;
                else if (CheckSpendVoucherDetails(AccountID) == true)
                    return true;
                //else if (CheckSpendVoucherMaster(AccountID) == true)
                //    return true;
                //else if (DeclaringIncomeAccounts(AccountID) == true)
                //    return true;
                //else if (DeclaringMainAccounts(AccountID) == true)
                //    return true;
                else if (ReceiptVoucherDetails(AccountID) == true)
                    return true;
                else if (ReceiptVoucherMaster(AccountID) == true)
                    return true;
                else if (SpendVoucherDetails(AccountID) == true)
                    return true;
                else if (SpendVoucherMaster(AccountID) == true)
                    return true;
                else if (VariousVoucherDetails(AccountID) == true)
                    return true;
                else if (PurchaseInvoiceMaster(AccountID) == true)
                    return true;
                else if (SalesInvoiceMaster(AccountID) == true)
                    return true;
                else if (SalesInvoiceReturnMaster(AccountID) == true)
                    return true;
                else if (HasDeclaringIncomeAccounts(AccountID) == true)
                    return true;
                else if (HasDeclaringMainAccounts(AccountID) == true)
                    return true;
                else if (PurchaseInvoiceReturnMaster(AccountID) == true)
                    return true;

            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

            return false;
        
        }
       /// <summary>
       /// This function is used to check if  the customer has Declaring Income Accounts
       /// </summary>
       /// <param name="AccountID"></param>
       /// <returns>return value boolen :true if has,false if has  not</returns>
        public bool HasDeclaringIncomeAccounts(long AccountID)
        {
            strSQL = "SELECT AccountID FROM Acc_DeclaringIncomeAccounts WHERE   (AccountID = " + AccountID + ")  and BranchID= " + MySession.GlobalBranchID;
             DataTable dt =Lip.SelectRecord(strSQL);
             if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
       /// <summary>
      /// This function is used to check if the customer has Declaring Main Accounts
       /// </summary>
       /// <param name="AccountID"></param>
       /// <returns>return value boolen:true if has, false if has not</returns>
        public bool HasDeclaringMainAccounts(long AccountID)
        {
            strSQL = "SELECT AccountID FROM Acc_DeclaringMainAccounts WHERE   (AccountID = " + AccountID + ") and BranchID= " + MySession.GlobalBranchID;
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
       /// <summary>
        /// this Function To check if the customer has   Receipt Voucher Details from Acc_CheckReceiptVoucherDetails table 
       /// </summary>
       /// <param name="AccountID"></param>
       /// <returns>return value boolen:true if has,false if has not</returns>
        public bool CheckReceiptVoucherDetails(long AccountID)
        {
            strSQL = "SELECT CheckReceiptVoucherID FROM Acc_CheckReceiptVoucherDetails WHERE     (AccountID = " + AccountID + ") and BranchID= " + MySession.GlobalBranchID ;//sql stetment
            DataTable dt = Lip.SelectRecord(strSQL);//execute sql select
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;





        }
       /// <summary>
       /// This function To Check if the customer has Check Receipt Voucher Master from Acc_CheckReceiptVoucherMaster Table
       /// </summary>
       /// <param name="AccountID"></param>
       /// <returns>return value boolen:true if has ,false if has not</returns>
       public bool CheckReceiptVoucherMaster(long AccountID)
        {
            strSQL = "SELECT CheckReceiptVoucherID FROM Acc_CheckReceiptVoucherMaster WHERE   " + " (DebitAccountID = " + AccountID + ") OR (DiscountAccountID = " + AccountID + ") and BranchID= " + MySession.GlobalBranchID;       
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;





        }
       /// <summary>
       /// This Function to Check if the customer has Check Spend Voucher Details
       /// </summary>
       /// <param name="AccountID"></param>
       /// <returns>return value boolen:true if has ,false if has not</returns>
       public bool CheckSpendVoucherDetails(long AccountID)
        {
            strSQL = "SELECT CheckSpendVoucherID FROM Acc_CheckSpendVoucherDetails WHERE    (AccountID = " + AccountID + ") and BranchID= " + MySession.GlobalBranchID ;        
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;

       }
       /// <summary>
       /// This function is used to Check if the customer has Check Spend Voucher Master from Acc_CheckSpendVoucherMaster
       /// </summary>
       /// <param name="AccountID"></param>
       /// <returns>return value boolen: Ture if the customer has any Transactions,false if the customer has not any Transactions </returns>
       public bool CheckSpendVoucherMaster(long AccountID)
        {
            strSQL = "SELECT CheckSpendVoucherID FROM Acc_CheckSpendVoucherMaster WHERE     " + " (CreditAccountID = " + AccountID + ") OR (DiscountAccountID = " + AccountID + ")  and BranchID= " + MySession.GlobalBranchID ;
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
       /// <summary>
       /// This function is used to check if the customer has Income Accounts 
       /// </summary>
       /// <param name="AccountID"></param>
       /// <returns>return value boolen :false </returns>
       public bool DeclaringIncomeAccounts(long AccountID)
        {
            strSQL = "SELECT ID FROM Acc_DeclaringIncomeAccounts WHERE   (AccountID = " + AccountID + ")  and BranchID= " + MySession.GlobalBranchID;
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
       /// <summary>
       /// This Function is used to check the customer is Main Accounts
       /// </summary>
       /// <param name="AccountID"></param>
       /// <returns>return value boolen:true if has ,false if has not</returns>
       public bool DeclaringMainAccounts(long AccountID)
        {
            strSQL = "SELECT ID FROM Acc_DeclaringMainAccounts WHERE   (AccountID = " + AccountID + ") and BranchID= " + MySession.GlobalBranchID;
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;





        }
       /// <summary>
       /// This Functon is used to Check if the customer has any Transactions in Acc_ReceiptVoucherDetails table 
       /// </summary>
       /// <param name="AccountID"></param>
       /// <returns>return value boolen :true if has, false if has not</returns>
       public bool ReceiptVoucherDetails(long AccountID)
        {
            strSQL = "SELECT ID FROM Acc_ReceiptVoucherDetails WHERE   (AccountID = " + AccountID + ")  and BranchID= " + MySession.GlobalBranchID;
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
       /// <summary>
       /// This function is used to check if the customer has Receipt Voucher Master
       /// </summary>
       /// <param name="AccountID"></param>
       /// <returns>return value boolen : true if has, false if has not</returns>
       public bool ReceiptVoucherMaster(long AccountID)
        {
            strSQL = "SELECT ReceiptVoucherID FROM Acc_ReceiptVoucherMaster WHERE(BranchID = " + UserInfo.BRANCHID + ") AND "
                    + " (DebitAccountID = " + AccountID + ") OR (DiscountAccountID = " + AccountID + ")  and BranchID= " + MySession.GlobalBranchID;        
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;





        }
       /// <summary>
       /// This function is used check if the customer has Spend Voucher Details
       /// </summary>
       /// <param name="AccountID"></param>
       /// <returns>return value boolen: Ture if has , false has not</returns>
       public bool SpendVoucherDetails(long AccountID)
        {
            strSQL = "SELECT SpendVoucherID FROM Acc_SpendVoucherDetails WHERE   (AccountID = " + AccountID + ") and BranchID= " + MySession.GlobalBranchID;

            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;





        }
       /// <summary>
       /// This function to used to check if the customer has Spend Voucher Master 
       /// </summary>
       /// <param name="AccountID"></param>
       /// <returns>return value boolen : true if has , false has not </returns>
       public bool SpendVoucherMaster(long AccountID)
        {
            strSQL = "SELECT SpendVoucherID FROM Acc_SpendVoucherMaster WHERE   "
               + " (CreditAccountID = " + AccountID + ") AND (DiscountAccountID = " + AccountID + ") and BranchID= " + MySession.GlobalBranchID;
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;

        }
       /// <summary>
       /// This functoin is used to Check  if the customer has Various Voucher Details
       /// </summary>
       /// <param name="AccountID"></param>
       /// <returns>return value boolen:true if has, false if has not </returns>
       public bool VariousVoucherDetails(long AccountID)
        {
            strSQL = "SELECT VoucherID FROM Acc_VariousVoucherDetails WHERE   (AccountID = " + AccountID + ") and BranchID= " + MySession.GlobalBranchID;

            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
       /// <summary>
       /// this function is used to check the customer has  Purchase Invoice Master 
       /// </summary>
       /// <param name="AccountID"></param>
       /// <returns></returns>
       public bool PurchaseInvoiceMaster(long AccountID)
        {
            strSQL = "SELECT InvoiceID FROM Sales_PurchaseInvoiceMaster WHERE   (CreditAccount = " + AccountID + ") OR "
               + " (DebitAccount = "+AccountID+ ") OR (DiscountCreditAccount = " + AccountID + ") OR (TransportDebitAccount = " + AccountID + ")  and BranchID= " + MySession.GlobalBranchID;
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
       /// <summary>
       /// This function is used to check if the customer has Purchase Invoice Return Master
       /// </summary>
       /// <param name="AccountID"></param>
       /// <returns>return value boolen: True if has, false if has not</returns>
       public bool PurchaseInvoiceReturnMaster(long AccountID)
        {
            strSQL = "SELECT InvoiceID FROM Sales_PurchaseInvoiceReturnMaster WHERE   "
            + " (DebitAccount = " + AccountID + ") OR (CreditAccount = " + AccountID + ") OR (DiscountDebitAccount = " + AccountID + ")  and BranchID= " + MySession.GlobalBranchID;           
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
      /// <summary>
      /// This function is used to check if the customer Sales Invoice Master
      /// </summary>
      /// <param name="AccountID"></param>
      /// <returns>return value boolen:true if has Sales Invoice , false has not</returns>
       public bool SalesInvoiceMaster(long AccountID)
        {
              strSQL = "SELECT InvoiceID FROM Sales_SalesInvoiceMaster WHERE   " 
            + " (DebitAccount = " +AccountID+ ") OR (CreditAccount = " +AccountID+ ") OR (DiscountDebitAccount = " +AccountID+ ")  and BranchID= " + MySession.GlobalBranchID;
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;





        }
      /// <summary>
      /// This function is used to check if the customer has Sales Invoice Return Master
      /// </summary>
      /// <param name="AccountID"></param>
      /// <returns>return value boolen:true if has Sales Invoice Return,false if has not </returns>
       public bool SalesInvoiceReturnMaster(long AccountID)
        {
            strSQL = "SELECT InvoiceID FROM Sales_SalesInvoiceReturnMaster WHERE   "
                      + " (DebitAccount = " + AccountID + ") OR (CreditAccount = " + AccountID + ") OR (DiscountCreditAccount = " + AccountID + ")  and BranchID= " + MySession.GlobalBranchID;
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }


      
   }
   
}

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
    class cSuppliers
    {

        #region Declare
        public readonly string TableName = "Sales_Suppliers";
        public readonly string PremaryKey = "SupplierID";
        public int SupplierID;
        public int NationalityID;
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
        public string CommercialRegister { get; set; }
        public int StopAccount { get; set; }
        public string BankAccountNo;
        public string BankName;
        public string AuthorizedPerson;
        public string VATID;
        public bool FoundResult;
        public bool NeedSaving;
        public bool IsNewRecord;
        private DataTable dt;
        private string strSQL;
        private object Result;
        #endregion

        #region function
        /// <summary>
        /// This function is used to read recored from DataTable to variable
        /// </summary>
        private void ReadRecord()
        {
            try
            {
                {
                    var withBlock = dt;
                    SupplierID = int.Parse(dt.Rows[0]["SupplierID"].ToString());
                    NationalityID = int.Parse(dt.Rows[0]["NationalityID"].ToString());

                    ArbName = dt.Rows[0]["ArbName"].ToString();
                    EngName = dt.Rows[0]["EngName"].ToString();
                    Notes = dt.Rows[0]["Notes"].ToString();
                    Address = dt.Rows[0]["Address"].ToString();
                    Tel = dt.Rows[0]["Tel"].ToString();
                    Fax = dt.Rows[0]["Fax"].ToString();
                    Mobile = dt.Rows[0]["Mobile"].ToString();
                    VATID = dt.Rows[0]["VATID"].ToString();
                    Email = dt.Rows[0]["Email"].ToString();
                    AccountID = double.Parse(dt.Rows[0]["AccountID"].ToString());
                    ParentAccountID = Comon.cDbl(dt.Rows[0]["ParentAccountID"].ToString());

                    CommercialRegister = dt.Rows[0]["CommercialRegister"].ToString();
                    StopAccount = Comon.cInt(dt.Rows[0]["StopAccount"].ToString());

                    BankAccountNo = dt.Rows[0]["BankAccountNo"].ToString();
                    BankName = dt.Rows[0]["BankName"].ToString();
                    AuthorizedPerson = dt.Rows[0]["AuthorizedPerson"].ToString();

                }
                FoundResult = true;
                IsNewRecord = false;
            }
            catch (Exception ex)
            {
                // Lip.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /// <summary>
        /// This function is used to select Supplier by SupplierID
        /// </summary>
        /// <param name="PremaryKeyValue"></param>
        public void GetRecordSet(long PremaryKeyValue)
        {
            try
            {
                FoundResult = false;
                strSQL = "SELECT Top 1 * FROM " + TableName
                    + " WHERE Cancel =0 AND " + PremaryKey + "=" + PremaryKeyValue + " and BranchID=" + MySession.GlobalBranchID;
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                {
                    ReadRecord();
                    FoundResult = true;
                }
                dt = null;
            }
            catch (Exception ex)
            {
                // WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// This Function is used to Get Record which set by sql stetment
        /// </summary>
        /// <param name="strSQL"></param>
        public void GetRecordSetBySQL(string strSQL)
        {
            try
            {
                FoundResult = false;
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                {
                    ReadRecord();
                    FoundResult = true;
                }
                dt = null;
            }
            catch (Exception ex)
            {
                //WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /// <summary>
        /// This functoin is used to get Max ID +1 to New ID
        /// </summary>
        /// <returns>return New ID by type long</returns>
        public long GetNewID()
        {
            try
            {
                DataTable dt;
                string strSQL;
                strSQL = "SELECT Max(" + PremaryKey + ") + 1 FROM " + TableName + " where BranchID=" + MySession.GlobalBranchID;
                dt = Lip.SelectRecord(strSQL);
                string GetNewID = dt.Rows[0][0] == DBNull.Value ? "1" : dt.Rows[0][0].ToString();
                return Convert.ToInt32(GetNewID);

            }
            catch (Exception ex)
            {
                return 0;
                // WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /// <summary>
        /// This Function To Check if the Account Has any Transactions
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns>return value Boolen : True if Has Transactions , False if Has not Transactions</returns>
        public bool CheckAccountHasTransactions(long AccountID)
        {
            try
            {
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
        /// This function is used to check if  the Supplier has Declaring Income Accounts
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns>return value boolen :true if has,false if has  not</returns>
        public bool HasDeclaringIncomeAccounts(long AccountID)
        {
            strSQL = "SELECT AccountID FROM Acc_DeclaringIncomeAccounts WHERE (BranchID = " + UserInfo.BRANCHID + ") AND (AccountID = " + AccountID + ")";
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;





        }
        /// <summary>
        /// This function is used to check if the Supplier has Declaring Main Accounts
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns>return value boolen: true if has, fale if has not</returns>
        public bool HasDeclaringMainAccounts(long AccountID)
        {
            strSQL = "SELECT AccountID FROM Acc_DeclaringMainAccounts WHERE (BranchID = " + UserInfo.BRANCHID + ") AND (AccountID = " + AccountID + ")";
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
            strSQL = "SELECT CheckReceiptVoucherID FROM Acc_CheckReceiptVoucherDetails WHERE (BranchID = " + UserInfo.BRANCHID + ") AND (AccountID = " + AccountID + ")";
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;





        }
        /// <summary>
        /// This function To Check if the supplier has Check Receipt Voucher Master from Acc_CheckReceiptVoucherMaster Table
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns>return value boolen:true if has , false if has not</returns>
        public bool CheckReceiptVoucherMaster(long AccountID)
        {
            strSQL = "SELECT CheckReceiptVoucherID FROM Acc_CheckReceiptVoucherMaster WHERE (BranchID = " + UserInfo.BRANCHID + ") AND " + " (DebitAccountID = " + AccountID + ") OR (DiscountAccountID = " + AccountID + ")";
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;





        }
        /// <summary>
        /// This Function to Check if the supplier has Check Spend Voucher Details
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns>return value boolen:true if has,false if not</returns>
        public bool CheckSpendVoucherDetails(long AccountID)
        {
            strSQL = "SELECT CheckSpendVoucherID FROM Acc_CheckSpendVoucherDetails WHERE  (BranchID = " + UserInfo.BRANCHID + ") " + " AND (AccountID = " + AccountID + ")";
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;





        }
        /// <summary>
        /// This function is used to Check if the supplier has Check Spend Voucher Master from Acc_CheckSpendVoucherMaster
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns>return value boolen: Ture if the supplier has any Transactions,false if the supplier has not any Transactions </returns>
        public bool CheckSpendVoucherMaster(long AccountID)
        {
            strSQL = "SELECT CheckSpendVoucherID FROM Acc_CheckSpendVoucherMaster WHERE (BranchID = " + UserInfo.BRANCHID + ") AND " + " (CreditAccountID = " + AccountID + ") OR (DiscountAccountID = " + AccountID + ")";
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;





        }
        /// <summary>
        /// This function is used to check if the Supplier has Income Accounts 
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns>return value boolen :false </returns>
        public bool DeclaringIncomeAccounts(long AccountID)
        {
            strSQL = "SELECT ID FROM Acc_DeclaringIncomeAccounts WHERE (BranchID = " + UserInfo.BRANCHID + ") AND (AccountID = " + AccountID + ")";
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;

        }
        /// <summary>
        /// This Function is used to check the supplier is Main Accounts
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns>return value boolen :true if has, false if has not</returns>
        public bool DeclaringMainAccounts(long AccountID)
        {
            strSQL = "SELECT ID FROM Acc_DeclaringMainAccounts WHERE (BranchID = " + UserInfo.BRANCHID + ") AND (AccountID = " + AccountID + ")";
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// This Functon is used to Check if the supplier has any Transactions in Acc_ReceiptVoucherDetails table 
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns>return value boolen :true if has, false if has not</returns>
        public bool ReceiptVoucherDetails(long AccountID)
        {
            strSQL = "SELECT ID FROM Acc_ReceiptVoucherDetails WHERE (BranchID = " + UserInfo.BRANCHID + ") AND (AccountID = " + AccountID + ")";
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// This function is used to check if the supplier  has Receipt Voucher Master
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns>return value boolen : true if has, false if has not</returns>
        public bool ReceiptVoucherMaster(long AccountID)
        {
            strSQL = "SELECT ReceiptVoucherID FROM Acc_ReceiptVoucherMaster WHERE(BranchID = " + UserInfo.BRANCHID + ") AND "
                    + " (DebitAccountID = " + AccountID + ") OR (DiscountAccountID = " + AccountID + ")";
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;





        }
        /// <summary>
        /// This function is used check if the Supplier  has Spend Voucher Details
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns>return value boolen: Ture if has , false has not</returns>
        public bool SpendVoucherDetails(long AccountID)
        {
            strSQL = "SELECT SpendVoucherID FROM Acc_SpendVoucherDetails WHERE (BranchID = " + UserInfo.BRANCHID + ") AND (AccountID = " + AccountID + ")";

            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;





        }
        /// <summary>
        /// This function to used to check if the Supplier has Spend Voucher Master 
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns>return value boolen : true if has , false has not </returns>
        public bool SpendVoucherMaster(long AccountID)
        {
            strSQL = "SELECT SpendVoucherID FROM Acc_SpendVoucherMaster WHERE (BranchID = " + UserInfo.BRANCHID + ") AND "
               + " (CreditAccountID = " + AccountID + ") AND (DiscountAccountID = " + AccountID + ")";
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;





        }
        /// <summary>
        /// This functoin is used to Check  if the supplier has Various Voucher Details
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns>return value boolen:true if has, false if has not </returns>
        public bool VariousVoucherDetails(long AccountID)
        {
            strSQL = "SELECT VoucherID FROM Acc_VariousVoucherDetails WHERE (BranchID = " + UserInfo.BRANCHID + ") AND (AccountID = " + AccountID + ")";

            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;





        }
        /// <summary>
        /// this function is used to check the Supplier has  Purchase Invoice Master 
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public bool PurchaseInvoiceMaster(long AccountID)
        {

            strSQL = "SELECT InvoiceID FROM Sales_PurchaseInvoiceMaster WHERE (BranchID = " + UserInfo.BRANCHID + ") AND (CreditAccount = " + AccountID + ") OR "
               + " (DebitAccount = " + AccountID + ") OR (DiscountCreditAccount = " + AccountID + ") OR (TransportDebitAccount = " + AccountID + ")";
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;





        }
        /// <summary>
        /// This function is used to check if the Supplier has Purchase Invoice Return Master
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns>return value boolen: True if has, false if has not</returns>
        public bool PurchaseInvoiceReturnMaster(long AccountID)
        {
            strSQL = "SELECT InvoiceID FROM Sales_PurchaseInvoiceReturnMaster WHERE  (BranchID = " + UserInfo.BRANCHID + ") AND "
            + " (DebitAccount = " + AccountID + ") OR (CreditAccount = " + AccountID + ") OR (DiscountDebitAccount = " + AccountID + ")";

            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// This function is used to check if the Supplier Sales Invoice Master
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns>return value boolen:true if has Sales Invoice , false has not</returns>
        public bool SalesInvoiceMaster(long AccountID)
        {
            strSQL = "SELECT InvoiceID FROM Sales_SalesInvoiceMaster WHERE (BranchID = " + UserInfo.BRANCHID + ") AND "
          + " (DebitAccount = " + AccountID + ") OR (CreditAccount = " + AccountID + ") OR (DiscountDebitAccount = " + AccountID + ")";
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// This function is used to check if the Supplier has Sales Invoice Return Master
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns>return value boolen:true if has Sales Invoice Return,false if has not </returns>
        public bool SalesInvoiceReturnMaster(long AccountID)
        {

            strSQL = "SELECT InvoiceID FROM Sales_SalesInvoiceReturnMaster WHERE (BranchID = " + UserInfo.BRANCHID + ") AND "
                      + " (DebitAccount = " + AccountID + ") OR (CreditAccount = " + AccountID + ") OR (DiscountCreditAccount = " + AccountID + ")";
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;





        }
        #endregion

    }
}

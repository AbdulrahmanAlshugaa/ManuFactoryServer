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
    public class cDrivers
    {

        public readonly string TableName = "Sales_Drivers";
        public readonly string PremaryKey = "DriverID";
        public int DriverID;
        public string ArbName;
        public string EngName;
        public string Tel;
        public string Mobile;
        public string Fax;
        public string Email;
        public string Address;
        public string Notes;
        public Nullable<double> AccountID;
        public Nullable<double> SpecialDiscount;

        public string VATID;
        public bool FoundResult;
        public bool NeedSaving;
        public bool IsNewRecord;

        private DataTable dt;
        private string strSQL;
        private object Result;

        private void ReadRecord()
        {
            try
            {
                {
                    var withBlock = dt;
                    DriverID = int.Parse(dt.Rows[0]["DriverID"].ToString());
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
                }
                FoundResult = true;
                IsNewRecord = false;
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        public void GetRecordSet(long PremaryKeyValue)
        {
            try
            {
                FoundResult = false;
                strSQL = "SELECT Top 1 * FROM " + TableName
                    + " WHERE Cancel =0 AND " + PremaryKey + "=" + PremaryKeyValue;
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
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

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
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        public long GetNewID()
        {
            try
            {
                DataTable dt;
                string strSQL;
                strSQL = "SELECT Max(" + PremaryKey + ") + 1 FROM " + TableName;
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
        public bool CheckAccountHasTransactions(long AccountID) {
            try {
                if (CheckReceiptVoucherDetails(AccountID) == true)
                    return true;
                else if (CheckReceiptVoucherMaster(AccountID) == true)
                    return true;
                else if (CheckSpendVoucherDetails(AccountID) == true)
                    return true;
                else if (CheckSpendVoucherMaster(AccountID) == true)
                    return true;
                else if (DeclaringIncomeAccounts(AccountID) == true)
                    return true;
                else if (DeclaringMainAccounts(AccountID) == true)
                    return true;
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
              
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

            return false;
        
        }
        public bool HasDeclaringIncomeAccounts(long AccountID)
        {
            strSQL = "SELECT AccountID FROM Acc_DeclaringIncomeAccounts WHERE (BranchID = " +UserInfo.BRANCHID + ") AND (AccountID = " + AccountID + ")";
             DataTable dt =Lip.SelectRecord(strSQL);
             if (dt.Rows.Count > 0)
                return true;
            else
                return false;





        }
        public bool HasDeclaringMainAccounts(long AccountID)
        {
            strSQL = "SELECT AccountID FROM Acc_DeclaringMainAccounts WHERE (BranchID = " + UserInfo.BRANCHID + ") AND (AccountID = " + AccountID + ")";
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;





        }
        public bool CheckReceiptVoucherDetails(long AccountID)
        {
            strSQL = "SELECT CheckReceiptVoucherID FROM Acc_CheckReceiptVoucherDetails WHERE (BranchID = " + UserInfo.BRANCHID + ") AND (AccountID = " + AccountID + ")";
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;





        }
        public bool CheckReceiptVoucherMaster(long AccountID)
        {
            strSQL = "SELECT CheckReceiptVoucherID FROM Acc_CheckReceiptVoucherMaster WHERE (BranchID = " + UserInfo.BRANCHID + ") AND " + " (DebitAccountID = " + AccountID + ") OR (DiscountAccountID = " + AccountID + ")";       
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;





        }
        public bool CheckSpendVoucherDetails(long AccountID)
        {
            strSQL = "SELECT CheckSpendVoucherID FROM Acc_CheckSpendVoucherDetails WHERE  (BranchID = " + UserInfo.BRANCHID + ") " + " AND (AccountID = " + AccountID + ")";        
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;





        }
        public bool CheckSpendVoucherMaster(long AccountID)
        {
            strSQL = "SELECT CheckSpendVoucherID FROM Acc_CheckSpendVoucherMaster WHERE (BranchID = " + UserInfo.BRANCHID + ") AND " + " (CreditAccountID = " + AccountID + ") OR (DiscountAccountID = " + AccountID + ")";
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;





        }
        public bool DeclaringIncomeAccounts(long AccountID)
        {
            strSQL = "SELECT ID FROM Acc_DeclaringIncomeAccounts WHERE (BranchID = " + UserInfo.BRANCHID + ") AND (AccountID = " + AccountID + ")";
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;





        }
        public bool DeclaringMainAccounts(long AccountID)
        {
            strSQL = "SELECT ID FROM Acc_DeclaringMainAccounts WHERE (BranchID = " + UserInfo.BRANCHID + ") AND (AccountID = " + AccountID + ")";
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;





        }
        public bool ReceiptVoucherDetails(long AccountID)
        {
            strSQL = "SELECT ID FROM Acc_ReceiptVoucherDetails WHERE (BranchID = " + UserInfo.BRANCHID + ") AND (AccountID = " + AccountID + ")";
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;





        }
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
        public bool SpendVoucherDetails(long AccountID)
        {
            strSQL = "SELECT SpendVoucherID FROM Acc_SpendVoucherDetails WHERE (BranchID = " + UserInfo.BRANCHID + ") AND (AccountID = " + AccountID + ")";

            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;





        }
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
        public bool VariousVoucherDetails(long AccountID)
        {
            strSQL = "SELECT VoucherID FROM Acc_VariousVoucherDetails WHERE (BranchID = " + UserInfo.BRANCHID + ") AND (AccountID = " + AccountID + ")";

            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;





        }
        public bool PurchaseInvoiceMaster(long AccountID)
        {

            strSQL = "SELECT InvoiceID FROM Sales_PurchaseInvoiceMaster WHERE (BranchID = " + UserInfo.BRANCHID + ") AND (CreditAccount = " + AccountID + ") OR "
               + " (DebitAccount = "+AccountID+ ") OR (DiscountCreditAccount = " + AccountID + ") OR (TransportDebitAccount = " + AccountID + ")";
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;





        }
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
        public bool SalesInvoiceMaster(long AccountID)
        {


              strSQL = "SELECT InvoiceID FROM Sales_SalesInvoiceMaster WHERE (BranchID = " +UserInfo.BRANCHID+ ") AND " 
            + " (DebitAccount = " +AccountID+ ") OR (CreditAccount = " +AccountID+ ") OR (DiscountDebitAccount = " +AccountID+ ")";
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;





        }
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




       




    }
   
}

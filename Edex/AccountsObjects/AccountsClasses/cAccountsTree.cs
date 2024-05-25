using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Data;
using Edex.Model;
using Edex.ModelSystem;
namespace Edex.AccountsObjects.AccountsClasses
{
    class cAccountsTree
    {
        public readonly string TableName = "Acc_Accounts";

        // Declare Table Fields
        public long AccountID;
        public string ArbName;
        public string EngName;
        public long BranchID;
        public double ParentAccountID;
        public int AccountLevel;
        public int AccountTypeID;
        public int CurrencyID;
        public int StopAccount;
        public decimal MinLimit;
        public decimal MaxLimit;
        public decimal Budget;
        public bool FoundResult;
        public bool NeedSaving;
        public bool IsNewRecord;


        private DataTable dt;
        private string strSQL;
        private object Result;

        public double BalncDebt;
        public double BalncCredit;
        public double AccountBalnc;

       
        private void ReadRecord()
        {
            try
            {
                {
                    var withBlock = dt;
                    foreach (DataRow row in dt.Rows)
                    {
                        AccountID = row.IsNull("AccountID") == true ? 0 : Comon.cLong(dt.Rows[0]["AccountID"].ToString());
                        ArbName = row.IsNull("ArbName") == true ? "" : dt.Rows[0]["ArbName"].ToString();
                        EngName = row.IsNull("EngName") == true ? "" : dt.Rows[0]["EngName"].ToString();
                        ParentAccountID = row.IsNull("ParentAccountID") == true ? 0 : Comon.cLong(dt.Rows[0]["ParentAccountID"].ToString());
                        AccountTypeID = row.IsNull("AccountTypeID") == true ? 0 : Comon.cInt(dt.Rows[0]["AccountTypeID"].ToString());
                        StopAccount = row.IsNull("StopAccount") == true ? 0 : Comon.cInt(dt.Rows[0]["StopAccount"].ToString());
                        MinLimit = row.IsNull("MinLimit") == true ? 0 : Comon.cInt(dt.Rows[0]["MinLimit"].ToString());
                        MaxLimit = row.IsNull("MaxLimit") == true ? 0 : Comon.cInt(dt.Rows[0]["MaxLimit"].ToString());
                        AccountLevel = row.IsNull("AccountLevel") == true ? 0 : Comon.cInt(dt.Rows[0]["AccountLevel"].ToString());
                        BalncDebt = row.IsNull("BalncDebt") == true ? 0 : Comon.cLong(dt.Rows[0]["BalncDebt"].ToString());
                        BalncCredit = row.IsNull("BalncCredit") == true ? 0 : Comon.cLong(dt.Rows[0]["BalncCredit"].ToString());
                        AccountBalnc = BalncDebt - BalncCredit;
                    }
                }
                FoundResult = true;
                IsNewRecord = false;
            }
            catch (Exception ex)
            {
               // WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public bool CheckIfStopAccount(long AccountID)
        {
            try
            {
               bool  CheckIfStopAccountv = false;
                GetAccountsRecordSet(AccountID);
                if (FoundResult == true)
                {
                    if (StopAccount == 1)
                       return  true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false ; 
            }
        }

        public void GetRecordSet(long BranchID)
        {
            try
            {
                FoundResult = false;
                strSQL = "SELECT Top 1 * FROM " + TableName
                    + " WHERE Cancel =0 And BranchID=" + BranchID;
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
        public void GetAccountsRecordSet(long AccountNo)
        {
            try
            {
                FoundResult = false;
                strSQL = "SELECT Top 1 * FROM Acc_Accounts"
                    + " WHERE Cancel =0 And AccountID =" + AccountNo + " And BranchID=" + UserInfo.BRANCHID;
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
        public double BalncAccountDebt(long  id)
        {
            string sqlstr;
            sqlstr = "SELECT SUM(Debt) as Balance   FROM RestrictionsDaily where acc_code=  " +  id  + "  AND BranchNum=" + UserInfo.BRANCHID + " and posted=1 and Cancel=0";


            dt = Lip.SelectRecord(sqlstr);

            try
            {
                if (dt.Rows.Count > 0)
                {
                    return Comon.cDbl(dt.Rows[0][0].ToString());
                }
                return 0;
            }


            catch (Exception ex)
            {
                return 0;
            }
        }
        public double BalncAccountCredit(object id)
        {
            string sqlstr;
            sqlstr = "SELECT sum(Credit)   FROM RestrictionsDaily where acc_code=  " +  id  + "   AND BranchNum=" + UserInfo.BRANCHID + " and posted=1 And Cancel=0";

            dt = Lip.SelectRecord(sqlstr);
            try
            {
                if (dt.Rows.Count > 0)
                {
                    return Comon.cDbl(dt.Rows[0][0].ToString());
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public double SUMMDENFBETWEEN(object NUMH, object FROMDT, object TODT)
        {
            DataTable dt = new DataTable();

            string sql;
            sql = "select SUM(DEBT)  from  RestrictionsDaily  WHERE  ACC_CODE=" + NUMH + " AND  posted=1 and RegistrationDate >=   " +  FROMDT  + "     and RegistrationDate <=   " + TODT  + "  and posted=1 And Cancel=0 and BranchNum=" + UserInfo.BRANCHID + " ";

            dt = Lip.SelectRecord(sql);

            try
            {
                return Comon.cDbl(dt.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public double SUMDAENFBETWEEN(object NUMH, object FROMDT, object TODT)
        {
            DataTable dt = new DataTable();

            string sql;

            sql = "select  SUM(CREDIT) from  RestrictionsDaily  WHERE  ACC_CODE= " + NUMH + "  AND  posted=1 And Cancel=0 and RegistrationDate >=   " +  FROMDT  + "     and RegistrationDate <=   " +  TODT  + "  and posted=1  and BranchNum=" + UserInfo.BRANCHID + " ";
            dt = Lip.SelectRecord(sql);
            try
            {
                return Comon.cDbl(dt.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public double SUMDebit(object NUMH)
        {
            DataTable dt = new DataTable();
            string sql;
            sql = "select SUM(DEBT)  from  RestrictionsDaily  WHERE  ACC_CODE=" + NUMH + " AND  posted=1   And Cancel=0 and BranchNum=" + UserInfo.BRANCHID;
            dt = Lip.SelectRecord(sql);
            try
            {
                return Comon.cDbl(dt.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public double SUMCridit(object NUMH)
        {
            DataTable dt = new DataTable();
            string sql;
            sql = "select  SUM(CREDIT) from  RestrictionsDaily  WHERE  ACC_CODE= " + NUMH + "  AND  posted=1   And Cancel=0 and BranchNum=" + UserInfo.BRANCHID;
            dt = Lip.SelectRecord(sql);
            try
            {
                return Comon.cDbl (dt.Rows[0][0].ToString ());
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }


   
}

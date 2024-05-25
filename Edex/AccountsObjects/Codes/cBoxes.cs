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
namespace Edex.AccountsObjects.Codes
{
    class cBoxes
    {
         
            #region Declare
        public readonly string TableName = "Acc_Boxes";
        public readonly string PremaryKey = "BoxID";
            public int BoxID;
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
            public int StopAccount { get; set; }
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
                        BoxID = int.Parse(dt.Rows[0]["BoxID"].ToString());
                        ArbName = dt.Rows[0]["ArbName"].ToString();
                        EngName = dt.Rows[0]["EngName"].ToString();
                        Notes = dt.Rows[0]["Notes"].ToString();   
                        AccountID = double.Parse(dt.Rows[0]["AccountID"].ToString());
                        StopAccount = Comon.cInt(dt.Rows[0]["StopAccount"].ToString());                   
                        ParentAccountID = Comon.cDbl(dt.Rows[0]["ParentAccountID"].ToString());

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
            /// This Function to Get data Customer by BoxID
            /// </summary>
            /// <param name="PremaryKeyValue"></param>
            public void GetRecordSet(long PremaryKeyValue)
            {
                try
                {
                    FoundResult = false;
                    strSQL = "SELECT Top 1 * FROM " + TableName
                        + " WHERE Cancel =0 AND " + PremaryKey + "=" + PremaryKeyValue + " and BranchID=" + MySession.GlobalBranchID;
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
                    strSQL = "SELECT Max(" + PremaryKey + ") + 1 FROM " + TableName + " where BranchID= " + MySession.GlobalBranchID;//stetement select Max Customer ID
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
            /// This function is used to check if  the customer has Declaring Income Accounts
            /// </summary>
            /// <param name="AccountID"></param>
            /// <returns>return value boolen :true if has,false if has  not</returns>
            public bool HasDeclaringIncomeAccounts(long AccountID)
            {
                strSQL = "SELECT AccountID FROM Acc_DeclaringIncomeAccounts WHERE   (AccountID = " + AccountID + ")";
                DataTable dt = Lip.SelectRecord(strSQL);
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
                strSQL = "SELECT AccountID FROM Acc_DeclaringMainAccounts WHERE   (AccountID = " + AccountID + ")";
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
                strSQL = "SELECT ID FROM Acc_DeclaringIncomeAccounts WHERE   (AccountID = " + AccountID + ")";
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
                strSQL = "SELECT ID FROM Acc_DeclaringMainAccounts WHERE   (AccountID = " + AccountID + ")";
                DataTable dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                    return true;
                else
                    return false;





            }
             
            
        
    }
}

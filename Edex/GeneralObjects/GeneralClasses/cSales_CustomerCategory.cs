using Edex.Model;
using Edex.ModelSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.GeneralObjects.GeneralClasses
{

    class cSales_CustomerCategory
    {
        /****************************this is region for Variable ***********************/
        #region Declare
        public readonly string TableName = "Sales_CustomerCategory";
        public readonly string PremaryKey = "CategoryID";

        // Declare Table Fields
        public long FacilityID;
        public long BranchID;
        public long CategoryID; 
        public string ArbName;
        public string EngName; 
        public string Notes;
       

        // Public BranchID As Integer

        public bool FoundResult;
        public bool NeedSaving;
        public bool IsNewRecord;

        private DataTable dt;
        private string strSQL;
        private object Result;
        #endregion

        /***************************this is region for function*************************/
        #region Function
        /// <summary>
        /// This Function is Used To Read data Recored From DataTable To variable and Proprties
        /// </summary>
        private void ReadRecord()
        {
            try
            {
                {
                    var withBlock = dt;
                    BranchID = long.Parse(dt.Rows[0]["BranchID"].ToString());
                    FacilityID = long.Parse(dt.Rows[0]["FacilityID"].ToString());
                    CategoryID = long.Parse(dt.Rows[0]["CategoryID"].ToString()); 
                    ArbName = dt.Rows[0]["ArbName"].ToString(); 
                    Notes = dt.Rows[0]["Notes"].ToString(); 
                    EngName = dt.Rows[0]["EngName"].ToString();
                 
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
        /// This Function to Get data User by CategoryID
        /// </summary>
        /// <param name="PremaryKeyValue"></param>
        public void GetRecordSet(long PremaryKeyValue)
        {
            try
            {
                FoundResult = false;
                strSQL = "SELECT Top 1 * FROM " + TableName
                       + " WHERE Cancel =0 AND " + PremaryKey + "=" + PremaryKeyValue+" and BranchID="+MySession.GlobalBranchID;
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

        /// <summary>
        /// This function to get record which  set by sql
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
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        /// <summary>
        /// This function to Get Max ID +1 for New ID
        /// </summary>
        /// <returns>return ID Type Long</returns>
        public long GetNewID()
        {
            try
            {
                DataTable dt;
                string strSQL;
                strSQL = "SELECT Max(" + PremaryKey + ") + 1 FROM " + TableName + " where  BranchID=" + MySession.GlobalBranchID;
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

        #endregion
    }
}

using Edex.ModelSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.GeneralObjects.GeneralClasses
{
    class cUser
    {
        /****************************this is region for Variable ***********************/
        #region Declare
        public readonly string TableName = "Users";
        public readonly string PremaryKey = "UserID";

        // Declare Table Fields
        public long FacilityID;
        public long BranchID;
        public long UserID;
        public long EmployeeID;
        public string Password;
        public int IsActive;
        public string ArbName;
        public string EngName;
        public string Tel;
        public string Mobile;
        public string Fax;
        public string Email;
        public string Address;
        public string Notes;
        public int NumberAllowedDays;
        public int AllowedDate;
        public int IsActiveAllowedDays;
                public int Gender;

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
                    BranchID =long.Parse(dt.Rows[0]["BranchID"].ToString());
                    FacilityID = long.Parse(dt.Rows[0]["FacilityID"].ToString());
                    UserID = long.Parse(dt.Rows[0]["UserID"].ToString());
                    EmployeeID = long.Parse(dt.Rows[0]["EmployeeID"].ToString());
                    ArbName = dt.Rows[0]["ArbName"].ToString();
                    Password =  dt.Rows[0]["Password"].ToString();
                    Notes = dt.Rows[0]["Notes"].ToString();
                    Address = dt.Rows[0]["Address"].ToString();
                    Mobile = dt.Rows[0]["Mobile"].ToString();
                    EngName = dt.Rows[0]["EngName"].ToString();
                    IsActive = int.Parse(dt.Rows[0]["IsActive"].ToString());
                    Email = dt.Rows[0]["Email"].ToString();
                    NumberAllowedDays= int.Parse(dt.Rows[0]["NumberAllowedDays"].ToString());
                    AllowedDate= int.Parse(dt.Rows[0]["AllowedDate"].ToString());
                    IsActiveAllowedDays= int.Parse(dt.Rows[0]["IsActiveAllowedDays"].ToString());
                    Gender= int.Parse(dt.Rows[0]["Gender"].ToString());
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
        /// This Function to Get data User by UserID
        /// </summary>
        /// <param name="PremaryKeyValue"></param>
        public void GetRecordSet(long PremaryKeyValue,int BranchID)
        {
            try
            {
                FoundResult = false;
                strSQL = "SELECT Top 1 * FROM " + TableName
                       + " WHERE Cancel =0 AND " + PremaryKey + "=" + PremaryKeyValue + " and BranchID=" + BranchID;
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
        public long GetNewID(int BrancID)
        {
            try
            {
                DataTable dt;
                string strSQL;
                strSQL = "SELECT Max(" + PremaryKey + ") + 1 FROM " + TableName+" where BranchID="+BrancID ;
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

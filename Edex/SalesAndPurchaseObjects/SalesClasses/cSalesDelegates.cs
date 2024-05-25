using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.SalesAndPurchaseObjects.SalesClasses
{
    class cSalesDelegates
    {
        #region declare 
        public readonly string TableName = "Sales_SalesDelegate";
        public readonly string PremaryKey = "DelegateID";
        public int DelegateID;
        public string ArbName;
        public string EngName;
        public double Percentage;
        public double Target;
        public string Tel;
        public string Mobile;
        public string Fax;
        public string Email;
        public string Address;
        public string Notes;

        public bool FoundResult;
        public bool NeedSaving;
        public bool IsNewRecord;

        private DataTable dt;
        private string strSQL;
        private object Result;
        #endregion
        /// <summary>
        /// This function is used to read record from dataTable Object to variable
        /// </summary>
        private void ReadRecord()
        {
            try
            {
                {
                    //Set Value To variabl 
                    var withBlock = dt;
                    DelegateID = int.Parse(dt.Rows[0]["DelegateID"].ToString());
                    ArbName = dt.Rows[0]["ArbName"].ToString();
                    EngName = dt.Rows[0]["EngName"].ToString();
                    Notes = dt.Rows[0]["Notes"].ToString();
                    Address = dt.Rows[0]["Address"].ToString();
                    Tel = dt.Rows[0]["Tel"].ToString();
                    Fax = dt.Rows[0]["Fax"].ToString();
                    Mobile = dt.Rows[0]["Mobile"].ToString();
                    Email = dt.Rows[0]["Email"].ToString();
                    Target = long.Parse(dt.Rows[0]["Target"].ToString());
                    Percentage = long.Parse(dt.Rows[0]["Percentage"].ToString());
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
        /// This Function to Get data Sales Delegate by DelegateID
        /// </summary>
        /// <param name="PremaryKeyValue"></param>
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
                strSQL = "SELECT Max(" + PremaryKey + ") + 1 FROM " + TableName;
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
    }
}

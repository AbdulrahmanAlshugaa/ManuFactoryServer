 
using Edex.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.GeneralObjects.GeneralClasses
{
    class cFacility
    {
        #region Declare variable &Proprties
        public readonly string TableName = "Facilityes";
        public readonly string PremaryKey = "ID";
        public int BranchID;
        public string ArbName;
        public string EngName;
        public string Tel;
        public string Fax;
        public string Email;
        public string Address;
        public int IsActive;

        public bool FoundResult;
        public bool IsNewRecord;
        private DataTable dt;
        private string strSQL;

        public int USERCREATED { get; set; }
        public int USERUPDATED { get; set; }
        public int USERDELETED { get; set; }
        public long DATECREATED { get; set; }
        public long DATEUPDATED { get; set; }
        public long DATEDELETED { get; set; }
        public int CREATEDTIME { get; set; }
        public int UPDATEDTIME { get; set; }
        public int DELETED { get; set; }
        
        public int FacilityID { get; set; }
        public string ComputerInfo { get; set; }
        public string EditComputerInfo { get; set; }
        public int TIMECREATED { get; set; }
        public int TIMEUPDATED { get; set; }
        public int TIMEDELETED { get; set; }
        #endregion

        /// <summary>
        /// This Function is Used To Read data Recored From DataTable To variable and Proprties
        /// </summary>
        private void ReadRecord()
        {
            try
            {
                {
                    var withBlock = dt;
                    BranchID = int.Parse(dt.Rows[0]["ID"].ToString());
                    ArbName = dt.Rows[0]["ArbName"].ToString();
                    EngName = dt.Rows[0]["EngName"].ToString();
                    Address = dt.Rows[0]["Address"].ToString();
                    Tel = dt.Rows[0]["Tel"].ToString();
                    Fax = dt.Rows[0]["Fax"].ToString();
                    IsActive = Comon.cInt(dt.Rows[0]["IsActive"].ToString());
                    Email = dt.Rows[0]["Email"].ToString();


                    USERCREATED = Comon.cInt(dt.Rows[0]["USERCREATED"]);
                    DATECREATED = Comon.cInt(dt.Rows[0]["DATECREATED"]);
                    TIMECREATED = Comon.cInt(dt.Rows[0]["TIMECREATED"]);

                    USERUPDATED = Comon.cInt(dt.Rows[0]["USERUPDATED"]);
                    DATEUPDATED = Comon.cInt(dt.Rows[0]["DATEUPDATED"]);
                    TIMEUPDATED = Comon.cInt(dt.Rows[0]["TIMEUPDATED"]);

                    USERDELETED = Comon.cInt(dt.Rows[0]["USERDELETED"]);
                    DATEDELETED = Comon.cInt(dt.Rows[0]["DATEDELETED"]);
                    TIMEDELETED = Comon.cInt(dt.Rows[0]["TIMEDELETED"]);

                    EditComputerInfo = dt.Rows[0]["EditComputerInfo"].ToString();
                    ComputerInfo = dt.Rows[0]["ComputerInfo"].ToString();

                  


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
        /// This Function to Get data facility by facilityID
        /// </summary>
        /// <param name="PremaryKeyValue"></param>
        public void GetRecordSet(long PremaryKeyValue)
        {
            try
            {
                FoundResult = false;
                strSQL = "SELECT   * FROM " + TableName
                    + " WHERE DELETED =0 AND " + PremaryKey + "=" + PremaryKeyValue;

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
                //WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
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

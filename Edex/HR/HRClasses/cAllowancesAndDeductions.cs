using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
 
namespace Edex.HR.HRClasses
{
    class cAllowancesAndDeductions
    {
        public string TableName = "HR_AllowanceAndDeduction";
        public readonly string PremaryKey = "SN";
        // Declare Table Fields
        public int SN;
        public long EmployeeID;
        public bool FoundResult;
        public bool IsNewRecord;
        public bool NeedSaving;

        public DataTable dt;

        public string strSQL;
        public Object Result;

        private void ReadRecord()
        {
            try
            {
                {
                    var withBlock = dt;
                    SN = int.Parse(dt.Rows[0]["SN"].ToString());
                    EmployeeID =long.Parse( dt.Rows[0]["EmployeeID"].ToString());
                    
                }
                FoundResult = true;
                IsNewRecord = false;
            }
            catch (Exception ex)
            {
                // Lip.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
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
                // WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
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
                //WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
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
                // WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

    }
}

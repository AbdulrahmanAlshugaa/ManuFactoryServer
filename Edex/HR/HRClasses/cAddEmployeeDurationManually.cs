using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.StockObjects.StoresClasses
{
    public class cAddEmployeeDurationManually
    {
        public  string TableName = "TimStf_FingersPrintArchive";
        public readonly string PremaryKey = "ID";
        // Declare Table Fields
        public int ID;
         
        public string Notes;

        public int EmployeeIDInFingerTecDevice;
        public long FingerPrintDate;
        public long FingerPrintTime;
         
        public bool FoundResult;
        public bool NeedSaving;
        public bool IsNewRecord;

        private DataTable dt;
        private string strSQL;
        private object Result;

        public Nullable<int> Cancel { get; set; }
        public Nullable<int> BranchID { get; set; }
        public Nullable<int> FacilityID { get; set; }
        public int UserID { get; set; }
        public double RegDate { get; set; }
        public double RegTime { get; set; }
        public int EditUserID { get; set; }
        public double EditTime { get; set; }
        public double EditDate { get; set; }
        public string ComputerInfo { get; set; }
        public string EditComputerInfo { get; set; }
        private void ReadRecord()
        {
            try
            {
                {
                    var withBlock = dt;
                    ID = int.Parse(dt.Rows[0]["ID"].ToString());
                    Notes = dt.Rows[0]["Notes"].ToString();
                    EmployeeIDInFingerTecDevice = int.Parse(dt.Rows[0]["EmployeeIDInFingerTecDevice"].ToString());
                    FingerPrintDate = int.Parse(dt.Rows[0]["FingerPrintDate"].ToString());
                    FingerPrintTime = int.Parse(dt.Rows[0]["FingerPrintTime"].ToString());

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

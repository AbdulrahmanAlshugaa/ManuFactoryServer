using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.StockObjects.StoresClasses
{
  
    public class cVacationRequest
    {
        public string TableName = "HR_VacationRequest";
        public readonly string PremaryKey = "SN";
        // Declare Table Fields
        public int SN { get; set; }
        public long EmployeeID { get; set; }
        public int VacationTypeID { get; set; }
        public long AccuredVacation { get; set; }
        public long RequestDate { get; set; }
        public decimal BalanceBeforeVacation { get; set; }
        public string Notes { get; set; }
        public long DaysRequired { get; set; }
        public int LeaveWithoutPay { get; set; }
        public long StartDate { get; set; }
        public long EndDate { get; set; }
        public long ReturnDate { get; set; }
        public long ActualVacationDays { get; set; }
        public decimal VacationSalary { get; set; }
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
                var withBlock = dt;
                SN = int.Parse(dt.Rows[0]["SN"].ToString());
                EmployeeID = long.Parse(dt.Rows[0]["EmployeeID"].ToString());
                VacationTypeID = int.Parse(dt.Rows[0]["VacationTypeID"].ToString());
                AccuredVacation = long.Parse(dt.Rows[0]["AccuredVacation"].ToString());
                RequestDate = int.Parse(dt.Rows[0]["RequestDate"].ToString());
                BalanceBeforeVacation = decimal.Parse(dt.Rows[0]["BalanceBeforeVacation"].ToString());
                Notes = dt.Rows[0]["Notes"].ToString();
                DaysRequired = long.Parse(dt.Rows[0]["DaysRequired"].ToString());
                StartDate = long.Parse(dt.Rows[0]["StartDate"].ToString());
                LeaveWithoutPay = int.Parse(dt.Rows[0]["LeaveWithoutPay"].ToString());
                ActualVacationDays = long.Parse(dt.Rows[0]["ActualVacationDays"].ToString());
                VacationSalary = long.Parse(dt.Rows[0]["VacationSalary"].ToString());
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

        public int GetNewID()
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

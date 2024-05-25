using Edex.Model;
using Edex.ModelSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.HR.HRClasses
{
    public class cEmployeeFiles
    {

        #region Declare
        public readonly string TableName = "HR_EmployeeFile";
        public readonly string PremaryKey = "EmployeeID";
        public long EmployeeID;
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
        public Nullable<double> WorkingHours;
        public string CardID;
        public int Sex;
        public int MaritalStatus;

        public double BirthDate;
        public double ContractEnd;
        public double DateStartWork;
        public double DateIssuanceID;

        public int Nationality;
        public int Religions;
        public int WorkType;
        public int ScientificDisciplines;
        public int Occupation;
        public int IqamaOccupation;
        public int ContractType;
        public int Administration;
        public int PaymentMethod;
        public int TerminationReason;
        public int Department;
        public int StopSalary=0;
        public int ClinicID = 0;
        public int Emptype = 0;
        public int CostCenterID = 0;
        public string LeaveNotes;
        public string WorkAddress;
        public string CompanyVehicle;

        public string CurrentSponsorMobile;

        public string CurrentSponsor;
        public double AccountMeter;
        public int StopAccount { get; set; }
        public string FootprintEmpID;
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
                    EmployeeID = long.Parse(dt.Rows[0]["EmployeeID"].ToString());
                    ArbName = dt.Rows[0]["ArbName"].ToString();
                    EngName = dt.Rows[0]["EngName"].ToString();
                    Notes = dt.Rows[0]["EmpNotes"].ToString();
                    Address = dt.Rows[0]["HomeAddress"].ToString();
                    Tel = dt.Rows[0]["HomeTel"].ToString();
                    Fax = dt.Rows[0]["WorkMobile"].ToString();
                    Mobile = dt.Rows[0]["HomeMobil"].ToString();
                    WorkingHours = long.Parse(dt.Rows[0]["WorkingHours"].ToString());
                    FootprintEmpID = dt.Rows[0]["FootprintEmpID"].ToString();
                    Email = dt.Rows[0]["WorkEmail"].ToString();
                    AccountID = double.Parse(dt.Rows[0]["OnAccountID"].ToString());
                    ParentAccountID = Comon.cDbl(dt.Rows[0]["ParentAccountID"].ToString());
                    StopAccount = Comon.cInt(dt.Rows[0]["StopAccount"].ToString());
                    TerminationReason = Comon.cInt(dt.Rows[0]["TerminationReason"].ToString());
                    CardID = dt.Rows[0]["ClinicID"].ToString();
                    Sex = Comon.cInt(dt.Rows[0]["Sex"].ToString());
                    Nationality = Comon.cInt(dt.Rows[0]["Nationality"].ToString());
                    Religions = Comon.cInt(dt.Rows[0]["Religions"].ToString());
                    WorkType = Comon.cInt(dt.Rows[0]["WorkType"].ToString());
                    WorkingHours = Comon.cInt(dt.Rows[0]["WorkingHours"].ToString());
                    ScientificDisciplines = Comon.cInt(dt.Rows[0]["ScientificDisciplines"].ToString());
                    Occupation = Comon.cInt(dt.Rows[0]["Occupation"].ToString());
                    Administration = Comon.cInt(dt.Rows[0]["Administration"].ToString());
                    Department = Comon.cInt(dt.Rows[0]["Department"].ToString());
                    CurrentSponsorMobile = dt.Rows[0]["CurrentSponsorMobile"].ToString();
                    CompanyVehicle = dt.Rows[0]["CompanyVehicle"].ToString();
                    CurrentSponsor = dt.Rows[0]["CurrentSponsor"].ToString();
                    ParentAccountID = Comon.cDbl(dt.Rows[0]["ParentAccountID"].ToString());
                    AccountMeter = Comon.cDbl(dt.Rows[0]["AccountMeter"].ToString());
                    
                    BirthDate = Comon.cDbl(dt.Rows[0]["BeginningDate"].ToString());
                    DateStartWork = Comon.cDbl(dt.Rows[0]["DateStartWork"].ToString());
                    DateIssuanceID = Comon.cDbl(dt.Rows[0]["DateIssuanceID"].ToString());
                    ContractEnd = Comon.cDbl(dt.Rows[0]["ContractEnd"].ToString());

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
        /// This Function to Get data Customer by EmployeeID
        /// </summary>
        /// <param name="PremaryKeyValue"></param>
        public void GetRecordSet(long PremaryKeyValue)
        {
            try
            {
                FoundResult = false;
                strSQL = "SELECT Top 1 * FROM " + TableName
                    + " WHERE Cancel =0 AND " + PremaryKey + "=" + PremaryKeyValue;
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
                strSQL = "SELECT Max(" + PremaryKey + ") + 1 FROM " + TableName;//stetement select Max Customer ID
                dt = Lip.SelectRecord(strSQL);
                string GetNewID = dt.Rows[0][0] == DBNull.Value ? "1" : dt.Rows[0][0].ToString();
                return Convert.ToInt64(GetNewID);
            }
            catch (Exception ex)
            {
                return 0;
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }




    }
}
   
 

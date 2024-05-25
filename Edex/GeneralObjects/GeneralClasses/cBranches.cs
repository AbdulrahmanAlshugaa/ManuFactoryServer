using Edex.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.GeneralObjects.GeneralClasses
{
   public class cBranches
   {
       #region Declare 
       public readonly string TableName = "Branches";
        public readonly string PremaryKey = "BranchID";
        public int BranchID;
        public int FacilityID { get; set; }
        public string ArbName;
        public string EngName;
        public string Tel;
        public string Fax;
        public string Email;
        public string Address;
        public int IsActive;
        public string VatID;
        public bool FoundResult;
        public bool NeedSaving;
        public bool IsNewRecord;
        private DataTable dt;
        private string strSQL;
        private object Result;

        public int UserCreted { get; set; }
        public int UserUpdated { get; set; }
        public int UserDeleted { get; set; }

        public int DateCreted { get; set; }
        public int DateUpdated { get; set; }
        public int DateDleted { get; set; }

        public int TimeCreted { get; set; }
        public int TimeUpdated { get; set; }
        public int TimeDeleted { get; set; }

        public int DELETED { get; set; }

        public string ComputerInfo { get; set; }
        public string EditComputerInfo { get; set; }
       #endregion
       /// <summary>
       /// This Function is Used To Read data Recored From DataTable To variable and Proprties
       /// </summary>
       private void  ReadRecord()
        {
            try
            {
              //S
                BranchID = int.Parse(dt.Rows[0]["BranchID"].ToString());
                ArbName = dt.Rows[0]["ArbName"].ToString();
                EngName = dt.Rows[0]["EngName"].ToString();
                Address = dt.Rows[0]["Address"].ToString();
                Tel = dt.Rows[0]["Tel"].ToString();
                Fax = dt.Rows[0]["Fax"].ToString();
                IsActive = Comon.cInt(dt.Rows[0]["IsActive"].ToString());
                Email = dt.Rows[0]["Email"].ToString();
                FacilityID = int.Parse(dt.Rows[0]["FACILITYID"].ToString());
                VatID = dt.Rows[0]["VatID"].ToString();

                UserCreted = Comon.cInt(dt.Rows[0]["UserCreted"].ToString());
                UserUpdated = Comon.cInt(dt.Rows[0]["UserUpdated"].ToString());
                UserDeleted = Comon.cInt(dt.Rows[0]["UserDeleted"].ToString());

                DateCreted = Comon.cInt(dt.Rows[0]["DateCreted"].ToString());
                DateUpdated = Comon.cInt(dt.Rows[0]["DateUpdated"].ToString());
                DateDleted = Comon.cInt(dt.Rows[0]["DateDleted"].ToString());

                TimeCreted = Comon.cInt(dt.Rows[0]["TimeCreted"].ToString());
                TimeUpdated = Comon.cInt(dt.Rows[0]["TimeUpdated"].ToString());
                TimeDeleted = Comon.cInt(dt.Rows[0]["TimeDeleted"].ToString());

                ComputerInfo = dt.Rows[0]["ComputerInfo"].ToString();
                EditComputerInfo = dt.Rows[0]["EditComputerInfo"].ToString();
                 
                FoundResult = true;
                IsNewRecord = false;
             
            }
            catch (Exception ex)
            {
                FoundResult = false;
            }
        }

       /// <summary>
       /// This Function to Get data Branch by BranchID
       /// </summary>
       /// <param name="PremaryKeyValue"></param>
        public void GetRecordSet(long PremaryKeyValue)
        {
            try
            {
                FoundResult = false;
                strSQL = "SELECT Top 1 * FROM " + TableName
                    + " WHERE cancel =0 AND " + PremaryKey + "=" + PremaryKeyValue;
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
       /// <summary>
       /// This Function To insert Data To Branches Table
       /// </summary>
       /// <param name="objRecord"></param>
       /// <param name="CmdTyepe"></param>
       /// <returns></returns>
        public Int32 InsertBranches(cBranches objRecord, int CmdTyepe)
        {
            Int32 objRet = 0;
            cOmex c = new cOmex();
            c.NewFields();
            c.Table = "Branches";
            c.AddNumericField("ID", objRecord.BranchID);
            c.AddNumericField("FacilityID", objRecord.FacilityID);
            c.AddStringField("ArbName", objRecord.ArbName);
            c.AddStringField("EngName", objRecord.EngName);
            c.AddStringField("Address", objRecord.Address);
            c.AddStringField("Fax", objRecord.Fax);
            c.AddStringField("Tel", objRecord.Tel);
            c.AddStringField("Email", objRecord.Email);
            c.AddStringField("Notes", objRecord.VatID);
            c.AddNumericField("IsActive", objRecord.IsActive);

            if (CmdTyepe == 1)
            {
                c.AddNumericField("TimeCreted", objRecord.TimeCreted);
                c.AddNumericField("UserCreted", objRecord.UserCreted);
                c.AddNumericField("DateCreted", objRecord.DateCreted);
                c.AddStringField("ComputerInfo", objRecord.ComputerInfo);
                c.AddNumericField("TimeUpdated", objRecord.TimeUpdated);
                c.AddNumericField("UserUpdated", objRecord.UserUpdated);
                c.AddNumericField("DateUpdated", objRecord.DateUpdated);
                c.AddStringField("EditComputerInfo", objRecord.EditComputerInfo);
            }
            else
            {
                c.AddNumericField("TimeUpdated", objRecord.TimeUpdated);
                c.AddNumericField("UserUpdated", objRecord.UserUpdated);
                c.AddNumericField("DateUpdated", objRecord.DateUpdated);
                c.AddStringField("EditComputerInfo", objRecord.EditComputerInfo);
            }

            c.AddStringField("VatID", objRecord.VatID);
            c.AddNumericField("TimeDeleted", 0);
            c.AddNumericField("UserDeleted", 0);
            c.AddNumericField("DateDleted", 0);

            c.AddNumericField("Deleted", 0);
            if (CmdTyepe == 1)
                c.ExecuteInsert();
            else
            {
                c.sCondition = " ID = " + objRecord.BranchID + " And FacilityID= " + objRecord.FacilityID;
                c.ExecuteUpdate();
            }
            objRet = objRecord.BranchID;
            return objRet;
        }
       /// <summary>
       /// This Function to Delete record from  Branches Table
       /// </summary>
       /// <param name="objRecord"></param>
       /// <returns></returns>
       public Int32 Delete(cBranches objRecord)
        {
            Int32 objRet = 0;
            cOmex c = new cOmex();
            c.Table = "Branches";
            c.NewFields();
            c.AddNumericField("TimeDeleted", objRecord.TimeDeleted);
            c.AddNumericField("UserDeleted", objRecord.UserDeleted);
            c.AddNumericField("DateDleted", objRecord.DateDleted);
            c.AddNumericField("DELETED", 1);
            c.sCondition = " ID = " + objRecord.BranchID + " And FacilityID =" + objRecord.FacilityID;
            c.ExecuteUpdate();
            objRet = objRecord.BranchID;
            return objRet;
        }

    }
}

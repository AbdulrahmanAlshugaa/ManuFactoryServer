﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.RestaurantSystem.Classes
{
    public class cSections
    {
        public readonly string TableName = "Res_Sections";
        public readonly string PremaryKey = "SectionID";

        // Declare Table Fields
        public int SectionID;
        public string ArbName;
        public string EngName;
        // Public BranchID As Integer
        public string Notes;

        public bool FoundResult;
        public bool NeedSaving;
        public bool IsNewRecord;

        private DataTable dt;
        private string strSQL;
        private object Result;

        private void ReadRecord()
        {
            try
            {
                {
                    var withBlock = dt;
                    SectionID = int.Parse(dt.Rows[0]["SectionID"].ToString());
                    ArbName = dt.Rows[0]["ArbName"].ToString();
                    EngName = dt.Rows[0]["EngName"].ToString();
                    Notes = dt.Rows[0]["Notes"].ToString();
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

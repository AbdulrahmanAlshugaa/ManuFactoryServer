//using AMSACO_Client.ServiceReferenceDentel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Edex.DAL
{
  public   class DAL_Trans
    {
        public string[] arrayDay = { "31", "28", "31", "30", "31", "30", "31", "31", "30", "31", "30", "31" };
        public string[] arrayMonth = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };

        public string[] arrayDays = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" };

        private Hashtable TempTable;
        private string _table;
        private string _sCondition;
        public string FormatType;
        public string[] PubArrParameters = new string[30];
        public long GlobalBRANCH_ID;
        public long GlobalUserID;
        //WebServiceDentelSoapClient dental;
        public string ProjectPath;
        public string strSQL="";
       

        ~DAL_Trans()
        {
        }

        public string Table
        {
            get { return _table; }
            set { _table = value; }
        }

        public string sCondition
        {
            get { return _sCondition; }
            set { _sCondition = value; }
        }

        public void NewFields()
        {
            if (TempTable != null)
            {
                TempTable.Clear();
            }
        }
               
        public void AddNumericField(string Filedname, string FieldValue)
        {
            if (TempTable == null)
            {
                TempTable = new Hashtable();
            }
            if (FieldValue == "False")
            {
                FieldValue = "0";
            }
            if (FieldValue == "True")
            {
                FieldValue = "1";
            }
            if (FieldValue == null)
            {
                FieldValue = "0";
            }

            if (FieldValue == string.Empty)
            {
                FieldValue = "0";
            }
            TempTable.Add(Filedname, FieldValue);

        }

        public void AddStringField(string Filedname, string FieldValue)
        {
            if (TempTable == null)
            {
                TempTable = new Hashtable();
            }

            if (FieldValue == string.Empty)
            {
                FieldValue = " ";
            }
            FieldValue = FieldValue.Replace("'", "’");
            TempTable.Add(Filedname, "'" + FieldValue + "'");


        }

        public string GetInsertQuary()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO " + Table + "(");
            foreach (DictionaryEntry entry in TempTable)
            {
                sb.Append(entry.Key.ToString() + ",");
            }
            sb = sb.Remove(sb.Length - 1, 1);

            sb.Append(") VALUES (");
            foreach (DictionaryEntry entry in TempTable)
            {
                sb.Append(entry.Value + ",");
            }

            sb = sb.Remove(sb.Length - 1, 1);
            sb.Append(")");
            return sb.ToString();
        }

        public string  StoreInsert()
        {

            strSQL = GetInsertQuary();
            return  strSQL;
        }

        public string GetUpdateStr()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Update " + Table + " Set ");
            foreach (DictionaryEntry entry in TempTable)
            {
                sb.Append(entry.Key.ToString() + "=");

                sb.Append(entry.Value + ",");
            }

            sb = sb.Remove(sb.Length - 1, 1);

            sb.Append(" Where " + sCondition);

            return sb.ToString();
        }

        public string StoreUpdate()
        {

            strSQL = GetUpdateStr();
            return strSQL;
        }

        public string StoreDelete()
        {

            string sDel = "Delete from " + Table + " where " + sCondition;
            return sDel;
        }

        public void ExecututeSQL(string strSQL)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.Text;
                    objCmd.CommandText = strSQL;
                    objCmd.ExecuteNonQuery();
                }
            }

        }

      
        public void ExecuteRecord(string strSQL)
        {
            ExecututeSQL(strSQL);

        }


        
        public bool ExecuteTransactions(List<string> DB)
        {
            SqlTransaction transaction;
            SqlCommand com = new SqlCommand();
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {

                if (objCnn.State != System.Data.ConnectionState.Open)
                    objCnn.Open();
                transaction = objCnn.BeginTransaction();
                com.CommandTimeout = 6000;
                com.Connection = objCnn;
                com.Transaction = transaction;
                com.CommandType = System.Data.CommandType.Text;
                for (int j = 0; j <= DB.Count - 1; j++)
                {
                    if (DB[j] != null)
                    {
                        com.CommandText = DB[j];
                        com.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
                return true;

            }

        }
    }
}

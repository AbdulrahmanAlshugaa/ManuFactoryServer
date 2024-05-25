using System;
using System.Data;
using System.Configuration;
using System.Web;


using System.Net;
using System.IO;
using System.Text;
using System.Globalization;
using System.Data.SqlClient;
using Edex.DAL;
using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Edex.DAL.Common;
using Edex.Model;

/// <summary>
/// Summary description for Common
/// </summary>

public   class cOmex
{
    #region Declare
    private   Hashtable TempTable;
    private   NameValueCollection coll;
    private   string _table;
    private   int _ExcuteType = 0;
    private   string _sCondition;
    public   string FormatType;
    public   string[] PubArrParameters = new string[30];
    public   long GlobalBRANCH_ID;
    public   long GlobalUserID;
    public   iLanguage Language;
    public   string ProjectPath;
    public   string strSQL;
    public   string Table
    {
        get { return _table; }
        set { _table = value; }
    }
    public   int ExcuteType
    {
        get { return _ExcuteType; }
        set { _ExcuteType = value; }
    }
    public   string sCondition
    {
        get { return _sCondition; }
        set { _sCondition = value; }
    }
    public   void NewFields()
    {
        if (TempTable != null)
        {
            TempTable.Clear();
        }
    }
    public enum iLanguage
    {
        Arabic = 0,
        English = 1
    }
    #endregion
    /// <summary>
    /// This Function To Add Numeric field, This function is type Over Load
    /// </summary>
    /// <param name="Filedname"></param>
    /// <param name="FieldValue"></param>
    public   void AddNumericField(string Filedname, string FieldValue)
    {
        if (TempTable == null)
        {
            TempTable = new Hashtable();
        }
        if (coll == null)
        {
            coll = new NameValueCollection();
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
        coll.Add(Filedname, FieldValue);
    }
    /// <summary>
    /// This Function To Add Numeric field, This function is type Over Load
    /// </summary>
    /// <param name="Filedname"></param>
    /// <param name="FieldValue"></param>
    public   void AddNumericField(string Filedname, int FieldValue)
    {
        if (TempTable == null)
        {
            TempTable = new Hashtable();
        }
        if (coll == null)
        {
            coll = new NameValueCollection();
        }
        TempTable.Add(Filedname, FieldValue);
        coll.Add(Filedname, FieldValue.ToString());
    }
    /// <summary>
    /// This Function is used to Add Field type of String
    /// </summary>
    /// <param name="Filedname"></param>
    /// <param name="FieldValue"></param>
    public  void AddStringField(string Filedname, string FieldValue)
    {
        if (TempTable == null)
        {
            TempTable = new Hashtable();
        }

        if (FieldValue == string.Empty)
        {
            FieldValue = " ";
        }
        FieldValue = FieldValue.Replace("'", "�");
        TempTable.Add(Filedname, "'" + FieldValue + "'");

    }
   /// <summary>
   /// This Function To 
   /// </summary>
   /// <returns></returns>
    public   string GetInsertQuary()
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
    public   void ExecuteInsert()
    {
        if (ExcuteType == 0)
        {
            ExecututeSQL(GetInsertQuary());
        }
        else
        {
            ExecuteProcedure(Table, "");
        }
    }
    public   void ExecututeSQL(string StrSQL)
    {
        using (SqlConnection objCnn = new GlobalConnection().Conn)
        {
            objCnn.Open();
            using (SqlCommand objCmd = objCnn.CreateCommand())
            {
                objCmd.CommandType = System.Data.CommandType.Text;
                objCmd.CommandText = StrSQL;
                objCmd.ExecuteNonQuery();
            }
        }
    }
    public   int ExecuteProcedure(string procedureName, string ParmName = "")
    {
        int iReturnValue = 0;

        try
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = procedureName;
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                    objCmd.ExecuteNonQuery();
                    iReturnValue = 1;
                }
            }

        }
        catch
        {
            iReturnValue = 0;
        }

        return iReturnValue;
    }
    public   string GetUpdateStr()
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
    public   void ExecuteUpdate()
    {
        if (ExcuteType == 0)
        {
            ExecututeSQL(GetUpdateStr());
        }
        else
        {
            ExecuteProcedure(Table, "");
        }

    }
    public   void ExecuteDelete()
    {
        string sDel = "Delete from " + Table + " where " + sCondition;
        ExecututeSQL(sDel);
    }
   
    
    
   

     

}

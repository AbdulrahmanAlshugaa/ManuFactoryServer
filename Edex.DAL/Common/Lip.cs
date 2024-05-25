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
using Edex.Model;
using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Edex.DAL.Common;
using System.Linq;
using System.Collections.Generic;
 
using Edex.Model;
 
 
//using MySql.Data.MySqlClient;

/// <summary>
/// Summary description for Common
/// </summary>
public static class Lip
{

   public static List<string> NameOfTableDetails=new List<string>();
   public static List<string> NameMasterarray = new List<string>();
   public static void FileNameTableDetailToList()
   {
               //Details table Name
               NameOfTableDetails.Add("Sales_PurchaseInvoiceDetails");
                NameOfTableDetails.Add("Sales_PurchaseInvoiceReturnDetails");
                NameOfTableDetails.Add("Sales_PurchaseSaveInvoiceDetails");
                NameOfTableDetails.Add("Sales_PurchaseSaveInvoiceReturnDetails");
                 NameOfTableDetails.Add("Sales_SalesInvoiceDetails");
                NameOfTableDetails.Add("Sales_SalesInvoiceReturnDetails");
                 NameOfTableDetails.Add("Stc_ItemsInonBail_Details");
                NameOfTableDetails.Add("Stc_ItemsOutonBail_Details");
                NameOfTableDetails.Add("Stc_GoodOpeningDetails");


               //Master table Name      
                NameMasterarray.Add("Sales_PurchaseInvoiceMaster");
                NameMasterarray.Add("Sales_PurchaseInvoiceReturnMaster");
                NameMasterarray.Add( "Sales_PurchaseInvoiceSaveMaster");
                NameMasterarray.Add("Sales_PurchaseInvoiceSaveReturnMaster");
                NameMasterarray.Add( "Sales_SalesInvoiceMaster");
                NameMasterarray.Add( "Sales_SalesInvoiceReturnMaster");
                NameMasterarray.Add("Stc_ItemsInonBail_Master");
                NameMasterarray.Add("Stc_ItemsOutonBail_Master");
                NameMasterarray.Add("Stc_GoodOpeningMaster");
   }
    /// <summary>
    /// This function is used to Get Max ID Restrictions when add new Restrictions
    /// </summary>
    /// <param name="FormName"></param>
    /// <param name="GlobalBranchID"></param>
    /// <returns></returns>
    public static long GetNewRestrictionsID(string FormName, int GlobalBranchID)
    {
        try
        {
            long ID = 0;
            DataTable dt;
            string strSQL;

            strSQL = "SELECT Max(RegistrationNo)+1 FROM RestrictionsDaily Where  BranchNum =" + GlobalBranchID;
            dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                ID = Comon.cLong(dt.Rows[0][0].ToString());

            strSQL = "Select Top 1 StartFrom From StartNumbering Where BranchID=" + GlobalBranchID + " And FormName='" + FormName + "'";
            dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
            {
                if (Comon.cLong(dt.Rows[0]["StartFrom"].ToString()) > ID)
                    ID = (Comon.cLong(dt.Rows[0]["StartFrom"].ToString()));
            }

            return ID;
        }
        catch (Exception ex)
        {
            return 1;
        }
    }
    public static bool ExecuteTransaction(DAL_Trans[] DB)
    {
        bool valu = false;
        using (SqlConnection connection = new SqlConnection(new GlobalConnection().Conn.ConnectionString))
        {
            connection.Open();
            SqlTransaction transaction;
            transaction = connection.BeginTransaction();

            try
            {
                foreach (var item in DB)
                {
                    using (var command = new SqlCommand())
                    {
                        if (item.strSQL != string.Empty)
                        {
                            command.Connection = connection;
                            command.CommandText = item.strSQL;
                            command.Transaction = transaction;
                            command.ExecuteNonQuery();
                        }
                    }
                }
                transaction.Commit();
                valu = true;
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);
                try
                {
                    transaction.Rollback();
                    valu = false;
                }
                catch (Exception ex2)
                {
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                    valu = false;
                }
            }
        }
        return valu;
    }
    public static long SaveFile(byte[] image, string TableName, string FieldName, int BranchID, string where = "")
    {
        try
        {
            string query;
            query = "Update  " + TableName + "  Set " + FieldName + "=@p Where BranchID=" + BranchID;
            if (where != string.Empty)
                query = query + " And " + where;

            using (SqlConnection objCnn = new GlobalConnection().Conn)//create a new SqlConnection object to connect to the database
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())// create a new SqlCommand object to execute a SQL command on the database
                {
                    objCmd.CommandText = query;
                    objCmd.Parameters.AddWithValue("@p", image);
                    objCmd.ExecuteNonQuery();
                }
            }

            return 1;
        }


        catch (Exception e)
        {
            return 0;
        }
    }
    public enum DateInterval
    {
        Day,
        DayOfYear,
        Hour,
        Minute,
        Month,
        Quarter,
        Second,
        Weekday,
        WeekOfYear,
        Year
    }
    /// <summary>
    /// This method returns the difference between two dates based on the specified interval type
    /// intervalType: the interval type (day, hour, minute, second etc.)
    /// dateOne: the first date object
    /// dateTwo: the second date object
    /// </summary>
    /// <param name="intervalType"></param>
    /// <param name="dateOne"></param>
    /// <param name="dateTwo"></param>
    /// <returns></returns>
    public static long DateDiff(DateInterval intervalType, System.DateTime dateOne, System.DateTime dateTwo)
    {
        switch (intervalType)
        {
            case DateInterval.Day:
            case DateInterval.DayOfYear:
                System.TimeSpan spanForDays = dateTwo - dateOne;
                return (long)spanForDays.TotalDays;
            case DateInterval.Hour:
                System.TimeSpan spanForHours = dateTwo - dateOne;
                return (long)spanForHours.TotalHours;
            case DateInterval.Minute:
                System.TimeSpan spanForMinutes = dateTwo - dateOne;
                return (long)spanForMinutes.TotalMinutes;
            case DateInterval.Month:
                return ((dateTwo.Year - dateOne.Year) * 12) + (dateTwo.Month - dateOne.Month);
            case DateInterval.Quarter:
                long dateOneQuarter = (long)System.Math.Ceiling(dateOne.Month / 3.0);
                long dateTwoQuarter = (long)System.Math.Ceiling(dateTwo.Month / 3.0);
                return (4 * (dateTwo.Year - dateOne.Year)) + dateTwoQuarter - dateOneQuarter;
            case DateInterval.Second:
                System.TimeSpan spanForSeconds = dateTwo - dateOne;
                return (long)spanForSeconds.TotalSeconds;
            case DateInterval.Weekday:
                System.TimeSpan spanForWeekdays = dateTwo - dateOne;
                return (long)(spanForWeekdays.TotalDays / 7.0);
            case DateInterval.WeekOfYear:
                // Modify the dates to get the first day of the week, and then calculate the number of weeks between them
                System.DateTime dateOneModified = dateOne;
                System.DateTime dateTwoModified = dateTwo;
                while (dateTwoModified.DayOfWeek != System.Globalization.DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek)
                {
                    dateTwoModified = dateTwoModified.AddDays(-1);
                }
                while (dateOneModified.DayOfWeek != System.Globalization.DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek)
                {
                    dateOneModified = dateOneModified.AddDays(-1);
                }
                System.TimeSpan spanForWeekOfYear = dateTwoModified - dateOneModified;
                return (long)(spanForWeekOfYear.TotalDays / 7.0);
            case DateInterval.Year:
                return dateTwo.Year - dateOne.Year;
            default:
                return 0;
        }

    }
  
   
    /// <summary>
    /// This method checks the security level of a given form for a given user
    /// frm: the form name
    /// FACILITYID: the facility ID
    /// BRANCHID: the branch ID
    /// USER_NAME: the username
    /// FormView: an out parameter for whether the user has view access to the form
    /// FormDelete: an out parameter for whether the user has delete access to the form
    /// FormAdd: an out parameter for whether the user has add access to the form
    /// FormUpdate: an out parameter for whether the user has update access to the form
    /// ReportView: an out parameter for whether the user has view access to the reports of the form
    /// Error: an out parameter for whether an error occurred
    /// </summary>
    /// <param name="frm"></param>
    /// <param name="FACILITYID"></param>
    /// <param name="BRANCHID"></param>
    /// <param name="USER_NAME"></param>
    /// <param name="FormView"></param>
    /// <param name="FormDelete"></param>
    /// <param name="FormAdd"></param>
    /// <param name="FormUpdate"></param>
    /// <param name="ReportView"></param>
    /// <param name="Error"></param>
    /// <returns></returns>
    public static bool FormSecurity(string frm, string FACILITYID, string BRANCHID, string USER_NAME, out bool FormView,
    out bool FormDelete, out bool FormAdd, out bool FormUpdate, out bool ReportView, out bool Error)
    {
        bool functionReturnValue = false;


        string strSQL = null;
        DataTable dtMenu = new DataTable();
        bool ItsReportScreen = false;
        DataTable dt = new DataTable();

        ItsReportScreen = false;
        functionReturnValue = true;

        //------------------
        FormDelete = false; FormView = false; FormAdd = false; FormUpdate = true; ReportView = false;
        Error = false;
        //-------------------
        try
        {
            strSQL = "Select * From Branches";
            dt = SelectRecord(strSQL);
            if (dt.Rows.Count == 0)
            {
                FormDelete = true;
                FormView = true;
                FormAdd = true;
                FormUpdate = true;
                return functionReturnValue;
            }
            strSQL = "SELECT GROUPID FROM USERS WHERE USER_NAME='" + USER_NAME + "' AND FACILITYID=" + FACILITYID + " AND BRANCHID=" + BRANCHID;
            string GroupId = "1";
            //string GroupId = DB.GetValue(strSQL);
            if (GroupId == string.Empty)
                return functionReturnValue;

            strSQL = "SELECT * FROM GROUP_FORMS_PERMISSIONS WHERE GROUPID=" + GroupId + " AND FORMNAME='" + frm + "' AND FACILITYID=" + FACILITYID + " AND BRANCHID=" + BRANCHID;
            dt = SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
            {
                FormDelete = Comon.cbool(dt.Rows[0]["FormDelete"]);
                FormView = Comon.cbool(dt.Rows[0]["FormView"]);
                FormAdd = Comon.cbool(dt.Rows[0]["FormAdd"]);
                FormUpdate = Comon.cbool(dt.Rows[0]["FormUpdate"]);

                if (dt.Rows[0]["FormDelete"].ToString() == "0" & dt.Rows[0]["FormView"].ToString() == "0" & dt.Rows[0]["FormAdd"].ToString() == "0" & dt.Rows[0]["FormUpdate"].ToString() == "0")
                {
                    //Interaction.MsgBox((Language == iLanguage.Arabic ? "��� ���� �������� ������ ��� ��� ������" : "You Do Not Have Permission To Enter To This Screen"), (Language == iLanguage.Arabic ? MsgBoxStyle.Exclamation + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading : MsgBoxStyle.Exclamation), "");
                    //frm.visible = false;
                    //frm = null;
                    Error = true;
                    functionReturnValue = false;
                    return functionReturnValue;
                }
            }
            else
            {
                FormDelete = false;
                FormView = false;
                FormAdd = false;
                FormUpdate = false;
                ItsReportScreen = true;
            }
            //strSQL = "Select * From FORMS Where FORMNAME='" + frm + "'";
            strSQL = "SELECT FORMS_TEMPLATE.MENUNAME,FORMS_TEMPLATE.ARBCAPTION,FORMS_TEMPLATE.ENGCAPTION FROM FORMS INNER JOIN FORMS_TEMPLATE"
            + " ON FORMS.FORMNAME = FORMS_TEMPLATE.FORMNAME WHERE FORMS.FORMNAME = '" + frm + "'";
            dtMenu = SelectRecord(strSQL);
            if (dtMenu.Rows.Count > 0)
            {
                strSQL = "Select * From MENUS Where MENUNAME='" + dtMenu.Rows[0]["MENUNAME"] + "' AND MENUS.FACILITYID =" + FACILITYID + " AND MENUS.BRANCHID =" + BRANCHID;
                dt = SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["IS_CLIENT_PURCHASE_IT"].ToString() == "0")
                    {
                        //Interaction.MsgBox((Language == iLanguage.Arabic ? "��� ���� �������� ������ ��� ��� ������" : "You Do Not Have Permission To Enter To This Screen"), (Language == iLanguage.Arabic ? MsgBoxStyle.Exclamation + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading : MsgBoxStyle.Exclamation), "");
                        //frm.visible = false;
                        //frm = null;
                        Error = true;
                        functionReturnValue = false;
                        return functionReturnValue;
                    }
                }
            }
            #region Reports permission
            //strSQL = "Select Top 1 * From UserReportsPermissions Where UserID=" + GlobalUserID + " And ReportName='" + "rpt" + Strings.Mid(frm.Name, 4, 100) + "' And BranchID=" + GlobalBranchID;
            //dt = DB.SelectRecord(strSQL).Tables[0];
            //if (dt.Rows.Count > 0)
            //{
            //   ReportView =cbool ( dt.Rows[0]["ReportView"]);
            //    ReportExport = cbool (dt.Rows[0]["ReportExport"]);

            //}
            //else
            //{
            //    ReportView = false;
            //    ReportExport = false;

            //}



            //strSQL = "Select Top 1 * From Reports Where ReportName='rpt" + Strings.Mid(frm.Name, 4, 100) + "'";
            //dtMenu = DB.SelectRecord(strSQL).Tables[0];
            //if (dtMenu.Rows.Count > 0)
            //{
            //    strSQL = "Select Top 1 * From Menus Where MenuName='" + dtMenu.Rows[0]["MenuName") + "'";
            //    dt = DB.SelectRecord(strSQL).Tables[0];
            //    if (dt.Rows.Count > 0)
            //    {
            //        if (dt.Rows[0]["IsClientPurchaseIt") == 0)
            //        {
            //            Interaction.MsgBox((Language == iLanguage.Arabic ? "��� ���� �������� ������ ��� ��� ������" : "You Do Not Have Permission To Enter To This Screen"), (Language == iLanguage.Arabic ? MsgBoxStyle.Exclamation + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading : MsgBoxStyle.Exclamation), "");
            //            frm.visible = false;
            //            frm = null;
            //            functionReturnValue = false;
            //            return functionReturnValue;
            //        }
            //    }
            //}


            //if (ItsReportScreen == true & ReportView == false & ReportExport == false)
            //{
            //    //Interaction.MsgBox((Language == iLanguage.Arabic ? "��� ���� �������� ������ ��� ��� ������" : "You Do Not Have Permission To Enter To This Screen"), (Language == iLanguage.Arabic ? MsgBoxStyle.Exclamation + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading : MsgBoxStyle.Exclamation), "");
            //    //frm.visible = false;
            //    //frm = null;
            //    Error = true;
            //    functionReturnValue = false;
            //    return functionReturnValue;
            //}

            //if (ItsReportScreen == false & FormView == false & FormDelete == false & FormUpdate == false & FormAdd == false)
            //{
            //    //Interaction.MsgBox((Language == iLanguage.Arabic ? "��� ���� �������� ������ ��� ��� ������" : "You Do Not Have Permission To Enter To This Screen"), (Language == iLanguage.Arabic ? MsgBoxStyle.Exclamation + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading : MsgBoxStyle.Exclamation), "");
            //    //frm.visible = false;
            //    //frm = null;
            //    Error = true;
            //    functionReturnValue = false;
            //    return functionReturnValue;
            //}
            #endregion
            return functionReturnValue;
        }
        catch
        {
            Error = true;
            functionReturnValue = false;
            return functionReturnValue;
        }
    }
    public static Boolean CheckPermionParentAccoutID()
    {
        if (Comon.cDbl(Edex.Model.MySession.GlobalDefaultParentBanksAccountID) <= 0)
            return false;
        else if (Comon.cDbl(Edex.Model.MySession.GlobalDefaultParentBoxesAccountID) <= 0)
            return false;
        else if (Comon.cDbl(Edex.Model.MySession.GlobalDefaultParentCustomerAccountID) <= 0)
            return false;
        else if (Comon.cDbl(Edex.Model.MySession.GlobalDefaultParentStoreAccountID) <= 0)
            return false;
        else if (Comon.cDbl(Edex.Model.MySession.GlobalDefaultParentSupplierAccountID) <= 0)
            return false;
        else if (Comon.cDbl(Edex.Model.MySession.GlobalDefaultParentEmployeeAccountID) <= 0)
            return false;
        return true;
    }
    public static bool CheckAccountingTransactions(double AccountID)
        {
             DataTable dt = Lip.SelectRecord("SELECT * FROM  [Acc_VariousVoucherMachinDetails] where AccountID=" + Comon.cLong(AccountID)+" and BranchID="+Edex.Model.MySession.GlobalBranchID);
             if (dt.Rows.Count <= 0)
                 return true;
             return false;
        }
    /// <summary>
    /// This method for selecting records from a database table
    /// </summary>
    /// <param name="StrSQL"></param>
    /// <returns></returns>
    public static DataTable SelectRecord(string StrSQL)
    {
        using (SqlConnection objCnn = new GlobalConnection().Conn)//create a new SqlConnection object to connect to the database
        {
            objCnn.Open();
            using (SqlCommand objCmd = objCnn.CreateCommand())// create a new SqlCommand object to execute a SQL command on the database
            {
                objCmd.CommandType = System.Data.CommandType.Text;// set the command type to text
                objCmd.CommandText = StrSQL;// set the SQL command text to the provided StrSQL parameter
                SqlDataReader myreader = objCmd.ExecuteReader();// create a new SqlDataReader object to fill a DataTable with the results from the SQL command
                DataTable dt = new DataTable();// create a new DataTable object to hold the results
                dt.Load(myreader);// load the data from the DataTableReader into the DataTable
                return dt;
            }
        }
    }
   /// <summary>
    ///This function is used to  Check Name Not Duplicated With out ID
   /// </summary>
   /// <param name="PrimaryKey"></param>
   /// <param name="TableName"></param>
   /// <param name="FieldName"></param>
   /// <param name="FieldValue"></param>
   /// <returns></returns>
    public static string CheckNameNotDuplicatedWithoutBranchID(string PrimaryKey, string TableName, string FieldName, string FieldValue)
    {
        DataTable dt = new DataTable();

        string strSQL = "";

        strSQL = "Select " + PrimaryKey + " From " + TableName + " Where  " + FieldName + " ='" + FieldValue + "'";
        dt = SelectRecord(strSQL);
        if (dt.Rows.Count > 0)
        {
            return "Duplicated Name";
        }
        else
        {
            return "";
        }
    }
    /// <summary>
    /// This method for selecting records from a database table,and return Value
    /// </summary>
    /// <param name="StrSQL"></param>
    /// <returns>return the value with object type String </returns>
    public static string GetValue(string StrSQL)
    {
        using (SqlConnection objCnn = new GlobalConnection().Conn)//create a new SqlConnection object to connect to the database
        {
            objCnn.Open();
            using (SqlCommand objCmd = objCnn.CreateCommand())// create a new SqlCommand object to execute a SQL command on the database
            {
                objCmd.CommandType = System.Data.CommandType.Text;// set the command type to text
                objCmd.CommandText = StrSQL;// set the SQL command text to the provided StrSQL parameter
                SqlDataReader myreader = objCmd.ExecuteReader();// create a new SqlDataReader object to fill a DataTable with the results from the SQL command
                DataTable dt = new DataTable();// create a new DataTable object to hold the results
                dt.Load(myreader);// load the data from the DataTableReader into the DataTable
                try
                {
                    if (dt != null && dt.Rows.Count > 0)//Check   
                        return dt.Rows[0][0].ToString();// return value 
                    else
                        return "";
                }
                catch (Exception)
                {

                    return "";
                }

            }
        }
    }

    /// <summary>
    /// This method converts SQL string either to English or Arabic language based on given language parameter
    /// </summary>
    /// <param name="strSQL"></param>
    /// <param name="lang"></param>
    public static void ConvertStrSQLToEnglishOrArabicLanguage(string strSQL, string lang)
    {
        // If the user language is English, replace "ARBNAME" with "ENGNAME"
        if (UserInfo.Language.ToString() == iLanguage.English.ToString())
        {
            strSQL = strSQL.ToUpper().Replace("ARBNAME", "ENGNAME");

            // Replace "AccountArbName" with "ACCOUNTENGNAME"
            strSQL = strSQL.ToUpper().Replace("AccountArbName", "ACCOUNTENGNAME");
        }
    }

    /// <summary>
    ///This method converts SQL string either to English or Arabic language based on given language parameter
    /// </summary>
    /// <param name="strSQL"></param>
    /// <param name="lang"></param>
    /// <returns></returns>
    public static string ConvertStrSQLLanguage(string strSQL, string lang)
    {
        // If the user language is English, replace "ARBNAME" with "ENGNAME"
        if (UserInfo.Language.ToString() == iLanguage.English.ToString())
        {
            strSQL = strSQL.ToUpper().Replace("ARBNAME", "ENGNAME");
            // Replace "AccountArbName" with "ACCOUNTENGNAME"
            strSQL = strSQL.ToUpper().Replace("AccountArbName", "ACCOUNTENGNAME");
        }
        return strSQL;// return Stetement sql
    }
    
    /// <summary>
    /// This Function is used to Convert data table to list  any type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="table"></param>
    /// <returns></returns>
    public static List<T> ConvertDataTable<T>(this DataTable table) where T : class, new()
    {
        try
        {
            List<T> list = new List<T>();
            foreach (var row in table.AsEnumerable())
            {
                T obj = new T();
                foreach (var prop in obj.GetType().GetProperties())
                {
                    try
                    {
                        PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                        propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                    }
                    catch
                    {
                        continue;
                    }
                }
                list.Add(obj);
            }
            return list;
        }
        catch
        {
            return null;
        }
    }

    //public static decimal GetItemQtyReminder(string BarCode, int StoreID, string BRANCHID, string FACILITYID)
    //{
    //    decimal functionReturnValue = 0;

    //    try
    //    {
    //        DataTable dt = new DataTable();
    //        string strSQL = null;
    //        decimal SumQty = 0;
    //        strSQL = "SELECT SUM(Sales_PurchaseInvoiceDetails.QTY + Sales_PurchaseInvoiceDetails.BONES) AS QTY FROM Sales_PurchaseInvoiceDetails LEFT OUTER JOIN Sales_PurchaseInvoiceMaster ON Sales_PurchaseInvoiceDetails.BRANCHID"
    //        + " = Sales_PurchaseInvoiceMaster.BRANCHID AND Sales_PurchaseInvoiceDetails.INVOICEID= Sales_PurchaseInvoiceMaster.INVOICEID AND Sales_PurchaseInvoiceDetails.FACILITYID = Sales_PurchaseInvoiceMaster.FACILITYID "
    //        + " WHERE Sales_PurchaseInvoiceDetails.BARCODE = '" + BarCode + "' AND Sales_PurchaseInvoiceDetails.BRANCHID =" + BRANCHID + " AND Sales_PurchaseInvoiceDetails.FACILITYID =" + FACILITYID
    //        + " AND Sales_PurchaseInvoiceDetails.STOREID =" + StoreID + " AND Sales_PurchaseInvoiceDetails.CANCEL = 0 AND Sales_PurchaseInvoiceMaster.INVOICEID >= 0";

    //        dt = SelectRecord(strSQL);
    //        if (dt.Rows.Count > 0)
    //        {
    //            SumQty = Comon.ConvertToDecimalQty(SumQty + Comon.cDec((dt.Rows[0]["QTY"].ToString())));
    //        }


    //        strSQL = "SELECT SUM(Sales_SalesInvoiceDetails.QTY + Sales_SalesInvoiceDetails.BONES) AS QTY FROM Sales_SalesInvoiceDetails LEFT OUTER JOIN Sales_SalesInvoiceMaster"
    //        + " ON Sales_SalesInvoiceDetails.INVOICEID = Sales_SalesInvoiceMaster.INVOICEID AND Sales_SalesInvoiceDetails.BRANCHID = Sales_SalesInvoiceMaster.BRANCHID"
    //        + " AND Sales_SalesInvoiceDetails.FACILITYID = Sales_SalesInvoiceMaster.FACILITYID WHERE Sales_SalesInvoiceMaster.CANCEL = 0 AND Sales_SalesInvoiceDetails.BARCODE"
    //        + " = '" + BarCode + "' AND Sales_SalesInvoiceDetails.BRANCHID =" + BRANCHID + " AND Sales_SalesInvoiceDetails.FACILITYID =" + FACILITYID + " AND Sales_SalesInvoiceDetails.STOREID= 1";
     

    //        dt = SelectRecord(strSQL);
    //        if (dt.Rows.Count > 0)
    //        {
    //            SumQty = Comon.ConvertToDecimalQty(SumQty - Comon.cDec((dt.Rows[0]["QTY"].ToString())));
    //        }

    //        //SelectQtyFromPurchaseInvoiceReturn
    //        //strSQL = "SELECT SUM(Sales_PurchaseInvoiceReturnDetails.QTY + Sales_PurchaseInvoiceReturnDetails.Bones) AS QTY " + " FROM Sales_PurchaseInvoiceReturnDetails LEFT OUTER JOIN Sales_PurchaseInvoiceReturnMaster ON " + " Sales_PurchaseInvoiceReturnDetails.BranchID = Sales_PurchaseInvoiceReturnMaster.BranchID AND " + " Sales_PurchaseInvoiceReturnDetails.InvoiceID = Sales_PurchaseInvoiceReturnMaster.InvoiceID" + " WHERE (Sales_PurchaseInvoiceReturnDetails.BranchID = " + WT.GlobalBranchID + ") AND (Sales_PurchaseInvoiceReturnMaster.Cancel = 0)" + " AND (Sales_PurchaseInvoiceReturnDetails.BarCode = '" + BarCode + "') AND (Sales_PurchaseInvoiceReturnMaster.StoreID = " + StoreID + ")";
    //        strSQL = "SELECT SUM(Sales_PurchaseInvReturnDetails.QTY + Sales_PurchaseInvReturnDetails.BONES) AS QTY FROM Sales_PurchaseInvReturnDetails LEFT"
    //        + " OUTER JOIN Sales_PurchaseInvReturnMaster ON Sales_PurchaseInvReturnDetails.BRANCHID = Sales_PurchaseInvReturnMaster.BRANCHID AND "
    //        + " Sales_PurchaseInvReturnDetails.INVOICEID = Sales_PurchaseInvReturnMaster.INVOICEID AND Sales_PurchaseInvReturnDetails.FACILITYID ="
    //        + " Sales_PurchaseInvReturnMaster.FACILITYID WHERE Sales_PurchaseInvReturnDetails.BRANCHID =" + BRANCHID + " AND Sales_PurchaseInvReturnDetails.FACILITYID =" + FACILITYID
    //        + " AND Sales_PurchaseInvReturnMaster.CANCEL = 0 AND Sales_PurchaseInvReturnDetails.BARCODE = '" + BarCode + "' AND Sales_PurchaseInvReturnMaster.STOREID =" + StoreID;
    //        dt = SelectRecord(strSQL);
    //        if (dt.Rows.Count > 0)
    //        {
    //            SumQty = Comon.ConvertToDecimalQty(SumQty - Comon.cDec((dt.Rows[0]["QTY"].ToString())));
    //        }

    //        //SelectQtyFromSalesInvoiceReturn
    //        //strSQL = "SELECT SUM(Sales_SalesInvoiceReturnDetails.QTY + Sales_SalesInvoiceReturnDetails.Bones) AS QTY" + " FROM Sales_SalesInvoiceReturnDetails LEFT OUTER JOIN Sales_SalesInvoiceReturnMaster ON Sales_SalesInvoiceReturnDetails.BranchID =" + " Sales_SalesInvoiceReturnMaster.BranchID AND Sales_SalesInvoiceReturnDetails.InvoiceID = Sales_SalesInvoiceReturnMaster.InvoiceID " + " WHERE (Sales_SalesInvoiceReturnDetails.BarCode = '" + BarCode + "') AND (Sales_SalesInvoiceReturnDetails.BranchID = " + WT.GlobalBranchID + ") AND " + " (Sales_SalesInvoiceReturnMaster.Cancel = 0) AND (Sales_SalesInvoiceReturnMaster.StoreID = " + StoreID + ")";
    //        strSQL = "SELECT SUM(Sales_PurchaseInvReturnDetails.QTY + Sales_PurchaseInvReturnDetails.BONES) AS QTY FROM Sales_PurchaseInvReturnDetails LEFT OUTER JOIN"
    //        + " Sales_PurchaseInvReturnMaster ON Sales_PurchaseInvReturnDetails.BRANCHID= Sales_PurchaseInvReturnMaster.BRANCHID AND Sales_PurchaseInvReturnDetails.INVOICEID"
    //        + " = Sales_PurchaseInvReturnMaster.INVOICEID AND Sales_PurchaseInvReturnDetails.FACILITYID = Sales_PurchaseInvReturnMaster.FACILITYID WHERE"
    //        + " Sales_PurchaseInvReturnDetails.BARCODE = '" + BarCode + "' AND Sales_PurchaseInvReturnDetails.BRANCHID =" + BRANCHID + " AND Sales_PurchaseInvReturnDetails.FACILITYID =" + FACILITYID
    //        + " AND Sales_PurchaseInvReturnMaster.CANCEL = 0 AND Sales_PurchaseInvReturnMaster.STOREID =" + StoreID;
    //        dt = SelectRecord(strSQL);
    //        if (dt.Rows.Count > 0)
    //        {
    //            SumQty = Comon.ConvertToDecimalQty(SumQty + Comon.cDec((dt.Rows[0]["QTY"].ToString())));
    //        }

    //        //Select QtyFromItemsTransfer_FromStore
    //        //strSQL = "SELECT SUM(Stc_ItemsTransferDetails.QTY) AS QTY FROM Stc_ItemsTransferMaster INNER JOIN " + " Stc_ItemsTransferDetails ON Stc_ItemsTransferMaster.TransferID = Stc_ItemsTransferDetails.TransferID " + " WHERE (Stc_ItemsTransferMaster.Cancel = 0)" + " AND (Stc_ItemsTransferMaster.FromStoreID = " + StoreID + ") AND (Stc_ItemsTransferDetails.BarCode = '" + BarCode + "') AND " + " (Stc_ItemsTransferMaster.FromBranchID = " + WT.GlobalBranchID + ")";
    //        strSQL = "SELECT SUM(Stc_ItemsTransferDetails.QTY) AS QTY FROM Stc_ItemsTransferMaster INNER JOIN Stc_ItemsTransferDetails ON Stc_ItemsTransferMaster.TRANSFERID"
    //        + " = Stc_ItemsTransferDetails.TRANSFERID AND Stc_ItemsTransferMaster.FACILITYID = Stc_ItemsTransferDetails.FACILITYID WHERE Stc_ItemsTransferMaster.CANCEL= 0"
    //        + " AND Stc_ItemsTransferMaster.FROMSTOREID =" + StoreID + " AND Stc_ItemsTransferMaster.FACILITYID =" + FACILITYID + " AND Stc_ItemsTransferDetails.BARCODE = '" + BarCode + "'"
    //        + " AND Stc_ItemsTransferMaster.FROMBRANCHID =" + BRANCHID;


    //        dt = SelectRecord(strSQL);
    //        if (dt.Rows.Count > 0)
    //        {
    //            SumQty = Comon.ConvertToDecimalQty(SumQty - Comon.cDec((dt.Rows[0]["QTY"].ToString())));
    //        }

    //        //SelectQtyFromItemsTransfer_ToStore
    //        //strSQL = "SELECT SUM(Stc_ItemsTransferDetails.QTY) AS QTY FROM Stc_ItemsTransferMaster INNER JOIN " + " Stc_ItemsTransferDetails ON Stc_ItemsTransferMaster.TransferID = Stc_ItemsTransferDetails.TransferID " + " WHERE (Stc_ItemsTransferMaster.Cancel = 0)" + " AND (Stc_ItemsTransferMaster.ToStoreID = " + StoreID + ") AND (Stc_ItemsTransferDetails.BarCode = '" + BarCode + "') AND " + " (Stc_ItemsTransferMaster.ToBranchID = " + WT.GlobalBranchID + ")";
    //        strSQL = "SELECT SUM(Stc_ItemsTransferDetails.QTY) AS QTY FROM Stc_ItemsTransferMaster INNER JOIN Stc_ItemsTransferDetails ON Stc_ItemsTransferMaster.TRANSFERID ="
    //        + " Stc_ItemsTransferDetails.TRANSFERID AND Stc_ItemsTransferDetails.FACILITYID = Stc_ItemsTransferMaster.FACILITYID WHERE Stc_ItemsTransferMaster.CANCEL= 0"
    //        + " AND Stc_ItemsTransferMaster.TOSTOREID=" + StoreID + " AND Stc_ItemsTransferDetails.BARCODE = '" + BarCode + "' AND Stc_ItemsTransferMaster.TOBRANCHID =" + BRANCHID
    //        + " AND Stc_ItemsTransferMaster.FACILITYID =" + FACILITYID;


    //        dt = SelectRecord(strSQL);
    //        if (dt.Rows.Count > 0)
    //        {
    //            SumQty = Comon.ConvertToDecimalQty(SumQty + Comon.cDec((dt.Rows[0]["QTY"].ToString())));
    //        }

    //        strSQL = "SELECT SUM(Stc_ItemsDismantlingDetails.QTY) AS QTY FROM Stc_ItemsDismantlingDetails LEFT OUTER JOIN Stc_ItemsDismantlingMaster ON Stc_ItemsDismantlingDetails.DISMANTLEID"
    //        + " = Stc_ItemsDismantlingMaster.DISMANTLEID AND Stc_ItemsDismantlingDetails.BRANCHID = Stc_ItemsDismantlingMaster.BRANCHID AND Stc_ItemsDismantlingMaster.FACILITYID"
    //        + " = Stc_ItemsDismantlingDetails.FACILITYID WHERE Stc_ItemsDismantlingMaster.CANCEL = 0 AND Stc_ItemsDismantlingDetails.FROMBARCODE = '" + BarCode + "'"
    //        + " AND Stc_ItemsDismantlingMaster.STOREID =" + StoreID + " AND Stc_ItemsDismantlingMaster.BRANCHID =" + BRANCHID + " AND Stc_ItemsDismantlingMaster.FACILITYID =" + FACILITYID;

    //        dt = SelectRecord(strSQL);
    //        if (dt.Rows.Count > 0)
    //        {
    //            SumQty = Comon.ConvertToDecimalQty(SumQty - Comon.cDec((dt.Rows[0]["QTY"].ToString())));
    //        }

    //        //SelectQtyFromItemsDismantling_Assembly
    //        //strSQL = "SELECT SUM(Stc_ItemsDismantlingDetails.DismantledQTY) AS QTY FROM Stc_ItemsDismantlingDetails LEFT OUTER JOIN" + " Stc_ItemsDismantlingMaster ON Stc_ItemsDismantlingDetails.DismantleID = Stc_ItemsDismantlingMaster.DismantleID AND " + " Stc_ItemsDismantlingDetails.BranchID = Stc_ItemsDismantlingMaster.BranchID WHERE (Stc_ItemsDismantlingMaster.Cancel = 0)" + " AND (Stc_ItemsDismantlingDetails.ToBarCode = '" + BarCode + "') AND (Stc_ItemsDismantlingMaster.StoreID = " + StoreID + ") AND" + " (Stc_ItemsDismantlingMaster.BranchID = " + WT.GlobalBranchID + ")";
    //        strSQL = "SELECT SUM(Stc_ItemsDismantlingDetails.DISMANTLEDQTY) AS QTY FROM Stc_ItemsDismantlingDetails LEFT OUTER JOIN Stc_ItemsDismantlingMaster ON Stc_ItemsDismantlingDetails.DISMANTLEID"
    //        + " = Stc_ItemsDismantlingMaster.DISMANTLEID AND Stc_ItemsDismantlingDetails.BRANCHID = Stc_ItemsDismantlingMaster.BRANCHID AND Stc_ItemsDismantlingDetails.FACILITYID = "
    //        + " Stc_ItemsDismantlingMaster.FACILITYID WHERE Stc_ItemsDismantlingMaster.CANCEL = 0 AND Stc_ItemsDismantlingDetails.TOBARCODE = '" + BarCode + "'"
    //        + " AND Stc_ItemsDismantlingMaster.STOREID =" + StoreID + " AND Stc_ItemsDismantlingMaster.BRANCHID=" + BRANCHID + " AND Stc_ItemsDismantlingMaster.FACILITYID =" + FACILITYID;
    //        dt = SelectRecord(strSQL);

    //        if (dt.Rows.Count > 0)
    //        {
    //            SumQty = Comon.ConvertToDecimalQty(SumQty + Comon.cDec((dt.Rows[0]["QTY"].ToString())));
    //        }


    //        //SelectQtyFromManu_ManufacturingOperations_Details �������
    //        //strSQL = "SELECT SUM(Manu_ManufacturingOperations_Details.QTY) AS Qty " + " FROM Manu_ManufacturingOperations_Details LEFT OUTER JOIN" + " Manu_ManufacturingOperations_Master ON Manu_ManufacturingOperations_Details.OperationID = Manu_ManufacturingOperations_Master.OperationID" + " WHERE Manu_ManufacturingOperations_Master.Cancel = 0 AND ParentBarCode = '" + BarCode + "' AND Manu_ManufacturingOperations_Master.StoreID = " + StoreID + " AND" + " Manu_ManufacturingOperations_Master.BranchID = " + WT.GlobalBranchID;
    //        //ds = DB.SelectRecord(strSQL);
    //        //if (ds != null)
    //        //{
    //        //    dt = ds.Tables[0];
    //        //    if (dt.Rows.Count > 0)
    //        //    {
    //        //        SumQty = ConvertToDecimalQty(SumQty + cDec((dt.Rows[0]["QTY"]).ToString()));
    //        //    }
    //        //}

    //        //Select SelectQtyFromManu_ManufacturingOperations_StuffDetails
    //        //strSQL = "SELECT Sum(Manu_ManufacturingOperations_StuffDetails.QTY) As Qty" + " FROM Manu_ManufacturingOperations_StuffDetails LEFT OUTER JOIN" + " Manu_ManufacturingOperations_Master ON " + " Manu_ManufacturingOperations_StuffDetails.OperationID = Manu_ManufacturingOperations_Master.OperationID And " + " Manu_ManufacturingOperations_StuffDetails.BranchID = Manu_ManufacturingOperations_Master.BranchID" + " WHERE Manu_ManufacturingOperations_Master.Cancel = 0 AND Manu_ManufacturingOperations_StuffDetails.BarCode = '" + BarCode + "' AND Manu_ManufacturingOperations_Master.StoreID = " + StoreID + " AND" + " Manu_ManufacturingOperations_Master.BranchID = " + WT.GlobalBranchID;
    //        //ds = DB.SelectRecord(strSQL);
    //        //if (ds != null)
    //        //{
    //        //    dt = ds.Tables[0];
    //        //    if (dt.Rows.Count > 0)
    //        //    {
    //        //        SumQty = ConvertToDecimalQty(SumQty - cDec((dt.Rows[0]["QTY"].ToString())));
    //        //    }
    //        //}

    //        //Select SelectQtyFromItemsInOnBail
    //        //strSQL = "SELECT SUM(Stc_ItemsInonBail_Details.QTY ) AS QTY" + " FROM Stc_ItemsInonBail_Details LEFT OUTER JOIN Stc_ItemsInonBail_Master ON Stc_ItemsInonBail_Details.BranchID" + " = Stc_ItemsInonBail_Master.BranchID AND Stc_ItemsInonBail_Details.InID = Stc_ItemsInonBail_Master.InID" + " WHERE (Stc_ItemsInonBail_Details.BarCode = '" + BarCode + "') AND (Stc_ItemsInonBail_Details.BranchID = " + WT.GlobalBranchID + ") AND" + " (Stc_ItemsInonBail_Master.StoreID = " + StoreID + ") AND (Stc_ItemsInonBail_Master.Cancel = 0) ";
    //        strSQL = "SELECT SUM(Stc_ItemsInonBail_Details.QTY) AS QTY FROM Stc_ItemsInonBail_Details LEFT OUTER JOIN Stc_ItemsInonBail_Master ON Stc_ItemsInonBail_Details.BRANCHID"
    //        + " = Stc_ItemsInonBail_Master.BRANCHID AND Stc_ItemsInonBail_Details.INID = Stc_ItemsInonBail_Master.INID AND Stc_ItemsInonBail_Details.FACILITYID = "
    //        + " Stc_ItemsInonBail_Master.FACILITYID WHERE Stc_ItemsInonBail_Details.BARCODE = '" + BarCode + "' AND Stc_ItemsInonBail_Details.BRANCHID=" + BRANCHID
    //        + " AND Stc_ItemsInonBail_Details.FACILITYID =" + FACILITYID + " AND Stc_ItemsInonBail_Master.STOREID=" + StoreID + " AND Stc_ItemsInonBail_Master.CANCEL = 0";



    //        dt = SelectRecord(strSQL);
    //        if (dt.Rows.Count > 0)
    //        {
    //            SumQty = Comon.ConvertToDecimalQty(SumQty + Comon.cDec((dt.Rows[0]["QTY"].ToString())));
    //        }

    //        //Select SelectQtyFromItemsOutonBail
    //        //strSQL = "SELECT SUM(Stc_ItemsOutonBail_Details.QTY ) AS QTY" + " FROM Stc_ItemsOutonBail_Details LEFT OUTER JOIN Stc_ItemsOutonBail_Master ON Stc_ItemsOutonBail_Details.BranchID" + " = Stc_ItemsOutonBail_Master.BranchID AND Stc_ItemsOutonBail_Details.OutID = Stc_ItemsOutonBail_Master.OutID" + " WHERE (Stc_ItemsOutonBail_Details.BarCode = '" + BarCode + "') AND (Stc_ItemsOutonBail_Details.BranchID = " + WT.GlobalBranchID + ") AND" + " (Stc_ItemsOutonBail_Master.StoreID = " + StoreID + ") AND (Stc_ItemsOutonBail_Master.Cancel = 0) ";
    //        strSQL = "SELECT SUM(Stc_ItemsOutonBail_Details.QTY) AS QTY FROM Stc_ItemsOutonBail_Details LEFT OUTER JOIN Stc_ItemsOutonBail_Master ON Stc_ItemsOutonBail_Details.BRANCHID"
    //        + " = Stc_ItemsOutonBail_Master.BRANCHID AND Stc_ItemsOutonBail_Details.OUTID= Stc_ItemsOutonBail_Master.OUTID AND Stc_ItemsOutonBail_Details.FACILITYID ="
    //        + " Stc_ItemsOutonBail_Master.FACILITYID WHERE Stc_ItemsOutonBail_Details.BARCODE = '" + BarCode + "' AND Stc_ItemsOutonBail_Details.BRANCHID =" + BRANCHID
    //        + " AND Stc_ItemsOutonBail_Details.FACILITYID =" + FACILITYID + " AND Stc_ItemsOutonBail_Master.STOREID =" + StoreID + " AND Stc_ItemsOutonBail_Master.CANCEL = 0";

    //        dt = SelectRecord(strSQL);
    //        if (dt.Rows.Count > 0)
    //        {
    //            SumQty = Comon.ConvertToDecimalQty(SumQty - Comon.cDec((dt.Rows[0]["QTY"].ToString())));
    //        }


    //        //SelectQtyFromItemsSpecialOffers
    //        DataTable dtOffers = default(DataTable);
    //        DataTable dtQty = default(DataTable);
    //        int j = 0;

    //        //��� ����� ���� ������ ������ ���� ����� ��� ������ ���� ���� ������
    //        //strSQL = "SELECT Sales_SpecialOffersDetails.OfferID, Sales_SpecialOffersDetails.QTY " + " FROM Sales_SpecialOffersMaster RIGHT OUTER JOIN " + " Sales_SpecialOffersDetails ON " + " Sales_SpecialOffersMaster.OfferID = Sales_SpecialOffersDetails.OfferID" + " WHERE (Sales_SpecialOffersDetails.BarCode = '" + BarCode + " ') AND (Sales_SpecialOffersMaster.StoreID = " + StoreID + ")";
    //        strSQL = "SELECT Sales_SpecialOffersDetails.OFFERID,Sales_SpecialOffersDetails.QTY FROM Sales_SpecialOffersMaster RIGHT OUTER JOIN Sales_SpecialOffersDetails"
    //        + " ON Sales_SpecialOffersMaster.OFFERID= Sales_SpecialOffersDetails.OFFERID AND Sales_SpecialOffersDetails.FACILITYID = Sales_SpecialOffersMaster.FACILITYID"
    //        + " WHERE Sales_SpecialOffersDetails.BARCODE   = '" + BarCode + "' AND Sales_SpecialOffersMaster.STOREID =" + StoreID + " AND Sales_SpecialOffersDetails.FACILITYID =" + FACILITYID;
    //        ConvertStrSQLToEnglishOrArabicLanguage(strSQL, "Eng");


    //        dtOffers = SelectRecord(strSQL);

    //        for (j = 0; j <= dtOffers.Rows.Count - 1; j++)
    //        {
    //            //��� ��� �������� �� ����� ����� ����� ����� ��� ������ ������� ����� � ���� ���� ����� ������ ������� �� ��� ������ �������� ���� ����� �����
    //            //strSQL = "SELECT SUM(QTY + Bones) AS SalesQTY" + " FROM Sales_SalesInvoiceDetails LEFT OUTER JOIN" + " Sales_SalesInvoiceMaster ON Sales_SalesInvoiceDetails.InvoiceID = Sales_SalesInvoiceMaster.InvoiceID AND " + " Sales_SalesInvoiceDetails.BranchID = Sales_SalesInvoiceMaster.BranchID" + " WHERE Sales_SalesInvoiceMaster.Cancel=0 And Sales_SalesInvoiceDetails.BarCode = '" + dtOffers.Rows(j)("OfferID") + " ' AND Sales_SalesInvoiceMaster.BranchID = " + WT.GlobalBranchID + " AND Sales_SalesInvoiceMaster.StoreID = " + StoreID;
    //            strSQL = "SELECT SUM(Sales_SalesInvoiceDetails.QTY + Sales_SalesInvoiceDetails.BONES) AS SalesQTY FROM Sales_SalesInvoiceDetails LEFT OUTER JOIN Sales_SalesInvoiceMaster"
    //            + " ON Sales_SalesInvoiceDetails.INVOICEID = Sales_SalesInvoiceMaster.INVOICEID AND Sales_SalesInvoiceDetails.BRANCHID = Sales_SalesInvoiceMaster.BRANCHID"
    //            + " AND Sales_SalesInvoiceDetails.FACILITYID = Sales_SalesInvoiceMaster.FACILITYID WHERE Sales_SalesInvoiceMaster.CANCEL= 0 AND Sales_SalesInvoiceDetails.BARCODE"
    //            + " = '" + dtOffers.Rows[j]["OfferID"] + "' AND Sales_SalesInvoiceMaster.BRANCHID =" + BRANCHID + " AND Sales_SalesInvoiceMaster.FACILITYID =" + FACILITYID + " AND Sales_SalesInvoiceMaster.STOREID=" + StoreID;


    //            dtQty = SelectRecord(strSQL);

    //            if (!string.IsNullOrEmpty(dtQty.Rows[0]["SalesQTY"].ToString()))
    //            {
    //                SumQty = Comon.ConvertToDecimalQty(SumQty) - (Comon.ConvertToDecimalQty(dtQty.Rows[0]["SalesQTY"]) * Comon.ConvertToDecimalQty(dtOffers.Rows[j]["Qty"]));
    //            }

    //            //��� ��� ����� �������� �� ����� ����� ����� ����� ��� ������ ������� ����� � ���� ���� ����� ������ �������� �� ��� ������ �������� ���� ����� �����
    //            //strSQL = "SELECT SUM(QTY + Bones) AS SalesQTY" + " FROM Sales_SalesInvoiceReturnDetails LEFT OUTER JOIN" + " Sales_SalesInvoiceReturnMaster ON Sales_SalesInvoiceReturnDetails.InvoiceID = Sales_SalesInvoiceReturnMaster.InvoiceID AND " + " Sales_SalesInvoiceReturnDetails.BranchID = Sales_SalesInvoiceReturnMaster.BranchID" + " WHERE Sales_SalesInvoiceReturnMaster.Cancel=0 And Sales_SalesInvoiceReturnDetails.BarCode = '" + dtOffers.Rows(j)("OfferID") + " ' AND Sales_SalesInvoiceReturnMaster.BranchID = " + WT.GlobalBranchID + " AND Sales_SalesInvoiceReturnMaster.StoreID = " + StoreID;
    //            strSQL = "SELECT SUM(Sales_PurchaseInvReturnDetails.QTY + Sales_PurchaseInvReturnDetails.BONES) AS SalesQTY FROM Sales_PurchaseInvReturnDetails LEFT OUTER JOIN Sales_PurchaseInvReturnMaster"
    //            + " ON Sales_PurchaseInvReturnDetails.INVOICEID = Sales_PurchaseInvReturnMaster.INVOICEID AND Sales_PurchaseInvReturnDetails.BRANCHID = Sales_PurchaseInvReturnMaster.BRANCHID"
    //            + " AND Sales_PurchaseInvReturnDetails.FACILITYID = Sales_PurchaseInvReturnMaster.FACILITYID WHERE Sales_PurchaseInvReturnMaster.CANCEL = 0 AND Sales_PurchaseInvReturnDetails.BARCODE"
    //            + " = '" + dtOffers.Rows[j]["OfferID"] + "' AND Sales_PurchaseInvReturnMaster.BRANCHID=" + BRANCHID + " AND Sales_PurchaseInvReturnMaster.FACILITYID =" + FACILITYID + " AND Sales_PurchaseInvReturnMaster.STOREID=" + StoreID;


    //            dtQty = SelectRecord(strSQL);

    //            if (!string.IsNullOrEmpty(dtQty.Rows[0]["SalesQTY"].ToString()))
    //            {
    //                SumQty = Comon.ConvertToDecimalQty(SumQty) + (Comon.ConvertToDecimalQty(dtQty.Rows[0]["SalesQTY"]) * Comon.ConvertToDecimalQty(dtOffers.Rows[j]["Qty"]));
    //            }
    //        }


    //        //SelectQtyFromItemsInsurance
    //        //strSQL = "SELECT SUM(Res_ItemsInsurance_Details.QTY ) AS ItemsInsuranceQTY" + " FROM Res_ItemsInsurance_Details LEFT OUTER JOIN" + " Res_ItemsInsurance_Master ON Res_ItemsInsurance_Details.InsuranceID = Res_ItemsInsurance_Master.InsuranceID AND " + " Res_ItemsInsurance_Details.BranchID = Res_ItemsInsurance_Master.BranchID" + " WHERE Res_ItemsInsurance_Master.Cancel=0 And (Res_ItemsInsurance_Details.BarCode = '" + BarCode + " ') AND (Res_ItemsInsurance_Details.BranchID = " + WT.GlobalBranchID + ") AND (Res_ItemsInsurance_Details.StoreID = " + StoreID + ")";
    //        //ds = DB.SelectRecord(strSQL);
    //        //if (ds != null)
    //        //{
    //        //    dt = ds.Tables[0];
    //        //    if (dt.Rows.Count > 0)
    //        //    {
    //        //        SumQty = ConvertToDecimalQty(SumQty - cDec((dt.Rows[0]["ItemsInsuranceQTY"])));
    //        //    }
    //        //}

    //        //SelectQtyFromItemsInsuranceReturn
    //        //strSQL = "SELECT SUM(Res_ItemsInsuranceReturn_Details.QTY ) AS ItemsInsuranceReturnQTY" + " FROM Res_ItemsInsuranceReturn_Details LEFT OUTER JOIN" + " Res_ItemsInsuranceReturn_Master ON Res_ItemsInsuranceReturn_Details.InsuranceID = Res_ItemsInsuranceReturn_Master.InsuranceID AND " + " Res_ItemsInsuranceReturn_Details.BranchID = Res_ItemsInsuranceReturn_Master.BranchID" + " WHERE Res_ItemsInsuranceReturn_Master.Cancel=0 And (Res_ItemsInsuranceReturn_Details.BarCode = '" + BarCode + " ') AND (Res_ItemsInsuranceReturn_Details.BranchID = " + WT.GlobalBranchID + ") AND (Res_ItemsInsuranceReturn_Details.StoreID = " + StoreID + ")";
    //        //ds = DB.SelectRecord(strSQL);
    //        //if (ds != null)
    //        //{
    //        //    dt = ds.Tables[0];
    //        //    if (dt.Rows.Count > 0)
    //        //    {
    //        //        SumQty = ConvertToDecimalQty(SumQty + cDec((dt.Rows[0]["ItemsInsuranceReturnQTY"])));
    //        //        // ********ItemsInsuranceReturnQTY
    //        //    }
    //        //}
    //        functionReturnValue = SumQty;

    //    }
    //    catch { }

    //    return functionReturnValue;
    //}
    //public static bool CheckIfValueCellIsPositive(ref System.Web.UI.WebControls.GridView GridView, int Row, int ColumnNameIndex, bool ShouldEnter = false)
    //{
    //    bool functionReturnValue = false;

    //    functionReturnValue = false;
    //    //GridView.ClearSelection();
    //    if (ShouldEnter == true)
    //    {
    //        if (GridView.Rows[Row].Cells[ColumnNameIndex].Text == "")
    //        {
    //            GridView.Rows[Row].Cells[ColumnNameIndex].Focus();
    //            //MessageBox.Show((Language == iLanguage.English ? "You must Enter Positive Number Inside The Selected Cell In Grid View" : "��� ����� ��� ���� ���� ����� ������ �� ������"));
    //            return functionReturnValue;
    //        }
    //        else
    //        {
    //            if (IsNumeric(GridView.Rows[Row].Cells[ColumnNameIndex].Text) == true)
    //            {
    //                if (Comon.cDec(GridView.Rows[Row].Cells[ColumnNameIndex].Text) <= 0)
    //                {
    //                    GridView.Rows[Row].Cells[ColumnNameIndex].Focus();
    //                    return functionReturnValue;
    //                }
    //            }
    //            else
    //            {
    //                GridView.Rows[Row].Cells[ColumnNameIndex].Focus();
    //                return functionReturnValue;
    //            }
    //        }

    //    }
    //    else
    //    {
    //        if (GridView.Rows[Row].Cells[ColumnNameIndex].Text != null)
    //        {
    //            if (IsNumeric(GridView.Rows[Row].Cells[ColumnNameIndex].Text) == true)
    //            {
    //                if (Comon.cDec(GridView.Rows[Row].Cells[ColumnNameIndex].Text) < 0)
    //                {
    //                    GridView.Rows[Row].Cells[ColumnNameIndex].Focus();
    //                    return functionReturnValue;
    //                }
    //            }
    //            else
    //            {
    //                GridView.Rows[Row].Cells[ColumnNameIndex].Focus();
    //                return functionReturnValue;
    //            }
    //        }
    //    }

    //    functionReturnValue = true;
    //    return functionReturnValue;

    //}
    /// <summary>
    ///  This method checks whether a string is numeric or not
    /// </summary>
    /// <param name="str"></param>
    /// <returns>return value bool if the string is number return true else return false </returns>
    public static bool IsNumeric(this String str)
    {
        try
        {
            Double.Parse(str.ToString()); // Try to parse the input string as a double
            return true; // The input string is numeric
        }
        catch // If any exception occurs during the conversion process, catch it and do nothing
        {
        }
        return false; // The input string is not numeric
    }
   
    public static string Right(string sValue, int iMaxLength)
    {
        //Check if the value is valid
        if (string.IsNullOrEmpty(sValue))
        {
            //Set valid empty string as string could be null
            sValue = string.Empty;
        }
        else if (sValue.Length > iMaxLength)
        {
            //Make the string no longer than the max length
            sValue = sValue.Substring(sValue.Length - iMaxLength, iMaxLength);
        }

        //Return the string
        return sValue;
    }
    public static string Left(string str, int count)
    {

        if (string.IsNullOrEmpty(str) || count < 1)
            return string.Empty;
        else
            return str.Substring(0, Math.Min(count, str.Length));


    }
    public static string Mid(string s, int start, int length)
    {
        if (start > s.Length || start < 0)
        {
            return s;
        }

        if (start + length > s.Length)
        {
            length = s.Length - start;
        }

        string ret = s.Substring(start, length);
        return ret;
    }
    public static int InStrRev(object s, string searchChar, int start = 1)
    {
        int ret = s.ToString().LastIndexOf(searchChar, start);
        return ret;
    }

    public static class MySession
    {
        /// <summary>
        /// This Function To Set Default value for properties
        /// </summary>
        public static void Start()
        {
            Property1 = "default value";
            xWeightItem = -3;
            GlobalDefaultStoreID = 0;
            GlobalDefaultCostCenterID = 0;
            GlobalDefaultSellerID = 0;
            GlobalCanChangeDocumentsDate = true;
            GlobalCanChangeInvoicePrice = true;
            GlobalShowItemQtyInSaleInvoice = false;
            GlobalCanDiscountOnCashierScreen = false;
            GlobalCanCloseCashier = false;
            GlobalCanChangePriceInCashierScreen = false;
            GlobalCanSearchItemInCashierScreen = false;
            GlobalDiscountPercentOnTotal = 100;
            GlobalCanSaleItemForZeroPrice = false;
            GlobalCanOpenCashierDrawerByF12 = false;
            GlobalPrintAndExportReportsByNotLoginLanguage = false;
            GlobalCanEditOrDeleteOnPastPaid = false;

            GlobalItemBarcodeWeightDigits = 7;
            GlobalPriceBarcodeWeightDigits = 5;
            GlobalPriceDigits = 2;
            GlobalQtyDigits = 2;
            GlobalUsingExpiryDate = false;
            GlobalUsingItemsSerials = false;
            GlobalAutoCalcFixAssetsDepreciation = false;
            GlobalCalcStockBy = "CostPrice";
            GlobalWayOfOutItems = "AllowOutItemsWithOutBalance";
            GlobalItemProfit = 20;
            GlobalAllowedPercentDiscount = 50;
            GlobalGoodsOpeningAccountID = 0;
            GlobalEndTermStockAccountID = 0;
            GlobalMaxBarcodeDigits = 15;

            PubSalePriceType = "SalePrice";
            PubCostPriceType = "CostPrice";
            PubCurrentDataBasePath = "";
            PubDatabaseName = "";
            PubCurrentLogicalName = "";
            PubServerName = "";
            PubEventType = "";
            PubSelectPrice = -999999;
            PubCancelTable = false;
            PubMoveFromTableToTable = false;
            PubMoveFromTable = 0;
            PubMoveToTable = 0;
            PubMoveFromSection = 0;
            PubMoveToSection = 0;
            PubSearchMultiRows = false;
            PubGetThrowForms = false;
            PubSelectSpecificMenusToThisComputer = "";
            PubSelectSpecificFormsToThisComputer = "";
            PubSelectSpecificReportsToThisComputer = "";
            PubConnectionOnLine = true;
            PubGetCustomerIDToPriceOffersForm = 0;
            PubGetVoucherIDToSalariesForm = 0;
            PubSelectedMenus = "";
            IsMessageSender = false;
            PrintInChequesPrinters = false;

        }
        /***************** add your session properties here, e.g like this:*************************/
        public static string Property1 { get; set; }
        public static DateTime MyDate { get; set; }
        public static int LoginId { get; set; }
        public static int xWeightItem { get; set; }
        public static long SupplierID { get; set; }
        public static string GlobalUserName { get; set; }
        public static string GlobalBranchName { get; set; }
        public static double GlobalAccountID { get; set; }
        public static int GlobalAccountsLevelDigits { get; set; }
        public static string GlobalComputerInfo { get; set; }
        public static int GlobalNoOfLevels { get; set; }
        public static long GlobalDefaultStoreID { get; set; }
        public static long GlobalDefaultCostCenterID { get; set; }
        public static long GlobalDefaultSellerID { get; set; }
     
        public static bool GlobalCanChangeDocumentsDate { get; set; }
        public static bool GlobalCanChangeInvoicePrice { get; set; }
        public static bool GlobalShowItemQtyInSaleInvoice { get; set; }
        public static bool GlobalCanDiscountOnCashierScreen { get; set; }
        public static bool GlobalCanCloseCashier { get; set; }
        public static bool GlobalCanChangePriceInCashierScreen { get; set; }
        public static bool GlobalCanSearchItemInCashierScreen { get; set; }
        public static double GlobalDiscountPercentOnTotal { get; set; }
        public static bool GlobalCanSaleItemForZeroPrice { get; set; }
        public static bool GlobalCanOpenCashierDrawerByF12 { get; set; }
        public static bool GlobalPrintAndExportReportsByNotLoginLanguage { get; set; }
        public static bool GlobalCanEditOrDeleteOnPastPaid { get; set; }

        public static int GlobalItemBarcodeWeightDigits { get; set; }
        public static int GlobalPriceBarcodeWeightDigits { get; set; }
        public static int GlobalPriceDigits { get; set; }
        public static int GlobalQtyDigits { get; set; }
        public static bool GlobalUsingExpiryDate { get; set; }
        public static bool GlobalUsingItemsSerials { get; set; }
        public static bool GlobalAutoCalcFixAssetsDepreciation { get; set; }
        public static string GlobalCalcStockBy { get; set; }
        public static string GlobalWayOfOutItems { get; set; }
        public static double GlobalItemProfit { get; set; }
        public static double GlobalAllowedPercentDiscount { get; set; }
        public static long GlobalGoodsOpeningAccountID { get; set; }
        public static long GlobalEndTermStockAccountID { get; set; }
        public static long GlobalMaxBarcodeDigits { get; set; }
        public static string FormatDate { get; set; }

        public static string PubSalePriceType { get; set; }
        public static string PubCostPriceType { get; set; }
        public static bool PubSelectedCtrlButton { get; set; }
        public static double PubSelectedPrice { get; set; }
        public static string PubCurrentDataBasePath { get; set; }
        public static string PubDatabaseName { get; set; }
        public static string PubCurrentLogicalName { get; set; }
        public static string PubServerName { get; set; }
        public static string PubEventType { get; set; }
        public static long PubSelectPrice { get; set; }
        public static string PubBarCode { get; set; }
        public static long PubSectionID { get; set; }
        public static string PubSectionName { get; set; }
        public static long PubTableID { get; set; }
        public static string PubTableName { get; set; }
        public static bool PubCancelTable { get; set; }
        public static bool PubMoveFromTableToTable { get; set; }
        public static int PubMoveFromTable { get; set; }
        public static int PubMoveToTable { get; set; }
        public static int PubMoveFromSection { get; set; }
        public static int PubMoveToSection { get; set; }
        public static bool PubSearchMultiRows { get; set; }
        public static bool PubGetThrowForms { get; set; }
        public static string PubSelectSpecificMenusToThisComputer { get; set; }
        public static string PubSelectSpecificFormsToThisComputer { get; set; }
        public static string PubSelectSpecificReportsToThisComputer { get; set; }
        public static bool PubConnectionOnLine { get; set; }
        public static int PubGetCustomerIDToPriceOffersForm { get; set; }
        public static long PubGetVoucherIDToSalariesForm { get; set; }
        public static string PubSelectedMenus { get; set; }
        public static string ExportType { get; set; }
        public static bool IsMessageSender { get; set; }
        public static string ExportedReportName { get; set; }
        public static bool PrintInChequesPrinters { get; set; }
        public static string DBName { get; set; }
        public static string defaultBackupPath { get; set; }
        public static string PubStrCon { get; set; }


    }
    //public static void FillCombo(DropDownList Cmb, string Tablename, string Code, string Name, string Order = "")
    //{
    //    string strSQL = "SELECT " + Code + "," + Name + " FROM " + Tablename;
    //    if (Order == "")
    //    {
    //        strSQL = strSQL + " ORDER BY " + Code;
    //    }
    //    else
    //    {
    //        strSQL = strSQL + " ORDER BY " + Order;
    //    }
    //    DataSet ds = new DataSet();
    //    Cmb.DataSource = SelectRecord(strSQL).DefaultView;
    //    Cmb.DataTextField = Name;
    //    Cmb.DataValueField = Code;
    //    Cmb.DataBind();

    //    ds = null;
    //}
    //public static void FillComboWithSql(DropDownList Cmb, string sql, string Code, string Name)
    //{
    //    string strSQL = sql;
    //    DataSet ds = new DataSet();
    //    Cmb.DataSource = SelectRecord(strSQL).DefaultView;
    //    Cmb.DataTextField = Name;
    //    Cmb.DataValueField = Code;
    //    Cmb.DataBind();
    //    Cmb.SelectedIndex = 0;
    //    ds = null;
    //}
    //public static void FillComboWithSearch(DropDownList Cmb, string Tablename, string Code, string Name, string swhere)
    //{
    //    string strSQL = "SELECT " + Code + "," + Name + " FROM " + Tablename + " where " + swhere + " ORDER BY " + Code;

    //    DataSet ds = new DataSet();
    //    Cmb.DataSource = SelectRecord(strSQL).DefaultView;
    //    Cmb.DataTextField = Name;
    //    Cmb.DataValueField = Code;
    //    Cmb.DataBind();

    //    ds = null;
    //}
    //public static void FillComboWithNewRow(DropDownList Cmb, string Tablename, string Code, string Name, string field1, string field2, string swhere = "")
    //{
    //    string strSQL;
    //    if (swhere == "")
    //    {
    //        strSQL = "SELECT " + Code + "," + Name + " FROM " + Tablename + " ORDER BY " + Code;
    //    }
    //    else
    //    {
    //        strSQL = "SELECT " + Code + "," + Name + " FROM " + Tablename + " where " + swhere + " ORDER BY " + Code;
    //    }

    //    DataTable dt = new DataTable();
    //    dt = SelectRecord(strSQL);

    //    DataRow row;
    //    row = dt.NewRow();

    //    // Then add the new row to the collection.
    //    row[Code] = field1;
    //    row[Name] = field2;
    //   dt.Rows.Add(row);

    //    DataView dv = dt.DefaultView;
    //    dv.Sort = Code + " Asc";
    //    DataTable sortedDT = dv.ToTable();

    //    Cmb.DataSource = sortedDT;
    //    Cmb.DataTextField = Name;
    //    Cmb.DataValueField = Code;
    //    Cmb.DataBind();


    //}
    private static Hashtable TempTable;
    private static NameValueCollection coll;
    private static string _table;
    private static int _ExcuteType = 0;
    private static string _sCondition;
    public static string FormatType;
    public static string[] PubArrParameters = new string[30];
    public static long GlobalBRANCH_ID;
    public static long GlobalUserID;
    public static iLanguage Language;
    public static string ProjectPath;
    public static string strSQL;
    public static string Table
    {
        get { return _table; }
        set { _table = value; }
    }
    public static int ExcuteType
    {
        get { return _ExcuteType; }
        set { _ExcuteType = value; }
    }
    public static string sCondition
    {
        get { return _sCondition; }
        set { _sCondition = value; }
    }
   /// <summary>
    /// this Function to Clear TempTable
   /// </summary>
    public static void NewFields()
    {
        if (TempTable != null)
        {
            TempTable.Clear();
        }
    }
    /// <summary>
    /// Type Language
    /// </summary>
    public enum iLanguage
    {
        Arabic = 0,
        English = 1
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Filedname"></param>
    /// <param name="FieldValue"></param>
    public static void AddNumericField(string Filedname, string FieldValue)
    {
        if (TempTable == null)// Check if the TempTable hash table is null and, if so, create a new instance
        {
            TempTable = new Hashtable();
        }
        if (coll == null)        // Check if the coll NameValueCollection is null and, if so, create a new instance
        {
            coll = new NameValueCollection();
        }
        if (FieldValue == "False") // Convert the FieldValue to 0 if it's "False"
        {
            FieldValue = "0";
        }
        if (FieldValue == "True")// Convert the FieldValue to 1 if it's "True"
        {
            FieldValue = "1";
        }
        if (FieldValue == null) // Convert the FieldValue to 0 if it's null
        {
            FieldValue = "0";
        }
        if (FieldValue == string.Empty) // Convert the FieldValue to 0 if it's an empty string
        {
            FieldValue = "0";
        }
        TempTable.Add(Filedname, FieldValue);// Add the Fieldname and FieldValue to the TempTable hash table
        coll.Add(Filedname, FieldValue); // Add the Fieldname and FieldValue to the coll NameValueCollection
    }

    /// <summary>
    /// This method adds a numeric field to a temporary Hashtable and a NameValueCollection
    /// </summary>
    /// <param name="Fieldname"></param>
    /// <param name="FieldValue"></param>
    public static void AddNumericField(string Fieldname, int FieldValue)
    {
        // Create a new Hashtable if the TempTable variable is null
        if (TempTable == null)
        {
            TempTable = new Hashtable();
        }
        // Create a new NameValueCollection if the coll variable is null
        if (coll == null)
        {
            coll = new NameValueCollection();
        }
        // Add the Fieldname and FieldValue to the TempTable variable
        TempTable.Add(Fieldname, FieldValue);
        // Add the Fieldname and FieldValue (as a string) to the coll variable
        coll.Add(Fieldname, FieldValue.ToString());
    }

    /// <summary>
    /// This method adds a string field to a temporary Hashtable
    /// </summary>
    /// <param name="Fieldname"></param>
    /// <param name="FieldValue"></param>
    public static void AddStringField(string Fieldname, string FieldValue)
    {
        // Create a new Hashtable if the TempTable variable is null
        if (TempTable == null)
        {
            TempTable = new Hashtable();
        }
        // If the FieldValue is an empty string, replace it with a single space character
        if (FieldValue == string.Empty)
        {
            FieldValue = " ";
        }
        // Replace any single quotes in the FieldValue with a special character (to avoid SQL injection)
        FieldValue = FieldValue.Replace("'", "�");

        // Add the Fieldname and FieldValue to the TempTable variable
        TempTable.Add(Fieldname, "'" + FieldValue + "'");
    }

    
   /// <summary>
   /// This method returns a string representing an SQL INSERT query
   /// </summary>
   /// <returns>return sql stetment</returns>
    public static string GetInsertQuary()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("INSERT INTO " + Table + "(");

        // Add column names to the query
        foreach (DictionaryEntry entry in TempTable)
        {
            sb.Append(entry.Key.ToString() + ",");
        }
        sb = sb.Remove(sb.Length - 1, 1);
        sb.Append(") VALUES (");

        // Add column values to the query
        foreach (DictionaryEntry entry in TempTable)
        {
            sb.Append(entry.Value + ",");
        }
        sb = sb.Remove(sb.Length - 1, 1);
        sb.Append(")");
        // Return the complete SQL query
        return sb.ToString();
    }

   /// <summary>
   ///  // This method executes an SQL INSERT query or a stored procedure based on the value of ExcuteType
   /// </summary>
    public static void ExecuteInsert()
    {
        if (ExcuteType == 0)
        {
            // Execute an SQL INSERT query by calling the GetInsertQuary() method
            ExecututeSQL(GetInsertQuary());
        }
        else
        {
            // Execute a stored procedure by calling the ExecuteProcedure() method
            ExecuteProcedure(Table, "");
        }
    }
    /// <summary>
    /// This Function to Executute SQL
    /// </summary>
    /// <param name="StrSQL"></param>
    public static void ExecututeSQL(string StrSQL)
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

    /// <summary>
    /// This Function To Execute Procedure 
    /// </summary>
    /// <param name="procedureName"></param>
    /// <param name="ParmName"></param>
    /// <returns></returns>
    public static int ExecuteProcedure(string procedureName, string ParmName = "")
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
    
   /// <summary>
   /// This method returns a SQL update statement string for the table and temporary table data provided
   /// </summary>
   /// <returns></returns>
    public static string GetUpdateStr()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Update " + Table + " Set ");
        // Iterate over the dictionary, adding each key/value pair to the SQL statement
        foreach (DictionaryEntry entry in TempTable)
        {
            sb.Append(entry.Key.ToString() + "=");

            sb.Append(entry.Value + ",");
        }
        sb = sb.Remove(sb.Length - 1, 1);
        // Add the WHERE clause to the SQL statement
        sb.Append(" Where " + sCondition);

        // Return the completed SQL statement
        return sb.ToString();
    }
    
   /// <summary>
   /// This method executes the update
   /// </summary>
    public static void ExecuteUpdate()
    {
        if (ExcuteType == 0) // if the execute type is 0, execute the SQL update
        {
            ExecututeSQL(GetUpdateStr());
        }
        else // if the execute type is not 0, execute the procedure
        {
            ExecuteProcedure(Table, "");
        }

    }
    
   /// <summary>
   /// This method executes the delete operation
   /// </summary>
    public static void ExecuteDelete()
    {
        string sDel = "Delete from " + Table + " where " + sCondition; // SQL statement for deleting rows from the table
        ExecututeSQL(sDel); // execute the SQL statement
    }
    public static string GetDelete()
    {
        string sDel = "Delete from " + Table + " where " + sCondition; // SQL statement for deleting rows from the table
        return sDel;
    }
    
   /// <summary>
   /// This method returns the server date in the format "yyyy/MM/dd"
   /// </summary>
   /// <returns></returns>
    public static string GetServerDate()
    {
        string functionReturnValue = null;
        // Select the current date/time from the database
        functionReturnValue = SelectRecord("Select GetDate()").Rows[0][0].ToString();
        // Set the DateTimeFormatInfo to format the date correctly
        System.Globalization.DateTimeFormatInfo GgeorgianDTF = new System.Globalization.CultureInfo("en-US", false).DateTimeFormat;
        GgeorgianDTF.Calendar = new System.Globalization.GregorianCalendar();
        GgeorgianDTF.ShortDatePattern = "yyyy/MM/dd";
        GgeorgianDTF.MonthDayPattern = "MMMM";

        // Convert the server date to a DateTime object and format it according to the DateTimeFormatInfo
        functionReturnValue = Comon.cDate(functionReturnValue).ToString("d", GgeorgianDTF);

        // Return the formatted server date
        return functionReturnValue;
    }

    /// <summary>
    /// // This method returns the server date in the format "yyyyMMdd"
    /// </summary>
    /// <returns></returns>
    public static string GetServerDateSerial()
    {
        string functionReturnValue = null;
        // Select the current date/time from the database
        functionReturnValue = SelectRecord("Select GetDate()").Rows[0][0].ToString();
        // Set the DateTimeFormatInfo to format the date correctly
        System.Globalization.DateTimeFormatInfo GgeorgianDTF = new System.Globalization.CultureInfo("en-US", false).DateTimeFormat;
        GgeorgianDTF.Calendar = new System.Globalization.GregorianCalendar();
        GgeorgianDTF.ShortDatePattern = "yyyyMMdd";
        GgeorgianDTF.MonthDayPattern = "MMMM";
        // Convert the server date to a DateTime object and format it according to the DateTimeFormatInfo
        functionReturnValue = Comon.cDate(functionReturnValue).ToString("d", GgeorgianDTF);
        // Return the formatted server date
        return functionReturnValue;
    }

   /// <summary>
    /// // This method returns the server time in the format "hh:mm"
   /// </summary>
   /// <returns></returns>
    public static string GetServerTime()
    {
        string functionReturnValue = null;
        // Select the current time from the database
        functionReturnValue = SelectRecord("SELECT CONVERT (time, SYSDATETIME())").Rows[0][0].ToString();
        // Extract the hours and minutes from the time value and format it
        functionReturnValue = (functionReturnValue.Substring(0, 2) + functionReturnValue.Substring(2, 3));
        // Return the formatted server time
        return functionReturnValue;
    }

    /// <summary>
    /// This method returns the server time in the format "hh mm"
    /// </summary>
    /// <returns></returns>
    public static string GetServerTimeSerial()
    {
        string functionReturnValue = null;
        // Select the current time from the database
        functionReturnValue = SelectRecord("SELECT CONVERT (time, SYSDATETIME())").Rows[0][0].ToString();
        // Replace the colon (:) with a space to separate the hours and minutes
        functionReturnValue = functionReturnValue.Replace(':', ' ');
        // Extract the hours and minutes from the time value and format it
        functionReturnValue = (functionReturnValue.Substring(0, 2) + functionReturnValue.Substring(3, 3));
        // If the functionReturnValue length is 2, it means that the hours part of the time value is a single digit. In this case, add a "12" in front of the hours part to convert the time to 12-hour format with AM/PM. For example, "9 30" becomes "09 30" and "2 45" becomes "12 45" (for 2:45 PM)
        if (functionReturnValue.Length == 2)
            functionReturnValue = "12" + functionReturnValue;

        // Return the formatted server time
        return functionReturnValue.Trim();
    }

    /// <summary>
    /// This method converts a decimal number to its word representation in a specified currency
    /// </summary>
    /// <param name="Num"></param>
    /// <param name="CurencyID"></param>
    /// <returns></returns>
    //public static string ToWords(decimal Num, int CurencyID)
    //{
    //    // Create a list of currency info for the supported currencies
    //    List<CurrencyInfo> currencies = new List<CurrencyInfo>();
    //    currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Syria));
    //    currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.UAE));
    //    currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia));
    //    currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Tunisia));
    //    currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Gold));
    //    // Create a new ToWord object to convert the given number to its word representation in the specified currency
    //    ToWord toWord = new ToWord(Convert.ToDecimal(Num.ToString()), currencies[CurencyID]);

    //    // Check the user's language preference and convert the word representation of the number to English or Arabic accordingly
    //    if (UserInfo.Language.ToString() == iLanguage.English.ToString())
    //        return toWord.ConvertToEnglish();
    //    else
    //        return toWord.ConvertToArabic();
    //}

    public static string ToWords(decimal Num, int CurencyID)
    {
        // Create a list of currency info for the supported currencies
        List<CurrencyInfo> currencies = new List<CurrencyInfo>();
        currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Syria));
        currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.UAE));
        currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia));
        currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Bahrain));
        currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Dolar));
        currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Gold));
        currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Qatar));
        // Create a new ToWord object to convert the given number to its word representation in the specified currency
        string currencyid = "2";
        string startupPath = Directory.GetCurrentDirectory() + "\\";
        try
        {
            var currency = new FileStream(@startupPath + "currency.txt", FileMode.Open, FileAccess.Read);
            
            if (currency != null)
            {
                using (var streamReader = new StreamReader(currency, Encoding.UTF8))
                {
                    currencyid = streamReader.ReadToEnd();
                }
            }
        }
        catch
        {
            currencyid = "2";
        }
        ToWord toWord = new ToWord(Convert.ToDecimal(Num.ToString()), currencies[Comon.cInt(CurencyID)]);

        // Check the user's language preference and convert the word representation of the number to English or Arabic accordingly
        if (UserInfo.Language.ToString() == iLanguage.English.ToString())
            return toWord.ConvertToEnglish();
        else
            return toWord.ConvertToArabic();
    }
    /// <summary>
    /// // This method gets a new ID for an item group
    /// </summary>
    /// <param name="GropID"></param>
    /// <returns></returns> 
    public static double GetNewID(double GropID)
    {
        double ID = 1;
        DataTable dt;
        string strSQL;
        // Build the SQL query to get the maximum ID in the item group
        strSQL = "SELECT Max(ItemGroupID)+1 FROM Stc_Items Where GroupID = " + GropID + " AND BaseID<>1 and TypeID<>" + 4;
        // Execute the query to get the results
        dt = Lip.SelectRecord(strSQL);
        // Check if the query returned a null value, meaning there are no previous IDs in the group
        if (dt.Rows[0][0] == DBNull.Value)
            return 1;
        else
            // Convert the value of the maximum ID to an integer and add 1 to get the new ID to be returned
            ID = Comon.cDbl(dt.Rows[0][0].ToString());

        // Return the new ID
        return ID;
    }
    public static int GetNewIDSaveItem(int GropID, int TypeID)
    {
        int ID = 1;
        DataTable dt;
        string strSQL;
        // Build the SQL query to get the maximum ID in the item group
        strSQL = "SELECT Max(ItemGroupID)+1 FROM Stc_Items Where GroupID = " + GropID + " and BranchID="+Edex.Model.MySession.GlobalBranchID+" AND BaseID<>1 and TypeID=" + TypeID;
        // Execute the query to get the results
        dt = Lip.SelectRecord(strSQL);
        // Check if the query returned a null value, meaning there are no previous IDs in the group
        if (dt.Rows[0][0] == DBNull.Value)
            return 1;
        else
            // Convert the value of the maximum ID to an integer and add 1 to get the new ID to be returned
            ID = Comon.cInt(dt.Rows[0][0].ToString());

        // Return the new ID
        return ID;
    }


    public static bool CheckTheItemIsHaveTransactionByBarCode(string BarCode, string TableNot)
    {
        //FileNameTableDetailToList();
        DataTable dt = new DataTable();        
       //foreach (var i in Enumerable.Range(0, NameOfTableDetails.Count))
       // {
       //     if (NameOfTableDetails[i].ToString() == TableNot)
       //     {
       //         continue;
       //     }
       //     else
       //     {
               
       //        dt = SelectRecord( "SELECT d.BarCode FROM "+ NameOfTableDetails[i]+"  as d INNER JOIN "+NameMasterarray[i]+" as m ON d.InvoiceID = m.InvoiceID and m.Cancel = 0 WHERE d.BarCode ='"+ BarCode+"'" );

       //         if (dt.Rows.Count > 0)
       //             return true;
       //     }
             
       // }
        dt = SelectRecord("SELECT  [BarCode] FROM [Stc_ItemsMoviing] where BarCode ='" + BarCode+"' and Cancel=0");

        if (dt.Rows.Count > 0)
            return true;
        return false;
    }

    public static bool CheckTheItemIsHaveTransactionByItemID(long ItemID)
    {
        DataTable dt = new DataTable();
        dt = SelectRecord("SELECT  [ItemID] FROM  [Stc_ItemsMoviing]  where [ItemID]=" +Comon.cLong(ItemID) + " and Cancel=0 and BranchID="+Edex.Model.MySession.GlobalBranchID);
        if (dt.Rows.Count > 0)
            return true;
        return false;
    }
    public static bool CheckTheAccountIsStope(double AccountID, int BranchID)
    {       
        int Flage =Comon.cInt( Lip.GetValue("SELECT   [StopAccount]  FROM  [Acc_Accounts] where [AccountID]=" + Comon.cLong(AccountID) + " and Cancel=0 and BranchID=" +Comon.cInt( BranchID)));
        if (Flage> 0)
            return true;
        return false;
    }

    public static int CheckTheAccountMaxLimit(double AccountID, int BranchID,decimal Ammount,int flage)
    {
        DataTable dtMaxLimit =  Lip.SelectRecord("SELECT   [MaxLimit],AllowMaxLimit  FROM  [Acc_Accounts] where [AccountID]=" + Comon.cLong(AccountID) + " and Cancel=0 and BranchID=" + Comon.cInt(BranchID));
        if (dtMaxLimit.Rows.Count > 0 && Comon.cDec(dtMaxLimit.Rows[0]["MaxLimit"])>0)
        {
            string strSQL = "SELECT   SUM(Acc_VariousVoucherMachinDetails.Debit) AS TotalDebit, SUM(Acc_VariousVoucherMachinDetails.Credit) AS TotalCredit " +
                    "FROM Acc_VariousVoucherMachinDetails " +
                    "INNER JOIN Acc_VariousVoucherMachinMaster ON Acc_VariousVoucherMachinDetails.BranchID = Acc_VariousVoucherMachinMaster.BranchID " +
                    "AND Acc_VariousVoucherMachinDetails.VoucherID = Acc_VariousVoucherMachinMaster.VoucherID " +
                    "WHERE Acc_VariousVoucherMachinMaster.Cancel = 0 " +
                    "AND Acc_VariousVoucherMachinDetails.AccountID = " + Comon.cLong(AccountID) +
                    "AND Acc_VariousVoucherMachinMaster.BranchID = " + Comon.cInt(BranchID);
                     
            DataTable dt = Lip.SelectRecord(strSQL);
            decimal Blance = 0;
            decimal TotalDebit = 0;
            decimal TotalCredit = 0;
            if (dt.Rows.Count > 0)
            {
                TotalDebit = Comon.cDec(dt.Rows[0]["TotalDebit"]);
                TotalCredit = Comon.cDec(dt.Rows[0]["TotalCredit"]);
            }
            if (flage == 1)
                Blance = Comon.cDec(Comon.cDec(Comon.cDec(TotalDebit) + Comon.cDec(Ammount)) - Comon.cDec(TotalCredit));
            else if (flage == 2)
                Blance = Comon.cDec(Comon.cDec(TotalDebit) - Comon.cDec(Comon.cDec(TotalCredit) + Comon.cDec(Ammount)));
            if (Blance > Comon.cDec(dtMaxLimit.Rows[0]["MaxLimit"]) && Comon.cInt(dtMaxLimit.Rows[0]["AllowMaxLimit"]) == 1)
                return 1;
            else if (Blance > Comon.cDec(dtMaxLimit.Rows[0]["MaxLimit"]))
                return 2;
        }
        return 0;
    }
    public static decimal CheckTheAccountBlance(double AccountID, int BranchID)
    {
        decimal Blance = 0;
        DataTable dtMaxLimit = Lip.SelectRecord("SELECT   [MaxLimit],AllowMaxLimit  FROM  [Acc_Accounts] where [AccountID]=" + Comon.cLong(AccountID) + " and Cancel=0 and BranchID=" + Comon.cInt(BranchID));
        if (dtMaxLimit.Rows.Count > 0)
        {
            string strSQL = "SELECT   SUM(Acc_VariousVoucherMachinDetails.Debit) AS TotalDebit, SUM(Acc_VariousVoucherMachinDetails.Credit) AS TotalCredit " +
                    "FROM Acc_VariousVoucherMachinDetails " +
                    "INNER JOIN Acc_VariousVoucherMachinMaster ON Acc_VariousVoucherMachinDetails.BranchID = Acc_VariousVoucherMachinMaster.BranchID " +
                    "AND Acc_VariousVoucherMachinDetails.VoucherID = Acc_VariousVoucherMachinMaster.VoucherID " +
                    "WHERE Acc_VariousVoucherMachinMaster.Cancel = 0 " +
                    "AND Acc_VariousVoucherMachinDetails.AccountID = " + Comon.cLong(AccountID) +
                    "AND Acc_VariousVoucherMachinMaster.BranchID = " + Comon.cInt(BranchID);

            DataTable dt = Lip.SelectRecord(strSQL);
          
            decimal TotalDebit = 0;
            decimal TotalCredit = 0;
            if (dt.Rows.Count > 0)
            {
                TotalDebit = Comon.cDec(dt.Rows[0]["TotalDebit"]);
                TotalCredit = Comon.cDec(dt.Rows[0]["TotalCredit"]);
            }
            Blance = Comon.cDec( Comon.cDec(TotalDebit)  - Comon.cDec(TotalCredit));
        }
        return Blance;
    }

    public static int CheckTheCustomerAllowAgeDebtOrNot(double CustomerAccountID, int BrancID)
    {
        try
        {
            if (CheckTheAccountBlance(CustomerAccountID, BrancID) > 0)
            {
                string StrSQL = " SELECT [MaxAgeDebt]  ,[AllowMaxAgeDebt]  FROM [Sales_Customers] where [AccountID]=" + CustomerAccountID + " and [BranchID]=" + BrancID;
                DataTable dt = Lip.SelectRecord(StrSQL);
             
                if (Comon.cDec(dt.Rows[0]["MaxAgeDebt"]) > 0)
                {
                    long Blance = CheckTheAgeOfDebtCustomer(CustomerAccountID, BrancID);
                    if (Blance >= Comon.cDec(dt.Rows[0]["MaxAgeDebt"]) && Comon.cInt(dt.Rows[0]["AllowMaxAgeDebt"]) == 1)
                        return 1;
                    else if (Blance >= Comon.cDec(dt.Rows[0]["MaxAgeDebt"]))
                        return 2;
                }
            }
            return 0;
        }
        catch
        {
            return 0;
        }
    }
    public static long CheckTheAgeOfDebtCustomer(double CustomerID, int BrancID)
    {
        try
        {
            string StrSQL = "SELECT * FROM CheckTheAgeOfDebtCustomer WHERE BranchID=" + BrancID + " AND CustomerID=" + CustomerID;
            DataTable dt = Lip.SelectRecord(StrSQL);
            string StrSQLS = "SELECT * FROM CheckTheAgeOfDebtCustomerSrvice  WHERE BranchID=" + BrancID + " AND CustomerID=" + CustomerID;
            DataTable dtS = Lip.SelectRecord(StrSQL);
            DataTable dtAll = dt.Clone();
            dtAll.Merge(dtS);
            long maxDateDiff = 0;
            for (int i = 0; i < dtAll.Rows.Count; i++)
            {
                long dateDiff = Comon.DateDiff(Comon.DateInterval.Day, Comon.ConvertSerialToDate(dtAll.Rows[i]["InvoiceDate"].ToString()), Comon.ConvertSerialToDate(dtAll.Rows[i]["ReceiptVoucherDate"].ToString()));

                if (dateDiff > maxDateDiff)
                  maxDateDiff = dateDiff;
            }
            return maxDateDiff;
        }
        catch
        {
            return 0;
        }
    }

    public static bool CheckDatabaseActivity()
    {
        string query = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME IN (SELECT name FROM sys.objects WHERE type = 'U' AND modify_date > " + Lip.GetServerDateSerial() + ")";
        int Flage = Comon.cInt(Lip.GetValue(query));
        return Flage > 0;
    }
//    public static decimal RemindQtyItemByMinUnit(int ItemID, int SizeID, double StoreID)
//    {
//        decimal value = 0;
//        decimal REmindQty = 0;
//        string filter = "Stc_ItemsMoviing.BranchID = " + UserInfo.BRANCHID + " AND Stc_ItemsMoviing.Cancel =0     AND  Stc_ItemsMoviing.ItemID =" + ItemID + "  And ";
//        if (StoreID == 0)
//            StoreID = Comon.cInt(MySession.GlobalDefaultStoreID.ToString());

//        if (StoreID > 0)
//            filter = filter + " dbo.Stc_ItemsMoviing.StoreID  =" + Comon.cDbl(StoreID) + "  And ";


//        filter = filter.Remove(filter.Length - 4, 4);
//        string StrSQL = @"SELECT dbo.Stc_ItemsMoviing.BarCode,'' as StoreName, dbo.Stc_ItemsMoviing.ItemID,     SUM(CASE WHEN dbo.Stc_ItemsMoviing.MoveType = 1 THEN dbo.Stc_ItemsMoviing.QTY ELSE -dbo.Stc_ItemsMoviing.QTY END) AS QtyBalance, dbo.Stc_SizingUnits.ArbName AS SizeName, dbo.Stc_SizingUnits.SizeID,dbo.Stc_ItemsMoviing.StoreID,dbo.Stc_ItemsMoviing.BranchID 
//                 FROM dbo.Stc_ItemsMoviing   LEFT OUTER JOIN dbo.Stc_SizingUnits ON dbo.Stc_ItemsMoviing.SizeID = dbo.Stc_SizingUnits.SizeID 
//                  WHERE  " + filter +
//                 " GROUP BY dbo.Stc_ItemsMoviing.BarCode,dbo.Stc_ItemsMoviing.StoreID ,dbo.Stc_ItemsMoviing.BranchID, dbo.Stc_SizingUnits.ArbName, dbo.Stc_ItemsMoviing.SizeID,dbo.Stc_ItemsMoviing.ItemID,dbo.Stc_SizingUnits.SizeID  ";

//        DataTable ItemdtBalance = Lip.SelectRecord(StrSQL);

//        //يمكن التكرار بعدد المخازن 
//        decimal rqTY = Comon.cDec(ItemdtBalance.Rows[0]["QtyBalance"].ToString());
//        REmindQty = rqTY;
//        decimal Qty = 0;
//        decimal Pakeg = 0;
//        string strSQL = "Select * from Stc_ItemUnits where ItemID=" + Comon.cLong(ItemID) + "  Order By PackingQty ";

//        DataTable Itemdt = Lip.SelectRecord(strSQL);

//        decimal PakegUnits = 0; //الباكج للوحدة المطلوبة


//        for (int j = 0; j <= Itemdt.Rows.Count - 1; j++)
//        {
//            Pakeg = Comon.cDec(Itemdt.Rows[j]["PackingQty"].ToString());
//            decimal remindunitqty = Comon.cDec(ItemdtBalance.Rows[j]["QtyBalance"].ToString());
//            Qty = Qty + Pakeg * remindunitqty;
//            if (SizeID == Comon.cInt(Itemdt.Rows[j]["SizeID"].ToString()))
//            {
//                PakegUnits = Pakeg;
//            }
//        }

//        REmindQty = Comon.ConvertToDecimalPrice(Qty.ToString());

//        if (SizeID > 0 && PakegUnits > 0)
//            REmindQty = Comon.cDec(REmindQty / PakegUnits);

//        return REmindQty;
//    }

//    public static decimal RemindQtyItemByMinUnit(int ItemID, int SizeID, double StoreID)
//    {
//        decimal value = 0;
//        decimal REmindQty = 0;
//        string filter = "Stc_ItemsMoviing.BranchID = " + UserInfo.BRANCHID + " AND Stc_ItemsMoviing.Cancel =0     AND  Stc_ItemsMoviing.ItemID =" + ItemID + "  And ";
//        if (StoreID == 0)
//            StoreID = Comon.cInt(MySession.GlobalDefaultStoreID.ToString());

//        if (StoreID > 0)
//            filter = filter + " dbo.Stc_ItemsMoviing.StoreID  =" + Comon.cDbl(StoreID) + "  And ";


//        filter = filter.Remove(filter.Length - 4, 4);
//        string StrSQL = @"SELECT 
//            dbo.Stc_ItemsMoviing.BarCode,
//            '' as StoreName, 
//            dbo.Stc_ItemsMoviing.ItemID,
//            ISNULL(SUM(CASE WHEN dbo.Stc_ItemsMoviing.MoveType = 1 THEN dbo.Stc_ItemsMoviing.QTY ELSE -dbo.Stc_ItemsMoviing.QTY END), 0) AS QtyBalance,
//            dbo.Stc_SizingUnits.ArbName AS SizeName,
//            dbo.Stc_SizingUnits.SizeID,
//            dbo.Stc_ItemsMoviing.StoreID,
//            dbo.Stc_ItemsMoviing.BranchID 
//        FROM 
//            dbo.Stc_SizingUnits
//        LEFT JOIN 
//            dbo.Stc_ItemsMoviing ON dbo.Stc_SizingUnits.SizeID = dbo.Stc_ItemsMoviing.SizeID
//        WHERE  " + filter + " GROUP BY  dbo.Stc_ItemsMoviing.BarCode,  dbo.Stc_ItemsMoviing.StoreID, dbo.Stc_ItemsMoviing.BranchID,   dbo.Stc_SizingUnits.ArbName,   dbo.Stc_ItemsMoviing.SizeID,   dbo.Stc_ItemsMoviing.ItemID,  dbo.Stc_SizingUnits.SizeID";

//        DataTable ItemdtBalance = Lip.SelectRecord(StrSQL);

//        //يمكن التكرار بعدد المخازن 
//        decimal rqTY = Comon.cDec(ItemdtBalance.Rows[0]["QtyBalance"].ToString());
//        REmindQty = rqTY;
//        decimal Qty = 0;
//        decimal Pakeg = 0;
//        string strSQL = "Select * from Stc_ItemUnits where ItemID=" + Comon.cLong(ItemID) + "  Order By PackingQty    ";

//        decimal PackingQtyThisSize=Comon.cDec(Lip.GetValue("Select PackingQty from Stc_ItemUnits where ItemID=" + Comon.cLong(ItemID) + "  and SizeID="+SizeID));
//        DataTable Itemdt = Lip.SelectRecord(strSQL);
//        decimal PakegUnits = 0; //الباكج للوحدة المطلوبة
//        DataTable PackingQtyMinmum=Lip.SelectRecord("Select * from Stc_ItemUnits where ItemID=" + Comon.cLong(ItemID) + " and and SizeID<"+SizeID+"  Order By PackingQty");
//          DataTable PackingQtyMaxmum=Lip.SelectRecord("Select * from Stc_ItemUnits where ItemID=" + Comon.cLong(ItemID) + " and and SizeID>"+SizeID+"  Order By PackingQty");
//        for (int j = 0; j <= ItemdtBalance.Rows.Count - 1; j++)
//        {
//              Pakeg = Comon.cDec(Itemdt.Rows[j]["PackingQty"].ToString());
//              decimal remindunitqty = Comon.cDec(ItemdtBalance.Rows[j]["QtyBalance"].ToString());
//              Qty +=  remindunitqty / Pakeg;
//            if (PackingQtyThisSize<Pakeg)
//                for (int i = 0; i < PackingQtyMinmum.Rows.Count; i++)
//                {
//                     Qty +=  remindunitqty / Comon.cDec(PackingQtyMinmum.Rows[i]["PackingQty"].ToString());
//                }
//            else if (PackingQtyThisSize>Pakeg)
//                for (int i = 0; i < PackingQtyMaxmum.Rows.Count; i++)
//                {
//                    Qty += remindunitqty * Comon.cDec(PackingQtyMaxmum.Rows[i]["PackingQty"].ToString());
//                }
           
            
//        }

//        REmindQty = Comon.ConvertToDecimalPrice(Qty.ToString());

       

//        return REmindQty;
//    }
    //public static decimal GetRemindQTY(int ItemID, int SizeID, double StoreID)
    //{
    //    DataTable PackingQtyMaxmum = Lip.SelectRecord("Select TOP 1  * from Stc_ItemUnits where ItemID=" + Comon.cLong(ItemID) + " and SizeID>" + SizeID + "  Order By PackingQty Asc");
    //    decimal PackingQtyThisSize = Comon.cDec(Lip.GetValue("Select PackingQty from Stc_ItemUnits where ItemID=" + Comon.cLong(ItemID) + "  and SizeID=" + SizeID));
    //    decimal Qty = 0;
    //    decimal remindUnitQty = Comon.cDec(RemindQtyItemByMinUnit(ItemID, SizeID, StoreID));
    //   // Qty = remindUnitQty;
    //    if (PackingQtyMaxmum.Rows.Count > 0)
    //    {
    //        for (int i = 0; i < PackingQtyMaxmum.Rows.Count; i++)
    //        {
    //            decimal currentPackingQty = Comon.cDec(PackingQtyMaxmum.Rows[i]["PackingQty"].ToString());

    //            if (currentPackingQty != PackingQtyThisSize)
    //            {
    //                decimal QtyTt = remindUnitQty / PackingQtyThisSize;
    //                decimal decimalPart = QtyTt % 1;  // الجزء العشري بعد النقطة
    //                Qty += decimalPart * PackingQtyThisSize;
    //            }

    //        }
    //    }
    //    else
    //    {
    //        int countSize = Comon.cInt(Lip.GetValue("Select count(SizeID) from Stc_ItemUnits where ItemID=" + Comon.cLong(ItemID)));
    //        if (countSize == 1)
    //            Qty = Comon.cDec(remindUnitQty);
    //        else
    //        Qty = Comon.cLong(Math.Floor(remindUnitQty));
    //    }
    //    return Qty;
    //}
    //public static decimal GetRemindQTY(int ItemID, int SizeID, double StoreID)
    //{
    //    DataTable PackingQtyMaxmum = Lip.SelectRecord("Select TOP 1  * from Stc_ItemUnits where ItemID=" + Comon.cLong(ItemID) + " and SizeID>" + SizeID + "  Order By PackingQty desc");
    //    decimal PackingQtyThisSize = Comon.cDec(Lip.GetValue("Select PackingQty from Stc_ItemUnits where ItemID=" + Comon.cLong(ItemID) + "  and SizeID=" + SizeID));
    //    decimal Qty = 0;
    //    decimal remindUnitQty = Comon.cDec(RemindQtyItemByMinUnit(ItemID, SizeID, StoreID));
    //    // Qty = remindUnitQty;
    //    if (PackingQtyMaxmum.Rows.Count > 0)
    //    {
    //        for (int i = 0; i < PackingQtyMaxmum.Rows.Count; i++)
    //        {
                 
    //             remindUnitQty = Comon.cDec(RemindQtyItemByMinUnit(ItemID, Comon.cInt(PackingQtyMaxmum.Rows[i]["SizeID"].ToString()), StoreID));
               
    //                decimal decimalPart = remindUnitQty % 1;  // الجزء العشري بعد النقطة
               
                  
    //                Qty =Math.Floor( decimalPart * PackingQtyThisSize);
                

    //        }
    //    }
    //    else
    //    {
    //        int countSize = Comon.cInt(Lip.GetValue("Select count(SizeID) from Stc_ItemUnits where ItemID=" + Comon.cLong(ItemID)));
    //        if (countSize == 1)
    //            Qty = Comon.cDec(remindUnitQty);
    //        else
    //            Qty = Comon.cLong(Math.Floor(remindUnitQty));
    //    }
    //    return Qty;
    //}
    //public static decimal GetRemindQTY(int ItemID, int SizeID, double StoreID)
    //{
    //    DataTable packingQtyMaxmum = Lip.SelectRecord("SELECT * FROM Stc_ItemUnits WHERE ItemID=" + Comon.cLong(ItemID) + " AND SizeID > " + SizeID + " ORDER BY PackingQty ASC");
    //    decimal packingQtyThisSize = Comon.cDec(Lip.GetValue("SELECT PackingQty FROM Stc_ItemUnits WHERE ItemID=" + Comon.cLong(ItemID) + " AND SizeID=" + SizeID));
    //    decimal remindUnitQty = Comon.cDec(RemindQtyItemByMinUnit(ItemID, SizeID, StoreID));
    //    decimal Qty = 0;
    //    if (packingQtyMaxmum.Rows.Count > 0)
    //    {
    //        remindUnitQty = Comon.cDec(RemindQtyItemByMinUnit(ItemID, Comon.cInt(packingQtyMaxmum.Rows[0]["SizeID"].ToString()), StoreID));
    //        decimal lastPackingQty = Comon.cDec(packingQtyMaxmum.Rows[packingQtyMaxmum.Rows.Count - 1]["PackingQty"].ToString());
    //        decimal decimalPart = Comon.cDec(remindUnitQty % 1);
    //        Qty += Comon.cDec(decimalPart * lastPackingQty);
    //        decimalPart = Comon.cDec(Qty % 1);
    //        if (packingQtyThisSize == Comon.cDec(Lip.GetValue("SELECT Max(PackingQty) FROM Stc_ItemUnits WHERE ItemID=" + Comon.cLong(ItemID))))
    //            Qty = (Comon.cDec(decimalPart) * Comon.cDec(packingQtyThisSize));
    //        else
    //            Qty = Math.Floor(Comon.cDec(decimalPart) * Comon.cDec(packingQtyThisSize));
    //    }
    //    else
    //    {
    //        int countSize = Comon.cInt(Lip.GetValue("SELECT COUNT(SizeID) FROM Stc_ItemUnits WHERE ItemID=" + Comon.cLong(ItemID)));
    //        Qty = countSize == 1 ? Comon.cDec(remindUnitQty) : Math.Floor(remindUnitQty);
    //    }
    //    return Qty;
    //}
    public static decimal GetRemindQTY(int ItemID, int SizeID, double StoreID)
    {

        decimal packingQtyThisSize = Comon.cDec(Lip.GetValue("SELECT PackingQty FROM Stc_ItemUnits WHERE ItemID=" + Comon.cLong(ItemID) + " and BranchID="+Edex.Model.MySession.GlobalBranchID+" AND SizeID=" + SizeID));
        DataTable packingQtyMaxmum = null;

        packingQtyMaxmum = Lip.SelectRecord("SELECT * FROM Stc_ItemUnits WHERE ItemID=" + Comon.cLong(ItemID) + " and BranchID="+Edex.Model.MySession.GlobalBranchID +" AND PackingQty <" + packingQtyThisSize + " ORDER BY PackingQty asc");

        decimal remindUnitQty = Comon.cDec(RemindQtyItemByMinUnit(ItemID, SizeID, StoreID));
        decimal Qty = 0;
        if (packingQtyMaxmum.Rows.Count > 0)
        {
            remindUnitQty = Comon.cDec(RemindQtyItemByMinUnit(ItemID, Comon.cInt(packingQtyMaxmum.Rows[0]["SizeID"].ToString()), StoreID));
            decimal lastPackingQty = Comon.cDec(packingQtyMaxmum.Rows[packingQtyMaxmum.Rows.Count - 1]["PackingQty"].ToString());
            decimal decimalPart = Comon.cDec(remindUnitQty % 1);
            Qty += Comon.cDec(decimalPart * lastPackingQty);
            decimalPart = Comon.cDec(Qty % 1);
            if (packingQtyThisSize == Comon.cDec(Lip.GetValue("SELECT Max(PackingQty) FROM Stc_ItemUnits WHERE ItemID=" + Comon.cLong(ItemID)+" and BranchID="+ Edex.Model.MySession.GlobalBranchID)))
                Qty = (Comon.cDec(decimalPart) * Comon.cDec(packingQtyThisSize));
            else
                Qty = Math.Floor(Comon.cDec(decimalPart) * Comon.cDec(packingQtyThisSize));
        }
        else
        {
            int countSize = Comon.cInt(Lip.GetValue("SELECT COUNT(SizeID) FROM Stc_ItemUnits WHERE ItemID=" + Comon.cLong(ItemID) + " and BranchID=" + Edex.Model.MySession.GlobalBranchID));
            Qty = countSize == 1 ? Comon.cDec(remindUnitQty) : Math.Floor(remindUnitQty);
        }
        return Qty;
    }
    public static decimal AverageUnit(long ItemID, int SizeID, double StoreID)
    {
        string filter = " Stc_ItemsMoviing.MoveType=1  and Stc_ItemsMoviing.BranchID = " + Edex.Model.MySession.GlobalBranchID + " AND Stc_ItemsMoviing.Cancel =0  and ";
        if(ItemID>0)
          filter += "  Stc_ItemsMoviing.ItemID =" + ItemID + "  And ";
        //if(SizeID>0)
        //    filter = "  Stc_ItemsMoviing.SizeID =" + SizeID + "  And ";
        //if (StoreID>0)
        //    filter += "  Stc_ItemsMoviing.StoreID =" +StoreID + "  And ";
        filter = filter.Remove(filter.Length - 4, 4);
        string StrSQL="SELECT  * FROM  Stc_ItemsMoviing  where "+filter;
        DataTable dt = Lip.SelectRecord(StrSQL);
        decimal AverageCostPrice = 0;
        decimal QtyinUnit = QtyItemByMinUnitToAverageCost(ItemID, SizeID, 0);
        decimal TotalCost = 0;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            TotalCost += Comon.cDec(Comon.cDec(dt.Rows[i]["QTY"].ToString()) * Comon.cDec(dt.Rows[i]["InPrice"].ToString()));
            
        }
        if (QtyinUnit>0)
          AverageCostPrice = Comon.ConvertToDecimalPrice(TotalCost / QtyinUnit);
        return AverageCostPrice;
    }
    public static decimal QtyItemByMinUnitOneFromStart(long ItemID, int SizeID, double StoreID)
    {
        decimal value = 0;
        decimal REmindQty = 0;
        string filter = "Stc_ItemsMoviing.BranchID = " + Edex.Model.MySession.GlobalBranchID + " AND Stc_ItemsMoviing.Cancel =0 AND  Stc_ItemsMoviing.ItemID =" + ItemID + "  And ";
        //if (StoreID == 0)
        //    StoreID = Comon.cInt(MySession.GlobalDefaultStoreID.ToString());

        if (StoreID > 0)
            filter = filter + " dbo.Stc_ItemsMoviing.StoreID  =" + Comon.cDbl(StoreID) + "  And ";

        if (SizeID > 0)
            filter = filter + " dbo.Stc_ItemsMoviing.SizeID  =" + Comon.cDbl(SizeID) + "  And ";

        filter = filter.Remove(filter.Length - 4, 4);
        string StrSQL = @"SELECT 
            dbo.Stc_ItemsMoviing.BarCode,
            '' as StoreName, 
            dbo.Stc_ItemsMoviing.ItemID,
            ISNULL(SUM(CASE WHEN dbo.Stc_ItemsMoviing.MoveType = 1 THEN dbo.Stc_ItemsMoviing.QTY ELSE -0 END), 0) AS QtyBalance,
            dbo.Stc_SizingUnits.ArbName AS SizeName,
            dbo.Stc_SizingUnits.SizeID, 
            dbo.Stc_ItemsMoviing.BranchID 
            FROM   dbo.Stc_SizingUnits  LEFT JOIN  
            dbo.Stc_ItemsMoviing ON dbo.Stc_SizingUnits.SizeID = dbo.Stc_ItemsMoviing.SizeID and dbo.Stc_SizingUnits.BranchID = dbo.Stc_ItemsMoviing.BranchID
            WHERE  " + filter + " GROUP BY  dbo.Stc_ItemsMoviing.BarCode,    dbo.Stc_ItemsMoviing.BranchID,   dbo.Stc_SizingUnits.ArbName,   dbo.Stc_ItemsMoviing.SizeID,   dbo.Stc_ItemsMoviing.ItemID,  dbo.Stc_SizingUnits.SizeID  ";

        DataTable ItemdtBalance = Lip.SelectRecord(StrSQL);
        decimal rqTY = 0;
        if (ItemdtBalance.Rows.Count > 0)
            rqTY = Comon.cDec(ItemdtBalance.Rows[0]["QtyBalance"].ToString());

        return rqTY;
    }
    public static decimal QtyItemByMinUnitToAverageCost(long ItemID, int SizeID, double StoreID)
    {
        decimal value = 0;
        decimal REmindQty = 0;
        string filter = "   Stc_ItemUnits.ItemID =" + ItemID + " and Stc_ItemUnits.BranchID="+Edex.Model.MySession.GlobalBranchID+"  And ";
        filter = filter.Remove(filter.Length - 4, 4);
        string StrSQL = @"SELECT ItemID,SizeID,PackingQty , 0.0 AS QTY, 0.0 AS Balance from  dbo.Stc_ItemUnits   WHERE  " + filter + "    Order by     PackingQty  ";
        DataTable ItemdtBalance = Lip.SelectRecord(StrSQL);
        ItemdtBalance.AcceptChanges();
        ItemdtBalance.Columns["QTY"].ReadOnly = false;
        ItemdtBalance.Columns["Balance"].ReadOnly = false;
        //يمكن التكرار بعدد المخازن 
        decimal Qty = 0;
        decimal Pakeg = 0;
        decimal rqTY = 0;
        int OrderSize = 0;
        decimal PakegUnits = 0; //الباكج للوحدة المطلوبة 
        for (int j = 0; j <= ItemdtBalance.Rows.Count - 1; j++)
        {
            int SiziunitID = Comon.cInt(ItemdtBalance.Rows[j]["SizeID"].ToString());
            rqTY = QtyItemByMinUnitOneFromStart(ItemID, SiziunitID, StoreID);
            ItemdtBalance.Rows[j]["QTY"] = rqTY.ToString();
            if (SizeID == SiziunitID)
                OrderSize = j;
        }
        decimal p = 0;
        decimal totalqty = 0;
        for (int j = 0; j <= ItemdtBalance.Rows.Count - 1; j++)
        {
            rqTY = Comon.cDec(ItemdtBalance.Rows[j]["QTY"]);
            p = rqTY;

            for (int i = j + 1; i <= ItemdtBalance.Rows.Count - 1; i++)
            {

                Pakeg = Comon.cDec(ItemdtBalance.Rows[i]["PackingQty"]);
                p = p * Pakeg;
                PakegUnits = Comon.cDec(ItemdtBalance.Rows[j]["PackingQty"]);
            }

            ItemdtBalance.Rows[j]["Balance"] = p.ToString();
            totalqty = Comon.cDec(totalqty + p);

        }
        for (int j = ItemdtBalance.Rows.Count - 1; j >= OrderSize + 1; j--)
        {
            PakegUnits = Comon.cDec(ItemdtBalance.Rows[j]["PackingQty"]);
            totalqty = totalqty / PakegUnits;
        }

        REmindQty = Comon.cDec(totalqty);
        return REmindQty;
    }
   
    public static decimal RemindQtyItemByMinUnitOne(long ItemID, int SizeID, double StoreID)
    {
        decimal value = 0;
        decimal REmindQty = 0;
        string filter = " Stc_ItemsMoviing.BranchID = " + Edex.Model.MySession.GlobalBranchID + " AND Stc_ItemsMoviing.Cancel =0     AND  Stc_ItemsMoviing.ItemID =" + ItemID + "  And ";
        if (StoreID == 0)
            StoreID = Comon.cInt(MySession.GlobalDefaultStoreID.ToString());

        if (StoreID > 0)
            filter = filter + " dbo.Stc_ItemsMoviing.StoreID  =" + Comon.cDbl(StoreID) + "  And ";

        if (SizeID > 0)
            filter = filter + " dbo.Stc_ItemsMoviing.SizeID  =" + Comon.cDbl(SizeID) + "  And ";

        filter += " Stc_ItemsMoviing.Posted=3  And ";

        filter = filter.Remove(filter.Length - 4, 4);
        string StrSQL = @"SELECT 
            dbo.Stc_ItemsMoviing.BarCode,
            '' as StoreName, 
            dbo.Stc_ItemsMoviing.ItemID,
            ISNULL(SUM(CASE WHEN dbo.Stc_ItemsMoviing.MoveType = 1 THEN dbo.Stc_ItemsMoviing.QTY ELSE -dbo.Stc_ItemsMoviing.QTY END), 0) AS QtyBalance,
            dbo.Stc_SizingUnits.ArbName AS SizeName,
            dbo.Stc_SizingUnits.SizeID,
            dbo.Stc_ItemsMoviing.StoreID,
            dbo.Stc_ItemsMoviing.BranchID 
            FROM   dbo.Stc_SizingUnits  LEFT JOIN  
            dbo.Stc_ItemsMoviing ON dbo.Stc_SizingUnits.SizeID = dbo.Stc_ItemsMoviing.SizeID and dbo.Stc_SizingUnits.BranchID = dbo.Stc_ItemsMoviing.BranchID
            WHERE  " + filter + " GROUP BY  dbo.Stc_ItemsMoviing.BarCode,  dbo.Stc_ItemsMoviing.StoreID, dbo.Stc_ItemsMoviing.BranchID,   dbo.Stc_SizingUnits.ArbName,   dbo.Stc_ItemsMoviing.SizeID,   dbo.Stc_ItemsMoviing.ItemID,  dbo.Stc_SizingUnits.SizeID  ";

        DataTable ItemdtBalance = Lip.SelectRecord(StrSQL);
        decimal rqTY = 0;
        if (ItemdtBalance.Rows.Count > 0)
            rqTY = Comon.cDec(ItemdtBalance.Rows[0]["QtyBalance"].ToString());

        return rqTY;
    }
    public static decimal RemindQtyItemByMinUnit(long ItemID, int SizeID, double StoreID)
    {
        decimal value = 0;
        decimal REmindQty = 0;
        string filter = "   Stc_ItemUnits.ItemID =" + ItemID + " and Stc_ItemUnits.BranchID=" + Edex.Model.MySession.GlobalBranchID + "  And ";
        filter = filter.Remove(filter.Length - 4, 4);
        string StrSQL = @"SELECT ItemID,SizeID,PackingQty , 0.0 AS QTY, 0.0 AS Balance from  dbo.Stc_ItemUnits   WHERE  " + filter + "    Order by     PackingQty  ";
        DataTable ItemdtBalance = Lip.SelectRecord(StrSQL);
        ItemdtBalance.AcceptChanges();
        ItemdtBalance.Columns["QTY"].ReadOnly = false;
        ItemdtBalance.Columns["Balance"].ReadOnly = false;
        //يمكن التكرار بعدد المخازن 
        decimal Qty = 0;
        decimal Pakeg = 0;
        decimal rqTY = 0;
        int OrderSize = 0;
        decimal PakegUnits = 0; //الباكج للوحدة المطلوبة 
        for (int j = 0; j <= ItemdtBalance.Rows.Count - 1; j++)
        {
            int SiziunitID = Comon.cInt(ItemdtBalance.Rows[j]["SizeID"].ToString());
            rqTY = RemindQtyItemByMinUnitOne(ItemID, SiziunitID, StoreID);
            ItemdtBalance.Rows[j]["QTY"] = rqTY.ToString();
            if (SizeID == SiziunitID)
                OrderSize = j;
        }
        decimal p = 0;
        decimal totalqty = 0;
        for (int j = 0; j <= ItemdtBalance.Rows.Count - 1; j++)
        {
            rqTY = Comon.cDec(ItemdtBalance.Rows[j]["QTY"]);
            p = rqTY;

            for (int i = j + 1; i <= ItemdtBalance.Rows.Count - 1; i++)
            {

                Pakeg = Comon.cDec(ItemdtBalance.Rows[i]["PackingQty"]);
                p = p * Pakeg;
                PakegUnits = Comon.cDec(ItemdtBalance.Rows[j]["PackingQty"]);
            }

            ItemdtBalance.Rows[j]["Balance"] = p.ToString();
            totalqty = Comon.cDec(totalqty + p);

        }
        for (int j = ItemdtBalance.Rows.Count - 1; j >= OrderSize + 1; j--)
        {
            PakegUnits = Comon.cDec(ItemdtBalance.Rows[j]["PackingQty"]);
            totalqty = totalqty / PakegUnits;
        }

        REmindQty = Comon.cDec(totalqty);
        return REmindQty;
    }
    


    public static DataTable ChekRemidQTY(string BarCode,double StoreID,int BranchID,int CostCenterID)
    {
      
        try
        {
             
            string filter = "";
            filter = "( dbo.Stc_ItemsMoviing.BranchID = " + BranchID + ") AND dbo.Stc_ItemsMoviing.Cancel =0   AND  dbo.Stc_ItemsMoviing.ItemID >0  AND";
            strSQL = "";
            //if (CostCenterID>0)
            //    filter += "  dbo.Stc_ItemsMoviing.CostCenterID=" + Comon.cInt(CostCenterID) + " AND ";


            if (BarCode != string.Empty)
                filter = filter + " dbo.Stc_ItemsMoviing.BarCode  ='" + BarCode + "'  AND ";
            if (StoreID >0)
                filter = filter + " dbo.Stc_ItemsMoviing.StoreID  =" + Comon.cDbl(StoreID) + "  And ";
           
           
            filter = filter.Remove(filter.Length - 4, 4);
            strSQL = " SELECT  dbo.Stc_ItemsMoviing.BarCode,'' as StoreName, dbo.Stc_ItemsMoviing.ItemID,"
                + "  SUM(CASE WHEN dbo.Stc_ItemsMoviing.DocumentTypeID = 15 THEN dbo.Stc_ItemsMoviing.QTY ELSE 0 END) AS QtyOpening, "
                + "  SUM(CASE WHEN dbo.Stc_ItemsMoviing.MoveType = 1 AND dbo.Stc_ItemsMoviing.DocumentTypeID <> 15 THEN dbo.Stc_ItemsMoviing.QTY ELSE 0 END) AS QtyIncomming, "
                + " SUM(CASE WHEN dbo.Stc_ItemsMoviing.MoveType = 2 THEN dbo.Stc_ItemsMoviing.QTY ELSE 0 END) AS QtyOut, "
                + "   SUM(CASE WHEN dbo.Stc_ItemsMoviing.MoveType = 1 THEN dbo.Stc_ItemsMoviing.QTY ELSE -dbo.Stc_ItemsMoviing.QTY END) AS QtyBalance, "
                + "  SUM(CASE WHEN dbo.Stc_ItemsMoviing.DocumentTypeID = 15 OR dbo.Stc_ItemsMoviing.DocumentTypeID = 23 THEN (dbo.Stc_ItemsMoviing.InPrice + dbo.Stc_ItemsMoviing.Bones) ELSE 0 END) / "
                + "  NULLIF(SUM(CASE WHEN dbo.Stc_ItemsMoviing.DocumentTypeID = 15 OR dbo.Stc_ItemsMoviing.DocumentTypeID = 23 THEN dbo.Stc_ItemsMoviing.QTY ELSE 0 END), 0) AS AverageCost, "
                + " dbo.Stc_SizingUnits.ArbName AS SizeName, dbo.Stc_SizingUnits.SizeID,dbo.Stc_ItemsMoviing.StoreID,dbo.Stc_ItemsMoviing.BranchID  FROM dbo.Stc_ItemsMoviing "
                + " LEFT OUTER JOIN dbo.Stc_SizingUnits ON dbo.Stc_ItemsMoviing.SizeID = dbo.Stc_SizingUnits.SizeID "
                + " WHERE  " + filter
                + " GROUP BY dbo.Stc_ItemsMoviing.BarCode,dbo.Stc_ItemsMoviing.StoreID ,dbo.Stc_ItemsMoviing.BranchID, dbo.Stc_SizingUnits.ArbName, dbo.Stc_ItemsMoviing.SizeID,dbo.Stc_ItemsMoviing.ItemID,dbo.Stc_SizingUnits.SizeID  ";

            Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());
            return Lip.SelectRecord(strSQL);
        }
        catch (Exception ex)
        {
            return null;  
        }
    }
    public static decimal GetQTYinCommandToThisItem(string TableNameDetils, string TableNameMaster, string FildNameQTY, string FildNameCommand, int CommandID, string ItemID, string where = "", string BarCode = "BarCode", int SizeID = 1)
    {
        string str = " select  sum(" + FildNameQTY + ") as " + FildNameQTY + ",Stc_ItemUnits.PackingQty ," + TableNameDetils + ".SizeID  from " + TableNameDetils + " INNER JOIN  " + TableNameMaster + " ON " + TableNameDetils + "." + FildNameCommand + "= " + TableNameMaster + "." + FildNameCommand + @"
           left outer join Stc_ItemUnits on " + TableNameDetils + "." + BarCode + @"= Stc_ItemUnits.BarCode 
           where    Stc_ItemUnits.BranchID=" + Edex.Model.MySession.GlobalBranchID + " and " + TableNameMaster + ".Cancel=0 and " + TableNameMaster + "." + FildNameCommand + "=" + CommandID + " and " + TableNameDetils + ".ItemID=" + ItemID + where + " group by PackingQty," + TableNameDetils + ".SizeID  order by PackingQty";
        DataTable ItemdtBalanceAll = Lip.SelectRecord(str);
        decimal Qty = 0;
        
        int OrderSize = 0;
        decimal PakegUnits = 0;
        decimal p = 0;
        decimal totalqty = 0;
        decimal REmindQty = 0;
        decimal rqTY = 0;
        decimal Pakeg = 0;
        if (ItemdtBalanceAll.Rows.Count > 0)
        {
            string StrSQL = @"SELECT ItemID,SizeID,PackingQty , 0.0 AS  " + FildNameQTY + "  from  dbo.Stc_ItemUnits   WHERE  ItemID=" + ItemID + " and BranchID=" + Edex.Model.MySession.GlobalBranchID+"    Order by     PackingQty  ";
            DataTable ItemdtBalance = Lip.SelectRecord(StrSQL);
            ItemdtBalance.Columns[FildNameQTY].ReadOnly = false;
            for (int j = 0; j <= ItemdtBalance.Rows.Count - 1; j++)
            {
                int SiziunitID = Comon.cInt(ItemdtBalance.Rows[j]["SizeID"].ToString());
                for (int k = 0; k <= ItemdtBalanceAll.Rows.Count - 1; k++)
                {
                    int sortedSizeID = Comon.cInt(ItemdtBalanceAll.Rows[k]["SizeID"].ToString());
                    if (SiziunitID == sortedSizeID)
                    {
                        p = Comon.cDec(ItemdtBalanceAll.Rows[k][FildNameQTY].ToString());
                        ItemdtBalance.Rows[j][FildNameQTY] = p.ToString();
                        break;
                    }
                    if (SizeID == SiziunitID)
                        OrderSize = j;
                }
            }
            for (int j = 0; j <= ItemdtBalance.Rows.Count - 1; j++)
            {
                rqTY = Comon.cDec(ItemdtBalance.Rows[j][FildNameQTY]);
                p = rqTY;
                int SiziunitID = Comon.cInt(ItemdtBalance.Rows[j]["SizeID"].ToString());
                if (SizeID == SiziunitID && p != 0)
                {
                    totalqty += p;
                    return Comon.ConvertToDecimalPrice(totalqty);                                                                                                                                    
                }
                for (int i = j + 1; i <= ItemdtBalance.Rows.Count - 1; i++)
                {
                    Pakeg = Comon.cDec(ItemdtBalance.Rows[i]["PackingQty"]);
                    p = p * Pakeg;
                    PakegUnits = Comon.cDec(ItemdtBalance.Rows[j]["PackingQty"]);
                }
                totalqty = Comon.cDec(totalqty + p);

            }
            for (int j = ItemdtBalance.Rows.Count - 1; j >= OrderSize + 1; j--)
            {
                PakegUnits = Comon.cDec(ItemdtBalance.Rows[j]["PackingQty"]);
                totalqty = totalqty / PakegUnits;
            }

            REmindQty = Comon.ConvertToDecimalPrice(totalqty);

        }
            REmindQty = Comon.cDec(totalqty);
            return REmindQty;
              
    }

   public static bool CheckDateISAvilable(string Date1)
    {
        DateTime dateOne = Comon.ConvertSerialToDate(Date1);
        DateTime dateTwo = Comon.ConvertSerialToDate(Lip.GetServerDate().ToString());
        TimeSpan timeSpan = dateTwo - dateOne;
        long Diff = (long)timeSpan.TotalDays;
         return Diff < 0;
    }
    public static bool CheckTheProcessesIsPosted(string TableName,int BranchID,int CurrentPosted,long PrimaryID, string PrimeryColName="InvoiceID",string Where=" ")
     {
         int PostedPervious = Comon.cInt(Lip.GetValue("select Posted From " + TableName + " where Cancel=0 and BranchID=" + BranchID + " and " + PrimeryColName + "=" + PrimaryID + Where));
       if (CurrentPosted >= PostedPervious)
           return true;
       return false;
     }


    
}

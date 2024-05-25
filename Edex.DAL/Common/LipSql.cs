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
//using MySql.Data.MySqlClient;

/// <summary>
/// Summary description for Common
/// </summary>

public static class Lip
    {


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
        public static DataTable SelectRecord(string StrSQL)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.Text;
                    objCmd.CommandText = StrSQL;
                     SqlDataReader myreader = objCmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(myreader);
                    return dt;
                }
            }
        }


    

    public static string GetValue(string StrSQL)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.Text;
                    objCmd.CommandText = StrSQL;
                    SqlDataReader myreader = objCmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(myreader);
                    try
                    {
                        if (dt != null && dt.Rows.Count > 0)
                            return dt.Rows[0][0].ToString();
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
        public static void  ConvertStrSQLToEnglishOrArabicLanguage(string strSQL, string lang)
        {
            if (UserInfo.Language.ToString() == iLanguage.English.ToString() )
            {
                strSQL = strSQL.ToUpper().Replace("ARBNAME", "ENGNAME");
                strSQL = strSQL.ToUpper().Replace("AccountArbName", "ACCOUNTENGNAME");
                


            }
             
             
        }

        public static string ConvertStrSQLLanguage(string strSQL, string lang)
        {
            if (UserInfo.Language.ToString() == iLanguage.English.ToString())
            {
                strSQL = strSQL.ToUpper().Replace("ARBNAME", "ENGNAME");
                strSQL = strSQL.ToUpper().Replace("AccountArbName", "ACCOUNTENGNAME");



            }
            return strSQL;

        }





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
        public static bool IsNumeric(this String str)
        {
            try
            {
                Double.Parse(str.ToString());
                return true;
            }
            catch
            {
            }
            return false;
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
            // private constructor
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

            

            // **** add your session properties here, e.g like this:
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
        public  static string[] PubArrParameters = new string[30];
        public  static long GlobalBRANCH_ID;
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
        public static void NewFields()
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
        public static void AddNumericField(string Filedname, string FieldValue)
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
        public static void AddNumericField(string Filedname, int FieldValue)
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
        public static void AddStringField(string Filedname, string FieldValue)
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
        public static string GetInsertQuary()
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
        public static void ExecuteInsert()
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
        public static int ExecuteProcedure(string procedureName, string ParmName = "")
        {
            int iReturnValue=0;

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
        public static string GetUpdateStr()
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
        public static void ExecuteUpdate()
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
        public static void ExecuteDelete()
        {
            string sDel = "Delete from " + Table + " where " + sCondition;
            ExecututeSQL(sDel);
        }
        public static string GetServerDate()
        {
            string functionReturnValue = null;
            functionReturnValue = SelectRecord("Select GetDate()").Rows[0][0].ToString();
            System.Globalization.DateTimeFormatInfo GgeorgianDTF = new System.Globalization.CultureInfo("en-US", false).DateTimeFormat;
            GgeorgianDTF.Calendar = new System.Globalization.GregorianCalendar();
            GgeorgianDTF.ShortDatePattern = "yyyy/MM/dd";
            GgeorgianDTF.MonthDayPattern = "MMMM";
            functionReturnValue = Comon.cDate(functionReturnValue).ToString("d", GgeorgianDTF);
            return functionReturnValue;

        }
        public static string GetServerDateSerial()
        {
            string functionReturnValue = null;
           
            functionReturnValue = SelectRecord("Select GetDate()").Rows[0][0].ToString();
            System.Globalization.DateTimeFormatInfo GgeorgianDTF = new System.Globalization.CultureInfo("en-US", false).DateTimeFormat;
            GgeorgianDTF.Calendar = new System.Globalization.GregorianCalendar();
            GgeorgianDTF.ShortDatePattern = "yyyyMMdd";
            GgeorgianDTF.MonthDayPattern = "MMMM";
            functionReturnValue = Comon.cDate(functionReturnValue).ToString("d", GgeorgianDTF);
            return functionReturnValue;
        }
        public static string GetServerTime()
        {
            string functionReturnValue = null;
            functionReturnValue = SelectRecord("SELECT CONVERT (time, SYSDATETIME())").Rows[0][0].ToString();
            functionReturnValue = (functionReturnValue.Substring(0, 2) + functionReturnValue.Substring(2, 3));
            return functionReturnValue;
        }
        public static string GetServerTimeSerial()
        {
            string functionReturnValue = null;
            functionReturnValue = SelectRecord("SELECT CONVERT (time, SYSDATETIME())").Rows[0][0].ToString().Replace(':', ' ');
            functionReturnValue = (functionReturnValue.Substring(0, 2) + functionReturnValue.Substring(3, 3));
            return functionReturnValue.Trim();
        }

        public static string ToWords(decimal Num, int CurencyID)
        {

            List<CurrencyInfo> currencies = new List<CurrencyInfo>();
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Syria));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.UAE));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Tunisia));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Gold));
            ToWord toWord = new ToWord(Convert.ToDecimal(Num.ToString()), currencies[2]);
            if (UserInfo.Language.ToString() == iLanguage.English.ToString())
                return toWord.ConvertToEnglish();
            else
                return toWord.ConvertToArabic();
        }
     
    }

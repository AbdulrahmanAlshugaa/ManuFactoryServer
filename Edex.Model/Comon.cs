using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Globalization;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Edex.Model.Language;

namespace Edex.Model
{
    /// <summary>
    /// Summary description for Common
    /// </summary>
    public static class Comon
    {
        public const string FilterOpenSpan = "<span style='background-color:Yellow;'>";
        public const string FilterCloseSpan = "</span>";
        public static string SearchArabicNames;
        /// <summary>
        /// This method takes in a weight in Whight units and a Cliperion value, and converts the weight to 21Calipar units
        /// </summary>
        /// <param name="Whight"></param>
        /// <param name="Cliperion"></param>
        /// <param name="GlobalGoldCliperionTrans"></param>
        /// <returns></returns>
        public static decimal ConvertTo21Caliber(decimal Whight, int Caliberion, int GlobalGoldCliperionTrans = 18)
        {
            // Convert Cliperion to equivalent in milligrams
            Caliberion = 1000 * Caliberion / 24;

            decimal d = 0;
            // Convert GlobalGoldCliperionTrans to equivalent in milligrams
            d = 1000 * GlobalGoldCliperionTrans / 24;

            // Calculate the conversion ratio between the two units and multiply it with the weight to get the converted weight in 21Caliber
            decimal ConvertTo = Comon.cDec(Comon.cDec(Comon.cDec(Whight) * Caliberion )/ d);

            // Return the converted weight in 21Caliber units
            return ConvertTo;
        }

     
        
        public static DateTime cDateTimeV2(object p)
        {
            try
            {
                string isyear = p.ToString().Substring(0, 4);
                if (cInt(isyear) > 0)
                {
                    return cDate(p);
                }
                string day = p.ToString().Substring(0, 2);
                if (day.Length == 1)
                    day = "0" + day;

                string month = p.ToString().Substring(3, 2);
                if (month.Length == 1)
                    month = "0" + month;

                string year = p.ToString().Substring(6, 4);
                if (year.Length == 1)
                    year = "0" + year;
                string[] str = p.ToString().Split(' ');

                string hh = p.ToString().Substring(11, 2);
                hh = hh.Length == 1 ? "0" + hh : hh;

                string mm = p.ToString().Substring(14, 2);
                mm = mm.Length == 1 ? "0" + mm : mm;

                string ss = DateTime.Now.Second.ToString();
                ss = ss.Length == 1 ? "0" + ss : ss;

                string dd = hh + ":" + mm + ":" + ss;
                var _time = DateTime.ParseExact(dd, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);


                var _day = DateTime.ParseExact(day, "dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                var _month = DateTime.ParseExact(month, "MM", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                var _year = DateTime.ParseExact(year, "yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);

                return new DateTime(_year.Year, _month.Month, _day.Day, _time.Hour, _time.Minute, _time.Second);

            }
            catch (Exception exp)
            {
                return cDate(p);

            }
        }
        /// <summary>
        /// This Function is used to Convert serial To Time 
        /// </summary>
        /// <param name="TheSerial"></param>
        /// <returns></returns>
        public static string ConvertSerialToTime(string TheSerial)
        {
            // initialize the output string variable
            string ConvertSerialToTime = "";

            // declare and initialize variables for hour and AM/PM designation
            int TheHour;
            string AMorPM = "";

            // normalize the incoming time string to four digits
            if (TheSerial.Length == 1) TheSerial = "1200";
            if (TheSerial.Length == 3) TheSerial = "0" + TheSerial;
            if (TheSerial.Length == 2) TheSerial = "00" + TheSerial;

            // extract the hour from the time string and determine AM/PM
            TheHour = Comon.cInt(TheSerial.Substring(0, 2));
            if (TheHour > 12 && TheHour <= 23)
            {
                TheHour = TheHour - 12;
                AMorPM = (UserInfo.Language == iLanguage.Arabic ? " م " : " PM ");
            }
            else if (TheHour < 12 && TheHour > 0)
                AMorPM = (UserInfo.Language == iLanguage.Arabic ? "ص  " : " AM ");
            else if (TheHour == 0)
            {
                TheHour = 12;
                AMorPM = (UserInfo.Language == iLanguage.Arabic ? "ص  " : " AM ");
            }
            else if (TheHour == 12)
                AMorPM = (UserInfo.Language == iLanguage.Arabic ? " م " : " PM ");

            // format the hour and minute as a string with AM/PM
            if (TheHour < 10)
                ConvertSerialToTime = "0" + TheHour + ":" + TheSerial.Substring(2, 2) + AMorPM;
            else
                ConvertSerialToTime = TheHour + ":" + TheSerial.Substring(2, 2) + AMorPM;

            // replace Arabic AM/PM designations with English ones
            ConvertSerialToTime = ConvertSerialToTime.Replace("م", "PM");
            ConvertSerialToTime = ConvertSerialToTime.Replace("ص", "AM");

            // return the final formatted time string
            return ConvertSerialToTime;
        }

        /// <summary>
        /// This function removes HTML tags from a given string and returns the cleaned up string
        /// </summary>
        /// <param name="_key"></param>
        /// <returns></returns>
        public static string MangeKey(string _key)
        {
            // Remove any occurrences of the opening filter span tag from the input string
            string pKey = _key.Replace(FilterOpenSpan, "");

            // Remove any occurrences of the HTML non-breaking space code from the result of the previous line
            pKey = pKey.Replace("&nbsp;", "");

            // Remove any occurrences of the closing filter span tag from the result of the previous line, yielding the final cleaned up string
            return pKey.Replace(FilterCloseSpan, "");
        }

        /// <summary>
        /// This function takes a string representation of a date and returns a formatted string in the format "yyyy-MM-dd"
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string GetDateFromat(string p)
        {
            // if the input string is empty, return a default starting date
            if (p == "")
                return "1429-01-01";

            // convert the input string to a DateTime object
            DateTime ddate = Convert.ToDateTime(p);

            // return the DateTime object formatted as a string using the "yyyy-MM-dd" format
            return ddate.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// This Function is used to Convert any object to Int 
        /// </summary>
        /// <param name="p"></param>
        /// <returns>return value int</returns>
        
        public static int  cInt(object p)
        {

            int result = -1;

            try
            {
               
                if (p != null)
                {
                    Int32.TryParse(p.ToString(), out result);
                }
            }
            catch (Exception e)
            {
                result = -1;
            }

            return result;
        }
        /// <summary>
        /// This Function is used to Convert value boolen to int
        /// </summary>
        /// <param name="p"></param>
        /// <returns>return int value :1 if the value is True , 0  else other value </returns>
        public static int cBooleanToInt(bool p)
        {

            int result = 0;

            try
            {

                if (p== true)
                {
                    result = 1;
                }
            }
            catch (Exception e)
            {
                result = 0;
            }
            return result;
        }

        /// <summary>
        /// This function is used to Convert value int to boolen
        /// </summary>
        /// <param name="p"></param>
        /// <returns>return value boolen: True if the value is equal to 1,false else other value </returns>
        public static bool cIntToBoolean(int p)
        {

            bool result = false;

            try
            {
                if (p == 1)
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                result = false;
            }

            return result;
        }
        /// <summary>
        /// This Function is used to Convert any object To Short 
        /// </summary>
        /// <param name="p"></param>
        /// <returns>return Short value</returns>
        public static short cShort(object p)
        {

            short result = -1;

            try
            {
                if (p != null)
                {
                    Int16.TryParse(p.ToString(), out result);
                }
            }
            catch (Exception e)
            {
                result = -1;
            }

            return result;
        }
        /// <summary>
        /// This functoin is used to Convert any object To Negative
        /// </summary>
        /// <param name="p"></param>
        /// <returns>return value int</returns>
        public static int cNegative(object p)
        {
            int result = -1;

            try
            {
                if (p != null)
                {
                    Int32.TryParse(p.ToString(), out result);
                }
            }
            catch (Exception e)
            {
                result = -1;
            }

            return result;
        }
        /// <summary>
        /// This function is used to Convert Any object To Long 
        /// </summary>
        /// <param name="p"></param>
        /// <returns>return value type of long(Int64)</returns>
        public static long cLong(object p)
        {
            try
            {
                return Convert.ToInt64(p);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// This function is used to Convert any object to Decimal
        /// </summary>
        /// <param name="Value"></param>
        /// <returns> return value decimal </returns>
        public static decimal ConvertToDecimalPrice(object Value)
        {
            try
            {
                // Check if Value is not null or empty
                if (Value != null && !string.IsNullOrEmpty(Value.ToString().Trim()))
                {
                    // Convert Value to a decimal, format it to have the correct number of digits,
                    // and then convert it back to a decimal value
                    string val = Convert.ToDecimal(Value).ToString("N" + MySession.GlobalPriceDigits);
                    return Convert.ToDecimal(val);
                }
                else
                {
                    // Return 0 if Value is null or empty
                    return 0;
                }
            }
            catch
            {
                // Return 0 if there is an exception while converting Value to a decimal
                return 0;
            }
        }
        /// <summary>
        /// This Functio is used to convert any object value  to decimal value 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns>return value decimal </returns> 
        public static decimal ConvertToDecimalCostPrice(object Value)
        {
            try
            {
                // Check if Value is not null or empty
                if (Value != null && !string.IsNullOrEmpty(Value.ToString().Trim()))
                {
                    // Convert Value to a decimal, format it to have the correct number of digits,
                    // and then convert it back to a decimal value
                    string val = Convert.ToDecimal(Value).ToString("N" + MySession.GlobalPriceDigits);
                    return Convert.ToDecimal(val);
                }
                else
                {
                    // Return 0 if Value is null or empty
                    return 0;
                }
            }
            catch
            {
                // Return 0 if there is an exception while converting Value to a decimal
                return 0;
            }
        }
       
        public static decimal ConvertToDecimalPriceTree(object Value)
        {
            try
            {
                // Check if Value is not null or empty
                if (Value != null && !string.IsNullOrEmpty(Value.ToString().Trim()))
                {
                    // Convert Value to a decimal, format it to have 3 digits,
                    // and then convert it back to a decimal value
                    string val = Convert.ToDecimal(Value).ToString("N" + 3);
                    return Convert.ToDecimal(val);
                }
                else
                {
                    // Return 0 if Value is null or empty
                    return 0;
                }
            }
            catch
            {
                // Return 0 if there is an exception while converting Value to a decimal
                return 0;
            }
        }
        /// <summary>
        /// This function is used to Convert To Decimal Qty
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static decimal ConvertToDecimalQty(object Value)
        {
            try
            {
                // Check if Value is not null or empty
                if (Value != null && !string.IsNullOrEmpty(Value.ToString().Trim()))
                {
                    // Convert Value to a decimal, format it to have the correct number of digits for quantity,
                    // and then convert it back to a decimal value
                    string val = Convert.ToDecimal(Value).ToString("N" + MySession.GlobalQtyDigits);
                    return Convert.ToDecimal(val);
                }
                else
                {
                    // Return 0 if Value is null or empty
                    return 0;
                }
            }
            catch
            {
                // Return 0 if there is an exception while converting Value to a decimal
                return 0;
            }
        }

        public static decimal ConvertToPositive(object Value)
        {
            try
            {
                if (Value != null && !string.IsNullOrEmpty(Value.ToString().Trim()) && Value.ToString() != "0" && cInt(Value) > 0 == true)
                {
                    return Convert.ToDecimal(Value);
                }
                else
                {

                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }
        public static DateTime cDate(object p)
        {
            try
            {
                //System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("es-ES");

                DateTime dat = Convert.ToDateTime(p.ToString());
                return dat;
            }
            catch
            {
                return DateTime.MaxValue;

            }
        }


        public static DateTime cDateV2(object p)
        {
            DateTime result = DateTime.MaxValue;
            try
            {
                if (p != null)
                {
                    DateTime.TryParse(p.ToString(), out result);
                }
            }
            catch (Exception e)
            {
                result = DateTime.MaxValue;
            }
            return result;
        }


        public static DateTime cDateTime(object p)
        {
            try
            {
                string isyear = p.ToString().Substring(0, 4);
                if (cInt(isyear) > 0)
                {
                    return cDate(p);
                }
                string day = p.ToString().Substring(0, 2);
                if (day.Length == 1)
                    day = "0" + day;

                string month = p.ToString().Substring(3, 2);
                if (month.Length == 1)
                    month = "0" + month;

                string year = p.ToString().Substring(6, 4);
                if (year.Length == 1)
                    year = "0" + year;
                string[] str = p.ToString().Split(' ');

                string hh = DateTime.Now.Hour.ToString();
                hh = hh.Length == 1 ? "0" + hh : hh;

                string mm = DateTime.Now.Minute.ToString();
                mm = mm.Length == 1 ? "0" + mm : mm;

                string ss = DateTime.Now.Second.ToString();
                ss = ss.Length == 1 ? "0" + ss : ss;

                string dd = hh + ":" + mm + ":" + ss;
                var _time = DateTime.ParseExact(dd, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);

                if (str.Length > 1)
                {
                    if (str.Length > 2)
                    {
                        _time = cDate(str[1] + " " + str[2]);
                    }
                    else
                    {
                        _time = DateTime.ParseExact(str[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                    }
                }
                var _day = DateTime.ParseExact(day, "dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                var _month = DateTime.ParseExact(month, "MM", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                var _year = DateTime.ParseExact(year, "yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);

                return new DateTime(_year.Year, _month.Month, _day.Day, _time.Hour, _time.Minute, _time.Second);

            }
            catch (Exception exp)
            {
                return cDate(p);

            }
        }

        public static bool cbool(object p)
        {
            try
            { 

                return Convert.ToBoolean(p);
            }
            catch
            {
                if (cInt(p) == 1)
                    return true;
                return false;
            }
        }

        public static int GetMax(string pTablename, string pField, string pWhere)
        {
            //DAL db = new DAL();
            //return Common.cInt(db.GetValue("Max(" + pField + ")", pTablename, pWhere));
            return 0;
        }
        public static int GetAutoIncrement(string pTablename, string pField, string pWhere)
        {
            int Max = GetMax(pTablename, pField, pWhere);

            return Max + 1;
        }
        public static Int64 cIntB(string p)
        {
            if (string.IsNullOrEmpty(p))
                return 0;
            else
                return Convert.ToInt64(p);

        }

        public static byte cByte(string p)
        {
            if (string.IsNullOrEmpty(p))
                return 0;
            else
                return Convert.ToByte(p);

        }
        public static Int16 cShort(string p)
        {
            if (string.IsNullOrEmpty(p))
                return 0;
            else
                return Convert.ToInt16(p);

        }
        public static Double cDbl(object p)
        {
            try
            {
                return Convert.ToDouble(p);
            }
            catch
            {
                return 0;
            }
        }
        public static Decimal cDec(object p)
        {
            try
            {
                return Convert.ToDecimal(p);
            }
            catch
            { return 0; }

        }
        public static Byte cbyte(string p)
        {
            return cByte(p);
        }

        public static object cStr(object p)
        {
            try
            {
                return p.ToString();
            }
            catch (Exception exp)
            {
                return "";
            }
        }
        public static DateTime ConvertFromHijriDateToEngDate(string hijri)
        {

            CultureInfo arSA = new CultureInfo("ar-SA");
            arSA.DateTimeFormat.Calendar = new HijriCalendar();
            return DateTime.ParseExact(hijri, "dd/MM/yyyy", arSA);
        }
        public static string ConvertFromEngDateToHijriDate(DateTime Date, string Calendar = "Hijri", string DateLangCulture = "en-US")
        {
            DateTimeFormatInfo DTFormat;
            DateLangCulture = DateLangCulture.ToLower();
            /// We can't have the hijri date writen in English. We will get a runtime error

            if (Calendar == "Hijri" && DateLangCulture.StartsWith("en-"))
            {
                DateLangCulture = "ar-sa";
            }

            /// Set the date time format to the given culture
            DTFormat = new System.Globalization.CultureInfo(DateLangCulture, false).DateTimeFormat;

            /// Set the calendar property of the date time format to the given calendar
            switch (Calendar)
            {
                case "Hijri":
                    DTFormat.Calendar = new System.Globalization.HijriCalendar();
                    break;

                case "Gregorian":
                    DTFormat.Calendar = new System.Globalization.GregorianCalendar();
                    break;

                default:
                    return "";
            }

            /// We format the date structure to whatever we want
            DTFormat.ShortDatePattern = "dd/MM/yyyy";
            return (Date.Date.ToString("f", DTFormat));
        }
        public static int ConvertDateToSerial(string TheDate)
        {
            string s;
            string s1;
            int functionReturnValue = 20190101;



            if (TheDate == "" || TheDate == "0")
            {
                functionReturnValue = 0;
                return functionReturnValue;
            }

            if (TheDate.Length < 10)
            {
                s1 = TheDate.Substring(2, 1);
                if (s1 != "/")
                {
                    s1 = "0" + TheDate.Substring(0, 1) + TheDate.Substring(1);

                    TheDate = s1;
                }
                s = TheDate.Substring(5, 1);

                if (s != "/")
                    s = TheDate.Substring(5, 4) + "0" + TheDate.Substring(3, 1) + TheDate.Substring(0, 2);
                functionReturnValue = Comon.cInt(s);

                //functionReturnValue = Comon.cInt();
                // functionReturnValue = 0;
                return functionReturnValue;
            }

            if(Comon.cInt(TheDate.Substring(0, 4))>0)
            {
                s = TheDate.Substring(0, 4) + TheDate.Substring(5, 2) + TheDate.Substring(8, 2);
                functionReturnValue = Comon.cInt(s);
            }

            if (Comon.cInt(TheDate.Substring(6, 4)) > 0)
            {
                s = TheDate.Substring(6, 4) + TheDate.Substring(3, 2) + TheDate.Substring(0, 2);
                functionReturnValue = Comon.cInt(s);
            }

            if (functionReturnValue == 0)
                functionReturnValue = 20190101;

            return functionReturnValue;

        }

        public static DateTime ConvertSerialToDate(string TheDate)
        {
            CultureInfo culture = new CultureInfo("en-US");
            DateTime functionReturnValue;

            if (TheDate == "")
            {
                functionReturnValue = new DateTime();
                return functionReturnValue;
            }

            if (TheDate == string.Empty)
            {
                functionReturnValue = new DateTime();
                return functionReturnValue;
            }

            if (TheDate.Length < 8)
            {
                functionReturnValue = new DateTime().AddYears(1000);
                functionReturnValue = DateTime.ParseExact("10/01/2023", "dd/MM/yyyy", culture);
                return functionReturnValue;
            }
            if (TheDate.Length > 8)
            {
                try
                {
                    return DateTime.ParseExact(TheDate, "yyyy/MM/dd", culture);
                }
                catch (Exception e)
                {
                    return DateTime.ParseExact(TheDate, "dd/MM/yyyy", culture);
                }

            }
            functionReturnValue = DateTime.ParseExact((TheDate.Substring(6, 2) + "/" + TheDate.Substring(4, 2) + "/" + TheDate.Substring(0, 4)).ToString(), "dd/MM/yyyy", culture);
            return functionReturnValue;

        }

        public static string ConvertSerialDateTo(string TheDate)
        {
            string functionReturnValue = "";

            if (TheDate == "")
            {
                functionReturnValue = "";
                return functionReturnValue;
            }

            if (TheDate == string.Empty)
            {
                functionReturnValue = "";
                return functionReturnValue;
            }

            if (TheDate.Length < 8)
            {
                functionReturnValue = "";
                return functionReturnValue;
            }
            functionReturnValue = (TheDate.Substring(6, 2) + "/" + TheDate.Substring(4, 2) + "/" + TheDate.Substring(0, 4)).ToString();
            return functionReturnValue;

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

        public static int InStr(int startIndex, object Value, string searchChar)
        {
            try
            {
                if (Value != null && !string.IsNullOrEmpty(Value.ToString().Trim()))
                {
                    int index = cStr(Value).ToString().IndexOf(searchChar, startIndex);
                    return index;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
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
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }


        public static List<TSource> ToList<TSource>(this DataTable dataTable) where TSource : new()
        {
            var dataList = new List<TSource>();

            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
            var objFieldNames = (from PropertyInfo aProp in typeof(TSource).GetProperties(flags)
                                 select new
                                 {
                                     Name = aProp.Name,
                                     Type = Nullable.GetUnderlyingType(aProp.PropertyType) ??
                             aProp.PropertyType
                                 }).ToList();
            var dataTblFieldNames = (from DataColumn aHeader in dataTable.Columns
                                     select new
                                     {
                                         Name = aHeader.ColumnName,
                                         Type = aHeader.DataType
                                     }).ToList();
            var commonFields = objFieldNames.Intersect(dataTblFieldNames).ToList();

            foreach (DataRow dataRow in dataTable.AsEnumerable().ToList())
            {
                var aTSource = new TSource();
                foreach (var aField in commonFields)
                {
                    PropertyInfo propertyInfos = aTSource.GetType().GetProperty(aField.Name);
                    var value = (dataRow[aField.Name] == DBNull.Value) ?
                    null : dataRow[aField.Name]; //if database field is nullable
                    propertyInfos.SetValue(aTSource, value, null);
                }
                dataList.Add(aTSource);
            }
            return dataList;
        }

    }

    public static class MySession
    {

        public static System.Windows.Forms.Form DefultMainParent { get; set; } 
        // **** add your session properties here, e.g like this:
        public static string Property1 { get; set; }
        public static string VAtCompnyGlobal { get; set; }

        public static string Cost { get; set; }
        public static string sumvalue { get; set; }

        public static string footer { get; set; }
        public static DateTime MyDate { get; set; }
        public static int LoginId { get; set; }
        public static int UserID { get; set; }
        public static int xWeightItem { get; set; }
        public static long SupplierID { get; set; }
        public static int PrintModel { get; set; }
        public static int PrintLAnguage { get; set; }
        public static int PrintBuildPill { get; set; }
        public static int UseNetINInvoiceSales { get; set; }

        public static int GlobalBranchID { get; set; }
        public static int GlobalFacilityID { get; set; }
        public static string GlobalUserName { get; set; }
        public static string GlobalBranchName { get; set; }
        public static string GlobalFacilityName { get; set; }

        public static iLanguage GlobalLanguageName { get; set; }
        public static double GlobalAccountID { get; set; }

        /*******************************************/
        public static int GlobalNumDecimalPlaces { get; set; }
        public static int GlobalNoOfLevels { get; set; }
        public static int GlobalAccountsLevelDigits { get; set; }

        public static string GlobalComputerInfo { get; set; }

        public static string GlobalDefaultStoreID { get; set; }
        public static string GlobalDefaultCostCenterID { get; set; }
        public static string GlobalDefaultCurencyID { get; set; }
        public static string GlobalDefaultNetTypeID { get; set; }
        public static string GlobalDefaultSupplierID { get; set; }
        public static string GlobalDefaultCustomerID { get; set; }
        public static string GlobalDefaultDebitAccountID { get; set; }
        public static string GlobalDefaultSalesDelegateID { get; set; }
        public static string GlobalDefaultSellerID { get; set; }
      



        public static decimal GlobalDiscountPercentOnItem { get; set; }
        public static decimal GlobalDiscountPercentOnTotal { get; set; }

        public static bool GlobalCanChangeDocumentsDate { get; set; }
        public static bool GlobalCanChangeInvoicePrice { get; set; }
        public static bool GlobalShowItemQtyInSaleInvoice { get; set; }
        public static bool GlobalCanDiscountOnCashierScreen { get; set; }
        public static bool GlobalCanCloseCashier { get; set; }
        public static bool GlobalCanChangePriceInCashierScreen { get; set; }
        public static bool GlobalCanSearchItemInCashierScreen { get; set; }

        public static bool GlobalCanSaleItemForZeroPrice { get; set; }
        public static bool GlobalCanOpenCashierDrawerByF12 { get; set; }
        public static bool GlobalPrintAndExportReportsByNotLoginLanguage { get; set; }
        public static bool GlobalCanEditOrDeleteOnPastPaid { get; set; }

        public static int GlobalItemBarcodeWeightDigits { get; set; }
        public static int GlobalPriceBarcodeWeightDigits { get; set; }
        public static int GlobalPriceDigits { get; set; }
        public static int GlobalQtyDigits { get; set; }
        public static int GlobalInventoryType { get; set; }

        public static int GlobalCostCalculationType { get; set; }
        public static bool GlobalUsingExpiryDate { get; set; }
        public static bool GlobalUsingItemsSerials { get; set; }
        public static bool GlobalAutoCalcFixAssetsDepreciation { get; set; }
        public static string GlobalCalcStockBy { get; set; }
        public static string GlobalWayOfOutItems { get; set; }
        public static double GlobalItemProfit { get; set; }

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
        public static string GlobalCompanyName { get; set; }
        public static string PubStrCon { get; set; }
        public static long GlobalPercentVat { get; set; }
        public static string GlobalHaveVat { get; set; }

        /******Using Date Items's *******/
        public static bool GlobalAllowUsingDateItems { get; set; }
        /******Formes Printing *******/
        public static string GlobalDefaultFormPrintingID { get; set; }
        public static string GlobalDefaultSaleFormPrintingID { get; set; }
        public static string GlobalDefaultPurchaseFormPrintingID { get; set; }
        public static string GlobalDefaultSaleReturnFormPrintingID { get; set; }
        public static string GlobalDefaultPurchaseReturnFormPrintingID { get; set; }
        public static string GlobalDefaultItemsInOnBailFormPrintingID { get; set; }
        public static string GlobalDefaultItemsOutonBailFormPrintingID { get; set; }
        public static string GlobalDefaultGoodsOpeningFormPrintingID { get; set; }

        /*********************Role For Form ****************************/
        /******AllowChangePurchaseDelegateID&SaleDelegateID*******/
        public static bool GlobalAllowChangefrmPurchaseInvoiceNetPrice { get; set; }
        public static bool GlobalAllowChangefrmPurchaseReturnInvoiceNetPrice { get; set; }
        public static bool GlobalAllowChangefrmSaleInvoiceNetPrice { get; set; }
        public static bool GlobalAllowChangefrmSaleReturnInvoiceNetPrice { get; set; }
        /******AllowChangePurchaseDelegateID&SaleDelegateID*******/
        public static bool GlobalAllowChangefrmPurchaseDelegateID { get; set; }
        public static bool GlobalAllowChangefrmPurchaseReturnDelegateID { get; set; }
        public static bool GlobalAllowChangefrmSaleDelegateID { get; set; }
        public static bool GlobalAllowChangefrmSaleReturnDelegateID { get; set; }


        /******AllowChangeSupplierID&CustomerID*******/
        public static bool GlobalAllowChangefrmPurchaseSupplierID { get; set; }
        public static bool GlobalAllowChangefrmSaleCustomerID { get; set; }
        public static bool GlobalAllowChangefrmPurchaseReturnSupplierID { get; set; }
        public static bool GlobalAllowChangefrmSaleReturnCustomerID { get; set; }
        /******ChangefrmPurchaseInvoicePrice*******/
        public static bool AllowChangefrmPurchaseInvoicePrice { get; set; }
        public static bool AllowChangefrmPurchaseReturnInvoicePrice { get; set; }
        public static bool AllowChangefrmSaleInvoicePrice { get; set; }
        public static bool AllowChangefrmSaleReturnInvoicePrice { get; set; }

        /******AllowChange Order Purchase*******/
        public static bool GlobalAllowChangefrmOrderPurchaseSupplierID { get; set; }
        public static bool GlobalAllowChangefrmOrderPurchaseStoreID { get; set; }
        public static bool GlobalAllowChangefrmOrderPurchaseDate { get; set; }
        public static bool GlobalAllowChangefrmOrderPurchaseDelegateID { get; set; }

        public static bool GlobalAllowChangefrmOrderPurchaseCurrncyID { get; set; }
        public static bool GlobalAllowChangefrmOrderPurchaseCostCenterID { get; set; }

        /******AllowChangeInvoiceDate*******/
        public static bool GlobalAllowChangefrmPurchaseInvoiceDate { get; set; }
        public static bool GlobalAllowChangefrmSaleInvoiceDate { get; set; }
        public static bool GlobalAllowChangefrmPurchaseReturnInvoiceDate { get; set; }
        public static bool GlobalAllowChangefrmSaleReturnInvoiceDate { get; set; }
        /******AllowChangeStoreID*******/
        public static bool GlobalAllowChangefrmPurchaseStoreID { get; set; }
        public static bool GlobalAllowChangefrmSaleStoreID { get; set; }
        public static bool GlobalAllowChangefrmPurchaseReturnStoreID { get; set; }
        public static bool GlobalAllowChangefrmSaleReturnStoreID { get; set; }
        /******AllowChangeCostCenterID*******/
        public static bool GlobalAllowChangefrmPurchaseCostCenterID { get; set; }
        public static bool GlobalAllowChangefrmSaleCostCenterID { get; set; }
        public static bool GlobalAllowChangefrmPurchaseReturnCostCenterID { get; set; }
        public static bool GlobalAllowChangefrmSaleReturnCostCenterID { get; set; }
        /******AllowChangeAllowChangePayMethodID*******/
        public static bool GlobalAllowChangefrmPurchasePayMethodID { get; set; }
        public static bool GlobalAllowChangefrmSalePayMethodID { get; set; }
        public static bool GlobalAllowChangefrmPurchaseReturnPayMethodID { get; set; }
        public static bool GlobalAllowChangefrmSaleReturnPayMethodID { get; set; }
        /******AllowChangeAllowChangeCurencyID*******/
        public static bool GlobalAllowChangefrmPurchaseCurencyID { get; set; }
        public static bool GlobalAllowChangefrmSaleCurencyID { get; set; }
        public static bool GlobalAllowChangefrmPurchaseReturnCurencyID { get; set; }
        public static bool GlobalAllowChangefrmSaleReturnCurencyID { get; set; }
        /******AllowChangeNetTypeID*******/
        public static bool GlobalAllowChangefrmPurchaseNetTypeID { get; set; }
        public static bool GlobalAllowChangefrmSaleNetTypeID { get; set; }
        public static bool GlobalAllowChangefrmPurchaseReturnNetTypeID { get; set; }
        public static bool GlobalAllowChangefrmSaleReturnNetTypeID { get; set; }
        /******DefaultPayMethodID*******/
        public static string GlobalDefaultPurchaseDebitAccountID { get; set; }
        public static string GlobalDefaultPurchaseDiscountAccountID { get; set; }
      
        public static string GlobalDefaultSaleCreditAccountID { get; set; }
        public static string GlobalDefaultSalesRevenueAccountID { get; set; }

        public static string GlobalDefaultSaleDebitAccountID { get; set; }
       
        public static string GlobalDefaultSaleReturnCreditAccountID { get; set; }

        /********Defult Purchase**********/
        public static string GlobalDefaultPurchaseAddtionalAccountID { get; set; }

        public static string GlobalDefaultSalesAddtionalAccountID { get; set; }
        public static string GlobalDefaultCostSalseAccountID { get; set; }
        public static string GlobalDefaultDiscountSalseAccountID { get; set; }
        public static string GlobalDefaultPurchaseCrditAccountID { get; set; }

        
        /********Defult Order Purchase**********/
        public static string GlobalDefaultOrderPurchaseDelegateID { get; set; }

        public static string GlobalDefaultOrderPurchaseStoreID { get; set; }
        public static string GlobalDefaultOrderPurchaseSupplierID { get; set; }
        public static string GlobalDefaultOrderPurchaseCostCenterID { get; set; }
        public static string GlobalDefaultOrderPurchaseCurrncyID { get; set; }
        /******DefaultPayMethodID*******/
        public static string GlobalDefaultPurchasePayMethodID { get; set; }
        public static string GlobalDefaultSalePayMethodID { get; set; }
        public static string GlobalDefaultPurchaseReturnPayMethodID { get; set; }
        public static string GlobalDefaultSaleReturnPayMethodID { get; set; }
        /******DefaultCurencyID*******/
        public static string GlobalDefaultPurchaseCurencyID { get; set; }
        public static string GlobalDefaultSaleCurencyID { get; set; }
        public static string GlobalDefaultPurchaseReturnCurencyID { get; set; }
        public static string GlobalDefaultSaleReturnCurencyID { get; set; }
        /******DefaultNetTypeID*******/
        public static string GlobalDefaultPurchaseNetTypeID { get; set; }
        public static string GlobalDefaultSaleNetTypeID { get; set; }
        public static string GlobalDefaultPurchaseReturnNetTypeID { get; set; }
        public static string GlobalDefaultSaleReturnNetTypeID { get; set; }
        /******DefaultSupplierID*******/
        public static string GlobalDefaultPurchaseSupplierID { get; set; }
        public static string GlobalDefaultSaleCustomerID { get; set; }
        public static string GlobalDefaultPurchaseReturnSupplierID { get; set; }
        public static string GlobalDefaultSaleReturnCustomerID { get; set; }
        /******DefaultCostCenterID*******/
        public static string GlobalDefaultPurchaseCostCenterID { get; set; }
        public static string GlobalDefaultSaleCostCenterID { get; set; }
        public static string GlobalDefaultPurchaseReturnCostCenterID { get; set; }
        public static string GlobalDefaultSaleReturnCostCenterID { get; set; }
        /******DefaultStoreID*******/
        public static string GlobalDefaultPurchaseStoreID { get; set; }
        public static string GlobalDefaultSaleStoreID { get; set; }
        public static string GlobalDefaultPurchaseReturnStoreID { get; set; }
        public static string GlobalDefaultSaleReturnStoreID { get; set; }
        /******ChangeDelegateID*******/
        public static string GlobalDefaultPurchaseDelegateID { get; set; }
        public static string GlobalDefaultPurchaseReturnDelegateID { get; set; }
        public static string GlobalDefaultSaleDelegateID { get; set; }
        public static string GlobalDefaultSaleReturnDelegateID { get; set; }
        public static string GlobalDefaultPurchaseReturnDebitAccountID { get; set; }
        public static string GlobalDefaultPurchaseReturnCrditAccountID { get; set; }

        /************* AccountID ****************/
        //________Purchase Invoice Allow
        public static bool GlobalAllowChangefrmPurchaseDebitAccountID { get; set; }
        public static bool GlobalAllowChangefrmPurchaseCreditAccountID { get; set; }
        public static bool GlobalAllowChangefrmPurchaseAdditionalAccountID { get; set; }
        public static bool GlobalAllowChangefrmPurchaseChequeAccountID { get; set; }
        public static bool GlobalAllowChangefrmPurchaseDiscountCreditAccountID { get; set; }
        public static bool GlobalAllowChangefrmPurchaseNetAccountID { get; set; }
        public static bool GlobalAllowChangefrmPurchaseTransportDebitAccountID { get; set; }
        //________Purchase Invoice Return
        public static bool GlobalAllowChangefrmPurchaseReturnDebitAccountID { get; set; }
        public static bool GlobalAllowChangefrmPurchaseReturnCreditAccountID { get; set; }
        public static bool GlobalAllowChangefrmPurchaseReturnAdditionalAccountID { get; set; }
        public static bool GlobalAllowChangefrmPurchaseReturnChequeAccountID { get; set; }
        public static bool GlobalAllowChangefrmPurchaseReturnDiscountCreditAccountID { get; set; }
        public static bool GlobalAllowChangefrmPurchaseReturnNetAccountID { get; set; }
        public static bool GlobalAllowChangefrmPurchaseReturnTransportDebitAccountID { get; set; }
        //________Sale Invoice 
        public static string GlobalDefaultSaleSellerID { get; set; }
        public static bool GlobalAllowChangefrmSaleSellerID { get; set; }

        public static bool GlobalAllowChangefrmSaleDebitAccountID { get; set; }
        public static bool GlobalAllowChangefrmSaleCreditAccountID { get; set; }
        public static bool GlobalAllowChangefrmSaleAdditionalAccountID { get; set; }
        public static bool GlobalAllowChangefrmSaleChequeAccountID { get; set; }
        public static bool GlobalAllowChangefrmSaleDiscountDebitAccountID { get; set; }
        public static bool GlobalAllowChangefrmSaleNetAccountID { get; set; }

        // order sales defult 
        public static string GlobalDefaultOrderSaleDelegateID { get; set; }
        public static string GlobalDefaultOrderSaleCostCenterID { get; set; }
        public static string GlobalDefaultOrderSaleStoreID { get; set; }
        public static string GlobalDefaultOrderSaleCustomerID { get; set; }
        public static string GlobalDefaultOrderSaleCurrncyID { get; set; }

        // order sales  allow 
        public static bool GlobalAllowChangefrmOrderSaleSellerID { get; set; }
        public static bool GlobalAllowChangefrmOrderSaleStoreID { get; set; }
        public static bool GlobalAllowChangefrmOrderSaleDate { get; set; }
        public static bool GlobalAllowChangefrmOrderSaleCostCenterID { get; set; }
        public static bool GlobalAllowChangefrmOrderSaleCustomerID { get; set; }
        public static bool GlobalAllowChangefrmOrderSaleDelegeteID { get; set; }
        public static bool GlobalAllowChangefrmOrderSaleCurencyID { get; set; }
        //________Sale Invoice Return
        public static string GlobalDefaultSaleReturnSellerID { get; set; }
        public static bool GlobalAllowChangefrmSaleReturnSellerID { get; set; }

        public static bool GlobalAllowChangefrmSaleReturnDebitAccountID { get; set; }
        public static bool GlobalAllowChangefrmSaleReturnCreditAccountID { get; set; }
        public static bool GlobalAllowChangefrmSaleReturnAdditionalAccountID { get; set; }
        public static bool GlobalAllowChangefrmSaleReturnChequeAccountID { get; set; }
        public static bool GlobalAllowChangefrmSaleReturnDiscountDebitAccountID { get; set; }
        public static bool GlobalAllowChangefrmSaleReturnNetAccountID { get; set; }
        /****************************************************/
        /******Spend Voucher*******/
        //________Spend Voucher Allow 

        public static bool GlobalAllowChangefrmSpendVoucherCreditAccountID { get; set; }
        public static bool GlobalAllowChangefrmSpendVoucherDiscountAccountID { get; set; }
        public static bool GlobalAllowChangefrmSpendVoucherDate { get; set; }
        public static bool GlobalAllowChangefrmSpendVoucherCurencyID { get; set; }
        public static bool GlobalAllowChangefrmSpendVoucherCostCenterID { get; set; }
        public static bool GlobalAllowDiscountPercentfrmSpendVoucher { get; set; }
        public static bool GlobalAllowChangefrmSpendVoucherPurchasesDelegateID { get; set; }
        //________Spend Voucher Default
        public static string GlobalDefaultSpendVoucherCurencyID { get; set; }
        public static string GlobalDefaultSpendVoucherCostCenterID { get; set; }
        public static string GlobalDiscountPercentSpendVoucher { get; set; }
        public static string GlobalDefaultSpendVoucherPurchasesDelegateID { get; set; }
        public static string GlobalDefaultSpendVoucherCrditAccountID { get; set; }

        /****************************************************/
        /******CheckSpend Voucher*******/
        //________CheckSpend Voucher Allow 

        public static bool GlobalAllowChangefrmCheckSpendVoucherCreditAccountID { get; set; }
        public static bool GlobalAllowChangefrmCheckSpendVoucherDiscountAccountID { get; set; }
        public static bool GlobalAllowChangefrmCheckSpendVoucherDate { get; set; }
        public static bool GlobalAllowChangefrmCheckSpendVoucherCurencyID { get; set; }
        public static bool GlobalAllowChangefrmCheckSpendVoucherCostCenterID { get; set; }
        public static bool GlobalAllowDiscountPercentfrmCheckSpendVoucher { get; set; }
        public static bool GlobalAllowChangefrmCheckSpendVoucherPurchasesDelegateID { get; set; }
        public static bool GlobalAllowChangefrmCheckSpendVoucherBankID { get; set; }
        //________CheckSpend Voucher Default
        public static string GlobalDefaultCheckSpendVoucherCurencyID { get; set; }
        public static string GlobalDefaultCheckSpendVoucherCostCenterID { get; set; }
        public static string GlobalDefaultCheckSpendVoucherCrditAccountID { get; set; }
        public static string GlobalDiscountPercentCheckSpendVoucher { get; set; }
        public static string GlobalDefaultCheckSpendVoucherPurchasesDelegateID { get; set; }
        public static string GlobalDefaultCheckSpendVoucherBankID { get; set; }
        /****************************************************/
        /******Receipt Voucher*******/
        //________Receipt Voucher Allow 
        public static bool GlobalAllowChangefrmReceiptVoucherDebitAccountID { get; set; }
        public static bool GlobalAllowChangefrmReceiptVoucherDiscountAccountID { get; set; }
        public static bool GlobalAllowChangefrmReceiptVoucherDate { get; set; }
        public static bool GlobalAllowChangefrmReceiptVoucherCurencyID { get; set; }
        public static bool GlobalAllowChangefrmReceiptVoucherCostCenterID { get; set; }
        public static bool GlobalAllowChangefrmReceiptVoucherSalesDelegateID { get; set; }
        public static bool GlobalAllowDiscountPercentfrmReceiptVoucher { get; set; }
        //________Receipt Voucher Default
        public static string GlobalDefaultReceiptVoucherCurencyID { get; set; }
        public static string GlobalDefaultReceiptVoucherCostCenterID { get; set; }
        public static string GlobalDefaultReceiptVoucherSalesDelegateID { get; set; }
        public static string GlobalDiscountPercentReceiptVoucher { get; set; }
        public static string GlobalDefaultReceiptVoucherDebitAccountID { get; set; }
        public static string GlobalDefaultReceiptVoucherIntermediateDiamondAccountID { get; set; }
        public static string GlobalDefaultReceiptVoucherIntermediateGoldAccountID { get; set; }

        /****************************************************/
        /******CheckReceipt Voucher*******/
        //________CheckReceipt Voucher Allow 
        public static bool GlobalAllowChangefrmCheckReceiptVoucherDebitAccountID { get; set; }
        public static bool GlobalAllowChangefrmCheckReceiptVoucherDiscountAccountID { get; set; }
        public static bool GlobalAllowChangefrmCheckReceiptVoucherDate { get; set; }
        public static bool GlobalAllowChangefrmCheckReceiptVoucherCurencyID { get; set; }
        public static bool GlobalAllowChangefrmCheckReceiptVoucherCostCenterID { get; set; }
        public static bool GlobalAllowChangefrmCheckReceiptVoucherSalesDelegateID { get; set; }
        public static bool GlobalAllowDiscountPercentfrmCheckReceiptVoucher { get; set; }
        public static bool GlobalAllowChangefrmCheckReceiptVoucherBankID { get; set; }
        //________CheckReceipt Voucher Default
        public static string GlobalDefaultCheckReceiptVoucherCurencyID { get; set; }
        public static string GlobalDefaultCheckReceiptVoucherCostCenterID { get; set; }
        public static string GlobalDefaultCheckReceiptVoucherDebitAccountID { get; set; }
        public static string GlobalDefaultCheckReceiptVoucherSalesDelegateID { get; set; }
        public static string GlobalDiscountPercentCheckReceiptVoucher { get; set; }
        public static string GlobalDefaultCheckReceiptVoucherBankID { get; set; }
        /****************************************************/
        /******Global Allow WhenClick & Enter Open Popup *******/
        public static bool GlobalAllowWhenClickOpenPopup { get; set; }
        public static bool GlobalAllowWhenEnterOpenPopup { get; set; }
        /************************************************************/
        /******Opening Voucher*******/
        //________Opening Voucher AccountID
        public static bool GlobalAllowChangefrmOpeningVoucherDebitAccountID { get; set; }
        public static bool GlobalAllowChangefrmOpeningVoucherCreditAccountID { get; set; }
        public static bool GlobalAllowChangefrmOpeningVoucherDate { get; set; }
        public static bool GlobalAllowChangefrmOpeningVoucherCostCenterID { get; set; }
        public static bool GlobalAllowChangefrmOpeningVoucherCurencyID { get; set; }
        //________Opening Default 
        public static string GlobalDefaultOpeningVoucherCostCenterID { get; set; }
        public static string GlobalDefaultOpeningVoucherCurencyID { get; set; }

        /******Various Voucher*******/
        //________Various Voucher Allow 
        public static bool GlobalAllowChangefrmVariousVoucherDate { get; set; }
        public static bool GlobalAllowChangefrmVariousVoucherCurencyID { get; set; }
        public static bool GlobalAllowChangefrmVariousVoucherCostCenterID { get; set; }
        public static bool GlobalAllowChangefrmVariousVoucherSalesDelegateID { get; set; }

        //________Various Voucher Default
        public static string GlobalDefaultVariousVoucherCurencyID { get; set; }
        public static string GlobalDefaultVariousVoucherCostCenterID { get; set; }
        public static string GlobalDefaultVariousVoucherSalesDelegateID { get; set; }

        /******Items In On Bail*******/
        //________Gold In On Bail  Allow 
        public static bool GlobalAllowChangefrmItemsInOnBailInvoiceDate { get; set; }
        public static bool GlobalAllowChangefrmItemsInOnBailStoreID { get; set; }
        public static bool GlobalAllowChangefrmItemsInOnBailCostCenterID { get; set; }
        public static bool GlobalAllowChangefrmItemsInOnBailCurencyID { get; set; }
        public static bool GlobalAllowChangefrmItemsInOnBailSupplierID { get; set; }
        public static bool GlobalAllowChangefrmItemsInOnBailCreditAccountID { get; set; }
        public static bool GlobalAllowChangefrmItemsInOnBailDebitAccountID { get; set; }
        //________Gold In On Bail Default

        public static string GlobalDefaultItemsInOnBailCurencyID { get; set; }
        public static string GlobalDefaultItemsInOnBailCostCenterID { get; set; }
        public static string GlobalDefaultItemsInOnBailSupplierID { get; set; }
        public static string GlobalDefaultItemsInOnBailStoreID { get; set; }


        //________Items In On Bail  Allow 
        public static bool GlobalAllowChangefrmMatirialInOnBailInvoiceDate { get; set; }
        public static bool GlobalAllowChangefrmMatirialInOnBailStoreID { get; set; }
        public static bool GlobalAllowChangefrmMatirialInOnBailCostCenterID { get; set; }
        public static bool GlobalAllowChangefrmMatirialInOnBailCurrncyID { get; set; }
        public static bool GlobalAllowChangefrmMatirialInOnBailSupplier { get; set; }
        public static bool GlobalAllowChangefrmMatirialInOnBailDebitAccountID { get; set; }
        public static bool GlobalAllowChangefrmMatirialInOnBailCrditAccountID { get; set; }

        //________Matirial On Bail  Allow 
        public static bool GlobalAllowChangefrmMatirialOutOnBailDebitAccountID { get; set; }
        public static bool GlobalAllowChangefrmMatirialOutOnBailCrditAccountID { get; set; }
        public static bool GlobalAllowChangefrmMatirialOutOnBailSupplierID { get; set; }
        public static bool GlobalAllowChangefrmMatirialOutOnBailCurrncyID { get; set; }
        public static bool GlobalAllowChangefrmMatirialOutOnBailCostCenterID { get; set; }
        public static bool GlobalAllowChangefrmMatirialOutOnBailStoreID { get; set; }
        public static bool GlobalAllowChangefrmMatirialOutOnBailInvoiceDate { get; set; }

        //________Gold Multi Allow 
        public static bool GlobalAllowChangefrmMaltiTransferInvoiceDate { get; set; }
        public static bool GlobalAllowChangefrmMaltiTransferStoreID { get; set; }
        public static bool GlobalAllowChangefrmMaltiTransferCostCenterID { get; set; }
        public static bool GlobalAllowChangefrmMaltiTransferCurrncyID { get; set; }
     
        //________Matirial In On Bail Default

        public static string GlobalDefaultMatirialInOnBailCurencyID { get; set; }
        public static string GlobalDefaultMatirialInOnBailCostCenterID { get; set; }
        public static string GlobalDefaultMatirialInOnBailStoreAccountID { get; set; }
        public static string GlobalDefaultMatirialInOnBailSupplierID { get; set; }


        //________Matirial out On Bail Default

        public static string GlobalDefaultMatirialOutOnBailSupplierID { get; set; }
        public static string GlobalDefaultMatirialOutOnBailStoreID { get; set; }
        public static string GlobalDefaultMatirialOutOnBailCostCenterID { get; set; }
        public static string GlobalDefaultMatirialOutOnBailCurencyID { get; set; }



        //________Gold Multi  Default

        public static string GlobalDefaultGoldMultiTransferStoreID { get; set; }
        public static string GlobalDefaultGoldMultiTransferCostCenterID { get; set; }
        public static string GlobalDefaultGoldMultiTransferCurencyID { get; set; }

        /******Items Out On Bail*******/
        //________Items Out On Bail  Allow 
        public static bool GlobalAllowChangefrmItemsOutOnBailInvoiceDate { get; set; }
        public static bool GlobalAllowChangefrmItemsOutOnBailStoreID { get; set; }
        public static bool GlobalAllowChangefrmItemsOutOnBailCostCenterID { get; set; }
        public static bool GlobalAllowChangefrmItemsOutOnBailCurencyID { get; set; }
        public static bool GlobalAllowChangefrmItemsOutOnBailCustomerID { get; set; }
        public static bool GlobalAllowChangefrmItemsOutOnBailCreditAccountID { get; set; }
        public static bool GlobalAllowChangefrmItemsOutOnBailDebitAccountID { get; set; }


        //________Matirial Multi  Default

        public static string GlobalDefaultMatirialMultiTransferStoreID { get; set; }
        public static string GlobalDefaultMatirialMultiTransferCostCenterID { get; set; }
        public static string GlobalDefaultMatirialMultiTransferCurencyID { get; set; }
        //________Matirial Multi Allow 
        public static bool GlobalAllowChangefrmMatirialMaltiTransferInvoiceDate { get; set; }
        public static bool GlobalAllowChangefrmMatirialMaltiTransferStoreID { get; set; }
        public static bool GlobalAllowChangefrmMatirialMaltiTransferCostCenterID { get; set; }
        public static bool GlobalAllowChangefrmMatirialMaltiTransferCurrncyID { get; set; }

        //________Items Out On Bail Default

        public static string GlobalDefaultItemsOutOnBailCurencyID { get; set; }
        public static string GlobalDefaultItemsOutOnBailCostCenterID { get; set; }
        public static string GlobalDefaultItemsOutOnBailCustomerID { get; set; }
        public static string GlobalDefaultItemsOutOnBailStoreID { get; set; }
        /******Items Out On Bail*******/
        //________Items Out On Bail  Allow 
        public static bool GlobalAllowChangefrmGoodsOpeningInvoiceDate { get; set; }
        public static bool GlobalAllowChangefrmGoodsOpeningStoreID { get; set; }
        public static bool GlobalAllowChangefrmGoodsOpeningCostCenterID { get; set; }
        public static bool GlobalAllowChangefrmGoodsOpeningCurencyID { get; set; }
        public static bool GlobalAllowChangefrmGoodsOpeningCreditAccountID { get; set; }
        public static bool GlobalAllowChangefrmGoodsOpeningDebitAccountID { get; set; }
        //________Items Out On Bail Default
        public static string GlobalDefaultGoodsOpeningCurencyID { get; set; }
        public static string GlobalDefaulGoodsOpeningStoreID { get; set; }
        public static string GlobalDefaultGoodsOpeningCostCenterID { get; set; }
        public static string GlobalDefaultGoodsOpeningStoreID { get; set; }
        public static string GlobalDefaulGoodsOpeningDebitAccountID { get; set; }
        public static string GlobalDefaulGoodsOpeningCrditAccountID { get; set; }

        //________Items Dismantling

        public static bool GlobalAllowChangefrmItemsDismantlingDate { get; set; }
        public static bool GlobalAllowChangefrmItemsDismantlingStoreID { get; set; }
        public static bool GlobalAllowChangefrmItemsDismantlingCostCenterID { get; set; }

        //________ Allow OutItems With OutBalance
        public static bool GlobalAllowOutItemsWithOutBalance { get; set; }
        //________ Allow Using Barcode
        public static bool GlobalAllowUsingBarcodeInInvoices { get; set; }


        public static string GlobalDefaultParentBanksAccountID { get; set; }
        public static string GlobalDefaultParentBoxesAccountID { get; set; }
        public static string GlobalDefaultParentStoreAccountID { get; set; }
        public static string GlobalDefaultParentCustomerAccountID { get; set; }
        public static string GlobalDefaultParentSupplierAccountID { get; set; }
        public static string GlobalDefaultParentEmployeeAccountID { get; set; }

        /****************ManuFactory***************************/

        //Allow
        public static bool GlobalDefaultCanRepetUseOrderOneOureMoreBeforeCasting { get; set; }
        public static bool GlobalDefaultCanRepetUseOrderOneOureMoreManufactory { get; set; }

        /****************Preparation***************************/
        //defult Wax
        public static string GlobalDefaultWaxCurencyID { get; set; }
        public static string GlobalDefaultWaxCostCenterID { get; set; }
        public static string GlobalDefaultWaxBeforeStoreAccontID { get; set; }
        public static string GlobalDefaultWaxEmployeeID { get; set; }
        public static string GlobalDefaultWaxAfterStoreAccontID { get; set; }

        //allow Wax
        public static bool GlobalAllowChangefrmWaxCommandDate { get; set; }
        public static bool GlobalAllowChangefrmWaxBeforeStoreID { get; set; }
        public static bool GlobalAllowChangefrmWaxAfterStoreID { get; set; }
        public static bool GlobalAllowChangefrmWaxEmployeeID { get; set; }
        public static bool GlobalAllowChangefrmWaxCostCenterID { get; set; }
        public static bool GlobalAllowChangefrmWaxCurrncyID { get; set; }
        public static bool GlobalAllowChangefrmWaxBranchID { get; set; }

        //defult Cad
        public static string GlobalDefaultCadCurencyID { get; set; }
        public static string GlobalDefaultCadCostCenterID { get; set; }
        public static string GlobalDefaultCadBeforeStoreAccontID { get; set; }
        public static string GlobalDefaultCadEmpolyeeID { get; set; }
        public static string GlobalDefaultCadAfterStoreAccontID { get; set; }

        //allow Cad
        public static bool GlobalAllowChangefrmCadCommandDate { get; set; }
        public static bool GlobalAllowChangefrmCadBeforeStoreID { get; set; }
        public static bool GlobalAllowChangefrmCadAfterStoreID { get; set; }
        public static bool GlobalAllowChangefrmCadEmployeeID { get; set; }
        public static bool GlobalAllowChangefrmCadCostCenterID { get; set; }
        public static bool GlobalAllowChangefrmCadCurrncyID { get; set; }
        public static bool GlobalAllowChangefrmCadBranchID { get; set; }

        //defult Zircon
        public static string GlobalDefaultZirconCurencyID { get; set; }
        public static string GlobalDefaultZirconCostCenterID { get; set; }
        public static string GlobalDefaultZirconBeforeStoreAccontID { get; set; }
        public static string GlobalDefaultZirconAfterStoreAccontID { get; set; }
        public static string GlobalDefaultZirconEmpolyeeID { get; set; }

        //allow Zircon
        public static bool GlobalAllowChangefrmZirconCommandDate { get; set; }
        public static bool GlobalAllowChangefrmZirconBeforeStoreID { get; set; }
        public static bool GlobalAllowChangefrmZirconAfterStoreID { get; set; }
        public static bool GlobalAllowChangefrmZirconEmployeeID { get; set; }
        public static bool GlobalAllowChangefrmZirconCostCenterID { get; set; }
        public static bool GlobalAllowChangefrmZirconCurrncyID { get; set; }
        public static bool GlobalAllowChangefrmZirconBranchID { get; set; }


        //defult Diamond
        public static string GlobalDefaultDiamondCurencyID { get; set; }
        public static string GlobalDefaultDiamondCostCenterID { get; set; }
        public static string GlobalDefaultDiamondBeforeStoreAccontID { get; set; }
        public static string GlobalDefaultDiamondAfterStoreAccontID { get; set; }
        public static string GlobalDefaultDiamondEmpolyeeID { get; set; }

        //allow Diamond
        public static bool GlobalAllowChangefrmDiamondCommandDate { get; set; }
        public static bool GlobalAllowChangefrmDiamondBeforeStoreID { get; set; }
        public static bool GlobalAllowChangefrmDiamondAfterStoreID { get; set; }
        public static bool GlobalAllowChangefrmDiamondEmployeeID { get; set; }
        public static bool GlobalAllowChangefrmDiamondCostCenterID { get; set; }
        public static bool GlobalAllowChangefrmDiamondCurrncyID { get; set; }
        public static bool GlobalAllowChangefrmDiamondBranchID { get; set; }


        //defult Afforstation
        public static string GlobalDefaultAfforstationCurencyID { get; set; }
        public static string GlobalDefaultAfforstationCostCenterID { get; set; }
        public static string GlobalDefaultAfforstationBeforeStoreAccontID { get; set; }
        public static string GlobalDefaultAfforstationAccountID { get; set; }
        public static string GlobalDefaultAfforstationBeforeEmpolyeeID { get; set; }
        public static string GlobalDefaultAfforstationAfterEmpolyeeID { get; set; }

        //allow Afforstation
        public static bool GlobalAllowChangefrmAfforstationCommandDate { get; set; }
        public static bool GlobalAllowChangefrmAfforstationBeforeStoreID { get; set; }
        public static bool GlobalAllowChangefrmAfforstationAfterStoreID { get; set; }
        public static bool GlobalAllowChangefrmAfforstationBeforeEmployeeID { get; set; }
        public static bool GlobalAllowChangefrmAfforstationAfterEmployeeID { get; set; }
        public static bool GlobalAllowChangefrmAfforstationCostCenterID { get; set; }
        public static bool GlobalAllowChangefrmAfforstationCurrncyID { get; set; }
        public static bool GlobalAllowChangefrmAfforstationBranchID { get; set; }

        //defult Casting
      
        public static string GlobalDefaultCastingCurrncyID { get; set; }
        public static string GlobalDefaultCastingCostCenterID { get; set; }
        public static string GlobalDefaultCastingStoreID { get; set; }
        public static string GlobalDefaultCastingAccountID { get; set; }
        public static string GlobalDefaultCastingEmployeeID { get; set; }

        //allow Casting
   
        public static bool GlobalAllowChangefrmCastingCommandDate { get; set; }
        public static bool GlobalAllowChangefrmCastingStoreID { get; set; } 
        public static bool GlobalAllowChangefrmCastingAccountID { get; set; }
        public static bool GlobalAllowChangefrmCastingEmployeeID { get; set; }
        public static bool GlobalAllowChangefrmCastingCostCenterID { get; set; }
        public static bool GlobalAllowChangefrmCastingCurrncyID { get; set; }
        
        //defult Manufactory
        public static string GlobalDefaultManufactoryCurrncyID { get; set; }
     
        public static string GlobalDefaultManufactoryStoreID { get; set; }
        public static string GlobalDefaultManufactoryAccountID { get; set; }
        public static string GlobalDefaultManufatoryEmployeeID { get; set; }

        //allow Manufactory
        public static bool GlobalAllowChangefrmManufactoryCommandDate { get; set; }
        public static bool GlobalAllowChangefrmManufactoryStoreID { get; set; }
        public static bool GlobalAllowChangefrmManufatoryAccountID { get; set; }
        public static bool GlobalAllowChangefrmManufactoryEmployeeID { get; set; }
     
        public static bool GlobalAllowChangefrmManufactoryCurrncyID { get; set; }

        //defult Commpounnd
        public static string GlobalDefaultCommpundCurrncyID { get; set; }
    
        public static string GlobalDefaultCompoundStoreID { get; set; }
        public static string GlobalDefaultCompoundAccountID { get; set; }
        public static string GlobalDefaultCompoundEmployeeID { get; set; }

        //allow Commpounnd
        public static bool GlobalAllowChangefrmCompundCommandDate { get; set; }
        public static bool GlobalAllowChangefrmCompoundStoreID { get; set; }
        public static bool GlobalAllowChangefrmCompoundAccountID { get; set; }
        public static bool GlobalAllowChangefrmCompoundEmployeeID { get; set; }
   
        public static bool GlobalAllowChangefrmCompundCurrncyID { get; set; }

        //defult Prntage
        public static string GlobalDefaultPrntageCurrncyID { get; set; }
      
        public static string GlobalDefaultPrntageStoreID { get; set; }
        public static string GlobalDefaultPrntageAccountID { get; set; }
        public static string GlobalDefaultPrntageEmployeeID { get; set; }
        public static string GlobalDefaultPrntage2StoreID { get; set; }
        public static string GlobalDefaultPrntage2AccountID { get; set; }

        //allow Prntage
        public static bool GlobalAllowChangefrmPrntageCommandDate { get; set; }
        public static bool GlobalAllowChangefrmPrntageStoreID { get; set; }
        public static bool GlobalAllowChangefrmPrntageAccountID { get; set; }

        public static bool GlobalAllowChangefrmPrntage2StoreID { get; set; }
        public static bool GlobalAllowChangefrmPrntage2AccountID { get; set; }
        public static bool GlobalAllowChangefrmPrntageEmployeeID { get; set; }     
        public static bool GlobalAllowChangefrmPrntageCurrncyID { get; set; }


        //defult Polishin
        public static string GlobalDefaultPolishinCurrncyID { get; set; }
     
        public static string GlobalDefaultPolishinStoreID { get; set; }
        public static string GlobalDefaultPolishinAccountID { get; set; }
        public static string GlobalDefaultPolishin2StoreID { get; set; }

        public static string GlobalDefaultPolishin2AccountID { get; set; }

        public static string GlobalDefaultPolishin3StoreID { get; set; }

        public static string GlobalDefaultPolishin3AccountID { get; set; }
        public static string GlobalDefaultPolishnEmployeeID { get; set; }

        //allow Polishin
        public static bool GlobalAllowChangefrmPolishnCommandDate { get; set; }
        public static bool GlobalAllowChangefrmPolishnStoreID { get; set; }
        public static bool GlobalAllowChangefrmPolishnAccountID { get; set; }
        public static bool GlobalAllowChangefrmPolishnEmployeeID { get; set; }
     
        public static bool GlobalAllowChangefrmPolishnCurrncyID { get; set; }
        public static bool GlobalAllowChangefrmPolishn2StoreID { get; set; }
        public static bool GlobalAllowChangefrmPolishn2AccountID { get; set; }

        public static bool GlobalAllowChangefrmPolishn3StoreID { get; set; }
        public static bool GlobalAllowChangefrmPolishn3AccountID { get; set; }

        //defult Addtional
        public static string GlobalDefaultAddtionalCurrncyID { get; set; }
      
        public static string GlobalDefaultAddtionalStoreID { get; set; }
        public static string GlobalDefaultAddtionalAccountID { get; set; }
        public static string GlobalDefaultAddtionalEmployeeID { get; set; }

        //allow Addtional
        public static bool GlobalAllowChangefrmAddtionalCommandDate { get; set; }
        public static bool GlobalAllowChangefrmAddtionalStoreID { get; set; }
        public static bool GlobalAllowChangefrmAddtionalAccountID { get; set; }
        public static bool GlobalAllowChangefrmAddtionalEmployeeID { get; set; }
    
        public static bool GlobalAllowChangefrmAddtionalCurrncyID { get; set; }


        //defult Dismant
        public static string GlobalDefaultDismantageCurrncyID { get; set; }
     
        public static string GlobalDefaultDismantageStoreID { get; set; }
        public static string GlobalDefaultDismantageAccountID { get; set; }
        public static string GlobalDefaultDismantageEmployeeID { get; set; }

        //allow Dismant
        public static bool GlobalAllowChangefrmDismantageCommandDate { get; set; }
        public static bool GlobalAllowChangefrmDismantageStoreID { get; set; }
        public static bool GlobalAllowChangefrmDismantageAccountID { get; set; }
        public static bool GlobalAllowChangefrmDismantageEmployeeID { get; set; }
      
        public static bool GlobalAllowChangefrmDismanatgeCurrncyID { get; set; }


        public static bool AllowOutQtyNegative { get; set; }
        public static bool AllowNotShowQTYInQtyField { get; set; }



      //**********restrction Order*********
        //allow
        public static bool GlobalAllowChangefrmOrderRestrctionCommandDate { get; set; }

        public static bool GlobalAllowChangefrmOrderRestrctionTypeID { get; set; }

        public static bool GlobalAllowChangefrmOrderRestrctionTypeMatirialID { get; set; }
        //Defult
        public static string GlobalDefaultTypeOrderRestrectionID { get; set; }

        public static string GlobalDefaultTypeMatirialOrderRestrectionID { get; set; }


        //Defult
        public static string GlobalDefaultPricePerGram24k { get; set; }

        public static string GlobalDefaultPricePerGram22k { get; set; }
        public static string GlobalDefaultPricePerGram21k { get; set; }

        public static string GlobalDefaultPricePerGram16k { get; set; }
        public static string GlobalDefaultPricePerGram18k { get; set; }
        public static string GlobalDefaultPricePerGram14k { get; set; }



        public static bool GlobalAllowBranchModificationAllScreens { get; set; }

        public static object GlobalDefaultProcessPostedStatus { get; set; }
    }

}




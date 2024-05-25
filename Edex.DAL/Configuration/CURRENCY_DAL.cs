

using Edex.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Edex.DAL
{
    public class CURRENCY_DAL
    {
        public static readonly string TableName = "ACC_CURRENCY";
        public static readonly string PremaryKey = "ID";
        public static CURRENCY_BO ConvertRowToObj(DataRow dr)
        {
            CURRENCY_BO obj = new CURRENCY_BO();
            obj.ID = Comon.cInt(dr["ID"]);
            obj.ARBNAME = dr["ArbName"].ToString();
            obj.ENGNAME = dr["EngName"].ToString();
            obj.NOTES = dr["Notes"].ToString();
            obj.BranchID = Comon.cInt(dr["BranchID"]);
            obj.FacilityID = Comon.cInt(dr["FacilityID"]);

           // obj.CurrncyPart = dr["CurrncyPart"].ToString();
            obj.CodeCurrency = dr["CurrencyCode"].ToString();
            obj.TransPricing = Comon.cDec(dr["ExchangeRate"].ToString());
            obj.MaxTransPricing = Comon.cDec(dr["MaxRate"].ToString());
            obj.MinTransPricing = Comon.cDec(dr["MinRate"].ToString());
            obj.TypeCurrency = Comon.cInt(dr["TypeCurrency"].ToString());
            obj.StoreCurrency = Comon.cInt(dr["StoreCurrency"]);
            obj.TAFQEETID = Comon.cInt(dr["TAFQEETID"]); 

            obj.FoundResult = true;
            return obj;
        }

        #region  GETDATA        
        public static CURRENCY_BO GetByID(long ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Acc_Currency_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@ID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            CURRENCY_BO Returned = new CURRENCY_BO();
                            Returned = (ConvertRowToObj(dt.Rows[0]));
                            return Returned;
                        }
                        else
                            return null;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }


        

        public static DataTable GetAll(int FacilityID)
        {

            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "Acc_Curency";
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 7));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                             
                            return dt;
                        }
                        else
                            return null;
                    }

                }
            }
            catch (Exception)
            {
                return null;
            }
        }


      
        #endregion

        #region  TRANSACTION  

        public static int InsertUpdate(CURRENCY_BO objRecord, bool IsNewRecord)
        {
            Int32 objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Acc_Currency_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@ID", objRecord.ID));
                    objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ARBNAME));
                    objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.ENGNAME));
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyCode", objRecord.CodeCurrency));
                    objCmd.Parameters.Add(new SqlParameter("@StoreCurrency", objRecord.StoreCurrency));
                    objCmd.Parameters.Add(new SqlParameter("@TypeCurrency", objRecord.TypeCurrency));
                    objCmd.Parameters.Add(new SqlParameter("@TAFQEETID", objRecord.TAFQEETID));
                    objCmd.Parameters.Add(new SqlParameter("@MaxRate", objRecord.MaxTransPricing));
                    objCmd.Parameters.Add(new SqlParameter("@MinRate", objRecord.MinTransPricing));
                    objCmd.Parameters.Add(new SqlParameter("@ExchangeRate", objRecord.TransPricing));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.NOTES));
                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.USERCREATED));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.DATECREATED));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.CREATEDTIME));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.USERUPDATED));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.UPDATEDTIME));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.DATEUPDATED));
                    objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", 0));
                    if (IsNewRecord)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    else
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Comon.cInt(obj);
                }
            }
            return objRet;
             
          

          

        }


        public static bool DeleteByID(CURRENCY_BO objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Acc_Currency_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@ID", objRecord.ID));               
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                    objCmd.ExecuteNonQuery();
                }
            }
            objRet = true;
            return objRet;
        }

       
        public static long GetNewID()
        {
            try
            {
                DataTable dt;
                string strSQL;
                strSQL = "SELECT Max(ID) + 1 FROM Acc_Currency";
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
        private static DataTable dt;
        private static string strSQL;
        private static object Result;

        public static CURRENCY_BO GetRecordSetBySQL(string strSQL)
        {
            try
            {
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                {
                    return ConvertRowToObj(dt.Rows[0]);
                }
                else
                    return null;
               
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}

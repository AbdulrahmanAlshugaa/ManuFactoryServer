using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using Edex.Model;
using Edex.DAL;

namespace Edex.DAL
{
    public class Sales_CustomersDAL
    {
        public static Sales_Customers ConvertRowToObj(DataRow dr)
        {
            Sales_Customers Obj = new Sales_Customers();
            Obj.CustomerID = Comon.cInt(dr["CustomerID"].ToString());
            Obj.BranchID = Comon.cInt(dr["BranchID"].ToString());
            Obj.FacilityID = Comon.cInt(dr["FacilityID"].ToString());
            Obj.AccountID = Comon.cLong(dr["AccountID"].ToString());
            Obj.ArbName = dr["ArbName"].ToString();
            Obj.EngName = dr["EngName"].ToString();
            Obj.Tel = dr["Tel"].ToString();
            Obj.Mobile = dr["Mobile"].ToString();
            Obj.Fax = dr["Fax"].ToString();
            Obj.Email = dr["Email"].ToString();
            Obj.Address = dr["Address"].ToString();
            Obj.Notes = dr["Notes"].ToString();
            Obj.UserID = Comon.cInt(dr["UserID"].ToString());
            Obj.RegDate = Comon.cLong(dr["RegDate"].ToString());
            Obj.RegTime = Comon.cLong(dr["RegTime"].ToString());
            Obj.EditUserID = Comon.cInt(dr["EditUserID"].ToString());
            Obj.EditTime = Comon.cLong(dr["EditTime"].ToString());
            Obj.EditDate = Comon.cLong(dr["EditDate"].ToString());
            Obj.ComputerInfo = dr["ComputerInfo"].ToString();
            Obj.EditComputerInfo = dr["EditComputerInfo"].ToString();
            Obj.Cancel = Comon.cInt(dr["Cancel"].ToString());
            Obj.SpecialDiscount = Comon.cLong(dr["SpecialDiscount"].ToString());
            Obj.ContactPerson = dr["ContactPerson"].ToString();
            Obj.Gender = Comon.cInt(dr["Gender"].ToString());
            Obj.NationalityID = Comon.cInt(dr["NationalityID"].ToString());
            Obj.IdentityNumber = dr["IdentityNumber"].ToString();
            Obj.IdentityTypeID = Comon.cInt(dr["IdentityTypeID"].ToString());
            Obj.IdentityExpiryDate = Comon.cLong(dr["IdentityExpiryDate"].ToString());
            Obj.CustomerType = dr["CustomerType"].ToString();
            Obj.IsInBlackList = Comon.cInt(dr["IsInBlackList"].ToString());
            Obj.BlockingReason = dr["BlockingReason"].ToString();
            Obj.VATID = dr["VATID"].ToString();
            Obj.MaxAgeDebt =Comon.cDec( dr["MaxAgeDebt"].ToString());
            Obj.MaxLimit =Comon.cDec( dr["MaxLimit"].ToString());
            Obj.AllowMaxAgeDebt =Comon.cInt( dr["AllowMaxAgeDebt"].ToString());
            Obj.AllowMaxLimit = Comon.cInt(dr["AllowMaxLimit"].ToString());
            return Obj;
        }

        public static Sales_Customers GetDataByID(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Sales_Customers_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@CustomerID  ", ID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            Sales_Customers Returned = new Sales_Customers();
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

        public static List<Sales_Customers> GetAllData(int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Sales_Customers_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<Sales_Customers> Returned = new List<Sales_Customers>();
                            foreach (DataRow rows in dt.Rows)
                                Returned.Add(ConvertRowToObj(rows));
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

        public DataTable GetSales_Customers(int FacilityID, int BranchID)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Sales_Customers_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                    SqlDataReader myreader = objCmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(myreader);
                    return dt;
                }
            }
        }

        public static Int32 InsertSales_Customers(Sales_Customers objRecord)
        {
            int objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Sales_Customers_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@CustomerID", objRecord.CustomerID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@AccountID", objRecord.AccountID));
                    objCmd.Parameters.Add(new SqlParameter("@ParentAccountID", objRecord.ParentAccountID));

                    objCmd.Parameters.Add(new SqlParameter("@StopAccount", objRecord.StopAccount)); 
                    objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
                    objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
                    objCmd.Parameters.Add(new SqlParameter("@Tel", objRecord.Tel));
                    objCmd.Parameters.Add(new SqlParameter("@Mobile", objRecord.Mobile));
                    objCmd.Parameters.Add(new SqlParameter("@Fax", objRecord.Fax));
                    objCmd.Parameters.Add(new SqlParameter("@Email", objRecord.Email));
                    objCmd.Parameters.Add(new SqlParameter("@Address", objRecord.Address));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
                    objCmd.Parameters.Add(new SqlParameter("@SpecialDiscount", objRecord.SpecialDiscount));
                    objCmd.Parameters.Add(new SqlParameter("@ContactPerson", objRecord.ContactPerson));
                    objCmd.Parameters.Add(new SqlParameter("@Gender", objRecord.Gender));
                    objCmd.Parameters.Add(new SqlParameter("@NationalityID", objRecord.NationalityID));
                    objCmd.Parameters.Add(new SqlParameter("@IdentityNumber", objRecord.IdentityNumber));
                    objCmd.Parameters.Add(new SqlParameter("@IdentityTypeID", objRecord.IdentityTypeID));
                    objCmd.Parameters.Add(new SqlParameter("@IdentityExpiryDate", objRecord.IdentityExpiryDate));
                    objCmd.Parameters.Add(new SqlParameter("@CustomerType", objRecord.CustomerType));
                    objCmd.Parameters.Add(new SqlParameter("@IsInBlackList", objRecord.IsInBlackList));

                    objCmd.Parameters.Add(new SqlParameter("@MaxLimit", objRecord.MaxLimit));
                    objCmd.Parameters.Add(new SqlParameter("@AllowMaxAgeDebt", objRecord.AllowMaxAgeDebt));
                    objCmd.Parameters.Add(new SqlParameter("@AllowMaxLimit", objRecord.AllowMaxLimit));
                    objCmd.Parameters.Add(new SqlParameter("@MaxAgeDebt", objRecord.MaxAgeDebt));

                    objCmd.Parameters.Add(new SqlParameter("@VATID", objRecord.VATID));
                    objCmd.Parameters.Add(new SqlParameter("@BlockingReason", objRecord.BlockingReason));


                    objCmd.Parameters.Add(new SqlParameter("@TransactionDate", objRecord.TransactionDate));
                    objCmd.Parameters.Add(new SqlParameter("@TypeCustomer", objRecord.TypeCustomer));
                    objCmd.Parameters.Add(new SqlParameter("@Region", objRecord.Region));
                    objCmd.Parameters.Add(new SqlParameter("@ConductorID", objRecord.ConductorID));
                    objCmd.Parameters.Add(new SqlParameter("@CollectionDay", objRecord.CollectionDay));
                    objCmd.Parameters.Add(new SqlParameter("@City", objRecord.City));
                    objCmd.Parameters.Add(new SqlParameter("@Category", objRecord.Category));
                    objCmd.Parameters.Add(new SqlParameter("@BankName", objRecord.BankName));
                    objCmd.Parameters.Add(new SqlParameter("@BankAccountNo", objRecord.BankAccountNo));
                    
                  
                    SqlParameter pvNewId = new SqlParameter();
                    pvNewId.ParameterName = "@product_count";
                    pvNewId.DbType = DbType.Int32;
                    pvNewId.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(pvNewId);
                
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    object obj = objCmd.ExecuteScalar();
                    string val = objCmd.Parameters["@product_count"].Value.ToString();

                    if (val != null)
                        objRet = Convert.ToInt32(val);
                }
            }
            return objRet;
        }

        public static Int32 InsertSales_Drivers(Sales_Customers objRecord)
        {
            int objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Sales_Drivers_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@DriverID", objRecord.CustomerID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@AccountID", objRecord.AccountID));
                    objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
                    objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
                    objCmd.Parameters.Add(new SqlParameter("@Tel", objRecord.Tel));
                    objCmd.Parameters.Add(new SqlParameter("@Mobile", objRecord.Mobile));
                    objCmd.Parameters.Add(new SqlParameter("@Fax", objRecord.Fax));
                    objCmd.Parameters.Add(new SqlParameter("@Email", objRecord.Email));
                    objCmd.Parameters.Add(new SqlParameter("@Address", objRecord.Address));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
                    objCmd.Parameters.Add(new SqlParameter("@SpecialDiscount", objRecord.SpecialDiscount));
                    objCmd.Parameters.Add(new SqlParameter("@ContactPerson", objRecord.ContactPerson));
                    objCmd.Parameters.Add(new SqlParameter("@Gender", objRecord.Gender));
                    objCmd.Parameters.Add(new SqlParameter("@NationalityID", objRecord.NationalityID));
                    objCmd.Parameters.Add(new SqlParameter("@IdentityNumber", objRecord.IdentityNumber));
                    objCmd.Parameters.Add(new SqlParameter("@IdentityTypeID", objRecord.IdentityTypeID));
                    objCmd.Parameters.Add(new SqlParameter("@IdentityExpiryDate", objRecord.IdentityExpiryDate));
                    objCmd.Parameters.Add(new SqlParameter("@CustomerType", objRecord.CustomerType));
                    objCmd.Parameters.Add(new SqlParameter("@IsInBlackList", objRecord.IsInBlackList));
                    objCmd.Parameters.Add(new SqlParameter("@VATID", objRecord.VATID));
                    objCmd.Parameters.Add(new SqlParameter("@BlockingReason", objRecord.BlockingReason));

                    SqlParameter pvNewId = new SqlParameter();
                    pvNewId.ParameterName = "@product_count";
                    pvNewId.DbType = DbType.Int32;
                    pvNewId.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(pvNewId);

                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    object obj = objCmd.ExecuteScalar();
                    string val = objCmd.Parameters["@product_count"].Value.ToString();

                    if (val != null)
                        objRet = Convert.ToInt32(val);
                }
            }
            return objRet;
        }


        public static Int32 UpdateSales_Customers(Sales_Customers objRecord)
        {
            Int32 objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Sales_Customers_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@CustomerID", objRecord.CustomerID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@AccountID", objRecord.AccountID));
                    objCmd.Parameters.Add(new SqlParameter("@StopAccount", objRecord.StopAccount)); 
                    objCmd.Parameters.Add(new SqlParameter("@ParentAccountID", objRecord.ParentAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
                    objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
                    objCmd.Parameters.Add(new SqlParameter("@Tel", objRecord.Tel));
                    objCmd.Parameters.Add(new SqlParameter("@Mobile", objRecord.Mobile));
                    objCmd.Parameters.Add(new SqlParameter("@Fax", objRecord.Fax));
                    objCmd.Parameters.Add(new SqlParameter("@Email", objRecord.Email));
                    objCmd.Parameters.Add(new SqlParameter("@Address", objRecord.Address));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
                    objCmd.Parameters.Add(new SqlParameter("@SpecialDiscount", objRecord.SpecialDiscount));
                    objCmd.Parameters.Add(new SqlParameter("@ContactPerson", objRecord.ContactPerson));
                    objCmd.Parameters.Add(new SqlParameter("@Gender", objRecord.Gender));
                    objCmd.Parameters.Add(new SqlParameter("@NationalityID", objRecord.NationalityID));
                    objCmd.Parameters.Add(new SqlParameter("@IdentityNumber", objRecord.IdentityNumber));
                    objCmd.Parameters.Add(new SqlParameter("@IdentityTypeID", objRecord.IdentityTypeID));
                    objCmd.Parameters.Add(new SqlParameter("@IdentityExpiryDate", objRecord.IdentityExpiryDate));
                    objCmd.Parameters.Add(new SqlParameter("@CustomerType", objRecord.CustomerType));
                    objCmd.Parameters.Add(new SqlParameter("@IsInBlackList", objRecord.IsInBlackList));
                    objCmd.Parameters.Add(new SqlParameter("@VATID", objRecord.VATID));
                    objCmd.Parameters.Add(new SqlParameter("@BlockingReason", objRecord.BlockingReason));
                    objCmd.Parameters.Add(new SqlParameter("@MaxLimit", objRecord.MaxLimit));
                    objCmd.Parameters.Add(new SqlParameter("@AllowMaxAgeDebt", objRecord.AllowMaxAgeDebt));
                    objCmd.Parameters.Add(new SqlParameter("@AllowMaxLimit", objRecord.AllowMaxLimit));
                    objCmd.Parameters.Add(new SqlParameter("@MaxAgeDebt", objRecord.MaxAgeDebt));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                    objCmd.Parameters.Add(new SqlParameter("@TransactionDate", objRecord.TransactionDate));
                    objCmd.Parameters.Add(new SqlParameter("@TypeCustomer", objRecord.TypeCustomer));
                    objCmd.Parameters.Add(new SqlParameter("@Region", objRecord.Region));
                    objCmd.Parameters.Add(new SqlParameter("@ConductorID", objRecord.ConductorID));
                    objCmd.Parameters.Add(new SqlParameter("@CollectionDay", objRecord.CollectionDay));
                    objCmd.Parameters.Add(new SqlParameter("@City", objRecord.City));
                    objCmd.Parameters.Add(new SqlParameter("@Category", objRecord.Category));
                    objCmd.Parameters.Add(new SqlParameter("@BankName", objRecord.BankName));
                    objCmd.Parameters.Add(new SqlParameter("@BankAccountNo", objRecord.BankAccountNo));
                    SqlParameter pvNewId = new SqlParameter();
                    pvNewId.ParameterName = "@product_count";
                    pvNewId.DbType = DbType.Int32;
                    pvNewId.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(pvNewId);


                  
                    object obj = objCmd.ExecuteScalar();
                    string val = objCmd.Parameters["@product_count"].Value.ToString();

                    if (val != null)
                        objRet = Convert.ToInt32(val);
                }
            }
            return objRet;
        }

        public static bool DeleteSales_Customers(Sales_Customers objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Sales_Customers_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@CustomerID", objRecord.CustomerID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@editdate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));

                    SqlParameter pvNewId = new SqlParameter();
                    pvNewId.ParameterName = "@product_count";
                    pvNewId.DbType = DbType.Int32;
                    pvNewId.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(pvNewId);

                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                    objCmd.ExecuteNonQuery();
                }
            }
            objRet = true;
            return objRet;
        }
        public static bool DeleteSales_CustomersByAccountID(Sales_Customers objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Sales_Customers_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@AccountID", objRecord.AccountID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@editdate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));

                    SqlParameter pvNewId = new SqlParameter();
                    pvNewId.ParameterName = "@product_count";
                    pvNewId.DbType = DbType.Int32;
                    pvNewId.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(pvNewId);

                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 6));
                    objCmd.ExecuteNonQuery();
                }
            }
            objRet = true;
            return objRet;
        }
        public static Sales_Customers GetRecordSetBySQL(string strSQL)
        {
            try
            {

                DataTable dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                    return ConvertRowToObj(dt.Rows[0]);
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;

            }
        }

        public static long GetNewID()
        {
            try
            {
                DataTable dt;
                string strSQL;
                Sales_Customers cClass = new Sales_Customers();
                strSQL = "SELECT Max(" + cClass.PremaryKey + ") + 1 FROM " + cClass.TableName;
                dt = Lip.SelectRecord(strSQL);
                string GetNewID = dt.Rows[0][0] == DBNull.Value ? "1" : dt.Rows[0][0].ToString();
                return Convert.ToInt32(GetNewID);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

    }
}

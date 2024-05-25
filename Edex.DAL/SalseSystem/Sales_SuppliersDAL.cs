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
    public class Sales_SuppliersDAL
   { 
        
      public static Sales_Suppliers ConvertRowToObj(DataRow dr)
      {
          Sales_Suppliers Obj = new Sales_Suppliers();
          Obj.SupplierID = Comon.cInt(dr["SupplierID"].ToString());
          Obj.BranchID = Comon.cInt(dr["BranchID"].ToString());
          Obj.FacilityID = Comon.cInt(dr["FacilityID"].ToString());
          Obj.BankAccountNo =  dr["BankAccountNo"].ToString();
          Obj.ArbName = dr["ArbName"].ToString();
          Obj.EngName = dr["EngName"].ToString();
          Obj.AccountID = Comon .cLong ( dr["AccountID"].ToString());

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
          Obj.BankName =  dr["BankName"].ToString();
          Obj.AuthorizedPerson = dr["AuthorizedPerson"].ToString();
          Obj.VATID = dr["VATID"].ToString();
        
          return Obj;
      }

      public  static Sales_Suppliers GetDataByID(int ID, int BranchID, int FacilityID)
      {
          try
          {
              using (SqlConnection objCnn = new GlobalConnection().Conn)
              {
                  objCnn.Open();
                  using (SqlCommand objCmd = objCnn.CreateCommand())
                  {
                      objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                      objCmd.CommandText = "[Sales_Suppliers_SP]";
                      objCmd.Parameters.Add(new SqlParameter("@SupplierID  ", ID));
                      objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                      objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                      objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                      SqlDataReader myreader = objCmd.ExecuteReader();
                      DataTable dt = new DataTable();
                      dt.Load(myreader);
                      if (dt != null)
                      {
                          Sales_Suppliers Returned = new Sales_Suppliers();
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

      public static List<Sales_Suppliers> GetAllData(int BranchID, int FacilityID)
      {
          try
          {
              using (SqlConnection objCnn = new GlobalConnection().Conn)
              {
                  objCnn.Open();
                  using (SqlCommand objCmd = objCnn.CreateCommand())
                  {
                      objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                      objCmd.CommandText = "[Sales_Suppliers_SP]";
                      objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                      objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                      objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                      SqlDataReader myreader = objCmd.ExecuteReader();
                      DataTable dt = new DataTable();
                      dt.Load(myreader);
                      if (dt != null)
                      {
                          List<Sales_Suppliers> Returned = new List<Sales_Suppliers>();
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

      public static DataTable GetSales_Suppliers(int FacilityID, int BranchID)
      {
          using (SqlConnection objCnn = new GlobalConnection().Conn)
         {
            objCnn.Open();
             using (SqlCommand objCmd = objCnn.CreateCommand())
            {
               objCmd.CommandType = System.Data.CommandType.StoredProcedure;
               objCmd.CommandText = "[Sales_Suppliers_SP]";
               objCmd.Parameters.Add(new SqlParameter("@FacilityID",  FacilityID));
               objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
               objCmd.Parameters.Add(new SqlParameter("@CMDTYPE",5));
               SqlDataReader myreader = objCmd.ExecuteReader();
               DataTable dt = new DataTable();
                  dt.Load(myreader);
         return dt;
      }
      }
      }

      public static Int32 InsertSales_Suppliers(Sales_Suppliers objRecord)
      {
         Int32 objRet = 0;
         using (SqlConnection objCnn = new GlobalConnection().Conn)
         {
            objCnn.Open();
             using (SqlCommand objCmd = objCnn.CreateCommand())
            {
               objCmd.CommandType = System.Data.CommandType.StoredProcedure;
               objCmd.CommandText = "[Sales_Suppliers_SP]";
               objCmd.Parameters.Add(new SqlParameter("@SupplierID", objRecord.SupplierID));
               objCmd.Parameters.Add(new SqlParameter("@NationalityID", objRecord.NationalityID));
               objCmd.Parameters.Add(new SqlParameter("@StopAccount", objRecord.StopAccount));
               objCmd.Parameters.Add(new SqlParameter("@CommercialRegister", objRecord.CommercialRegister));
               objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
               objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
               objCmd.Parameters.Add(new SqlParameter("@BankAccountNo", objRecord.BankAccountNo));
               objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
               objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
               objCmd.Parameters.Add(new SqlParameter("@AccountID", objRecord.AccountID));
               objCmd.Parameters.Add(new SqlParameter("@ParentAccountID", objRecord.ParentAccountID));
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
               objCmd.Parameters.Add(new SqlParameter("@BankName", objRecord.BankName));
               objCmd.Parameters.Add(new SqlParameter("@AuthorizedPerson", objRecord.AuthorizedPerson));
               objCmd.Parameters.Add(new SqlParameter("@VATID", objRecord.VATID));
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

      public static Int32 UpdateSales_Suppliers(Sales_Suppliers objRecord)
      {
          Int32 objRet = 0;
      
         using (SqlConnection objCnn = new GlobalConnection().Conn)
         {
            objCnn.Open();
             using (SqlCommand objCmd = objCnn.CreateCommand())
            {
               objCmd.CommandType = System.Data.CommandType.StoredProcedure;
               objCmd.CommandText = "[Sales_Suppliers_SP]";
               objCmd.Parameters.Add(new SqlParameter("@SupplierID", objRecord.SupplierID));
               objCmd.Parameters.Add(new SqlParameter("@NationalityID", objRecord.NationalityID));

               objCmd.Parameters.Add(new SqlParameter("@StopAccount", objRecord.StopAccount));
               objCmd.Parameters.Add(new SqlParameter("@CommercialRegister", objRecord.CommercialRegister));
               objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
               objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
               objCmd.Parameters.Add(new SqlParameter("@BankAccountNo", objRecord.BankAccountNo));
               objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
               objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
               objCmd.Parameters.Add(new SqlParameter("@AccountID", objRecord.AccountID));
               objCmd.Parameters.Add(new SqlParameter("@ParentAccountID", objRecord.ParentAccountID));
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
               objCmd.Parameters.Add(new SqlParameter("@BankName", objRecord.BankName));
               objCmd.Parameters.Add(new SqlParameter("@AuthorizedPerson", objRecord.AuthorizedPerson));
               objCmd.Parameters.Add(new SqlParameter("@VATID", objRecord.VATID));  
               objCmd.Parameters.Add(new SqlParameter("@CMDTYPE",2));
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

      public static bool DeleteSales_Suppliers(Sales_Suppliers objRecord)
      {
         bool objRet = false;
         objRet = false;
         using (SqlConnection objCnn = new GlobalConnection().Conn)
         {
            objCnn.Open();
             using (SqlCommand objCmd = objCnn.CreateCommand())
            {
               objCmd.CommandType = System.Data.CommandType.StoredProcedure;
               objCmd.CommandText = "[Sales_Suppliers_SP]";
               objCmd.Parameters.Add(new SqlParameter("@SupplierID",objRecord. SupplierID));
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

               objCmd.Parameters.Add(new SqlParameter("@CMDTYPE",4));
               objCmd.ExecuteNonQuery();
            }
         }
         objRet = true;
         return objRet;
      }

      public static bool DeleteSales_SuppliersByAccountID(Sales_Suppliers objRecord)
      {
          bool objRet = false;
          objRet = false;
          using (SqlConnection objCnn = new GlobalConnection().Conn)
          {
              objCnn.Open();
              using (SqlCommand objCmd = objCnn.CreateCommand())
              {
                  objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                  objCmd.CommandText = "[Sales_Suppliers_SP]";
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
      
   }
}

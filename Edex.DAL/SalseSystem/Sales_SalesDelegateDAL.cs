using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using Edex.Model;

namespace Edex.DAL
{
   public class Sales_SalesDelegateDAL
   {

      public static Sales_SalesDelegate ConvertRowToObj(DataRow dr)
       {

           Sales_SalesDelegate Obj = new Sales_SalesDelegate();
           Obj.DelegateID = int.Parse(dr["DelegateID"].ToString());
           Obj.ArbName = dr["ARBNAME"].ToString();
           Obj.EngName = dr["ENGNAME"].ToString();
           Obj.BranchID = int.Parse(dr["BranchID"].ToString());
           Obj.FacilityID = int.Parse(dr["FacilityID"].ToString());
           Obj.Tel = dr["Tel"].ToString();
           Obj.Mobile = dr["Mobile"].ToString();
           Obj.Fax = dr["Fax"].ToString();
           Obj.Email = dr["Email"].ToString();
           Obj.Address = dr["Address"].ToString();
           Obj.Notes = dr["Notes"].ToString();
           Obj.Percentage = long.Parse(dr["Percentage"].ToString());
           Obj.Target = long.Parse(dr["Target"].ToString());
          
           Obj.UserID = int.Parse(dr["UserID"].ToString());
           Obj.RegDate = (long.Parse(dr["RegDate"].ToString()));
           Obj.EditUserID = Comon.cInt(dr["EditUserID"].ToString());
           Obj.RegTime = (long.Parse(dr["RegTime"].ToString()));
           Obj.EditUserID = (int.Parse(dr["EditUserID"].ToString()));
           Obj.EditDate = (long.Parse(dr["EditDate"].ToString()));
           Obj.EditTime = (int.Parse(dr["EditTime"].ToString()));
           Obj.ComputerInfo =dr["ComputerInfo"].ToString();
           Obj.EditComputerInfo = dr["EditComputerInfo"].ToString();
           Obj.Cancel = int.Parse(dr["Cancel"].ToString());
           return Obj;
       } 
      public Int32 Cancel{ get; set;}

      public static Sales_SalesDelegate GetDataByID(int ID, int BranchID, int FacilityID)
      {
          try
          {
              using (SqlConnection objCnn = new GlobalConnection().Conn)
              {
                  objCnn.Open();
                  using (SqlCommand objCmd = objCnn.CreateCommand())
                  {
                      objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                      objCmd.CommandText = "[Sales_SalesDelegate_SP]";
                      objCmd.Parameters.Add(new SqlParameter("@DelegateID", ID));
                      objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                      objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                      objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                      SqlDataReader myreader = objCmd.ExecuteReader();
                      DataTable dt = new DataTable();
                      dt.Load(myreader);
                      if (dt != null)
                      {
                          Sales_SalesDelegate Returned = new Sales_SalesDelegate();
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
      
      public static List<Sales_SalesDelegate> GetAllData(int BranchID, int FacilityID)
      {
          try
          {
              using (SqlConnection objCnn = new GlobalConnection().Conn)
              {
                  objCnn.Open();
                  using (SqlCommand objCmd = objCnn.CreateCommand())
                  {
                      objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                      objCmd.CommandText = "[Sales_SalesDelegate_SP]";
                      objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                      objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                      objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                      SqlDataReader myreader = objCmd.ExecuteReader();
                      DataTable dt = new DataTable();
                      dt.Load(myreader);
                      if (dt != null)
                      {
                          List<Sales_SalesDelegate> Returned = new List<Sales_SalesDelegate>();
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


      public DataTable GetSales_SalesDelegate(int BranchID, int FacilityID)
      {
          using (SqlConnection objCnn = new GlobalConnection().Conn)
         {
            objCnn.Open();
            using (SqlCommand objCmd = objCnn.CreateCommand())
            {
                objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                objCmd.CommandText = "[Sales_SalesDelegate_SP]";
                objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                SqlDataReader myreader = objCmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(myreader);
                return dt;
            }
      }
      }
      public static Int32 InsertSales_SalesDelegate(Sales_SalesDelegate objRecord)
      {
         Int32 objRet = 0;
         using (SqlConnection objCnn = new GlobalConnection().Conn)
         {
            objCnn.Open();
             using (SqlCommand objCmd = objCnn.CreateCommand())
            {
               objCmd.CommandType = System.Data.CommandType.StoredProcedure;
               objCmd.CommandText = "[Sales_SalesDelegate_SP]";
               objCmd.Parameters.Add(new SqlParameter("@DelegateID", objRecord.DelegateID));
               objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
               objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
               objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
               objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
               objCmd.Parameters.Add(new SqlParameter("@Tel", objRecord.Tel));
               objCmd.Parameters.Add(new SqlParameter("@Mobile", objRecord.Mobile));
               objCmd.Parameters.Add(new SqlParameter("@Fax", objRecord.Fax));
               objCmd.Parameters.Add(new SqlParameter("@Email", objRecord.Email));
               objCmd.Parameters.Add(new SqlParameter("@Address", objRecord.Address));
               objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
               objCmd.Parameters.Add(new SqlParameter("@Percentage", objRecord.Percentage));
               objCmd.Parameters.Add(new SqlParameter("@Target", objRecord.Target));
               objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
               objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
               objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
               objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
               objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
               objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
               objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo));
               objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
               objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
               objCmd.Parameters.Add(new SqlParameter("@CMDTYPE",1));
               object obj = objCmd.ExecuteScalar();
               if (obj != null)
                  objRet = Convert.ToInt32(obj);
            }
         }
         return objRet;
      }
      public static bool UpdateSales_SalesDelegate(Sales_SalesDelegate objRecord)
      {
         bool objRet = false;
         objRet = false;
         using (SqlConnection objCnn = new GlobalConnection().Conn)
         {
            objCnn.Open();
             using (SqlCommand objCmd = objCnn.CreateCommand())
            {
               objCmd.CommandType = System.Data.CommandType.StoredProcedure;
               objCmd.CommandText = "[Sales_SalesDelegate_SP]";
               objCmd.Parameters.Add(new SqlParameter("@DelegateID", objRecord.DelegateID));
               objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
               objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
               objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
               objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
               objCmd.Parameters.Add(new SqlParameter("@Tel", objRecord.Tel));
               objCmd.Parameters.Add(new SqlParameter("@Mobile", objRecord.Mobile));
               objCmd.Parameters.Add(new SqlParameter("@Fax", objRecord.Fax));
               objCmd.Parameters.Add(new SqlParameter("@Email", objRecord.Email));
               objCmd.Parameters.Add(new SqlParameter("@Address", objRecord.Address));
               objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
               objCmd.Parameters.Add(new SqlParameter("@Percentage", objRecord.Percentage));
               objCmd.Parameters.Add(new SqlParameter("@Target", objRecord.Target));
               objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
               objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
               objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
               objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
               objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
               objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
               objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo));
               objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
               objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
               objCmd.Parameters.Add(new SqlParameter("@CMDTYPE",2));
               objCmd.ExecuteNonQuery();
            }
         }
         objRet = true;
         return objRet;
      }
      public static bool DeleteSales_SalesDelegate(Sales_SalesDelegate objRecord)
      {
         bool objRet = false;
         objRet = false;
         using (SqlConnection objCnn = new GlobalConnection().Conn)
         {
            objCnn.Open();
             using (SqlCommand objCmd = objCnn.CreateCommand())
            {
               objCmd.CommandType = System.Data.CommandType.StoredProcedure;
               objCmd.CommandText = "[Sales_SalesDelegate_SP]";
               objCmd.Parameters.Add(new SqlParameter("@DelegateID",objRecord. DelegateID));
               objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
               objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
               objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
               objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
               objCmd.Parameters.Add(new SqlParameter("@CMDTYPE",4));
               objCmd.ExecuteNonQuery();
            }
         }
         objRet = true;
         return objRet;
      }

      public static Sales_SalesDelegate GetRecordSetBySQL(string strSQL)
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

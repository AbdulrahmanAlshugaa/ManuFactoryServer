using Edex.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.DAL
{
    public class STC_ITEMSSOURCE_DAL
    {

      public static Stc_ItemsSource ConvertRowToObj(DataRow dr)
      {
         
          Stc_ItemsSource Obj = new Stc_ItemsSource();
          Obj.SourceID = int.Parse(dr["SourceID"].ToString());
          Obj.ArbName = dr["ARBNAME"].ToString();
          Obj.EngName = dr["ENGNAME"].ToString();
          Obj.Notes = dr["NOTES"].ToString();
          Obj.UserID = int.Parse(dr["UserID"].ToString());
         

         // Obj.RegDate = Comon.ConvertSerialToDate(long.Parse(dr["RegDate"].ToString()));
          Obj.EditUserID = Comon.cInt(dr["EditUserID"].ToString());
          //Obj.EditDate = Com.ConvertSerialToDate(long.Parse(dr["EditDate"].ToString()));
          return Obj;
      }

      public static Stc_ItemsSource  GetDataByID(int ID, int BranchID,int FacilityID)
      {
          try
          {
              using (SqlConnection objCnn = new GlobalConnection().Conn)
              {
                  objCnn.Open();
                  using (SqlCommand objCmd = objCnn.CreateCommand())
                  {
                      objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                      objCmd.CommandText = "[Stc_ItemsSource_SP]";
                      objCmd.Parameters.Add(new SqlParameter("@SourceID  ", ID));
                      objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                      SqlDataReader myreader = objCmd.ExecuteReader();
                      DataTable dt = new DataTable();
                      dt.Load(myreader);
                      if (dt != null)
                      {
                          Stc_ItemsSource  Returned = new Stc_ItemsSource();
                          Returned=(ConvertRowToObj(dt.Rows[0]));
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

      public static List<Stc_ItemsSource> GetAllData(int BranchID,int FacilityID)
      {
          try
          {
              using (SqlConnection objCnn = new GlobalConnection().Conn)
              {
                  objCnn.Open();
                  using (SqlCommand objCmd = objCnn.CreateCommand())
                  {
                      objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                      objCmd.CommandText = "[Stc_ItemsSource_SP]";
                      objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                      SqlDataReader myreader = objCmd.ExecuteReader();
                      DataTable dt = new DataTable();
                      dt.Load(myreader);
                      if (dt != null)
                      {
                          List<Stc_ItemsSource> Returned = new List<Stc_ItemsSource>();
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

      public static Int32 InsertStc_Stores(Stc_ItemsSource objRecord)
      {
          Int32 objRet = 0;
          using (SqlConnection objCnn = new GlobalConnection().Conn)
          {
              objCnn.Open();
              using (SqlCommand objCmd = objCnn.CreateCommand())
              {
                  objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                  objCmd.CommandText = "[Stc_ItemsSource_SP]";
                  
                 
                  objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
                  objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
                  objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                  objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                  objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                  objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                  objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                  objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                  objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                  objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo));
                  objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                  objCmd.Parameters.Add(new SqlParameter("@Cancel", 0));
                  objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                  object obj = objCmd.ExecuteScalar();
                  if (obj != null)
                      objRet = Convert.ToInt32(obj);
              }
          }
          return objRet;
      }

      public static bool UpdateStc_Stores(Stc_ItemsSource objRecord)
      {
          bool objRet = false;
          objRet = false;
          using (SqlConnection objCnn = new GlobalConnection().Conn)
          {
              objCnn.Open();
              using (SqlCommand objCmd = objCnn.CreateCommand())
              {
                  objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                  objCmd.CommandText = "[Stc_ItemsSource_SP]";
                  objCmd.Parameters.Add(new SqlParameter("@SourceID", objRecord.SourceID));
                 

                  objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
                  objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
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
                  objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                  objCmd.ExecuteNonQuery();
              }
          }
          objRet = true;
          return objRet;
      }

      public static bool DeleteStc_Stores(Stc_ItemsSource objRecord)
      {
          bool objRet = false;
          objRet = false;
          using (SqlConnection objCnn = new GlobalConnection().Conn)
          {
              objCnn.Open();
              using (SqlCommand objCmd = objCnn.CreateCommand())
              {
                  objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                  objCmd.CommandText = "[Stc_ItemsSource_SP]";
                  objCmd.Parameters.Add(new SqlParameter("@SourceID", objRecord.SourceID));
                  objCmd.Parameters.Add(new SqlParameter("@ModifiedBy", objRecord.UserID));
                  objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                  objCmd.ExecuteNonQuery();
              }
          }
          objRet = true;
          return objRet;
      }

    }
}

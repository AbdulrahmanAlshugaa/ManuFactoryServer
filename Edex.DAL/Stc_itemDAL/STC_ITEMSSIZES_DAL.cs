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
    public class STC_ITEMSSIZES_DAL
    {
      /// <summary>
        /// this function is used to Convert data row to object type of Stc_ItemsSizes 
        /// </summary>
        /// <param name="dr"></param>
        /// <returns>return data  with object Stc_ItemsSizes</returns>
      public static Stc_ItemsSizes ConvertRowToObj(DataRow dr)
      {
         
          Stc_ItemsSizes Obj = new Stc_ItemsSizes();
          Obj.SizeID = int.Parse(dr["SizeID"].ToString());
          Obj.ArbName = dr["ARBNAME"].ToString();
          Obj.EngName = dr["ENGNAME"].ToString();
          Obj.UserID = int.Parse(dr["UserID"].ToString());
         // Obj.RegDate = Comon.ConvertSerialToDate(long.Parse(dr["RegDate"].ToString()));
          Obj.EditUserID = Comon.cInt(dr["EditUserID"].ToString());
          //Obj.EditDate = Com.ConvertSerialToDate(long.Parse(dr["EditDate"].ToString()));
          return Obj;
      }
      /// <summary>
    /// this function is used to get data by id 
    /// </summary>
    /// <param name="ID"></param>
    /// <returns>return data by object Stc_ItemsSizes</returns>
        public static Stc_ItemsSizes  GetDataByID(int ID)
      {
          try
          {
              using (SqlConnection objCnn = new GlobalConnection().Conn)
              {
                  objCnn.Open();
                  using (SqlCommand objCmd = objCnn.CreateCommand())
                  {  //set value to proprities SqlCommand object
                      objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                      objCmd.CommandText = "[Stc_ItemsSizes_SP]";
                      objCmd.Parameters.Add(new SqlParameter("@SizeID  ", ID));
                      objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                      SqlDataReader myreader = objCmd.ExecuteReader();
                      DataTable dt = new DataTable();
                      dt.Load(myreader);
                      if (dt != null)
                      {
                          Stc_ItemsSizes  Returned = new Stc_ItemsSizes();
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
     
      /// <summary>
        /// this function is used to get all Data by facility id 
        /// </summary>
        /// <param name="FacilityID"></param>
        /// <returns>return all data with list type of Stc_ItemsSizes </returns>
        public static List<Stc_ItemsSizes> GetAllData(int FacilityID)
      {
          try
          {
              using (SqlConnection objCnn = new GlobalConnection().Conn)
              {
                  objCnn.Open();
                  using (SqlCommand objCmd = objCnn.CreateCommand())
                  {//set value to proprities SqlCommand object
                      objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                      objCmd.CommandText = "[Stc_ItemsSizes_SP]";
                      objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                      objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                      SqlDataReader myreader = objCmd.ExecuteReader();
                      DataTable dt = new DataTable();
                      dt.Load(myreader);
                      if (dt != null)
                      {
                          List<Stc_ItemsSizes> Returned = new List<Stc_ItemsSizes>();
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
      /// <summary>
     /// this function is used to insert into Stc_ItemsSizes
     /// </summary>
     /// <param name="objRecord"></param>
     /// <returns>return 0,1 or number error  </returns>
        public static Int32 Insert(Stc_ItemsSizes objRecord)
        {
            Int32 objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {//set value to proprities SqlCommand object
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_ItemsSizes_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@SizeID", objRecord.SizeID));
                    objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
                    objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.FacilityID));
                    if (objRecord.SizeID == 0)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    else
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                   
                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(obj);
                }
            }
            return objRet;
        }
      /// <summary>
      /// this function is used to updata Stc_ItemsSizes table 
      /// </summary>
      /// <param name="objRecord"></param>
      /// <returns>return value boolen falge </returns>
        public static bool Update(Stc_ItemsSizes objRecord)
      {
          bool objRet = false;
          objRet = false;
          using (SqlConnection objCnn = new GlobalConnection().Conn)
          {
              objCnn.Open();
              using (SqlCommand objCmd = objCnn.CreateCommand())
              {//set value to proprities SqlCommand object
                  objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                  objCmd.CommandText = "[Stc_ItemsSizes_SP]";
                  objCmd.Parameters.Add(new SqlParameter("@SizeID", objRecord.SizeID));
                  objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
                  objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
                  objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                  objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                  objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                  objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                  objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                  objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                  objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo));
                  objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                  objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
                  objCmd.Parameters.Add(new SqlParameter("@typeUnit", objRecord.typeUnit));
                  objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));

                  objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                  objCmd.ExecuteNonQuery();
              }
          }
          objRet = true;
          return objRet;
      }
      /// <summary>
    /// this function is used to delete from Stc_ItemsSizes table 
    /// </summary>
    /// <param name="objRecord"></param>
    /// <returns>return value boolen flage opration </returns>
     public static bool Delete(Stc_ItemsSizes objRecord)
      {
          bool objRet = false;
          objRet = false;
          using (SqlConnection objCnn = new GlobalConnection().Conn)
          {
              objCnn.Open();
              using (SqlCommand objCmd = objCnn.CreateCommand())
              {//set value to proprities SqlCommand object
                  objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                  objCmd.CommandText = "[Stc_ItemsSizes_SP]";
                  objCmd.Parameters.Add(new SqlParameter("@SizeID", objRecord.SizeID));
                  objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.UserID));
                  objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                  objCmd.ExecuteNonQuery();
              }
          }
          objRet = true;
          return objRet;
      }

    }
}

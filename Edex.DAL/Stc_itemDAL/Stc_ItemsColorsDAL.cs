using Edex.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Edex.DAL.Stc_itemDAL
{
   public  class Stc_ItemsColorsDAL
    {
       /// <summary>
       /// This function To convert row to object Stc_ItemsColors
       /// </summary>
       /// <param name="dr"></param>
       /// <returns>return data by object Stc_ItemsColors  </returns>
        public static Stc_ItemsColors ConvertRowToObj(DataRow dr)
        {
            Stc_ItemsColors Obj = new Stc_ItemsColors();
            Obj.ColorID = int.Parse(dr["BaseID"].ToString());
            Obj.ArbName = dr["ArName"].ToString();
            Obj.EngName = dr["EngName"].ToString();
            Obj.UserID = int.Parse(dr["UserID"].ToString());
            Obj.Cancel = int.Parse(dr["Cancel"].ToString());
            Obj.EditUserID = Comon.cInt(dr["EditUserID"].ToString());
            Obj.EditDate = Comon.cLong(dr["EditDate"].ToString());
            Obj.EditTime = Comon.cLong(dr["EditTime"].ToString());
            Obj.ComputerInfo = dr["ComputerInfo"].ToString();
            Obj.EditComputerInfo = dr["EditComputerInfo"].ToString();
            Obj.BranchID = int.Parse(dr["BranchID"].ToString());
            Obj.FacilityID = int.Parse(dr["FacilityID"].ToString());
            return Obj;
        }

       /// <summary>
       /// this function to get data by Id color
       /// </summary>
       /// <param name="ColorID"></param>
        /// <returns>return data by object Stc_ItemsColors</returns>
        public static Stc_ItemsColors GetDataByID(int ColorID)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_ItemsColors_sp]";
                    objCmd.Parameters.Add(new SqlParameter("@ColorID", ColorID));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                    SqlDataReader myreader = objCmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(myreader);
                    if (dt != null)
                    {
                        Stc_ItemsColors Returned = new Stc_ItemsColors();
                        Returned = (ConvertRowToObj(dt.Rows[0]));
                        return Returned;
                    }
                    else
                        return null;
                }
            }
        }
       
       /// <summary>
       /// this function is used to Get All Data 
       /// </summary>
       /// <returns> return data with object  Stc_ItemsColors</returns>
       public static List<Stc_ItemsColors> GetAllData()
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_ItemsColors_sp]";

                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                    SqlDataReader myreader = objCmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(myreader);
                    if (dt != null)
                    {
                        List<Stc_ItemsColors> Returned = new List<Stc_ItemsColors>();
                        foreach (DataRow rows in dt.Rows)
                            Returned.Add(ConvertRowToObj(rows));
                        return Returned;
                    }
                    else
                        return null;
                }
            }
        }
       /// <summary>
       /// this function is used to insert data into Stc_ItemsColors
       /// </summary>
       /// <param name="objRecord"></param>
       /// <returns>return number opration saccess or error,or onther </returns>
       public static Int32 Insert(Stc_ItemsColors objRecord)
        {
            Int32 objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_ItemsColors_sp]";
                   objCmd.Parameters.Add(new SqlParameter("@ColorID", objRecord.ColorID));
                    objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
                    objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));

                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", 0));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", 0));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", 0));
                    objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
                    if (objRecord.ColorID == 0)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    else
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        //objRet = Convert.ToInt32(obj);
                        return objRet;
                }
            }
            return objRet;
        }
      /// <summary>
      /// this function is used to update Stc_ItemsColors table 
      /// </summary>
      /// <param name="objRecord"></param>
      /// <returns> return value falge</returns>
       public static bool Update(Stc_ItemsColors objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_ItemsColors_sp]";
                    objCmd.Parameters.Add(new SqlParameter("@ColorID", objRecord.ColorID));
                    objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
                    objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));

                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));

                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                    objCmd.ExecuteNonQuery();
                }
            }
            objRet = true;
            return objRet;
        }
       /// <summary>
       /// this function is used to delete from Stc_ItemsColors
       /// </summary>
       /// <param name="objRecord"></param>
       /// <returns>return value falge opration </returns>
       public static bool Delete(Stc_ItemsColors objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_ItemsColors_sp]";
                    objCmd.Parameters.Add(new SqlParameter("@ColorID", objRecord.ColorID));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@editdate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                    objCmd.ExecuteNonQuery();
                }
            }
            objRet = true;
            return objRet;
        }
    }
}

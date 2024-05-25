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
    public class Stc_ItemTypesDAL
    {
        #region Declare
        public Int32 TypeID { get; set; }
        public String ArbName { get; set; }
        public String EngName { get; set; }
        #endregion
      
        /// <summary>
        /// this is constractor 
        /// </summary>
        public Stc_ItemTypesDAL()
        {

        }
       
        /// <summary>
        /// this function is used to  convert row to object Stc_ItemTypes
        /// </summary>
        /// <param name="dr"></param>
        /// <returns>return  data object type Stc_ItemTypes  </returns>
        public static Stc_ItemTypes ConvertRowToObj(DataRow dr)
        {
            Stc_ItemTypes Obj = new Stc_ItemTypes();
            Obj.TypeID = int.Parse(dr["TypeID"].ToString());
            Obj.ArbName = dr["ARBNAME"].ToString();
            Obj.EngName = dr["ENGNAME"].ToString();
            return Obj;
        }

        /// <summary>
        /// this function is used to get data By id 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="BranchID"></param>
        /// <param name="FacilityID"></param>
        /// <returns>return data with  object Stc_ItemTypes  </returns>
        public static Stc_ItemTypes GetDataByID(int ID, int BranchID, int FacilityID)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_ItemTypes_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@TypeID  ", ID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                    SqlDataReader myreader = objCmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(myreader);
                    if (dt != null)
                    {
                        Stc_ItemTypes Returned = new Stc_ItemTypes();
                        Returned = (ConvertRowToObj(dt.Rows[0]));
                        return Returned;
                    }
                    else
                        return null;
                }
            }
        }

        /// <summary>
        /// this function is used to get all data by Branch id and Facility ID
        /// </summary>
        /// <param name="BranchID"></param>
        /// <param name="FacilityID"></param>
        /// <returns>return data by list type of Stc_ItemTypes </returns>
        public static List<Stc_ItemTypes> GetAllData(int BranchID, int FacilityID)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_ItemTypes_SP]";
                    objCmd.Parameters.AddWithValue("@BranchID", BranchID);
                    objCmd.Parameters.AddWithValue("@FacilityID", FacilityID);
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                    SqlDataReader myreader = objCmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(myreader);
                    if (dt != null)
                    {
                        List<Stc_ItemTypes> Returned = new List<Stc_ItemTypes>();
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
        /// this function is used to insert to Stc_ItemTypes table 
        /// </summary>
        /// <param name="objRecord"></param>
        /// <returns>return number opration if is saccess or error</returns>
        public static Int32 Insert(Stc_ItemTypes objRecord)
        {
            Int32 objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_ItemTypes_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@TypeID", objRecord.TypeID));
                    objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
                    objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(obj);
                }
            }
            return objRet;
        }

        /// <summary>
        /// this is function is used to update Stc_ItemTypes table 
        /// </summary>
        /// <param name="objRecord"></param>
        /// <returns>return value boolen true if the opration is executed saccess or false is error update</returns>
        public static bool Update(Stc_ItemTypes objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_ItemTypes_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@TypeID", objRecord.TypeID));
                    objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
                    objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                    objCmd.ExecuteNonQuery();
                }
            }
            objRet = true;
            return objRet;
        }

        /// <summary>
        /// this function is used to delete item by id
        /// </summary>
        /// <param name="objRecord"></param>
        /// <returns></returns>
        public static bool Delete(Stc_ItemTypes objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_ItemTypes_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@TypeID", objRecord.TypeID));
                    objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
                    objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                    objCmd.ExecuteNonQuery();
                }
            }
            objRet = true;
            return objRet;
        }

    }
}
